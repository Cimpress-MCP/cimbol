// Copyright 2021 Cimpress plc.
// Licensed under the Apache License, Version 2.0
// http://www.apache.org/licenses/LICENSE-2.0

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Cimpress.Cimbol.Compiler.SyntaxTree;
using Cimpress.Cimbol.Exceptions;
using Cimpress.Cimbol.Runtime.Functions;
using Cimpress.Cimbol.Runtime.Types;
using Cimpress.Cimbol.Utilities;

namespace Cimpress.Cimbol.Compiler.Emit
{
    /// <summary>
    /// A collection of methods for generating expression trees.
    /// </summary>
    internal static partial class CodeGen
    {
        /// <summary>
        /// Generate the expression tree for a binary operation with no type coercion.
        /// </summary>
        /// <param name="methodInfo">The method to run.</param>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand.</param>
        /// <returns>An expression that perform the given operation on the left and right operands.</returns>
        internal static Expression BinaryOp(MethodInfo methodInfo, Expression left, Expression right)
        {
            return Expression.Call(null, methodInfo, left, right);
        }

        /// <summary>
        /// Generate the expression tree for a binary operation with type coercion.
        /// </summary>
        /// <param name="methodInfo">The method to run.</param>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand.</param>
        /// <param name="targetType">The target type for the operation parameters.</param>
        /// <returns>An expression that perform the given operation on the left and right operands.</returns>
        internal static Expression BinaryOp(MethodInfo methodInfo, Expression left, Expression right, Type targetType)
        {
            MethodInfo castFunction;

            if (targetType == typeof(BooleanValue))
            {
                castFunction = LocalValueFunctions.CastBooleanInfo;
            }
            else if (targetType == typeof(NumberValue))
            {
                castFunction = LocalValueFunctions.CastNumberInfo;
            }
            else if (targetType == typeof(StringValue))
            {
                castFunction = LocalValueFunctions.CastStringInfo;
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(targetType));
            }

            return Expression.Call(
                null,
                methodInfo,
                left.Type == targetType ? left : Expression.Call(left, castFunction),
                right.Type == targetType ? right : Expression.Call(right, castFunction));
        }

        /// <summary>
        /// Generate the expression tree for a member access.
        /// </summary>
        /// <param name="operand">The operand for the access.</param>
        /// <param name="member">The name of the member to access.</param>
        /// <returns>An expression that access the member on the operand.</returns>
        internal static Expression Access(Expression operand, string member)
        {
            return Expression.Call(operand, LocalValueFunctions.AccessInfo, Expression.Constant(member));
        }

        /// <summary>
        /// Generate the expression tree for a binary logical AND operation that short-circuits.
        /// This can't be implemented like all of the other binary operations because the short-circuiting behavior.
        /// Passing an expression into a function forces it to evaluate, which foregoes the benefits of boolean short-circuiting.
        /// </summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand.</param>
        /// <returns>An expression that performs the logical AND on the left and right operands.</returns>
        internal static Expression AndAlso(Expression left, Expression right)
        {
            var castFunction = LocalValueFunctions.CastBooleanInfo;

            var leftExpression = left.Type == typeof(BooleanValue) ? left : Expression.Call(left, castFunction);

            var rightExpression = right.Type == typeof(BooleanValue) ? right : Expression.Call(right, castFunction);
            
            var andExpression = Expression.AndAlso(
                Expression.Property(leftExpression, LocalValueFunctions.BooleanValueInfo),
                Expression.Property(rightExpression, LocalValueFunctions.BooleanValueInfo));

            return Expression.New(LocalValueFunctions.BooleanValueConstructorInfo, andExpression);
        }

        /// <summary>
        /// Generate the expression tree for a block of expressions.
        /// </summary>
        /// <param name="expressions">The list of expressions to include in the block.</param>
        /// <returns>A block of expressions.</returns>
        internal static Expression Block(IEnumerable<Expression> expressions)
        {
            return Expression.Block(expressions);
        }

