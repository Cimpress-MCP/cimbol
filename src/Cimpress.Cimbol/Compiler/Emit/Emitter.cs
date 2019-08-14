using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Cimpress.Cimbol.Compiler.SyntaxTree;
using Cimpress.Cimbol.Runtime.Functions;
using Cimpress.Cimbol.Runtime.Types;

namespace Cimpress.Cimbol.Compiler.Emit
{
    /// <summary>
    /// An emitter.
    /// </summary>
    public class Emitter
    {
        /// <summary>
        /// Emit a lambda expression from a program node.
        /// </summary>
        /// <param name="programNode">The program node to emit from.</param>
        /// <returns>An expression that encompasses executing the entire program.</returns>
        public LambdaExpression EmitProgram(ProgramNode programNode)
        {
            if (programNode == null)
            {
                throw new ArgumentNullException(nameof(programNode));
            }

            var symbolRegistry = SymbolRegistry.Build(programNode);

            var dependencyTable = DependencyTable.Build(programNode);

            var executionPlan = new ExecutionPlan(dependencyTable);

            var argumentDeclarations = new List<ParameterExpression>(programNode.Arguments.Count());

            var initializationExpressions = new List<Expression>(programNode.Constants.Count() + programNode.Modules.Count());

            var expressions = new List<Expression>();

            var variableDeclarations = new List<ParameterExpression>();

            foreach (var argumentNode in programNode.Arguments)
            {
                var symbol = symbolRegistry.Arguments[argumentNode.Name];
                argumentDeclarations.Add(symbol.Variable);
            }

            foreach (var constantNode in programNode.Constants)
            {
                var symbol = symbolRegistry.Constants[constantNode.Name];
                variableDeclarations.Add(symbol.Variable);

                var constant = EmitConstantDeclaration(constantNode, symbol);
                initializationExpressions.Add(constant);
            }

            var executableDeclarations = Enumerable.Empty<KeyValuePair<IDeclarationNode, Expression>>();
            foreach (var moduleNode in programNode.Modules)
            {
                var symbol = symbolRegistry.Modules[moduleNode.Name];
                variableDeclarations.Add(symbol.Variable);

                var symbolTable = symbolRegistry.SymbolTables[moduleNode];
                foreach (var childSymbol in symbolTable)
                {
                    variableDeclarations.Add(childSymbol.Value.Variable);
                }

                var module = EmitModuleDeclaration(symbol);
                initializationExpressions.Add(module);

                var moduleBody = EmitModuleBody(moduleNode, programNode, symbolRegistry);
                executableDeclarations = executableDeclarations.Concat(moduleBody);
            }

            var expressionLookup = executableDeclarations.ToDictionary(pair => pair.Key, pair => pair.Value);

            var executionSteps = executionPlan.ExecutionGroups.SelectMany(executionGroup => executionGroup.ExecutionSteps);
            foreach (var executionStep in executionSteps)
            {
                var expression = expressionLookup[executionStep.Node];

                expressions.Add(expression);
            }

            var programReturn = BuildProgramReturn(programNode, symbolRegistry);

            var lambdaBody = Expression.Block(
                variableDeclarations,
                initializationExpressions.Concat(expressions).Concat(new[] { programReturn }));

            var lambda = Expression.Lambda(lambdaBody, argumentDeclarations);

            return lambda;
        }

        /// <summary>
        /// Emit an expression from a constant declaration node.
        /// </summary>
        /// <param name="constantNode">The constant declaration node to emit from.</param>
        /// <param name="symbol">The symbol to assign the value of the constant declaration.</param>
        /// <returns>An expression that initializes and assigns the value of a constant.</returns>
        internal Expression EmitConstantDeclaration(ConstantDeclarationNode constantNode, Symbol symbol)
        {
            return Expression.Assign(symbol.Variable, Expression.Constant(constantNode.Value));
        }

