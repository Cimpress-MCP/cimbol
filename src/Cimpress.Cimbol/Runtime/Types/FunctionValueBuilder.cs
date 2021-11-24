// Copyright 2021 Cimpress plc.
// Licensed under the Apache License, Version 2.0
// http://www.apache.org/licenses/LICENSE-2.0

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Cimpress.Cimbol.Exceptions;
using Cimpress.Cimbol.Runtime.Functions;

namespace Cimpress.Cimbol.Runtime.Types
{
    /// <summary>
    /// Builds functions intended to be called from within Cimbol from methods declared in the CLR.
    /// </summary>
    internal static class FunctionValueBuilder
    {
        private const BindingFlags MethodInfoFlags = BindingFlags.NonPublic | BindingFlags.Static;

        private static readonly MethodInfo
            CastGenericInfo = typeof(FunctionValueBuilder).GetMethod(nameof(CastGeneric), MethodInfoFlags);

        private static readonly MethodInfo
            CastGenericArrayInfo = typeof(FunctionValueBuilder).GetMethod(nameof(CastGenericArray), MethodInfoFlags);

        private static readonly IDictionary<Type, MethodInfo> CastMap = BuildCastMap();

        /// <summary>
        /// Build a function that properly validates and performs overloading upon a list of methods.
        /// </summary>
        /// <param name="methods">The list of methods to build the function from.</param>
        /// <returns>A function that invokes the correct function after validating arguments.</returns>
        internal static Func<ILocalValue[], ILocalValue> Build(IEnumerable<Delegate> methods)
        {
            var orderedMethods = PreprocessDelegates(methods);

            return BuildFunctionMapper(orderedMethods);
        }

        private static Expression BuildFunctionErrorHandler(Delegate[] methods, ParameterExpression argumentsParameter)
        {
            var expectedCount = Expression.Constant(methods[0].Method.GetParameters().Length);

            var receivedCount = Expression.Property(argumentsParameter, LocalValueFunctions.ArrayLengthInfo);

            return Expression.Throw(
                Expression.Call(ErrorFunctions.ArgumentCountErrorInfo, expectedCount, receivedCount),
                typeof(ILocalValue));
        }

        private static Expression BuildFunctionInvoker(Delegate method, ParameterExpression argumentsParameter)
        {
            var parameters = method.Method.GetParameters();

            var arguments = parameters.Select((parameterInfo, index) =>
            {
                var converterInfo = GetCastForType(parameterInfo.ParameterType);

                // Retrieves a converter for the method's argument type and calls it with the list of arguments what number argument it is.
                // For example, if you've declared the method "Min(NumberValue, NumberValue[])", this will generate the following code:
                //   MathModule.Min(
                //     CastNumber(arguments, 0),     -> Take the first argument in the call and convert it to a number
                //     CastNumberArray(arguments, 1) -> Take the second an on arguments and convert them to an array of numbers
                //   );
                // So the call "Math.Min(1, 2, 3, 4)" will convert to something like "MathModule.Min(1, new decimal[] {2, 3, 4})".
                return (Expression)Expression.Call(converterInfo, argumentsParameter, Expression.Constant(index));
            }).ToArray();

            var methodCall = method.Method.IsStatic
                ? Expression.Call(method.Method, arguments)
                : Expression.Call(Expression.Constant(method.Target), method.Method, arguments);

            return Expression.Convert(methodCall, typeof(ILocalValue));
        }

        private static Func<ILocalValue[], ILocalValue> BuildFunctionMapper(Delegate[] methods)
        {
            var hasVariadicOverload = methods.Any(IsVariadic);

            var argumentsParameter = Expression.Parameter(typeof(ILocalValue[]), "arguments");

            // Use the number of arguments as the value to switch off of.
            var switchValue = Expression.Property(argumentsParameter, LocalValueFunctions.ArrayLengthInfo);

            // If there is a variadic overload, don't generate a case condition for it.
            var caseMethods = hasVariadicOverload ? methods.Take(methods.Length - 1) : methods;

            // Build the switch branches for the non-variadic overloads.
            var caseBranches = caseMethods.Select(method =>
            {
                var caseBody = BuildFunctionInvoker(method, argumentsParameter);

                var caseTest = Expression.Constant(method.Method.GetParameters().Length);

                return Expression.SwitchCase(caseBody, caseTest);
            }).ToArray();

            // Build the switch branches for the variadic overload if it exists.
            Expression defaultBranch;
            if (hasVariadicOverload)
            {
                // If there is a variadic call, check to see if it has more or less than what is required.
                // If it's less than what's required, it is an error. Otherwise, call the variadic function.
                var checkOverloadCondition = Expression.GreaterThanOrEqual(
                    Expression.Property(argumentsParameter, LocalValueFunctions.ArrayLengthInfo),
                    Expression.Constant(methods.Last().Method.GetParameters().Length - 1));

                defaultBranch = Expression.Condition(
                    checkOverloadCondition,
                    BuildFunctionInvoker(methods.Last(), argumentsParameter),
                    BuildFunctionErrorHandler(methods, argumentsParameter));
            }
            else
            {
                // If there is no variadic call, an incorrect number of arguments is always an error.
                defaultBranch = BuildFunctionErrorHandler(methods, argumentsParameter);
            }

            // Build the switch expression.
            var mapperBody = Expression.Switch(switchValue, defaultBranch, caseBranches);

            var mapperLambda = Expression.Lambda<Func<ILocalValue[], ILocalValue>>(mapperBody, argumentsParameter);

            return mapperLambda.Compile();
        }

