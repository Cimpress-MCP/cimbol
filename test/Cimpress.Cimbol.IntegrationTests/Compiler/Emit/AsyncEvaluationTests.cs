﻿// Copyright 2021 Cimpress plc.
// Licensed under the Apache License, Version 2.0
// http://www.apache.org/licenses/LICENSE-2.0

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
        [TestCase(CompilationProfile.Minimal)]
        [TestCase(CompilationProfile.Trace)]
        [TestCase(CompilationProfile.Verbose)]
        public void Should_CompileAndRun_When_GivenSmallProgramWithSingleAwait(CompilationProfile compilationProfile)
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
                new FunctionValue(new[] { (Func<NumberValue, PromiseValue>)SomeFunction }));
            var module = program.AddModule("Main");
            module.AddImport("ConstantA", constantA);
            module.AddFormula("ResultA", "await ConstantA(2)");
            var executable = program.Compile(compilationProfile);

            var result = executable.Call().Result;

            Assert.That(result.Errors, Has.Length.EqualTo(0));
            Assert.That(result.Modules, Has.Count.EqualTo(1));
            var resultModule = result.Modules["Main"];
            Assert.That(resultModule, Is.Not.Null);
            Assert.That(resultModule.Value["ResultA"], Has.Property("Value").EqualTo(2));
        }

        [Test]
        [TestCase(CompilationProfile.Minimal)]
        [TestCase(CompilationProfile.Trace)]
        [TestCase(CompilationProfile.Verbose)]
        public void Should_CompileAndRun_When_GivenSmallProgramWithMultipleAwaits(CompilationProfile compilationProfile)
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
                new FunctionValue(new[] { (Func<NumberValue, PromiseValue>)SomeFunction }));
            var module = program.AddModule("Main");
            module.AddImport("ConstantA", constantA);
            module.AddFormula("ResultA", "await ConstantA(2)");
            module.AddFormula("ResultB", "await ConstantA(ResultA + 1)");
            module.AddFormula("ResultC", "ResultA + ResultB");
            var executable = program.Compile(compilationProfile);

            var result = executable.Call().Result;

            Assert.That(result.Errors, Has.Length.EqualTo(0));
            Assert.That(result.Modules, Has.Count.EqualTo(1));
            var resultModule = result.Modules["Main"];
            Assert.That(resultModule, Is.Not.Null);
            Assert.That(resultModule.Value["ResultA"], Has.Property("Value").EqualTo(2));
            Assert.That(resultModule.Value["ResultB"], Has.Property("Value").EqualTo(3));
            Assert.That(resultModule.Value["ResultC"], Has.Property("Value").EqualTo(5));
        }

        [Test]
        [TestCase(CompilationProfile.Minimal)]
        [TestCase(CompilationProfile.Trace)]
        [TestCase(CompilationProfile.Verbose)]
        public void Should_CompileAndRun_When_GivenSmallAsyncProgramFromBenchmarks(CompilationProfile compilationProfile)
        {
            PromiseValue AsyncFunction(NumberValue numberValue)
            {
                var newNumberValue = new NumberValue(numberValue.Value * 2);

                return new PromiseValue(Task.FromResult<ILocalValue>(newNumberValue));
            }

            var program = new Program();
            var constantValue1 = new FunctionValue(new[] { (Func<NumberValue, PromiseValue>)AsyncFunction });
            var constant1 = program.AddConstant("AsyncFunction", constantValue1);
            var module = program.AddModule("main");
            module.AddImport("AsyncFunction", constant1);
            module.AddFormula("ResultA", "await AsyncFunction(2)");
            module.AddFormula("ResultB", "await AsyncFunction(4)");
            module.AddFormula("ResultC", "await AsyncFunction(ResultA + ResultB)");
            module.AddFormula("ResultD", "ResultA + ResultB + ResultC");
            var executable = program.Compile(compilationProfile);

            var result = executable.Call().Result;

            Assert.That(result.Errors, Has.Length.EqualTo(0));
            Assert.That(result.Modules, Has.Count.EqualTo(1));
            var resultModule = result.Modules["Main"];
            Assert.That(resultModule, Is.Not.Null);
            Assert.That(resultModule.Value["ResultA"], Has.Property("Value").EqualTo(4));
            Assert.That(resultModule.Value["ResultB"], Has.Property("Value").EqualTo(8));
            Assert.That(resultModule.Value["ResultC"], Has.Property("Value").EqualTo(24));
            Assert.That(resultModule.Value["ResultD"], Has.Property("Value").EqualTo(36));
        }
    }
}