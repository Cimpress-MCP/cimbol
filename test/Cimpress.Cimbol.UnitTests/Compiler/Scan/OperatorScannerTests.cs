using Cimpress.Cimbol.Compiler.Scan;
using Cimpress.Cimbol.Compiler.Utilities;
using NUnit.Framework;

namespace Cimpress.Cimbol.UnitTests.Compiler.Scan
{
    [TestFixture]
    public class OperatorScannerTests
    {
        [Test]
        [TestCase("+", TokenType.Add)]
        [TestCase("-", TokenType.Subtract)]
        [TestCase("*", TokenType.Multiply)]
        [TestCase("/", TokenType.Divide)]
        [TestCase("^", TokenType.Power)]
        [TestCase("%", TokenType.Remainder)]
        [TestCase("++", TokenType.Concatenate)]
        [TestCase("==", TokenType.Equal)]
        [TestCase("!=", TokenType.NotEqual)]
        [TestCase("<", TokenType.LessThan)]
        [TestCase("<=", TokenType.LessThanEqual)]
        [TestCase(">", TokenType.GreaterThan)]
        [TestCase(">=", TokenType.GreaterThanEqual)]
        [TestCase("=", TokenType.Assign)]
        public void Should_MakeOperatorToken_When_GivenOperatorSource(string source, TokenType type)
        {
            var scanner = new Scanner(new SourceText(source));
            var token = scanner.NextOperator();
            Assert.AreEqual(type, token.Type);
            Assert.AreEqual(source, token.Value);
        }
    }
}