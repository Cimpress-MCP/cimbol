using System;
using System.Collections.Generic;
using System.Reflection;
using Cimpress.Cimbol.Runtime.Types;

namespace Cimpress.Cimbol.Runtime.Functions
{
    /// <summary>
    /// A collection of functions for use during the evaluation of Cimbol programs.
    /// </summary>
    public static class RuntimeFunctions
    {
        /// <summary>
        /// The cached <see cref="ConstructorInfo"/> for the constructor for a <see cref="ListValue"/>.
        /// </summary>
        public static ConstructorInfo ListConstructorInfo { get; } =
            typeof(ListValue).GetConstructor(new[] { typeof(ILocalValue[]) });

        /// <summary>
        /// The cached <see cref="ConstructorInfo"/> for the constructor for a <see cref="NotSupportedException"/>.
        /// </summary>
        public static ConstructorInfo NotSupportedExceptionConstructorInfo { get; } =
            typeof(NotSupportedException).GetConstructor(Array.Empty<Type>());

        /// <summary>
        /// The cached <see cref="ConstructorInfo"/> for the constructor for a <see cref="ListValue"/>.
        /// </summary>
        public static ConstructorInfo ObjectConstructorInfo { get; } =
            typeof(ObjectValue).GetConstructor(new[] { typeof(IDictionary<string, ILocalValue>) });

        /// <summary>
        /// The cached <see cref="ConstructorInfo"/> for the constructor for a <see cref="Dictionary{string,ILocalValue}"/>.
        /// </summary>
        public static ConstructorInfo ObjectDictionaryConstructorInfo { get; } =
            typeof(Dictionary<string, ILocalValue>).GetConstructor(new[] { typeof(IEqualityComparer<string>) });

        /// <summary>
        /// The cached <see cref="MethodInfo"/> for the function for access a member of an object value.
        /// </summary>
        public static MethodInfo AccessInfo { get; } =
            typeof(ILocalValue).GetMethod("Access", BindingFlags.Instance | BindingFlags.Public);

        /// <summary>
        /// The cached <see cref="MethodInfo"/> for the function for casting a value to a boolean.
        /// </summary>
        public static MethodInfo CastBooleanInfo { get; } =
            typeof(ILocalValue).GetMethod("CastBoolean", BindingFlags.Instance | BindingFlags.Public);

        /// <summary>
        /// The cached <see cref="MethodInfo"/> for the function for casting a value to a number.
        /// </summary>
        public static MethodInfo CastNumberInfo { get; } =
            typeof(ILocalValue).GetMethod("CastNumber", BindingFlags.Instance | BindingFlags.Public);

        /// <summary>
        /// The cached <see cref="MethodInfo"/> for the function for casting a value to a string.
        /// </summary>
        public static MethodInfo CastStringInfo { get; } =
            typeof(ILocalValue).GetMethod("CastString", BindingFlags.Instance | BindingFlags.Public);

        /// <summary>
        /// The cached <see cref="MethodInfo"/> for the function for invoke a function value.
        /// </summary>
        public static MethodInfo InvokeInfo { get; } =
            typeof(ILocalValue).GetMethod("Invoke", BindingFlags.Instance | BindingFlags.Public);

        /// <summary>
        /// The cached <see cref="MethodInfo"/> for the function for checking equality.
        /// </summary>
        public static MethodInfo EqualToInfo { get; } =
            typeof(RuntimeFunctions).GetMethod("EqualTo", BindingFlags.Public | BindingFlags.Static);

        /// <summary>
        /// The cached <see cref="MethodInfo"/> for the function for checking inequality.
        /// </summary>
        public static MethodInfo NotEqualToInfo { get; } =
            typeof(RuntimeFunctions).GetMethod("NotEqualTo", BindingFlags.Public | BindingFlags.Static);

        /// <summary>
        /// The cached <see cref="MethodInfo"/> for the function for checking truthfulness.
        /// </summary>
        public static MethodInfo IfTrueInfo { get; } =
            typeof(RuntimeFunctions).GetMethod("IfTrue", BindingFlags.Public | BindingFlags.Static);

