// Copyright 2021 Cimpress plc.
// Licensed under the Apache License, Version 2.0
// http://www.apache.org/licenses/LICENSE-2.0

using Cimpress.Cimbol.Runtime.Types;
using NUnit.Framework;

namespace Cimpress.Cimbol.IntegrationTests.Compiler.Emit
{
    [TestFixture]
    public class ImportEvaluationTests
    {
        [Test]
        [TestCase(CompilationProfile.Minimal)]
        [TestCase(CompilationProfile.Trace)]
        [TestCase(CompilationProfile.Verbose)]
        public void Should_HaveExportExposed_When_ExportingImport(CompilationProfile compilationProfile)
        {
            var program = new Program();
            var argument = program.AddArgument("Argument01");
            var module1 = program.AddModule("Module01");
            module1.AddImport("Import01", argument, true);
            var module2 = program.AddModule("Module02");
            module2.AddImport("Import02", module1, true);
            module2.AddFormula("Formula01", "Import02.Import01 + 1");
            var executable = program.Compile(compilationProfile);

            var result = executable.Call(new NumberValue(1)).Result;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Errors, Has.Length.EqualTo(0));
            Assert.That(result.Modules, Has.Count.EqualTo(2));
            result.Modules.TryGetValue("Module01", out var resultModule1);
            Assert.That(resultModule1, Is.Not.Null);
            Assert.That(resultModule1.Value, Contains.Key("Import01"));
            Assert.That(resultModule1.Value["Import01"], Has.Property("Value").EqualTo(1));
            result.Modules.TryGetValue("Module02", out var resultModule2);
            Assert.That(resultModule2, Is.Not.Null);
            Assert.That(resultModule2.Value, Contains.Key("Import02"));
            var resultImport = resultModule2.Value["Import02"] as ObjectValue;
            Assert.That(resultImport, Is.Not.Null);
            Assert.That(resultImport.Value, Contains.Key("Import01"));
            Assert.That(resultImport.Value["Import01"], Has.Property("Value").EqualTo(1));
            Assert.That(resultModule2.Value, Contains.Key("Formula01"));
            Assert.That(resultModule2.Value["Formula01"], Has.Property("Value").EqualTo(2));
        }

        [Test]
        [TestCase(CompilationProfile.Minimal)]
        [TestCase(CompilationProfile.Trace)]
        [TestCase(CompilationProfile.Verbose)]
        public void Should_PassThroughImport_When_UsingDefaultOnImportedValue(CompilationProfile compilationProfile)
        {
            var program = new Program();
            var argument = program.AddArgument("Argument01");
            var module1 = program.AddModule("Module01");
            module1.AddImport("Import01", argument, true);
            var module2 = program.AddModule("Module02");
            module2.AddImport("Import02", module1, true);
            module2.AddFormula("Formula01", "default(Import02.Import01, 5)");
            var executable = program.Compile(compilationProfile);

            var result = executable.Call(new ILocalValue[] { null }).Result;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Errors, Has.Length.EqualTo(0));
            Assert.That(result.Modules, Has.Count.EqualTo(2));
            result.Modules.TryGetValue("Module01", out var resultModule1);
            Assert.That(resultModule1, Is.Not.Null);
            Assert.That(resultModule1.Value, Contains.Key("Import01"));
            Assert.That(resultModule1.Value["Import01"], Is.EqualTo(null));
            result.Modules.TryGetValue("Module02", out var resultModule2);
            Assert.That(resultModule2, Is.Not.Null);
            Assert.That(resultModule2.Value, Contains.Key("Import02"));
            var resultImport = resultModule2.Value["Import02"] as ObjectValue;
            Assert.That(resultImport, Is.Not.Null);
            Assert.That(resultImport.Value, Contains.Key("Import01"));
            Assert.That(resultImport.Value["Import01"], Is.Null);
            Assert.That(resultModule2.Value, Contains.Key("Formula01"));
            Assert.That(resultModule2.Value["Formula01"], Has.Property("Value").EqualTo(5));
        }
    }
}