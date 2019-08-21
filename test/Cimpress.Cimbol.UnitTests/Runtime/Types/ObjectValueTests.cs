using System;
using System.Collections.Generic;
using Cimpress.Cimbol.Exceptions;
using Cimpress.Cimbol.Runtime.Types;
using NUnit.Framework;

namespace Cimpress.Cimbol.UnitTests.Runtime.Types
{
    [TestFixture]
    public class ObjectValueTests
    {
        [Test]
        public void Should_ThrowException_When_AccessingAnElement()
        {
            var innerDictionary = new Dictionary<string, ILocalValue> { ["x"] = new BooleanValue(true) };
            var objectValue = new ObjectValue(innerDictionary);
            var booleanResult = objectValue.Access("x") as BooleanValue;
            Assert.IsNotNull(booleanResult);
            Assert.AreEqual(true, booleanResult.Value);
            Assert.That(innerDictionary["x"], Is.SameAs(booleanResult));
        }

        [Test]
        public void Should_ThrowException_When_AccessingAMissingElement()
        {
            var innerDictionary = new Dictionary<string, ILocalValue> { ["x"] = new BooleanValue(true) };
            var objectValue = new ObjectValue(innerDictionary);
            Assert.Throws<CimbolRuntimeException>(() => objectValue.Access("y"));
        }

        [Test]
        public void Should_ThrowException_When_ConvertingToBoolean()
        {
            var objectValue = new ObjectValue(new Dictionary<string, ILocalValue>());
            Assert.Throws<CimbolRuntimeException>(() => objectValue.CastBoolean());
        }

        [Test]
        public void Should_ThrowException_When_ConvertingToNumber()
        {
            var objectValue = new ObjectValue(new Dictionary<string, ILocalValue>());
            Assert.Throws<CimbolRuntimeException>(() => objectValue.CastNumber());
        }

        [Test]
        public void Should_ThrowException_When_ConvertingToString()
        {
            var objectValue = new ObjectValue(new Dictionary<string, ILocalValue>());
            Assert.Throws<CimbolRuntimeException>(() => objectValue.CastString());
        }

        [Test]
        public void Should_ThrowException_When_Invoked()
        {
            var objectValue = new ObjectValue(new Dictionary<string, ILocalValue>());
            Assert.Throws<CimbolRuntimeException>(() => objectValue.Invoke());
        }

        [Test]
        public void Should_RetrieveValue_When_GivenValue()
        {
            var objectContents = new Dictionary<string, ILocalValue>();
            var objectValue = new ObjectValue(objectContents);
            Assert.That(objectContents, Is.SameAs(objectValue.Value));
        }
    }
}