        /// <summary>
        /// Emit an expression from a module declaration node.
        /// </summary>
        /// <param name="symbol">The symbol to assign the value of the module declaration.</param>
        /// <returns>An expression that initializes the container for the exports of a module.</returns>
        internal Expression EmitModuleDeclaration(Symbol symbol)
        {
            var innerInitialization = Expression.New(
                RuntimeFunctions.ObjectDictionaryConstructorInfo,
                Expression.Constant(StringComparer.OrdinalIgnoreCase));

            var outerInitialization = Expression.New(RuntimeFunctions.ObjectConstructorInfo, innerInitialization);

            return Expression.Assign(symbol.Variable, outerInitialization);
        }

        /// <summary>
        /// Emit the expressions contained within a module declaration node.
        /// </summary>
        /// <param name="moduleNode">The module declaration node to emit from.</param>
        /// <param name="programNode">The parent program node of the module declaration node.</param>
        /// <param name="symbolRegistry">The symbol registry for the program.</param>
        /// <returns>A mapping of declarations to expressions.</returns>
        internal IDictionary<IDeclarationNode, Expression> EmitModuleBody(
            ModuleDeclarationNode moduleNode,
            ProgramNode programNode,
            SymbolRegistry symbolRegistry)
        {
            var moduleSymbol = symbolRegistry.Modules[moduleNode.Name];

            var symbolTable = symbolRegistry.SymbolTables[moduleNode];

            var expressionTable = new Dictionary<IDeclarationNode, Expression>();

            foreach (var formulaNode in moduleNode.Formulas)
            {
                expressionTable[formulaNode] = EmitFormulaDeclaration(formulaNode, moduleSymbol, symbolTable);
            }

            foreach (var importNode in moduleNode.Imports)
            {
                expressionTable[importNode] = EmitImportDeclaration(importNode, programNode, symbolRegistry, symbolTable);
            }

            return expressionTable;
        }

        /// <summary>
        /// Emit an expression from a formula declaration node.
        /// </summary>
        /// <param name="formulaNode">The formula declaration node to emit from.</param>
        /// <param name="exportSymbol">The parent module node's symbol.</param>
        /// <param name="symbolTable">The symbol table for the current scope.</param
        /// <returns>An expression that encompasses evaluating and assigning a formula.</returns>
        internal Expression EmitFormulaDeclaration(
            FormulaDeclarationNode formulaNode,
            Symbol exportSymbol,
            SymbolTable symbolTable)
        {
            var formulaSymbol = symbolTable.Resolve(formulaNode.Name);

            var expression = EmitExpression(formulaNode.Body, symbolTable);

            var assignment = Expression.Assign(formulaSymbol.Variable, expression);

            if (!formulaNode.IsExported)
            {
                return assignment;
            }

            var export = Expression.Call(
                exportSymbol.Variable,
                RuntimeFunctions.ObjectAssignInfo,
                Expression.Constant(formulaNode.Name),
                assignment);

            return export;
        }

        /// <summary>
        /// Emit an expression from an import declaration node.
        /// </summary>
        /// <param name="importNode">The import declaration node to emit from.</param>
        /// <param name="programNode">The parent program node of the import declaration node.</param>
        /// <param name="symbolRegistry">The symbol registry for the program.</param>
        /// <param name="symbolTable">The symbol table for the current scope.</param>
        /// <returns>An expression that performs an import.</returns>
        internal Expression EmitImportDeclaration(
            ImportDeclarationNode importNode,
            ProgramNode programNode,
            SymbolRegistry symbolRegistry,
            SymbolTable symbolTable)
        {
            var firstName = importNode.ImportPath.ElementAtOrDefault(0);
            var secondName = importNode.ImportPath.ElementAtOrDefault(1);

            var importSymbol = symbolTable.Resolve(importNode.Name);

            if (importNode.ImportType == ImportType.Argument)
            {
                var variable = symbolRegistry.Arguments[firstName].Variable;

                return Expression.Assign(importSymbol.Variable, variable);
            }

            if (importNode.ImportType == ImportType.Constant)
            {
                var variable = symbolRegistry.Constants[firstName].Variable;

                return Expression.Assign(importSymbol.Variable, variable);
            }

            if (importNode.ImportType == ImportType.Formula)
            {
                var moduleNode = programNode.GetModuleDeclaration(firstName);

                var externalSymbolTable = symbolRegistry.SymbolTables[moduleNode];

                var variable = externalSymbolTable.Resolve(secondName).Variable;

                return Expression.Assign(importSymbol.Variable, variable);
            }

            if (importNode.ImportType == ImportType.Module)
            {
                var variable = symbolRegistry.Modules[firstName].Variable;

                return Expression.Assign(importSymbol.Variable, variable);
            }

            throw new NotSupportedException("ErrorCode005");
        }

