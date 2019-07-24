using System;
using Cimpress.Cimbol.Compiler.Parse;
using Cimpress.Cimbol.Compiler.Scan;
using Cimpress.Cimbol.Compiler.SyntaxTree;
using Cimpress.Cimbol.Compiler.Utilities;
using NUnit.Framework;

namespace Cimpress.Cimbol.UnitTests.Compiler.Parse
{
    [TestFixture]
    public class InvokeParserTests
    {
        [Test]
        public void Should_PassThrough_When_NotInvokeOrAccess()
        {
            var tokenStream = ParseTestUtilities.CreateTokenStream(
                new Token("x", TokenType.Identifier, new Position(0, 0), new Position(0, 0)));

            var parser = new Parser(tokenStream);

            var result = parser.Invoke();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IdentifierNode>(result);
        }

        [Test]
        [TestCase("if", TokenType.IfKeyword)]
        [TestCase("list", TokenType.ListKeyword)]
        [TestCase("object", TokenType.ObjectKeyword)]
        [TestCase("where", TokenType.WhereKeyword)]
        public void Should_ParseMacroNode_When_GivenMacro(string macroName, TokenType tokenType)
        {
            var tokenStream = ParseTestUtilities.CreateTokenStream(
                new Token(macroName, tokenType, new Position(0, 0), new Position(0, 0)),
                new Token("(", TokenType.LeftParenthesis, new Position(0, 0), new Position(0, 0)),
                new Token("x", TokenType.Identifier, new Position(0, 0), new Position(0, 0)),
                new Token(")", TokenType.RightParenthesis, new Position(0, 0), new Position(0, 0)));

            var parser = new Parser(tokenStream);

            var result = parser.Invoke();
            Assert.IsInstanceOf<MacroNode>(result);
            var macroResult = result as MacroNode;
            Assert.IsNotNull(macroResult);
            Assert.AreEqual(macroName, macroResult.Macro);
            Assert.That(macroResult.Arguments, Has.Length.EqualTo(1));
        }

        [Test]
        [TestCase("if", TokenType.IfKeyword)]
        [TestCase("list", TokenType.ListKeyword)]
        [TestCase("object", TokenType.ObjectKeyword)]
        [TestCase("where", TokenType.WhereKeyword)]
        public void Should_ParseMacroNode_When_GivenMacroWithNamedArguments(string macroName, TokenType tokenType)
        {
            var tokenStream = ParseTestUtilities.CreateTokenStream(
                new Token(macroName, tokenType, new Position(0, 0), new Position(0, 0)),
                new Token("(", TokenType.LeftParenthesis, new Position(0, 0), new Position(0, 0)),
                new Token("a", TokenType.Identifier, new Position(0, 0), new Position(0, 0)),
                new Token("=", TokenType.Assign, new Position(0, 0), new Position(0, 0)),
                new Token("x", TokenType.Identifier, new Position(0, 0), new Position(0, 0)),
                new Token(")", TokenType.RightParenthesis, new Position(0, 0), new Position(0, 0)));

            var parser = new Parser(tokenStream);

            var result = parser.Invoke();
            Assert.IsInstanceOf<MacroNode>(result);
            var macroResult = result as MacroNode;
            Assert.IsNotNull(macroResult);
            Assert.AreEqual(macroName, macroResult.Macro);
            Assert.That(macroResult.Arguments, Has.Length.EqualTo(1));
        }

        [Test]
        public void Should_ParseAccessNode_When_GivenSingleAccess()
        {
            var tokenStream = ParseTestUtilities.CreateTokenStream(
                new Token("x", TokenType.Identifier, new Position(0, 0), new Position(0, 0)),
                new Token(".", TokenType.Period, new Position(0, 0), new Position(0, 0)),
                new Token("y", TokenType.Identifier, new Position(0, 0), new Position(0, 0)));

            var parser = new Parser(tokenStream);

            var result = parser.Invoke() as AccessNode;
            Assert.IsNotNull(result);
            Assert.AreEqual("y", result.Member);

            var innerResult1 = result.Value as IdentifierNode;
            Assert.IsNotNull(innerResult1);
        }

