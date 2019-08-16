using System;
using System.Threading.Tasks;
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

            var resultModule = result.Value["Main"] as ObjectValue;
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

            var resultModule = result.Value["Main"] as ObjectValue;
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

            var resultModule = result.Value["Main"] as ObjectValue;
            Assert.That(resultModule, Is.Not.Null);
            Assert.That(resultModule.Value["ResultA"], Has.Property("Value").EqualTo(5));
            Assert.That(resultModule.Value["ResultB"], Has.Property("Value").EqualTo(100));
            Assert.That(resultModule.Value["ResultC"], Has.Property("Value").EqualTo(1));
        }

        [Test]
        public void Should_CompileAndRun_When_GivenSmallProgramWithSingleAwait()
        {
            PromiseValue SomeFunction(NumberValue number)
            {
                async Task<ILocalValue> InnerFunction()
                {
                    await Task.Delay(1000);

                    return number;
                }

                return new PromiseValue(InnerFunction());
            }

            var program = new Program();
            var constantA = program.AddConstant(
                "ConstantA",
                new FunctionValue((Func<NumberValue, PromiseValue>)SomeFunction));
            var module = program.AddModule("Main");
            module.AddReference("ConstantA", constantA);
            module.AddFormula("ResultA", "await ConstantA(2)");
            var executable = program.Compile();

            var result = executable.Call().Result;

            var resultModule = result.Value["Main"] as ObjectValue;
            Assert.That(resultModule, Is.Not.Null);
            Assert.That(resultModule.Value["ResultA"], Has.Property("Value").EqualTo(2));
        }

        [Test]
        public void Should_CompileAndRun_When_GivenSmallProgramWithMultipleAwaits()
        {
            PromiseValue SomeFunction(NumberValue number)
            {
                Task<ILocalValue> InnerFunction()
                {
                    return Task.FromResult<ILocalValue>(number);
                }

                return new PromiseValue(InnerFunction());
            }

            var program = new Program();
            var constantA = program.AddConstant(
                "ConstantA",
                new FunctionValue((Func<NumberValue, PromiseValue>)SomeFunction));
            var module = program.AddModule("Main");
            module.AddReference("ConstantA", constantA);
            module.AddFormula("ResultA", "await ConstantA(2)");
            module.AddFormula("ResultB", "await ConstantA(ResultA + 1)");
            module.AddFormula("ResultC", "ResultA + ResultB");
            var executable = program.Compile();

            var result = executable.Call().Result;

            var resultModule = result.Value["Main"] as ObjectValue;
            Assert.That(resultModule, Is.Not.Null);
            Assert.That(resultModule.Value["ResultA"], Has.Property("Value").EqualTo(2));
            Assert.That(resultModule.Value["ResultB"], Has.Property("Value").EqualTo(3));
            Assert.That(resultModule.Value["ResultC"], Has.Property("Value").EqualTo(5));
        }

        [Test]
        public void Should_CompileAndRun_When_GivenSmallAsyncProgramFromBenchmarks()
        {
            PromiseValue AsyncFunction(NumberValue numberValue)
            {
                var newNumberValue = new NumberValue(numberValue.Value * 2);

                return new PromiseValue(Task.FromResult<ILocalValue>(newNumberValue));
            }

            var program = new Program();
            var constantValue1 = new FunctionValue((Func<NumberValue, PromiseValue>)AsyncFunction);
            var constant1 = program.AddConstant("AsyncFunction", constantValue1);
            var module = program.AddModule("main");
            module.AddReference("AsyncFunction", constant1);
            module.AddFormula("ResultA", "await AsyncFunction(2)");
            module.AddFormula("ResultB", "await AsyncFunction(4)");
            module.AddFormula("ResultC", "await AsyncFunction(ResultA + ResultB)");
            module.AddFormula("ResultD", "ResultA + ResultB + ResultC");
            var executable = program.Compile();

            var result = executable.Call().Result;

            var resultModule = result.Value["Main"] as ObjectValue;
            Assert.That(resultModule, Is.Not.Null);
            Assert.That(resultModule.Value["ResultA"], Has.Property("Value").EqualTo(4));
            Assert.That(resultModule.Value["ResultB"], Has.Property("Value").EqualTo(8));
            Assert.That(resultModule.Value["ResultC"], Has.Property("Value").EqualTo(24));
            Assert.That(resultModule.Value["ResultD"], Has.Property("Value").EqualTo(36));
        }
    }
}