using System;
using Cimpress.Cimbol.Compiler.Scan;
using Cimpress.Cimbol.Compiler.Utilities;
using NUnit.Framework;

namespace Cimpress.Cimbol.UnitTests.Compiler.Scan
{
    [TestFixture]
    public class ScannerTests
    {
        [Test]
        [TestCase("123", TokenType.NumberLiteral)]
        [TestCase("\"cat\"", TokenType.StringLiteral)]
        [TestCase("'dog'", TokenType.Identifier)]
        [TestCase("fish", TokenType.Identifier)]
        [TestCase("(", TokenType.LeftParenthesis)]
        [TestCase(".", TokenType.Period)]
        [TestCase(",", TokenType.Comma)]
        [TestCase("+", TokenType.Add)]
        [TestCase("-", TokenType.Subtract)]
        [TestCase("*", TokenType.Multiply)]
        [TestCase("/", TokenType.Divide)]
        [TestCase("and", TokenType.And)]
        [TestCase("or", TokenType.Or)]
        [TestCase("xor", TokenType.Xor)]
        [TestCase("not", TokenType.Not)]
        public void Should_PickCorrectToken_When_GivenSource(string source, TokenType type)
        {
            var scanner = new Scanner(new SourceText(source));
            var token = scanner.Next();
            Assert.AreEqual(type, token.Type);
            Assert.AreEqual(source, token.Value);
        }

        [Test]
        [TestCase("🦷")]
        [TestCase("{")]
        [TestCase("}")]
        [TestCase("[")]
        [TestCase("]")]
        [TestCase("&")]
        [TestCase("#")]
        [TestCase("$")]
        public void Should_ThrowError_When_GivenUnrecognizableCharacters(string source)
        {
            var scanner = new Scanner(new SourceText(source));
            Assert.Throws<NotSupportedException>(() => scanner.Next());
        }
    }
}