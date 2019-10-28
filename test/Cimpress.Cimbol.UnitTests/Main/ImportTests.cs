using System;
using Cimpress.Cimbol.Compiler.SyntaxTree;
using Cimpress.Cimbol.Runtime.Types;
using NUnit.Framework;

namespace Cimpress.Cimbol.UnitTests.Main
{
    [TestFixture]
    public class ImportTests
    {
        [Test]
        public void Should_ThrowArgumentNullException_When_ConstructedWithNullModule()
        {
            var argument = new Argument(new Program(), "x");

            Assert.Throws<ArgumentNullException>(() => new Import(null, "y", argument, false));
        }

        [Test]
        public void Should_ThrowArgumentNullException_When_ConstructedWithNullName()
        {
            var program = new Program();
            var module = new Module(program, "x");
            var argument = new Argument(program, "y");

            Assert.Throws<ArgumentNullException>(() => new Import(module, null, argument, false));
        }

        [Test]
        public void Should_ThrowArgumentNullException_When_ConstructedWithNullValue()
        {
            var module = new Module(new Program(), "x");

            Assert.Throws<ArgumentNullException>(() => new Import(module, "y", null, false));
        }

        [Test]
        public void Should_UnnestImportedValue_When_ConstructedWithImportAsValue()
        {
            var program = new Program();
            var argument = new Argument(program, "Argument01");
            var module = new Module(program, "Module01");
            var parentImport = new Import(module, "Import01", argument, false);
            var import = new Import(module, "Import02", parentImport, false);

            var result = import.Value;

            Assert.That(result, Is.SameAs(argument));
        }

        [Test]
        public void ShouldNot_UnnestImportedValue_When_ConstructWithArgumentAsValue()
        {
            var program = new Program();
            var argument = new Argument(program, "Argument01");
            var module = new Module(program, "Module01");
            var import = new Import(module, "Import01", argument, false);

            var result = import.Value;

            Assert.That(result, Is.SameAs(argument));
        }

        [Test]
        public void ShouldNot_UnnestImportedValue_When_ConstructWithConstantAsValue()
        {
            var program = new Program();
            var constant = new Constant(program, "Constant01", BooleanValue.True);
            var module = new Module(program, "Module01");
            var import = new Import(module, "Import01", constant, false);

            var result = import.Value;

            Assert.That(result, Is.SameAs(constant));
        }

        [Test]
        public void ShouldNot_UnnestImportedValue_When_ConstructWithFormulaAsValue()
        {
            var program = new Program();
            var module = new Module(program, "Module01");
            var formula = new Formula(module, "Formula01", "1", FormulaFlags.Private);
            var import = new Import(module, "Import01", formula, false);

            var result = import.Value;

            Assert.That(result, Is.SameAs(formula));
        }

        [Test]
        public void ShouldNot_UnnestImportedValue_When_ConstructWithModuleAsValue()
        {
            var program = new Program();
            var otherModule = new Module(program, "Module01");
            var module = new Module(program, "Module02");
            var import = new Import(module, "Import01", otherModule, false);

            var result = import.Value;

            Assert.That(result, Is.SameAs(otherModule));
        }

        [Test]
        public void Should_ConstructImportNode_When_GivenArgumentImport()
        {
            var program = new Program();
            var argument = new Argument(program, "Argument01");
            var module = new Module(program, "Module01");
            var import = new Import(module, "Import01", argument, false);

            var result = import.ToSyntaxTree();

            Assert.That(result.ImportPath, Is.EqualTo(new[] { argument.Name }));
            Assert.That(result.ImportType, Is.EqualTo(ImportType.Argument));
            Assert.That(result.IsExported, Is.False);
            Assert.That(result.Name, Is.EqualTo(import.Name));
        }

        [Test]
        public void Should_ConstructImportNode_When_GivenConstantImport()
        {
            var program = new Program();
            var constant = new Constant(program, "Constant01", BooleanValue.True);
            var module = new Module(program, "Module01");
            var import = new Import(module, "Import01", constant, false);

            var result = import.ToSyntaxTree();

            Assert.That(result.ImportPath, Is.EqualTo(new[] { constant.Name }));
            Assert.That(result.ImportType, Is.EqualTo(ImportType.Constant));
            Assert.That(result.IsExported, Is.False);
            Assert.That(result.Name, Is.EqualTo(import.Name));
        }

        [Test]
        public void Should_ConstructImportNode_When_GivenFormulaImport()
        {
            var program = new Program();
            var module = new Module(program, "Module01");
            var formula = new Formula(module, "Formula01", "1");
            var import = new Import(module, "Import01", formula, false);

            var result = import.ToSyntaxTree();

            Assert.That(result.ImportPath, Is.EqualTo(new[] { module.Name, formula.Name }));
            Assert.That(result.ImportType, Is.EqualTo(ImportType.Formula));
            Assert.That(result.IsExported, Is.False);
            Assert.That(result.Name, Is.EqualTo(import.Name));
        }

        [Test]
        public void Should_ConstructImportNode_When_GivenModuleImport()
        {
            var program = new Program();
            var otherModule = new Module(program, "Module01");
            var module = new Module(program, "Module02");
            var import = new Import(module, "Import01", otherModule, false);

            var result = import.ToSyntaxTree();

            Assert.That(result.ImportPath, Is.EqualTo(new[] { otherModule.Name }));
            Assert.That(result.ImportType, Is.EqualTo(ImportType.Module));
            Assert.That(result.IsExported, Is.False);
            Assert.That(result.Name, Is.EqualTo(import.Name));
        }
    }
}