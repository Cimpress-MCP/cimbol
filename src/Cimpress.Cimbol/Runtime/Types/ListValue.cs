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
#pragma warning disable CA1303
            throw new NotSupportedException("ErrorCode062");
#pragma warning restore CA1303
        }

        /// <inheritdoc cref="ILocalValue.CastBoolean"/>
        public BooleanValue CastBoolean()
        {
#pragma warning disable CA1303
            throw new NotSupportedException("ErrorCode063");
#pragma warning restore CA1303
        }

        /// <inheritdoc cref="ILocalValue.CastNumber"/>
        public NumberValue CastNumber()
        {
#pragma warning disable CA1303
            throw new NotSupportedException("ErrorCode064");
#pragma warning restore CA1303
        }

        /// <inheritdoc cref="ILocalValue.CastString"/>
        public StringValue CastString()
        {
#pragma warning disable CA1303
            throw new NotSupportedException("ErrorCode065");
#pragma warning restore CA1303
        }

        /// <inheritdoc cref="ILocalValue.Invoke"/>
        public bool EqualTo(ILocalValue other)
        {
            return ReferenceEquals(this, other);
        }

        /// <inheritdoc cref="ILocalValue.Invoke"/>
        public ILocalValue Invoke(params ILocalValue[] arguments)
        {
#pragma warning disable CA1303
            throw new NotSupportedException("ErrorCode066");
#pragma warning restore CA1303
        }
    }
}