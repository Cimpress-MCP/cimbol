using System;
using Cimpress.Cimbol.Compiler.Scan;
using Cimpress.Cimbol.Compiler.Source;
using NUnit.Framework;

namespace Cimpress.Cimbol.UnitTests.Compiler.Scan
{
    [TestFixture]
    public class BareWordScannerTests
    {
        [Test]
        [TestCase("abc", TokenType.Identifier)]
        [TestCase("ABC", TokenType.Identifier)]
        [TestCase("x123", TokenType.Identifier)]
        [TestCase("X123", TokenType.Identifier)]
        [TestCase("_123", TokenType.Identifier)]
        [TestCase("лошадь", TokenType.Identifier)]
        [TestCase("sousVêtements", TokenType.Identifier)]
        [TestCase("𓀀𓂀𓃞", TokenType.Identifier)]
        [TestCase("うま", TokenType.Identifier)]
        public void Should_MakeIdentifier_When_GivenIdentifierSource(string source, TokenType type)
        {
            var scanner = new Scanner(new SourceText(source));
            var token = scanner.NextBareWord();
            Assert.AreEqual(type, token.Type);
            Assert.AreEqual(source, token.Value);
        }

        [Test]
        [TestCase("🦷")]
        [TestCase("🂡")]
        public void ShouldNot_MakeIdentifier_When_GiveGarbageSource(string source)
        {
            var scanner = new Scanner(new SourceText(source));
            Assert.Throws<NotSupportedException>(() => scanner.NextBareWord());
        }

        [Test]
        [TestCase("and", TokenType.And)]
        [TestCase("AND", TokenType.And)]
        [TestCase("aNd", TokenType.And)]
        [TestCase("await", TokenType.AwaitKeyword)]
        [TestCase("AWAIT", TokenType.AwaitKeyword)]
        [TestCase("aWaIt", TokenType.AwaitKeyword)]
        [TestCase("false", TokenType.FalseKeyword)]
        [TestCase("FALSE", TokenType.FalseKeyword)]
        [TestCase("fAlSe", TokenType.FalseKeyword)]
        [TestCase("if", TokenType.IfKeyword)]
        [TestCase("IF", TokenType.IfKeyword)]
        [TestCase("iF", TokenType.IfKeyword)]
        [TestCase("list", TokenType.ListKeyword)]
        [TestCase("LIST", TokenType.ListKeyword)]
        [TestCase("lIsT", TokenType.ListKeyword)]
        [TestCase("not", TokenType.Not)]
        [TestCase("NOT", TokenType.Not)]
        [TestCase("nOt", TokenType.Not)]
        [TestCase("object", TokenType.ObjectKeyword)]
        [TestCase("OBJECT", TokenType.ObjectKeyword)]
        [TestCase("oBjEcT", TokenType.ObjectKeyword)]
        [TestCase("or", TokenType.Or)]
        [TestCase("OR", TokenType.Or)]
        [TestCase("oR", TokenType.Or)]
        [TestCase("true", TokenType.TrueKeyword)]
        [TestCase("TRUE", TokenType.TrueKeyword)]
        [TestCase("tRuE", TokenType.TrueKeyword)]
        [TestCase("where", TokenType.WhereKeyword)]
        [TestCase("WHERE", TokenType.WhereKeyword)]
        [TestCase("wHeRe", TokenType.WhereKeyword)]
        [TestCase("xor", TokenType.Xor)]
        [TestCase("XOR", TokenType.Xor)]
        [TestCase("xOr", TokenType.Xor)]
        public void Should_MakeKeyword_When_GivenKeywordSource(string source, TokenType type)
        {
            var scanner = new Scanner(new SourceText(source));
            var token = scanner.NextBareWord();
            Assert.AreEqual(type, token.Type);
            Assert.AreEqual(source, token.Value);
        }
    }
}