        /// <summary>
        /// Generate the expression tree for creating a constant value.
        /// </summary>
        /// <param name="localValue">The local value to create as a constant.</param>
        /// <returns>An expression that returns the provided value.</returns>
        internal static Expression Constant(ILocalValue localValue)
        {
            return Expression.Constant(localValue, localValue.GetType());
        }

        /// <summary>
        /// Generate the expression tree for defaulting a potentially missing value.
        /// </summary>
        /// <param name="value">The expression that evaluates to the root of the path to test.</param>
        /// <param name="fallbackValue">The fallback expression if the path is not valid.</param>
        /// <param name="path">The path to check for validity.</param>
        /// <returns>An expression that returns a defaulted value.</returns>
        internal static Expression Default(
            ParameterExpression value,
            Expression fallbackValue,
            IReadOnlyCollection<string> path)
        {
            var objectPath = path.Skip(1).ToArray();
            return value == null
                ? fallbackValue
                : Expression.Call(RuntimeFunctions.DefaultInfo, value, fallbackValue, Expression.Constant(objectPath));
        }

        /// <summary>
        /// Generate the expression tree for throwing an error.
        /// </summary>
        /// <param name="error">The error to throw.</param>
        /// <param name="errorType">The type of the expression that can throw.</param>
        /// <returns>An expression that throws an error.</returns>
        internal static Expression Error(CimbolRuntimeException error, Type errorType = null)
        {
            return Expression.Throw(Expression.Constant(error), errorType ?? typeof(ILocalValue));
        }

        /// <summary>
        /// Generate the expression tree for checking if an object exists.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="path">The path.</param>
        /// <returns>An expression that returns a boolean value indicating whether an object exists.</returns>
        internal static Expression Exists(ParameterExpression value, IReadOnlyCollection<string> path)
        {
            var objectPath = path.Skip(1).ToArray();

            return value == null
                ? Expression.Constant(BooleanValue.False)
                : Expression.Call(RuntimeFunctions.ExistsInfo, value, Expression.Constant(objectPath)) as Expression;
        }

        /// <summary>
        /// Generate the expression tree for accessing the value of an identifier.
        /// </summary>
        /// <param name="symbol">The symbol to resolve.</param>
        /// <param name="identifierName">The name of the identifier.</param>
        /// <returns>An expression that accesses the value of an identifier.</returns>
        internal static Expression Identifier(Symbol symbol, string identifierName)
        {
            var errorType = symbol?.Variable.Type ?? typeof(ILocalValue);
            var error = Error(CimbolRuntimeException.UnresolvedIdentifier(identifierName), errorType);
            return symbol == null ? error : Expression.Coalesce(symbol.Variable, error);
        }

        /// <summary>
        /// Generate the expression tree for an if macro.
        /// </summary>
        /// <param name="arguments">The list of named arguments to the if macro.</param>
        /// <returns>An expression that performs a conditional check.</returns>
        internal static Expression IfMacro(Tuple<string, Expression>[] arguments)
        {
            var firstBranch = arguments.ElementAtOrDefault(1);
            var secondBranch = arguments.ElementAtOrDefault(2);

            if (firstBranch == null)
            {
                // TODO: Use the actual formula name.
                // TODO: Track positional information inside the if-expression AST node.
                throw CimbolCompilationException.IfExpressionBranchCountError(
                    null,
                    new Position(0, 0),
                    new Position(0, 0));
            }

            var test = Expression.Call(null, RuntimeFunctions.IfTrueInfo, arguments[0].Item2);

            if (firstBranch.Item1.Equals("then", StringComparison.OrdinalIgnoreCase))
            {
                var ifTrue = firstBranch.Item2;

                var ifFalse = secondBranch?.Item1?.Equals("else", StringComparison.OrdinalIgnoreCase) == true
                        ? secondBranch.Item2
                        : Error(CimbolRuntimeException.IfConditionError());

                return Expression.Condition(test, ifTrue, ifFalse, typeof(ILocalValue));
            }

            if (firstBranch.Item1.Equals("else", StringComparison.OrdinalIgnoreCase))
            {
                var ifTrue = secondBranch?.Item1?.Equals("then", StringComparison.OrdinalIgnoreCase) == true
                    ? secondBranch.Item2
                    : Error(CimbolRuntimeException.IfConditionError());

                var ifFalse = firstBranch.Item2;

                return Expression.Condition(test, ifTrue, ifFalse, typeof(ILocalValue));
            }

#pragma warning disable CA1303
            throw new NotSupportedException("ErrorCode009");
#pragma warning restore CA1303
        }

