using System;
using System.Collections.Generic;
using Cimpress.Cimbol.Exceptions;
using Cimpress.Cimbol.Runtime.Types;

namespace Cimpress.Cimbol.Runtime.Modules
{
    /// <summary>
    /// Builds an object containing a collection of math functions.
    /// </summary>
    public class MathModule
    {
        /// <summary>
        /// Constructs the math module for Cimbol.
        /// </summary>
        /// <returns>An object value containing math functions.</returns>
        public static ObjectValue Build()
        {
            var absFunction = new FunctionValue(new Delegate[]
            {
                (Func<NumberValue, NumberValue>)Abs1,
            });

            var ceilFunction = new FunctionValue(new Delegate[]
            {
                (Func<NumberValue, NumberValue>)Ceil1,
                (Func<NumberValue, NumberValue, NumberValue>)Ceil2,
            });

            var cosFunction = new FunctionValue(new Delegate[]
            {
                (Func<NumberValue, NumberValue>)Cos1,
            });

            var floorFunction = new FunctionValue(new Delegate[]
            {
                (Func<NumberValue, NumberValue>)Floor1,
                (Func<NumberValue, NumberValue, NumberValue>)Floor2,
            });

            var logFunction = new FunctionValue(new Delegate[]
            {
                (Func<NumberValue, NumberValue, NumberValue>)Log2,
            });

            var maxFunction = new FunctionValue(new Delegate[]
            {
                (Func<NumberValue, NumberValue[], NumberValue>)MaxN,
            });

            var minFunction = new FunctionValue(new Delegate[]
            {
                (Func<NumberValue, NumberValue[], NumberValue>)MinN,
            });

            var roundFunction = new FunctionValue(new Delegate[]
            {
                (Func<NumberValue, NumberValue>)Round1,
                (Func<NumberValue, NumberValue, NumberValue>)Round2,
            });

            var sinFunction = new FunctionValue(new Delegate[]
            {
                (Func<NumberValue, NumberValue>)Sin1,
            });

            var tanFunction = new FunctionValue(new Delegate[]
            {
                (Func<NumberValue, NumberValue>)Tan1,
            });

            var objectContents = new Dictionary<string, ILocalValue>(StringComparer.OrdinalIgnoreCase)
            {
                { "Abs", absFunction },
                { "Ceil", ceilFunction },
                { "Cos", cosFunction },
                { "E", new NumberValue((decimal)Math.E) },
                { "Floor", floorFunction },
                { "Log", logFunction },
                { "Max", maxFunction },
                { "Min", minFunction },
                { "Pi", new NumberValue((decimal)Math.PI) },
                { "Round", roundFunction },
                { "Sin", sinFunction },
                { "Tan", tanFunction },
            };

            return new ObjectValue(objectContents);
        }

        /// <summary>
        /// Returns the absolute value of the given number.
        /// </summary>
        /// <param name="value">The value to take the absolute value of.</param>
        /// <returns>The absolute value of the given value.</returns>
        internal static NumberValue Abs1(NumberValue value)
        {
            var result = Math.Abs(value.Value);
            return new NumberValue(result);
        }

        /// <summary>
        /// Returns the smallest integer greater than the given number.
        /// </summary>
        /// <param name="value">The value to get the ceiling of.</param>
        /// <returns>The ceiling of the given value.</returns>
        internal static NumberValue Ceil1(NumberValue value)
        {
            var result = Math.Ceiling(value.Value);
            return new NumberValue(result);
        }

        /// <summary>
        /// Returns the smallest number greater than the given number, with the given number of decimal places.
        /// </summary>
        /// <param name="value">The value to get the ceiling of.</param>
        /// <param name="decimals">The number of decimal places to preserve.</param>
        /// <returns>The ceiling of the given value.</returns>
        internal static NumberValue Ceil2(NumberValue value, NumberValue decimals)
        {
            var magnitude = (decimal)Math.Pow(10, (double)decimals.Value);
            var result = Math.Ceiling(value.Value * magnitude) / magnitude;
            return new NumberValue(result);
        }

        /// <summary>
        /// Returns the cosine of the given value.
        /// </summary>
        /// <param name="value">An angle, measured in degrees.</param>
        /// <returns>The cosine of <see cref="value"/>.</returns>
        internal static NumberValue Cos1(NumberValue value)
        {
            var radians = ((double)value.Value * Math.PI) / 180d;
            var result = (decimal)Math.Cos(radians);
            return new NumberValue(result);
        }

