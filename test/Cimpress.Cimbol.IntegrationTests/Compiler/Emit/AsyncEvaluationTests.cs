using System;
using System.Threading.Tasks;
using Cimpress.Cimbol.Runtime.Types;
using NUnit.Framework;

namespace Cimpress.Cimbol.IntegrationTests.Compiler.Emit
{
    [TestFixture]
    public class AsyncEvaluationTests
    {
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