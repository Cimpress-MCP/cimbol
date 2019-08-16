using System;
using System.Text.RegularExpressions;

namespace Cimpress.Cimbol.Utilities
{
    /// <summary>
    /// A collection of helper methods for serializing and deserializing strings.
    /// </summary>
    public static class StringSerializer
    {
        private static readonly Regex StringRegex = new Regex("^\".*\"$");

        /// <summary>
        /// Deserialize a string containing a string into a string value.
        /// </summary>
        /// <param name="stringSource">A string container a string.</param>
        /// <returns>A string value.</returns>
        public static string DeserializeString(string stringSource)
        {
            if (stringSource == null)
            {
                throw new ArgumentNullException(nameof(stringSource));
            }

            if (!StringRegex.IsMatch(stringSource))
            {
#pragma warning disable CA1303
                // Expected a number but received something else.
                throw new NotSupportedException("ErrorCode083");
#pragma warning restore CA1303
            }

            var unquoted = stringSource.Substring(1, stringSource.Length - 2);
            var unescaped = StringEscaper.UnescapeString(unquoted);
            return unescaped;
        }

        /// <summary>
        /// Serialize a string value into a string containing a string.
        /// </summary>
        /// <param name="stringSource">A string value.</param>
        /// <returns>A string containing a string.</returns>
        public static string SerializeString(string stringSource)
        {
            if (stringSource == null)
            {
                throw new ArgumentNullException(nameof(stringSource));
            }

            var escaped = StringEscaper.EscapeString(stringSource);
            var quoted = $"\"{escaped}\"";
            return quoted;
        }
    }
}