        /// <summary>
        /// Emit an expression from an expression node.
        /// </summary>
        /// <param name="node">The syntax tree node to emit from.</param>
        /// <param name="symbolTable">The symbol table for the current scope.</param>
        /// <returns>The result of compiling the syntax tree to an expression tree.</returns>
        internal Expression EmitExpression(IExpressionNode node, SymbolTable symbolTable)
        {
            if (symbolTable == null)
            {
                throw new ArgumentNullException(nameof(symbolTable));
            }

            switch (node)
            {
                case AccessNode accessNode:
                    return EmitAccessNode(accessNode, symbolTable);

                case BinaryOpNode binaryOpNode:
                    return EmitBinaryOpNode(binaryOpNode, symbolTable);

                case BlockNode blockNode:
                    return EmitBlockNode(blockNode, symbolTable);

                case IdentifierNode identifierNode:
                    return EmitIdentifierNode(identifierNode, symbolTable);

                case InvokeNode invokeNode:
                    return EmitInvokeNode(invokeNode, symbolTable);

                case LiteralNode literalNode:
                    return EmitLiteralNode(literalNode);

                case MacroNode macroNode:
                    return EmitMacroNode(macroNode, symbolTable);

                case UnaryOpNode unaryOpNode:
                    return EmitUnaryOpNode(unaryOpNode, symbolTable);

                default:
                    throw new NotSupportedException("ErrorCode006");
            }
        }

        /// <summary>
        /// Emit an expression from an access node.
        /// </summary>
        /// <param name="accessNode">The syntax node to emit from.</param>
        /// <param name="symbolTable">The symbol table for the current scope.</param>
        /// <returns>The result of compiling the syntax tree to an expression tree.</returns>
        internal Expression EmitAccessNode(AccessNode accessNode, SymbolTable symbolTable)
        {
            var value = EmitExpression(accessNode.Value, symbolTable);

            return Expression.Call(value, RuntimeFunctions.AccessInfo, Expression.Constant(accessNode.Member));
        }

