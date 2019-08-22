using System;
using Cimpress.Cimbol.Exceptions;
using Cimpress.Cimbol.Utilities;
using NUnit.Framework;

namespace Cimpress.Cimbol.UnitTests.Compiler.Utilities
{
    [TestFixture]
    public class NumberSerializerTests
    {
        [Test]
        [TestCase("1", 1)]
        [TestCase("-2", -2)]
        [TestCase("3.4", 3.4)]
        [TestCase("-5.6", -5.6)]
        [TestCase(".7", 0.7)]
        [TestCase("-.8", -0.8)]
        public void Should_DeserializeToNumber_When_Number(string given, decimal expected)
        {
            var actual = NumberSerializer.DeserializeNumber(given);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        [TestCase("cat")]
        [TestCase("1.2.3")]
        [TestCase("1.2frog")]
        [TestCase("frog1.2")]
        public void ShouldNot_SerializeToNumber_When_NotNumber(string given)
        {
            Assert.Throws<CimbolInternalException>(() => NumberSerializer.DeserializeNumber(given));
        }

        [Test]
        [TestCase(1, "1")]
        [TestCase(-2, "-2")]
        [TestCase(3.4, "3.4")]
        [TestCase(-5.6, "-5.6")]
        [TestCase(0.7, "0.7")]
        [TestCase(-0.8, "-0.8")]
        [TestCase(1, "1")]
        public void Should_SerializeToNumber_When_Number(decimal given, string expected)
        {
            var actual = NumberSerializer.SerializeNumber(given);
            Assert.AreEqual(expected, actual);
        }
    }
}