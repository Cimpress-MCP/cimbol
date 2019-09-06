using Cimpress.Cimbol.Exceptions;
using Cimpress.Cimbol.Runtime.Functions;

namespace Cimpress.Cimbol.Runtime.Types
{
    /// <summary>
    /// An implementation of <see cref="ILocalValue"/> used to stored <see cref="bool"/> values.
    /// </summary>
    public class BooleanValue : ILocalValue
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BooleanValue"/> class.
        /// </summary>
        /// <param name="value">The value stored in the <see cref="BooleanValue"/>.</param>
        public BooleanValue(bool value)
        {
            Value = value;
        }

        /// <summary>
        /// The <see cref="BooleanValue"/> equivalent to "false".
        /// </summary>
        public static BooleanValue False { get; } = new BooleanValue(false);

        /// <summary>
        /// The <see cref="BooleanValue"/> equivalent to "true".
        /// </summary>
        public static BooleanValue True { get; } = new BooleanValue(true);

        /// <summary>
        /// The value stored in the <see cref="BooleanValue"/>.
        /// </summary>
        public bool Value { get; }

        /// <inheritdoc cref="ILocalValue.Access"/>
        public ILocalValue Access(string key)
        {
            throw CimbolRuntimeException.AccessError();
        }

        /// <inheritdoc cref="ILocalValue.CastBoolean"/>
        public BooleanValue CastBoolean()
        {
            return this;
        }

        /// <inheritdoc cref="ILocalValue.CastNumber"/>
        public NumberValue CastNumber()
        {
            throw CimbolRuntimeException.CastNumberError(typeof(BooleanValue));
        }

        /// <inheritdoc cref="ILocalValue.CastString"/>
        public StringValue CastString()
        {
            throw CimbolRuntimeException.CastStringError(typeof(BooleanValue));
        }

        /// <inheritdoc cref="ILocalValue.EqualTo"/>
        public bool EqualTo(ILocalValue other)
        {
            switch (other)
            {
                case BooleanValue otherBoolean:
                    return RuntimeFunctions.InnerEqualTo(this, otherBoolean);

                default:
                    return false;
            }
        }

        /// <inheritdoc cref="ILocalValue.Invoke"/>
        public ILocalValue Invoke(params ILocalValue[] arguments)
        {
            throw CimbolRuntimeException.InvocationError();
        }
    }
}