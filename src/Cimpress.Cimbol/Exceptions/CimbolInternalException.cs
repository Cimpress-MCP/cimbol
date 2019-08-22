using System;
using System.Runtime.Serialization;

namespace Cimpress.Cimbol.Exceptions
{
    /// <summary>
    /// The base exception for exceptions internal to Cimbol.
    /// </summary>
    public class CimbolInternalException : InvalidOperationException
    {
        private const string DefaultMessage = "An error internal to Cimbol has occurred.";

        /// <summary>
        /// Initializes a new instance of the <see cref="CimbolInternalException"/> class.
        /// </summary>
        public CimbolInternalException()
            : base(DefaultMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CimbolInternalException"/> class
        /// with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public CimbolInternalException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CimbolInternalException"/> class
        /// with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or null if no inner exception is specified.</param>
        public CimbolInternalException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CimbolInternalException"/> class.
        /// </summary>
        /// <param name="serializationInfo">The <see cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="streamingContext">The <see cref="StreamingContext"/> that contains contextual information about the source or destination.</param>
        protected CimbolInternalException(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        {
        }
    }
}