        /// <summary>
        /// Emit an expression from a binary operation node.
        /// </summary>
        /// <param name="binaryOpNode">The syntax tree node to emit from.</param>
        /// <param name="symbolTable">The symbol table for the current scope.</param>
        /// <returns>The result of compiling the syntax tree to an expression tree.</returns>
        internal Expression EmitBinaryOpNode(BinaryOpNode binaryOpNode, SymbolTable symbolTable)
        {
            var left = EmitExpression(binaryOpNode.Left, symbolTable);

            var right = EmitExpression(binaryOpNode.Right, symbolTable);

            switch (binaryOpNode.OpType)
            {
                case BinaryOpType.Add:
                    return BuildBinaryExpression(RuntimeFunctions.MathAddInfo, left, right, typeof(NumberValue));

                case BinaryOpType.And:
                    return BuildBinaryExpression(RuntimeFunctions.BooleanAndInfo, left, right, typeof(BooleanValue));

                case BinaryOpType.Concatenate:
                    return BuildBinaryExpression(RuntimeFunctions.StringConcatenateInfo, left, right, typeof(StringValue));

                case BinaryOpType.Divide:
                    return BuildBinaryExpression(RuntimeFunctions.MathDivideInfo, left, right, typeof(NumberValue));

                case BinaryOpType.Equal:
                    return BuildBinaryExpression(RuntimeFunctions.EqualToInfo, left, right);

                case BinaryOpType.GreaterThan:
                    return BuildBinaryExpression(RuntimeFunctions.CompareGreaterThanInfo, left, right, typeof(NumberValue));

                case BinaryOpType.GreaterThanOrEqual:
                    return BuildBinaryExpression(RuntimeFunctions.CompareGreaterThanOrEqualInfo, left, right, typeof(NumberValue));

                case BinaryOpType.LessThan:
                    return BuildBinaryExpression(RuntimeFunctions.CompareLessThanInfo, left, right, typeof(NumberValue));

                case BinaryOpType.LessThanOrEqual:
                    return BuildBinaryExpression(RuntimeFunctions.CompareLessThanOrEqualInfo, left, right, typeof(NumberValue));

                case BinaryOpType.Multiply:
                    return BuildBinaryExpression(RuntimeFunctions.MathMultiplyInfo, left, right, typeof(NumberValue));

                case BinaryOpType.NotEqual:
                    return BuildBinaryExpression(RuntimeFunctions.NotEqualToInfo, left, right);

                case BinaryOpType.Or:
                    return BuildBinaryExpression(RuntimeFunctions.BooleanOrInfo, left, right, typeof(BooleanValue));

                case BinaryOpType.Power:
                    return BuildBinaryExpression(RuntimeFunctions.MathPowerInfo, left, right, typeof(NumberValue));

                case BinaryOpType.Remainder:
                    return BuildBinaryExpression(RuntimeFunctions.MathRemainderInfo, left, right, typeof(NumberValue));

                case BinaryOpType.Subtract:
                    return BuildBinaryExpression(RuntimeFunctions.MathSubtractInfo, left, right, typeof(NumberValue));

                default:
                    throw new ArgumentOutOfRangeException(nameof(binaryOpNode));
            }
        }

        /// <summary>
        /// Emit an expression from a block node.
        /// </summary>
        /// <param name="blockNode">The syntax tree node to emit from.</param>
        /// <param name="symbolTable">The symbol table for the current scope.</param>
        /// <returns>The result of compiling the syntax tree to an expression tree.</returns>
        internal Expression EmitBlockNode(BlockNode blockNode, SymbolTable symbolTable)
        {
            var expressions = blockNode.Expressions.Select(expression => EmitExpression(expression, symbolTable));
            return Expression.Block(expressions);
        }

        /// <summary>
        /// Emit an expression from an identifier node.
        /// </summary>
        /// <param name="identifierNode">The syntax tree node to emit from.</param>
        /// <param name="symbolTable">The symbol table for the current scope.</param>
        /// <returns>The result of compiling the syntax tree to an expression tree.</returns>
        internal Expression EmitIdentifierNode(IdentifierNode identifierNode, SymbolTable symbolTable)
        {
            return symbolTable.TryResolve(identifierNode.Identifier, out var variable) ? variable.Variable : BuildError();
        }

        /// <summary>
        /// Emit an expression from an invoke node.
        /// </summary>
        /// <param name="invokeNode">The syntax tree node to emit from.</param>
        /// <param name="symbolTable">The symbol table for the current scope.</param>
        /// <returns>The result of compiling the syntax tree to an expression tree.</returns>
        internal Expression EmitInvokeNode(InvokeNode invokeNode, SymbolTable symbolTable)
        {
            var function = EmitExpression(invokeNode.Function, symbolTable);
            var arguments = invokeNode.Arguments
                .Select(argument => EmitExpression(argument.Value, symbolTable))
                .ToArray();
            var argumentList = Expression.NewArrayInit(typeof(ILocalValue), arguments);
            return Expression.Call(function, RuntimeFunctions.InvokeInfo, argumentList);
        }

