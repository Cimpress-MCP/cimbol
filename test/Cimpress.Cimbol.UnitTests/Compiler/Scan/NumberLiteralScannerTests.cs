using System;
using Cimpress.Cimbol.Compiler.Scan;
using Cimpress.Cimbol.Compiler.Utilities;
using NUnit.Framework;

namespace Cimpress.Cimbol.UnitTests.Compiler.Scan
{
    [TestFixture]
    public class NumberLiteralScannerTests
    {
        [Test]
        [TestCase("123", TokenType.NumberLiteral)]
        [TestCase("456.123", TokenType.NumberLiteral)]
        [TestCase(".789", TokenType.NumberLiteral)]
        [TestCase("1e9", TokenType.NumberLiteral)]
        [TestCase("1e-9", TokenType.NumberLiteral)]
        [TestCase("1e+9", TokenType.NumberLiteral)]
        [TestCase("2e99", TokenType.NumberLiteral)]
        [TestCase("2e-99", TokenType.NumberLiteral)]
        [TestCase("2e+99", TokenType.NumberLiteral)]
        [TestCase("3e999", TokenType.NumberLiteral)]
        [TestCase("3e-999", TokenType.NumberLiteral)]
        [TestCase("3e+999", TokenType.NumberLiteral)]
        public void Should_MakeNumberLiteralToken_When_GivenNumberLiteralSource(string source, TokenType type)
        {
            var scanner = new Scanner(new SourceText(source));
            var token = scanner.NextNumberLiteral();
            Assert.AreEqual(type, token.Type);
            Assert.AreEqual(source, token.Value);
        }

        [Test]
        [TestCase("e")]
        [TestCase("e1")]
        [TestCase(".")]
        [TestCase(".e")]
        [TestCase(".e1")]
        [TestCase("-")]
        [TestCase("+")]
        [TestCase("--1")]
        [TestCase("++1")]
        [TestCase("1e1234")]
        [TestCase("1e-1234")]
        [TestCase("1e+1234")]
        public void ShouldNot_MakeNumberLiteralToken_When_GivenGarbageSource(string source)
        {
            var scanner = new Scanner(new SourceText(source));
            Assert.Throws<NotSupportedException>(() => scanner.NextNumberLiteral());
        }
    }
}