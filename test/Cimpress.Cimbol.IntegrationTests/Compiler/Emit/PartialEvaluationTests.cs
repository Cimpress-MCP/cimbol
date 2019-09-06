using System.Linq;
using Cimpress.Cimbol.Exceptions;
using NUnit.Framework;

namespace Cimpress.Cimbol.IntegrationTests.Compiler.Emit
{
    [TestFixture]
    public class PartialEvaluationTests
    {
        [Test]
        public void Should_CompileAndRunMinimal_When_GivenProgramWithSingleError()
        {
            var program = new Program();
            var module = program.AddModule("Main");
            module.AddFormula("Formula1", "3 / 0");
            module.AddFormula("Formula2", "1");
            module.AddFormula("Formula3", "Formula2 + 1");
            var executable = program.Compile(CompilationProfile.Minimal);

            var result = executable.Call().Result;

            var resultError = result.Errors.First();
            Assert.That(resultError, Is.InstanceOf<CimbolRuntimeException>());
            Assert.That(resultError.Formula, Is.Null);
            Assert.That(resultError.Module, Is.Null);
        }

        [Test]
        public void Should_CompileAndRunMinimal_When_GivenProgramWithMultipleErrors()
        {
            var program = new Program();
            var module = program.AddModule("Main");
            module.AddFormula("Formula1", "3 / 0");
            module.AddFormula("Formula2", "3 / 0");
            module.AddFormula("Formula3", "1");
            var executable = program.Compile(CompilationProfile.Minimal);

            var result = executable.Call().Result;

            var resultError = result.Errors.First();
            Assert.That(resultError, Is.InstanceOf<CimbolRuntimeException>());
            Assert.That(resultError.Formula, Is.Null);
            Assert.That(resultError.Module, Is.Null);
        }

        [Test]
        public void Should_CompileAndRunMinimal_When_GivenProgramWithFormulaDependentOnError()
        {
            var program = new Program();
            var module = program.AddModule("Main");
            module.AddFormula("Formula1", "3 / 0");
            module.AddFormula("Formula2", "1");
            module.AddFormula("Formula3", "Formula1 + 1");
            var executable = program.Compile(CompilationProfile.Minimal);

            var result = executable.Call().Result;

            var resultError = result.Errors.First();
            Assert.That(resultError, Is.InstanceOf<CimbolRuntimeException>());
            Assert.That(resultError.Formula, Is.Null);
            Assert.That(resultError.Module, Is.Null);
        }

        [Test]
        public void Should_CompileAndRunTrace_When_GivenProgramWithSingleError()
        {
            var program = new Program();
            var module = program.AddModule("Main");
            module.AddFormula("Formula1", "3 / 0");
            module.AddFormula("Formula2", "1");
            module.AddFormula("Formula3", "Formula2 + 1");
            var executable = program.Compile(CompilationProfile.Trace);

            var result = executable.Call().Result;

            var resultError = result.Errors.First();
            Assert.That(resultError, Is.InstanceOf<CimbolRuntimeException>());
            Assert.That(resultError.Formula, Is.EqualTo("Formula1"));
            Assert.That(resultError.Module, Is.EqualTo("Main"));
        }

        [Test]
        public void Should_CompileAndRunTrace_When_GivenProgramWithMultipleErrors()
        {
            var program = new Program();
            var module = program.AddModule("Main");
            module.AddFormula("Formula1", "3 / 0");
            module.AddFormula("Formula2", "3 / 0");
            module.AddFormula("Formula3", "1");
            var executable = program.Compile(CompilationProfile.Trace);

            var result = executable.Call().Result;

            var resultError = result.Errors.First();
            Assert.That(resultError, Is.InstanceOf<CimbolRuntimeException>());
            Assert.That(resultError.Formula, Is.EqualTo("Formula1").Or.EqualTo("Formula2"));
            Assert.That(resultError.Module, Is.EqualTo("Main"));
        }

        [Test]
        public void Should_CompileAndRunTrace_When_GivenProgramWithFormulaDependentOnError()
        {
            var program = new Program();
            var module = program.AddModule("Main");
            module.AddFormula("Formula1", "3 / 0");
            module.AddFormula("Formula2", "1");
            module.AddFormula("Formula3", "Formula1 + 1");
            var executable = program.Compile(CompilationProfile.Trace);

            var result = executable.Call().Result;

            var resultError = result.Errors.First();
            Assert.That(resultError, Is.InstanceOf<CimbolRuntimeException>());
            Assert.That(resultError.Formula, Is.EqualTo("Formula1"));
            Assert.That(resultError.Module, Is.EqualTo("Main"));
        }

