﻿// Copyright 2021 Cimpress plc.
// Licensed under the Apache License, Version 2.0
// http://www.apache.org/licenses/LICENSE-2.0

using System.Collections.Generic;
using Cimpress.Cimbol.Exceptions;

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

        /// <inheritdoc cref="ILocalValue.Access"/>
        public ILocalValue Access(string key)
        {
            if (Value.TryGetValue(key, out var value) && value != null)
            {
                return value;
            }

            throw CimbolRuntimeException.MemberNotFoundError(key);
        }

        /// <inheritdoc cref="ILocalValue.CastBoolean"/>
        public BooleanValue CastBoolean()
        {
            throw CimbolRuntimeException.CastBooleanError(typeof(ObjectValue));
        }

        /// <inheritdoc cref="ILocalValue.CastNumber"/>
        public NumberValue CastNumber()
        {
            throw CimbolRuntimeException.CastNumberError(typeof(ObjectValue));
        }

        /// <inheritdoc cref="ILocalValue.CastString"/>
        public StringValue CastString()
        {
            throw CimbolRuntimeException.CastStringError(typeof(ObjectValue));
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