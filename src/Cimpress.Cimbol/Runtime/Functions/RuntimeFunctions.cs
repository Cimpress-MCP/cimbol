using System;
using System.Reflection;
using Cimpress.Cimbol.Exceptions;
using Cimpress.Cimbol.Runtime.Types;

namespace Cimpress.Cimbol.Runtime.Functions
{
    /// <summary>
    /// A collection of functions for use during the evaluation of Cimbol programs.
    /// </summary>
    internal static class RuntimeFunctions
    {
        /// <summary>
        /// The cached <see cref="MethodInfo"/> for the add function.
        /// </summary>
        internal static MethodInfo MathAddInfo { get; } =
            typeof(RuntimeFunctions).GetMethod("Add", BindingFlags.NonPublic | BindingFlags.Static);

        /// <summary>
        /// The cached <see cref="MethodInfo"/> for the and function.
        /// </summary>
        internal static MethodInfo BooleanAndInfo { get; } =
            typeof(RuntimeFunctions).GetMethod("And", BindingFlags.NonPublic | BindingFlags.Static);

        /// <summary>
        /// The cached <see cref="MethodInfo"/> for the concatenate function.
        /// </summary>
        internal static MethodInfo StringConcatenateInfo { get; } =
            typeof(RuntimeFunctions).GetMethod("Concatenate", BindingFlags.NonPublic | BindingFlags.Static);

        /// <summary>
        /// The cached <see cref="MethodInfo"/> for the default function.
        /// </summary>
        internal static MethodInfo DefaultInfo { get; } =
            typeof(RuntimeFunctions).GetMethod("Default", BindingFlags.NonPublic | BindingFlags.Static);

        /// <summary>
        /// The cached <see cref="MethodInfo"/> for the exists function.
        /// </summary>
        internal static MethodInfo ExistsInfo { get; } =
            typeof(RuntimeFunctions).GetMethod("Exists", BindingFlags.NonPublic | BindingFlags.Static);

        /// <summary>
        /// The cached <see cref="MethodInfo"/> for the divide function.
        /// </summary>
        internal static MethodInfo MathDivideInfo { get; } =
            typeof(RuntimeFunctions).GetMethod("Divide", BindingFlags.NonPublic | BindingFlags.Static);

        /// <summary>
        /// The cached <see cref="MethodInfo"/> for the function for checking equality.
        /// </summary>
        internal static MethodInfo EqualToInfo { get; } =
            typeof(RuntimeFunctions).GetMethod("EqualTo", BindingFlags.NonPublic | BindingFlags.Static);

        /// <summary>
        /// The cached <see cref="MethodInfo"/> for the greater than function.
        /// </summary>
        internal static MethodInfo CompareGreaterThanInfo { get; } =
            typeof(RuntimeFunctions).GetMethod("GreaterThan", BindingFlags.NonPublic | BindingFlags.Static);

        /// <summary>
        /// The cached <see cref="MethodInfo"/> for the greater than or equal function.
        /// </summary>
        internal static MethodInfo CompareGreaterThanOrEqualInfo { get; } =
            typeof(RuntimeFunctions).GetMethod("GreaterThanOrEqual", BindingFlags.NonPublic | BindingFlags.Static);

        /// <summary>
        /// The cached <see cref="MethodInfo"/> for the function for checking truthfulness.
        /// </summary>
        internal static MethodInfo IfTrueInfo { get; } =
            typeof(RuntimeFunctions).GetMethod("IfTrue", BindingFlags.NonPublic | BindingFlags.Static);

        /// <summary>
        /// The cached <see cref="MethodInfo"/> for the less than function.
        /// </summary>
        internal static MethodInfo CompareLessThanInfo { get; } =
            typeof(RuntimeFunctions).GetMethod("LessThan", BindingFlags.NonPublic | BindingFlags.Static);

        /// <summary>
        /// The cached <see cref="MethodInfo"/> for the less than or equal function.
        /// </summary>
        internal static MethodInfo CompareLessThanOrEqualInfo { get; } =
            typeof(RuntimeFunctions).GetMethod("LessThanOrEqual", BindingFlags.NonPublic | BindingFlags.Static);

        /// <summary>
        /// The cached <see cref="MethodInfo"/> for the multiply function.
        /// </summary>
        internal static MethodInfo MathMultiplyInfo { get; } =
            typeof(RuntimeFunctions).GetMethod("Multiply", BindingFlags.NonPublic | BindingFlags.Static);

