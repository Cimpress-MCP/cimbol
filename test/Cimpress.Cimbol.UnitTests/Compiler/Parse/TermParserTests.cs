// Copyright 2021 Cimpress plc.
// Licensed under the Apache License, Version 2.0
// http://www.apache.org/licenses/LICENSE-2.0

using Cimpress.Cimbol.Compiler.Parse;
using Cimpress.Cimbol.Compiler.Scan;
using Cimpress.Cimbol.Compiler.SyntaxTree;
using Cimpress.Cimbol.Utilities;
using NUnit.Framework;

namespace Cimpress.Cimbol.UnitTests.Compiler.Parse
{
    [TestFixture]
    public class TermParserTests
    {
        [Test]
        public void Should_PassThroughAdd_When_NotAdd()
        {
            var tokenStream = ParseTestUtilities.CreateTokenStream(
                new Token("x", TokenType.Identifier, new Position(0, 0), new Position(0, 0)));

            var parser = new Parser("formula", tokenStream);

            ParserAssert.Parses<IdentifierNode>(() => parser.Add());
        }

        [Test]
        [TestCase("+", TokenType.Add, BinaryOpType.Add)]
        [TestCase("-", TokenType.Subtract, BinaryOpType.Subtract)]
        public void Should_ParseBinaryOpNode_When_GivenAdd(string value, TokenType token, BinaryOpType op)
        {
            var tokenStream = ParseTestUtilities.CreateTokenStream(
                new Token("x", TokenType.Identifier, new Position(0, 0), new Position(0, 0)),
                new Token(value, token, new Position(0, 0), new Position(0, 0)),
                new Token("y", TokenType.Identifier, new Position(0, 0), new Position(0, 0)));

            var parser = new Parser("formula", tokenStream);

            var result = ParserAssert.Parses<BinaryOpNode>(() => parser.Add());
            Assert.AreEqual(op, result.OpType);

            var leftResult = ParserAssert.HasProperty<IdentifierNode>(result, "Left");
            Assert.AreEqual("x", leftResult.Identifier);

            var rightResult = ParserAssert.HasProperty<IdentifierNode>(result, "Right");
            Assert.AreEqual("y", rightResult.Identifier);
        }

        [Test]
        [TestCase("+", TokenType.Add)]
        [TestCase("-", TokenType.Subtract)]
        public void Should_ParseLeftAssociative_When_GivenAdd(string value, TokenType token)
        {
            var tokenStream = ParseTestUtilities.CreateTokenStream(
                new Token("x", TokenType.Identifier, new Position(0, 0), new Position(0, 0)),
                new Token(value, token, new Position(0, 0), new Position(0, 0)),
                new Token("y", TokenType.Identifier, new Position(0, 0), new Position(0, 0)),
                new Token(value, token, new Position(0, 0), new Position(0, 0)),
                new Token("z", TokenType.Identifier, new Position(0, 0), new Position(0, 0)));

            var parser = new Parser("formula", tokenStream);

            var result = ParserAssert.Parses<BinaryOpNode>(() => parser.Add());
            ParserAssert.HasProperty<BinaryOpNode>(result, "Left");
            ParserAssert.HasProperty<IdentifierNode>(result, "Right");
        }

        [Test]
        public void Should_PassThroughAdd_When_NotConcatenate()
        {
            var tokenStream = ParseTestUtilities.CreateTokenStream(
                new Token("x", TokenType.Identifier, new Position(0, 0), new Position(0, 0)));

            var parser = new Parser("formula", tokenStream);

            ParserAssert.Parses<IdentifierNode>(() => parser.Concatenate());
        }

        [Test]
        [TestCase("++", TokenType.Concatenate, BinaryOpType.Concatenate)]
        public void Should_ParseBinaryOpNode_When_GivenConcatenate(string value, TokenType token, BinaryOpType op)
        {
            var tokenStream = ParseTestUtilities.CreateTokenStream(
                new Token("x", TokenType.Identifier, new Position(0, 0), new Position(0, 0)),
                new Token(value, token, new Position(0, 0), new Position(0, 0)),
                new Token("y", TokenType.Identifier, new Position(0, 0), new Position(0, 0)));

            var parser = new Parser("formula", tokenStream);

            var result = ParserAssert.Parses<BinaryOpNode>(() => parser.Concatenate());
            Assert.AreEqual(op, result.OpType);

            var leftResult = ParserAssert.HasProperty<IdentifierNode>(result, "Left");
            Assert.AreEqual("x", leftResult.Identifier);

            var rightResult = ParserAssert.HasProperty<IdentifierNode>(result, "Right");
            Assert.AreEqual("y", rightResult.Identifier);
        }

