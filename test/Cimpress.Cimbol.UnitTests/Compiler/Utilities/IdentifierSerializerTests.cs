using System;
using Cimpress.Cimbol.Exceptions;
using Cimpress.Cimbol.Utilities;
using NUnit.Framework;

namespace Cimpress.Cimbol.UnitTests.Compiler.Utilities
{
    [TestFixture]
    public class IdentifierSerializerTests
    {
        [Test]
        [TestCase("abc", "abc")]
        [TestCase("'abc'", "abc")]
        [TestCase("_xyz", "_xyz")]
        [TestCase("'_xyz'", "_xyz")]
        [TestCase("_123", "_123")]
        [TestCase("'_123'", "_123")]
        [TestCase("'123'", "123")]
        [TestCase("'my dog'", "my dog")]
        [TestCase("'\\\"'", "\"")]
        [TestCase("'\\''", "'")]
        [TestCase("'\\\\'", "\\")]
        [TestCase("'\\n'", "\n")]
        [TestCase("'\\r'", "\r")]
        [TestCase("'\\t'", "\t")]
        [TestCase("'\\u0041'", "A")]
        [TestCase("'\\U00000041'", "A")]
        public void Should_DeserializeToString_When_Identifier(string given, string expected)
        {
            var actual = IdentifierSerializer.DeserializeIdentifier(given);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        [TestCase("123")]
        [TestCase("my dog")]
        [TestCase("\n")]
        public void ShouldNot_DeserializeToString_When_NotIdentifier(string given)
        {
            Assert.Throws<CimbolInternalException>(() => IdentifierSerializer.DeserializeIdentifier(given));
        }

        [Test]
        [TestCase("abc", "abc")]
        [TestCase("_def", "_def")]
        [TestCase("'ghi'", "'\\'ghi\\''")]
        [TestCase("\"jkl\"", "'\\\"jkl\\\"'")]
        [TestCase("\n", "'\\n'")]
        public void Should_SerializeToString_When_Identifier(string given, string expected)
        {
            var actual = IdentifierSerializer.SerializeIdentifier(given);
            Assert.AreEqual(expected, actual);
        }
    }
}