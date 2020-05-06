using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Cimpress.Cimbol.Compiler.SyntaxTree;
using Cimpress.Cimbol.Exceptions;
using Cimpress.Cimbol.Runtime;
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
        /// Initialize a new instance of the <see cref="Emitter"/> class.
        /// </summary>
        public Emitter()
        {
            CompilationProfile = CompilationProfile.Minimal;
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="Emitter"/> class.
        /// </summary>
        /// <param name="compilationProfile">The error level to use when returning errors.</param>
        public Emitter(CompilationProfile compilationProfile)
        {
            CompilationProfile = compilationProfile;
        }

        /// <summary>
        /// The error level to use when returning errors.
        /// </summary>
        public CompilationProfile CompilationProfile { get; }

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

            var declarationHierarchy = new DeclarationHierarchy(programNode);

            var dependencyTable = new DependencyTable(programNode);

            var symbolRegistry = new SymbolRegistry(programNode, declarationHierarchy, dependencyTable);

            var executionPlan = new ExecutionPlan(declarationHierarchy, dependencyTable, symbolRegistry);

            var arguments = new List<ParameterExpression>(programNode.Arguments.Count());

            var expressions = new List<Expression>();

            var variables = new List<ParameterExpression>();

            foreach (var argumentNode in programNode.Arguments)
            {
                var symbol = symbolRegistry.Arguments.Resolve(argumentNode.Name);
                arguments.Add(symbol.Variable);
            }

            foreach (var constantNode in programNode.Constants)
            {
                var symbol = symbolRegistry.Constants.Resolve(constantNode.Name);
                variables.Add(symbol.Variable);

                var constant = EmitConstantInitialization(constantNode, symbol);
                expressions.Add(constant);
            }

            foreach (var moduleNode in programNode.Modules)
            {
                var symbol = symbolRegistry.Modules.Resolve(moduleNode.Name);
                variables.Add(symbol.Variable);

                var symbolTable = symbolRegistry.GetModuleScope(moduleNode);
                foreach (var childSymbol in symbolTable.Symbols)
                {
                    if (!childSymbol.IsReference)
                    {
                        variables.Add(childSymbol.Variable);
                    }
                }

                var module = EmitModuleInitialization(symbol);
                expressions.Add(module);
            }

            // Initialize the error list and store its variable in the list of program-scoped variables.
            if (CompilationProfile == CompilationProfile.Verbose)
            {
                expressions.Add(EmitErrorListInitialization(symbolRegistry.ErrorList));

                variables.Add(symbolRegistry.ErrorList.Variable);
            }

            // Initialize the skip list and store its variable in the list of program-scoped variables.
            if (CompilationProfile == CompilationProfile.Verbose)
            {
                expressions.Add(EmitSkipListInitialization(symbolRegistry.SkipList, executionPlan));

                variables.Add(symbolRegistry.SkipList.Variable);
            }

            var programBody = EmitExecutionPlan(
                executionPlan,
                programNode,
                symbolRegistry);

            expressions.Add(programBody);

            return CodeGen.ProgramLambda(arguments, variables, expressions);
        }

        /// <summary>
        /// Emit an expression from a constant node.
        /// </summary>
        /// <param name="constantNode">The constant node to emit from.</param>
        /// <param name="symbol">The symbol to assign the value of the constant.</param>
        /// <returns>An expression that initializes and assigns the value of a constant.</returns>
        internal Expression EmitConstantInitialization(ConstantNode constantNode, Symbol symbol)
        {
            return Expression.Assign(symbol.Variable, Expression.Constant(constantNode.Value));
        }

        /// <summary>
        /// Emit an expression that declares and initializes the error list.
        /// </summary>
        /// <param name="errorListSymbol">The symbol to assign the error list to.</param>
        /// <returns>An expression that initializes the error list with an empty array with some allocated space.</returns>
        internal Expression EmitErrorListInitialization(Symbol errorListSymbol)
        {
            const int DefaultErrorAllocation = 4;

            var errorListInit = Expression.New(
                StandardFunctions.ExceptionListConstructorInfo,
                Expression.Constant(DefaultErrorAllocation));

            return Expression.Assign(errorListSymbol.Variable, errorListInit);
        }

        /// <summary>
        /// Emit an expression from a module node.
        /// </summary>
        /// <param name="symbol">The symbol to assign the value of the module.</param>
        /// <returns>An expression that initializes the container for the exports of a module.</returns>
        internal Expression EmitModuleInitialization(Symbol symbol)
        {
            var innerInitialization = Expression.New(
                StandardFunctions.DictionaryConstructorInfo,
                Expression.Constant(StringComparer.OrdinalIgnoreCase));

            var outerInitialization = Expression.New(LocalValueFunctions.ObjectValueConstructorInfo, innerInitialization);

            return Expression.Assign(symbol.Variable, outerInitialization);
        }

        /// <summary>
        /// Emit an expression that declares and initializes the skip list.
        /// </summary>
        /// <param name="skipListSymbol">The symbol to assign the skip list to.</param>
        /// <param name="executionPlan">The execution plan for the program.</param>
        /// <returns>An expression that initializes the skip list with an array of true values.</returns>
        internal Expression EmitSkipListInitialization(Symbol skipListSymbol, ExecutionPlan executionPlan)
        {
            var executionStepCount = executionPlan.ExecutionGroups
                .SelectMany(executionGroup => executionGroup.ExecutionSteps)
                .Count();

            var skipList = new bool[executionStepCount];
            for (var i = 0; i < executionStepCount; ++i)
            {
                skipList[i] = true;
            }

            return Expression.Assign(skipListSymbol.Variable, Expression.Constant(skipList));
        }

        /// <summary>
        /// Emit an expression from an execution plan.
        /// </summary>
        /// <param name="executionPlan">The execution plan.</param>
        /// <param name="programNode">The program that the execution plan belongs to.</param>
        /// <param name="symbolRegistry">The symbol registry for the program.</param>
        /// <returns>An expression that executes an execution plan.</returns>
        internal Expression EmitExecutionPlan(
            ExecutionPlan executionPlan,
            ProgramNode programNode,
            SymbolRegistry symbolRegistry)
        {
            var executionGroupExpressions = new List<LambdaExpression>(executionPlan.ExecutionGroups.Count);

            foreach (var executionGroup in executionPlan.ExecutionGroups)
            {
                var executionGroupExpression = EmitExecutionGroup(
                    executionGroup,
                    symbolRegistry);

                executionGroupExpressions.Add(executionGroupExpression);
            }

            var outputBuilder = CodeGen.ProgramReturn(programNode, symbolRegistry, CompilationProfile);

            var executionGroupChain = CodeGen.ExecutionGroupChain(executionGroupExpressions, outputBuilder);

            return executionGroupChain;
        }

        /// <summary>
        /// Emit an expression that executes an execution group.
        /// </summary>
        /// <param name="executionGroup">The execution group.</param>
        /// <param name="symbolRegistry">The symbol registry for the program.</param>
        /// <returns>An expression that executes an execution group.</returns>
        internal LambdaExpression EmitExecutionGroup(
            ExecutionGroup executionGroup,
            SymbolRegistry symbolRegistry)
        {
            var asynchronousStepExpressions = new List<Expression>(executionGroup.ExecutionSteps.Count);

            var synchronousStepExpressions = new List<Expression>(executionGroup.ExecutionSteps.Count);

            foreach (var executionStep in executionGroup.ExecutionSteps)
            {
                if (executionStep.DeclarationNode is FormulaNode formulaNode)
                {
                    var executionStepExpression = EmitFormula(
                        formulaNode,
                        executionStep.ModuleNode,
                        symbolRegistry,
                        executionStep.ExecutionStepContext);

                    if (executionStep.IsAsynchronous)
                    {
                        asynchronousStepExpressions.Add(executionStepExpression);
                    }
                    else
                    {
                        synchronousStepExpressions.Add(executionStepExpression);
                    }
                }
                else if (executionStep.DeclarationNode is ImportNode importNode)
                {
                    var executionStepExpression = EmitImport(importNode, executionStep.ModuleNode, symbolRegistry);

                    if (executionStepExpression != null)
                    {
                        synchronousStepExpressions.Add(executionStepExpression);
                    }
                }
                else
                {
                    throw new CimbolInternalException("Unrecognized declaration node type.");
                }
            }

            var executionGroupExpression = CodeGen.ExecutionGroup(
                asynchronousStepExpressions,
                synchronousStepExpressions);

            return executionGroupExpression;
        }

        /// <summary>
        /// Emit an expression from an execution step.
        /// </summary>
        /// <param name="formulaNode">The execution step being emitted.</param>
        /// <param name="moduleNode">The program that the execution step belongs to.</param>
        /// <param name="symbolRegistry">The symbol registry for the program.</param>
        /// <param name="executionStepContext">The context for the execution step that the formula node belongs to.</param>
        /// <returns>An expression that executes an execution step.</returns>
        internal Expression EmitFormula(
            FormulaNode formulaNode,
            ModuleNode moduleNode,
            SymbolRegistry symbolRegistry,
            ExecutionStepContext executionStepContext)
        {
            var symbolTable = symbolRegistry.GetModuleScope(moduleNode);

            var exportSymbol = symbolRegistry.Modules.Resolve(moduleNode.Name);

            var internalExpression = EmitExpression(formulaNode.Body, symbolTable);

            var isExported = formulaNode.IsExported;

            var internalSymbol = symbolTable.Resolve(formulaNode.Name);

            // If the execution step is asynchronous, create a handler (a callback that runs once the returned task is resolved).
            // This handler's code depends on whether or not it is exported.
            // Once this handler exists, create the actual execution step that runs the handler once it finishes evaluating.
            if (formulaNode.IsAsynchronous)
            {
                var handler = isExported
                    ? CodeGen.ExecutionStepAsyncHandlerExported(
                        internalSymbol.Variable,
                        formulaNode.Name,
                        exportSymbol.Variable)
                    : CodeGen.ExecutionStepAsyncHandler(internalSymbol.Variable);

                return CodeGen.ExecutionStepAsync(
                    internalExpression,
                    handler,
                    executionStepContext,
                    symbolRegistry.ErrorList.Variable,
                    symbolRegistry.SkipList.Variable,
                    CompilationProfile);
            }

            // If the execution step is synchronous, do the inverse of the above.
            // First, create an evaluator that performs error handling and skipping among other tasks.
            // This is equivalent to the second step in the asynchronous case.
            // Then, depending on if the variable is exported or not, wrap the evaluator in a handler that assigns the result correctly.
            var evaluator = CodeGen.ExecutionStepSyncEvaluation(
                internalExpression,
                executionStepContext,
                symbolRegistry.ErrorList.Variable,
                symbolRegistry.SkipList.Variable,
                CompilationProfile);

            return isExported
                ? CodeGen.ExecutionStepSyncExported(
                    evaluator,
                    internalSymbol.Variable,
                    formulaNode.Name,
                    exportSymbol.Variable)
                : CodeGen.ExecutionStepSync(evaluator, internalSymbol.Variable);
        }

        /// <summary>
        /// Emit an expression from an import declaration node.
        /// </summary>
        /// <param name="importNode">The import declaration node to emit from.</param>
        /// <param name="moduleNode">The program that the import step belongs to.</param>
        /// <param name="symbolRegistry">The symbol registry for the program.</param>
        /// <returns>An expression that performs an import.</returns>
        internal Expression EmitImport(
            ImportNode importNode,
            ModuleNode moduleNode,
            SymbolRegistry symbolRegistry)
        {
            if (!importNode.IsExported)
            {
                return null;
            }

            var symbolTable = symbolRegistry.GetModuleScope(moduleNode);

            var exportSymbol = symbolRegistry.Modules.Resolve(moduleNode.Name);

            var internalSymbol = symbolTable.Resolve(importNode.Name);

            return CodeGen.ExecutionStepExport(internalSymbol.Variable, importNode.Name, exportSymbol.Variable);
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

                case DefaultNode defaultNode:
                    return EmitDefaultNode(defaultNode, symbolTable);

                case ExistsNode existsNode:
                    return EmitExistsNode(existsNode, symbolTable);

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
                    throw new CimbolInternalException("Unrecognized expression node type.");
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
                    throw new CimbolInternalException("Unrecognized binary operation type.");
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
        /// Emit an expression from a default node.
        /// </summary>
        /// <param name="defaultNode">The syntax tree node to emit from.</param>
        /// <param name="symbolTable">The symbol table for the current scope.</param>
        /// <returns>The result of compiling the syntax tree to an expression tree.</returns>
        internal Expression EmitDefaultNode(DefaultNode defaultNode, SymbolTable symbolTable)
        {
            var fallbackExpression = EmitExpression(defaultNode.Fallback, symbolTable);
            var symbol = symbolTable.Resolve(defaultNode.Path.First());
            return CodeGen.Default(symbol?.Variable, fallbackExpression, defaultNode.Path);
        }

        /// <summary>
        /// Emits an expression from an exists node.
        /// </summary>
        /// <param name="existsNode">The syntax tree node to emit from.</param>
        /// <param name="symbolTable">The symbol table for the current scope.</param>
        /// <returns>The result of compiling the syntax tree to an expression tree.</returns>
        internal Expression EmitExistsNode(ExistsNode existsNode, SymbolTable symbolTable)
        {
            var symbol = symbolTable.Resolve(existsNode.Path.First());
            return CodeGen.Exists(symbol?.Variable, existsNode.Path);
        }

        /// <summary>
        /// Emit an expression from an identifier node.
        /// </summary>
        /// <param name="identifierNode">The syntax tree node to emit from.</param>
        /// <param name="symbolTable">The symbol table for the current scope.</param>
        /// <returns>The result of compiling the syntax tree to an expression tree.</returns>
        internal Expression EmitIdentifierNode(IdentifierNode identifierNode, SymbolTable symbolTable)
        {
            var symbol = symbolTable.Resolve(identifierNode.Identifier);
            return CodeGen.Identifier(symbol, identifierNode.Identifier);
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
                    throw new CimbolInternalException("Unrecognized macro type.");
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
                    throw new CimbolInternalException("Unrecognized unary operation type.");
            }
        }
    }
}
