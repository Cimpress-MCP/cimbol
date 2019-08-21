using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Cimpress.Cimbol.Compiler.Emit;
using Cimpress.Cimbol.Compiler.SyntaxTree;
using Cimpress.Cimbol.Exceptions;
using Cimpress.Cimbol.Runtime.Types;
using NUnit.Framework;

namespace Cimpress.Cimbol.UnitTests.Compiler.Emit
{
    [TestFixture]
    public class DependencyTableTests
    {
        [Test]
        public void Should_BuildDependencyTable_When_GivenEmptyProgramNode()
        {
            var programNode = new ProgramNode(
                Enumerable.Empty<ArgumentDeclarationNode>(),
                Enumerable.Empty<ConstantDeclarationNode>(),
                Enumerable.Empty<ModuleDeclarationNode>());

            var result = DependencyTable.Build(programNode);

            var expected = Enumerable.Empty<ISet<IDeclarationNode>>();
            Assert.That(result.MinimalPartialOrder(), Is.EqualTo(expected));
        }

        [Test]
        public void Should_BuildDependencyTable_When_GivenSingleFormula()
        {
            var formulaNode = new FormulaDeclarationNode("x", new LiteralNode(BooleanValue.True), false);
            var moduleNode = new ModuleDeclarationNode(
                "x",
                Enumerable.Empty<ImportDeclarationNode>(),
                new[] { formulaNode });
            var programNode = new ProgramNode(
                Enumerable.Empty<ArgumentDeclarationNode>(),
                Enumerable.Empty<ConstantDeclarationNode>(),
                new[] { moduleNode });

            var result = DependencyTable.Build(programNode);

            var expected = new[] { new[] { formulaNode }.ToImmutableHashSet() };
            Assert.That(result.MinimalPartialOrder(), Is.EqualTo(expected));
        }

        [Test]
        public void Should_BuildDependencyTable_When_GivenIndependentFormulas()
        {
            var formulaNode1 = new FormulaDeclarationNode("x", new LiteralNode(BooleanValue.True), false);
            var formulaNode2 = new FormulaDeclarationNode("y", new LiteralNode(BooleanValue.True), false);
            var moduleNode = new ModuleDeclarationNode(
                "x",
                Enumerable.Empty<ImportDeclarationNode>(),
                new[] { formulaNode1, formulaNode2 });
            var programNode = new ProgramNode(
                Enumerable.Empty<ArgumentDeclarationNode>(),
                Enumerable.Empty<ConstantDeclarationNode>(),
                new[] { moduleNode });

            var result = DependencyTable.Build(programNode);

            var expected = new[] { new[] { formulaNode1, formulaNode2 }.ToImmutableHashSet() };
            Assert.That(result.MinimalPartialOrder(), Is.EqualTo(expected));
        }

        [Test]
        public void Should_BuildDependencyTable_When_GivenDependentFormulas()
        {
            var formulaNode1 = new FormulaDeclarationNode("x", new LiteralNode(BooleanValue.True), false);
            var formulaNode2 = new FormulaDeclarationNode("y", new IdentifierNode("x"), false);
            var moduleNode = new ModuleDeclarationNode(
                "x",
                Enumerable.Empty<ImportDeclarationNode>(),
                new[] { formulaNode1, formulaNode2 });
            var programNode = new ProgramNode(
                Enumerable.Empty<ArgumentDeclarationNode>(),
                Enumerable.Empty<ConstantDeclarationNode>(),
                new[] { moduleNode });

            var result = DependencyTable.Build(programNode);

            var expected = new[]
            {
                new[] { formulaNode1 }.ToImmutableHashSet(),
                new[] { formulaNode2 }.ToImmutableHashSet(),
            };
            Assert.That(result.MinimalPartialOrder(), Is.EqualTo(expected));
        }

