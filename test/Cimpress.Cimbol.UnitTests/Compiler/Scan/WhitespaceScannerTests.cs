using Cimpress.Cimbol.Compiler.Scan;
using Cimpress.Cimbol.Compiler.Source;
using NUnit.Framework;

namespace Cimpress.Cimbol.UnitTests.Compiler.Scan
{
    [TestFixture]
    public class WhitespaceScannerTests
    {
        [Test]
        [TestCase(" 123", TokenType.NumberLiteral)]
        [TestCase("\f123", TokenType.NumberLiteral)]
        [TestCase("\n123", TokenType.NumberLiteral)]
        [TestCase("\r123", TokenType.NumberLiteral)]
        [TestCase("\t123", TokenType.NumberLiteral)]
        public void Should_IgnoreWhitespace_When_GivenWhitespaceSource(string source, TokenType type)
        {
            var scanner = new Scanner("formula", new SourceText("formula", source));
            var token = scanner.Next();
            Assert.AreEqual(type, token.Type);
            Assert.AreEqual(source?.Trim(), token.Value);
        }
    }
}