        private static IDictionary<Type, MethodInfo> BuildCastMap()
        {
            var flags = BindingFlags.NonPublic | BindingFlags.Static;

            var castAnyInfo = typeof(FunctionValueBuilder).GetMethod(nameof(CastAny), flags);
            var castAnyArrayInfo = typeof(FunctionValueBuilder).GetMethod(nameof(CastAnyArray), flags);

            var castBooleanInfo = typeof(FunctionValueBuilder).GetMethod(nameof(CastBoolean), flags);
            var castBooleanArrayInfo = typeof(FunctionValueBuilder).GetMethod(nameof(CastBooleanArray), flags);

            var castNumberInfo = typeof(FunctionValueBuilder).GetMethod(nameof(CastNumber), flags);
            var castNumberArrayInfo = typeof(FunctionValueBuilder).GetMethod(nameof(CastNumberArray), flags);

            var castStringInfo = typeof(FunctionValueBuilder).GetMethod(nameof(CastString), flags);
            var castStringArrayInfo = typeof(FunctionValueBuilder).GetMethod(nameof(CastStringArray), flags);

            return new Dictionary<Type, MethodInfo>
            {
                { typeof(BooleanValue), castBooleanInfo },
                { typeof(BooleanValue[]), castBooleanArrayInfo },
                { typeof(FunctionValue), CastGenericInfo.MakeGenericMethod(typeof(FunctionValue)) },
                { typeof(FunctionValue[]), CastGenericArrayInfo.MakeGenericMethod(typeof(FunctionValue)) },
                { typeof(ListValue), CastGenericInfo.MakeGenericMethod(typeof(ListValue)) },
                { typeof(ListValue[]), CastGenericArrayInfo.MakeGenericMethod(typeof(ListValue)) },
                { typeof(NumberValue), castNumberInfo },
                { typeof(NumberValue[]), castNumberArrayInfo },
                { typeof(ObjectValue), CastGenericInfo.MakeGenericMethod(typeof(ObjectValue)) },
                { typeof(ObjectValue[]), CastGenericArrayInfo.MakeGenericMethod(typeof(ObjectValue)) },
                { typeof(PromiseValue), CastGenericInfo.MakeGenericMethod(typeof(PromiseValue)) },
                { typeof(PromiseValue[]), CastGenericArrayInfo.MakeGenericMethod(typeof(PromiseValue)) },
                { typeof(StringValue), castStringInfo },
                { typeof(StringValue[]), castStringArrayInfo },
                { typeof(ILocalValue), castAnyInfo },
                { typeof(ILocalValue[]), castAnyArrayInfo },
            };
        }

        private static ILocalValue CastAny(ILocalValue[] arguments, int index)
        {
            return arguments[index];
        }

        private static ILocalValue[] CastAnyArray(ILocalValue[] arguments, int index)
        {
            var castArguments = new ILocalValue[arguments.Length - index];

            for (var i = 0; i < arguments.Length - index; i += 1)
            {
                castArguments[i] = arguments[i + index];
            }

            return castArguments;
        }

        private static BooleanValue CastBoolean(ILocalValue[] arguments, int index)
        {
            var localValue = arguments[index];

            return localValue.CastBoolean();
        }

        private static BooleanValue[] CastBooleanArray(ILocalValue[] arguments, int index)
        {
            var castArguments = new BooleanValue[arguments.Length - index];

            for (var i = 0; i < arguments.Length - index; i += 1)
            {
                castArguments[i] = arguments[i + index].CastBoolean();
            }

            return castArguments;
        }

        private static T CastGeneric<T>(ILocalValue[] arguments, int index)
            where T : ILocalValue
        {
            var localValue = arguments[index];

            switch (localValue)
            {
                case T tValue:
                    return tValue;

                default:
                    throw CimbolRuntimeException.ArgumentTypeError(typeof(T), localValue.GetType());
            }
        }

