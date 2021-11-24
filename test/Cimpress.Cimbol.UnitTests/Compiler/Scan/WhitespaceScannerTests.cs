// Copyright 2021 Cimpress plc.
// Licensed under the Apache License, Version 2.0
// http://www.apache.org/licenses/LICENSE-2.0

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
        [TestCase("123 ", TokenType.NumberLiteral)]
        [TestCase("123\f", TokenType.NumberLiteral)]
        [TestCase("123\n", TokenType.NumberLiteral)]
        [TestCase("123\r", TokenType.NumberLiteral)]
        [TestCase("123\t", TokenType.NumberLiteral)]
        public void Should_IgnoreWhitespace_When_GivenWhitespaceSource(string source, TokenType type)
        {
            var scanner = new Scanner("formula", new SourceText("formula", source));
            
            var token = scanner.Next();
            while (scanner.Next().Type != TokenType.EndOfFile) { }

            Assert.AreEqual(type, token.Type);
            Assert.AreEqual(source?.Trim(), token.Value);
        }
    }
}