        /// <summary>
        /// The cached <see cref="MethodInfo"/> for the power function.
        /// </summary>
        internal static MethodInfo MathNegateInfo { get; } =
            typeof(RuntimeFunctions).GetMethod("Negate", BindingFlags.NonPublic | BindingFlags.Static);

        /// <summary>
        /// The cached <see cref="MethodInfo"/> for the not function.
        /// </summary>
        internal static MethodInfo BooleanNotInfo { get; } =
            typeof(RuntimeFunctions).GetMethod("Not", BindingFlags.NonPublic | BindingFlags.Static);

        /// <summary>
        /// The cached <see cref="MethodInfo"/> for the function for checking inequality.
        /// </summary>
        internal static MethodInfo NotEqualToInfo { get; } =
            typeof(RuntimeFunctions).GetMethod("NotEqualTo", BindingFlags.NonPublic | BindingFlags.Static);

        /// <summary>
        /// The cached <see cref="MethodInfo"/> for the or function.
        /// </summary>
        internal static MethodInfo BooleanOrInfo { get; } =
            typeof(RuntimeFunctions).GetMethod("Or", BindingFlags.NonPublic | BindingFlags.Static);

        /// <summary>
        /// The cached <see cref="MethodInfo"/> for the power function.
        /// </summary>
        internal static MethodInfo MathPowerInfo { get; } =
            typeof(RuntimeFunctions).GetMethod("Power", BindingFlags.NonPublic | BindingFlags.Static);

        /// <summary>
        /// The cached <see cref="MethodInfo"/> for the remainder function.
        /// </summary>
        internal static MethodInfo MathRemainderInfo { get; } =
            typeof(RuntimeFunctions).GetMethod("Remainder", BindingFlags.NonPublic | BindingFlags.Static);

        /// <summary>
        /// The cached <see cref="MethodInfo"/> for the subtract function.
        /// </summary>
        internal static MethodInfo MathSubtractInfo { get; } =
            typeof(RuntimeFunctions).GetMethod("Subtract", BindingFlags.NonPublic | BindingFlags.Static);

        /// <summary>
        /// Add two numbers together.
        /// </summary>
        /// <param name="leftValue">The left number to add.</param>
        /// <param name="rightValue">The right number to add.</param>
        /// <returns>The result of adding the left and right numbers.</returns>
        internal static NumberValue Add(NumberValue leftValue, NumberValue rightValue)
        {
            return new NumberValue(leftValue.Value + rightValue.Value);
        }

        /// <summary>
        /// Take the logical AND of two booleans.
        /// </summary>
        /// <param name="leftValue">The left boolean to AND.</param>
        /// <param name="rightValue">The right boolean to AND.</param>
        /// <returns>The result of taking the logical AND of the two booleans.</returns>
        internal static BooleanValue And(BooleanValue leftValue, BooleanValue rightValue)
        {
            return new BooleanValue(leftValue.Value && rightValue.Value);
        }

        /// <summary>
        /// Concatenate two strings together.
        /// </summary>
        /// <param name="leftValue">The left string to concatenate.</param>
        /// <param name="rightValue">The right string to concatenate.</param>
        /// <returns>The result of concatenating the left and right strings.</returns>
        internal static StringValue Concatenate(StringValue leftValue, StringValue rightValue)
        {
            return new StringValue(leftValue.Value + rightValue.Value);
        }

        /// <summary>
        /// Default a value to another value if it does not exist.
        /// </summary>
        /// <param name="baseValue">The value to check.</param>
        /// <param name="fallbackValue">The value to use if the prior value is invalid.</param>
        /// <param name="path">The path of members to access.</param>
        /// <returns>Returns the value if it exists, and the fallback value otherwise.</returns>
        internal static ILocalValue Default(ILocalValue baseValue, ILocalValue fallbackValue, string[] path)
        {
            if (baseValue == null)
            {
                return fallbackValue;
            }

            var currentValue = baseValue;

            foreach (var identifier in path)
            {
                if (currentValue != null && currentValue is ObjectValue currentObjectValue)
                {
                    currentObjectValue.Value.TryGetValue(identifier, out currentValue);
                }
                else
                {
                    return fallbackValue;
                }
            }

            return currentValue ?? fallbackValue;
        }

