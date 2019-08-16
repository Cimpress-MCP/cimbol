using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

            var arguments = new List<ParameterExpression>(programNode.Arguments.Count());

            var expressions = new List<Expression>();

            var variables = new List<ParameterExpression>();

            foreach (var argumentNode in programNode.Arguments)
            {
                var symbol = symbolRegistry.Arguments[argumentNode.Name];
                arguments.Add(symbol.Variable);
            }

            foreach (var constantNode in programNode.Constants)
            {
                var symbol = symbolRegistry.Constants[constantNode.Name];
                variables.Add(symbol.Variable);

                var constant = EmitConstantDeclaration(constantNode, symbol);
                expressions.Add(constant);
            }

            var executableDeclarations = Enumerable.Empty<KeyValuePair<IDeclarationNode, Expression>>();
            foreach (var moduleNode in programNode.Modules)
            {
                var symbol = symbolRegistry.Modules[moduleNode.Name];
                variables.Add(symbol.Variable);

                var symbolTable = symbolRegistry.SymbolTables[moduleNode];
                foreach (var childSymbol in symbolTable)
                {
                    variables.Add(childSymbol.Value.Variable);
                }

                var module = EmitModuleDeclaration(symbol);
                expressions.Add(module);

                var moduleBody = EmitModuleBody(moduleNode, programNode, symbolRegistry);
                executableDeclarations = executableDeclarations.Concat(moduleBody);
            }

            var expressionLookup = executableDeclarations.ToDictionary(pair => pair.Key, pair => pair.Value);

            var programOutput = CodeGen.ProgramReturn(programNode, symbolRegistry);

            var programBody = EmitExecutionGroups(executionPlan.ExecutionGroups, expressionLookup, programOutput);

            expressions.Add(programBody);

            return CodeGen.ProgramLambda(arguments, variables, expressions);
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
                StandardFunctions.DictionaryConstructorInfo,
                Expression.Constant(StringComparer.OrdinalIgnoreCase));

            var outerInitialization = Expression.New(LocalValueFunctions.ObjectValueConstructorInfo, innerInitialization);

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
        /// Emit an expression from a series of execution groups.
        /// </summary>
        /// <param name="executionGroups">The list of execution groups.</param>
        /// <param name="executionStepLookup">A mapping of execution steps to expressions.</param>
        /// <param name="outputBuilder">The expression that builds the program output.</param>
        /// <returns>An expression that executes a series of execution groups.</returns>
        internal Expression EmitExecutionGroups(
            IReadOnlyCollection<ExecutionGroup> executionGroups,
            IDictionary<IDeclarationNode, Expression> executionStepLookup,
            Expression outputBuilder)
        {
            var executionGroupExpressions = executionGroups.Select(executionGroup =>
            {
                var asynchronousStepExpressions = executionGroup.ExecutionSteps
                    .Where(executionStep => executionStep.IsAsynchronous)
                    .Select(executionStep => executionStepLookup[executionStep.Node])
                    .ToArray();

                var synchronousStepExpressions = executionGroup.ExecutionSteps
                    .Where(executionStep => !executionStep.IsAsynchronous)
                    .Select(executionStep => executionStepLookup[executionStep.Node])
                    .ToArray();

                var executionGroupExpression = CodeGen.ExecutionGroup(
                    asynchronousStepExpressions,
                    synchronousStepExpressions);

                return executionGroupExpression;
            });

            var executionGroupChain = CodeGen.ExecutionGroupChain(executionGroupExpressions, outputBuilder);

            return executionGroupChain;
        }

        /// <summary>
        /// Emit an expression from a formula declaration node.
        /// </summary>
        /// <param name="formulaNode">The formula declaration node to emit from.</param>
        /// <param name="exportSymbol">The parent module node's symbol.</param>
        /// <param name="symbolTable">The symbol table for the current scope.</param>
        /// <returns>An expression that encompasses evaluating and assigning a formula.</returns>
        internal Expression EmitFormulaDeclaration(
            FormulaDeclarationNode formulaNode,
            Symbol exportSymbol,
            SymbolTable symbolTable)
        {
            var formulaSymbol = symbolTable.Resolve(formulaNode.Name);

            var expression = EmitExpression(formulaNode.Body, symbolTable);

            if (formulaNode.IsAsynchronous)
            {
                if (formulaNode.IsExported)
                {
                    return CodeGen.ExecutionStepAsyncExported(
                        expression,
                        formulaSymbol.Variable,
                        formulaNode.Name,
                        exportSymbol.Variable);
                }

                return CodeGen.ExecutionStepAsync(expression, formulaSymbol.Variable);
            }

            if (formulaNode.IsExported)
            {
                return CodeGen.ExecutionStepSyncExported(
                    expression,
                    formulaSymbol.Variable,
                    formulaNode.Name,
                    exportSymbol.Variable);
            }

            return CodeGen.ExecutionStepSync(expression, formulaSymbol.Variable);
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

#pragma warning disable CA1303
            throw new NotSupportedException("ErrorCode005");
#pragma warning restore CA1303
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
#pragma warning disable CA1303
                    throw new NotSupportedException("ErrorCode006");
#pragma warning restore CA1303
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

            return CodeGen.Access(value, accessNode.Member);
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
                    return CodeGen.BinaryOp(RuntimeFunctions.MathAddInfo, left, right, typeof(NumberValue));

                case BinaryOpType.And:
                    return CodeGen.BinaryOp(RuntimeFunctions.BooleanAndInfo, left, right, typeof(BooleanValue));

                case BinaryOpType.Concatenate:
                    return CodeGen.BinaryOp(RuntimeFunctions.StringConcatenateInfo, left, right, typeof(StringValue));

                case BinaryOpType.Divide:
                    return CodeGen.BinaryOp(RuntimeFunctions.MathDivideInfo, left, right, typeof(NumberValue));

                case BinaryOpType.Equal:
                    return CodeGen.BinaryOp(RuntimeFunctions.EqualToInfo, left, right);

                case BinaryOpType.GreaterThan:
                    return CodeGen.BinaryOp(RuntimeFunctions.CompareGreaterThanInfo, left, right, typeof(NumberValue));

                case BinaryOpType.GreaterThanOrEqual:
                    return CodeGen.BinaryOp(RuntimeFunctions.CompareGreaterThanOrEqualInfo, left, right, typeof(NumberValue));

                case BinaryOpType.LessThan:
                    return CodeGen.BinaryOp(RuntimeFunctions.CompareLessThanInfo, left, right, typeof(NumberValue));

                case BinaryOpType.LessThanOrEqual:
                    return CodeGen.BinaryOp(RuntimeFunctions.CompareLessThanOrEqualInfo, left, right, typeof(NumberValue));

                case BinaryOpType.Multiply:
                    return CodeGen.BinaryOp(RuntimeFunctions.MathMultiplyInfo, left, right, typeof(NumberValue));

                case BinaryOpType.NotEqual:
                    return CodeGen.BinaryOp(RuntimeFunctions.NotEqualToInfo, left, right);

                case BinaryOpType.Or:
                    return CodeGen.BinaryOp(RuntimeFunctions.BooleanOrInfo, left, right, typeof(BooleanValue));

                case BinaryOpType.Power:
                    return CodeGen.BinaryOp(RuntimeFunctions.MathPowerInfo, left, right, typeof(NumberValue));

                case BinaryOpType.Remainder:
                    return CodeGen.BinaryOp(RuntimeFunctions.MathRemainderInfo, left, right, typeof(NumberValue));

                case BinaryOpType.Subtract:
                    return CodeGen.BinaryOp(RuntimeFunctions.MathSubtractInfo, left, right, typeof(NumberValue));

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
            return CodeGen.Block(expressions);
        }

        /// <summary>
        /// Emit an expression from an identifier node.
        /// </summary>
        /// <param name="identifierNode">The syntax tree node to emit from.</param>
        /// <param name="symbolTable">The symbol table for the current scope.</param>
        /// <returns>The result of compiling the syntax tree to an expression tree.</returns>
        internal Expression EmitIdentifierNode(IdentifierNode identifierNode, SymbolTable symbolTable)
        {
            return symbolTable.TryResolve(identifierNode.Identifier, out var variable) ? variable.Variable : CodeGen.Error();
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
            return Expression.Call(function, LocalValueFunctions.InvokeInfo, argumentList);
        }

        /// <summary>
        /// Emit an expression from a literal node.
        /// </summary>
        /// <param name="literalNode">The syntax tree node to emit from.</param>
        /// <returns>The result of compiling the syntax tree to an expression tree.</returns>
        internal Expression EmitLiteralNode(LiteralNode literalNode)
        {
            return CodeGen.Constant(literalNode.Value);
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
                    return CodeGen.IfMacro(arguments);

                case "LIST":
                    return CodeGen.ListMacro(arguments);

                case "OBJECT":
                    return CodeGen.ObjectMacro(arguments);

                case "WHERE":
                    return CodeGen.WhereMacro(arguments);

                default:
#pragma warning disable CA1303
                    throw new NotSupportedException("ErrorCode007");
#pragma warning restore CA1303
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
                case UnaryOpType.Await:
                    // TODO: Handle mid-expression async calls.
                    return operand;

                case UnaryOpType.Negate:
                    return CodeGen.UnaryOp(RuntimeFunctions.MathNegateInfo, operand, typeof(NumberValue));

                case UnaryOpType.Not:
                    return CodeGen.UnaryOp(RuntimeFunctions.BooleanNotInfo, operand, typeof(BooleanValue));

                default:
                    throw new ArgumentOutOfRangeException(nameof(unaryOpNode));
            }
        }
    }
}
