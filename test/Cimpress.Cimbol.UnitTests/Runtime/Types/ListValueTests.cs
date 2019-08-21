using System;
using Cimpress.Cimbol.Exceptions;
using Cimpress.Cimbol.Runtime.Types;
using NUnit.Framework;

namespace Cimpress.Cimbol.UnitTests.Runtime.Types
{
    [TestFixture]
    public class ListValueTests
    {
        [Test]
        public void Should_ThrowException_When_AccessingAnElement()
        {
            var listValue = new ListValue(Array.Empty<ILocalValue>());
            Assert.Throws<CimbolRuntimeException>(() => listValue.Access("test"));
        }

        [Test]
        public void Should_ThrowException_When_ConvertingToBoolean()
        {
            var listValue = new ListValue(Array.Empty<ILocalValue>());
            Assert.Throws<CimbolRuntimeException>(() => listValue.CastBoolean());
        }

        [Test]
        public void Should_ThrowException_When_ConvertingToNumber()
        {
            var listValue = new ListValue(Array.Empty<ILocalValue>());
            Assert.Throws<CimbolRuntimeException>(() => listValue.CastNumber());
        }

        [Test]
        public void Should_ThrowException_When_ConvertingToString()
        {
            var listValue = new ListValue(Array.Empty<ILocalValue>());
            Assert.Throws<CimbolRuntimeException>(() => listValue.CastString());
        }

        [Test]
        public void Should_ThrowException_When_Invoked()
        {
            var listValue = new ListValue(Array.Empty<ILocalValue>());
            Assert.Throws<CimbolRuntimeException>(() => listValue.Invoke());
        }

        [Test]
        public void Should_RetrieveValue_When_GivenValue()
        {
            var listContents = Array.Empty<ILocalValue>();
            var listValue = new ListValue(listContents);
            Assert.That(listContents, Is.SameAs(listValue.Value));
        }
    }
}