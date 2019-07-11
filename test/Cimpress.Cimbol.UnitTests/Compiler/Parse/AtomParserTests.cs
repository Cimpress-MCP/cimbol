using System;
using Cimpress.Cimbol.Compiler.Parse;
using Cimpress.Cimbol.Compiler.Scan;
using Cimpress.Cimbol.Compiler.SyntaxTree;
using Cimpress.Cimbol.Compiler.Utilities;
using NUnit.Framework;

namespace Cimpress.Cimbol.UnitTests.Compiler.Parse
{
    [TestFixture]
    public class AtomParserTests
    {
        [Test]
        public void Should_ParseConstantNodeWithBoolean_When_GivenTrueKeyword()
        {
            var tokenStream = ParseTestUtilities.CreateTokenStream(
                new Token(string.Empty, TokenType.TrueKeyword, new Position(0, 0), new Position(0, 0)));

            var parser = new Parser(tokenStream);

            var result = parser.Atom() as ConstantNode;

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<bool>(result.Value);
            Assert.AreEqual(true, result.Value);
        }

        [Test]
        public void Should_ParseConstantNodeWithBoolean_When_GivenParenthesizedTrueKeyword()
        {
            var tokenStream = ParseTestUtilities.CreateTokenStream(
                new Token("(", TokenType.LeftParenthesis, new Position(0, 0), new Position(0, 0)),
                new Token(string.Empty, TokenType.TrueKeyword, new Position(0, 0), new Position(0, 0)),
                new Token(")", TokenType.RightParenthesis, new Position(0, 0), new Position(0, 0)));

            var parser = new Parser(tokenStream);

            var result = parser.Atom() as ConstantNode;

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<bool>(result.Value);
            Assert.AreEqual(true, result.Value);
        }

        [Test]
        public void Should_ParseConstantNodeWithBoolean_When_GivenFalseKeyword()
        {
            var tokenStream = ParseTestUtilities.CreateTokenStream(
                new Token(string.Empty, TokenType.FalseKeyword, new Position(0, 0), new Position(0, 0)));

            var parser = new Parser(tokenStream);

            var result = parser.Atom() as ConstantNode;

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<bool>(result.Value);
            Assert.AreEqual(false, result.Value);
        }

        [Test]
        public void Should_ParseConstantNodeWithBoolean_When_GivenParenthesizedFalseKeyword()
        {
            var tokenStream = ParseTestUtilities.CreateTokenStream(
                new Token("(", TokenType.LeftParenthesis, new Position(0, 0), new Position(0, 0)),
                new Token(string.Empty, TokenType.FalseKeyword, new Position(0, 0), new Position(0, 0)),
                new Token(")", TokenType.RightParenthesis, new Position(0, 0), new Position(0, 0)));

            var parser = new Parser(tokenStream);

            var result = parser.Atom() as ConstantNode;

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<bool>(result.Value);
            Assert.AreEqual(false, result.Value);
        }

        [Test]
        public void Should_ParseConstantNodeWithNumber_When_GivenNumberLiteral()
        {
            var tokenStream = ParseTestUtilities.CreateTokenStream(
                new Token("123", TokenType.NumberLiteral, new Position(0, 0), new Position(0, 0)));

            var parser = new Parser(tokenStream);

            var result = parser.Atom() as ConstantNode;

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<decimal>(result.Value);
            Assert.AreEqual(123, result.Value);
        }

        [Test]
        public void Should_ParseConstantNodeWithNumber_When_GivenParenthesizedNumberLiteral()
        {
            var tokenStream = ParseTestUtilities.CreateTokenStream(
                new Token("(", TokenType.LeftParenthesis, new Position(0, 0), new Position(0, 0)),
                new Token("123", TokenType.NumberLiteral, new Position(0, 0), new Position(0, 0)),
                new Token(")", TokenType.RightParenthesis, new Position(0, 0), new Position(0, 0)));

            var parser = new Parser(tokenStream);

            var result = parser.Atom() as ConstantNode;

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<decimal>(result.Value);
            Assert.AreEqual(123, result.Value);
        }

        [Test]
        public void Should_ParseConstantNodeWithString_When_GivenStringLiteral()
        {
            var tokenStream = ParseTestUtilities.CreateTokenStream(
                new Token("\"hat\"", TokenType.StringLiteral, new Position(0, 0), new Position(0, 0)));

            var parser = new Parser(tokenStream);

            var result = parser.Atom() as ConstantNode;

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<string>(result.Value);
            Assert.AreEqual("hat", result.Value);
        }

        [Test]
        public void Should_ParseConstantNodeWithString_When_GivenParenthesizedStringLiteral()
        {
            var tokenStream = ParseTestUtilities.CreateTokenStream(
                new Token("(", TokenType.LeftParenthesis, new Position(0, 0), new Position(0, 0)),
                new Token("\"hat\"", TokenType.StringLiteral, new Position(0, 0), new Position(0, 0)),
                new Token(")", TokenType.RightParenthesis, new Position(0, 0), new Position(0, 0)));

            var parser = new Parser(tokenStream);

            var result = parser.Atom() as ConstantNode;

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<string>(result.Value);
            Assert.AreEqual("hat", result.Value);
        }

        [Test]
        public void Should_ParseIdentifierNode_When_GivenIdentifier()
        {
            var tokenStream = ParseTestUtilities.CreateTokenStream(
                new Token("hat", TokenType.Identifier, new Position(0, 0), new Position(0, 0)));

            var parser = new Parser(tokenStream);

            var result = parser.Atom() as IdentifierNode;

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<string>(result.Identifier);
            Assert.AreEqual("hat", result.Identifier);
        }

        [Test]
        public void Should_ParseIdentifierNode_When_GivenParenthesizedIdentifier()
        {
            var tokenStream = ParseTestUtilities.CreateTokenStream(
                new Token("(", TokenType.LeftParenthesis, new Position(0, 0), new Position(0, 0)),
                new Token("hat", TokenType.Identifier, new Position(0, 0), new Position(0, 0)),
                new Token(")", TokenType.RightParenthesis, new Position(0, 0), new Position(0, 0)));

            var parser = new Parser(tokenStream);

            var result = parser.Atom() as IdentifierNode;

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<string>(result.Identifier);
            Assert.AreEqual("hat", result.Identifier);
        }

        [Test]
        public void ShouldNot_ParseAtom_When_GivenInvalidToken()
        {
            var tokenStream = ParseTestUtilities.CreateTokenStream(
                new Token("+", TokenType.Add, new Position(0, 0), new Position(0, 0)));

            var parser = new Parser(tokenStream);

            Assert.Throws<NotSupportedException>(() => parser.Atom());
        }

        [Test]
        public void ShouldNot_ParseAtom_When_GivenEndOfFile()
        {
            var tokenStream = ParseTestUtilities.CreateTokenStream();

            var parser = new Parser(tokenStream);

            Assert.Throws<NotSupportedException>(() => parser.Atom());
        }
    }
}