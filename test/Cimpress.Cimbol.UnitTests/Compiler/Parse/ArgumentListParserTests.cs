// Copyright 2021 Cimpress plc.
// Licensed under the Apache License, Version 2.0
// http://www.apache.org/licenses/LICENSE-2.0

using System.Linq;
using Cimpress.Cimbol.Compiler.Parse;
using Cimpress.Cimbol.Compiler.Scan;
using Cimpress.Cimbol.Compiler.SyntaxTree;
using Cimpress.Cimbol.Exceptions;
using Cimpress.Cimbol.Utilities;
using NUnit.Framework;

namespace Cimpress.Cimbol.UnitTests.Compiler.Parse
{
    [TestFixture]
    public class ArgumentListParserTests
    {
        [Test]
        public void Should_ParseArgumentList_When_GivenNoArguments()
        {
            var tokenStream = ParseTestUtilities.CreateTokenStream(
                new Token("(", TokenType.LeftParenthesis, new Position(0, 0), new Position(0, 0)),
                new Token(")", TokenType.RightParenthesis, new Position(0, 0), new Position(0, 0)));

            var parser = new Parser("formula", tokenStream);

            var result = parser.ArgumentList()?.ToList();
            Assert.IsNotNull(result);
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void Should_ParseArgumentList_When_GivenOneArgument()
        {
            var tokenStream = ParseTestUtilities.CreateTokenStream(
                new Token("(", TokenType.LeftParenthesis, new Position(0, 0), new Position(0, 0)),
                new Token("x", TokenType.Identifier, new Position(0, 0), new Position(0, 0)),
                new Token(")", TokenType.RightParenthesis, new Position(0, 0), new Position(0, 0)));

            var parser = new Parser("formula", tokenStream);

            var result = parser.ArgumentList()?.ToList();
            Assert.IsNotNull(result);
            Assert.That(result, Has.Count.EqualTo(1));

            var innerResult1 = result[0];
            Assert.IsNotNull(innerResult1);
            Assert.IsInstanceOf<PositionalArgument>(innerResult1);
        }

        [Test]
        public void Should_ParseArgumentList_When_GivenTwoArguments()
        {
            var tokenStream = ParseTestUtilities.CreateTokenStream(
                new Token("(", TokenType.LeftParenthesis, new Position(0, 0), new Position(0, 0)),
                new Token("x", TokenType.Identifier, new Position(0, 0), new Position(0, 0)),
                new Token(",", TokenType.Comma, new Position(0, 0), new Position(0, 0)),
                new Token("y", TokenType.Identifier, new Position(0, 0), new Position(0, 0)),
                new Token(")", TokenType.RightParenthesis, new Position(0, 0), new Position(0, 0)));

            var parser = new Parser("formula", tokenStream);

            var result = parser.ArgumentList()?.ToList();
            Assert.IsNotNull(result);
            Assert.That(result, Has.Count.EqualTo(2));

            var innerResult1 = result[0];
            Assert.IsNotNull(innerResult1);
            Assert.IsInstanceOf<PositionalArgument>(innerResult1);

            var innerResult2 = result[1];
            Assert.IsNotNull(innerResult2);
            Assert.IsInstanceOf<PositionalArgument>(innerResult2);
        }

        [Test]
        public void Should_ParseNamedArgumentList_When_GivenNoArguments()
        {
            var tokenStream = ParseTestUtilities.CreateTokenStream(
                new Token("(", TokenType.LeftParenthesis, new Position(0, 0), new Position(0, 0)),
                new Token(")", TokenType.RightParenthesis, new Position(0, 0), new Position(0, 0)));

            var parser = new Parser("formula", tokenStream);

            var result = parser.NamedArgumentList()?.ToList();
            Assert.IsNotNull(result);
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void Should_ParseNamedArgumentList_When_GivenOnePositionArgument()
        {
            var tokenStream = ParseTestUtilities.CreateTokenStream(
                new Token("(", TokenType.LeftParenthesis, new Position(0, 0), new Position(0, 0)),
                new Token("x", TokenType.Identifier, new Position(0, 0), new Position(0, 0)),
                new Token(")", TokenType.RightParenthesis, new Position(0, 0), new Position(0, 0)));

            var parser = new Parser("formula", tokenStream);

            var result = parser.NamedArgumentList()?.ToList();
            Assert.IsNotNull(result);
            Assert.That(result, Has.Count.EqualTo(1));

            var innerResult1 = result[0];
            Assert.IsNotNull(innerResult1);
            Assert.IsInstanceOf<PositionalArgument>(innerResult1);
        }

        [Test]
        public void Should_ParseNamedArgumentList_When_GivenTwoPositionArguments()
        {
            var tokenStream = ParseTestUtilities.CreateTokenStream(
                new Token("(", TokenType.LeftParenthesis, new Position(0, 0), new Position(0, 0)),
                new Token("x", TokenType.Identifier, new Position(0, 0), new Position(0, 0)),
                new Token(",", TokenType.Comma, new Position(0, 0), new Position(0, 0)),
                new Token("y", TokenType.Identifier, new Position(0, 0), new Position(0, 0)),
                new Token(")", TokenType.RightParenthesis, new Position(0, 0), new Position(0, 0)));

            var parser = new Parser("formula", tokenStream);

            var result = parser.NamedArgumentList()?.ToList();
            Assert.IsNotNull(result);
            Assert.That(result, Has.Count.EqualTo(2));

            var innerResult1 = result[0];
            Assert.IsNotNull(innerResult1);
            Assert.IsInstanceOf<PositionalArgument>(innerResult1);

            var innerResult2 = result[1];
            Assert.IsNotNull(innerResult2);
            Assert.IsInstanceOf<PositionalArgument>(innerResult2);
        }

        [Test]
        public void Should_ParseNamedArgumentList_When_GivenOneNamedArgument()
        {
            var tokenStream = ParseTestUtilities.CreateTokenStream(
                new Token("(", TokenType.LeftParenthesis, new Position(0, 0), new Position(0, 0)),
                new Token("a", TokenType.Identifier, new Position(0, 0), new Position(0, 0)),
                new Token("=", TokenType.Assign, new Position(0, 0), new Position(0, 0)),
                new Token("x", TokenType.Identifier, new Position(0, 0), new Position(0, 0)),
                new Token(")", TokenType.RightParenthesis, new Position(0, 0), new Position(0, 0)));

            var parser = new Parser("formula", tokenStream);

            var result = parser.NamedArgumentList()?.ToList();
            Assert.IsNotNull(result);
            Assert.That(result, Has.Count.EqualTo(1));

            var innerResult1 = result[0] as NamedArgument;
            Assert.IsNotNull(innerResult1);
            Assert.IsInstanceOf<NamedArgument>(innerResult1);
            Assert.AreEqual("a", innerResult1.Name);
        }

        [Test]
        public void Should_ParseNamedArgumentList_When_GivenTwoNamedArguments()
        {
            var tokenStream = ParseTestUtilities.CreateTokenStream(
                new Token("(", TokenType.LeftParenthesis, new Position(0, 0), new Position(0, 0)),
                new Token("a", TokenType.Identifier, new Position(0, 0), new Position(0, 0)),
                new Token("=", TokenType.Assign, new Position(0, 0), new Position(0, 0)),
                new Token("x", TokenType.Identifier, new Position(0, 0), new Position(0, 0)),
                new Token(",", TokenType.Comma, new Position(0, 0), new Position(0, 0)),
                new Token("b", TokenType.Identifier, new Position(0, 0), new Position(0, 0)),
                new Token("=", TokenType.Assign, new Position(0, 0), new Position(0, 0)),
                new Token("y", TokenType.Identifier, new Position(0, 0), new Position(0, 0)),
                new Token(")", TokenType.RightParenthesis, new Position(0, 0), new Position(0, 0)));

            var parser = new Parser("formula", tokenStream);

            var result = parser.NamedArgumentList()?.ToList();
            Assert.IsNotNull(result);
            Assert.That(result, Has.Count.EqualTo(2));

            var innerResult1 = result[0] as NamedArgument;
            Assert.IsNotNull(innerResult1);
            Assert.IsInstanceOf<NamedArgument>(innerResult1);
            Assert.AreEqual("a", innerResult1.Name);

            var innerResult2 = result[1] as NamedArgument;
            Assert.IsNotNull(innerResult2);
            Assert.IsInstanceOf<NamedArgument>(innerResult2);
            Assert.AreEqual("b", innerResult2.Name);
        }

        [Test]
        public void Should_ParseNamedArgumentList_When_GivenPositionalThenNamedArguments()
        {
            var tokenStream = ParseTestUtilities.CreateTokenStream(
                new Token("(", TokenType.LeftParenthesis, new Position(0, 0), new Position(0, 0)),
                new Token("x", TokenType.Identifier, new Position(0, 0), new Position(0, 0)),
                new Token(",", TokenType.Comma, new Position(0, 0), new Position(0, 0)),
                new Token("b", TokenType.Identifier, new Position(0, 0), new Position(0, 0)),
                new Token("=", TokenType.Assign, new Position(0, 0), new Position(0, 0)),
                new Token("y", TokenType.Identifier, new Position(0, 0), new Position(0, 0)),
                new Token(")", TokenType.RightParenthesis, new Position(0, 0), new Position(0, 0)));

            var parser = new Parser("formula", tokenStream);

            var result = parser.NamedArgumentList()?.ToList();
            Assert.IsNotNull(result);
            Assert.That(result, Has.Count.EqualTo(2));

            var innerResult1 = result[0] as PositionalArgument;
            Assert.IsNotNull(innerResult1);
            Assert.IsInstanceOf<PositionalArgument>(innerResult1);

            var innerResult2 = result[1] as NamedArgument;
            Assert.IsNotNull(innerResult2);
            Assert.IsInstanceOf<NamedArgument>(innerResult2);
            Assert.AreEqual("b", innerResult2.Name);
        }

        [Test]
        public void Should_ParseNamedArgumentList_When_GivenNamedThenPositionalArguments()
        {
            var tokenStream = ParseTestUtilities.CreateTokenStream(
                new Token("(", TokenType.LeftParenthesis, new Position(0, 0), new Position(0, 0)),
                new Token("a", TokenType.Identifier, new Position(0, 0), new Position(0, 0)),
                new Token("=", TokenType.Assign, new Position(0, 0), new Position(0, 0)),
                new Token("x", TokenType.Identifier, new Position(0, 0), new Position(0, 0)),
                new Token(",", TokenType.Comma, new Position(0, 0), new Position(0, 0)),
                new Token("y", TokenType.Identifier, new Position(0, 0), new Position(0, 0)),
                new Token(")", TokenType.RightParenthesis, new Position(0, 0), new Position(0, 0)));

            var parser = new Parser("formula", tokenStream);

            var result = parser.NamedArgumentList()?.ToList();
            Assert.IsNotNull(result);
            Assert.That(result, Has.Count.EqualTo(2));

            var innerResult1 = result[0] as NamedArgument;
            Assert.IsNotNull(innerResult1);
            Assert.IsInstanceOf<NamedArgument>(innerResult1);
            Assert.AreEqual("a", innerResult1.Name);

            var innerResult2 = result[1] as PositionalArgument;
            Assert.IsNotNull(innerResult2);
            Assert.IsInstanceOf<PositionalArgument>(innerResult2);
        }

        [Test]
        public void ShouldNot_ParseArgumentList_When_MissingLeftParenthesis()
        {
            var tokenStream = ParseTestUtilities.CreateTokenStream(
                new Token("x", TokenType.Identifier, new Position(0, 0), new Position(0, 0)),
                new Token(",", TokenType.Comma, new Position(0, 0), new Position(0, 0)),
                new Token("y", TokenType.Identifier, new Position(0, 0), new Position(0, 0)),
                new Token(")", TokenType.RightParenthesis, new Position(0, 0), new Position(0, 0)));

            var parser = new Parser("formula", tokenStream);

            Assert.Throws<CimbolCompilationException>(() => parser.ArgumentList()?.ToList());
        }

        [Test]
        public void ShouldNot_ParseArgumentList_When_MissingFirstArgument()
        {
            var tokenStream = ParseTestUtilities.CreateTokenStream(
                new Token("(", TokenType.LeftParenthesis, new Position(0, 0), new Position(0, 0)),
                new Token(",", TokenType.Comma, new Position(0, 0), new Position(0, 0)),
                new Token("y", TokenType.Identifier, new Position(0, 0), new Position(0, 0)),
                new Token(")", TokenType.RightParenthesis, new Position(0, 0), new Position(0, 0)));

            var parser = new Parser("formula", tokenStream);

            Assert.Throws<CimbolCompilationException>(() => parser.ArgumentList()?.ToList());
        }

        [Test]
        public void ShouldNot_ParseArgumentList_When_MissingComma()
        {
            var tokenStream = ParseTestUtilities.CreateTokenStream(
                new Token("(", TokenType.LeftParenthesis, new Position(0, 0), new Position(0, 0)),
                new Token("x", TokenType.Identifier, new Position(0, 0), new Position(0, 0)),
                new Token("y", TokenType.Identifier, new Position(0, 0), new Position(0, 0)),
                new Token(")", TokenType.RightParenthesis, new Position(0, 0), new Position(0, 0)));

            var parser = new Parser("formula", tokenStream);

            Assert.Throws<CimbolCompilationException>(() => parser.ArgumentList()?.ToList());
        }

        [Test]
        public void ShouldNot_ParseArgumentList_When_MissingSecondArgument()
        {
            var tokenStream = ParseTestUtilities.CreateTokenStream(
                new Token("(", TokenType.LeftParenthesis, new Position(0, 0), new Position(0, 0)),
                new Token("x", TokenType.Identifier, new Position(0, 0), new Position(0, 0)),
                new Token(",", TokenType.Comma, new Position(0, 0), new Position(0, 0)),
                new Token(")", TokenType.RightParenthesis, new Position(0, 0), new Position(0, 0)));

            var parser = new Parser("formula", tokenStream);

            Assert.Throws<CimbolCompilationException>(() => parser.ArgumentList()?.ToList());
        }

        [Test]
        public void ShouldNot_ParseArgumentList_When_MissingRightParenthesis()
        {
            var tokenStream = ParseTestUtilities.CreateTokenStream(
                new Token("(", TokenType.LeftParenthesis, new Position(0, 0), new Position(0, 0)),
                new Token("x", TokenType.Identifier, new Position(0, 0), new Position(0, 0)),
                new Token(",", TokenType.Comma, new Position(0, 0), new Position(0, 0)),
                new Token("y", TokenType.Identifier, new Position(0, 0), new Position(0, 0)));

            var parser = new Parser("formula", tokenStream);

            Assert.Throws<CimbolCompilationException>(() => parser.ArgumentList()?.ToList());
        }

        [Test]
        public void ShouldNot_ParseArgumentList_When_GivenNamedArgument()
        {
            var tokenStream = ParseTestUtilities.CreateTokenStream(
                new Token("(", TokenType.LeftParenthesis, new Position(0, 0), new Position(0, 0)),
                new Token("a", TokenType.Identifier, new Position(0, 0), new Position(0, 0)),
                new Token("=", TokenType.Assign, new Position(0, 0), new Position(0, 0)),
                new Token("x", TokenType.Identifier, new Position(0, 0), new Position(0, 0)),
                new Token(")", TokenType.Identifier, new Position(0, 0), new Position(0, 0)));

            var parser = new Parser("formula", tokenStream);

            Assert.Throws<CimbolCompilationException>(() => parser.ArgumentList()?.ToList());
        }

        [Test]
        public void ShouldNot_ParseNamedArgumentList_When_MissingLeftParenthesis()
        {
            var tokenStream = ParseTestUtilities.CreateTokenStream(
                new Token("(", TokenType.LeftParenthesis, new Position(0, 0), new Position(0, 0)),
                new Token("x", TokenType.Identifier, new Position(0, 0), new Position(0, 0)),
                new Token(",", TokenType.Comma, new Position(0, 0), new Position(0, 0)),
                new Token("b", TokenType.Identifier, new Position(0, 0), new Position(0, 0)),
                new Token("=", TokenType.Assign, new Position(0, 0), new Position(0, 0)),
                new Token("y", TokenType.Identifier, new Position(0, 0), new Position(0, 0)),
                new Token(")", TokenType.LeftParenthesis, new Position(0, 0), new Position(0, 0)));

            var parser = new Parser("formula", tokenStream);

            Assert.Throws<CimbolCompilationException>(() => parser.ArgumentList()?.ToList());
        }

        [Test]
        public void ShouldNot_ParseNamedArgumentList_When_MissingPositionalArgument()
        {
            var tokenStream = ParseTestUtilities.CreateTokenStream(
                new Token("(", TokenType.LeftParenthesis, new Position(0, 0), new Position(0, 0)),
                new Token(",", TokenType.Comma, new Position(0, 0), new Position(0, 0)),
                new Token("b", TokenType.Identifier, new Position(0, 0), new Position(0, 0)),
                new Token("=", TokenType.Assign, new Position(0, 0), new Position(0, 0)),
                new Token("y", TokenType.Identifier, new Position(0, 0), new Position(0, 0)),
                new Token(")", TokenType.LeftParenthesis, new Position(0, 0), new Position(0, 0)));

            var parser = new Parser("formula", tokenStream);

            Assert.Throws<CimbolCompilationException>(() => parser.ArgumentList()?.ToList());
        }

        [Test]
        public void ShouldNot_ParseNamedArgumentList_When_MissingComma()
        {
            var tokenStream = ParseTestUtilities.CreateTokenStream(
                new Token("(", TokenType.LeftParenthesis, new Position(0, 0), new Position(0, 0)),
                new Token("x", TokenType.Identifier, new Position(0, 0), new Position(0, 0)),
                new Token("b", TokenType.Identifier, new Position(0, 0), new Position(0, 0)),
                new Token("=", TokenType.Assign, new Position(0, 0), new Position(0, 0)),
                new Token("y", TokenType.Identifier, new Position(0, 0), new Position(0, 0)),
                new Token(")", TokenType.LeftParenthesis, new Position(0, 0), new Position(0, 0)));

            var parser = new Parser("formula", tokenStream);

            Assert.Throws<CimbolCompilationException>(() => parser.ArgumentList()?.ToList());
        }

        [Test]
        public void ShouldNot_ParseNamedArgumentList_When_MissingNamedArgumentName()
        {
            var tokenStream = ParseTestUtilities.CreateTokenStream(
                new Token("(", TokenType.LeftParenthesis, new Position(0, 0), new Position(0, 0)),
                new Token("x", TokenType.Identifier, new Position(0, 0), new Position(0, 0)),
                new Token(",", TokenType.Comma, new Position(0, 0), new Position(0, 0)),
                new Token("=", TokenType.Assign, new Position(0, 0), new Position(0, 0)),
                new Token("y", TokenType.Identifier, new Position(0, 0), new Position(0, 0)),
                new Token(")", TokenType.LeftParenthesis, new Position(0, 0), new Position(0, 0)));

            var parser = new Parser("formula", tokenStream);

            Assert.Throws<CimbolCompilationException>(() => parser.ArgumentList()?.ToList());
        }

        [Test]
        public void ShouldNot_ParseNamedArgumentList_When_MissingNamedArgumentEqualSign()
        {
            var tokenStream = ParseTestUtilities.CreateTokenStream(
                new Token("(", TokenType.LeftParenthesis, new Position(0, 0), new Position(0, 0)),
                new Token("x", TokenType.Identifier, new Position(0, 0), new Position(0, 0)),
                new Token(",", TokenType.Comma, new Position(0, 0), new Position(0, 0)),
                new Token("b", TokenType.Identifier, new Position(0, 0), new Position(0, 0)),
                new Token("y", TokenType.Identifier, new Position(0, 0), new Position(0, 0)),
                new Token(")", TokenType.LeftParenthesis, new Position(0, 0), new Position(0, 0)));

            var parser = new Parser("formula", tokenStream);

            Assert.Throws<CimbolCompilationException>(() => parser.ArgumentList()?.ToList());
        }

        [Test]
        public void ShouldNot_ParseNamedArgumentList_When_MissingNamedArgumentValue()
        {
            var tokenStream = ParseTestUtilities.CreateTokenStream(
                new Token("(", TokenType.LeftParenthesis, new Position(0, 0), new Position(0, 0)),
                new Token("x", TokenType.Identifier, new Position(0, 0), new Position(0, 0)),
                new Token(",", TokenType.Comma, new Position(0, 0), new Position(0, 0)),
                new Token("b", TokenType.Identifier, new Position(0, 0), new Position(0, 0)),
                new Token("=", TokenType.Assign, new Position(0, 0), new Position(0, 0)),
                new Token(")", TokenType.LeftParenthesis, new Position(0, 0), new Position(0, 0)));

            var parser = new Parser("formula", tokenStream);

            Assert.Throws<CimbolCompilationException>(() => parser.ArgumentList()?.ToList());
        }

        [Test]
        public void ShouldNot_ParseNamedArgumentList_When_MissingRightParenthesis()
        {
            var tokenStream = ParseTestUtilities.CreateTokenStream(
                new Token("(", TokenType.LeftParenthesis, new Position(0, 0), new Position(0, 0)),
                new Token("x", TokenType.Identifier, new Position(0, 0), new Position(0, 0)),
                new Token(",", TokenType.Comma, new Position(0, 0), new Position(0, 0)),
                new Token("b", TokenType.Identifier, new Position(0, 0), new Position(0, 0)),
                new Token("=", TokenType.Assign, new Position(0, 0), new Position(0, 0)),
                new Token("y", TokenType.Identifier, new Position(0, 0), new Position(0, 0)));

            var parser = new Parser("formula", tokenStream);

            Assert.Throws<CimbolCompilationException>(() => parser.ArgumentList()?.ToList());
        }
    }
}