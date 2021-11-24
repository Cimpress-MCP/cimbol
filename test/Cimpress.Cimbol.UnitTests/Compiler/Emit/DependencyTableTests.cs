// Copyright 2021 Cimpress plc.
// Licensed under the Apache License, Version 2.0
// http://www.apache.org/licenses/LICENSE-2.0

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
                Enumerable.Empty<ArgumentNode>(),
                Enumerable.Empty<ConstantNode>(),
                Enumerable.Empty<ModuleNode>());

            var result = new DependencyTable(programNode);

            var expected = Enumerable.Empty<ISet<IDeclarationNode>>();
            Assert.That(result.MinimalPartialOrder(), Is.EqualTo(expected));
        }

        [Test]
        public void Should_BuildDependencyTable_When_GivenSingleFormula()
        {
            var formulaNode = new FormulaNode("x", new LiteralNode(BooleanValue.True), false);
            var moduleNode = new ModuleNode(
                "x",
                Enumerable.Empty<ImportNode>(),
                new[] { formulaNode });
            var programNode = new ProgramNode(
                Enumerable.Empty<ArgumentNode>(),
                Enumerable.Empty<ConstantNode>(),
                new[] { moduleNode });

            var result = new DependencyTable(programNode);

            var expected = new[] { new[] { formulaNode }.ToImmutableHashSet() };
            Assert.That(result.MinimalPartialOrder(), Is.EqualTo(expected));
        }

        [Test]
        public void Should_BuildDependencyTable_When_GivenIndependentFormulas()
        {
            var formulaNode1 = new FormulaNode("x", new LiteralNode(BooleanValue.True), false);
            var formulaNode2 = new FormulaNode("y", new LiteralNode(BooleanValue.True), false);
            var moduleNode = new ModuleNode(
                "x",
                Enumerable.Empty<ImportNode>(),
                new[] { formulaNode1, formulaNode2 });
            var programNode = new ProgramNode(
                Enumerable.Empty<ArgumentNode>(),
                Enumerable.Empty<ConstantNode>(),
                new[] { moduleNode });

            var result = new DependencyTable(programNode);

            var expected = new[] { new[] { formulaNode1, formulaNode2 }.ToImmutableHashSet() };
            Assert.That(result.MinimalPartialOrder(), Is.EqualTo(expected));
        }

        [Test]
        public void Should_BuildDependencyTable_When_GivenDependentFormulas()
        {
            var formulaNode1 = new FormulaNode("x", new LiteralNode(BooleanValue.True), false);
            var formulaNode2 = new FormulaNode("y", new IdentifierNode("x"), false);
            var moduleNode = new ModuleNode(
                "x",
                Enumerable.Empty<ImportNode>(),
                new[] { formulaNode1, formulaNode2 });
            var programNode = new ProgramNode(
                Enumerable.Empty<ArgumentNode>(),
                Enumerable.Empty<ConstantNode>(),
                new[] { moduleNode });

            var result = new DependencyTable(programNode);

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
            var importNode1 = new ImportNode("x", new[] { "a", "b" }, ImportType.Formula, false);
            var formulaNode1 = new FormulaNode("y", new IdentifierNode("x"), false);
            var formulaNode2 = new FormulaNode("b", new LiteralNode(BooleanValue.True), false);
            var moduleNode1 = new ModuleNode("x", new[] { importNode1 }, new[] { formulaNode1 });
            var moduleNode2 = new ModuleNode(
                "a",
                Enumerable.Empty<ImportNode>(),
                new[] { formulaNode2 });
            var programNode = new ProgramNode(
                Enumerable.Empty<ArgumentNode>(),
                Enumerable.Empty<ConstantNode>(),
                new[] { moduleNode1, moduleNode2 });

            var result = new DependencyTable(programNode);

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
            var importNode1 = new ImportNode("x", new[] { "a" }, ImportType.Module, false);
            var formulaNode1 = new FormulaNode("y", new IdentifierNode("x"), false);
            var formulaNode2 = new FormulaNode("b", new LiteralNode(BooleanValue.True), true);
            var moduleNode1 = new ModuleNode("x", new[] { importNode1 }, new[] { formulaNode1 });
            var moduleNode2 = new ModuleNode(
                "a",
                Enumerable.Empty<ImportNode>(),
                new[] { formulaNode2 });
            var programNode = new ProgramNode(
                Enumerable.Empty<ArgumentNode>(),
                Enumerable.Empty<ConstantNode>(),
                new[] { moduleNode1, moduleNode2 });

            var result = new DependencyTable(programNode);

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
            var formulaNode1 = new FormulaNode("x", new IdentifierNode("y"), false);
            var moduleNode = new ModuleNode(
                "x",
                Enumerable.Empty<ImportNode>(),
                new[] { formulaNode1 });
            var programNode = new ProgramNode(
                Enumerable.Empty<ArgumentNode>(),
                Enumerable.Empty<ConstantNode>(),
                new[] { moduleNode });

            var result = new DependencyTable(programNode);

            var expected = new[] { new[] { formulaNode1 }.ToImmutableHashSet() };
            Assert.That(result.MinimalPartialOrder(), Is.EqualTo(expected));
        }

        [Test]
        public void Should_BuildDependencyTable_When_GivenUnresolvedModuleInFormulaImport()
        {
            var importNode1 = new ImportNode("x", new[] { "a", "b" }, ImportType.Formula, false);
            var moduleNode1 = new ModuleNode(
                "x",
                new[] { importNode1 },
                Enumerable.Empty<FormulaNode>());
            var programNode = new ProgramNode(
                Enumerable.Empty<ArgumentNode>(),
                Enumerable.Empty<ConstantNode>(),
                new[] { moduleNode1 });

            var result = new DependencyTable(programNode);

            var expected = new[] { new[] { importNode1 }.ToImmutableHashSet() };
            Assert.That(result.MinimalPartialOrder(), Is.EqualTo(expected));
        }

        [Test]
        public void Should_BuildDependencyTable_When_GivenUnresolvedFormulaInFormulaImport()
        {
            var importNode1 = new ImportNode("x", new[] { "a", "b" }, ImportType.Formula, false);
            var moduleNode1 = new ModuleNode(
                "x",
                new[] { importNode1 },
                Enumerable.Empty<FormulaNode>());
            var moduleNode2 = new ModuleNode(
                "a",
                new[] { importNode1 },
                Enumerable.Empty<FormulaNode>());
            var programNode = new ProgramNode(
                Enumerable.Empty<ArgumentNode>(),
                Enumerable.Empty<ConstantNode>(),
                new[] { moduleNode1, moduleNode2 });

            var result = new DependencyTable(programNode);

            var expected = new[] { new[] { importNode1 }.ToImmutableHashSet() };
            Assert.That(result.MinimalPartialOrder(), Is.EqualTo(expected));
        }

        [Test]
        public void Should_BuildDependencyTable_When_GivenUnresolvedModuleImport()
        {
            var importNode1 = new ImportNode("x", new[] { "a" }, ImportType.Module, false);
            var moduleNode = new ModuleNode(
                "x",
                new[] { importNode1 },
                Enumerable.Empty<FormulaNode>());
            var programNode = new ProgramNode(
                Enumerable.Empty<ArgumentNode>(),
                Enumerable.Empty<ConstantNode>(),
                new[] { moduleNode });

            var result = new DependencyTable(programNode);

            var expected = new[] { new[] { importNode1 }.ToImmutableHashSet() };
            Assert.That(result.MinimalPartialOrder(), Is.EqualTo(expected));
        }

        [Test]
        public void ShouldNot_BuildDependencyTable_When_GivenTwoUnexportedCyclicModuleImports()
        {
            var importNode1 = new ImportNode("b", new[] { "x" }, ImportType.Module, false);
            var importNode2 = new ImportNode("y", new[] { "a" }, ImportType.Module, false);
            var formulaNode1 = new FormulaNode("c", new IdentifierNode("b"), false);
            var formulaNode2 = new FormulaNode("z", new IdentifierNode("y"), false);
            var moduleNode1 = new ModuleNode("a", new[] { importNode1 }, new[] { formulaNode1 });
            var moduleNode2 = new ModuleNode("x", new[] { importNode2 }, new[] { formulaNode2 });
            var programNode = new ProgramNode(
                Enumerable.Empty<ArgumentNode>(),
                Enumerable.Empty<ConstantNode>(),
                new[] { moduleNode1, moduleNode2 });

            var result = new DependencyTable(programNode);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void ShouldNot_BuildDependencyTable_When_GivenTwoCyclicFormulas()
        {
            var formulaNode1 = new FormulaNode("x", new IdentifierNode("y"), false);
            var formulaNode2 = new FormulaNode("y", new IdentifierNode("x"), false);
            var moduleNode = new ModuleNode(
                "x",
                Enumerable.Empty<ImportNode>(),
                new[] { formulaNode1, formulaNode2 });
            var programNode = new ProgramNode(
                Enumerable.Empty<ArgumentNode>(),
                Enumerable.Empty<ConstantNode>(),
                new[] { moduleNode });

            Assert.Throws<CimbolCompilationException>(() => new DependencyTable(programNode));
        }

        [Test]
        public void ShouldNot_BuildDependencyTable_When_GivenThreeCyclicFormulas()
        {
            var formulaNode1 = new FormulaNode("x", new IdentifierNode("z"), false);
            var formulaNode2 = new FormulaNode("y", new IdentifierNode("x"), false);
            var formulaNode3 = new FormulaNode("z", new IdentifierNode("y"), false);
            var moduleNode = new ModuleNode(
                "x",
                Enumerable.Empty<ImportNode>(),
                new[] { formulaNode1, formulaNode2, formulaNode3 });
            var programNode = new ProgramNode(
                Enumerable.Empty<ArgumentNode>(),
                Enumerable.Empty<ConstantNode>(),
                new[] { moduleNode });

            Assert.Throws<CimbolCompilationException>(() => new DependencyTable(programNode));
        }

        [Test]
        public void ShouldNot_BuildDependencyTable_When_GivenTwoCyclicFormulaImports()
        {
            var importNode1 = new ImportNode("b", new[] { "x", "z" }, ImportType.Formula, false);
            var importNode2 = new ImportNode("y", new[] { "a", "c" }, ImportType.Formula, false);
            var formulaNode1 = new FormulaNode("c", new IdentifierNode("b"), false);
            var formulaNode2 = new FormulaNode("z", new IdentifierNode("y"), false);
            var moduleNode1 = new ModuleNode("a", new[] { importNode1 }, new[] { formulaNode1 });
            var moduleNode2 = new ModuleNode("x", new[] { importNode2 }, new[] { formulaNode2 });
            var programNode = new ProgramNode(
                Enumerable.Empty<ArgumentNode>(),
                Enumerable.Empty<ConstantNode>(),
                new[] { moduleNode1, moduleNode2 });

            Assert.Throws<CimbolCompilationException>(() => new DependencyTable(programNode));
        }

        [Test]
        public void ShouldNot_BuildDependencyTable_When_GivenTwoCyclicModuleImports()
        {
            var importNode1 = new ImportNode("b", new[] { "x" }, ImportType.Module, false);
            var importNode2 = new ImportNode("y", new[] { "a" }, ImportType.Module, false);
            var formulaNode1 = new FormulaNode("c", new IdentifierNode("b"), true);
            var formulaNode2 = new FormulaNode("z", new IdentifierNode("y"), true);
            var moduleNode1 = new ModuleNode("a", new[] { importNode1 }, new[] { formulaNode1 });
            var moduleNode2 = new ModuleNode("x", new[] { importNode2 }, new[] { formulaNode2 });
            var programNode = new ProgramNode(
                Enumerable.Empty<ArgumentNode>(),
                Enumerable.Empty<ConstantNode>(),
                new[] { moduleNode1, moduleNode2 });

            Assert.Throws<CimbolCompilationException>(() => new DependencyTable(programNode));
        }

        [Test]
        public void ShouldNot_BuildDependencyTable_When_GivenFormulaImportWithWrongPathLength()
        {
            var importNode1 = new ImportNode("x", new[] { "a" }, ImportType.Formula, false);
            var moduleNode1 = new ModuleNode(
                "x",
                new[] { importNode1 },
                Enumerable.Empty<FormulaNode>());
            var programNode = new ProgramNode(
                Enumerable.Empty<ArgumentNode>(),
                Enumerable.Empty<ConstantNode>(),
                new[] { moduleNode1 });

            Assert.Throws<CimbolInternalException>(() => new DependencyTable(programNode));
        }

        [Test]
        public void ShouldNot_BuildDependencyTable_When_GivenModuleImportWithWrongPathLength()
        {
            var importNode1 = new ImportNode("x", new[] { "a", "b" }, ImportType.Module, false);
            var moduleNode1 = new ModuleNode(
                "x",
                new[] { importNode1 },
                Enumerable.Empty<FormulaNode>());
            var programNode = new ProgramNode(
                Enumerable.Empty<ArgumentNode>(),
                Enumerable.Empty<ConstantNode>(),
                new[] { moduleNode1 });

            Assert.Throws<CimbolInternalException>(() => new DependencyTable(programNode));
        }
    }
}