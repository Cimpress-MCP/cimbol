using Cimpress.Cimbol.Compiler.Parse;
using Cimpress.Cimbol.Compiler.Scan;
using Cimpress.Cimbol.Compiler.SyntaxTree;
using Cimpress.Cimbol.Utilities;
using NUnit.Framework;

namespace Cimpress.Cimbol.UnitTests.Compiler.Parse
{
    [TestFixture]
    public class ComparisonParserTests
    {
        [Test]
        public void Should_PassThroughComparison_When_NotComparison()
        {
            var tokenStream = ParseTestUtilities.CreateTokenStream(
                new Token("x", TokenType.Identifier, new Position(0, 0), new Position(0, 0)));

            var parser = new Parser("formula", tokenStream);

            var result = parser.Comparison();
            Assert.IsInstanceOf<IdentifierNode>(result);
        }

        [Test]
        [TestCase("==", TokenType.Equal, BinaryOpType.Equal)]
        [TestCase(">", TokenType.GreaterThan, BinaryOpType.GreaterThan)]
        [TestCase(">=", TokenType.GreaterThanEqual, BinaryOpType.GreaterThanOrEqual)]
        [TestCase("<", TokenType.LessThan, BinaryOpType.LessThan)]
        [TestCase("<=", TokenType.LessThanEqual, BinaryOpType.LessThanOrEqual)]
        [TestCase("!=", TokenType.NotEqual, BinaryOpType.NotEqual)]
        public void Should_ParseComparisonNode_When_GivenComparison(string value, TokenType token, BinaryOpType op)
        {
            var tokenStream = ParseTestUtilities.CreateTokenStream(
                new Token("x", TokenType.Identifier, new Position(0, 0), new Position(0, 0)),
                new Token(value, token, new Position(0, 0), new Position(0, 0)),
                new Token("y", TokenType.Identifier, new Position(0, 0), new Position(0, 0)));

            var parser = new Parser("formula", tokenStream);

            var result = parser.Comparison();
            Assert.IsInstanceOf<BinaryOpNode>(result);
            var binaryOpResult = result as BinaryOpNode;
            Assert.IsNotNull(binaryOpResult);
            Assert.AreEqual(op, binaryOpResult.OpType);

            var leftResult = binaryOpResult.Left;
            Assert.IsInstanceOf<IdentifierNode>(leftResult);
            var identifierLeftResult = leftResult as IdentifierNode;
            Assert.IsNotNull(identifierLeftResult);
            Assert.AreEqual("x", identifierLeftResult.Identifier);

            var rightResult = binaryOpResult.Right;
            Assert.IsInstanceOf<IdentifierNode>(rightResult);
            var identifierRightResult = rightResult as IdentifierNode;
            Assert.IsNotNull(identifierRightResult);
            Assert.AreEqual("y", identifierRightResult.Identifier);
        }

        [Test]
        [TestCase("==", TokenType.Equal)]
        [TestCase(">", TokenType.GreaterThan)]
        [TestCase(">=", TokenType.GreaterThanEqual)]
        [TestCase("<", TokenType.LessThan)]
        [TestCase("<=", TokenType.LessThanEqual)]
        [TestCase("!=", TokenType.NotEqual)]
        public void Should_ParseComparisonNodeAsLeftAssociative_When_GivenComparison(string value, TokenType token)
        {
            var tokenStream = ParseTestUtilities.CreateTokenStream(
                new Token("x", TokenType.Identifier, new Position(0, 0), new Position(0, 0)),
                new Token(value, token, new Position(0, 0), new Position(0, 0)),
                new Token("y", TokenType.Identifier, new Position(0, 0), new Position(0, 0)),
                new Token(value, token, new Position(0, 0), new Position(0, 0)),
                new Token("z", TokenType.Identifier, new Position(0, 0), new Position(0, 0)));

            var parser = new Parser("formula", tokenStream);

            var result = parser.Comparison();
            Assert.IsInstanceOf<BinaryOpNode>(result);
            var binaryOpResult = result as BinaryOpNode;
            Assert.IsNotNull(binaryOpResult);
            Assert.IsInstanceOf<BinaryOpNode>(binaryOpResult.Left);
            Assert.IsInstanceOf<IdentifierNode>(binaryOpResult.Right);
        }

