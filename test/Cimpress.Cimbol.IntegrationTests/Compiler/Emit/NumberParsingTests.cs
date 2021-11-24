// Copyright 2021 Cimpress plc.
// Licensed under the Apache License, Version 2.0
// http://www.apache.org/licenses/LICENSE-2.0

using System.Linq;
using System.Threading.Tasks;
using Cimpress.Cimbol.Exceptions;
using NUnit.Framework;

namespace Cimpress.Cimbol.IntegrationTests.Compiler.Emit
{
    [TestFixture]
    public class NumberParsingTests
    {
        [Test]
        [TestCase("123", 123, TestName = "Integers")]
        [TestCase("1.23", 1.23, TestName = "Decimals")]
        [TestCase("1.2e3", 1200, TestName = "LowerCaseUnsignedScientificNotation")]
        [TestCase("1.2E3", 1200, TestName = "UpperCaseUnsignedScientificNotation")]
        [TestCase("1.2e+3", 1200, TestName = "LowerCasePositiveScientificNotation")]
        [TestCase("1.2E+3", 1200, TestName = "UpperCasePositiveScientificNotation")]
        [TestCase("1.2e-3", 0.0012, TestName = "LowerCaseNegativeScientificNotation")]
        [TestCase("1.2E-3", 0.0012, TestName = "UpperCaseNegativeScientificNotation")]
        public async Task Should_ParseNumbers_When_ParsingAtCompileTime(string expression, decimal expected)
        {
            var program = new Program();
            
            var module = program.AddModule("Main");
            module.AddFormula("Formula1", expression);

            var executable = program.Compile(CompilationProfile.Verbose);

            var result = await executable.Call();

            Assert.That(result.Modules["Main"].Value["Formula1"], Has.Property("Value").EqualTo(expected));
        }
        
        [Test]
        [TestCase("\"123\" + \"456\"", 579, TestName = "Integers")]
        [TestCase("\"1.23\" + \"4.56\"", 5.79, TestName = "Decimals")]
        [TestCase("\"3.2e1\" + \"6.5e4\"", 65032, TestName = "LowerCaseUnsignedScientificNotation")]
        [TestCase("\"3.2E1\" + \"6.5E4\"", 65032, TestName = "UpperCaseUnsignedScientificNotation")]
        [TestCase("\"3.2e-1\" + \"6.5e+4\"", 65000.32, TestName = "LowerCaseSignedScientificNotation")]
        [TestCase("\"3.2E-1\" + \"6.5E+4\"", 65000.32, TestName = "UpperCaseSignedScientificNotation")]
        public async Task Should_ParseNumbers_When_ParsingAtRuntime(string expression, decimal expected)
        {
            var program = new Program();

            var module = program.AddModule("Main");
            module.AddFormula("Formula1", expression);

            var executable = program.Compile(CompilationProfile.Verbose);

            var result = await executable.Call();

            Assert.That(result.Modules["Main"].Value["Formula1"], Has.Property("Value").EqualTo(expected));
        }

        [Test]
        [TestCase("\"123\" == 123", TestName = "Integers")]
        [TestCase("\"1.23\" == 1.23", TestName = "Decimals")]
        [TestCase("\"3.2e1\" == 3.2e1", TestName = "LowerCaseUnsignedScientificNotation")]
        [TestCase("\"3.2E1\" == 3.2E1", TestName = "UpperCaseUnsignedScientificNotation")]
        [TestCase("\"3.2e+1\" == 3.2e+1", TestName = "LowerCasePositiveScientificNotation")]
        [TestCase("\"3.2E+1\" == 3.2E+1", TestName = "UpperCasePositiveScientificNotation")]
        [TestCase("\"3.2e-1\" == 3.2e-1", TestName = "LowerCaseNegativeScientificNotation")]
        [TestCase("\"3.2E-1\" == 3.2E-1", TestName = "UpperCaseNegativeScientificNotation")]
        public async Task Should_ParseNumbers_When_DoingEquality(string expression)
        {
            var program = new Program();

            var module = program.AddModule("Main");
            module.AddFormula("Formula1", expression);

            var executable = program.Compile(CompilationProfile.Verbose);

            var result = await executable.Call();

            Assert.That(result.Modules["Main"].Value["Formula1"], Has.Property("Value").EqualTo(true));
        }

        [Test]
        [TestCase("1e30", TestName = "MoreThanMaxValue")]
        [TestCase("-1e30", TestName = "LessThanMinValue")]
        public async Task ShouldNot_ParseNumbersAtCompileTime_When_NumberIsInvalid(string expression)
        {
            var program = new Program();

            var module = program.AddModule("Main");
            module.AddFormula("Formula1", expression);

            Assert.That(
                () => program.Compile(CompilationProfile.Verbose),
                Throws.TypeOf<CimbolInternalException>().With.Property("Message").Contains("number"));
        }

        [Test]
        [TestCase("\"1e30\" + 0", TestName = "MoreThanMaxValue")]
        [TestCase("\"-1e30\" + 0", TestName = "LessThanMinValue")]
        public async Task ShouldNot_ParseNumbersAtRuntime_When_NumberIsInvalid(string expression)
        {
            var program = new Program();

            var module = program.AddModule("Main");
            module.AddFormula("Formula1", expression);

            var executable = program.Compile(CompilationProfile.Verbose);

            var result = await executable.Call();
            
            Assert.That(result.Errors, Has.Exactly(1).Items);
            Assert.That(result.Errors.ElementAt(0).Formula, Is.EqualTo("Formula1"));
            Assert.That(result.Errors.ElementAt(0).Message, Contains.Substring("number"));
        }
    }
}