using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Cimpress.Cimbol.Compiler.Utilities
{
    /// <summary>
    /// A collection of utility methods for escaping and unescaping strings.
    /// </summary>
    public static class StringEscaper
    {
        private static readonly Regex EscapeRegex = new Regex(
            @"[\x00-\x1F\\""'\x7F-\x9F]",
            RegexOptions.Compiled);

        private static readonly Regex UnescapeRegex = new Regex(
            @"\\u[0-9A-Fa-f]{4}|\\U[0-9A-Fa-f]{8}|\\[\\""'nrt]",
            RegexOptions.Compiled);

        /// <summary>
        /// Given a string, return the escaped version of that string.
        /// </summary>
        /// <param name="source">The string to escape.</param>
        /// <returns>An escaped string.</returns>
        public static string EscapeString(string source)
        {
            var escaped = EscapeRegex.Replace(
                source,
                m =>
                {
                    switch (m.Value)
                    {
                        case "\"":
                            return "\\\"";

                        case "'":
                            return "\\'";

                        case "\\":
                            return "\\\\";

                        case "\n":
                            return "\\n";

                        case "\r":
                            return "\\r";

                        case "\t":
                            return "\\t";

                        default:
                            return UnicodeToEscapeSequence(char.ConvertToUtf32(m.Value, 0));
                    }
                });

            return escaped;
        }

        /// <summary>
        /// Given an escaped string, return the unescaped version of that string.
        /// </summary>
        /// <param name="source">The string to unescape.</param>
        /// <returns>An unescaped string.</returns>
        public static string UnescapeString(string source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var unescaped = UnescapeRegex.Replace(
                source,
                m =>
                {
                    switch (m.Value.Substring(1, 1))
                    {
                        case "n":
                            return "\n";

                        case "r":
                            return "\r";

                        case "t":
                            return "\t";

                        case "u":
                            return EscapeSequenceToUnicode(m.Value.Substring(2, 4));

                        case "U":
                            return EscapeSequenceToUnicode(m.Value.Substring(2, 8));

                        case "\\":
                            return "\\";

                        case "\"":
                            return "\"";

                        case "'":
                            return "'";

                        default:
                            return m.Value;
                    }
                });

            return unescaped;
        }

        private static string EscapeSequenceToUnicode(string escapeSequence)
        {
            var code = int.Parse(escapeSequence, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            return Convert.ToChar(code).ToString(CultureInfo.InvariantCulture);
        }

        private static string UnicodeToEscapeSequence(int codepoint)
        {
            var fmtString = codepoint > 0xFFFF ? "\\U{0:D8}" : "\\u{0:D4}";
            return string.Format(CultureInfo.InvariantCulture, fmtString, codepoint);
        }
    }
}