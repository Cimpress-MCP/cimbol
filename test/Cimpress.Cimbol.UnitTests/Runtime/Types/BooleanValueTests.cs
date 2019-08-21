using System;
using Cimpress.Cimbol.Exceptions;
using Cimpress.Cimbol.Runtime.Types;
using NUnit.Framework;

namespace Cimpress.Cimbol.UnitTests.Runtime.Types
{
    [TestFixture]
    public class BooleanValueTests
    {
        [Test]
        public void Should_ThrowException_When_AccessingAnElement()
        {
            var booleanValue = new BooleanValue(true);
            Assert.Throws<CimbolRuntimeException>(() => booleanValue.Access("test"));
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void Should_ReturnSelf_When_ConvertingToBoolean(bool value)
        {
            var booleanValue = new BooleanValue(value);
            var result = booleanValue.CastBoolean();
            Assert.AreEqual(value, result.Value);
            Assert.That(result, Is.SameAs(booleanValue));
        }

        [Test]
        public void Should_ThrowException_When_ConvertingToNumber()
        {
            var booleanValue = new BooleanValue(true);
            Assert.Throws<CimbolRuntimeException>(() => booleanValue.CastNumber());
        }

        [Test]
        public void Should_ThrowException_When_ConvertingToString()
        {
            var booleanValue = new BooleanValue(true);
            Assert.Throws<CimbolRuntimeException>(() => booleanValue.CastString());
        }

        [Test]
        public void Should_ThrowException_When_Invoked()
        {
            var booleanValue = new BooleanValue(true);
            Assert.Throws<CimbolRuntimeException>(() => booleanValue.Invoke());
        }

        [Test]
        public void Should_RetrieveValue_When_GivenValue()
        {
            var booleanValue = new BooleanValue(true);
            Assert.AreEqual(true, booleanValue.Value);
        }
    }
}