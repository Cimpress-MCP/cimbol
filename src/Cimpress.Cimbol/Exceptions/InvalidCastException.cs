using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Serialization;
using Cimpress.Cimbol.Runtime.Types;

namespace Cimpress.Cimbol.Exceptions
{
    /// <summary>
    /// The exception thrown when an error occurs casting from one type to another.
    /// </summary>
    [Serializable]
    public class InvalidCastException : CimbolRuntimeException
    {
        private const string DefaultMessage = "Cannot convert between the two provided types.";

        private const string ParametricMessage = "Cannot convert from type {0} to type {1}.";

        private static readonly IDictionary<Type, string> TypeMap = new Dictionary<Type, string>
        {
            { typeof(BooleanValue), "boolean" },
            { typeof(FunctionValue), "function" },
            { typeof(ListValue), "list" },
            { typeof(NumberValue), "number" },
            { typeof(ObjectValue), "object" },
            { typeof(PromiseValue), "promise" },
            { typeof(StringValue), "string" },
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidCastException"/> class.
        /// </summary>
        public InvalidCastException()
            : base(DefaultMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidCastException"/> class
        /// with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public InvalidCastException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidCastException"/> class
        /// with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or null if no inner exception is specified.</param>
        public InvalidCastException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidCastException"/> class.
        /// </summary>
        /// <param name="fromType">The type the was the cast was from.</param>
        /// <param name="toType">The type that was the cast was to.</param>
        public InvalidCastException(Type fromType, Type toType)
            : base(FormatMessage(fromType, toType))
        {
            FromType = fromType;

            ToType = toType;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CimbolRuntimeException"/> class.
        /// </summary>
        /// <param name="serializationInfo">The <see cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="streamingContext">The <see cref="StreamingContext"/> that contains contextual information about the source or destination.</param>
        protected InvalidCastException(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        {
        }

        /// <summary>
        /// The type that was the cast was from.
        /// </summary>
        public Type FromType { get; }

        /// <summary>
        /// The type that the case was to.
        /// </summary>
        public Type ToType { get; }

        private static string FormatMessage(Type fromType, Type toType)
        {
            return string.Format(
                CultureInfo.InvariantCulture,
                ParametricMessage,
                GetTypePlainText(fromType),
                GetTypePlainText(toType));
        }

        private static string GetTypePlainText(Type type)
        {
            const string unknownType = "user defined type";

            return TypeMap.TryGetValue(type, out var result) ? result : unknownType;
        }
    }
}