        [Test]
        public void Should_ParseAccessNode_When_GivenDoubleAccess()
        {
            var tokenStream = ParseTestUtilities.CreateTokenStream(
                new Token("x", TokenType.Identifier, new Position(0, 0), new Position(0, 0)),
                new Token(".", TokenType.Period, new Position(0, 0), new Position(0, 0)),
                new Token("y", TokenType.Identifier, new Position(0, 0), new Position(0, 0)),
                new Token(".", TokenType.Period, new Position(0, 0), new Position(0, 0)),
                new Token("z", TokenType.Identifier, new Position(0, 0), new Position(0, 0)));

            var parser = new Parser(tokenStream);

            var result = parser.Invoke() as AccessNode;
            Assert.IsNotNull(result);
            Assert.AreEqual("z", result.Member);

            var innerResult1 = result.Value as AccessNode;
            Assert.IsNotNull(innerResult1);
            Assert.AreEqual("y", innerResult1.Member);

            var innerResult2 = innerResult1.Value as IdentifierNode;
            Assert.IsNotNull(innerResult2);
        }

        [Test]
        public void Should_ParseInvokeNode_When_GivenSingleFunctionInvocation()
        {
            var tokenStream = ParseTestUtilities.CreateTokenStream(
                new Token("x", TokenType.Identifier, new Position(0, 0), new Position(0, 0)),
                new Token("(", TokenType.LeftParenthesis, new Position(0, 0), new Position(0, 0)),
                new Token("y", TokenType.Identifier, new Position(0, 0), new Position(0, 0)),
                new Token(")", TokenType.RightParenthesis, new Position(0, 0), new Position(0, 0)));

            var parser = new Parser(tokenStream);

            var result = parser.Invoke();
            Assert.IsInstanceOf<InvokeNode>(result);
            var invokeResult = result as InvokeNode;
            Assert.IsNotNull(invokeResult);
            Assert.That(invokeResult.Arguments, Has.Length.EqualTo(1));

            var innerResult1 = invokeResult.Function as IdentifierNode;
            Assert.IsNotNull(innerResult1);
        }

        [Test]
        public void Should_ParseInvokeNode_When_GivenDoubleFunctionInvocation()
        {
            var tokenStream = ParseTestUtilities.CreateTokenStream(
                new Token("x", TokenType.Identifier, new Position(0, 0), new Position(0, 0)),
                new Token("(", TokenType.LeftParenthesis, new Position(0, 0), new Position(0, 0)),
                new Token("y", TokenType.Identifier, new Position(0, 0), new Position(0, 0)),
                new Token(")", TokenType.RightParenthesis, new Position(0, 0), new Position(0, 0)),
                new Token("(", TokenType.LeftParenthesis, new Position(0, 0), new Position(0, 0)),
                new Token("z", TokenType.Identifier, new Position(0, 0), new Position(0, 0)),
                new Token(")", TokenType.RightParenthesis, new Position(0, 0), new Position(0, 0)));

            var parser = new Parser(tokenStream);

            var result = parser.Invoke();
            Assert.IsInstanceOf<InvokeNode>(result);
            var invokeResult = result as InvokeNode;
            Assert.IsNotNull(invokeResult);
            Assert.That(invokeResult.Arguments, Has.Length.EqualTo(1));

            var innerResult1 = invokeResult.Function;
            Assert.IsInstanceOf<InvokeNode>(innerResult1);
            var invokeInnerResult1 = innerResult1 as InvokeNode;
            Assert.IsNotNull(invokeInnerResult1);
            Assert.That(invokeInnerResult1.Arguments, Has.Length.EqualTo(1));

            var innerResult2 = invokeInnerResult1.Function as IdentifierNode;
            Assert.IsNotNull(innerResult2);
        }