        [Test]
        public void Should_PassThroughLogicalAnd_When_NotLogicalAnd()
        {
            var tokenStream = ParseTestUtilities.CreateTokenStream(
                new Token("x", TokenType.Identifier, new Position(0, 0), new Position(0, 0)));

            var parser = new Parser("formula", tokenStream);

            var result = parser.LogicalAnd();
            Assert.IsInstanceOf<IdentifierNode>(result);
        }

        [Test]
        public void Should_ParseBinaryOpNode_When_GivenLogicalAnd()
        {
            var tokenStream = ParseTestUtilities.CreateTokenStream(
                new Token("x", TokenType.Identifier, new Position(0, 0), new Position(0, 0)),
                new Token("and", TokenType.And, new Position(0, 0), new Position(0, 0)),
                new Token("y", TokenType.Identifier, new Position(0, 0), new Position(0, 0)));

            var parser = new Parser("formula", tokenStream);

            var result = parser.LogicalAnd();
            Assert.IsInstanceOf<BinaryOpNode>(result);
            var binaryOpResult = result as BinaryOpNode;
            Assert.IsNotNull(binaryOpResult);
            Assert.AreEqual(BinaryOpType.And, binaryOpResult.OpType);

            var leftResult = binaryOpResult.Left;
            Assert.IsInstanceOf<IdentifierNode>(leftResult);
            var identifierLeftResult = leftResult as IdentifierNode;
            Assert.IsNotNull(identifierLeftResult);
            Assert.AreEqual("x", identifierLeftResult.Identifier);

            var rightResult = binaryOpResult.Right;
            Assert.IsInstanceOf<IdentifierNode>(rightResult);
            var identifierRightResult = rightResult as IdentifierNode;
            Assert.IsNotNull(identifierRightResult);
            Assert.AreEqual("y", identifierRightResult.Identifier);
        }

        [Test]
        public void Should_ParseLeftAssociative_When_GivenLogicalAnd()
        {
            var tokenStream = ParseTestUtilities.CreateTokenStream(
                new Token("x", TokenType.Identifier, new Position(0, 0), new Position(0, 0)),
                new Token("and", TokenType.And, new Position(0, 0), new Position(0, 0)),
                new Token("y", TokenType.Identifier, new Position(0, 0), new Position(0, 0)),
                new Token("and", TokenType.And, new Position(0, 0), new Position(0, 0)),
                new Token("z", TokenType.Identifier, new Position(0, 0), new Position(0, 0)));

            var parser = new Parser("formula", tokenStream);

            var result = parser.LogicalAnd();
            Assert.IsInstanceOf<BinaryOpNode>(result);
            var binaryOpResult = result as BinaryOpNode;
            Assert.IsNotNull(binaryOpResult);
            Assert.IsInstanceOf<BinaryOpNode>(binaryOpResult.Left);
            Assert.IsInstanceOf<IdentifierNode>(binaryOpResult.Right);
        }

        [Test]
        public void Should_PassThroughLogicalNot_When_NotLogicalNot()
        {
            var tokenStream = ParseTestUtilities.CreateTokenStream(
                new Token("x", TokenType.Identifier, new Position(0, 0), new Position(0, 0)));

            var parser = new Parser("formula", tokenStream);

            var result = parser.LogicalNot();
            Assert.IsInstanceOf<IdentifierNode>(result);
        }

        [Test]
        public void Should_ParseUnaryOpNode_When_GivenLogicalNot()
        {
            var tokenStream = ParseTestUtilities.CreateTokenStream(
                new Token("not", TokenType.Not, new Position(0, 0), new Position(0, 0)),
                new Token("x", TokenType.Identifier, new Position(0, 0), new Position(0, 0)));

            var parser = new Parser("formula", tokenStream);

            var result = parser.LogicalNot();
            Assert.IsInstanceOf<UnaryOpNode>(result);
            var unaryOpResult = result as UnaryOpNode;
            Assert.IsNotNull(unaryOpResult);
            Assert.AreEqual(UnaryOpType.Not, unaryOpResult.OpType);

            var innerResult = unaryOpResult.Operand;
            Assert.IsInstanceOf<IdentifierNode>(innerResult);
        }

