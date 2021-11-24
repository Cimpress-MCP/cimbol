// Copyright 2021 Cimpress plc.
// Licensed under the Apache License, Version 2.0
// http://www.apache.org/licenses/LICENSE-2.0

using System;
using System.Text.RegularExpressions;
using Cimpress.Cimbol.Exceptions;

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
        /// <param name="source">The source to deserialize.</param>
        /// <returns>The result fo deserializing the provided source.</returns>
        public static string DeserializeString(string source)
        {
            if (TryDeserializeString(source, out var result))
            {
                return result;
            }

            throw new CimbolInternalException("There was an error deserializing a string.");
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

            return $"\"{StringEscaper.EscapeString(stringSource)}\"";
        }

        /// <summary>
        /// Try to deserialize a some source into a string value.
        /// </summary>
        /// <param name="source">The source to deserialize.</param>
        /// <param name="result">The result of deserializing the provided source.</param>
        /// <returns>True if the deserialization was success and false otherwise.</returns>
        public static bool TryDeserializeString(string source, out string result)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (!StringRegex.IsMatch(source))
            {
                result = null;

                return false;
            }

            result = StringEscaper.UnescapeString(source.Substring(1, source.Length - 2));

            return true;
        }
    }
}