        /// <summary>
        /// The cached <see cref="MethodInfo"/> for the and function.
        /// </summary>
        public static MethodInfo BooleanAndInfo { get; } =
            typeof(RuntimeFunctions).GetMethod("And", BindingFlags.Public | BindingFlags.Static);

        /// <summary>
        /// The cached <see cref="MethodInfo"/> for the or function.
        /// </summary>
        public static MethodInfo BooleanOrInfo { get; } =
            typeof(RuntimeFunctions).GetMethod("Or", BindingFlags.Public | BindingFlags.Static);

        /// <summary>
        /// The cached <see cref="MethodInfo"/> for the not function.
        /// </summary>
        public static MethodInfo BooleanNotInfo { get; } =
            typeof(RuntimeFunctions).GetMethod("Not", BindingFlags.Public | BindingFlags.Static);

        /// <summary>
        /// The cached <see cref="MethodInfo"/> for the add function.
        /// </summary>
        public static MethodInfo MathAddInfo { get; } =
            typeof(RuntimeFunctions).GetMethod("Add", BindingFlags.Public | BindingFlags.Static);

        /// <summary>
        /// The cached <see cref="MethodInfo"/> for the greater than function.
        /// </summary>
        public static MethodInfo CompareGreaterThanInfo { get; } =
            typeof(RuntimeFunctions).GetMethod("GreaterThan", BindingFlags.Public | BindingFlags.Static);

        /// <summary>
        /// The cached <see cref="MethodInfo"/> for the greater than or equal function.
        /// </summary>
        public static MethodInfo CompareGreaterThanOrEqualInfo { get; } =
            typeof(RuntimeFunctions).GetMethod("GreaterThanOrEqual", BindingFlags.Public | BindingFlags.Static);

        /// <summary>
        /// The cached <see cref="MethodInfo"/> for the less than function.
        /// </summary>
        public static MethodInfo CompareLessThanInfo { get; } =
            typeof(RuntimeFunctions).GetMethod("LessThan", BindingFlags.Public | BindingFlags.Static);

        /// <summary>
        /// The cached <see cref="MethodInfo"/> for the less than or equal function.
        /// </summary>
        public static MethodInfo CompareLessThanOrEqualInfo { get; } =
            typeof(RuntimeFunctions).GetMethod("LessThanOrEqual", BindingFlags.Public | BindingFlags.Static);

        /// <summary>
        /// The cached <see cref="MethodInfo"/> for the subtract function.
        /// </summary>
        public static MethodInfo MathSubtractInfo { get; } =
            typeof(RuntimeFunctions).GetMethod("Subtract", BindingFlags.Public | BindingFlags.Static);

        /// <summary>
        /// The cached <see cref="MethodInfo"/> for the multiply function.
        /// </summary>
        public static MethodInfo MathMultiplyInfo { get; } =
            typeof(RuntimeFunctions).GetMethod("Multiply", BindingFlags.Public | BindingFlags.Static);

        /// <summary>
        /// The cached <see cref="MethodInfo"/> for the divide function.
        /// </summary>
        public static MethodInfo MathDivideInfo { get; } =
            typeof(RuntimeFunctions).GetMethod("Divide", BindingFlags.Public | BindingFlags.Static);

        /// <summary>
        /// The cached <see cref="MethodInfo"/> for the remainder function.
        /// </summary>
        public static MethodInfo MathRemainderInfo { get; } =
            typeof(RuntimeFunctions).GetMethod("Remainder", BindingFlags.Public | BindingFlags.Static);

        /// <summary>
        /// The cached <see cref="MethodInfo"/> for the power function.
        /// </summary>
        public static MethodInfo MathPowerInfo { get; } =
            typeof(RuntimeFunctions).GetMethod("Power", BindingFlags.Public | BindingFlags.Static);

