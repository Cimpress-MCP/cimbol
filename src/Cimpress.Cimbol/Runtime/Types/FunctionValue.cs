using System;
using System.Collections.Generic;
using Cimpress.Cimbol.Exceptions;

namespace Cimpress.Cimbol.Runtime.Types
{
    /// <summary>
    /// An implementation of <see cref="ILocalValue"/> used to stored <see cref="Delegate"/> values.
    /// </summary>
    public class FunctionValue : ILocalValue
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FunctionValue"/> class.
        /// </summary>
        /// <param name="value">The value stored in the <see cref="FunctionValue"/>.</param>
        public FunctionValue(Func<ILocalValue[], ILocalValue> value)
        {
            Value = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FunctionValue"/> class from a list of methods.
        /// </summary>
        /// <param name="methods">The list of methods to generate the function value from.</param>
        public FunctionValue(IEnumerable<Delegate> methods)
        {
            if (methods == null)
            {
                throw new ArgumentNullException(nameof(methods));
            }

            Value = FunctionValueBuilder.Build(methods);
        }

        /// <summary>
        /// The value stored in the <see cref="FunctionValue"/>.
        /// </summary>
        public Func<ILocalValue[], ILocalValue> Value { get; }

        /// <inheritdoc cref="ILocalValue.Access"/>
        public ILocalValue Access(string key)
        {
            throw CimbolRuntimeException.AccessError();
        }

        /// <inheritdoc cref="ILocalValue.CastBoolean"/>
        public BooleanValue CastBoolean()
        {
            throw CimbolRuntimeException.CastBooleanError(typeof(FunctionValue));
        }

        /// <inheritdoc cref="ILocalValue.CastNumber"/>
        public NumberValue CastNumber()
        {
            throw CimbolRuntimeException.CastNumberError(typeof(FunctionValue));
        }

        /// <inheritdoc cref="ILocalValue.CastString"/>
        public StringValue CastString()
        {
            throw CimbolRuntimeException.CastStringError(typeof(FunctionValue));
        }

        /// <inheritdoc cref="ILocalValue.Invoke"/>
        public bool EqualTo(ILocalValue other)
        {
            return ReferenceEquals(this, other);
        }

        /// <inheritdoc cref="ILocalValue.Invoke"/>
        public ILocalValue Invoke(params ILocalValue[] arguments)
        {
            return Value(arguments);
        }
    }
}