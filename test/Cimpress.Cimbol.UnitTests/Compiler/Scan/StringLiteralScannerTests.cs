using Cimpress.Cimbol.Compiler.Scan;
using Cimpress.Cimbol.Compiler.Source;
using Cimpress.Cimbol.Exceptions;
using NUnit.Framework;

namespace Cimpress.Cimbol.UnitTests.Compiler.Scan
{
    public class StringLiteralScannerTests
    {
        [Test]
        [TestCase("\"abc\"", TokenType.StringLiteral)]
        [TestCase("\"ABC\"", TokenType.StringLiteral)]
        [TestCase("\"\\n\"", TokenType.StringLiteral)]
        [TestCase("\"\\r\"", TokenType.StringLiteral)]
        [TestCase("\"\\t\"", TokenType.StringLiteral)]
        [TestCase("\"\\\\\"", TokenType.StringLiteral)]
        [TestCase("\"\\\"\"", TokenType.StringLiteral)]
        [TestCase("\"\\'\"", TokenType.StringLiteral)]
        [TestCase("\"\\u0123\"", TokenType.StringLiteral)]
        [TestCase("\"\\ubeef\"", TokenType.StringLiteral)]
        [TestCase("\"\\uBEEF\"", TokenType.StringLiteral)]
        [TestCase("\"\\U01234567\"", TokenType.StringLiteral)]
        [TestCase("\"\\Udeadbeef\"", TokenType.StringLiteral)]
        [TestCase("\"\\UDEADBEEF\"", TokenType.StringLiteral)]
        [TestCase("\"Quick brown fox\"", TokenType.StringLiteral)]
        [TestCase("\"qUiCk BrOwN fOx\"", TokenType.StringLiteral)]
        [TestCase("\"123\"", TokenType.StringLiteral)]
        [TestCase("\"x123\"", TokenType.StringLiteral)]
        [TestCase("\"X123\"", TokenType.StringLiteral)]
        [TestCase("\"x 123\"", TokenType.StringLiteral)]
        [TestCase("\"X 123\"", TokenType.StringLiteral)]
        [TestCase("\"123y\"", TokenType.StringLiteral)]
        [TestCase("\"123Y\"", TokenType.StringLiteral)]
        [TestCase("\"123 y\"", TokenType.StringLiteral)]
        [TestCase("\"123 Y\"", TokenType.StringLiteral)]
        [TestCase("\"_123\"", TokenType.StringLiteral)]
        [TestCase("\"лошадь\"", TokenType.StringLiteral)]
        [TestCase("\"sous vêtements\"", TokenType.StringLiteral)]
        [TestCase("\"𓀀𓂀𓃞\"", TokenType.StringLiteral)]
        [TestCase("\"うま\"", TokenType.StringLiteral)]
        public void Should_MakeStringLiteral_When_GivenStringLiteralSource(string source, TokenType type)
        {
            var scanner = new Scanner("formula", new SourceText("formula", source));
            var token = scanner.NextStringLiteral();
            Assert.AreEqual(type, token.Type);
            Assert.AreEqual(source, token.Value);
        }

        [Test]
        [TestCase("\"\n\"")]
        [TestCase("\"\\x\"")]
        [TestCase("\"\\u000G\"")]
        [TestCase("\"\\U0000000G\"")]
        public void ShouldNot_MakeStringLiteral_When_GivenGarbageSource(string source)
        {
            var scanner = new Scanner("formula", new SourceText("formula", source));
            Assert.Throws<CimbolCompilationException>(() => scanner.NextStringLiteral());
        }
    }
}