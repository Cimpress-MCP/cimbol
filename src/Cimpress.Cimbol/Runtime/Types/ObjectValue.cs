﻿using System;
using System.Collections.Generic;

namespace Cimpress.Cimbol.Runtime.Types
{
    /// <summary>
    /// An implementation of <see cref="ILocalValue"/> used to stored <see cref="IDictionary{string,ILocalValue}"/> values.
    /// </summary>
    public class ObjectValue : ILocalValue
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectValue"/> class.
        /// </summary>
        /// <param name="value">The value stored in the <see cref="ObjectValue"/>.</param>
        public ObjectValue(IDictionary<string, ILocalValue> value)
        {
            Value = value;
        }

        /// <summary>
        /// The value stored in the <see cref="ObjectValue"/>.
        /// </summary>
        public IDictionary<string, ILocalValue> Value { get; }

        /// <inheritdoc cref="ILocalValue.Access{T}"/>
        public ILocalValue Access(string key)
        {
            if (Value.TryGetValue(key, out var value))
            {
                return value;
            }

            throw new NotSupportedException("ErrorCode070");
        }

        /// <inheritdoc cref="ILocalValue.CastBoolean"/>
        public BooleanValue CastBoolean()
        {
            throw new NotSupportedException("ErrorCode071");
        }

        /// <inheritdoc cref="ILocalValue.CastNumber"/>
        public NumberValue CastNumber()
        {
            throw new NotSupportedException("ErrorCode072");
        }

        /// <inheritdoc cref="ILocalValue.CastString"/>
        public StringValue CastString()
        {
            throw new NotSupportedException("ErrorCode073");
        }

        /// <inheritdoc cref="ILocalValue.Invoke"/>
        public bool EqualTo(ILocalValue other)
        {
            return ReferenceEquals(this, other);
        }

        /// <inheritdoc cref="ILocalValue.Invoke"/>
        public ILocalValue Invoke(params ILocalValue[] arguments)
        {
            throw new NotSupportedException("ErrorCode074");
        }

        /// <summary>
        /// Assign a value to the given key.
        /// </summary>
        /// <param name="key">The key to set.</param>
        /// <param name="value">The value to set the key to.</param>
        internal void Assign(string key, ILocalValue value)
        {
            Value[key] = value;
        }
    }
}