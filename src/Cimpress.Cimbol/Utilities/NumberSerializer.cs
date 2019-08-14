using System;
using System.Globalization;

namespace Cimpress.Cimbol.Utilities
{
    /// <summary>
    /// A collection of helper methods for serializing and deserializing numbers.
    /// </summary>
    public static class NumberSerializer
    {
        /// <summary>
        /// Deserialize a string containing a number into a number value.
        /// </summary>
        /// <param name="numberSource">A string container a number.</param>
        /// <returns>A number value.</returns>
        public static decimal DeserializeNumber(string numberSource)
        {
            if (decimal.TryParse(numberSource, out var number))
            {
                return number;
            }

            // Expected a number but received something else.
            throw new NotSupportedException("ErrorCode082");
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
    }
}