        /// <summary>
        /// The cached <see cref="MethodInfo"/> for the power function.
        /// </summary>
        public static MethodInfo MathNegateInfo { get; } =
            typeof(RuntimeFunctions).GetMethod("Negate", BindingFlags.Public | BindingFlags.Static);

        /// <summary>
        /// The cached <see cref="MethodInfo"/> for the concatenate function.
        /// </summary>
        public static MethodInfo StringConcatenateInfo { get; } =
            typeof(RuntimeFunctions).GetMethod("Concatenate", BindingFlags.Public | BindingFlags.Static);

        /// <summary>
        /// The cached <see cref="MethodInfo"/> for adding items to a dictionary of local values.
        /// </summary>
        public static MethodInfo ObjectDictionaryAdd { get; } =
            typeof(Dictionary<string, ILocalValue>).GetMethod("Add", BindingFlags.Instance | BindingFlags.Public);

        /// <summary>
        /// Take the logical AND of two booleans.
        /// </summary>
        /// <param name="leftValue">The left boolean to AND.</param>
        /// <param name="rightValue">The right boolean to AND.</param>
        /// <returns>The result of taking the logical AND of the two booleans.</returns>
        public static BooleanValue And(BooleanValue leftValue, BooleanValue rightValue)
        {
#pragma warning disable CA1062
            return new BooleanValue(leftValue.Value && rightValue.Value);
#pragma warning restore CA1062
        }

        /// <summary>
        /// Take the logical OR of two booleans.
        /// </summary>
        /// <param name="leftValue">The left boolean to OR.</param>
        /// <param name="rightValue">The right boolean to OR.</param>
        /// <returns>The result of taking the logical OR of the two booleans.</returns>
        public static BooleanValue Or(BooleanValue leftValue, BooleanValue rightValue)
        {
#pragma warning disable CA1062
            return new BooleanValue(leftValue.Value || rightValue.Value);
#pragma warning restore CA1062
        }

        /// <summary>
        /// Take the logical NOT of a boolean.
        /// </summary>
        /// <param name="value">The boolean to NOT.</param>
        /// <returns>The result of taking the logical NOT of a boolean.</returns>
        public static BooleanValue Not(BooleanValue value)
        {
#pragma warning disable CA1062
            return !value.Value ? BooleanValue.True : BooleanValue.False;
#pragma warning restore CA1062
        }

        /// <summary>
        /// Determine if one number is greater than another number.
        /// </summary>
        /// <param name="leftValue">The left number to compare.</param>
        /// <param name="rightValue">The right number to compare.</param>
        /// <returns>The result of determining if the left value is greater than the right value..</returns>
        public static BooleanValue GreaterThan(NumberValue leftValue, NumberValue rightValue)
        {
#pragma warning disable CA1062
            return leftValue.Value > rightValue.Value ? BooleanValue.True : BooleanValue.False;
#pragma warning restore CA1062
        }

        /// <summary>
        /// Determine if one number is greater than or equal to another number.
        /// </summary>
        /// <param name="leftValue">The left number to compare.</param>
        /// <param name="rightValue">The right number to compare.</param>
        /// <returns>The result of determining if the left value is greater than or equal to the right value..</returns>
        public static BooleanValue GreaterThanOrEqual(NumberValue leftValue, NumberValue rightValue)
        {
#pragma warning disable CA1062
            return leftValue.Value >= rightValue.Value ? BooleanValue.True : BooleanValue.False;
#pragma warning restore CA1062
        }

        /// <summary>
        /// Determine if one number is less than another number.
        /// </summary>
        /// <param name="leftValue">The left number to compare.</param>
        /// <param name="rightValue">The right number to compare.</param>
        /// <returns>The result of determining if the left value is less than the right value..</returns>
        public static BooleanValue LessThan(NumberValue leftValue, NumberValue rightValue)
        {
#pragma warning disable CA1062
            return leftValue.Value < rightValue.Value ? BooleanValue.True : BooleanValue.False;
#pragma warning restore CA1062
        }

