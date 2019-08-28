using Cimpress.Cimbol.Exceptions;
using Cimpress.Cimbol.Runtime.Types;
using NUnit.Framework;

namespace Cimpress.Cimbol.IntegrationTests.Compiler.Emit
{
    [TestFixture]
    public class PartialEvaluationTests
    {
        [Test]
        public void Should_CompileAndRun_When_GivenProgramWithSingleError()
        {
            var program = new Program();
            var module = program.AddModule("Main");
            module.AddFormula("Formula1", "3 / 0");
            module.AddFormula("Formula2", "1");
            module.AddFormula("Formula3", "Formula2 + 1");
            var executable = program.Compile();

            var result = executable.Call().Result;

            var resultModule = result.Value["Main"] as ObjectValue;
            Assert.That(resultModule, Is.Not.Null);
            Assert.That(resultModule.Value, Has.Count.EqualTo(3));
            Assert.That(resultModule.Value["Formula2"], Has.Property("Value").EqualTo(1));
            Assert.That(resultModule.Value["Formula3"], Has.Property("Value").EqualTo(2));
            var resultErrorFormula = resultModule.Value["Formula1"] as ErrorValue;
            Assert.That(resultErrorFormula, Is.Not.Null);
            Assert.That(resultErrorFormula.Value, Is.Not.Null);
            Assert.That(resultErrorFormula.Value, Is.InstanceOf<CimbolRuntimeException>());
        }

        [Test]
        public void Should_CompileAndRun_When_GivenProgramWithMultipleErrors()
        {
            var program = new Program();
            var module = program.AddModule("Main");
            module.AddFormula("Formula1", "3 / 0");
            module.AddFormula("Formula2", "3 / 0");
            module.AddFormula("Formula3", "1");
            var executable = program.Compile();

            var result = executable.Call().Result;

            var resultModule = result.Value["Main"] as ObjectValue;
            Assert.That(resultModule, Is.Not.Null);
            Assert.That(resultModule.Value, Has.Count.EqualTo(3));
            Assert.That(resultModule.Value["Formula3"], Has.Property("Value").EqualTo(1));
            var resultErrorFormula1 = resultModule.Value["Formula1"] as ErrorValue;
            Assert.That(resultErrorFormula1, Is.Not.Null);
            Assert.That(resultErrorFormula1.Value, Is.Not.Null);
            Assert.That(resultErrorFormula1.Value, Is.InstanceOf<CimbolRuntimeException>());
            var resultErrorFormula2 = resultModule.Value["Formula1"] as ErrorValue;
            Assert.That(resultErrorFormula2, Is.Not.Null);
            Assert.That(resultErrorFormula2.Value, Is.Not.Null);
            Assert.That(resultErrorFormula2.Value, Is.InstanceOf<CimbolRuntimeException>());
        }

        [Test]
        public void Should_CompileAndRun_When_GivenProgramWithFormulaDependentOnError()
        {
            var program = new Program();
            var module = program.AddModule("Main");
            module.AddFormula("Formula1", "3 / 0");
            module.AddFormula("Formula2", "1");
            module.AddFormula("Formula3", "Formula1 + 1");
            var executable = program.Compile();

            var result = executable.Call().Result;

            var resultModule = result.Value["Main"] as ObjectValue;
            Assert.That(resultModule, Is.Not.Null);
            Assert.That(resultModule.Value, Has.Count.EqualTo(3));
            Assert.That(resultModule.Value["Formula2"], Has.Property("Value").EqualTo(1));
            var resultErrorFormula = resultModule.Value["Formula1"] as ErrorValue;
            Assert.That(resultErrorFormula, Is.Not.Null);
            Assert.That(resultErrorFormula.Value, Is.Not.Null);
            Assert.That(resultErrorFormula.Value, Is.InstanceOf<CimbolRuntimeException>());
            var resultSkipFormula = resultModule.Value["Formula3"] as ErrorValue;
            Assert.That(resultSkipFormula, Is.Not.Null);
            Assert.That(resultSkipFormula.Value, Is.Null);
        }
    }
}