        /// <summary>
        /// Emit an expression from a literal node.
        /// </summary>
        /// <param name="literalNode">The syntax tree node to emit from.</param>
        /// <returns>The result of compiling the syntax tree to an expression tree.</returns>
        internal Expression EmitLiteralNode(LiteralNode literalNode)
        {
            return Expression.Constant(literalNode.Value);
        }

        /// <summary>
        /// Emit an expression from a macro node.
        /// </summary>
        /// <param name="macroNode">The syntax tree node to emit from.</param>
        /// <param name="symbolTable">The symbol table for the current scope.</param>
        /// <returns>The result of compiling the syntax tree to an expression tree.</returns>
        internal Expression EmitMacroNode(MacroNode macroNode, SymbolTable symbolTable)
        {
            var arguments = new Tuple<string, Expression>[macroNode.Arguments.Length];

            for (var i = 0; i < macroNode.Arguments.Length; ++i)
            {
                var argument = macroNode.Arguments[i];

                if (argument is NamedArgument namedArgument)
                {
                    arguments[i] = Tuple.Create(namedArgument.Name, EmitExpression(argument.Value, symbolTable));
                }
                else
                {
                    arguments[i] = Tuple.Create((string)null, EmitExpression(argument.Value, symbolTable));
                }
            }

            switch (macroNode.Macro.ToUpperInvariant())
            {
                case "IF":
                    return BuildIfMacro(arguments);

                case "LIST":
                    return BuildListMacro(arguments);

                case "OBJECT":
                    return BuildObjectMacro(arguments);

                case "WHERE":
                    return BuildWhereMacro(arguments);

                default:
                    throw new NotSupportedException("ErrorCode007");
            }
        }

        /// <summary>
        /// Emit an expression from a unary operation node.
        /// </summary>
        /// <param name="unaryOpNode">The syntax tree node to emit from.</param>
        /// <param name="symbolTable">The symbol table for the current scope.</param>
        /// <returns>The result of compiling the syntax tree to an expression tree.</returns>
        internal Expression EmitUnaryOpNode(UnaryOpNode unaryOpNode, SymbolTable symbolTable)
        {
            var operand = EmitExpression(unaryOpNode.Operand, symbolTable);

            switch (unaryOpNode.OpType)
            {
                case UnaryOpType.Negate:
                    return Expression.Call(
                        null,
                        RuntimeFunctions.MathNegateInfo,
                        Expression.Call(operand, RuntimeFunctions.CastNumberInfo));

                case UnaryOpType.Not:
                    return Expression.Call(
                        null,
                        RuntimeFunctions.BooleanNotInfo,
                        Expression.Call(operand, RuntimeFunctions.CastBooleanInfo));
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(unaryOpNode));
            }
        }

        private Expression BuildBinaryExpression(MethodInfo methodInfo, Expression left, Expression right)
        {
            return Expression.Call(null, methodInfo, left, right);
        }

        private Expression BuildBinaryExpression(MethodInfo methodInfo, Expression left, Expression right, Type targetType)
        {
            MethodInfo castFunction;

            if (targetType == typeof(BooleanValue))
            {
                castFunction = RuntimeFunctions.CastBooleanInfo;
            }
            else if (targetType == typeof(NumberValue))
            {
                castFunction = RuntimeFunctions.CastNumberInfo;
            }
            else if (targetType == typeof(StringValue))
            {
                castFunction = RuntimeFunctions.CastStringInfo;
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(targetType));
            }

