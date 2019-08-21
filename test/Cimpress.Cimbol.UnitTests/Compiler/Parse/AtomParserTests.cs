using Cimpress.Cimbol.Compiler.Parse;
using Cimpress.Cimbol.Compiler.Scan;
using Cimpress.Cimbol.Compiler.SyntaxTree;
using Cimpress.Cimbol.Exceptions;
using Cimpress.Cimbol.Runtime.Types;
using Cimpress.Cimbol.Utilities;
using NUnit.Framework;

namespace Cimpress.Cimbol.UnitTests.Compiler.Parse
{
    [TestFixture]
    public class AtomParserTests
    {
        [Test]
        public void Should_ParseLiteralNodeWithBoolean_When_GivenTrueKeyword()
        {
            var tokenStream = ParseTestUtilities.CreateTokenStream(
                new Token(string.Empty, TokenType.TrueKeyword, new Position(0, 0), new Position(0, 0)));
            var parser = new Parser("formula", tokenStream);

            var result = parser.Atom() as LiteralNode;

            Assert.That(result, Is.Not.Null);
            var resultValue = result.Value as BooleanValue;
            Assert.That(resultValue, Is.Not.Null);
            Assert.That(resultValue.Value, Is.EqualTo(true));
        }

        [Test]
        public void Should_ParseLiteralNodeWithBoolean_When_GivenParenthesizedTrueKeyword()
        {
            var tokenStream = ParseTestUtilities.CreateTokenStream(
                new Token("(", TokenType.LeftParenthesis, new Position(0, 0), new Position(0, 0)),
                new Token(string.Empty, TokenType.TrueKeyword, new Position(0, 0), new Position(0, 0)),
                new Token(")", TokenType.RightParenthesis, new Position(0, 0), new Position(0, 0)));
            var parser = new Parser("formula", tokenStream);

            var result = parser.Atom() as LiteralNode;

            Assert.That(result, Is.Not.Null);
            var resultValue = result.Value as BooleanValue;
            Assert.That(resultValue, Is.Not.Null);
            Assert.That(resultValue.Value, Is.EqualTo(true));
        }

        [Test]
        public void Should_ParseLiteralNodeWithBoolean_When_GivenFalseKeyword()
        {
            var tokenStream = ParseTestUtilities.CreateTokenStream(
                new Token(string.Empty, TokenType.FalseKeyword, new Position(0, 0), new Position(0, 0)));
            var parser = new Parser("formula", tokenStream);

            var result = parser.Atom() as LiteralNode;

            Assert.That(result, Is.Not.Null);
            var resultValue = result.Value as BooleanValue;
            Assert.That(resultValue, Is.Not.Null);
            Assert.That(resultValue.Value, Is.EqualTo(false));
        }

        [Test]
        public void Should_ParseLiteralNodeWithBoolean_When_GivenParenthesizedFalseKeyword()
        {
            var tokenStream = ParseTestUtilities.CreateTokenStream(
                new Token("(", TokenType.LeftParenthesis, new Position(0, 0), new Position(0, 0)),
                new Token(string.Empty, TokenType.FalseKeyword, new Position(0, 0), new Position(0, 0)),
                new Token(")", TokenType.RightParenthesis, new Position(0, 0), new Position(0, 0)));
            var parser = new Parser("formula", tokenStream);

            var result = parser.Atom() as LiteralNode;

            Assert.That(result, Is.Not.Null);
            var resultValue = result.Value as BooleanValue;
            Assert.That(resultValue, Is.Not.Null);
            Assert.That(resultValue.Value, Is.EqualTo(false));
        }

        [Test]
        public void Should_ParseLiteralNodeWithNumber_When_GivenNumberLiteral()
        {
            var tokenStream = ParseTestUtilities.CreateTokenStream(
                new Token("123", TokenType.NumberLiteral, new Position(0, 0), new Position(0, 0)));
            var parser = new Parser("formula", tokenStream);

            var result = parser.Atom() as LiteralNode;

            Assert.That(result, Is.Not.Null);
            var resultValue = result.Value as NumberValue;
            Assert.That(resultValue, Is.Not.Null);
            Assert.That(resultValue.Value, Is.EqualTo(123));
        }

        [Test]
        public void Should_ParseLiteralNodeWithNumber_When_GivenParenthesizedNumberLiteral()
        {
            var tokenStream = ParseTestUtilities.CreateTokenStream(
                new Token("(", TokenType.LeftParenthesis, new Position(0, 0), new Position(0, 0)),
                new Token("123", TokenType.NumberLiteral, new Position(0, 0), new Position(0, 0)),
                new Token(")", TokenType.RightParenthesis, new Position(0, 0), new Position(0, 0)));

            var parser = new Parser("formula", tokenStream);

            var result = parser.Atom() as LiteralNode;

            Assert.That(result, Is.Not.Null);
            var resultValue = result.Value as NumberValue;
            Assert.That(resultValue, Is.Not.Null);
            Assert.That(resultValue.Value, Is.EqualTo(123));
        }

        [Test]
        public void Should_ParseLiteralNodeWithString_When_GivenStringLiteral()
        {
            var tokenStream = ParseTestUtilities.CreateTokenStream(
                new Token("\"hat\"", TokenType.StringLiteral, new Position(0, 0), new Position(0, 0)));
            var parser = new Parser("formula", tokenStream);

            var result = parser.Atom() as LiteralNode;

            Assert.That(result, Is.Not.Null);
            var resultValue = result.Value as StringValue;
            Assert.That(resultValue, Is.Not.Null);
            Assert.That(resultValue.Value, Is.EqualTo("hat"));
        }

        [Test]
        public void Should_ParseLiteralNodeWithString_When_GivenParenthesizedStringLiteral()
        {
            var tokenStream = ParseTestUtilities.CreateTokenStream(
                new Token("(", TokenType.LeftParenthesis, new Position(0, 0), new Position(0, 0)),
                new Token("\"hat\"", TokenType.StringLiteral, new Position(0, 0), new Position(0, 0)),
                new Token(")", TokenType.RightParenthesis, new Position(0, 0), new Position(0, 0)));
            var parser = new Parser("formula", tokenStream);

            var result = parser.Atom() as LiteralNode;

            Assert.That(result, Is.Not.Null);
            var resultValue = result.Value as StringValue;
            Assert.That(resultValue, Is.Not.Null);
            Assert.That(resultValue.Value, Is.EqualTo("hat"));
        }

        [Test]
        public void Should_ParseIdentifierNode_When_GivenIdentifier()
        {
            var tokenStream = ParseTestUtilities.CreateTokenStream(
                new Token("hat", TokenType.Identifier, new Position(0, 0), new Position(0, 0)));
            var parser = new Parser("formula", tokenStream);

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
            var parser = new Parser("formula", tokenStream);

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
            var parser = new Parser("formula", tokenStream);

            Assert.Throws<CimbolCompilationException>(() => parser.Atom());
        }

        [Test]
        public void ShouldNot_ParseAtom_When_GivenEndOfFile()
        {
            var tokenStream = ParseTestUtilities.CreateTokenStream();
            var parser = new Parser("formula", tokenStream);

            Assert.Throws<CimbolCompilationException>(() => parser.Atom());
        }
    }
}