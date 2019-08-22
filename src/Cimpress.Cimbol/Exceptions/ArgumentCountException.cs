using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace Cimpress.Cimbol.Exceptions
{
    /// <summary>
    /// An exception thrown when the arguments provided to a Cimbol executable do not match the required number of arguments.
    /// </summary>
    [Serializable]
    public class ArgumentCountException : Exception
    {
        private const string DefaultMessage = "An incorrect number of arguments were provided to the program.";

        /// <summary>
        /// Initializes a new instance of the <see cref="ArgumentCountException"/> class.
        /// </summary>
        public ArgumentCountException()
            : base(DefaultMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArgumentCountException"/> class
        /// with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public ArgumentCountException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArgumentCountException"/> class
        /// with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or null if no inner exception is specified.</param>
        public ArgumentCountException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArgumentCountException"/> class.
        /// </summary>
        /// <param name="expected">The number of arguments that were expected to be provided.</param>
        /// <param name="received">The number of arguments received.</param>
        public ArgumentCountException(int expected, int received)
            : base(FormatMessage(expected, received))
        {
            ExpectedCount = expected;

            ReceivedCount = received;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CimbolCompilationException"/> class.
        /// </summary>
        /// <param name="serializationInfo">The <see cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="streamingContext">The <see cref="StreamingContext"/> that contains contextual information about the source or destination.</param>
        protected ArgumentCountException(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        {
        }

        /// <summary>
        /// The number of arguments that were expected to be provided.
        /// </summary>
        public int? ExpectedCount { get; }

        /// <summary>
        /// The number of arguments received.
        /// </summary>
        public int? ReceivedCount { get; }

        private static string FormatMessage(int expected, int received)
        {
            const string message = "An incorrect number of arguments were provided to the program. Expected {0} but received {1}.";

            var formattedMessage = string.Format(
                CultureInfo.InvariantCulture,
                message,
                expected,
                received);

            return formattedMessage;
        }
    }
}