using System;
using System.Threading.Tasks;

namespace Cimpress.Cimbol.Runtime.Types
{
    /// <summary>
    /// An implementation of <see cref="ILocalValue"/> used to store <see cref="Task{ILocalValue}"/> values.
    /// </summary>
    public class PromiseValue : ILocalValue
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PromiseValue"/> class.
        /// </summary>
        /// <param name="value">The value stored in the <see cref="PromiseValue"/>.</param>
        public PromiseValue(Task<ILocalValue> value)
        {
            Value = value;
        }

        /// <summary>
        /// The value stored in the <see cref="PromiseValue"/>.
        /// </summary>
        public Task<ILocalValue> Value { get; }

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

        /// <inheritdoc cref="ILocalValue.EqualTo"/>
        public bool EqualTo(ILocalValue other)
        {
            return ReferenceEquals(this, other);
        }

        /// <inheritdoc cref="ILocalValue.Invoke"/>
        public ILocalValue Invoke(params ILocalValue[] arguments)
        {
            throw new NotSupportedException();
        }
    }
}