        /// <summary>
        /// Determine if one number is less than or equal to another number.
        /// </summary>
        /// <param name="leftValue">The left number to compare.</param>
        /// <param name="rightValue">The right number to compare.</param>
        /// <returns>The result of determining if the left value is less than or equal to the right value..</returns>
        public static BooleanValue LessThanOrEqual(NumberValue leftValue, NumberValue rightValue)
        {
#pragma warning disable CA1062
            return leftValue.Value <= rightValue.Value ? BooleanValue.True : BooleanValue.False;
#pragma warning restore CA1062
        }

        /// <summary>
        /// Add two numbers together.
        /// </summary>
        /// <param name="leftValue">The left number to add.</param>
        /// <param name="rightValue">The right number to add.</param>
        /// <returns>The result of adding the left and right numbers.</returns>
        public static NumberValue Add(NumberValue leftValue, NumberValue rightValue)
        {
#pragma warning disable CA1062
            return new NumberValue(leftValue.Value + rightValue.Value);
#pragma warning restore CA1062
        }

        /// <summary>
        /// Subtract one number from another number.
        /// </summary>
        /// <param name="leftValue">The left number to subtract from.</param>
        /// <param name="rightValue">The right number to subtract with.</param>
        /// <returns>The result of subtracting the right number from the left number.</returns>
        public static NumberValue Subtract(NumberValue leftValue, NumberValue rightValue)
        {
#pragma warning disable CA1062
            return new NumberValue(leftValue.Value - rightValue.Value);
#pragma warning restore CA1062
        }

        /// <summary>
        /// Multiply two numbers together.
        /// </summary>
        /// <param name="leftValue">The left number to multiply.</param>
        /// <param name="rightValue">The right number to multiply.</param>
        /// <returns>The result of multiplying the left and right numbers.</returns>
        public static NumberValue Multiply(NumberValue leftValue, NumberValue rightValue)
        {
#pragma warning disable CA1062
            return new NumberValue(leftValue.Value * rightValue.Value);
#pragma warning restore CA1062
        }

        /// <summary>
        /// Divide one number into another number.
        /// </summary>
        /// <param name="leftValue">The left number to divide from.</param>
        /// <param name="rightValue">The right number to divide with.</param>
        /// <returns>The result of dividing the left number with the right number.</returns>
        public static NumberValue Divide(NumberValue leftValue, NumberValue rightValue)
        {
#pragma warning disable CA1062
            return new NumberValue(leftValue.Value / rightValue.Value);
#pragma warning restore CA1062
        }

        /// <summary>
        /// Take the remainder from dividing one number into another number.
        /// </summary>
        /// <param name="leftValue">The left number to divide from.</param>
        /// <param name="rightValue">The right number to divide with.</param>
        /// <returns>The result of taking the remainder from the dividing the right number into the left number.</returns>
        public static NumberValue Remainder(NumberValue leftValue, NumberValue rightValue)
        {
#pragma warning disable CA1062
            return new NumberValue(leftValue.Value % rightValue.Value);
#pragma warning restore CA1062
        }

        /// <summary>
        /// Raise one number to the power of another number.
        /// </summary>
        /// <param name="leftValue">The number to raise to a power.</param>
        /// <param name="rightValue">The number that is the power.</param>
        /// <returns>The result of raising the left number to power of the right number.</returns>
        public static NumberValue Power(NumberValue leftValue, NumberValue rightValue)
        {
#pragma warning disable CA1062
            return new NumberValue((decimal)Math.Pow((double)leftValue.Value, (double)rightValue.Value));
#pragma warning restore CA1062
        }

        /// <summary>
        /// Negate a number.
        /// </summary>
        /// <param name="value">The number to negate.</param>
        /// <returns>The resulting of negating the number.</returns>
        public static NumberValue Negate(NumberValue value)
        {
#pragma warning disable CA1062
            return new NumberValue(-value.Value);
#pragma warning restore CA1062
        }

