﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cimpress.Cimbol.Exceptions;

namespace Cimpress.Cimbol.Runtime.Types
{
    /// <summary>
    /// An implementation of <see cref="ILocalValue"/> used to stored <see cref="Delegate"/> values.
    /// </summary>
    public class FunctionValue : ILocalValue
    {
        private static readonly IDictionary<Type, Func<ILocalValue, ILocalValue>> CastMap = BuildTypeMapping();

        private readonly int _argumentCount;

        private readonly Func<ILocalValue[], object[]> _argumentMapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="FunctionValue"/> class.
        /// </summary>
        /// <param name="value">The value stored in the <see cref="FunctionValue"/>.</param>
        /// <param name="argumentMapper">The method to use to cast the provided arguments to the correct types.</param>
        public FunctionValue(Delegate value, Func<ILocalValue[], object[]> argumentMapper = null)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            // Validate that the function return type implements ILocalValue.
            if (!typeof(ILocalValue).IsAssignableFrom(value.Method.ReturnType))
            {
                throw new ArgumentException("Delegate must return a type that implements ILocalValue.", nameof(value));
            }

            // Validate that each parameter type implements ILocalValue.
            foreach (var parameterInfo in value.Method.GetParameters())
            {
                var parameterType = parameterInfo.ParameterType;

                if (!typeof(ILocalValue).IsAssignableFrom(parameterType))
                {
                    throw new ArgumentException(
                        "Delegate must only accept arguments of types that implements ILocalValue.",
                        nameof(value));
                }
            }

            _argumentCount = value.Method.GetParameters().Length;

            _argumentMapper = argumentMapper ?? BuildArgumentMapper(value.Method);

            Value = value;
        }

        /// <summary>
        /// The value stored in the <see cref="FunctionValue"/>.
        /// </summary>
        public Delegate Value { get; }

        /// <inheritdoc cref="ILocalValue.Access"/>
        public ILocalValue Access(string key)
        {
            throw CimbolRuntimeException.AccessError(null);
        }

        /// <inheritdoc cref="ILocalValue.CastBoolean"/>
        public BooleanValue CastBoolean()
        {
            throw CimbolRuntimeException.CastBooleanError(null, typeof(FunctionValue));
        }

        /// <inheritdoc cref="ILocalValue.CastNumber"/>
        public NumberValue CastNumber()
        {
            throw CimbolRuntimeException.CastNumberError(null, typeof(FunctionValue));
        }

        /// <inheritdoc cref="ILocalValue.CastString"/>
        public StringValue CastString()
        {
            throw CimbolRuntimeException.CastStringError(null, typeof(FunctionValue));
        }

        /// <inheritdoc cref="ILocalValue.Invoke"/>
        public bool EqualTo(ILocalValue other)
        {
            return ReferenceEquals(this, other);
        }

        /// <inheritdoc cref="ILocalValue.Invoke"/>
        public ILocalValue Invoke(params ILocalValue[] arguments)
        {
            // Turn off the null check analyzer - we can assume this is always not null and we don't want to incur a
            //   performance hit during code evaluation just to satisfy the code analyzer.
#pragma warning disable CA1062
            if (arguments.Length != _argumentCount)
            {
                throw CimbolRuntimeException.ArgumentCountError(null, _argumentCount, arguments.Length);
            }
#pragma warning restore CA1062

            var castArguments = _argumentMapper(arguments);

            return Value.Method.Invoke(Value.Target, castArguments) as ILocalValue;
        }

        private static IDictionary<Type, Func<ILocalValue, ILocalValue>> BuildTypeMapping()
        {
            var castOverrideMap = new Dictionary<Type, Func<ILocalValue, ILocalValue>>
            {
                { typeof(BooleanValue), localValue => localValue.CastBoolean() },
                { typeof(FunctionValue), DefaultConversion<FunctionValue> },
                { typeof(ListValue), DefaultConversion<ListValue> },
                { typeof(NumberValue), localValue => localValue.CastNumber() },
                { typeof(ObjectValue), DefaultConversion<ObjectValue> },
                { typeof(PromiseValue), DefaultConversion<PromiseValue> },
                { typeof(StringValue), localValue => localValue.CastString() },
            };

            return castOverrideMap;
        }

        private static T DefaultConversion<T>(ILocalValue localValue)
            where T : ILocalValue
        {
            switch (localValue)
            {
                case T tValue:
                    return tValue;

                default:
                    throw CimbolRuntimeException.ArgumentTypeError(null, typeof(T), localValue.GetType());
            }
        }

        private Func<ILocalValue[], object[]> BuildArgumentMapper(MethodInfo methodInfo)
        {
            // Build an array of functions that convert their corresponding argument to the correct type.
            var castFunctions = methodInfo
                .GetParameters()
                .Select(parameterInfo => CastMap[parameterInfo.ParameterType])
                .ToArray();

            // Create a function that uses this information to cast arguments from one type to another.
            object[] ArgumentMapper(ILocalValue[] arguments)
            {
                var mappedArguments = new object[arguments.Length];

                for (var i = 0; i < arguments.Length; ++i)
                {
                    mappedArguments[i] = castFunctions[i](arguments[i]);
                }

                return mappedArguments;
            }

            return ArgumentMapper;
        }
    }
}