// Copyright 2021 Cimpress plc.
// Licensed under the Apache License, Version 2.0
// http://www.apache.org/licenses/LICENSE-2.0

using System;
using Cimpress.Cimbol.Compiler.Parse;
using Cimpress.Cimbol.Compiler.Scan;
using Cimpress.Cimbol.Compiler.SyntaxTree;
using Cimpress.Cimbol.Exceptions;
using Cimpress.Cimbol.Utilities;
using NUnit.Framework;

namespace Cimpress.Cimbol.UnitTests.Compiler.Parse
{
    [TestFixture]
    public class ParserTests
    {
        [Test]
        public void ShouldNot_ConstructParser_When_GivenNullTokenStream()
        {
            Assert.Throws<ArgumentNullException>(() => new Parser("formula", null));
        }

        [Test]
        public void Should_ThrowExpectedException_When_LookingPastLastToken()
        {
            var tokenStream = ParseTestUtilities.CreateTokenStream(
                new Token("if", TokenType.IfKeyword, new Position(0, 0), new Position(0, 0)),
                new Token("(", TokenType.LeftParenthesis, new Position(0, 0), new Position(0, 0)));

            var parser = new Parser("formula", tokenStream);

            Assert.Throws<CimbolCompilationException>(() => parser.Root());
        }

        [Test]
        public void Should_ParseRoot_When_GivenCompleteExpression()
        {
            var tokenStream = ParseTestUtilities.CreateTokenStream(
                new Token("(", TokenType.LeftParenthesis, new Position(0, 0), new Position(0, 0)),
                new Token("x", TokenType.Identifier, new Position(0, 0), new Position(0, 0)),
                new Token("+", TokenType.Add, new Position(0, 0), new Position(0, 0)),
                new Token("y", TokenType.Identifier, new Position(0, 0), new Position(0, 0)),
                new Token(")", TokenType.RightParenthesis, new Position(0, 0), new Position(0, 0)),
                new Token("/", TokenType.Divide, new Position(0, 0), new Position(0, 0)),
                new Token("z", TokenType.Identifier, new Position(0, 0), new Position(0, 0)));

            var parser = new Parser("formula", tokenStream);

            var result = ParserAssert.Parses<BinaryOpNode>(() => parser.Root());
            Assert.AreEqual(BinaryOpType.Divide, result.OpType);

            var leftResult = ParserAssert.HasProperty<BinaryOpNode>(result, "Left");
            Assert.AreEqual(BinaryOpType.Add, leftResult.OpType);

            var rightResult = ParserAssert.HasProperty<IdentifierNode>(result, "Right");
            Assert.AreEqual("z", rightResult.Identifier);

            var leftLeftResult = ParserAssert.HasProperty<IdentifierNode>(leftResult, "Left");
            Assert.AreEqual("x", leftLeftResult.Identifier);

            var leftRightResult = ParserAssert.HasProperty<IdentifierNode>(leftResult, "Right");
            Assert.AreEqual("y", leftRightResult.Identifier);
        }

        [Test]
        public void ShouldNot_ParseRoot_When_GivenNoExpression()
        {
            var tokenStream = ParseTestUtilities.CreateTokenStream();

            var parser = new Parser("formula", tokenStream);

            Assert.Throws<CimbolCompilationException>(() => parser.Root());
        }

        [Test]
        public void ShouldNot_ParseRoot_When_GivenTwoExpressions()
        {
            var tokenStream = ParseTestUtilities.CreateTokenStream(
                new Token("(", TokenType.LeftParenthesis, new Position(0, 0), new Position(0, 0)),
                new Token("x", TokenType.Identifier, new Position(0, 0), new Position(0, 0)),
                new Token("+", TokenType.Add, new Position(0, 0), new Position(0, 0)),
                new Token("y", TokenType.Identifier, new Position(0, 0), new Position(0, 0)),
                new Token(")", TokenType.RightParenthesis, new Position(0, 0), new Position(0, 0)),
                new Token("z", TokenType.Identifier, new Position(0, 0), new Position(0, 0)));

            var parser = new Parser("formula", tokenStream);

            Assert.Throws<CimbolCompilationException>(() => parser.Root());
        }
    }
}