            return Expression.Call(
                null,
                methodInfo,
                Expression.Call(left, castFunction),
                Expression.Call(right, castFunction));
        }

        private Expression BuildError()
        {
            return Expression.Throw(
                Expression.New(RuntimeFunctions.NotSupportedExceptionConstructorInfo),
                typeof(ILocalValue));
        }

        private Expression BuildIfMacro(Tuple<string, Expression>[] arguments)
        {
            var firstBranch = arguments.ElementAtOrDefault(1);
            var secondBranch = arguments.ElementAtOrDefault(2);

            if (firstBranch == null)
            {
                throw new NotSupportedException("ErrorCode008");
            }

            var test = Expression.Call(null, RuntimeFunctions.IfTrueInfo, arguments[0].Item2);

            if (firstBranch.Item1.Equals("then", StringComparison.OrdinalIgnoreCase))
            {
                var ifTrue = firstBranch.Item2;

                var ifFalse = secondBranch?.Item1?.Equals("else", StringComparison.OrdinalIgnoreCase) == true
                        ? secondBranch.Item2
                        : BuildError();

                return Expression.Condition(test, ifTrue, ifFalse, typeof(ILocalValue));
            }

            if (firstBranch.Item1.Equals("else", StringComparison.OrdinalIgnoreCase))
            {
                var ifTrue = secondBranch?.Item1?.Equals("then", StringComparison.OrdinalIgnoreCase) == true
                    ? secondBranch.Item2
                    : BuildError();

                var ifFalse = firstBranch.Item2;

                return Expression.Condition(test, ifTrue, ifFalse, typeof(ILocalValue));
            }

            throw new NotSupportedException("ErrorCode009");
        }

        private Expression BuildListMacro(Tuple<string, Expression>[] arguments)
        {
            var elements = new Expression[arguments.Length];
            for (var i = 0; i < arguments.Length; ++i)
            {
                elements[i] = arguments[i].Item2;
            }

            var array = Expression.NewArrayInit(typeof(ILocalValue), elements);

            return Expression.New(RuntimeFunctions.ListConstructorInfo, array);
        }

        private Expression BuildObjectMacro(Tuple<string, Expression>[] arguments)
        {
            var elements = new ElementInit[arguments.Length];
            for (var i = 0; i < arguments.Length; ++i)
            {
                var key = Expression.Constant(arguments[i].Item1);
                var value = arguments[i].Item2;
                elements[i] = Expression.ElementInit(RuntimeFunctions.ObjectDictionaryAdd, key, value);
            }

            var init = Expression.New(
                RuntimeFunctions.ObjectDictionaryConstructorInfo,
                Expression.Constant(StringComparer.OrdinalIgnoreCase));
            var dictionary = Expression.ListInit(init, elements);

            return Expression.New(RuntimeFunctions.ObjectConstructorInfo, dictionary);
        }

        private Expression BuildProgramReturn(ProgramNode programNode, SymbolRegistry symbolRegistry)
        {
            var modules = programNode.Modules.ToArray();

            var elements = new ElementInit[modules.Length];
            for (var i = 0; i < modules.Length; ++i)
            {
                var key = Expression.Constant(modules[i].Name);
                var value = symbolRegistry.Modules[modules[i].Name].Variable;
                elements[i] = Expression.ElementInit(RuntimeFunctions.ObjectDictionaryAdd, key, value);
            }

            var init = Expression.New(
                RuntimeFunctions.ObjectDictionaryConstructorInfo,
                Expression.Constant(StringComparer.OrdinalIgnoreCase));
            var dictionary = Expression.ListInit(init, elements);

            return Expression.New(RuntimeFunctions.ObjectConstructorInfo, dictionary);
        }

        private Expression BuildWhereMacro(Tuple<string, Expression>[] arguments)
        {
            if (arguments.Length == 0)
            {
                throw new NotSupportedException("ErrorCode010");
            }

            var head = arguments.Length % 2 == 1 ? arguments.Last().Item2 : BuildError();

            var conditionCount = arguments.Length / 2;
            for (var i = conditionCount - 1; i >= 0; --i)
            {
                var caseExpression = Expression.Call(null, RuntimeFunctions.IfTrueInfo, arguments[i * 2].Item2);
                var doExpression = arguments[(i * 2) + 1].Item2;

                head = Expression.Condition(caseExpression, doExpression, head, typeof(ILocalValue));
            }

            return head;
        }
    }
}