        /// <summary>
        /// Generate the expression tree for constructing a list.
        /// </summary>
        /// <param name="arguments">The list of named arguments to the list macro.</param>
        /// <returns>An expression that constructs a list.</returns>
        internal static Expression ListMacro(Tuple<string, Expression>[] arguments)
        {
            var elements = new Expression[arguments.Length];
            for (var i = 0; i < arguments.Length; ++i)
            {
                elements[i] = arguments[i].Item2;
            }

            var array = Expression.NewArrayInit(typeof(ILocalValue), elements);

            return Expression.New(LocalValueFunctions.ListValueConstructorInfo, array);
        }

        /// <summary>
        /// Generate the expression tree for constructing an object.
        /// </summary>
        /// <param name="arguments">The list of named arguments to the object macro.</param>
        /// <returns>An expression that constructs an object.</returns>
        internal static Expression ObjectMacro(Tuple<string, Expression>[] arguments)
        {
            var elements = new ElementInit[arguments.Length];
            for (var i = 0; i < arguments.Length; ++i)
            {
                var key = Expression.Constant(arguments[i].Item1);
                var value = arguments[i].Item2;
                elements[i] = Expression.ElementInit(StandardFunctions.DictionaryAddInfo, key, value);
            }

            var init = Expression.New(
                StandardFunctions.DictionaryConstructorInfo,
                Expression.Constant(StringComparer.OrdinalIgnoreCase));
            var dictionary = Expression.ListInit(init, elements);

            return Expression.New(LocalValueFunctions.ObjectValueConstructorInfo, dictionary);
        }

        /// <summary>
        /// Generate the expression tree for a binary logical OR operation that short-circuits.
        /// This can't be implemented like all of the other binary operations because the short-circuiting behavior.
        /// Passing an expression into a function forces it to evaluate, which foregoes the benefits of boolean short-circuiting.
        /// </summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand.</param>
        /// <returns>An expression that performs the logical OR on the left and right operands.</returns>
        internal static Expression OrElse(Expression left, Expression right)
        {
            var castFunction = LocalValueFunctions.CastBooleanInfo;

            var leftExpression = left.Type == typeof(BooleanValue) ? left : Expression.Call(left, castFunction);

            var rightExpression = right.Type == typeof(BooleanValue) ? right : Expression.Call(right, castFunction);
            
            var orExpression = Expression.OrElse(
                Expression.Property(leftExpression, LocalValueFunctions.BooleanValueInfo),
                Expression.Property(rightExpression, LocalValueFunctions.BooleanValueInfo));

            return Expression.New(LocalValueFunctions.BooleanValueConstructorInfo, orExpression);
        }

        /// <summary>
        /// Generate the expression tree that wraps the entire program and returns a collection of exported formula results.
        /// </summary>
        /// <param name="arguments">The list of arguments to the program.</param>
        /// <param name="variables">The list of variables in the program.</param>
        /// <param name="expressions">The list of expressions in the program.</param>
        /// <returns>A lambda expression that runs a program and returns its results.</returns>
        internal static LambdaExpression ProgramLambda(
            IEnumerable<ParameterExpression> arguments,
            IEnumerable<ParameterExpression> variables,
            IEnumerable<Expression> expressions)
        {
            var lambdaBody = Expression.Block(variables, expressions);

            var lambda = Expression.Lambda(lambdaBody, arguments);

            return lambda;
        }

