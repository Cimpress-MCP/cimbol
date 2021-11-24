// Copyright 2021 Cimpress plc.
// Licensed under the Apache License, Version 2.0
// http://www.apache.org/licenses/LICENSE-2.0

using System.Globalization;
using Cimpress.Cimbol.Exceptions;

namespace Cimpress.Cimbol.Utilities
{
    /// <summary>
    /// A collection of helper methods for serializing and deserializing numbers.
    /// </summary>
    public static class NumberSerializer
    {
        /// <summary>
        /// The format to use for deserializing numbers.
        /// </summary>
        public const NumberStyles NumberStyle = NumberStyles.Float;
        
        /// <summary>
        /// Deserialize a string containing a number into a number value.
        /// </summary>
        /// <param name="source">A string container a number.</param>
        /// <returns>A number value.</returns>
        public static decimal DeserializeNumber(string source)
        {
            if (TryDeserializeNumber(source, out var result))
            {
                return result;
            }

            throw new CimbolInternalException("There was an error deserializing a number.");
        }

        /// <summary>
        /// Serialize a number value into a string containing a number.
        /// </summary>
        /// <param name="numberSource">A number value.</param>
        /// <returns>A string containing a number.</returns>
        public static string SerializeNumber(decimal numberSource)
        {
            return numberSource.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Try to deserialize a some source into a number value.
        /// </summary>
        /// <param name="source">The source to deserialize.</param>
        /// <param name="result">The result of deserializing the provided source.</param>
        /// <returns>True if the deserialization was success and false otherwise.</returns>
        public static bool TryDeserializeNumber(string source, out decimal result)
        {
            return decimal.TryParse(source, NumberStyle, CultureInfo.InvariantCulture,  out result);
        }
    }
}