        [Test]
        public void Should_ParseInvokeAndAccessNodes_When_GivenAccessFollowedByFunctionInvocation()
        {
            var tokenStream = ParseTestUtilities.CreateTokenStream(
                new Token("x", TokenType.Identifier, new Position(0, 0), new Position(0, 0)),
                new Token(".", TokenType.Period, new Position(0, 0), new Position(0, 0)),
                new Token("y", TokenType.Identifier, new Position(0, 0), new Position(0, 0)),
                new Token("(", TokenType.LeftParenthesis, new Position(0, 0), new Position(0, 0)),
                new Token("z", TokenType.Identifier, new Position(0, 0), new Position(0, 0)),
                new Token(")", TokenType.RightParenthesis, new Position(0, 0), new Position(0, 0)));

            var parser = new Parser(tokenStream);

            var result = parser.Invoke();
            Assert.IsInstanceOf<InvokeNode>(result);
            var invokeResult = result as InvokeNode;
            Assert.IsNotNull(invokeResult);
            Assert.That(invokeResult.Arguments, Has.Length.EqualTo(1));

            var innerResult1 = invokeResult.Function;
            Assert.IsInstanceOf<AccessNode>(innerResult1);
            var accessInnerResult1 = innerResult1 as AccessNode;
            Assert.IsNotNull(accessInnerResult1);
            Assert.AreEqual("y", accessInnerResult1.Member);

            var innerResult2 = accessInnerResult1.Value;
            var identifierInnerResult2 = innerResult2 as IdentifierNode;
            Assert.IsNotNull(identifierInnerResult2);
        }

        [Test]
        public void Should_ParseInvokeAndAccessNodes_When_GivenFunctionInvocationFollowedByAccess()
        {
            var tokenStream = ParseTestUtilities.CreateTokenStream(
                new Token("x", TokenType.Identifier, new Position(0, 0), new Position(0, 0)),
                new Token("(", TokenType.LeftParenthesis, new Position(0, 0), new Position(0, 0)),
                new Token("y", TokenType.Identifier, new Position(0, 0), new Position(0, 0)),
                new Token(")", TokenType.RightParenthesis, new Position(0, 0), new Position(0, 0)),
                new Token(".", TokenType.Period, new Position(0, 0), new Position(0, 0)),
                new Token("z", TokenType.Identifier, new Position(0, 0), new Position(0, 0)));

            var parser = new Parser(tokenStream);

            var result = parser.Invoke();
            Assert.IsInstanceOf<AccessNode>(result);
            var accessResult = result as AccessNode;
            Assert.IsNotNull(accessResult);
            Assert.AreEqual("z", accessResult.Member);

            var innerResult1 = accessResult.Value;
            Assert.IsInstanceOf<InvokeNode>(innerResult1);
            var invokeInnerResult1 = innerResult1 as InvokeNode;
            Assert.IsNotNull(invokeInnerResult1);
            Assert.That(invokeInnerResult1.Arguments, Has.Length.EqualTo(1));

            var innerResult2 = invokeInnerResult1.Function;
            var identifierInnerResult2 = innerResult2 as IdentifierNode;
            Assert.IsNotNull(identifierInnerResult2);
        }

        [Test]
        public void ShouldNot_ParseAccessNode_When_MissingMember()
        {
            var tokenStream = ParseTestUtilities.CreateTokenStream(
                new Token("x", TokenType.Identifier, new Position(0, 0), new Position(0, 0)),
                new Token(".", TokenType.Period, new Position(0, 0), new Position(0, 0)));

            var parser = new Parser(tokenStream);

            Assert.Throws<NotSupportedException>(() => parser.Invoke());
        }

        [Test]
        public void ShouldNot_ParseAccessNode_When_GivenDoublePeriod()
        {
            var tokenStream = ParseTestUtilities.CreateTokenStream(
                new Token("x", TokenType.Identifier, new Position(0, 0), new Position(0, 0)),
                new Token(".", TokenType.Period, new Position(0, 0), new Position(0, 0)),
                new Token(".", TokenType.Period, new Position(0, 0), new Position(0, 0)),
                new Token("y", TokenType.Identifier, new Position(0, 0), new Position(0, 0)));

            var parser = new Parser(tokenStream);

            Assert.Throws<NotSupportedException>(() => parser.Invoke());
        }

        [Test]
        public void ShouldNot_ParseInvokeNode_When_MissingRightParenthesis()
        {
            var tokenStream = ParseTestUtilities.CreateTokenStream(
                new Token("x", TokenType.Identifier, new Position(0, 0), new Position(0, 0)),
                new Token("(", TokenType.LeftParenthesis, new Position(0, 0), new Position(0, 0)),
                new Token("y", TokenType.Identifier, new Position(0, 0), new Position(0, 0)));

            var parser = new Parser(tokenStream);

            Assert.Throws<NotSupportedException>(() => parser.Invoke());
        }
    }
}