        [Test]
        public void Should_BuildDependencyTable_When_GivenDependentFormulaImport()
        {
            var importNode1 = new ImportDeclarationNode("x", new[] { "a", "b" }, ImportType.Formula);
            var formulaNode1 = new FormulaDeclarationNode("y", new IdentifierNode("x"), false);
            var formulaNode2 = new FormulaDeclarationNode("b", new LiteralNode(BooleanValue.True), false);
            var moduleNode1 = new ModuleDeclarationNode("x", new[] { importNode1 }, new[] { formulaNode1 });
            var moduleNode2 = new ModuleDeclarationNode(
                "a",
                Enumerable.Empty<ImportDeclarationNode>(),
                new[] { formulaNode2 });
            var programNode = new ProgramNode(
                Enumerable.Empty<ArgumentDeclarationNode>(),
                Enumerable.Empty<ConstantDeclarationNode>(),
                new[] { moduleNode1, moduleNode2 });

            var result = DependencyTable.Build(programNode);

            var expected = new[]
            {
                new[] { formulaNode2 }.ToImmutableHashSet<IDeclarationNode>(),
                new[] { importNode1 }.ToImmutableHashSet<IDeclarationNode>(),
                new[] { formulaNode1 }.ToImmutableHashSet<IDeclarationNode>(),
            };
            Assert.That(result.MinimalPartialOrder(), Is.EqualTo(expected));
        }

        [Test]
        public void Should_BuildDependencyTable_When_GivenDependentModuleImport()
        {
            var importNode1 = new ImportDeclarationNode("x", new[] { "a" }, ImportType.Module);
            var formulaNode1 = new FormulaDeclarationNode("y", new IdentifierNode("x"), false);
            var formulaNode2 = new FormulaDeclarationNode("b", new LiteralNode(BooleanValue.True), true);
            var moduleNode1 = new ModuleDeclarationNode("x", new[] { importNode1 }, new[] { formulaNode1 });
            var moduleNode2 = new ModuleDeclarationNode(
                "a",
                Enumerable.Empty<ImportDeclarationNode>(),
                new[] { formulaNode2 });
            var programNode = new ProgramNode(
                Enumerable.Empty<ArgumentDeclarationNode>(),
                Enumerable.Empty<ConstantDeclarationNode>(),
                new[] { moduleNode1, moduleNode2 });

            var result = DependencyTable.Build(programNode);

            var expected = new[]
            {
                new[] { formulaNode2 }.ToImmutableHashSet<IDeclarationNode>(),
                new[] { importNode1 }.ToImmutableHashSet<IDeclarationNode>(),
                new[] { formulaNode1 }.ToImmutableHashSet<IDeclarationNode>(),
            };
            Assert.That(result.MinimalPartialOrder(), Is.EqualTo(expected));
        }

        [Test]
        public void Should_BuildDependencyTable_When_GivenUnresolvedFormula()
        {
            var formulaNode1 = new FormulaDeclarationNode("x", new IdentifierNode("y"), false);
            var moduleNode = new ModuleDeclarationNode(
                "x",
                Enumerable.Empty<ImportDeclarationNode>(),
                new[] { formulaNode1 });
            var programNode = new ProgramNode(
                Enumerable.Empty<ArgumentDeclarationNode>(),
                Enumerable.Empty<ConstantDeclarationNode>(),
                new[] { moduleNode });

            var result = DependencyTable.Build(programNode);

            var expected = new[] { new[] { formulaNode1 }.ToImmutableHashSet() };
            Assert.That(result.MinimalPartialOrder(), Is.EqualTo(expected));
        }

        [Test]
        public void Should_BuildDependencyTable_When_GivenUnresolvedModuleInFormulaImport()
        {
            var importNode1 = new ImportDeclarationNode("x", new[] { "a", "b" }, ImportType.Formula);
            var moduleNode1 = new ModuleDeclarationNode(
                "x",
                new[] { importNode1 },
                Enumerable.Empty<FormulaDeclarationNode>());
            var programNode = new ProgramNode(
                Enumerable.Empty<ArgumentDeclarationNode>(),
                Enumerable.Empty<ConstantDeclarationNode>(),
                new[] { moduleNode1 });

            var result = DependencyTable.Build(programNode);

            var expected = new[] { new[] { importNode1 }.ToImmutableHashSet() };
            Assert.That(result.MinimalPartialOrder(), Is.EqualTo(expected));
        }