        /// <summary>
        /// Divide one number into another number.
        /// </summary>
        /// <param name="leftValue">The left number to divide from.</param>
        /// <param name="rightValue">The right number to divide with.</param>
        /// <returns>The result of dividing the left number with the right number.</returns>
        internal static NumberValue Divide(NumberValue leftValue, NumberValue rightValue)
        {
            if (rightValue.Value != 0)
            {
                return new NumberValue(leftValue.Value / rightValue.Value);
            }

            throw CimbolRuntimeException.DivideByZeroError();
        }

        /// <summary>
        /// Check if two values are equal.
        /// </summary>
        /// <param name="leftValue">The left string to compare.</param>
        /// <param name="rightValue">The right string to compare.</param>
        /// <returns>The result of checking if the left and right values are equal.</returns>
        internal static BooleanValue EqualTo(ILocalValue leftValue, ILocalValue rightValue)
        {
            return leftValue.EqualTo(rightValue) ? BooleanValue.True : BooleanValue.False;
        }

        /// <summary>
        /// Check whether a given value exists.
        /// </summary>
        /// <param name="baseValue">The base value.</param>
        /// <param name="path">The path.</param>
        /// <returns>A boolean indicating whether or not the value exists.</returns>
        internal static BooleanValue Exists(ILocalValue baseValue, string[] path)
        {
            var currentValue = baseValue;

            foreach (var identifier in path)
            {
                if (currentValue != null && currentValue is ObjectValue currentObjectValue)
                {
                    currentObjectValue.Value.TryGetValue(identifier, out currentValue);
                }
                else
                {
                    return BooleanValue.False;
                }
            }

            return currentValue != null ? BooleanValue.True : BooleanValue.False;
        }

        /// <summary>
        /// Determine if one number is greater than another number.
        /// </summary>
        /// <param name="leftValue">The left number to compare.</param>
        /// <param name="rightValue">The right number to compare.</param>
        /// <returns>The result of determining if the left value is greater than the right value..</returns>
        internal static BooleanValue GreaterThan(NumberValue leftValue, NumberValue rightValue)
        {
            return leftValue.Value > rightValue.Value ? BooleanValue.True : BooleanValue.False;
        }

        /// <summary>
        /// Determine if one number is greater than or equal to another number.
        /// </summary>
        /// <param name="leftValue">The left number to compare.</param>
        /// <param name="rightValue">The right number to compare.</param>
        /// <returns>The result of determining if the left value is greater than or equal to the right value..</returns>
        internal static BooleanValue GreaterThanOrEqual(NumberValue leftValue, NumberValue rightValue)
        {
            return leftValue.Value >= rightValue.Value ? BooleanValue.True : BooleanValue.False;
        }

        /// <summary>
        /// Check if a value is truthful.
        /// </summary>
        /// <param name="value">The value to check for truthfulness.</param>
        /// <returns>The result of checking if the value is truthful.</returns>
        internal static bool IfTrue(ILocalValue value)
        {
            return value.CastBoolean().Value;
        }

        /// <summary>
        /// Check if two booleans are equal.
        /// </summary>
        /// <param name="leftValue">The left boolean to compare.</param>
        /// <param name="rightValue">The right boolean to compare.</param>
        /// <returns>The result of checking if the left and right booleans are equal.</returns>
        internal static bool InnerEqualTo(BooleanValue leftValue, BooleanValue rightValue)
        {
            return leftValue.Value == rightValue.Value;
        }

        /// <summary>
        /// Check if two numbers are equal.
        /// </summary>
        /// <param name="leftValue">The left number to compare.</param>
        /// <param name="rightValue">The right number to compare.</param>
        /// <returns>The result of checking if the left and right numbers are equal.</returns>
        internal static bool InnerEqualTo(NumberValue leftValue, NumberValue rightValue)
        {
            return leftValue.Value == rightValue.Value;
        }

        /// <summary>
        /// Check if a number and a string are equal.
        /// </summary>
        /// <param name="leftValue">The left number to compare.</param>
        /// <param name="rightValue">The right string to compare.</param>
        /// <returns>The result of checking if the left number and right string are equal.</returns>
        internal static bool InnerEqualTo(NumberValue leftValue, StringValue rightValue)
        {
            if (!decimal.TryParse(rightValue.Value, out var result))
            {
                return false;
            }

            return leftValue.Value == result;
        }

