using System;
using System.Collections.Generic;
using System.Reflection;
using Cimpress.Cimbol.Runtime.Functions;
using Cimpress.Cimbol.Runtime.Types;
using NUnit.Framework;

namespace Cimpress.Cimbol.UnitTests.Runtime.Functions
{
    [TestFixture]
    public class RuntimeFunctionsTests
    {
        [Test]
        [TestCaseSource(typeof(RuntimeFunctionsTests), nameof(ConstructorInfoTestCases))]
        public void Should_HaveNonNullConstructorInfos_When_Accessed(ConstructorInfo constructorInfo)
        {
            Assert.That(constructorInfo, Is.Not.Null);
        }

        [Test]
        [TestCaseSource(typeof(RuntimeFunctionsTests), nameof(MethodInfoTestCases))]
        public void Should_HaveNonNullMethodInfos_When_Accessed(MethodInfo constructorInfo)
        {
            Assert.That(constructorInfo, Is.Not.Null);
        }

        [Test]
        [TestCaseSource(typeof(RuntimeFunctionsTests), nameof(EqualToTestCases))]
        public void Should_CheckEquality_When_GivenLocalValues(ILocalValue left, ILocalValue right, bool expected)
        {
            var result = RuntimeFunctions.EqualTo(left, right);

            Assert.That(result.Value, Is.EqualTo(expected));
        }

        [Test]
        [TestCaseSource(typeof(RuntimeFunctionsTests), nameof(EqualToTestCases))]
        public void Should_CheckInequality_When_GivenLocalValues(ILocalValue left, ILocalValue right, bool expected)
        {
            var result = RuntimeFunctions.NotEqualTo(left, right);

            Assert.That(result.Value, Is.EqualTo(!expected));
        }

