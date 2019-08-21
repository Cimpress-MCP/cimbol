using System;
using System.Globalization;
using System.Runtime.Serialization;
using Cimpress.Cimbol.Compiler.Scan;
using Cimpress.Cimbol.Utilities;

namespace Cimpress.Cimbol.Exceptions
{
    /// <summary>
    /// The base exception for Cimbol compilation exceptions.
    /// </summary>
    [Serializable]
    public class CimbolCompilationException : Exception
    {
        private const string DefaultMessage = "An error occurred compiling an expression.";

        /// <summary>
        /// Initializes a new instance of the <see cref="CimbolCompilationException"/> class.
        /// </summary>
        public CimbolCompilationException()
            : base(DefaultMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CimbolCompilationException"/> class
        /// with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public CimbolCompilationException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CimbolCompilationException"/> class
        /// with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or null if no inner exception is specified.</param>
        public CimbolCompilationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CimbolCompilationException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="formula">The formula name where the exception occurred.</param>
        public CimbolCompilationException(string message, string formula)
            : base(message)
        {
            Formula = formula;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CimbolCompilationException"/> class with the position of the error.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="formula">The formula name where the exception occurred.</param>
        /// <param name="position">The position of the exception.</param>
        public CimbolCompilationException(string message, string formula, Position? position)
            : base(message)
        {
            End = position;

            Formula = formula;

            Start = position;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CimbolCompilationException"/> class with the start and end positions of the error.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="formula">The formula name where the exception occurred.</param>
        /// <param name="start">The starting position of the exception.</param>
        /// <param name="end">The ending position of the exception.</param>
        public CimbolCompilationException(string message, string formula, Position? start, Position? end)
            : base(message)
        {
            End = end;

            Formula = formula;

            Start = start;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CimbolCompilationException"/> class.
        /// </summary>
        /// <param name="serializationInfo">The <see cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="streamingContext">The <see cref="StreamingContext"/> that contains contextual information about the source or destination.</param>
        protected CimbolCompilationException(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        {
        }

        /// <summary>
        /// The ending position of the exception.
        /// </summary>
        public Position? End { get; }

        /// <summary>
        /// The formula where the exception occurred.
        /// </summary>
        public string Formula { get; }

        /// <summary>
        /// The starting position of the exception.
        /// </summary>
        public Position? Start { get; }

        /// <summary>
        /// Builds a <see cref="CimbolCompilationException"/> class with a message about the existence of cycles in the program.
        /// </summary>
        /// <param name="formula">The formula name where the exception occurred.</param>
        /// <returns>An instance of the <see cref="CimbolCompilationException"/> class with a pre-populated error message.</returns>
        internal static CimbolCompilationException CycleError(string formula)
        {
            const string message = "A formula cannot depend on formulas that in turn depend on it.";

            return new CimbolCompilationException(message, formula);
        }

        /// <summary>
        /// Builds a <see cref="CimbolCompilationException"/> class with a message that a terminal was expected but none was found.
        /// </summary>
        /// <param name="formula">The formula name where the exception occurred.</param>
        /// <param name="start">The starting position of the exception.</param>
        /// <param name="end">The ending position of the exception.</param>
        /// <returns>An instance of the <see cref="CimbolCompilationException"/> class with a pre-populated error message.</returns>
        internal static CimbolCompilationException ExpectedTerminalError(string formula, Position start, Position end)
        {
            const string message = "Expected an identifier, number, string, or boolean.";

            return new CimbolCompilationException(message, formula, start, end);
        }

        /// <summary>
        /// Builds a <see cref="CimbolCompilationException"/> class with a message about the required number of branches in an if-expression.
        /// </summary>
        /// <param name="formula">The formula name where the exception occurred.</param>
        /// <param name="start">The starting position of the exception.</param>
        /// <param name="end">The ending position of the exception.</param>
        /// <returns>An instance of the <see cref="CimbolCompilationException"/> class with a pre-populated error message.</returns>
        internal static CimbolCompilationException IfExpressionBranchCountError(
            string formula,
            Position start,
            Position end)
        {
            const string message = "An if-expression needs at least one branch.";

            return new CimbolCompilationException(message, formula, start, end);
        }

        /// <summary>
        /// Builds a <see cref="CimbolCompilationException"/> class with a message about an unknown branch in an if-expression.
        /// </summary>
        /// <param name="formula">The formula name where the exception occurred.</param>
        /// <param name="start">The starting position of the exception.</param>
        /// <param name="end">The ending position of the exception.</param>
        /// <param name="branchName">The name of the unknown branch.</param>
        /// <returns>An instance of the <see cref="CimbolCompilationException"/> class with a pre-populated error message.</returns>
        internal static CimbolCompilationException IfExpressionUnknownBranchError(
            string formula,
            Position start,
            Position end,
            string branchName)
        {
            const string message = "An unrecognized branch was found in an if-expression: {0}.";

            var formattedMessage = string.Format(
                CultureInfo.InvariantCulture,
                message,
                IdentifierSerializer.SerializeIdentifier(branchName));

            return new CimbolCompilationException(formattedMessage, formula, start, end);
        }

        /// <summary>
        /// Builds a <see cref="CimbolCompilationException"/> class with a message about an escaped identifier containing a new line.
        /// </summary>
        /// <param name="formula">The formula name where the exception occurred.</param>
        /// <param name="start">The starting position of the exception.</param>
        /// <param name="end">The ending position of the exception.</param>
        /// <returns>An instance of the <see cref="CimbolCompilationException"/> class with a pre-populated error message.</returns>
        internal static CimbolCompilationException IdentifierNewLineError(string formula, Position start, Position end)
        {
            const string message = "Identifiers cannot contain new lines.";

            return new CimbolCompilationException(message, formula, start, end);
        }

        /// <summary>
        /// Builds a <see cref="CimbolCompilationException"/> class with a message about an escaped identifier starting without a single quote.
        /// </summary>
        /// <param name="formula">The formula name where the exception occurred.</param>
        /// <param name="start">The starting position of the exception.</param>
        /// <param name="end">The ending position of the exception.</param>
        /// <returns>An instance of the <see cref="CimbolCompilationException"/> class with a pre-populated error message.</returns>
        internal static CimbolCompilationException IdentifierQuoteStartError(
            string formula,
            Position start,
            Position end)
        {
            const string message = "Escaped identifiers must start with a single quote.";

            return new CimbolCompilationException(message, formula, start, end);
        }

        /// <summary>
        /// Builds a <see cref="CimbolCompilationException"/> class with a message about an identifier not starting with a letter or underscore.
        /// </summary>
        /// <param name="formula">The formula name where the exception occurred.</param>
        /// <param name="start">The starting position of the exception.</param>
        /// <param name="end">The ending position of the exception.</param>
        /// <returns>An instance of the <see cref="CimbolCompilationException"/> class with a pre-populated error message.</returns>
        internal static CimbolCompilationException IdentifierStartError(string formula, Position start, Position end)
        {
            const string message = "Identifiers must start with either a letter or an underscore.";

            return new CimbolCompilationException(message, formula, start, end);
        }

        /// <summary>
        /// Builds a <see cref="CimbolCompilationException"/> class with a message about the invalid hexadecimal unicode sequence that was encountered.
        /// </summary>
        /// <param name="formula">The formula name where the exception occurred.</param>
        /// <param name="start">The starting position of the exception.</param>
        /// <param name="end">The ending position of the exception.</param>
        /// <returns>An instance of the <see cref="CimbolCompilationException"/> class with a pre-populated error message.</returns>
        internal static CimbolCompilationException InvalidHexadecimalSequenceError(
            string formula,
            Position start,
            Position end)
        {
            const string message = "Hexadecimal sequences can only contain the letters A-F and the number 0-9.";

            return new CimbolCompilationException(message, formula, start, end);
        }

        /// <summary>
        /// Builds a <see cref="CimbolCompilationException"/> class with a message about the invalid unicode sequence that was encountered.
        /// </summary>
        /// <param name="formula">The formula name where the exception occurred.</param>
        /// <param name="start">The starting position of the exception.</param>
        /// <param name="end">The ending position of the exception.</param>
        /// <returns>An instance of the <see cref="CimbolCompilationException"/> class with a pre-populated error message.</returns>
        internal static CimbolCompilationException InvalidUnicodeSequenceError(
            string formula,
            Position start,
            Position end)
        {
            const string message = "Encountered a malformed text sequence.";

            return new CimbolCompilationException(message, formula, start, end);
        }

        /// <summary>
        /// Builds a <see cref="CimbolCompilationException"/> class with a message about the maximum allowed size of an exponent.
        /// </summary>
        /// <param name="formula">The formula name where the exception occurred.</param>
        /// <param name="start">The starting position of the exception.</param>
        /// <param name="end">The ending position of the exception.</param>
        /// <returns>An instance of the <see cref="CimbolCompilationException"/> class with a pre-populated error message.</returns>
        internal static CimbolCompilationException NumberExponentMaxSizeError(
            string formula,
            Position start,
            Position end)
        {
            const string message = "The exponent component of a number cannot be more than three digits in length.";

            return new CimbolCompilationException(message, formula, start, end);
        }

        /// <summary>
        /// Builds a <see cref="CimbolCompilationException"/> class with a message about the minimum allowed size of an exponent.
        /// </summary>
        /// <param name="formula">The formula name where the exception occurred.</param>
        /// <param name="start">The starting position of the exception.</param>
        /// <param name="end">The ending position of the exception.</param>
        /// <returns>An instance of the <see cref="CimbolCompilationException"/> class with a pre-populated error message.</returns>
        internal static CimbolCompilationException NumberExponentMinSizeError(
            string formula,
            Position start,
            Position end)
        {
            const string message = "The exponent component of a number must be at least one digit in length.";

            return new CimbolCompilationException(message, formula, start, end);
        }

        /// <summary>
        /// Builds a <see cref="CimbolCompilationException"/> class with a message about a poorly-formatted number.
        /// </summary>
        /// <param name="formula">The formula name where the exception occurred.</param>
        /// <param name="start">The starting position of the exception.</param>
        /// <param name="end">The ending position of the exception.</param>
        /// <returns>An instance of the <see cref="CimbolCompilationException"/> class with a pre-populated error message.</returns>
        internal static CimbolCompilationException NumberFormatError(string formula, Position start, Position end)
        {
            const string message = "Numbers must have a whole or decimal part.";

            return new CimbolCompilationException(message, formula, start, end);
        }

        /// <summary>
        /// Builds a <see cref="CimbolCompilationException"/> class with a message about a number that doesn't start with a digit or period.
        /// </summary>
        /// <param name="formula">The formula name where the exception occurred.</param>
        /// <param name="start">The starting position of the exception.</param>
        /// <param name="end">The ending position of the exception.</param>
        /// <returns>An instance of the <see cref="CimbolCompilationException"/> class with a pre-populated error message.</returns>
        internal static CimbolCompilationException NumberStartError(string formula, Position start, Position end)
        {
            const string message = "Numbers must start with a digit or period.";

            return new CimbolCompilationException(message, formula, start, end);
        }

        /// <summary>
        /// Builds a <see cref="CimbolCompilationException"/> class with a message about the new line present in the string literal.
        /// </summary>
        /// <param name="formula">The formula name where the exception occurred.</param>
        /// <param name="start">The starting position of the exception.</param>
        /// <param name="end">The ending position of the exception.</param>
        /// <returns>An instance of the <see cref="CimbolCompilationException"/> class with a pre-populated error message.</returns>
        internal static CimbolCompilationException StringNewLineError(string formula, Position start, Position end)
        {
            const string message = "Strings cannot contain new lines.";

            return new CimbolCompilationException(message, formula, start, end);
        }

        /// <summary>
        /// Builds a <see cref="CimbolCompilationException"/> class with a message about the missing double quote at the start of a string.
        /// </summary>
        /// <param name="formula">The formula name where the exception occurred.</param>
        /// <param name="start">The starting position of the exception.</param>
        /// <param name="end">The ending position of the exception.</param>
        /// <returns>An instance of the <see cref="CimbolCompilationException"/> class with a pre-populated error message.</returns>
        internal static CimbolCompilationException StringQuoteStartError(
            string formula,
            Position start,
            Position end)
        {
            const string message = "Strings must start and end with a double quote.";

            return new CimbolCompilationException(message, formula, start, end);
        }

        /// <summary>
        /// Builds a <see cref="CimbolCompilationException"/> class with a message that one token type was expected but another was received.
        /// </summary>
        /// <param name="formula">The formula name where the exception occurred.</param>
        /// <param name="start">The starting position of the exception.</param>
        /// <param name="end">The ending position of the exception.</param>
        /// <param name="expected">The expected token type.</param>
        /// <param name="received">The received token type.</param>
        /// <returns>An instance of the <see cref="CimbolCompilationException"/> class with a pre-populated error message.</returns>
        internal static CimbolCompilationException TokenMismatchError(
            string formula,
            Position start,
            Position end,
            TokenType expected,
            TokenType received)
        {
            const string message = "Expected a token of type {0} but received a token of type {1}.";

            var expectedTokenName = expected.ToString();

            var receivedTokenName = received.ToString();

            var formattedMessage = string.Format(
                CultureInfo.InvariantCulture,
                message,
                expectedTokenName,
                receivedTokenName);

            return new CimbolCompilationException(formattedMessage, formula, start, end);
        }

        /// <summary>
        /// Builds a <see cref="CimbolCompilationException"/> class with a message about the end-of-file that was encountered.
        /// </summary>
        /// <param name="formula">The formula name where the exception occurred.</param>
        /// <param name="start">The starting position of the exception.</param>
        /// <param name="end">The ending position of the exception.</param>
        /// <returns>An instance of the <see cref="CimbolCompilationException"/> class with a pre-populated error message.</returns>
        internal static CimbolCompilationException UnexpectedEndOfFileError(
            string formula,
            Position start,
            Position end)
        {
            const string message = "The end of a formula was reached unexpectedly.";

            return new CimbolCompilationException(message, formula, start, end);
        }

        /// <summary>
        /// Builds a <see cref="CimbolCompilationException"/> class with a message about the unrecognized character that was encountered.
        /// </summary>
        /// <param name="formula">The formula name where the exception occurred.</param>
        /// <param name="start">The starting position of the exception.</param>
        /// <param name="end">The ending position of the exception.</param>
        /// <param name="character">The character that was encountered.</param>
        /// <returns>An instance of the <see cref="CimbolCompilationException"/> class with a pre-populated error message.</returns>
        internal static CimbolCompilationException UnrecognizedCharacterError(
            string formula,
            Position start,
            Position end,
            string character)
        {
            const string message = "An unrecognized character was encountered: {0}.";

            var formattedMessage = string.Format(CultureInfo.InvariantCulture, message, character);

            return new CimbolCompilationException(formattedMessage, formula, start, end);
        }

        /// <summary>
        /// Builds a <see cref="CimbolCompilationException"/> class with a message about the unrecognized escape sequence that was encountered.
        /// </summary>
        /// <param name="formula">The formula name where the exception occurred.</param>
        /// <param name="start">The starting position of the exception.</param>
        /// <param name="end">The ending position of the exception.</param>
        /// <param name="escapeSequence">The escape sequence that was encountered.</param>
        /// <returns>An instance of the <see cref="CimbolCompilationException"/> class with a pre-populated error message.</returns>
        internal static CimbolCompilationException UnrecognizedEscapeSequenceError(
            string formula,
            Position start,
            Position end,
            string escapeSequence)
        {
            const string message = "An unrecognized escape sequence was encountered: {0}.";

            var formattedMessage = string.Format(CultureInfo.InvariantCulture, message, escapeSequence);

            return new CimbolCompilationException(formattedMessage, formula, start, end);
        }

        /// <summary>
        /// Builds a <see cref="CimbolCompilationException"/> class with a message about the unrecognized operator that was encountered.
        /// </summary>
        /// <param name="formula">The formula name where the exception occurred.</param>
        /// <param name="start">The starting position of the exception.</param>
        /// <param name="end">The ending position of the exception.</param>
        /// <param name="operator">The operator that was encountered.</param>
        /// <returns>An instance of the <see cref="CimbolCompilationException"/> class with a pre-populated error message.</returns>
        internal static CimbolCompilationException UnrecognizedOperatorError(
            string formula,
            Position start,
            Position end,
            string @operator)
        {
            const string message = "An unrecognized operator was encountered: {0}.";

            var formattedMessage = string.Format(CultureInfo.InvariantCulture, message, @operator);

            return new CimbolCompilationException(formattedMessage, formula, start, end);
        }

        /// <summary>
        /// Builds a <see cref="CimbolCompilationException"/> class with a message about the required number of branches in a where-expression.
        /// </summary>
        /// <param name="formula">The formula name where the exception occurred.</param>
        /// <param name="start">The starting position of the exception.</param>
        /// <param name="end">The ending position of the exception.</param>
        /// <returns>An instance of the <see cref="CimbolCompilationException"/> class with a pre-populated error message.</returns>
        internal static CimbolCompilationException WhereExpressionBranchCountError(
            string formula,
            Position start,
            Position end)
        {
            const string message = "A where-expression needs at least one branch.";

            return new CimbolCompilationException(message, formula, start, end);
        }

        /// <summary>
        /// Builds a <see cref="CimbolCompilationException"/> class with a message about an unknown branch in an where-expression.
        /// </summary>
        /// <param name="formula">The formula name where the exception occurred.</param>
        /// <param name="start">The starting position of the exception.</param>
        /// <param name="end">The ending position of the exception.</param>
        /// <param name="branchName">The name of the unknown branch.</param>
        /// <returns>An instance of the <see cref="CimbolCompilationException"/> class with a pre-populated error message.</returns>
        internal static CimbolCompilationException WhereExpressionUnknownBranchError(
            string formula,
            Position start,
            Position end,
            string branchName)
        {
            const string message = "An unrecognized branch was found in a a where-expression: {0}.";

            var formattedMessage = string.Format(
                CultureInfo.InvariantCulture,
                message,
                IdentifierSerializer.SerializeIdentifier(branchName));

            return new CimbolCompilationException(formattedMessage, formula, start, end);
        }
    }
}