        /// <summary>
        /// Check if two strings are equal.
        /// </summary>
        /// <param name="leftValue">The left string to compare.</param>
        /// <param name="rightValue">The right string to compare.</param>
        /// <returns>The result of checking if the left and right strings are equal.</returns>
        internal static bool InnerEqualTo(StringValue leftValue, StringValue rightValue)
        {
            return leftValue.Value.Equals(rightValue.Value, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Determine if one number is less than another number.
        /// </summary>
        /// <param name="leftValue">The left number to compare.</param>
        /// <param name="rightValue">The right number to compare.</param>
        /// <returns>The result of determining if the left value is less than the right value..</returns>
        internal static BooleanValue LessThan(NumberValue leftValue, NumberValue rightValue)
        {
            return leftValue.Value < rightValue.Value ? BooleanValue.True : BooleanValue.False;
        }

        /// <summary>
        /// Determine if one number is less than or equal to another number.
        /// </summary>
        /// <param name="leftValue">The left number to compare.</param>
        /// <param name="rightValue">The right number to compare.</param>
        /// <returns>The result of determining if the left value is less than or equal to the right value..</returns>
        internal static BooleanValue LessThanOrEqual(NumberValue leftValue, NumberValue rightValue)
        {
            return leftValue.Value <= rightValue.Value ? BooleanValue.True : BooleanValue.False;
        }

        /// <summary>
        /// Multiply two numbers together.
        /// </summary>
        /// <param name="leftValue">The left number to multiply.</param>
        /// <param name="rightValue">The right number to multiply.</param>
        /// <returns>The result of multiplying the left and right numbers.</returns>
        internal static NumberValue Multiply(NumberValue leftValue, NumberValue rightValue)
        {
            return new NumberValue(leftValue.Value * rightValue.Value);
        }

        /// <summary>
        /// Negate a number.
        /// </summary>
        /// <param name="value">The number to negate.</param>
        /// <returns>The resulting of negating the number.</returns>
        internal static NumberValue Negate(NumberValue value)
        {
            return new NumberValue(-value.Value);
        }

        /// <summary>
        /// Take the logical NOT of a boolean.
        /// </summary>
        /// <param name="value">The boolean to NOT.</param>
        /// <returns>The result of taking the logical NOT of a boolean.</returns>
        internal static BooleanValue Not(BooleanValue value)
        {
            return !value.Value ? BooleanValue.True : BooleanValue.False;
        }

        /// <summary>
        /// Check if two values are not equal.
        /// </summary>
        /// <param name="leftValue">The left string to compare.</param>
        /// <param name="rightValue">The right string to compare.</param>
        /// <returns>The result of checking if the left and right values are not equal.</returns>
        internal static BooleanValue NotEqualTo(ILocalValue leftValue, ILocalValue rightValue)
        {
            return leftValue.EqualTo(rightValue) ? BooleanValue.False : BooleanValue.True;
        }

        /// <summary>
        /// Take the logical OR of two booleans.
        /// </summary>
        /// <param name="leftValue">The left boolean to OR.</param>
        /// <param name="rightValue">The right boolean to OR.</param>
        /// <returns>The result of taking the logical OR of the two booleans.</returns>
        internal static BooleanValue Or(BooleanValue leftValue, BooleanValue rightValue)
        {
            return new BooleanValue(leftValue.Value || rightValue.Value);
        }

        /// <summary>
        /// Raise one number to the power of another number.
        /// </summary>
        /// <param name="leftValue">The number to raise to a power.</param>
        /// <param name="rightValue">The number that is the power.</param>
        /// <returns>The result of raising the left number to power of the right number.</returns>
        internal static NumberValue Power(NumberValue leftValue, NumberValue rightValue)
        {
            return new NumberValue((decimal)Math.Pow((double)leftValue.Value, (double)rightValue.Value));
        }

        /// <summary>
        /// Take the remainder from dividing one number into another number.
        /// </summary>
        /// <param name="leftValue">The left number to divide from.</param>
        /// <param name="rightValue">The right number to divide with.</param>
        /// <returns>The result of taking the remainder from the dividing the right number into the left number.</returns>
        internal static NumberValue Remainder(NumberValue leftValue, NumberValue rightValue)
        {
            return new NumberValue(leftValue.Value % rightValue.Value);
        }

        /// <summary>
        /// Subtract one number from another number.
        /// </summary>
        /// <param name="leftValue">The left number to subtract from.</param>
        /// <param name="rightValue">The right number to subtract with.</param>
        /// <returns>The result of subtracting the right number from the left number.</returns>
        internal static NumberValue Subtract(NumberValue leftValue, NumberValue rightValue)
        {
            return new NumberValue(leftValue.Value - rightValue.Value);
        }
    }
}