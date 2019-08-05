using System;
using System.Text.RegularExpressions;

namespace Cimpress.Cimbol.Utilities
{
    /// <summary>
    /// A collection of helper methods for serializing and deserializing identifiers.
    /// </summary>
    public static class IdentifierSerializer
    {
        private static readonly Regex BareIdentifierRegex = new Regex("^[_\\p{L}][_\\p{L}\\p{N}]*$");

        private static readonly Regex EscapedIdentifierRegex = new Regex("^'.*'$");

        /// <summary>
        /// Deserialize a string containing an identifier into an identifier value.
        /// </summary>
        /// <param name="identifierSource">A string container an identifier.</param>
        /// <returns>An identifier value.</returns>
        public static string DeserializeIdentifier(string identifierSource)
        {
            if (identifierSource == null)
            {
                throw new ArgumentNullException(nameof(identifierSource));
            }

            if (BareIdentifierRegex.IsMatch(identifierSource))
            {
                return identifierSource;
            }

            if (EscapedIdentifierRegex.IsMatch(identifierSource))
            {
                var unquoted = identifierSource.Substring(1, identifierSource.Length - 2);
                var unescaped = StringEscaper.UnescapeString(unquoted);
                return unescaped;
            }

            throw new NotSupportedException();
        }

        /// <summary>
        /// Serialize an identifier value into a string containing an identifier.
        /// </summary>
        /// <param name="identifierSource">An identifier value.</param>
        /// <returns>A string containing an identifier.</returns>
        public static string SerializeIdentifier(string identifierSource)
        {
            if (identifierSource == null)
            {
                throw new ArgumentNullException(nameof(identifierSource));
            }

            if (BareIdentifierRegex.IsMatch(identifierSource))
            {
                return identifierSource;
            }

            var escaped = StringEscaper.EscapeString(identifierSource);
            var quoted = $"'{escaped}'";
            return quoted;
        }
    }
}