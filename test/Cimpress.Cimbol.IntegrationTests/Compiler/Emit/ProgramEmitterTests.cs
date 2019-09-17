using Cimpress.Cimbol.Runtime.Types;
using NUnit.Framework;

namespace Cimpress.Cimbol.IntegrationTests.Compiler.Emit
{
    [TestFixture]
    public class ProgramEmitterTests
    {
        [Test]
        public void Should_CompileAndRun_When_GivenSmallPureProgram()
        {
            var program = new Program();
            var module = program.AddModule("Main");
            module.AddFormula("ResultA", "ResultB + ResultC + ResultD");
            module.AddFormula("ResultB", "ResultE * 20");
            module.AddFormula("ResultC", "ResultD + 30");
            module.AddFormula("ResultD", "136");
            module.AddFormula("ResultE", "200");
            var executable = program.Compile();

            var result = executable.Call().Result;

            Assert.That(result.Errors, Has.Length.EqualTo(0));
            Assert.That(result.Modules, Has.Count.EqualTo(1));
            var resultModule = result.Modules["Main"];
            Assert.That(resultModule, Is.Not.Null);
            Assert.That(resultModule.Value["ResultA"], Has.Property("Value").EqualTo(4302));
            Assert.That(resultModule.Value["ResultB"], Has.Property("Value").EqualTo(4000));
            Assert.That(resultModule.Value["ResultC"], Has.Property("Value").EqualTo(166));
            Assert.That(resultModule.Value["ResultD"], Has.Property("Value").EqualTo(136));
            Assert.That(resultModule.Value["ResultE"], Has.Property("Value").EqualTo(200));
        }

        [Test]
        public void Should_CompileAndRun_When_GivenSmallProgramWithArguments()
        {
            var program = new Program();
            var argumentA = program.AddArgument("ArgumentA");
            var argumentB = program.AddArgument("ArgumentB");
            var argumentC = program.AddArgument("ArgumentC");
            var module = program.AddModule("Main");
            module.AddReference("ArgumentA", argumentA);
            module.AddReference("ArgumentB", argumentB);
            module.AddReference("ArgumentC", argumentC);
            module.AddFormula("ResultA", "ArgumentA");
            module.AddFormula("ResultB", "ArgumentB * 10");
            module.AddFormula("ResultC", "ArgumentC % 4");
            var executable = program.Compile();

            var result = executable.Call(new NumberValue(5), new NumberValue(10), new NumberValue(17)).Result;

            Assert.That(result.Errors, Has.Length.EqualTo(0));
            Assert.That(result.Modules, Has.Count.EqualTo(1));
            var resultModule = result.Modules["Main"];
            Assert.That(resultModule, Is.Not.Null);
            Assert.That(resultModule.Value["ResultA"], Has.Property("Value").EqualTo(5));
            Assert.That(resultModule.Value["ResultB"], Has.Property("Value").EqualTo(100));
            Assert.That(resultModule.Value["ResultC"], Has.Property("Value").EqualTo(1));
        }

        [Test]
        public void Should_CompileAndRun_When_GivenSmallProgramWithConstants()
        {
            var program = new Program();
            var constantA = program.AddConstant("ConstantA", new NumberValue(5));
            var constantB = program.AddConstant("ConstantB", new NumberValue(10));
            var constantC = program.AddConstant("ConstantC", new NumberValue(17));
            var module = program.AddModule("Main");
            module.AddReference("ConstantA", constantA);
            module.AddReference("ConstantB", constantB);
            module.AddReference("ConstantC", constantC);
            module.AddFormula("ResultA", "ConstantA");
            module.AddFormula("ResultB", "ConstantB * 10");
            module.AddFormula("ResultC", "ConstantC % 4");
            var executable = program.Compile();

            var result = executable.Call().Result;

            Assert.That(result.Errors, Has.Length.EqualTo(0));
            Assert.That(result.Modules, Has.Count.EqualTo(1));
            var resultModule = result.Modules["Main"];
            Assert.That(resultModule, Is.Not.Null);
            Assert.That(resultModule.Value["ResultA"], Has.Property("Value").EqualTo(5));
            Assert.That(resultModule.Value["ResultB"], Has.Property("Value").EqualTo(100));
            Assert.That(resultModule.Value["ResultC"], Has.Property("Value").EqualTo(1));
        }

        [Test]
        public void Should_CompileAndRun_When_GivenFormulaWithSpaces()
        {
            var program = new Program();
            var module = program.AddModule("Main");
            module.AddFormula("My Formula", "1");
            module.AddFormula("Other Formula", "'My Formula' + 1");
            var executable = program.Compile();

            var result = executable.Call().Result;

            Assert.That(result.Errors, Has.Length.EqualTo(0));
            Assert.That(result.Modules, Has.Count.EqualTo(1));
            var resultModule = result.Modules["Main"];
            Assert.That(resultModule, Is.Not.Null);
            Assert.That(resultModule.Value["My Formula"], Has.Property("Value").EqualTo(1));
            Assert.That(resultModule.Value["Other Formula"], Has.Property("Value").EqualTo(2));
        }

        [Test]
        public void Should_CompileAndRun_When_GivenObjectWithPropertiesWithSpaces()
        {
            var program = new Program();
            var module = program.AddModule("Main");
            module.AddFormula("Formula1", "object('Some Key' = 1)");
            module.AddFormula("Formula2", "Formula1.'Some Key' + 1");
            var executable = program.Compile();

            var result = executable.Call().Result;

            Assert.That(result.Errors, Has.Length.EqualTo(0));
            Assert.That(result.Modules, Has.Count.EqualTo(1));
            var resultModule = result.Modules["Main"];
            Assert.That(resultModule, Is.Not.Null);
            Assert.That(resultModule.Value["Formula2"], Has.Property("Value").EqualTo(2));
        }

        [Test]
        public void Should_CompileAndRun_When_AccessingExportedValueWithSpaces()
        {
            var program = new Program();
            var module1 = program.AddModule("Other");
            module1.AddFormula("Formula1 With Spaces", "1");
            var module2 = program.AddModule("Main");
            module2.AddReference("Other", module1);
            module2.AddFormula("Formula1", "Other.'Formula1 With Spaces' + 1");
            var executable = program.Compile();

            var result = executable.Call().Result;

            Assert.That(result.Errors, Has.Length.EqualTo(0));
            Assert.That(result.Modules, Has.Count.EqualTo(2));
            var resultModule = result.Modules["Main"];
            Assert.That(resultModule, Is.Not.Null);
            Assert.That(resultModule.Value["Formula1"], Has.Property("Value").EqualTo(2));
        }
    }
}