// Copyright 2021 Cimpress plc.
// Licensed under the Apache License, Version 2.0
// http://www.apache.org/licenses/LICENSE-2.0

using System;
using System.Text.RegularExpressions;
using Cimpress.Cimbol.Exceptions;

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
        /// <param name="source">A string container an identifier.</param>
        /// <returns>An identifier value.</returns>
        public static string DeserializeIdentifier(string source)
        {
            if (TryDeserializeIdentifier(source, out var result))
            {
                return result;
            }

            throw new CimbolInternalException("There was an error deserializing an identifier.");
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

        /// <summary>
        /// Try to deserialize a some source into an identifier.
        /// </summary>
        /// <param name="source">The source to deserialize.</param>
        /// <param name="result">The result of deserializing the provided source.</param>
        /// <returns>True if the deserialization was success and false otherwise.</returns>
        public static bool TryDeserializeIdentifier(string source, out string result)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (BareIdentifierRegex.IsMatch(source))
            {
                result = source;

                return true;
            }

            if (EscapedIdentifierRegex.IsMatch(source))
            {
                result = StringEscaper.UnescapeString(source.Substring(1, source.Length - 2));

                return true;
            }

            result = null;

            return false;
        }
    }
}