        /// <summary>
        /// Generate the expression tree that returns a collection of objects of exported formula results.
        /// </summary>
        /// <param name="programNode">The program.</param>
        /// <param name="symbolRegistry">The symbol registry for the program.</param>
        /// <param name="compilationProfile">The error level for the program.</param>
        /// <returns>An expression that returns a collection of objects of exported formula results.</returns>
        internal static Expression ProgramReturn(
            ProgramNode programNode,
            SymbolRegistry symbolRegistry,
            CompilationProfile compilationProfile)
        {
            var modules = programNode.Modules.ToArray();

            var elements = new ElementInit[modules.Length];
            for (var i = 0; i < modules.Length; ++i)
            {
                var key = Expression.Constant(modules[i].Name);
                var value = symbolRegistry.Modules.Resolve(modules[i].Name).Variable;
                elements[i] = Expression.ElementInit(StandardFunctions.ModuleDictionaryAddInfo, key, value);
            }

            var moduleInit = Expression.New(
                StandardFunctions.ModuleDictionaryConstructorInfo,
                Expression.Constant(StringComparer.OrdinalIgnoreCase));
            var moduleDictionary = Expression.ListInit(moduleInit, elements);

            var errorList = compilationProfile == CompilationProfile.Verbose
                ? (Expression)symbolRegistry.ErrorList.Variable
                : Expression.Constant(new List<CimbolRuntimeException>());

            return Expression.New(
                LocalValueFunctions.EvaluationResultConstructorInfo,
                moduleDictionary,
                errorList);
        }

        /// <summary>
        /// Generate the expression tree for a unary operation with no type coercion.
        /// </summary>
        /// <param name="methodInfo">The method to run.</param>
        /// <param name="operand">The operand.</param>
        /// <returns>An expression that performs the given operation on the operand.</returns>
        internal static Expression UnaryOp(MethodInfo methodInfo, Expression operand)
        {
            return Expression.Call(null, methodInfo, operand);
        }

        /// <summary>
        /// Generate the expression tree for a unary operation with type coercion.
        /// </summary>
        /// <param name="methodInfo">The method to run.</param>
        /// <param name="operand">The operand.</param>
        /// <param name="targetType">The target type for the operation parameter.</param>
        /// <returns>An expression that performs the given operation on the operand.</returns>
        internal static Expression UnaryOp(MethodInfo methodInfo, Expression operand, Type targetType)
        {
            MethodInfo castFunction;

            if (targetType == typeof(BooleanValue))
            {
                castFunction = LocalValueFunctions.CastBooleanInfo;
            }
            else if (targetType == typeof(NumberValue))
            {
                castFunction = LocalValueFunctions.CastNumberInfo;
            }
            else if (targetType == typeof(StringValue))
            {
                castFunction = LocalValueFunctions.CastStringInfo;
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(targetType));
            }

            return Expression.Call(
                null,
                methodInfo,
                operand.Type == targetType ? operand : Expression.Call(operand, castFunction));
        }

        /// <summary>
        /// Generate the expression tree for an where macro.
        /// </summary>
        /// <param name="arguments">The list of named arguments to the where macro.</param>
        /// <returns>An expression that performs a series of conditional checks.</returns>
        internal static Expression WhereMacro(Tuple<string, Expression>[] arguments)
        {
            if (arguments.Length == 0)
            {
                // TODO: Use the actual formula name.
                // TODO: Track positional information inside the where-expression AST node.
                throw CimbolCompilationException.WhereExpressionBranchCountError(
                    null,
                    new Position(0, 0),
                    new Position(0, 0));
            }

            var head = arguments.Length % 2 == 1
                ? arguments.Last().Item2
                : Error(CimbolRuntimeException.WhereConditionError());

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