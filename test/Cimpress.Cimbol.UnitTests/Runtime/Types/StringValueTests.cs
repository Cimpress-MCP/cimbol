using System;
using Cimpress.Cimbol.Exceptions;
using Cimpress.Cimbol.Runtime.Types;
using NUnit.Framework;

namespace Cimpress.Cimbol.UnitTests.Runtime.Types
{
    [TestFixture]
    public class StringValueTests
    {
        [Test]
        public void Should_ThrowException_When_AccessingAnElement()
        {
            var stringValue = new StringValue("test");
            Assert.Throws<CimbolRuntimeException>(() => stringValue.Access("test"));
        }

        [Test]
        public void Should_ThrowException_When_ConvertingToBoolean()
        {
            var stringValue = new StringValue("test");
            Assert.Throws<CimbolRuntimeException>(() => stringValue.CastBoolean());
        }

        [Test]
        [TestCase("1", 1)]
        [TestCase("1.23", 1.23)]
        [TestCase("-1", -1)]
        [TestCase("-1.23", -1.23)]
        public void Should_ReturnNumber_When_ConvertingValidNumber(string value, decimal expected)
        {
            var stringValue = new StringValue(value);
            var result = stringValue.CastNumber();
            Assert.AreEqual(expected, result.Value);
        }

        [Test]
        [TestCase("test")]
        public void Should_ThrowException_When_ConvertingInvalidNumber(string value)
        {
            var stringValue = new StringValue(value);
            Assert.Throws<CimbolRuntimeException>(() => stringValue.CastNumber());
        }

        [Test]
        public void Should_ReturnSelf_When_ConvertingToString()
        {
            var stringValue = new StringValue("test");
            var result = stringValue.CastString();
            Assert.AreEqual("test", result.Value);
            Assert.That(stringValue, Is.SameAs(result));
        }

        [Test]
        public void Should_ThrowException_When_Invoked()
        {
            var stringValue = new StringValue("test");
            Assert.Throws<CimbolRuntimeException>(() => stringValue.Invoke());
        }

        [Test]
        public void Should_RetrieveValue_When_GivenValue()
        {
            var stringValue = new StringValue("test");
            Assert.AreEqual("test", stringValue.Value);
        }
    }
}