        [Test]
        public void Should_BuildDependencyTable_When_GivenUnresolvedFormulaInFormulaImport()
        {
            var importNode1 = new ImportDeclarationNode("x", new[] { "a", "b" }, ImportType.Formula);
            var moduleNode1 = new ModuleDeclarationNode(
                "x",
                new[] { importNode1 },
                Enumerable.Empty<FormulaDeclarationNode>());
            var moduleNode2 = new ModuleDeclarationNode(
                "a",
                new[] { importNode1 },
                Enumerable.Empty<FormulaDeclarationNode>());
            var programNode = new ProgramNode(
                Enumerable.Empty<ArgumentDeclarationNode>(),
                Enumerable.Empty<ConstantDeclarationNode>(),
                new[] { moduleNode1, moduleNode2 });

            var result = DependencyTable.Build(programNode);

            var expected = new[] { new[] { importNode1 }.ToImmutableHashSet() };
            Assert.That(result.MinimalPartialOrder(), Is.EqualTo(expected));
        }

        [Test]
        public void Should_BuildDependencyTable_When_GivenUnresolvedModuleImport()
        {
            var importNode1 = new ImportDeclarationNode("x", new[] { "a" }, ImportType.Module);
            var moduleNode = new ModuleDeclarationNode(
                "x",
                new[] { importNode1 },
                Enumerable.Empty<FormulaDeclarationNode>());
            var programNode = new ProgramNode(
                Enumerable.Empty<ArgumentDeclarationNode>(),
                Enumerable.Empty<ConstantDeclarationNode>(),
                new[] { moduleNode });

            var result = DependencyTable.Build(programNode);

            var expected = new[] { new[] { importNode1 }.ToImmutableHashSet() };
            Assert.That(result.MinimalPartialOrder(), Is.EqualTo(expected));
        }

