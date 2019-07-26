using System;
using Cimpress.Cimbol.Runtime.Functions;

namespace Cimpress.Cimbol.Runtime.Types
{
    /// <summary>
    /// An implementation of <see cref="ILocalValue"/> used to stored <see cref="string"/> values.
    /// </summary>
    public class StringValue : ILocalValue
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StringValue"/> class.
        /// </summary>
        /// <param name="value">The value stored in the <see cref="StringValue"/>.</param>
        public StringValue(string value)
        {
            Value = value;
        }

        /// <summary>
        /// The value stored in the <see cref="StringValue"/>.
        /// </summary>
        public string Value { get; }

        /// <inheritdoc cref="ILocalValue.Access{T}"/>
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
            if (decimal.TryParse(Value, out var result))
            {
                return new NumberValue(result);
            }

            throw new NotSupportedException();
        }

        /// <inheritdoc cref="ILocalValue.CastString"/>
        public StringValue CastString()
        {
            return this;
        }

        /// <inheritdoc cref="ILocalValue.EqualTo"/>
        public bool EqualTo(ILocalValue other)
        {
            switch (other)
            {
                case NumberValue otherNumber:
                    return RuntimeFunctions.InnerEqualTo(otherNumber, this);

                case StringValue otherString:
                    return RuntimeFunctions.InnerEqualTo(this, otherString);

                default:
                    return false;
            }
        }

        /// <inheritdoc cref="ILocalValue.Invoke"/>
        public ILocalValue Invoke(params ILocalValue[] arguments)
        {
            throw new NotSupportedException();
        }
    }
}