        [Test]
        [TestCase("++", TokenType.Concatenate)]
        public void Should_ParseLeftAssociative_When_GivenConcatenate(string value, TokenType token)
        {
            var tokenStream = ParseTestUtilities.CreateTokenStream(
                new Token("x", TokenType.Identifier, new Position(0, 0), new Position(0, 0)),
                new Token(value, token, new Position(0, 0), new Position(0, 0)),
                new Token("y", TokenType.Identifier, new Position(0, 0), new Position(0, 0)),
                new Token(value, token, new Position(0, 0), new Position(0, 0)),
                new Token("z", TokenType.Identifier, new Position(0, 0), new Position(0, 0)));

            var parser = new Parser("formula", tokenStream);

            var result = ParserAssert.Parses<BinaryOpNode>(() => parser.Concatenate());
            ParserAssert.HasProperty<BinaryOpNode>(result, "Left");
            ParserAssert.HasProperty<IdentifierNode>(result, "Right");
        }

        [Test]
        public void Should_PassThrough_When_NotFactor()
        {
            var tokenStream = ParseTestUtilities.CreateTokenStream(
                new Token("x", TokenType.Identifier, new Position(0, 0), new Position(0, 0)));

            var parser = new Parser("formula", tokenStream);

            ParserAssert.Parses<IdentifierNode>(() => parser.Unary());
        }

        [Test]
        public void Should_PassThrough_When_PositiveFactor()
        {
            var tokenStream = ParseTestUtilities.CreateTokenStream(
                new Token("+", TokenType.Add, new Position(0, 0), new Position(0, 0)),
                new Token("x", TokenType.Identifier, new Position(0, 0), new Position(0, 0)));

            var parser = new Parser("formula", tokenStream);

            ParserAssert.Parses<IdentifierNode>(() => parser.Unary());
        }

        [Test]
        [TestCase("-", TokenType.Subtract, UnaryOpType.Negate)]
        public void Should_ParseUnaryOpNode_When_GivenFactor(string value, TokenType token, UnaryOpType op)
        {
            var tokenStream = ParseTestUtilities.CreateTokenStream(
                new Token(value, token, new Position(0, 0), new Position(0, 0)),
                new Token("x", TokenType.Identifier, new Position(0, 0), new Position(0, 0)));

            var parser = new Parser("formula", tokenStream);

            var result = ParserAssert.Parses<UnaryOpNode>(() => parser.Unary());
            Assert.AreEqual(op, result.OpType);

            var innerResult = ParserAssert.HasProperty<IdentifierNode>(result, "Operand");
            Assert.AreEqual("x", innerResult.Identifier);
        }

        [Test]
        [TestCase("-", TokenType.Subtract, UnaryOpType.Negate)]
        public void Should_ParseUnaryOpNode_When_GivenNestedFactors(string value, TokenType token, UnaryOpType op)
        {
            var tokenStream = ParseTestUtilities.CreateTokenStream(
                new Token(value, token, new Position(0, 0), new Position(0, 0)),
                new Token(value, token, new Position(0, 0), new Position(0, 0)),
                new Token(value, token, new Position(0, 0), new Position(0, 0)),
                new Token("x", TokenType.Identifier, new Position(0, 0), new Position(0, 0)));

            var parser = new Parser("formula", tokenStream);

            var result = ParserAssert.Parses<UnaryOpNode>(() => parser.Unary());
            Assert.AreEqual(op, result.OpType);

            var innerResult = ParserAssert.HasProperty<IdentifierNode>(result, "Operand");
            Assert.AreEqual("x", innerResult.Identifier);
        }

        [Test]
        [TestCase("-", TokenType.Subtract)]
        public void Should_PruneRedundantUnaryOperations_When_GivenNestedFactors(
            string value,
            TokenType token)
        {
            var tokenStream = ParseTestUtilities.CreateTokenStream(
                new Token(value, token, new Position(0, 0), new Position(0, 0)),
                new Token(value, token, new Position(0, 0), new Position(0, 0)),
                new Token("x", TokenType.Identifier, new Position(0, 0), new Position(0, 0)));

            var parser = new Parser("formula", tokenStream);

            var result = ParserAssert.Parses<IdentifierNode>(() => parser.Unary());
            Assert.That(result.Identifier, Is.EqualTo("x"));
        }

        [Test]
        public void Should_PassThroughMultiply_When_NotGivenMultiply()
        {
            var tokenStream = ParseTestUtilities.CreateTokenStream(
                new Token("x", TokenType.Identifier, new Position(0, 0), new Position(0, 0)));

            var parser = new Parser("formula", tokenStream);

            ParserAssert.Parses<IdentifierNode>(() => parser.Multiply());
        }

