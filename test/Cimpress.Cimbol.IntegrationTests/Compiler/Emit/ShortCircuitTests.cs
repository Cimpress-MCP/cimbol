// Copyright 2021 Cimpress plc.
// Licensed under the Apache License, Version 2.0
// http://www.apache.org/licenses/LICENSE-2.0

using System;
using Cimpress.Cimbol.Runtime.Types;
using NUnit.Framework;

namespace Cimpress.Cimbol.IntegrationTests.Compiler.Emit
{
    [TestFixture]
    public class ShortCircuitTests
    {
        [Test]
        public void Should_ShortCircuitAndExpression_When_LeftOperandIsFalseAndRightOperandIsTrue()
        {
            var (functionValue, getVisited) = CreateMockFunction(true);

            var program = new Program();
            var constant1 = program.AddConstant("Constant1", functionValue);
            var module = program.AddModule("Module1");
            module.AddImport("Import1", constant1);
            module.AddFormula("Formula1", "false and Import1()");
            var executable = program.Compile();

            var result = executable.Call().Result;
            Assert.That(result.Errors, Is.Empty);
            Assert.That(result.Modules["Module1"].Value["Formula1"].CastBoolean().Value, Is.False);
            Assert.That(getVisited(), Is.False);
        }
        
        [Test]
        public void Should_ShortCircuitAndExpression_When_BothOperandsAreFalse()
        {
            var (functionValue, getVisited) = CreateMockFunction(false);

            var program = new Program();
            var constant1 = program.AddConstant("Constant1", functionValue);
            var module = program.AddModule("Module1");
            module.AddImport("Import1", constant1);
            module.AddFormula("Formula1", "false and Import1()");
            var executable = program.Compile();

            var result = executable.Call().Result;
            Assert.That(result.Errors, Is.Empty);
            Assert.That(result.Modules["Module1"].Value["Formula1"].CastBoolean().Value, Is.False);
            Assert.That(getVisited(), Is.False);
        }

        [Test]
        public void ShouldNot_ShortCircuitAndExpression_When_LeftOperandIsTrueAndRightOperandsIsFalse()
        {
            var (functionValue, getVisited) = CreateMockFunction(false);

            var program = new Program();
            var constant1 = program.AddConstant("Constant1", functionValue);
            var module = program.AddModule("Module1");
            module.AddImport("Import1", constant1);
            module.AddFormula("Formula1", "true and Import1()");
            var executable = program.Compile();

            var result = executable.Call().Result;
            Assert.That(result.Errors, Is.Empty);
            Assert.That(result.Modules["Module1"].Value["Formula1"].CastBoolean().Value, Is.False);
            Assert.That(getVisited(), Is.True);
        }

        [Test]
        public void ShouldNot_ShortCircuitAndExpression_When_BothOperandsAreTrue()
        {
            var (functionValue, getVisited) = CreateMockFunction(true);

            var program = new Program();
            var constant1 = program.AddConstant("Constant1", functionValue);
            var module = program.AddModule("Module1");
            module.AddImport("Import1", constant1);
            module.AddFormula("Formula1", "true and Import1()");
            var executable = program.Compile();

            var result = executable.Call().Result;
            Assert.That(result.Errors, Is.Empty);
            Assert.That(result.Modules["Module1"].Value["Formula1"].CastBoolean().Value, Is.True);
            Assert.That(getVisited(), Is.True);
        }

        [Test]
        public void Should_ShortCircuitOrExpression_When_LeftOperandIsTrueAndRightOperandIsFalse()
        {
            var (functionValue, getVisited) = CreateMockFunction(false);

            var program = new Program();
            var constant1 = program.AddConstant("Constant1", functionValue);
            var module = program.AddModule("Module1");
            module.AddImport("Import1", constant1);
            module.AddFormula("Formula1", "true or Import1()");
            var executable = program.Compile();

            var result = executable.Call().Result;
            Assert.That(result.Errors, Is.Empty);
            Assert.That(result.Modules["Module1"].Value["Formula1"].CastBoolean().Value, Is.True);
            Assert.That(getVisited(), Is.False);
        }

        [Test]
        public void Should_ShortCircuitOrExpression_When_BothOperandsAreTrue()
        {
            var (functionValue, getVisited) = CreateMockFunction(true);

            var program = new Program();
            var constant1 = program.AddConstant("Constant1", functionValue);
            var module = program.AddModule("Module1");
            module.AddImport("Import1", constant1);
            module.AddFormula("Formula1", "true or Import1()");
            var executable = program.Compile();

            var result = executable.Call().Result;
            Assert.That(result.Errors, Is.Empty);
            Assert.That(result.Modules["Module1"].Value["Formula1"].CastBoolean().Value, Is.True);
            Assert.That(getVisited(), Is.False);
        }

        [Test]
        public void ShouldNot_ShortCircuitOrExpression_When_LeftOperandIsFalseAndRightOperandsIsTrue()
        {
            var (functionValue, getVisited) = CreateMockFunction(true);

            var program = new Program();
            var constant1 = program.AddConstant("Constant1", functionValue);
            var module = program.AddModule("Module1");
            module.AddImport("Import1", constant1);
            module.AddFormula("Formula1", "false or Import1()");
            var executable = program.Compile();

            var result = executable.Call().Result;
            Assert.That(result.Errors, Is.Empty);
            Assert.That(result.Modules["Module1"].Value["Formula1"].CastBoolean().Value, Is.True);
            Assert.That(getVisited(), Is.True);
        }

        [Test]
        public void ShouldNot_ShortCircuitOrExpression_When_BothOperandsAreFalse()
        {
            var (functionValue, getVisited) = CreateMockFunction(false);

            var program = new Program();
            var constant1 = program.AddConstant("Constant1", functionValue);
            var module = program.AddModule("Module1");
            module.AddImport("Import1", constant1);
            module.AddFormula("Formula1", "false or Import1()");
            var executable = program.Compile();

            var result = executable.Call().Result;
            Assert.That(result.Errors, Is.Empty);
            Assert.That(result.Modules["Module1"].Value["Formula1"].CastBoolean().Value, Is.False);
            Assert.That(getVisited(), Is.True);
        }

        private (FunctionValue, Func<bool>) CreateMockFunction(bool returns)
        {
            var visited = false;

            BooleanValue Function()
            {
                visited = true;
                return returns ? BooleanValue.True : BooleanValue.False;
            }

            bool GetVisited() => visited;

            var functionValue = new FunctionValue(new[] { (Func<BooleanValue>)Function });

            return (functionValue, GetVisited);
        }
    }
}