        [Test]
        [TestCase(true, true)]
        [TestCase(false, false)]
        public void Should_ReturnIfTrue_When_GivenBoolean(bool operand, bool expected)
        {
            var value = new BooleanValue(operand);

            var result = RuntimeFunctions.IfTrue(value);

            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        [TestCase(false, false, false)]
        [TestCase(false, true, false)]
        [TestCase(true, false, false)]
        [TestCase(true, true, true)]
        public void Should_GetLogicalAnd_When_GivenTwoBooleans(bool left, bool right, bool expected)
        {
            var leftValue = new BooleanValue(left);
            var rightValue = new BooleanValue(right);

            var result = RuntimeFunctions.And(leftValue, rightValue);

            Assert.That(result.Value, Is.EqualTo(expected));
        }

        [Test]
        [TestCase(false, true)]
        [TestCase(true, false)]
        public void Should_GetLogicalNot_When_GivenBoolean(bool operand, bool expected)
        {
            var value = new BooleanValue(operand);

            var result = RuntimeFunctions.Not(value);

            Assert.That(result.Value, Is.EqualTo(expected));
        }

        [Test]
        [TestCase(false, false, false)]
        [TestCase(false, true, true)]
        [TestCase(true, false, true)]
        [TestCase(true, true, true)]
        public void Should_GetLogicalOr_When_GivenTwoBooleans(bool left, bool right, bool expected)
        {
            var leftValue = new BooleanValue(left);
            var rightValue = new BooleanValue(right);

            var result = RuntimeFunctions.Or(leftValue, rightValue);

            Assert.That(result.Value, Is.EqualTo(expected));
        }

        [Test]
        [TestCase(2, 1, true)]
        [TestCase(2, 2, false)]
        [TestCase(2, 3, false)]
        public void Should_GetGreaterThan_When_GivenTwoNumbers(decimal left, decimal right, bool expected)
        {
            var leftValue = new NumberValue(left);
            var rightValue = new NumberValue(right);

            var result = RuntimeFunctions.GreaterThan(leftValue, rightValue);

            Assert.That(result.Value, Is.EqualTo(expected));
        }

        [Test]
        [TestCase(2, 1, true)]
        [TestCase(2, 2, true)]
        [TestCase(2, 3, false)]
        public void Should_GetGreaterThanOrEqual_When_GivenTwoNumbers(decimal left, decimal right, bool expected)
        {
            var leftValue = new NumberValue(left);
            var rightValue = new NumberValue(right);

            var result = RuntimeFunctions.GreaterThanOrEqual(leftValue, rightValue);

            Assert.That(result.Value, Is.EqualTo(expected));
        }

        [Test]
        [TestCase(2, 1, false)]
        [TestCase(2, 2, false)]
        [TestCase(2, 3, true)]
        public void Should_GetLessThan_When_GivenTwoNumbers(decimal left, decimal right, bool expected)
        {
            var leftValue = new NumberValue(left);
            var rightValue = new NumberValue(right);

            var result = RuntimeFunctions.LessThan(leftValue, rightValue);

            Assert.That(result.Value, Is.EqualTo(expected));
        }

        [Test]
        [TestCase(2, 1, false)]
        [TestCase(2, 2, true)]
        [TestCase(2, 3, true)]
        public void Should_GetLessThanOrEqual_When_GivenTwoNumbers(decimal left, decimal right, bool expected)
        {
            var leftValue = new NumberValue(left);
            var rightValue = new NumberValue(right);

            var result = RuntimeFunctions.LessThanOrEqual(leftValue, rightValue);

            Assert.That(result.Value, Is.EqualTo(expected));
        }

        [Test]
        [TestCase(3, 2, 5)]
        public void Should_Add_When_GivenTwoNumbers(decimal left, decimal right, decimal expected)
        {
            var leftValue = new NumberValue(left);
            var rightValue = new NumberValue(right);

            var result = RuntimeFunctions.Add(leftValue, rightValue);

            Assert.That(result.Value, Is.EqualTo(expected));
        }

        [Test]
        [TestCase(3, 2, 1)]
        public void Should_Subtract_When_GivenTwoNumbers(decimal left, decimal right, decimal expected)
        {
            var leftValue = new NumberValue(left);
            var rightValue = new NumberValue(right);

            var result = RuntimeFunctions.Subtract(leftValue, rightValue);

            Assert.That(result.Value, Is.EqualTo(expected));
        }

        [Test]
        [TestCase(3, 2, 6)]
        public void Should_Multiply_When_GivenTwoNumbers(decimal left, decimal right, decimal expected)
        {
            var leftValue = new NumberValue(left);
            var rightValue = new NumberValue(right);

            var result = RuntimeFunctions.Multiply(leftValue, rightValue);

            Assert.That(result.Value, Is.EqualTo(expected));
        }

        [Test]
        [TestCase(3, 2, 1.5)]
        public void Should_Divide_When_GivenTwoNumbers(decimal left, decimal right, decimal expected)
        {
            var leftValue = new NumberValue(left);
            var rightValue = new NumberValue(right);

            var result = RuntimeFunctions.Divide(leftValue, rightValue);

            Assert.That(result.Value, Is.EqualTo(expected));
        }

        [Test]
        [TestCase(3, 2, 1)]
        [TestCase(-3, 2, -1)]
        [TestCase(3, -2, 1)]
        [TestCase(-3, -2, -1)]
        [TestCase(2.5, 2, 0.5)]
        [TestCase(4, 2.5, 1.5)]
        [TestCase(2, 3, 2)]
        public void Should_GetRemainder_When_GivenTwoNumbers(decimal left, decimal right, decimal expected)
        {
            var leftValue = new NumberValue(left);
            var rightValue = new NumberValue(right);

            var result = RuntimeFunctions.Remainder(leftValue, rightValue);

            Assert.That(result.Value, Is.EqualTo(expected));
        }

        [Test]
        [TestCase(3, 2, 9)]
        [TestCase(3, 1, 3)]
        [TestCase(3, 0, 1)]
        [TestCase(4, 0.5, 2)]
        [TestCase(4, -1, 0.25)]
        [TestCase(4, -0.5, 0.5)]
        public void Should_GetPower_When_GivenTwoNumbers(decimal left, decimal right, decimal expected)
        {
            var leftValue = new NumberValue(left);
            var rightValue = new NumberValue(right);

            var result = RuntimeFunctions.Power(leftValue, rightValue);

            Assert.That(result.Value, Is.EqualTo(expected));
        }

        [Test]
        [TestCase(1, -1)]
        [TestCase(-1, 1)]
        public void Should_Negate_When_GivenNumber(decimal operand, decimal expected)
        {
            var value = new NumberValue(operand);

            var result = RuntimeFunctions.Negate(value);

            Assert.That(result.Value, Is.EqualTo(expected));
        }

        [Test]
        [TestCase("cat ", "dog", "cat dog")]
        [TestCase("cat", "", "cat")]
        [TestCase("", "dog", "dog")]
        public void Should_Concatenate_When_GivenTwoStrings(string left, string right, string expected)
        {
            var leftValue = new StringValue(left);
            var rightValue = new StringValue(right);

            var result = RuntimeFunctions.Concatenate(leftValue, rightValue);

            Assert.That(result.Value, Is.EqualTo(expected));
        }

        [Test]
        [TestCase(true, true, true)]
        [TestCase(true, false, false)]
        [TestCase(false, true, false)]
        [TestCase(false, false, true)]
        public void Should_GetEquality_When_DoingInnerEqualToOnStrings(bool left, bool right, bool expected)
        {
            var leftValue = new BooleanValue(left);
            var rightValue = new BooleanValue(right);

            var result = RuntimeFunctions.InnerEqualTo(leftValue, rightValue);

            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        [TestCase(1, 1, true)]
        [TestCase(1, 2, false)]
        public void Should_GetEquality_When_DoingInnerEqualToOnNumbers(decimal left, decimal right, bool expected)
        {
            var leftValue = new NumberValue(left);
            var rightValue = new NumberValue(right);

            var result = RuntimeFunctions.InnerEqualTo(leftValue, rightValue);

            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        [TestCase(1, "1", true)]
        [TestCase(1, "2", false)]
        [TestCase(1, "1.0", true)]
        [TestCase(1, "2.0", false)]
        [TestCase(1, "cat", false)]
        public void Should_GetEquality_When_DoingInnerEqualToOnNumberAndString(decimal left, string right, bool expected)
        {
            var leftValue = new NumberValue(left);
            var rightValue = new StringValue(right);

            var result = RuntimeFunctions.InnerEqualTo(leftValue, rightValue);

            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        [TestCase("cat", "cat", true)]
        [TestCase("cat", "CAT", true)]
        [TestCase("cat", "dog", false)]
        [TestCase("cat", "DOG", false)]
        [TestCase("1", "1", true)]
        [TestCase("1", "1.0", false)]
        public void Should_GetEquality_When_DoingInnerEqualToOnStrings(string left, string right, bool expected)
        {
            var leftValue = new StringValue(left);
            var rightValue = new StringValue(right);

            var result = RuntimeFunctions.InnerEqualTo(leftValue, rightValue);

            Assert.That(result, Is.EqualTo(expected));
        }

        private static IEnumerable<TestCaseData> ConstructorInfoTestCases()
        {
            yield return new TestCaseData(LocalValueFunctions.ListValueConstructorInfo);
            yield return new TestCaseData(StandardFunctions.NotSupportedExceptionConstructorInfo);
            yield return new TestCaseData(LocalValueFunctions.ObjectValueConstructorInfo);
            yield return new TestCaseData(StandardFunctions.DictionaryConstructorInfo);
        }

        private static IEnumerable<TestCaseData> MethodInfoTestCases()
        {
            yield return new TestCaseData(LocalValueFunctions.AccessInfo);
            yield return new TestCaseData(RuntimeFunctions.BooleanAndInfo);
            yield return new TestCaseData(RuntimeFunctions.BooleanNotInfo);
            yield return new TestCaseData(RuntimeFunctions.BooleanOrInfo);
            yield return new TestCaseData(LocalValueFunctions.CastBooleanInfo);
            yield return new TestCaseData(LocalValueFunctions.CastNumberInfo);
            yield return new TestCaseData(LocalValueFunctions.CastStringInfo);
            yield return new TestCaseData(RuntimeFunctions.CompareGreaterThanInfo);
            yield return new TestCaseData(RuntimeFunctions.CompareGreaterThanOrEqualInfo);
            yield return new TestCaseData(RuntimeFunctions.CompareLessThanInfo);
            yield return new TestCaseData(RuntimeFunctions.CompareLessThanOrEqualInfo);
            yield return new TestCaseData(RuntimeFunctions.EqualToInfo);
            yield return new TestCaseData(RuntimeFunctions.IfTrueInfo);
            yield return new TestCaseData(LocalValueFunctions.InvokeInfo);
            yield return new TestCaseData(RuntimeFunctions.MathAddInfo);
            yield return new TestCaseData(RuntimeFunctions.MathSubtractInfo);
            yield return new TestCaseData(RuntimeFunctions.MathMultiplyInfo);
            yield return new TestCaseData(RuntimeFunctions.MathDivideInfo);
            yield return new TestCaseData(RuntimeFunctions.MathRemainderInfo);
            yield return new TestCaseData(RuntimeFunctions.MathPowerInfo);
            yield return new TestCaseData(RuntimeFunctions.MathNegateInfo);
            yield return new TestCaseData(RuntimeFunctions.NotEqualToInfo);
            yield return new TestCaseData(StandardFunctions.DictionaryAddInfo);
            yield return new TestCaseData(RuntimeFunctions.StringConcatenateInfo);
        }

        private static IEnumerable<TestCaseData> EqualToTestCases()
        {
            var listValue1 = new ListValue(Array.Empty<ILocalValue>());
            var listValue2 = new ListValue(Array.Empty<ILocalValue>());

            var objectValue1 = new ObjectValue(new Dictionary<string, ILocalValue>());
            var objectValue2 = new ObjectValue(new Dictionary<string, ILocalValue>());

            yield return new TestCaseData(BooleanValue.False, BooleanValue.False, true);
            yield return new TestCaseData(BooleanValue.False, BooleanValue.True, false);
            yield return new TestCaseData(BooleanValue.True, BooleanValue.False, false);
            yield return new TestCaseData(BooleanValue.True, BooleanValue.True, true);

            yield return new TestCaseData(new NumberValue(1), new NumberValue(1), true);
            yield return new TestCaseData(new NumberValue(1), new NumberValue(0), false);
            yield return new TestCaseData(new NumberValue(1), BooleanValue.True, false);
            yield return new TestCaseData(BooleanValue.True, new NumberValue(1), false);
            yield return new TestCaseData(new NumberValue(0), BooleanValue.False, false);
            yield return new TestCaseData(BooleanValue.False, new NumberValue(0), false);

            yield return new TestCaseData(new StringValue("cat"), new StringValue("cat"), true);
            yield return new TestCaseData(new StringValue("cat"), new StringValue("dog"), false);
            yield return new TestCaseData(new StringValue("cat"), BooleanValue.True, false);
            yield return new TestCaseData(BooleanValue.True, new StringValue("cat"), false);
            yield return new TestCaseData(new StringValue("cat"), BooleanValue.False, false);
            yield return new TestCaseData(BooleanValue.False, new StringValue("cat"), false);

            yield return new TestCaseData(new NumberValue(1), new StringValue("1"), true);
            yield return new TestCaseData(new StringValue("1"), new NumberValue(1), true);
            yield return new TestCaseData(new NumberValue(1), new StringValue("0"), false);
            yield return new TestCaseData(new StringValue("0"), new NumberValue(1), false);

            yield return new TestCaseData(listValue1, listValue1, true);
            yield return new TestCaseData(listValue1, listValue2, false);
            yield return new TestCaseData(listValue1, BooleanValue.False, false);
            yield return new TestCaseData(BooleanValue.False, listValue1, false);
            yield return new TestCaseData(listValue1, new NumberValue(0), false);
            yield return new TestCaseData(new NumberValue(0), listValue1, false);
            yield return new TestCaseData(listValue1, new StringValue(string.Empty), false);
            yield return new TestCaseData(new StringValue(string.Empty), listValue1, false);

            yield return new TestCaseData(objectValue1, objectValue1, true);
            yield return new TestCaseData(objectValue1, objectValue2, false);
            yield return new TestCaseData(objectValue1, BooleanValue.False, false);
            yield return new TestCaseData(BooleanValue.False, objectValue1, false);
            yield return new TestCaseData(objectValue1, new NumberValue(0), false);
            yield return new TestCaseData(new NumberValue(0), objectValue1, false);
            yield return new TestCaseData(objectValue1, new StringValue(string.Empty), false);
            yield return new TestCaseData(new StringValue(string.Empty), objectValue1, false);
        }
    }
}