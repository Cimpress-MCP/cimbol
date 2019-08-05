using Cimpress.Cimbol.Runtime.Types;
using NUnit.Framework;

namespace Cimpress.Cimbol.IntegrationTests.Compiler.Emit
{
    [TestFixture]
    public class ExpressionEvaluationTests
    {
        [Test]
        [TestCase("1", 1)]
        [TestCase("-1", -1)]
        [TestCase("-1", -1)]
        [TestCase("1 + 3", 4)]
        [TestCase("5 - 2", 3)]
        [TestCase("2 * 3", 6)]
        [TestCase("9 / 3", 3)]
        [TestCase("9 % 4", 1)]
        [TestCase("2 ^ 3", 8)]
        [TestCase("-27", -27)]
        [TestCase("1 + 3 + 5", 9)]
        [TestCase("1 - 3 - 5", -7)]
        [TestCase("2 * 3 * 5", 30)]
        [TestCase("10 / 5 / 4", 0.5)]
        [TestCase("99 % 21 % 9", 6)]
        [TestCase("2 ^ 3 ^ 2", 512)]
        [TestCase("1 / 2 * 2 / 3 * 3", 1)]
        [TestCase("((5+4) - (10*34)) + (4-5-6)", -338)]
        [TestCase("((2 ^ 3) ^ 4)", 4096)]
        [TestCase("81 ^ (1 / 2)", 9)]
        [TestCase("2 / (0.125)", 16)]

        public void Should_EqualNumber_When_Evaluated(string expression, decimal expected)
        {
            var program = new Program();
            var module = program.AddModule("Main");
            module.AddFormula("Formula", expression);
            var executable = program.Compile();

            var result = executable.Call();

            var resultModule = result.Value["Main"] as ObjectValue;
            Assert.That(resultModule, Is.Not.Null);
            Assert.That(resultModule.Value["Formula"], Has.Property("Value").EqualTo(expected).Within(0.00000001m));
        }
    }
}