        [Test]
        public void ShouldNot_BuildDependencyTable_When_GivenTwoUnexportedCyclicModuleImports()
        {
            var importNode1 = new ImportDeclarationNode("b", new[] { "x" }, ImportType.Module);
            var importNode2 = new ImportDeclarationNode("y", new[] { "a" }, ImportType.Module);
            var formulaNode1 = new FormulaDeclarationNode("c", new IdentifierNode("b"), false);
            var formulaNode2 = new FormulaDeclarationNode("z", new IdentifierNode("y"), false);
            var moduleNode1 = new ModuleDeclarationNode("a", new[] { importNode1 }, new[] { formulaNode1 });
            var moduleNode2 = new ModuleDeclarationNode("x", new[] { importNode2 }, new[] { formulaNode2 });
            var programNode = new ProgramNode(
                Enumerable.Empty<ArgumentDeclarationNode>(),
                Enumerable.Empty<ConstantDeclarationNode>(),
                new[] { moduleNode1, moduleNode2 });

            var result = DependencyTable.Build(programNode);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void ShouldNot_BuildDependencyTable_When_GivenTwoCyclicFormulas()
        {
            var formulaNode1 = new FormulaDeclarationNode("x", new IdentifierNode("y"), false);
            var formulaNode2 = new FormulaDeclarationNode("y", new IdentifierNode("x"), false);
            var moduleNode = new ModuleDeclarationNode(
                "x",
                Enumerable.Empty<ImportDeclarationNode>(),
                new[] { formulaNode1, formulaNode2 });
            var programNode = new ProgramNode(
                Enumerable.Empty<ArgumentDeclarationNode>(),
                Enumerable.Empty<ConstantDeclarationNode>(),
                new[] { moduleNode });

            Assert.Throws<CimbolCompilationException>(() => DependencyTable.Build(programNode));
        }

        [Test]
        public void ShouldNot_BuildDependencyTable_When_GivenThreeCyclicFormulas()
        {
            var formulaNode1 = new FormulaDeclarationNode("x", new IdentifierNode("z"), false);
            var formulaNode2 = new FormulaDeclarationNode("y", new IdentifierNode("x"), false);
            var formulaNode3 = new FormulaDeclarationNode("z", new IdentifierNode("y"), false);
            var moduleNode = new ModuleDeclarationNode(
                "x",
                Enumerable.Empty<ImportDeclarationNode>(),
                new[] { formulaNode1, formulaNode2, formulaNode3 });
            var programNode = new ProgramNode(
                Enumerable.Empty<ArgumentDeclarationNode>(),
                Enumerable.Empty<ConstantDeclarationNode>(),
                new[] { moduleNode });

            Assert.Throws<CimbolCompilationException>(() => DependencyTable.Build(programNode));
        }

        [Test]
        public void ShouldNot_BuildDependencyTable_When_GivenTwoCyclicFormulaImports()
        {
            var importNode1 = new ImportDeclarationNode("b", new[] { "x", "z" }, ImportType.Formula);
            var importNode2 = new ImportDeclarationNode("y", new[] { "a", "c" }, ImportType.Formula);
            var formulaNode1 = new FormulaDeclarationNode("c", new IdentifierNode("b"), false);
            var formulaNode2 = new FormulaDeclarationNode("z", new IdentifierNode("y"), false);
            var moduleNode1 = new ModuleDeclarationNode("a", new[] { importNode1 }, new[] { formulaNode1 });
            var moduleNode2 = new ModuleDeclarationNode("x", new[] { importNode2 }, new[] { formulaNode2 });
            var programNode = new ProgramNode(
                Enumerable.Empty<ArgumentDeclarationNode>(),
                Enumerable.Empty<ConstantDeclarationNode>(),
                new[] { moduleNode1, moduleNode2 });

            Assert.Throws<CimbolCompilationException>(() => DependencyTable.Build(programNode));
        }

        [Test]
        public void ShouldNot_BuildDependencyTable_When_GivenTwoCyclicModuleImports()
        {
            var importNode1 = new ImportDeclarationNode("b", new[] { "x" }, ImportType.Module);
            var importNode2 = new ImportDeclarationNode("y", new[] { "a" }, ImportType.Module);
            var formulaNode1 = new FormulaDeclarationNode("c", new IdentifierNode("b"), true);
            var formulaNode2 = new FormulaDeclarationNode("z", new IdentifierNode("y"), true);
            var moduleNode1 = new ModuleDeclarationNode("a", new[] { importNode1 }, new[] { formulaNode1 });
            var moduleNode2 = new ModuleDeclarationNode("x", new[] { importNode2 }, new[] { formulaNode2 });
            var programNode = new ProgramNode(
                Enumerable.Empty<ArgumentDeclarationNode>(),
                Enumerable.Empty<ConstantDeclarationNode>(),
                new[] { moduleNode1, moduleNode2 });

            Assert.Throws<CimbolCompilationException>(() => DependencyTable.Build(programNode));
        }

        [Test]
        public void ShouldNot_BuildDependencyTable_When_GivenFormulaImportWithWrongPathLength()
        {
            var importNode1 = new ImportDeclarationNode("x", new[] { "a" }, ImportType.Formula);
            var moduleNode1 = new ModuleDeclarationNode(
                "x",
                new[] { importNode1 },
                Enumerable.Empty<FormulaDeclarationNode>());
            var programNode = new ProgramNode(
                Enumerable.Empty<ArgumentDeclarationNode>(),
                Enumerable.Empty<ConstantDeclarationNode>(),
                new[] { moduleNode1 });

            Assert.Throws<NotSupportedException>(() => DependencyTable.Build(programNode));
        }

        [Test]
        public void ShouldNot_BuildDependencyTable_When_GivenModuleImportWithWrongPathLength()
        {
            var importNode1 = new ImportDeclarationNode("x", new[] { "a", "b" }, ImportType.Module);
            var moduleNode1 = new ModuleDeclarationNode(
                "x",
                new[] { importNode1 },
                Enumerable.Empty<FormulaDeclarationNode>());
            var programNode = new ProgramNode(
                Enumerable.Empty<ArgumentDeclarationNode>(),
                Enumerable.Empty<ConstantDeclarationNode>(),
                new[] { moduleNode1 });

            Assert.Throws<NotSupportedException>(() => DependencyTable.Build(programNode));
        }
    }
}