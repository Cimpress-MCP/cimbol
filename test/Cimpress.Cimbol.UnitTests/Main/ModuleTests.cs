using System;
using System.Linq;
using Cimpress.Cimbol.Compiler.SyntaxTree;
using Cimpress.Cimbol.Runtime.Types;
using NUnit.Framework;

namespace Cimpress.Cimbol.UnitTests.Main
{
    [TestFixture]
    public class ModuleTests
    {
        [Test]
        public void Should_Initialize_When_ProvidedWithValidArguments()
        {
            Assert.DoesNotThrow(() =>
            {
                var module = new Module(new Program(), "module");
            });
        }

        [Test]
        public void Should_ThrowArgumentNullException_When_InitializedWithNullProgram()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var module = new Module(null, "module");
            });
        }

        [Test]
        public void Should_ThrowArgumentNullException_When_InitializedWithNullName()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var module = new Module(new Program(), null);
            });
        }

        [Test]
        public void Should_RetrieveSameProgram_When_InitializedWithProgram()
        {
            var program = new Program();
            var module = new Module(program, "module");
            Assert.That(module.Program, Is.SameAs(program));
        }

        [Test]
        public void Should_RetrieveSameName_When_InitializedWithName()
        {
            var name = "module";
            var module = new Module(new Program(), name);
            Assert.That(module.Name, Is.SameAs(name));
        }

        [Test]
        public void Should_AddFormula_When_SameNameDoesNotExist()
        {
            var program = new Program();
            var module = program.AddModule("module");
            module.AddFormula("formula", "1 + 2");
            Assert.DoesNotThrow(() => module.AddFormula("other formula", "2 + 3"));
        }

        [Test]
        public void ShouldNot_AddFormula_When_FormulaNameIsNull()
        {
            var program = new Program();
            var module = program.AddModule("module");
            Assert.Throws<ArgumentNullException>(() => module.AddFormula(null, "2 + 3"));
        }

        [Test]
        public void ShouldNot_AddFormula_When_FormulaValueIsNull()
        {
            var program = new Program();
            var module = program.AddModule("module");
            Assert.Throws<ArgumentNullException>(() => module.AddFormula("formula", null));
        }

        [Test]
        public void ShouldNot_AddFormula_When_SameNameExists()
        {
            var program = new Program();
            var module = program.AddModule("module");
            module.AddFormula("formula", "1 + 2");
            Assert.Throws<NotSupportedException>(() => module.AddFormula("formula", "2 + 3"));
        }

        [Test]
        public void ShouldNot_AddFormula_When_SameNameExistsInDifferentCase()
        {
            var program = new Program();
            var module = program.AddModule("module");
            module.AddFormula("formula", "1 + 2");
            Assert.Throws<NotSupportedException>(() => module.AddFormula("FORMULA", "2 + 3"));
        }

        [Test]
        public void Should_GetFormula_When_FormulaExists()
        {
            var program = new Program();
            var module = program.AddModule("module");
            module.AddFormula("formula", "1 + 2", FormulaFlags.Internal);

            var retrievedFormula = module.TryGetFormula("formula", out var formula);

            Assert.That(retrievedFormula, Is.True);
            Assert.That(formula, Is.Not.Null);
            Assert.That(formula.Value, Is.EqualTo("1 + 2"));
            Assert.That(formula.IsExported, Is.EqualTo(false));
            Assert.That(formula.IsReferenceable, Is.EqualTo(true));
        }

        [Test]
        public void Should_AddArgumentReference_When_SameNameDoesNotExist()
        {
            var program = new Program();
            var resource = program.AddArgument("argument");
            var module = program.AddModule("module");
            Assert.DoesNotThrow(() => module.AddReference("argument reference", resource));
        }

        [Test]
        public void Should_AddConstantReference_When_SameNameDoesNotExist()
        {
            var program = new Program();
            var resource = program.AddConstant("constant", new NumberValue(5));
            var module = program.AddModule("module");
            Assert.DoesNotThrow(() => module.AddReference("constant reference", resource));
        }

        [Test]
        public void Should_AddModuleReference_When_SameNameDoesNotExist()
        {
            var program = new Program();
            var resource = program.AddModule("other module");
            var module = program.AddModule("module");
            Assert.DoesNotThrow(() => module.AddReference("module reference", resource));
        }

        [Test]
        public void Should_AddFormulaReference_When_SameNameDoesNotExist()
        {
            var program = new Program();
            var moduleA = program.AddModule("module a");
            var moduleB = program.AddModule("module b");
            var resource = moduleB.AddFormula("formula", "1 + 2");
            Assert.DoesNotThrow(() => moduleA.AddReference("output", resource));
        }

        [Test]
        public void ShouldNot_AddReference_When_ResourceIsNull()
        {
            var program = new Program();
            var module = program.AddModule("module");
            Assert.Throws<ArgumentNullException>(() => module.AddReference("resource", null));
        }

        [Test]
        public void ShouldNot_AddReference_When_ReferenceNameIsNull()
        {
            var program = new Program();
            var resource = program.AddArgument("argument");
            var module = program.AddModule("module");
            Assert.Throws<ArgumentNullException>(() => module.AddReference(null, resource));
        }

        [Test]
        public void ShouldNot_AddReference_When_ReferenceIsSelf()
        {
            var program = new Program();
            var module = program.AddModule("module");
            Assert.Throws<NotSupportedException>(() => module.AddReference("x", module));
        }

        [Test]
        public void ShouldNot_AddReference_When_ReferenceBelongsToDifferentProgram()
        {
            var programA = new Program();
            var resource = programA.AddArgument("argument");
            var programB = new Program();
            var module = programB.AddModule("module");
            Assert.Throws<NotSupportedException>(() => module.AddReference("resource", resource));
        }

        [Test]
        public void ShouldNot_AddReference_When_NameAlreadyExists()
        {
            var program = new Program();
            var resourceA = program.AddArgument("argument a");
            var resourceB = program.AddArgument("argument b");
            var module = program.AddModule("module");
            module.AddReference("argument", resourceA);
            Assert.Throws<NotSupportedException>(() => module.AddReference("argument", resourceB));
        }

        [Test]
        public void ShouldNot_AddFormulaReference_When_FormulaComesFromSelf()
        {
            var program = new Program();
            var module = program.AddModule("module");
            var resource = module.AddFormula("formula", "1 + 2");
            Assert.Throws<NotSupportedException>(() => module.AddReference("output", resource));
        }

        [Test]
        public void ShouldNot_AddFormulaReference_When_NotReferenceable()
        {
            var program = new Program();
            var moduleA = program.AddModule("module a");
            var moduleB = program.AddModule("module b");
            var resource = moduleB.AddFormula("formula", "1 + 2", FormulaFlags.Private);
            Assert.Throws<NotSupportedException>(() => moduleA.AddReference("output", resource));
        }

        [Test]
        public void ShouldNot_AddFormula_When_SameNameExistsAsReference()
        {
            var program = new Program();
            var resource = program.AddArgument("argument");
            var module = program.AddModule("module");
            module.AddReference("formula", resource);
            Assert.Throws<NotSupportedException>(() => module.AddFormula("formula", "1 + 2"));
        }

        [Test]
        public void ShouldNot_AddReference_When_SameNameExistsAsFormula()
        {
            var program = new Program();
            var resource = program.AddArgument("argument");
            var module = program.AddModule("module");
            module.AddFormula("formula", "1 + 2");
            Assert.Throws<NotSupportedException>(() => module.AddReference("formula", resource));
        }

        [Test]
        public void Should_CreateModuleDeclaratioNode_When_GivenEmptyModule()
        {
            var program = new Program();
            var module = program.AddModule("module");

            var result = module.ToSyntaxTree();
            Assert.That(result.Exports, Is.Empty);
            Assert.That(result.Formulas, Is.Empty);
            Assert.That(result.Imports, Is.Empty);
        }

        [Test]
        public void Should_CreateModuleDeclarationNode_When_GivenSingleArgumentImport()
        {
            var program = new Program();
            var resource = program.AddArgument("argument");
            var module = program.AddModule("module");
            module.AddReference("argument", resource);

            var result = module.ToSyntaxTree();

            Assert.That(result.Imports, Has.Length.EqualTo(1));
            var import = result.Imports.Single();
            Assert.That(import.Identifier, Is.EqualTo("argument"));
            Assert.That(import.ImportPath, Is.EqualTo(new[] { "argument" }));
            Assert.That(import.ImportType, Is.EqualTo(ImportType.Argument));
        }

        [Test]
        public void Should_CreateModuleDeclarationNode_When_GivenSingleConstantImport()
        {
            var program = new Program();
            var resource = program.AddConstant("constant", BooleanValue.True);
            var module = program.AddModule("module");
            module.AddReference("constant", resource);

            var result = module.ToSyntaxTree();

            Assert.That(result.Imports, Has.Length.EqualTo(1));
            var import = result.Imports.Single();
            Assert.That(import.Identifier, Is.EqualTo("constant"));
            Assert.That(import.ImportPath, Is.EqualTo(new[] { "constant" }));
            Assert.That(import.ImportType, Is.EqualTo(ImportType.Constant));
        }

        [Test]
        public void Should_CreateModuleDeclarationNode_When_GivenSingleFormulaImport()
        {
            var program = new Program();
            var otherModule = program.AddModule("other module");
            var resource = otherModule.AddFormula("formula", "x");
            var module = program.AddModule("module");
            module.AddReference("formula", resource);

            var result = module.ToSyntaxTree();

            Assert.That(result.Imports, Has.Length.EqualTo(1));
            var import = result.Imports.Single();
            Assert.That(import.Identifier, Is.EqualTo("formula"));
            Assert.That(import.ImportPath, Is.EqualTo(new[] { "other module", "formula" }));
            Assert.That(import.ImportType, Is.EqualTo(ImportType.Formula));
        }

        [Test]
        public void Should_CreateModuleDeclarationNode_When_GivenSingleModuleImport()
        {
            var program = new Program();
            var resource = program.AddModule("other module");
            var module = program.AddModule("module");
            module.AddReference("other module", resource);

            var result = module.ToSyntaxTree();

            Assert.That(result.Imports, Has.Length.EqualTo(1));
            var import = result.Imports.Single();
            Assert.That(import.Identifier, Is.EqualTo("other module"));
            Assert.That(import.ImportPath, Is.EqualTo(new[] { "other module" }));
            Assert.That(import.ImportType, Is.EqualTo(ImportType.Module));
        }

        [Test]
        public void Should_CreateModuleDeclaration_When_GivenSingleExportedFormula()
        {
            var program = new Program();
            var module = program.AddModule("module");
            module.AddFormula("formula", "x", FormulaFlags.Exported);

            var result = module.ToSyntaxTree();

            Assert.That(result.Exports, Has.Length.EqualTo(1));
            Assert.That(result.Formulas, Has.Length.EqualTo(1));
            var export = result.Exports.Single();
            Assert.That(export.Identifier, Is.EqualTo("formula"));
            var formula = result.Formulas.Single();
            Assert.That(formula.Name, Is.EqualTo("formula"));
        }

        [Test]
        public void Should_CreateModuleDeclaration_When_GivenSingleReferenceableFormula()
        {
            var program = new Program();
            var module = program.AddModule("module");
            module.AddFormula("formula", "x", FormulaFlags.Referenceable);

            var result = module.ToSyntaxTree();

            Assert.That(result.Exports, Has.Length.EqualTo(1));
            Assert.That(result.Formulas, Has.Length.EqualTo(1));
            var export = result.Exports.Single();
            Assert.That(export.Identifier, Is.EqualTo("formula"));
            var formula = result.Formulas.Single();
            Assert.That(formula.Name, Is.EqualTo("formula"));
        }

        [Test]
        public void Should_CreateModuleDeclaration_When_GivenSingleInternalFormula()
        {
            var program = new Program();
            var module = program.AddModule("module");
            module.AddFormula("formula", "x", FormulaFlags.None);

            var result = module.ToSyntaxTree();

            Assert.That(result.Exports, Is.Empty);
            Assert.That(result.Formulas, Has.Length.EqualTo(1));
            var formula = result.Formulas.Single();
            Assert.That(formula.Name, Is.EqualTo("formula"));
        }
    }
}