        /// <summary>
        /// Returns the largest integer less than the given number.
        /// </summary>
        /// <param name="value">The value to get the floor of.</param>
        /// <returns>The floor of the given value.</returns>
        internal static NumberValue Floor1(NumberValue value)
        {
            var result = Math.Floor(value.Value);
            return new NumberValue(result);
        }

        /// <summary>
        /// Returns the largest number less than the given number, with the given number of decimal places.
        /// </summary>
        /// <param name="value">The value to get the floor of.</param>
        /// <param name="decimals">The number of decimal places to preserve.</param>
        /// <returns>The floor of the given value.</returns>
        internal static NumberValue Floor2(NumberValue value, NumberValue decimals)
        {
            var magnitude = (decimal)Math.Pow(10, (double)decimals.Value);
            var result = Math.Floor(value.Value * magnitude) / magnitude;
            return new NumberValue(result);
        }

        /// <summary>
        /// Returns the logarithm of the given number in the given base.
        /// </summary>
        /// <param name="value">The number whose logarithm is to be found.</param>
        /// <param name="baseValue">The base of the logarithm.</param>
        /// <returns>The logarithm of the given value.</returns>
        internal static NumberValue Log2(NumberValue value, NumberValue baseValue)
        {
            if (value.Value <= 0)
            {
                throw CimbolRuntimeException.DomainException(nameof(value), "greater than zero");
            }
    
            if (baseValue.Value <= 0)
            {
                throw CimbolRuntimeException.DomainException(nameof(baseValue), "greater than zero");
            }

            var result = (decimal)Math.Log((double)value.Value, (double)baseValue.Value);
            return new NumberValue(result);
        } 

        /// <summary>
        /// Finds the maximum value in the given list of numbers.
        /// </summary>
        /// <param name="value1">The first, required value to take the maximum of.</param>
        /// <param name="values">An optional list of additional numbers to take the maximum of.</param>
        /// <returns>The maximum number in the given list of numbers.</returns>
        internal static NumberValue MaxN(NumberValue value1, params NumberValue[] values)
        {
            var result = value1.Value;

            foreach (var value in values)
            {
                if (value.Value > result)
                {
                    result = value.Value;
                }
            }

            return new NumberValue(result);
        }

        /// <summary>
        /// Finds the minimum value in the given list of numbers.
        /// </summary>
        /// <param name="value1">The first, required value to take the minimum of.</param>
        /// <param name="values">An optional list of additional numbers to take the minimum of.</param>
        /// <returns>The minimum number in the given list of numbers.</returns>
        internal static NumberValue MinN(NumberValue value1, params NumberValue[] values)
        {
            var result = value1.Value;

            foreach (var value in values)
            {
                if (value.Value < result)
                {
                    result = value.Value;
                }
            }

            return new NumberValue(result);
        }

        /// <summary>
        /// Returns the closest integer to the given number.
        /// </summary>
        /// <param name="value">The value to get the round.</param>
        /// <returns>The result of rounding the given value.</returns>
        internal static NumberValue Round1(NumberValue value)
        {
            var result = Math.Round(value.Value);
            return new NumberValue(result);
        }

        /// <summary>
        /// Returns the closest number to the given number, preserving the given number of decimal places.
        /// </summary>
        /// <param name="value">The value to get the round.</param>
        /// <param name="decimals">The number of decimal places to preserve.</param>
        /// <returns>The result of rounding the given value.</returns>
        internal static NumberValue Round2(NumberValue value, NumberValue decimals)
        {
            var magnitude = (decimal)Math.Pow(10, (double)decimals.Value);
            var result = Math.Round(value.Value * magnitude) / magnitude;
            return new NumberValue(result);
        }

        /// <summary>
        /// Returns the sine of the given value.
        /// </summary>
        /// <param name="value">An angle, measured in degrees.</param>
        /// <returns>The sine of <see cref="value"/>.</returns>
        internal static NumberValue Sin1(NumberValue value)
        {
            var radians = ((double)value.Value * Math.PI) / 180d;
            var result = (decimal)Math.Sin(radians);
            return new NumberValue(result);
        }

        /// <summary>
        /// Returns the tangent of the given value.
        /// </summary>
        /// <param name="value">An angle, measured in degrees.</param>
        /// <returns>The tangent of <see cref="value"/>.</returns>
        internal static NumberValue Tan1(NumberValue value)
        {
            var radians = ((double)value.Value * Math.PI) / 180d;
            var result = (decimal)Math.Tan(radians);
            return new NumberValue(result);
        }
    }
}