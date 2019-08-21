using System;
using System.Globalization;
using System.Runtime.Serialization;
using Cimpress.Cimbol.Utilities;

namespace Cimpress.Cimbol.Exceptions
{
    /// <summary>
    /// The base exception for Cimbol runtime exceptions.
    /// </summary>
    [Serializable]
    public class CimbolRuntimeException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CimbolRuntimeException"/> class.
        /// </summary>
        public CimbolRuntimeException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CimbolRuntimeException"/> class
        /// with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public CimbolRuntimeException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CimbolRuntimeException"/> class
        /// with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or null if no inner exception is specified.</param>
        public CimbolRuntimeException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CimbolRuntimeException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="formula">The formula name where the exception occurred.</param>
        public CimbolRuntimeException(string message, string formula)
            : base(message)
        {
            Formula = formula;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CimbolRuntimeException"/> class.
        /// </summary>
        /// <param name="serializationInfo">The <see cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="streamingContext">The <see cref="StreamingContext"/> that contains contextual information about the source or destination.</param>
        protected CimbolRuntimeException(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        {
        }

        /// <summary>
        /// The formula where the exception occurred.
        /// </summary>
        public string Formula { get; }

        /// <summary>
        /// Builds a <see cref="CimbolRuntimeException"/> class with a message about a value that does not support accessing.
        /// </summary>
        /// <param name="formula">The formula name where the exception occurred.</param>
        /// <returns>An instance of the <see cref="CimbolRuntimeException"/> class with a pre-populated error message.</returns>
        internal static CimbolRuntimeException AccessError(string formula)
        {
            const string message = "The given type does not support member access.";

            return new CimbolRuntimeException(message, formula);
        }

        /// <summary>
        /// Builds a <see cref="CimbolRuntimeException"/> class with a message about a function called with the wrong number of arguments.
        /// </summary>
        /// <param name="formula">The formula name where the exception occurred.</param>
        /// <param name="expected">The expected number of arguments.</param>
        /// <param name="received">The received number of arguments.</param>
        /// <returns>An instance of the <see cref="CimbolRuntimeException"/> class with a pre-populated error message.</returns>
        internal static CimbolRuntimeException ArgumentCountError(string formula, int expected, int received)
        {
            const string message = "Expected {0} {2} but {1} {3} provided.";

            var argumentString = expected == 1 ? "argument" : "arguments";

            var wasString = received == 1 ? "was" : "were";

            var formattedMessage = string.Format(
                CultureInfo.InvariantCulture,
                message,
                expected,
                received,
                argumentString,
                wasString);

            return new CimbolRuntimeException(formattedMessage, formula);
        }

        /// <summary>
        /// Builds a <see cref="CimbolRuntimeException"/> class with a message about a function called with the wrong type of arguments.
        /// </summary>
        /// <param name="formula">The formula name where the exception occurred.</param>
        /// <param name="expected">The type that was expected.</param>
        /// <param name="received">The type that was received.</param>
        /// <returns>An instance of the <see cref="CimbolRuntimeException"/> class with a pre-populated error message.</returns>
        internal static CimbolRuntimeException ArgumentTypeError(string formula, Type expected, Type received)
        {
            const string message = "Expected an argument of type {0} but received an argument of type {1}.";

            var formattedMessage = string.Format(
                CultureInfo.InvariantCulture,
                message,
                expected.Name,
                received.Name);

            return new CimbolRuntimeException(formattedMessage, formula);
        }

        /// <summary>
        /// Builds a <see cref="CimbolRuntimeException"/> class with a message about a value that is not a promise being awaited.
        /// </summary>
        /// <param name="formula">The formula name where the exception occurred.</param>
        /// <returns>An instance of the <see cref="CimbolRuntimeException"/> class with a pre-populated error message.</returns>
        internal static CimbolRuntimeException AwaitError(string formula)
        {
            const string message = "Cannot await on the result of a value that is not a promise.";

            return new CimbolRuntimeException(message, formula);
        }

        /// <summary>
        /// Builds a <see cref="CimbolRuntimeException"/> class with a message about a type that could not be converted to a boolean.
        /// </summary>
        /// <param name="formula">The formula name where the exception occurred.</param>
        /// <param name="received">The type that could be cast to a boolean.</param>
        /// <returns>An instance of the <see cref="CimbolRuntimeException"/> class with a pre-populated error message.</returns>
        internal static CimbolRuntimeException CastBooleanError(string formula, Type received)
        {
            const string message = "Cannot convert from a {0} to a boolean.";

            // TODO: Improve the way to determine what name to display for a type.
            var typeName = received.Name;

            var formattedMessage = string.Format(CultureInfo.InvariantCulture, message, typeName);

            return new CimbolRuntimeException(formattedMessage, formula);
        }

        /// <summary>
        /// Builds a <see cref="CimbolRuntimeException"/> class with a message about a type that could not be converted to a number.
        /// </summary>
        /// <param name="formula">The formula name where the exception occurred.</param>
        /// <param name="received">The type that could not be cast to a number.</param>
        /// <returns>An instance of the <see cref="CimbolRuntimeException"/> class with a pre-populated error message.</returns>
        internal static CimbolRuntimeException CastNumberError(string formula, Type received)
        {
            const string message = "Cannot convert from a {0} to a number.";

            // TODO: Improve the way to determine what name to display for a type.
            var typeName = received.Name;

            var formattedMessage = string.Format(CultureInfo.InvariantCulture, message, typeName);

            return new CimbolRuntimeException(formattedMessage, formula);
        }

        /// <summary>
        /// Builds a <see cref="CimbolRuntimeException"/> class with a message about a type that could not be converted to a string.
        /// </summary>
        /// <param name="formula">The formula name where the exception occurred.</param>
        /// <param name="received">The type that could not be cast to a string.</param>
        /// <returns>An instance of the <see cref="CimbolRuntimeException"/> class with a pre-populated error message.</returns>
        internal static CimbolRuntimeException CastStringError(string formula, Type received)
        {
            const string message = "Cannot convert from a {0} to a string.";

            // TODO: Improve the way to determine what name to display for a type.
            var typeName = received.Name;

            var formattedMessage = string.Format(CultureInfo.InvariantCulture, message, typeName);

            return new CimbolRuntimeException(formattedMessage, formula);
        }

        /// <summary>
        /// Builds a <see cref="CimbolRuntimeException"/> class with a message about an attempt to divide by zero.
        /// </summary>
        /// <param name="formula">The formula name where the exception occurred.</param>
        /// <returns>An instance of the <see cref="CimbolRuntimeException"/> class with a pre-populated error message.</returns>
        internal static CimbolRuntimeException DivideByZeroError(string formula)
        {
            const string message = "Attempted to divide by zero.";

            return new CimbolRuntimeException(message, formula);
        }

        /// <summary>
        /// Builds a <see cref="CimbolRuntimeException"/> class with a message about no condition being met in an if-expression.
        /// </summary>
        /// <param name="formula">The formula name where the exception occurred.</param>
        /// <returns>An instance of the <see cref="CimbolRuntimeException"/> class with a pre-populated error message.</returns>
        internal static CimbolRuntimeException IfConditionError(string formula)
        {
            const string message = "No condition was met in the if-expression.";

            return new CimbolRuntimeException(message, formula);
        }

        /// <summary>
        /// Builds a <see cref="CimbolRuntimeException"/> class with a message about a value that is not callable being invoked.
        /// </summary>
        /// <param name="formula">The formula name where the exception occurred.</param>
        /// <returns>An instance of the <see cref="CimbolRuntimeException"/> class with a pre-populated error message.</returns>
        internal static CimbolRuntimeException InvocationError(string formula)
        {
            const string message = "The given type does not support being called like a function.";

            return new CimbolRuntimeException(message, formula);
        }

        /// <summary>
        /// Builds a <see cref="CimbolRuntimeException"/> class with a message about a member not being found on an object.
        /// </summary>
        /// <param name="formula">The formula name where the exception occurred.</param>
        /// <param name="member">The member that was not found on the object.</param>
        /// <returns>An instance of the <see cref="CimbolRuntimeException"/> class with a pre-populated error message.</returns>
        internal static CimbolRuntimeException MemberNotFoundError(string formula, string member)
        {
            const string message = "The member {0} was not found on the object.";

            var formattedMessage = string.Format(
                CultureInfo.InvariantCulture,
                message,
                IdentifierSerializer.SerializeIdentifier(member));

            return new CimbolRuntimeException(formattedMessage, formula);
        }

        /// <summary>
        /// Builds a <see cref="CimbolRuntimeException"/> class with a message about a string that could not be converted to a boolean.
        /// </summary>
        /// <param name="formula">The formula name where the exception occurred.</param>
        /// <param name="value">The string value that could not be converted a string.</param>
        /// <returns>An instance of the <see cref="CimbolRuntimeException"/> class with a pre-populated error message.</returns>
        internal static CimbolRuntimeException NotABooleanError(string formula, string value)
        {
            const string message = "The string {0} could not be converted to a boolean.";

            var formattedMessage = string.Format(
                CultureInfo.InvariantCulture,
                message,
                StringSerializer.SerializeString(value));

            return new CimbolRuntimeException(formattedMessage, formula);
        }

        /// <summary>
        /// Builds a <see cref="CimbolRuntimeException"/> class with a message about a string that could not be converted to a number.
        /// </summary>
        /// <param name="formula">The formula name where the exception occurred.</param>
        /// <param name="value">The string value that could not be converted to a number.</param>
        /// <returns>An instance of the <see cref="CimbolRuntimeException"/> class with a pre-populated error message.</returns>
        internal static CimbolRuntimeException NotANumberError(string formula, string value)
        {
            const string message = "The string {0} could not be converted to a number.";

            var formattedMessage = string.Format(
                CultureInfo.InvariantCulture,
                message,
                StringSerializer.SerializeString(value));

            return new CimbolRuntimeException(formattedMessage, formula);
        }

        /// <summary>
        /// Builds a <see cref="CimbolRuntimeException"/> class with a message about an identifier that could not be resolved.
        /// </summary>
        /// <param name="formula">The formula name where the exception occurred.</param>
        /// <param name="name">The name of the unresolved identifier.</param>
        /// <returns>An instance of the <see cref="CimbolRuntimeException"/> class with a pre-populated error message.</returns>
        internal static CimbolRuntimeException UnresolvedIdentifier(string formula, string name)
        {
            const string message = "The name {0} could not be resolved to a value.";

            var formattedMessage = string.Format(
                CultureInfo.InvariantCulture,
                message,
                IdentifierSerializer.SerializeIdentifier(name));

            return new CimbolRuntimeException(formattedMessage, formula);
        }

        /// <summary>
        /// Builds a <see cref="CimbolRuntimeException"/> class with a message about no condition being met in a where-expression.
        /// </summary>
        /// <param name="formula">The formula name where the exception occurred.</param>
        /// <returns>An instance of the <see cref="CimbolRuntimeException"/> class with a pre-populated error message.</returns>
        internal static CimbolRuntimeException WhereConditionError(string formula)
        {
            const string message = "No condition was met in the where-expression.";

            return new CimbolRuntimeException(message, formula);
        }
    }
}