        private static T[] CastGenericArray<T>(ILocalValue[] arguments, int index)
            where T : ILocalValue
        {
            var castArguments = new T[arguments.Length - index];

            for (var i = 0; i < arguments.Length - index; i += 1)
            {
                var localValue = arguments[index + i];

                switch (localValue)
                {
                    case T tValue:
                        castArguments[i] = tValue;
                        break;

                    default:
                        throw CimbolRuntimeException.ArgumentTypeError(typeof(T), localValue.GetType());
                }
            }

            return castArguments;
        }

        private static NumberValue CastNumber(ILocalValue[] arguments, int index)
        {
            var localValue = arguments[index];

            return localValue.CastNumber();
        }

        private static NumberValue[] CastNumberArray(ILocalValue[] arguments, int index)
        {
            var castArguments = new NumberValue[arguments.Length - index];

            for (var i = 0; i < arguments.Length - index; i += 1)
            {
                castArguments[i] = arguments[i + index].CastNumber();
            }

            return castArguments;
        }

        private static StringValue CastString(ILocalValue[] arguments, int index)
        {
            var localValue = arguments[index];

            return localValue.CastString();
        }

        private static StringValue[] CastStringArray(ILocalValue[] arguments, int index)
        {
            var castArguments = new StringValue[arguments.Length - index];

            for (var i = 0; i < arguments.Length - index; i += 1)
            {
                castArguments[i] = arguments[i + index].CastString();
            }

            return castArguments;
        }

        private static MethodInfo GetCastForType(Type type)
        {
            if (CastMap.TryGetValue(type, out var castFunction))
            {
                return castFunction;
            }

            return type.IsArray
                ? CastGenericArrayInfo.MakeGenericMethod(type.GetElementType())
                : CastGenericInfo.MakeGenericMethod(type);
        }

        private static bool IsVariadic(Delegate method)
        {
            var parameters = method.Method.GetParameters();
            if (parameters.Length < 1)
            {
                return false;
            }

            var lastParameter = parameters.Last();

            return lastParameter.ParameterType.IsArray;
        }

        private static Delegate[] PreprocessDelegates(IEnumerable<Delegate> methods)
        {
            // Sorting the list of methods simplifies the logic for validating the methods and generating a FunctionValue.
            var orderedMethods = methods.OrderBy(method => method.Method.GetParameters().Length).ToArray();

            var foundVariadic = false;

            var lastArgumentCount = -1;

            if (!orderedMethods.Any())
            {
                throw new ArgumentException("A function must have at least one backing method.");
            }

            foreach (var method in orderedMethods)
            {
                var methodInfo = method.Method;

                var isVariadic = IsVariadic(method);

                // Validate that the return type implements ILocalValue.
                if (!typeof(ILocalValue).IsAssignableFrom(methodInfo.ReturnType))
                {
                    throw new ArgumentException("Function return values must implement ILocalValue.");
                }

                var parameterInfos = methodInfo.GetParameters();

                for (var parameterInfoIndex = 0; parameterInfoIndex < parameterInfos.Length; parameterInfoIndex += 1)
                {
                    var parameterInfo = parameterInfos[parameterInfoIndex];
                    var parameterType = parameterInfo.ParameterType;
                    var isLast = parameterInfoIndex == parameterInfos.Length - 1;

                    // Validate that any variadic parameter implements ILocalValue and is the last argument.
                    if (typeof(ILocalValue[]).IsAssignableFrom(parameterType))
                    {
                        if (!isLast)
                        {
                            throw new ArgumentException("Function can only take a variable number of arguments as the last parameter.");
                        }

                        continue;
                    }

                    // Validate that any non-variadic parameter implements ILocalValue.
                    if (!typeof(ILocalValue).IsAssignableFrom(parameterType))
                    {
                        throw new ArgumentException("Delegate must only accept arguments of types that implements ILocalValue.");
                    }
                }

                var argumentCount = isVariadic ? parameterInfos.Length - 1 : parameterInfos.Length;

                // Validate that the argument count is distinct for each method.
                if (argumentCount == lastArgumentCount)
                {
                    throw new ArgumentException("All function overloads must have a distinct number of arguments.");
                }

                lastArgumentCount = argumentCount;

                // Validate that there is only one variadic call and that it has the most arguments.
                if (foundVariadic)
                {
                    throw new ArgumentException("The variadic call for a function cannot have fewer arguments than another overload.");
                }

                foundVariadic = isVariadic;
            }

            return orderedMethods;
        }
    }
}