using Cimpress.Cimbol.Compiler.Parse;
using Cimpress.Cimbol.Compiler.Scan;
using Cimpress.Cimbol.Compiler.SyntaxTree;
using Cimpress.Cimbol.Exceptions;
using Cimpress.Cimbol.Utilities;
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
            var parser = new Parser("formula", tokenStream);

            var result = parser.Invoke();

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IdentifierNode>(result);
        }

        [Test]
        public void Should_ParseDefaultNode_When_GivenSingleIdentifier()
        {
            var position = new Position(0, 0);
            var tokenStream = ParseTestUtilities.CreateTokenStream(
                new Token("default", TokenType.DefaultKeyword, position, position),
                new Token("(", TokenType.LeftParenthesis, position, position),
                new Token("x", TokenType.Identifier, position, position),
                new Token(",", TokenType.Comma, position, position),
                new Token("1", TokenType.NumberLiteral, position, position),
                new Token(")", TokenType.RightParenthesis, position, position));
            var parser = new Parser("formula", tokenStream);

            var result = parser.Invoke();

            Assert.That(result, Is.InstanceOf<DefaultNode>());
            var defaultNodeResult = result as DefaultNode;
            Assert.That(defaultNodeResult, Is.Not.Null);
            Assert.That(defaultNodeResult.Fallback, Is.InstanceOf<LiteralNode>());
            Assert.That(defaultNodeResult.IsAsynchronous, Is.False);
            Assert.That(defaultNodeResult.Path, Is.EqualTo(new[] { "x" }));
        }

        [Test]
        public void Should_ParseDefaultNode_When_GivenMultipleIdentifiers()
        {
            var position = new Position(0, 0);
            var tokenStream = ParseTestUtilities.CreateTokenStream(
                new Token("default", TokenType.DefaultKeyword, position, position),
                new Token("(", TokenType.LeftParenthesis, position, position),
                new Token("x", TokenType.Identifier, position, position),
                new Token(".", TokenType.Period, position, position),
                new Token("y", TokenType.Identifier, position, position),
                new Token(".", TokenType.Period, position, position),
                new Token("z", TokenType.Identifier, position, position),
                new Token(",", TokenType.Comma, position, position),
                new Token("1", TokenType.NumberLiteral, position, position),
                new Token(")", TokenType.RightParenthesis, position, position));
            var parser = new Parser("formula", tokenStream);

            var result = parser.Invoke();

            Assert.That(result, Is.InstanceOf<DefaultNode>());
            var defaultNodeResult = result as DefaultNode;
            Assert.That(defaultNodeResult, Is.Not.Null);
            Assert.That(defaultNodeResult.Fallback, Is.InstanceOf<LiteralNode>());
            Assert.That(defaultNodeResult.IsAsynchronous, Is.False);
            Assert.That(defaultNodeResult.Path, Is.EqualTo(new[] { "x", "y", "z" }));
        }

        [Test]
        public void ShouldNot_ParseDefaultNode_When_GivenNoArguments()
        {
            var position = new Position(0, 0);
            var tokenStream = ParseTestUtilities.CreateTokenStream(
                new Token("default", TokenType.DefaultKeyword, position, position),
                new Token("(", TokenType.LeftParenthesis, position, position),
                new Token(")", TokenType.RightParenthesis, position, position));
            var parser = new Parser("formula", tokenStream);

            var result = Assert.Throws<CimbolCompilationException>(() => parser.Invoke());

            Assert.That(result.Formula, Is.EqualTo("formula"));
        }

        [Test]
        public void ShouldNot_ParseDefaultNode_When_GivenOneArgument()
        {
            var position = new Position(0, 0);
            var tokenStream = ParseTestUtilities.CreateTokenStream(
                new Token("default", TokenType.DefaultKeyword, position, position),
                new Token("(", TokenType.LeftParenthesis, position, position),
                new Token("x", TokenType.Identifier, position, position),
                new Token(")", TokenType.RightParenthesis, position, position));
            var parser = new Parser("formula", tokenStream);

            var result = Assert.Throws<CimbolCompilationException>(() => parser.Invoke());

            Assert.That(result.Formula, Is.EqualTo("formula"));
        }

        [Test]
        public void ShouldNot_ParseDefaultNode_When_GivenThreeArguments()
        {
            var position = new Position(0, 0);
            var tokenStream = ParseTestUtilities.CreateTokenStream(
                new Token("default", TokenType.DefaultKeyword, position, position),
                new Token("(", TokenType.LeftParenthesis, position, position),
                new Token("x", TokenType.Identifier, position, position),
                new Token(",", TokenType.Comma, position, position),
                new Token("y", TokenType.Identifier, position, position),
                new Token(",", TokenType.Comma, position, position),
                new Token("z", TokenType.Identifier, position, position),
                new Token(")", TokenType.RightParenthesis, position, position));
            var parser = new Parser("formula", tokenStream);

            var result = Assert.Throws<CimbolCompilationException>(() => parser.Invoke());

            Assert.That(result.Formula, Is.EqualTo("formula"));
        }

        [Test]
        public void ShouldNot_ParseDefaultNode_When_FirstArgumentIsNotIdentifierChain()
        {
            var position = new Position(0, 0);
            var tokenStream = ParseTestUtilities.CreateTokenStream(
                new Token("default", TokenType.DefaultKeyword, position, position),
                new Token("(", TokenType.LeftParenthesis, position, position),
                new Token("1", TokenType.NumberLiteral, position, position),
                new Token(",", TokenType.Comma, position, position),
                new Token("2", TokenType.NumberLiteral, position, position),
                new Token(")", TokenType.RightParenthesis, position, position));
            var parser = new Parser("formula", tokenStream);

            var result = Assert.Throws<CimbolCompilationException>(() => parser.Invoke());

            Assert.That(result.Formula, Is.EqualTo("formula"));
        }

        [Test]
        public void ShouldNot_ParseDefaultNode_When_FirstArgumentIsMissing()
        {
            var position = new Position(0, 0);
            var tokenStream = ParseTestUtilities.CreateTokenStream(
                new Token("default", TokenType.DefaultKeyword, position, position),
                new Token("(", TokenType.LeftParenthesis, position, position),
                new Token(",", TokenType.Comma, position, position),
                new Token("1", TokenType.NumberLiteral, position, position),
                new Token(")", TokenType.RightParenthesis, position, position));
            var parser = new Parser("formula", tokenStream);

            var result = Assert.Throws<CimbolCompilationException>(() => parser.Invoke());

            Assert.That(result.Formula, Is.EqualTo("formula"));
        }

        [Test]
        public void ShouldNot_ParseDefaultNode_When_SecondArgumentIsMissing()
        {
            var position = new Position(0, 0);
            var tokenStream = ParseTestUtilities.CreateTokenStream(
                new Token("default", TokenType.DefaultKeyword, position, position),
                new Token("(", TokenType.LeftParenthesis, position, position),
                new Token("x", TokenType.Identifier, position, position),
                new Token(",", TokenType.Comma, position, position),
                new Token(")", TokenType.RightParenthesis, position, position));
            var parser = new Parser("formula", tokenStream);

            var result = Assert.Throws<CimbolCompilationException>(() => parser.Invoke());

            Assert.That(result.Formula, Is.EqualTo("formula"));
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

            var parser = new Parser("formula", tokenStream);

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

            var parser = new Parser("formula", tokenStream);

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
        public void Should_ParseMacroNode_When_GivenMacroWithNamedArgumentsWithSpaces(
            string macroName,
            TokenType tokenType)
        {
            var tokenStream = ParseTestUtilities.CreateTokenStream(
                new Token(macroName, tokenType, new Position(0, 0), new Position(0, 0)),
                new Token("(", TokenType.LeftParenthesis, new Position(0, 0), new Position(0, 0)),
                new Token("'a b c'", TokenType.Identifier, new Position(0, 0), new Position(0, 0)),
                new Token("=", TokenType.Assign, new Position(0, 0), new Position(0, 0)),
                new Token("x", TokenType.Identifier, new Position(0, 0), new Position(0, 0)),
                new Token(")", TokenType.RightParenthesis, new Position(0, 0), new Position(0, 0)));

            var parser = new Parser("formula", tokenStream);

            var result = parser.Invoke();
            Assert.That(result, Is.InstanceOf<MacroNode>());
            var macroResult = result as MacroNode;
            Assert.That(macroResult, Is.Not.Null);
            Assert.That(macroResult.Macro, Is.EqualTo(macroName));
            Assert.That(macroResult.Arguments, Has.Length.EqualTo(1));
            var argumentResult = macroResult.Arguments[0] as NamedArgument;
            Assert.That(argumentResult, Is.Not.Null);
            Assert.That(argumentResult.Name, Is.EqualTo("a b c"));
        }

        [Test]
        public void Should_ParseAccessNode_When_GivenSingleAccess()
        {
            var tokenStream = ParseTestUtilities.CreateTokenStream(
                new Token("x", TokenType.Identifier, new Position(0, 0), new Position(0, 0)),
                new Token(".", TokenType.Period, new Position(0, 0), new Position(0, 0)),
                new Token("y", TokenType.Identifier, new Position(0, 0), new Position(0, 0)));

            var parser = new Parser("formula", tokenStream);

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

            var parser = new Parser("formula", tokenStream);

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

            var parser = new Parser("formula", tokenStream);

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

            var parser = new Parser("formula", tokenStream);

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

            var parser = new Parser("formula", tokenStream);

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

            var parser = new Parser("formula", tokenStream);

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

            var parser = new Parser("formula", tokenStream);

            Assert.Throws<CimbolCompilationException>(() => parser.Invoke());
        }

        [Test]
        public void ShouldNot_ParseAccessNode_When_GivenDoublePeriod()
        {
            var tokenStream = ParseTestUtilities.CreateTokenStream(
                new Token("x", TokenType.Identifier, new Position(0, 0), new Position(0, 0)),
                new Token(".", TokenType.Period, new Position(0, 0), new Position(0, 0)),
                new Token(".", TokenType.Period, new Position(0, 0), new Position(0, 0)),
                new Token("y", TokenType.Identifier, new Position(0, 0), new Position(0, 0)));

            var parser = new Parser("formula", tokenStream);

            Assert.Throws<CimbolCompilationException>(() => parser.Invoke());
        }

        [Test]
        public void ShouldNot_ParseInvokeNode_When_MissingRightParenthesis()
        {
            var tokenStream = ParseTestUtilities.CreateTokenStream(
                new Token("x", TokenType.Identifier, new Position(0, 0), new Position(0, 0)),
                new Token("(", TokenType.LeftParenthesis, new Position(0, 0), new Position(0, 0)),
                new Token("y", TokenType.Identifier, new Position(0, 0), new Position(0, 0)));

            var parser = new Parser("formula", tokenStream);

            Assert.Throws<CimbolCompilationException>(() => parser.Invoke());
        }

        [Test]
        public void Should_ParseExistsNode_When_GivenSingleIdentifier()
        {
            var position = new Position(0, 0);
            var tokenStream = ParseTestUtilities.CreateTokenStream(
                new Token("exists", TokenType.ExistsKeyword, position, position),
                new Token("(", TokenType.LeftParenthesis, position, position),
                new Token("x", TokenType.Identifier, position, position),
                new Token(")", TokenType.RightParenthesis, position, position));
            var parser = new Parser("formula", tokenStream);

            var result = parser.Invoke();

            Assert.That(result, Is.InstanceOf<ExistsNode>());
            var existsNodeResult = result as ExistsNode;
            Assert.That(existsNodeResult, Is.Not.Null);
            Assert.That(existsNodeResult.IsAsynchronous, Is.False);
            Assert.That(existsNodeResult.Path, Is.EqualTo(new[] { "x" }));
        }

        [Test]
        public void Should_ParseExistsNode_When_GivenMultipleIdentifiers()
        {
            var position = new Position(0, 0);
            var tokenStream = ParseTestUtilities.CreateTokenStream(
                new Token("exists", TokenType.ExistsKeyword, position, position),
                new Token("(", TokenType.LeftParenthesis, position, position),
                new Token("x", TokenType.Identifier, position, position),
                new Token(".", TokenType.Period, position, position),
                new Token("y", TokenType.Identifier, position, position),
                new Token(".", TokenType.Period, position, position),
                new Token("z", TokenType.Identifier, position, position),
                new Token(")", TokenType.RightParenthesis, position, position));
            var parser = new Parser("formula", tokenStream);

            var result = parser.Invoke();

            Assert.That(result, Is.InstanceOf<ExistsNode>());
            var existsNodeResult = result as ExistsNode;
            Assert.That(existsNodeResult, Is.Not.Null);
            Assert.That(existsNodeResult.IsAsynchronous, Is.False);
            Assert.That(existsNodeResult.Path, Is.EqualTo(new[] { "x", "y", "z" }));
        }

        [Test]
        public void ShouldNot_ParseExistsNode_When_GivenNoArguments()
        {
            var position = new Position(0, 0);
            var tokenStream = ParseTestUtilities.CreateTokenStream(
                new Token("exists", TokenType.ExistsKeyword, position, position),
                new Token("(", TokenType.LeftParenthesis, position, position),
                new Token(")", TokenType.RightParenthesis, position, position));
            var parser = new Parser("formula", tokenStream);

            var result = Assert.Throws<CimbolCompilationException>(() => parser.Invoke());

            Assert.That(result.Formula, Is.EqualTo("formula"));
        }

        [Test]
        public void ShouldNot_ParseExistsNode_When_GivenMoreThenOneArgument()
        {
            var position = new Position(0, 0);
            var tokenStream = ParseTestUtilities.CreateTokenStream(
                new Token("exists", TokenType.ExistsKeyword, position, position),
                new Token("(", TokenType.LeftParenthesis, position, position),
                new Token("x", TokenType.Identifier, position, position),
                new Token(",", TokenType.Comma, position, position),
                new Token("y", TokenType.Identifier, position, position),
                new Token(")", TokenType.RightParenthesis, position, position));
            var parser = new Parser("formula", tokenStream);

            var result = Assert.Throws<CimbolCompilationException>(() => parser.Invoke());

            Assert.That(result.Formula, Is.EqualTo("formula"));
            Assert.That(result.Message, Is.EqualTo("The exists function takes one argument."));
        }
    }
}