// Copyright 2021 Cimpress plc.
// Licensed under the Apache License, Version 2.0
// http://www.apache.org/licenses/LICENSE-2.0

using System;
using System.Collections.Generic;
using System.Linq;
using Cimpress.Cimbol.Runtime.Types;
using NUnit.Framework;

namespace Cimpress.Cimbol.IntegrationTests.Compiler.Emit
{
    [TestFixture]
    public class DefaultEvaluationTests
    {
        [SetUp]
        public void SetUp()
        {
        }

        [Test]
        [TestCase(CompilationProfile.Minimal)]
        [TestCase(CompilationProfile.Trace)]
        [TestCase(CompilationProfile.Verbose)]
        public void Should_Default_When_IdentifierDoesNotExist(CompilationProfile compilationProfile)
        {
            var program = new Program();
            var module = program.AddModule("Main");
            module.AddFormula("Formula1", "default(Formula2, 1)");
            var executable = program.Compile(compilationProfile);

            var result = executable.Call().Result;

            Assert.That(result.Modules["Main"].Value["Formula1"], Has.Property("Value").EqualTo(1));
        }

        [Test]
        [TestCase(CompilationProfile.Minimal)]
        [TestCase(CompilationProfile.Trace)]
        [TestCase(CompilationProfile.Verbose)]
        public void Should_Default_When_ArgumentIsNull(CompilationProfile compilationProfile)
        {
            var program = new Program();
            var argument = program.AddArgument("Argument1");
            var module = program.AddModule("Main");
            module.AddImport("Argument1", argument);
            module.AddFormula("Formula1", "default(Argument1, 1)");
            var executable = program.Compile(compilationProfile);

            var result = executable.Call((ILocalValue)null).Result;

            Assert.That(result.Modules["Main"].Value["Formula1"], Has.Property("Value").EqualTo(1));
        }

        [Test]
        [TestCase(CompilationProfile.Minimal)]
        [TestCase(CompilationProfile.Trace)]
        [TestCase(CompilationProfile.Verbose)]
        public void Should_Default_When_FunctionReturnsNull(CompilationProfile compilationProfile)
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
            module.AddFormula("Formula1", "Constant1()");
            module.AddFormula("Formula2", "default(Formula1, 1)");
            var executable = program.Compile(compilationProfile);

            var result = executable.Call().Result;

            Assert.That(result.Modules["Main"].Value["Formula2"], Has.Property("Value").EqualTo(1));
        }

        [Test]
        [TestCase(CompilationProfile.Minimal)]
        [TestCase(CompilationProfile.Trace)]
        [TestCase(CompilationProfile.Verbose)]
        public void Should_Default_When_MemberDoesNotExist(CompilationProfile compilationProfile)
        {
            var program = new Program();
            var module = program.AddModule("Main");
            module.AddFormula("Formula1", "object(Member1 = 1)");
            module.AddFormula("Formula2", "default(Formula1.Member2, 1)");
            var executable = program.Compile(compilationProfile);

            var result = executable.Call().Result;

            Assert.That(result.Modules["Main"].Value["Formula2"], Has.Property("Value").EqualTo(1));
        }

        [Test]
        [TestCase(CompilationProfile.Minimal)]
        [TestCase(CompilationProfile.Trace)]
        [TestCase(CompilationProfile.Verbose)]
        public void ShouldNot_Default_When_VariableExists(CompilationProfile compilationProfile)
        {
            var program = new Program();
            var module = program.AddModule("Main");
            module.AddFormula("Formula1", "1");
            module.AddFormula("Formula2", "default(Formula1, 2)");
            var executable = program.Compile(compilationProfile);

            var result = executable.Call().Result;

            Assert.That(result.Modules["Main"].Value["Formula2"], Has.Property("Value").EqualTo(1));
        }

        [Test]
        [TestCase(CompilationProfile.Minimal)]
        [TestCase(CompilationProfile.Trace)]
        [TestCase(CompilationProfile.Verbose)]
        public void ShouldNot_Default_When_MemberExists(CompilationProfile compilationProfile)
        {
            var program = new Program();
            var module = program.AddModule("Main");
            module.AddFormula("Formula1", "object(Member1 = 1)");
            module.AddFormula("Formula2", "default(Formula1.Member1, 2)");
            var executable = program.Compile(compilationProfile);

            var result = executable.Call().Result;

            Assert.That(result.Modules["Main"].Value["Formula2"], Has.Property("Value").EqualTo(1));
        }

        [Test]
        [TestCase(CompilationProfile.Minimal)]
        [TestCase(CompilationProfile.Trace)]
        [TestCase(CompilationProfile.Verbose)]
        public void ShouldNot_Default_When_DefaultedFormulaHasError(CompilationProfile compilationProfile)
        {
            var program = new Program();
            var module = program.AddModule("Main");
            module.AddFormula("Formula1", "1 / 0");
            module.AddFormula("Formula2", "default(Formula1, 1)");
            var executable = program.Compile(compilationProfile);

            var result = executable.Call().Result;

            if (compilationProfile == CompilationProfile.Verbose)
            {
                Assert.That(result.Modules["Main"].Value["Formula2"], Is.Null);
            }
            else
            {
                Assert.That(result.Modules, Has.Count.EqualTo(0));
            }
        }

        [Test]
        [TestCase(CompilationProfile.Minimal)]
        [TestCase(CompilationProfile.Trace)]
        [TestCase(CompilationProfile.Verbose)]
        public void ShouldNot_Default_When_DefaultedFormulaWasSkipped(CompilationProfile compilationProfile)
        {
            var program = new Program();
            var module = program.AddModule("Main");
            module.AddFormula("Formula1", "1 / 0");
            module.AddFormula("Formula2", "Formula1");
            module.AddFormula("Formula3", "default(Formula2, 1)");
            var executable = program.Compile(compilationProfile);

            var result = executable.Call().Result;

            if (compilationProfile == CompilationProfile.Verbose)
            {
                Assert.That(result.Modules["Main"].Value["Formula3"], Is.Null);
            }
            else
            {
                Assert.That(result.Modules, Has.Count.EqualTo(0));
            }
        }
        
        [Test]
        [TestCase(CompilationProfile.Minimal)]
        [TestCase(CompilationProfile.Trace)]
        [TestCase(CompilationProfile.Verbose)]
        public void Should_Default_When_IdentifierIsNull(CompilationProfile compilationProfile)
        {
            var program = new Program();
            var module = program.AddModule("Main");
            
            var objectValue = new ObjectValue(new Dictionary<string, ILocalValue> { { "member", null } });
            var constant = program.AddConstant("something", objectValue);
            
            module.AddImport("something", constant);
            module.AddFormula("Formula1", "default(something.member, 1)");
            
            var executable = program.Compile(compilationProfile);

            var result = executable.Call().Result;

            Assert.That(result.Modules["Main"].Value["Formula1"], Has.Property("Value").EqualTo(1));
        }

        private static ILocalValue MockNestedObject(string[] path, ILocalValue value)
        {
            var current = value;

            foreach (var name in path.Reverse())
            {
                var inner = new Dictionary<string, ILocalValue>(StringComparer.OrdinalIgnoreCase)
                {
                    { name, current },
                };

                current = new ObjectValue(inner);
            }

            return current;
        }
    }
}