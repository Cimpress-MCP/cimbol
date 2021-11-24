// Copyright 2021 Cimpress plc.
// Licensed under the Apache License, Version 2.0
// http://www.apache.org/licenses/LICENSE-2.0

using System.Linq;
using Cimpress.Cimbol.Compiler.Emit;
using Cimpress.Cimbol.Compiler.SyntaxTree;
using Cimpress.Cimbol.Runtime.Types;
using NUnit.Framework;

namespace Cimpress.Cimbol.UnitTests.Compiler.Emit
{
    [TestFixture]
    public class SymbolRegistryTests
    {
        [Test]
        public void Should_CreateSymbolRegistryWithArgument_When_GivenProgramNodeWithArgumentDeclaration()
        {
            var argumentDeclarationNode = new ArgumentNode("x");
            var programNode = new ProgramNode(
                new[] { argumentDeclarationNode },
                Enumerable.Empty<ConstantNode>(),
                Enumerable.Empty<ModuleNode>());
            var declarationHierarchy = new DeclarationHierarchy(programNode);
            var dependencyTable = new DependencyTable(programNode);

            var result = new SymbolRegistry(programNode, declarationHierarchy, dependencyTable);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Arguments.Symbols, Has.Count.EqualTo(1));
            Assert.That(result.Arguments.Resolve("x"), Is.Not.Null);
            Assert.That(result.Constants.Symbols, Is.Empty);
            Assert.That(result.Modules.Symbols, Is.Empty);
            Assert.That(result.Scopes, Is.Empty);
        }

        [Test]
        public void Should_CreateSymbolRegistryWithConstant_When_GivenProgramNodeWithConstantDeclaration()
        {
            var constantDeclarationNode = new ConstantNode("x", BooleanValue.True);
            var programNode = new ProgramNode(
                Enumerable.Empty<ArgumentNode>(),
                new[] { constantDeclarationNode },
                Enumerable.Empty<ModuleNode>());
            var declarationHierarchy = new DeclarationHierarchy(programNode);
            var dependencyTable = new DependencyTable(programNode);

            var result = new SymbolRegistry(programNode, declarationHierarchy, dependencyTable);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Arguments.Symbols, Is.Empty);
            Assert.That(result.Constants.Symbols, Has.Count.EqualTo(1));
            Assert.That(result.Constants.Resolve("x"), Is.Not.Null);
            Assert.That(result.Modules.Symbols, Is.Empty);
            Assert.That(result.Scopes, Is.Empty);
        }

        [Test]
        public void Should_CreateSymbolRegistryWithModule_When_GivenProgramNodeWithModuleDeclaration()
        {
            var moduleDeclarationNode = new ModuleNode(
                "x",
                Enumerable.Empty<ImportNode>(),
                Enumerable.Empty<FormulaNode>());
            var programNode = new ProgramNode(
                Enumerable.Empty<ArgumentNode>(),
                Enumerable.Empty<ConstantNode>(),
                new[] { moduleDeclarationNode });
            var declarationHierarchy = new DeclarationHierarchy(programNode);
            var dependencyTable = new DependencyTable(programNode);

            var result = new SymbolRegistry(programNode, declarationHierarchy, dependencyTable);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Arguments.Symbols, Is.Empty);
            Assert.That(result.Constants.Symbols, Is.Empty);
            Assert.That(result.Modules.Symbols, Has.Count.EqualTo(1));
            Assert.That(result.Modules.Resolve("x"), Is.Not.Null);
            Assert.That(result.Scopes, Has.Count.EqualTo(1));
            Assert.That(result.Scopes, Contains.Key(moduleDeclarationNode.Name));
        }

        [Test]
        public void Should_CreateSymbolTableInRegistryWithSymbol_When_GivenProgramNodeWithModuleWithFormula()
        {
            var formulaDeclarationNode = new FormulaNode("x", new IdentifierNode("y"), true);
            var moduleDeclarationNode = new ModuleNode(
                "x",
                Enumerable.Empty<ImportNode>(),
                new[] { formulaDeclarationNode });
            var programNode = new ProgramNode(
                Enumerable.Empty<ArgumentNode>(),
                Enumerable.Empty<ConstantNode>(),
                new[] { moduleDeclarationNode });
            var declarationHierarchy = new DeclarationHierarchy(programNode);
            var dependencyTable = new DependencyTable(programNode);

            var result = new SymbolRegistry(programNode, declarationHierarchy, dependencyTable)
                .Scopes[moduleDeclarationNode.Name];

            Assert.That(result, Is.Not.Null);
            var resultSymbol = result.Resolve("x");
            Assert.That(resultSymbol, Is.Not.Null);
            Assert.That(resultSymbol.Name, Is.EqualTo("x"));
        }

        [Test]
        public void Should_CreateSymbolTableInRegistryWithSymbol_When_GivenProgramNodeWithModuleWithImport()
        {
            var importDeclarationNode = new ImportNode("x", new[] { "y" }, ImportType.Constant, false);
            var moduleDeclarationNode = new ModuleNode(
                "x",
                new[] { importDeclarationNode },
                Enumerable.Empty<FormulaNode>());
            var constantNode = new ConstantNode("y", BooleanValue.True);
            var programNode = new ProgramNode(
                Enumerable.Empty<ArgumentNode>(),
                new[] { constantNode },
                new[] { moduleDeclarationNode });
            var declarationHierarchy = new DeclarationHierarchy(programNode);
            var dependencyTable = new DependencyTable(programNode);

            var result = new SymbolRegistry(programNode, declarationHierarchy, dependencyTable)
                .Scopes[moduleDeclarationNode.Name];

            Assert.That(result, Is.Not.Null);
            var resultSymbol = result.Resolve("x");
            Assert.That(resultSymbol, Is.Not.Null);
            Assert.That(resultSymbol.Name, Is.EqualTo("x"));
        }
    }
}