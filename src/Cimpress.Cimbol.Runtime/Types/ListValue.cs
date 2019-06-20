using System;
using System.Collections.Generic;

namespace Cimpress.Cimbol.Runtime.Types
{
    /// <summary>
    /// An implementation of <see cref="ILocalValue"/> used to store <see cref="ILocalValue[]"/> values.
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
            throw new NotSupportedException();
        }

        /// <inheritdoc cref="ILocalValue.CastBoolean"/>
        public BooleanValue CastBoolean()
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc cref="ILocalValue.CastNumber"/>
        public NumberValue CastNumber()
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc cref="ILocalValue.CastString"/>
        public StringValue CastString()
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc cref="ILocalValue.Invoke"/>
        public ILocalValue Invoke(params ILocalValue[] arguments)
        {
            throw new NotSupportedException();
        }
    }
}