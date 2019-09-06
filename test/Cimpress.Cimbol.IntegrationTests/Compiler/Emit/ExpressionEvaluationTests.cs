using System.Collections;
using System.Linq;
using NUnit.Framework;

namespace Cimpress.Cimbol.IntegrationTests.Compiler.Emit
{
    [TestFixture]
    public class ExpressionEvaluationTests
    {
        internal static IEnumerable TestCases
        {
            get
            {
                var cases = new[]
                {
                    new object[] { "1", 1m },
                    new object[] { "-1", -1m },
                    new object[] { "1 + 3", 4m },
                    new object[] { "5 - 2", 3m },
                    new object[] { "2 * 3", 6m },
                    new object[] { "9 / 3", 3m },
                    new object[] { "9 % 4", 1m },
                    new object[] { "2 ^ 3", 8m },
                    new object[] { "-27", -27m },
                    new object[] { "1 + 3 + 5", 9m },
                    new object[] { "1 - 3 - 5", -7m },
                    new object[] { "2 * 3 * 5", 30m },
                    new object[] { "10 / 5 / 4", 0.5m },
                    new object[] { "99 % 21 % 9", 6m },
                    new object[] { "2 ^ 3 ^ 2", 512m },
                    new object[] { "1 / 2 * 2 / 3 * 3", 1m },
                    new object[] { "((5+4) - (10*34)) + (4-5-6)", -338m },
                    new object[] { "((2 ^ 3) ^ 4)", 4096m },
                    new object[] { "81 ^ (1 / 2)", 9m },
                    new object[] { "2 / (0.125)", 16m },
                };

                var environments = new[]
                {
                    new object[] { CompilationProfile.Minimal },
                    new object[] { CompilationProfile.Trace },
                    new object[] { CompilationProfile.Verbose },
                };

                foreach (var expression in cases)
                {
                    foreach (var environment in environments)
                    {
                        var arguments = expression.Concat(environment).ToArray();

                        yield return new TestCaseData(arguments);
                    }
                }
            }
        }

        [Test]
        [TestCaseSource(nameof(TestCases))]
        public void Should_EqualNumber_When_Evaluated(string expression, decimal expected, CompilationProfile compilationProfile)
        {
            var program = new Program();
            var module = program.AddModule("Main");
            module.AddFormula("Formula", expression);
            var executable = program.Compile(compilationProfile);

            var result = executable.Call().Result;

            Assert.That(result.Errors, Has.Length.EqualTo(0));
            Assert.That(result.Modules, Has.Count.EqualTo(1));
            var resultModule = result.Modules["Main"];
            Assert.That(resultModule, Is.Not.Null);
            Assert.That(resultModule.Value["Formula"], Has.Property("Value").EqualTo(expected).Within(0.00000001m));
        }
    }
}