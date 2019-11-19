using Cimpress.Cimbol.Runtime.Modules;
using Cimpress.Cimbol.Runtime.Types;
using NUnit.Framework;

namespace Cimpress.Cimbol.UnitTests.Runtime.Modules
{
    [TestFixture]
    public class MathModuleTests
    {
        [Test]
        [TestCase("Math.Abs(1)", 1)]
        [TestCase("Math.Abs(\"1\")", 1)]
        [TestCase("Math.Abs(-1)", 1)]
        [TestCase("Math.Abs(\"-1\")", 1)]
        [TestCase("Math.Abs(0)", 0)]
        [TestCase("Math.Abs(\"0\")", 0)]
        public void Should_GetAbsoluteValue_When_CallingMathAbs(string formula, decimal expected)
        {
            var program = BuildProgram(formula);
            var executable = program.Compile();

            var results = executable.Call();
            var result = results.Result.Modules["Main"].Value["Formula01"] as NumberValue;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.EqualTo(expected));
        }

        [Test]
        [TestCase("Math.Ceil(-1.23)", -1)]
        [TestCase("Math.Ceil(-0.23)", 0)]
        [TestCase("Math.Ceil(0.0)", 0)]
        [TestCase("Math.Ceil(0.23)", 1)]
        [TestCase("Math.Ceil(1.23)", 2)]
        [TestCase("Math.Ceil(-1.23, -1)", 0)]
        [TestCase("Math.Ceil(-0.23, -1)", 0)]
        [TestCase("Math.Ceil(0.0, -1)", 0)]
        [TestCase("Math.Ceil(0.23, -1)", 10)]
        [TestCase("Math.Ceil(1.23, -1)", 10)]
        [TestCase("Math.Ceil(-1.23, 0)", -1)]
        [TestCase("Math.Ceil(-0.23, 0)", 0)]
        [TestCase("Math.Ceil(0.0, 0)", 0)]
        [TestCase("Math.Ceil(0.23, 0)", 1)]
        [TestCase("Math.Ceil(1.23, 0)", 2)]
        [TestCase("Math.Ceil(-1.23, 1)", -1.2)]
        [TestCase("Math.Ceil(-0.23, 1)", -0.2)]
        [TestCase("Math.Ceil(0.0, 1)", 0)]
        [TestCase("Math.Ceil(0.23, 1)", 0.3)]
        [TestCase("Math.Ceil(1.23, 1)", 1.3)]
        public void Should_GetCeiling_When_CallingMathCeiling(string formula, decimal expected)
        {
            var program = BuildProgram(formula);
            var executable = program.Compile();

            var results = executable.Call();
            var result = results.Result.Modules["Main"].Value["Formula01"] as NumberValue;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.EqualTo(expected));
        }

        [Test]
        [TestCase("Math.Cos(0)", 1)]
        [TestCase("Math.Cos(60)", 0.5)]
        [TestCase("Math.Cos(90)", 0)]
        [TestCase("Math.Cos(120)", -0.5)]
        [TestCase("Math.Cos(180)", -1)]
        [TestCase("Math.Cos(240)", -0.5)]
        [TestCase("Math.Cos(270)", 0)]
        [TestCase("Math.Cos(300)", 0.5)]
        [TestCase("Math.Cos(360)", 1)]
        public void Should_GetCos_When_CallingMathCos(string formula, decimal expected)
        {
            var program = BuildProgram(formula);
            var executable = program.Compile();

            var results = executable.Call();
            var result = results.Result.Modules["Main"].Value["Formula01"] as NumberValue;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.EqualTo(expected).Within(1e-10));
        }

        [Test]
        [TestCase("Math.Floor(-1.23)", -2)]
        [TestCase("Math.Floor(-0.23)", -1)]
        [TestCase("Math.Floor(0.0)", 0)]
        [TestCase("Math.Floor(0.23)", 0)]
        [TestCase("Math.Floor(1.23)", 1)]
        [TestCase("Math.Floor(-1.23, -1)", -10)]
        [TestCase("Math.Floor(-0.23, -1)", -10)]
        [TestCase("Math.Floor(0.0, -1)", 0)]
        [TestCase("Math.Floor(0.23, -1)", 0)]
        [TestCase("Math.Floor(1.23, -1)", 0)]
        [TestCase("Math.Floor(-1.23, 0)", -2)]
        [TestCase("Math.Floor(-0.23, 0)", -1)]
        [TestCase("Math.Floor(0.0, 0)", 0)]
        [TestCase("Math.Floor(0.23, 0)", 0)]
        [TestCase("Math.Floor(1.23, 0)", 1)]
        [TestCase("Math.Floor(-1.23, 1)", -1.3)]
        [TestCase("Math.Floor(-0.23, 1)", -0.3)]
        [TestCase("Math.Floor(0.0, 1)", 0)]
        [TestCase("Math.Floor(0.23, 1)", 0.2)]
        [TestCase("Math.Floor(1.23, 1)", 1.2)]
        public void Should_GetFloor_When_CallingMathFloor(string formula, decimal expected)
        {
            var program = BuildProgram(formula);
            var executable = program.Compile();

            var results = executable.Call();
            var result = results.Result.Modules["Main"].Value["Formula01"] as NumberValue;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.EqualTo(expected));
        }

        [Test]
        [TestCase("Math.Log(1, 2)", 0)]
        [TestCase("Math.Log(2, 2)", 1)]
        [TestCase("Math.Log(4, 2)", 2)]
        [TestCase("Math.Log(8, 2)", 3)]
        [TestCase("Math.Log(1, 10)", 0)]
        [TestCase("Math.Log(10, 10)", 1)]
        [TestCase("Math.Log(100, 10)", 2)]
        [TestCase("Math.Log(1000, 10)", 3)]
        public void Should_GetLogBaseN_When_CallingMathLog(string formula, decimal expected)
        {
            var program = BuildProgram(formula);
            var executable = program.Compile();

            var results = executable.Call();
            var result = results.Result.Modules["Main"].Value["Formula01"] as NumberValue;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.EqualTo(expected));
        }

        [Test]
        [TestCase("Math.Max(-1)", -1)]
        [TestCase("Math.Max(\"-1\")", -1)]
        [TestCase("Math.Max(0)", 0)]
        [TestCase("Math.Max(\"0\")", 0)]
        [TestCase("Math.Max(1)", 1)]
        [TestCase("Math.Max(\"1\")", 1)]
        [TestCase("Math.Max(-1, 1)", 1)]
        [TestCase("Math.Max(1, -1)", 1)]
        [TestCase("Math.Max(-1, 0, 1)", 1)]
        [TestCase("Math.Max(1, 0, -1)", 1)]
        [TestCase("Math.Max(1, 2, 3, 4, 5)", 5)]
        [TestCase("Math.Max(5, 4, 3, 2, 1)", 5)]
        public void Should_GetMaximumNumber_When_CallingMathMax(string formula, decimal expected)
        {
            var program = BuildProgram(formula);
            var executable = program.Compile();

            var results = executable.Call();
            var result = results.Result.Modules["Main"].Value["Formula01"] as NumberValue;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.EqualTo(expected));
        }

        [Test]
        [TestCase("Math.Min(-1)", -1)]
        [TestCase("Math.Min(\"-1\")", -1)]
        [TestCase("Math.Min(0)", 0)]
        [TestCase("Math.Min(\"0\")", 0)]
        [TestCase("Math.Min(1)", 1)]
        [TestCase("Math.Min(\"1\")", 1)]
        [TestCase("Math.Min(-1, 1)", -1)]
        [TestCase("Math.Min(1, -1)", -1)]
        [TestCase("Math.Min(-1, 0, 1)", -1)]
        [TestCase("Math.Min(1, 0, -1)", -1)]
        [TestCase("Math.Min(1, 2, 3, 4, 5)", 1)]
        [TestCase("Math.Min(5, 4, 3, 2, 1)", 1)]
        public void Should_GetMinimumNumber_When_CallingMathMin(string formula, decimal expected)
        {
            var program = BuildProgram(formula);
            var executable = program.Compile();

            var results = executable.Call();
            var result = results.Result.Modules["Main"].Value["Formula01"] as NumberValue;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.EqualTo(expected));
        }

        [Test]
        [TestCase("Math.Round(-2.5)", -2)]
        [TestCase("Math.Round(-1.5)", -2)]
        [TestCase("Math.Round(-0.5)", 0)]
        [TestCase("Math.Round(0)", 0)]
        [TestCase("Math.Round(0.3)", 0)]
        [TestCase("Math.Round(0.5)", 0)]
        [TestCase("Math.Round(0.7)", 1)]
        [TestCase("Math.Round(1.3)", 1)]
        [TestCase("Math.Round(1.5)", 2)]
        [TestCase("Math.Round(1.7)", 2)]
        [TestCase("Math.Round(2.3)", 2)]
        [TestCase("Math.Round(2.5)", 2)]
        [TestCase("Math.Round(2.7)", 3)]

        [TestCase("Math.Round(-25, -1)", -20)]
        [TestCase("Math.Round(-15, -1)", -20)]
        [TestCase("Math.Round(-5, -1)", 0)]
        [TestCase("Math.Round(0, -1)", 0)]
        [TestCase("Math.Round(3, -1)", 0)]
        [TestCase("Math.Round(5, -1)", 0)]
        [TestCase("Math.Round(7, -1)", 10)]
        [TestCase("Math.Round(13, -1)", 10)]
        [TestCase("Math.Round(15, -1)", 20)]
        [TestCase("Math.Round(17, -1)", 20)]
        [TestCase("Math.Round(23, -1)", 20)]
        [TestCase("Math.Round(25, -1)", 20)]
        [TestCase("Math.Round(27, -1)", 30)]

        [TestCase("Math.Round(-2.5, 0)", -2)]
        [TestCase("Math.Round(-1.5, 0)", -2)]
        [TestCase("Math.Round(-0.5, 0)", 0)]
        [TestCase("Math.Round(0, 0)", 0)]
        [TestCase("Math.Round(0.3, 0)", 0)]
        [TestCase("Math.Round(0.5, 0)", 0)]
        [TestCase("Math.Round(0.7, 0)", 1)]
        [TestCase("Math.Round(1.3, 0)", 1)]
        [TestCase("Math.Round(1.5, 0)", 2)]
        [TestCase("Math.Round(1.7, 0)", 2)]
        [TestCase("Math.Round(2.3, 0)", 2)]
        [TestCase("Math.Round(2.5, 0)", 2)]
        [TestCase("Math.Round(2.7, 0)", 3)]

        [TestCase("Math.Round(-0.25, 1)", -0.2)]
        [TestCase("Math.Round(-0.25, 1)", -0.2)]
        [TestCase("Math.Round(-0.15, 1)", -0.2)]
        [TestCase("Math.Round(-0.05, 1)", 0)]
        [TestCase("Math.Round(0, 1)", 0)]
        [TestCase("Math.Round(0.03, 1)", 0)]
        [TestCase("Math.Round(0.05, 1)", 0)]
        [TestCase("Math.Round(0.07, 1)", 0.1)]
        [TestCase("Math.Round(0.13, 1)", 0.1)]
        [TestCase("Math.Round(0.15, 1)", 0.2)]
        [TestCase("Math.Round(0.17, 1)", 0.2)]
        [TestCase("Math.Round(0.23, 1)", 0.2)]
        [TestCase("Math.Round(0.25, 1)", 0.2)]
        [TestCase("Math.Round(0.27, 1)", 0.3)]
        public void Should_Round_When_CallingMathRound(string formula, decimal expected)
        {
            var program = BuildProgram(formula);
            var executable = program.Compile();

            var results = executable.Call();
            var result = results.Result.Modules["Main"].Value["Formula01"] as NumberValue;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.EqualTo(expected));
        }

        [Test]
        [TestCase("Math.Sin(0)", 0)]
        [TestCase("Math.Sin(30)", 0.5)]
        [TestCase("Math.Sin(90)", 1)]
        [TestCase("Math.Sin(150)", 0.5)]
        [TestCase("Math.Sin(180)", 0)]
        [TestCase("Math.Sin(210)", -0.5)]
        [TestCase("Math.Sin(270)", -1)]
        [TestCase("Math.Sin(330)", -0.5)]
        [TestCase("Math.Sin(360)", 0)]
        public void Should_GetSin_When_CallingMathSin(string formula, decimal expected)
        {
            var program = BuildProgram(formula);
            var executable = program.Compile();

            var results = executable.Call();
            var result = results.Result.Modules["Main"].Value["Formula01"] as NumberValue;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.EqualTo(expected).Within(1e-10));
        }

        [Test]
        [TestCase("Math.Tan(0)", 0)]
        [TestCase("Math.Tan(45)", 1)]
        [TestCase("Math.Tan(135)", -1)]
        [TestCase("Math.Tan(180)", 0)]
        [TestCase("Math.Tan(225)", 1)]
        [TestCase("Math.Tan(315)", -1)]
        [TestCase("Math.Tan(360)", 0)]
        public void Should_GetTan_When_CallingMathTan(string formula, decimal expected)
        {
            var program = BuildProgram(formula);
            var executable = program.Compile();

            var results = executable.Call();
            var result = results.Result.Modules["Main"].Value["Formula01"] as NumberValue;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.EqualTo(expected).Within(1e-10));
        }

        private Program BuildProgram(string formula)
        {
            var program = new Program();

            var mathModule = MathModule.Build();
            var mathModuleConstant = program.AddConstant("Math", mathModule);

            var mainModule = program.AddModule("Main");
            mainModule.AddImport("Math", mathModuleConstant, false);
            mainModule.AddFormula("Formula01", formula);

            return program;
        }
    }
}