        [Test]
        [TestCase("/", TokenType.Divide, BinaryOpType.Divide)]
        [TestCase("*", TokenType.Multiply, BinaryOpType.Multiply)]
        [TestCase("%", TokenType.Remainder, BinaryOpType.Remainder)]
        public void Should_ParseBinaryOpNode_When_GivenMultiply(string value, TokenType token, BinaryOpType op)
        {
            var tokenStream = ParseTestUtilities.CreateTokenStream(
                new Token("x", TokenType.Identifier, new Position(0, 0), new Position(0, 0)),
                new Token(value, token, new Position(0, 0), new Position(0, 0)),
                new Token("y", TokenType.Identifier, new Position(0, 0), new Position(0, 0)));

            var parser = new Parser("formula", tokenStream);

            var result = ParserAssert.Parses<BinaryOpNode>(() => parser.Multiply());
            Assert.AreEqual(op, result.OpType);

            var leftResult = ParserAssert.HasProperty<IdentifierNode>(result, "Left");
            Assert.AreEqual("x", leftResult.Identifier);

            var rightResult = ParserAssert.HasProperty<IdentifierNode>(result, "Right");
            Assert.AreEqual("y", rightResult.Identifier);
        }

        [Test]
        [TestCase("/", TokenType.Multiply)]
        [TestCase("*", TokenType.Divide)]
        [TestCase("%", TokenType.Remainder)]
        public void Should_ParseLeftAssociative_When_GivenMultiply(string value, TokenType token)
        {
            var tokenStream = ParseTestUtilities.CreateTokenStream(
                new Token("x", TokenType.Identifier, new Position(0, 0), new Position(0, 0)),
                new Token(value, token, new Position(0, 0), new Position(0, 0)),
                new Token("y", TokenType.Identifier, new Position(0, 0), new Position(0, 0)),
                new Token(value, token, new Position(0, 0), new Position(0, 0)),
                new Token("z", TokenType.Identifier, new Position(0, 0), new Position(0, 0)));

            var parser = new Parser("formula", tokenStream);

            var result = ParserAssert.Parses<BinaryOpNode>(() => parser.Multiply());
            ParserAssert.HasProperty<BinaryOpNode>(result, "Left");
            ParserAssert.HasProperty<IdentifierNode>(result, "Right");
        }

        [Test]
        public void Should_PassThroughPower_When_NotGivenPower()
        {
            var tokenStream = ParseTestUtilities.CreateTokenStream(
                new Token("x", TokenType.Identifier, new Position(0, 0), new Position(0, 0)));

            var parser = new Parser("formula", tokenStream);

            ParserAssert.Parses<IdentifierNode>(() => parser.Power());
        }

        [Test]
        [TestCase("^", TokenType.Power, BinaryOpType.Power)]
        public void Should_ParseBinaryOpNode_When_GivenPower(string value, TokenType token, BinaryOpType op)
        {
            var tokenStream = ParseTestUtilities.CreateTokenStream(
                new Token("x", TokenType.Identifier, new Position(0, 0), new Position(0, 0)),
                new Token(value, token, new Position(0, 0), new Position(0, 0)),
                new Token("y", TokenType.Identifier, new Position(0, 0), new Position(0, 0)));

            var parser = new Parser("formula", tokenStream);

            var result = ParserAssert.Parses<BinaryOpNode>(() => parser.Power());
            Assert.AreEqual(op, result.OpType);

            var leftResult = ParserAssert.HasProperty<IdentifierNode>(result, "Left");
            Assert.AreEqual("x", leftResult.Identifier);

            var rightResult = ParserAssert.HasProperty<IdentifierNode>(result, "Right");
            Assert.AreEqual("y", rightResult.Identifier);
        }

        [Test]
        [TestCase("^", TokenType.Power)]
        public void Should_ParseRightAssociative_When_GivenPower(string value, TokenType token)
        {
            var tokenStream = ParseTestUtilities.CreateTokenStream(
                new Token("x", TokenType.Identifier, new Position(0, 0), new Position(0, 0)),
                new Token(value, token, new Position(0, 0), new Position(0, 0)),
                new Token("y", TokenType.Identifier, new Position(0, 0), new Position(0, 0)),
                new Token(value, token, new Position(0, 0), new Position(0, 0)),
                new Token("z", TokenType.Identifier, new Position(0, 0), new Position(0, 0)));

            var parser = new Parser("formula", tokenStream);

            var result = ParserAssert.Parses<BinaryOpNode>(() => parser.Power());
            ParserAssert.HasProperty<IdentifierNode>(result, "Left");
            ParserAssert.HasProperty<BinaryOpNode>(result, "Right");
        }
    }
}