        [Test]
        public void Should_ParseUnaryOpNodees_When_GivenMultipleLogicalNotOperations()
        {
            var tokenStream = ParseTestUtilities.CreateTokenStream(
                new Token("not", TokenType.Not, new Position(0, 0), new Position(0, 0)),
                new Token("not", TokenType.Not, new Position(0, 0), new Position(0, 0)),
                new Token("x", TokenType.Identifier, new Position(0, 0), new Position(0, 0)));

            var parser = new Parser("formula", tokenStream);

            var result = parser.LogicalNot();
            Assert.IsInstanceOf<UnaryOpNode>(result);
            var unaryOpResult = result as UnaryOpNode;
            Assert.IsNotNull(unaryOpResult);
            Assert.AreEqual(UnaryOpType.Not, unaryOpResult.OpType);

            var innerResult1 = unaryOpResult.Operand;
            Assert.IsInstanceOf<UnaryOpNode>(innerResult1);
            var unaryOpInnerResult1 = innerResult1 as UnaryOpNode;
            Assert.IsNotNull(unaryOpInnerResult1);
            Assert.AreEqual(UnaryOpType.Not, unaryOpInnerResult1.OpType);

            var innerResult2 = unaryOpInnerResult1.Operand;
            Assert.IsInstanceOf<IdentifierNode>(innerResult2);
        }

        [Test]
        public void Should_PassThroughLogicalOr_When_NotLogicalOr()
        {
            var tokenStream = ParseTestUtilities.CreateTokenStream(
                new Token("x", TokenType.Identifier, new Position(0, 0), new Position(0, 0)));

            var parser = new Parser("formula", tokenStream);

            var result = parser.LogicalOr();
            Assert.IsInstanceOf<IdentifierNode>(result);
        }

        [Test]
        public void Should_ParseBinaryOpNode_When_GivenLogicalOr()
        {
            var tokenStream = ParseTestUtilities.CreateTokenStream(
                new Token("x", TokenType.Identifier, new Position(0, 0), new Position(0, 0)),
                new Token("or", TokenType.Or, new Position(0, 0), new Position(0, 0)),
                new Token("y", TokenType.Identifier, new Position(0, 0), new Position(0, 0)));

            var parser = new Parser("formula", tokenStream);

            var result = parser.LogicalOr();
            Assert.IsInstanceOf<BinaryOpNode>(result);
            var binaryOpResult = result as BinaryOpNode;
            Assert.IsNotNull(binaryOpResult);
            Assert.AreEqual(BinaryOpType.Or, binaryOpResult.OpType);

            var leftResult = binaryOpResult.Left;
            Assert.IsInstanceOf<IdentifierNode>(leftResult);
            var identifierLeftResult = leftResult as IdentifierNode;
            Assert.IsNotNull(identifierLeftResult);
            Assert.AreEqual("x", identifierLeftResult.Identifier);

            var rightResult = binaryOpResult.Right;
            Assert.IsInstanceOf<IdentifierNode>(rightResult);
            var identifierRightResult = rightResult as IdentifierNode;
            Assert.IsNotNull(identifierRightResult);
            Assert.AreEqual("y", identifierRightResult.Identifier);
        }

        [Test]
        public void Should_ParseLeftAssociative_When_GivenLogicalOr()
        {
            var tokenStream = ParseTestUtilities.CreateTokenStream(
                new Token("x", TokenType.Identifier, new Position(0, 0), new Position(0, 0)),
                new Token("or", TokenType.Or, new Position(0, 0), new Position(0, 0)),
                new Token("y", TokenType.Identifier, new Position(0, 0), new Position(0, 0)),
                new Token("or", TokenType.Or, new Position(0, 0), new Position(0, 0)),
                new Token("z", TokenType.Identifier, new Position(0, 0), new Position(0, 0)));

            var parser = new Parser("formula", tokenStream);

            var result = parser.LogicalOr();
            Assert.IsInstanceOf<BinaryOpNode>(result);
            var binaryOpResult = result as BinaryOpNode;
            Assert.IsNotNull(binaryOpResult);
            Assert.IsInstanceOf<BinaryOpNode>(binaryOpResult.Left);
            Assert.IsInstanceOf<IdentifierNode>(binaryOpResult.Right);
        }
    }
}