        /// <summary>
        /// Concatenate two strings together.
        /// </summary>
        /// <param name="leftValue">The left string to concatenate.</param>
        /// <param name="rightValue">The right string to concatenate.</param>
        /// <returns>The result of concatenating the left and right strings.</returns>
        public static StringValue Concatenate(StringValue leftValue, StringValue rightValue)
        {
#pragma warning disable CA1062
            return new StringValue(leftValue.Value + rightValue.Value);
#pragma warning restore CA1062
        }

        /// <summary>
        /// Check if a value is truthful.
        /// </summary>
        /// <param name="value">The value to check for truthfulness.</param>
        /// <returns>The result of checking if the value is truthful.</returns>
        public static bool IfTrue(ILocalValue value)
        {
#pragma warning disable CA1062
            return value.CastBoolean().Value;
#pragma warning restore CA1062
        }

        /// <summary>
        /// Check if two values are equal.
        /// </summary>
        /// <param name="leftValue">The left string to compare.</param>
        /// <param name="rightValue">The right string to compare.</param>
        /// <returns>The result of checking if the left and right values are equal.</returns>
        public static BooleanValue EqualTo(ILocalValue leftValue, ILocalValue rightValue)
        {
#pragma warning disable CA1062
            return leftValue.EqualTo(rightValue) ? BooleanValue.True : BooleanValue.False;
#pragma warning restore CA1062
        }

        /// <summary>
        /// Check if two values are not equal.
        /// </summary>
        /// <param name="leftValue">The left string to compare.</param>
        /// <param name="rightValue">The right string to compare.</param>
        /// <returns>The result of checking if the left and right values are not equal.</returns>
        public static BooleanValue NotEqualTo(ILocalValue leftValue, ILocalValue rightValue)
        {
#pragma warning disable CA1062
            return leftValue.EqualTo(rightValue) ? BooleanValue.False : BooleanValue.True;
#pragma warning restore CA1062
        }

        /// <summary>
        /// Check if two booleans are equal.
        /// </summary>
        /// <param name="leftValue">The left boolean to compare.</param>
        /// <param name="rightValue">The right boolean to compare.</param>
        /// <returns>The result of checking if the left and right booleans are equal.</returns>
        public static bool InnerEqualTo(BooleanValue leftValue, BooleanValue rightValue)
        {
#pragma warning disable CA1062
            return leftValue.Value == rightValue.Value;
#pragma warning restore CA1062
        }

        /// <summary>
        /// Check if two numbers are equal.
        /// </summary>
        /// <param name="leftValue">The left number to compare.</param>
        /// <param name="rightValue">The right number to compare.</param>
        /// <returns>The result of checking if the left and right numbers are equal.</returns>
        public static bool InnerEqualTo(NumberValue leftValue, NumberValue rightValue)
        {
#pragma warning disable CA1062
            return leftValue.Value == rightValue.Value;
#pragma warning restore CA1062
        }

        /// <summary>
        /// Check if a number and a string are equal.
        /// </summary>
        /// <param name="leftValue">The left number to compare.</param>
        /// <param name="rightValue">The right string to compare.</param>
        /// <returns>The result of checking if the left number and right string are equal.</returns>
        public static bool InnerEqualTo(NumberValue leftValue, StringValue rightValue)
        {
#pragma warning disable CA1062
            if (!decimal.TryParse(rightValue.Value, out var result))
            {
                return false;
            }

            return leftValue.Value == result;
#pragma warning restore CA1062
        }

        /// <summary>
        /// Check if two strings are equal.
        /// </summary>
        /// <param name="leftValue">The left string to compare.</param>
        /// <param name="rightValue">The right string to compare.</param>
        /// <returns>The result of checking if the left and right strings are equal.</returns>
        public static bool InnerEqualTo(StringValue leftValue, StringValue rightValue)
        {
#pragma warning disable CA1062
            return leftValue.Value.Equals(rightValue.Value, StringComparison.OrdinalIgnoreCase);
#pragma warning restore CA1062
        }
    }
}