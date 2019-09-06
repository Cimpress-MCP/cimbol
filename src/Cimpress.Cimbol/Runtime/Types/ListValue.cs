using System.Collections.Generic;
using Cimpress.Cimbol.Exceptions;

namespace Cimpress.Cimbol.Runtime.Types
{
    /// <summary>
    /// An implementation of <see cref="ILocalValue"/> used to store an array of <see cref="ILocalValue"/> values.
    /// </summary>
    public class ListValue : ILocalValue
    {
        private readonly ILocalValue[] _value;

        /// <summary>
        /// Initializes a new instance of the <see cref="ListValue"/> class.
        /// </summary>
        /// <param name="value">The value stored in the <see cref="ListValue"/>.</param>
        public ListValue(ILocalValue[] value)
        {
            _value = value;
        }

        /// <summary>
        /// The value stored in the <see cref="ListValue"/>.
        /// </summary>
        public IReadOnlyCollection<ILocalValue> Value => _value;

        /// <inheritdoc cref="ILocalValue.Access"/>
        public ILocalValue Access(string key)
        {
            throw CimbolRuntimeException.AccessError();
        }

        /// <inheritdoc cref="ILocalValue.CastBoolean"/>
        public BooleanValue CastBoolean()
        {
            throw CimbolRuntimeException.CastBooleanError(typeof(ListValue));
        }

        /// <inheritdoc cref="ILocalValue.CastNumber"/>
        public NumberValue CastNumber()
        {
            throw CimbolRuntimeException.CastNumberError(typeof(ListValue));
        }

        /// <inheritdoc cref="ILocalValue.CastString"/>
        public StringValue CastString()
        {
            throw CimbolRuntimeException.CastStringError(typeof(ListValue));
        }

        /// <inheritdoc cref="ILocalValue.Invoke"/>
        public bool EqualTo(ILocalValue other)
        {
            return ReferenceEquals(this, other);
        }

        /// <inheritdoc cref="ILocalValue.Invoke"/>
        public ILocalValue Invoke(params ILocalValue[] arguments)
        {
            throw CimbolRuntimeException.InvocationError();
        }
    }
}