using Cimpress.Cimbol.Exceptions;

namespace Cimpress.Cimbol.Runtime.Types
{
    /// <summary>
    /// An implementation of <see cref="ILocalValue"/> used to store errors.
    /// </summary>
    public class ErrorValue : ILocalValue
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorValue"/> class.
        /// </summary>
        /// <param name="value">The value stored in the <see cref="ErrorValue"/>.</param>
        public ErrorValue(CimbolRuntimeException value)
        {
            Value = value;
        }

        /// <summary>
        /// The value stored in the <see cref="ErrorValue"/>.
        /// </summary>
        public CimbolRuntimeException Value { get; }

        /// <inheritdoc cref="ILocalValue.Access"/>
        public ILocalValue Access(string key)
        {
            throw CimbolRuntimeException.AccessError(null);
        }

        /// <inheritdoc cref="ILocalValue.CastBoolean"/>
        public BooleanValue CastBoolean()
        {
            throw CimbolRuntimeException.CastBooleanError(null, typeof(ErrorValue));
        }

        /// <inheritdoc cref="ILocalValue.CastNumber"/>
        public NumberValue CastNumber()
        {
            throw CimbolRuntimeException.CastNumberError(null, typeof(ErrorValue));
        }

        /// <inheritdoc cref="ILocalValue.CastString"/>
        public StringValue CastString()
        {
            throw CimbolRuntimeException.CastStringError(null, typeof(ErrorValue));
        }

        /// <inheritdoc cref="ILocalValue.EqualTo"/>
        public bool EqualTo(ILocalValue other)
        {
            return false;
        }

        /// <inheritdoc cref="ILocalValue.Invoke"/>
        public ILocalValue Invoke(params ILocalValue[] arguments)
        {
            throw CimbolRuntimeException.InvocationError(null);
        }
    }
}