using Cimpress.Cimbol.Compiler.Scan;
using Cimpress.Cimbol.Compiler.Source;
using Cimpress.Cimbol.Exceptions;
using NUnit.Framework;

namespace Cimpress.Cimbol.UnitTests.Compiler.Scan
{
    [TestFixture]
    public class EscapedIdentifierScannerTests
    {
        [Test]
        [TestCase("'abc'", TokenType.Identifier)]
        [TestCase("'ABC'", TokenType.Identifier)]
        [TestCase("'\\n'", TokenType.Identifier)]
        [TestCase("'\\r'", TokenType.Identifier)]
        [TestCase("'\\t'", TokenType.Identifier)]
        [TestCase("'\\\\'", TokenType.Identifier)]
        [TestCase("'\\\"'", TokenType.Identifier)]
        [TestCase("'\\''", TokenType.Identifier)]
        [TestCase("'\\u0123'", TokenType.Identifier)]
        [TestCase("'\\ubeef'", TokenType.Identifier)]
        [TestCase("'\\uBEEF'", TokenType.Identifier)]
        [TestCase("'\\U01234567'", TokenType.Identifier)]
        [TestCase("'\\Udeadbeef'", TokenType.Identifier)]
        [TestCase("'\\UDEADBEEF'", TokenType.Identifier)]
        [TestCase("'Quick brown fox'", TokenType.Identifier)]
        [TestCase("'qUiCk BrOwN fOx'", TokenType.Identifier)]
        [TestCase("'123'", TokenType.Identifier)]
        [TestCase("'x123'", TokenType.Identifier)]
        [TestCase("'X123'", TokenType.Identifier)]
        [TestCase("'x 123'", TokenType.Identifier)]
        [TestCase("'X 123'", TokenType.Identifier)]
        [TestCase("'123y'", TokenType.Identifier)]
        [TestCase("'123Y'", TokenType.Identifier)]
        [TestCase("'123 y'", TokenType.Identifier)]
        [TestCase("'123 Y'", TokenType.Identifier)]
        [TestCase("'_123'", TokenType.Identifier)]
        [TestCase("'лошадь'", TokenType.Identifier)]
        [TestCase("'sous vêtements'", TokenType.Identifier)]
        [TestCase("'𓀀𓂀𓃞'", TokenType.Identifier)]
        [TestCase("'うま'", TokenType.Identifier)]
        public void Should_MakeEscapedIdentifier_When_GivenEscapedIdentifierSource(string source, TokenType type)
        {
            var scanner = new Scanner("formula", new SourceText("formula", source));
            var token = scanner.NextEscapedIdentifier();
            Assert.AreEqual(type, token.Type);
            Assert.AreEqual(source, token.Value);
        }

        [Test]
        [TestCase("'\n'")]
        [TestCase("'\\x'")]
        [TestCase("'\\u000G'")]
        [TestCase("'\\U0000000G'")]
        public void ShouldNot_MakeEscapedIdentifier_When_GivenGarbageSource(string source)
        {
            var scanner = new Scanner("formula", new SourceText("formula", source));
            Assert.Throws<CimbolCompilationException>(() => scanner.NextEscapedIdentifier());
        }
    }
}