        [Test]
        public void Should_CompileAndRunVerbose_When_GivenProgramWithSingleError()
        {
            var program = new Program();
            var module = program.AddModule("Main");
            module.AddFormula("Formula1", "3 / 0");
            module.AddFormula("Formula2", "1");
            module.AddFormula("Formula3", "Formula2 + 1");
            var executable = program.Compile(CompilationProfile.Verbose);

            var result = executable.Call().Result;

            Assert.That(result.Errors, Has.Length.EqualTo(1));
            Assert.That(result.Modules, Has.Count.EqualTo(1));
            var resultModule = result.Modules["Main"];
            Assert.That(resultModule, Is.Not.Null);
            Assert.That(resultModule.Value, Has.Count.EqualTo(3));
            Assert.That(resultModule.Value["Formula1"], Is.Null);
            Assert.That(resultModule.Value["Formula2"], Has.Property("Value").EqualTo(1));
            Assert.That(resultModule.Value["Formula3"], Has.Property("Value").EqualTo(2));
            var resultError1 = result.Errors.First(x => x.Formula == "Formula1");
            Assert.That(resultError1, Is.InstanceOf<CimbolRuntimeException>());
            Assert.That(resultError1.Formula, Is.EqualTo("Formula1"));
            Assert.That(resultError1.Module, Is.EqualTo("Main"));
        }

        [Test]
        public void Should_CompileAndRunVerbose_When_GivenProgramWithMultipleErrors()
        {
            var program = new Program();
            var module = program.AddModule("Main");
            module.AddFormula("Formula1", "3 / 0");
            module.AddFormula("Formula2", "3 / 0");
            module.AddFormula("Formula3", "1");
            var executable = program.Compile(CompilationProfile.Verbose);

            var result = executable.Call().Result;

            Assert.That(result.Errors, Has.Length.EqualTo(2));
            Assert.That(result.Modules, Has.Count.EqualTo(1));
            var resultModule = result.Modules["Main"];
            Assert.That(resultModule, Is.Not.Null);
            Assert.That(resultModule.Value, Has.Count.EqualTo(3));
            Assert.That(resultModule.Value["Formula1"], Is.Null);
            Assert.That(resultModule.Value["Formula2"], Is.Null);
            Assert.That(resultModule.Value["Formula3"], Has.Property("Value").EqualTo(1));
            var resultError1 = result.Errors.First(x => x.Formula == "Formula1");
            Assert.That(resultError1, Is.InstanceOf<CimbolRuntimeException>());
            Assert.That(resultError1.Formula, Is.EqualTo("Formula1"));
            Assert.That(resultError1.Module, Is.EqualTo("Main"));
            var resultError2 = result.Errors.First(x => x.Formula == "Formula2");
            Assert.That(resultError2, Is.InstanceOf<CimbolRuntimeException>());
            Assert.That(resultError2.Formula, Is.EqualTo("Formula2"));
            Assert.That(resultError2.Module, Is.EqualTo("Main"));
        }

        [Test]
        public void Should_CompileAndRunVerbose_When_GivenProgramWithFormulaDependentOnError()
        {
            var program = new Program();
            var module = program.AddModule("Main");
            module.AddFormula("Formula1", "3 / 0");
            module.AddFormula("Formula2", "1");
            module.AddFormula("Formula3", "Formula1 + 1");
            var executable = program.Compile(CompilationProfile.Verbose);

            var result = executable.Call().Result;

            Assert.That(result.Errors, Has.Length.EqualTo(2));
            Assert.That(result.Modules, Has.Count.EqualTo(1));
            var resultModule = result.Modules["Main"];
            Assert.That(resultModule, Is.Not.Null);
            Assert.That(resultModule.Value, Has.Count.EqualTo(3));
            Assert.That(resultModule.Value["Formula1"], Is.Null);
            Assert.That(resultModule.Value["Formula2"], Has.Property("Value").EqualTo(1));
            Assert.That(resultModule.Value["Formula3"], Is.Null);
            var resultError1 = result.Errors.First(x => x.Formula == "Formula1");
            Assert.That(resultError1, Is.InstanceOf<CimbolRuntimeException>());
            Assert.That(resultError1.Formula, Is.EqualTo("Formula1"));
            Assert.That(resultError1.Module, Is.EqualTo("Main"));
            var resultError2 = result.Errors.First(x => x.Formula == "Formula3");
            Assert.That(resultError2, Is.InstanceOf<CimbolRuntimeException>());
            Assert.That(resultError2.Formula, Is.EqualTo("Formula3"));
            Assert.That(resultError2.Module, Is.EqualTo("Main"));
        }
    }
}