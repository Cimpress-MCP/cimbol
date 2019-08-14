using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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

            _argumentCount = value.Method.GetParameters().Length;

            _argumentMapper = argumentMapper ?? BuildArgumentMapper(value.Method);

            Value = value;
        }

        /// <summary>
        /// The value stored in the <see cref="FunctionValue"/>.
        /// </summary>
        public Delegate Value { get; }

        /// <inheritdoc cref="ILocalValue.Access{T}"/>
        public ILocalValue Access(string key)
        {
            throw new NotSupportedException("ErrorCode054");
        }

        /// <inheritdoc cref="ILocalValue.CastBoolean"/>
        public BooleanValue CastBoolean()
        {
            throw new NotSupportedException("ErrorCode055");
        }

        /// <inheritdoc cref="ILocalValue.CastNumber"/>
        public NumberValue CastNumber()
        {
            throw new NotSupportedException("ErrorCode056");
        }

        /// <inheritdoc cref="ILocalValue.CastString"/>
        public StringValue CastString()
        {
            throw new NotSupportedException("ErrorCode057");
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
                throw new NotSupportedException("ErrorCode058");
            }
#pragma warning enable CA1062

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
                    throw new NotSupportedException("ErrorCode059");
            }
        }

        private Func<ILocalValue[], object[]> BuildArgumentMapper(MethodInfo methodInfo)
        {
            // Validate that the function return type implements ILocalValue.
            if (!typeof(ILocalValue).IsAssignableFrom(methodInfo.ReturnType))
            {
                throw new NotSupportedException("ErrorCode060");
            }

            // Validate that each parameter type implements ILocalValue.
            foreach (var parameterInfo in methodInfo.GetParameters())
            {
                var parameterType = parameterInfo.ParameterType;

                if (!typeof(ILocalValue).IsAssignableFrom(parameterType))
                {
                    throw new NotSupportedException("ErrorCode061");
                }
            }

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