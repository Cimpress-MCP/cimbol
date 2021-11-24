// Copyright 2021 Cimpress plc.
// Licensed under the Apache License, Version 2.0
// http://www.apache.org/licenses/LICENSE-2.0

using System;
using Cimpress.Cimbol.Exceptions;
using Cimpress.Cimbol.Runtime.Types;
using NUnit.Framework;

namespace Cimpress.Cimbol.UnitTests.Runtime.Types
{
    [TestFixture]
    public class NumberValueTests
    {
        [Test]
        public void Should_ThrowException_When_AccessingAnElement()
        {
            var numberValue = new NumberValue(1);
            Assert.Throws<CimbolRuntimeException>(() => numberValue.Access("test"));
        }

        [Test]
        public void Should_ThrowException_When_ConvertingToBoolean()
        {
            var numberValue = new NumberValue(1);
            Assert.Throws<CimbolRuntimeException>(() => numberValue.CastBoolean());
        }

        [Test]
        public void Should_ReturnSelf_When_ConvertingToNumber()
        {
            var numberValue = new NumberValue(1);
            var numberResult = numberValue.CastNumber();
            Assert.IsNotNull(numberResult);
            Assert.AreEqual(1, numberResult.Value);
            Assert.That(numberValue, Is.SameAs(numberResult));
        }

        [Test]
        [TestCase(1, "1")]
        [TestCase(1.23, "1.23")]
        [TestCase(-1, "-1")]
        [TestCase(-1.23, "-1.23")]
        public void Should_ReturnString_When_ConvertingToString(decimal value, string expected)
        {
            var numberValue = new NumberValue(value);
            var stringResult = numberValue.CastString();
            Assert.IsNotNull(stringResult);
            Assert.AreEqual(expected, stringResult.Value);
        }

        [Test]
        public void Should_ThrowException_When_Invoked()
        {
            var numberValue = new NumberValue(1);
            Assert.Throws<CimbolRuntimeException>(() => numberValue.Invoke());
        }

        [Test]
        public void Should_RetrieveValue_When_GivenValue()
        {
            var numberValue = new NumberValue(1);
            Assert.AreEqual(1, numberValue.Value);
        }
    }
}