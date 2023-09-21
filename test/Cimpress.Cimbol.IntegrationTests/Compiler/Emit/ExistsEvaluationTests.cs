// Copyright 2021 Cimpress plc.
// Licensed under the Apache License, Version 2.0
// http://www.apache.org/licenses/LICENSE-2.0

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cimpress.Cimbol.Runtime.Types;
using NUnit.Framework;

namespace Cimpress.Cimbol.IntegrationTests.Compiler.Emit
{
    [TestFixture]
    public class ExistsEvaluationTests
    {
        [Test]
        [TestCase(CompilationProfile.Minimal)]
        [TestCase(CompilationProfile.Trace)]
        [TestCase(CompilationProfile.Verbose)]
        public async Task Should_ReturnBoolean_When_ObjectExists(CompilationProfile compilationProfile)
        {
            var program = new Program();

            var module = program.AddModule("Main");
            module.AddFormula("Formula2", "4");
            module.AddFormula("Formula1", "exists(Formula2)");

            var executable = program.Compile(compilationProfile);

            var result = await executable.Call();

            Assert.That(result.Modules["Main"].Value["Formula1"], Has.Property("Value").EqualTo(true));
        }

        [Test]
        [TestCase(CompilationProfile.Minimal)]
        [TestCase(CompilationProfile.Trace)]
        [TestCase(CompilationProfile.Verbose)]
        public void Should_ReturnBoolean_When_ObjectDoesNotExist(CompilationProfile compilationProfile)
        {
            var program = new Program();

            var module = program.AddModule("Main");
            module.AddFormula("Formula1", "exists(Formula2)");

            var executable = program.Compile(compilationProfile);

            var result = executable.Call().Result;

            Assert.That(result.Modules["Main"].Value["Formula1"], Has.Property("Value").EqualTo(false));
        }

        [Test]
        [TestCase(CompilationProfile.Minimal)]
        [TestCase(CompilationProfile.Trace)]
        [TestCase(CompilationProfile.Verbose)]
        public async Task Should_ReturnFalse_When_ObjectDoesNotHaveKey(CompilationProfile compilationProfile)
        {
            var program = new Program();

            var module = program.AddModule("Main");
            module.AddFormula("Formula2", "object(Member1 = 1)");
            module.AddFormula("Formula1", "exists(Formula2.NotAMember)");

            var executable = program.Compile(compilationProfile);

            var result = await executable.Call();

            Assert.That(result.Modules["Main"].Value["Formula1"], Has.Property("Value").EqualTo(false));
        }

        [Test]
        [TestCase(CompilationProfile.Minimal)]
        [TestCase(CompilationProfile.Trace)]
        [TestCase(CompilationProfile.Verbose)]
        public async Task Should_ReturnTrue_When_ObjectHasKey(CompilationProfile compilationProfile)
        {
            var program = new Program();

            var module = program.AddModule("Main");
            module.AddFormula("Formula2", "object(Member1 = 1)");
            module.AddFormula("Formula1", "exists(Formula2.Member1)");

            var executable = program.Compile(compilationProfile);

            var result = await executable.Call();

            Assert.That(result.Modules["Main"].Value["Formula1"], Has.Property("Value").EqualTo(true));
        }

        [Test]
        [TestCase(CompilationProfile.Minimal)]
        [TestCase(CompilationProfile.Trace)]
        [TestCase(CompilationProfile.Verbose)]
        public async Task Should_ReturnFalse_WhenObjectKeyIsNull(CompilationProfile compilationProfile)
        {
            NumberValue Function()
            {
                return null;
            }

            var program = new Program();
            var functionValue = new FunctionValue(new[] { (Func<NumberValue>)Function });
            var constant = program.AddConstant("Constant1", functionValue);

            var module = program.AddModule("Main");
            module.AddImport("Constant1", constant);
            module.AddFormula("Formula2", "object(Member1 = Constant1())");
            module.AddFormula("Formula1", "exists(Formula2.Member1)");

            var executable = program.Compile(compilationProfile);

            var result = await executable.Call();

            Assert.That(result.Modules["Main"].Value["Formula1"], Has.Property("Value").EqualTo(false));
        }

        [Test]
        [TestCase(CompilationProfile.Minimal)]
        [TestCase(CompilationProfile.Trace)]
        [TestCase(CompilationProfile.Verbose)]
        public async Task Should_ReturnFalse_WhenObjectIsNull(CompilationProfile compilationProfile)
        {
            NumberValue Function()
            {
                return null;
            }

            var program = new Program();
            var functionValue = new FunctionValue(new[] { (Func<NumberValue>)Function });
            var constant = program.AddConstant("Constant1", functionValue);

            var module = program.AddModule("Main");
            module.AddImport("Constant1", constant);
            module.AddFormula("Formula2", "Constant1)");
            module.AddFormula("Formula1", "exists(Formula2.Member1)");

            var executable = program.Compile(compilationProfile);

            var result = await executable.Call();

            Assert.That(result.Modules["Main"].Value["Formula1"], Has.Property("Value").EqualTo(false));
        }

        [Test]
        [TestCase(CompilationProfile.Minimal)]
        [TestCase(CompilationProfile.Trace)]
        [TestCase(CompilationProfile.Verbose)]
        public async Task Should_ReturnFalse_When_ObjectMemberIsNull(CompilationProfile compilationProfile)
        {
            NumberValue Function()
            {
                return null;
            }

            var program = new Program();
            var objectValue = new ObjectValue(new Dictionary<string, ILocalValue> { { "member", null } });
            var constant = program.AddConstant("something", objectValue);

            var module = program.AddModule("Main");
            module.AddImport("something", constant);
            module.AddFormula("Formula1", "exists(something.member)");

            var executable = program.Compile(compilationProfile);

            var result = await executable.Call();

            Assert.That(result.Modules["Main"].Value["Formula1"], Has.Property("Value").EqualTo(false));
        }
        
        [Test]
        [TestCase(CompilationProfile.Minimal)]
        [TestCase(CompilationProfile.Trace)]
        [TestCase(CompilationProfile.Verbose)]
        public async Task Should_ReturnTrue_When_ObjectMemberNameContainsWhitespace(CompilationProfile compilationProfile)
        {
            var program = new Program();
            var objectValue = new ObjectValue(new Dictionary<string, ILocalValue> { { "member name", new StringValue("1") } });
            var constant = program.AddConstant("something", objectValue);

            var module = program.AddModule("Main");
            module.AddImport("something", constant);
            module.AddFormula("Formula1", "exists(something.'member name')");

            var executable = program.Compile(compilationProfile);

            var result = await executable.Call();

            Assert.That(result.Modules["Main"].Value["Formula1"], Has.Property("Value").EqualTo(true));
        }
        
        [Test]
        [TestCase(CompilationProfile.Minimal)]
        [TestCase(CompilationProfile.Trace)]
        [TestCase(CompilationProfile.Verbose)]
        public async Task Should_ReturnTrue_When_ImportNameContainsWhitespace(CompilationProfile compilationProfile)
        {
            var program = new Program();
            var objectValue = new ObjectValue(new Dictionary<string, ILocalValue> { { "member name", new StringValue("1") } });
            var constant = program.AddConstant("something with spaces", objectValue);

            var module = program.AddModule("Main");
            module.AddImport("something with spaces", constant);
            module.AddFormula("Formula1", "exists('something with spaces'.'member name')");

            var executable = program.Compile(compilationProfile);

            var result = await executable.Call();

            Assert.That(result.Modules["Main"].Value["Formula1"], Has.Property("Value").EqualTo(true));
        }
    }
}
