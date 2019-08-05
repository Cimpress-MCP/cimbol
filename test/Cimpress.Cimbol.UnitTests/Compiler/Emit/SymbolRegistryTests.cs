using System;
using System.Collections.Generic;
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
        public void Should_InitializeWithArguments_When_GivenArguments()
        {
            var arguments = new Dictionary<string, Symbol>(StringComparer.OrdinalIgnoreCase)
            {
                { "x", new Symbol("x", typeof(ILocalValue)) },
            };
            var symbolRegistry = new SymbolRegistry(
                arguments,
                new Dictionary<string, Symbol>(StringComparer.OrdinalIgnoreCase),
                new Dictionary<string, Symbol>(StringComparer.OrdinalIgnoreCase),
                new Dictionary<IDeclarationNode, SymbolTable>());

            var result = symbolRegistry.Arguments;

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Has.Count.EqualTo(1));
            Assert.That(result, Contains.Key("x"));
        }

        [Test]
        public void Should_InitializeWithConstants_When_GivenConstants()
        {
            var constants = new Dictionary<string, Symbol>(StringComparer.OrdinalIgnoreCase)
            {
                { "x", new Symbol("x", typeof(ILocalValue)) },
            };
            var symbolRegistry = new SymbolRegistry(
                new Dictionary<string, Symbol>(StringComparer.OrdinalIgnoreCase),
                constants,
                new Dictionary<string, Symbol>(StringComparer.OrdinalIgnoreCase),
                new Dictionary<IDeclarationNode, SymbolTable>());

            var result = symbolRegistry.Constants;

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Has.Count.EqualTo(1));
            Assert.That(result, Contains.Key("x"));
        }

        [Test]
        public void Should_InitializeWithModules_When_GivenModules()
        {
            var modules = new Dictionary<string, Symbol>(StringComparer.OrdinalIgnoreCase)
            {
                { "x", new Symbol("x", typeof(ILocalValue)) },
            };
            var symbolRegistry = new SymbolRegistry(
                new Dictionary<string, Symbol>(StringComparer.OrdinalIgnoreCase),
                new Dictionary<string, Symbol>(StringComparer.OrdinalIgnoreCase),
                modules,
                new Dictionary<IDeclarationNode, SymbolTable>());

            var result = symbolRegistry.Modules;

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Has.Count.EqualTo(1));
            Assert.That(result, Contains.Key("x"));
        }

        [Test]
        public void Should_InitializeWithSymbolTables_When_GivenSymbolTables()
        {
            var moduleDeclaration = new ModuleDeclarationNode(
                "x",
                Enumerable.Empty<ImportDeclarationNode>(),
                Enumerable.Empty<FormulaDeclarationNode>());
            var symbolTable = new SymbolTable();
            var symbolTables = new Dictionary<IDeclarationNode, SymbolTable>
            {
                { moduleDeclaration, symbolTable },
            };
            var symbolRegistry = new SymbolRegistry(
                new Dictionary<string, Symbol>(StringComparer.OrdinalIgnoreCase),
                new Dictionary<string, Symbol>(StringComparer.OrdinalIgnoreCase),
                new Dictionary<string, Symbol>(StringComparer.OrdinalIgnoreCase),
                symbolTables);

            var result = symbolRegistry.SymbolTables;

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Has.Count.EqualTo(1));
            Assert.That(result, Contains.Key(moduleDeclaration));
        }

        [Test]
        public void Should_CreateSymbolRegistryWithArgument_When_GivenProgramNodeWithArgumentDeclaration()
        {
            var argumentDeclarationNode = new ArgumentDeclarationNode("x");
            var programNode = new ProgramNode(
                new[] { argumentDeclarationNode },
                Enumerable.Empty<ConstantDeclarationNode>(),
                Enumerable.Empty<ModuleDeclarationNode>());

            var result = SymbolRegistry.Build(programNode);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Arguments, Has.Count.EqualTo(1));
            Assert.That(result.Arguments, Contains.Key("x"));
            Assert.That(result.Constants, Is.Empty);
            Assert.That(result.Modules, Is.Empty);
            Assert.That(result.SymbolTables, Is.Empty);
        }

        [Test]
        public void Should_CreateSymbolRegistryWithConstant_When_GivenProgramNodeWithConstantDeclaration()
        {
            var constantDeclarationNode = new ConstantDeclarationNode("x", BooleanValue.True);
            var programNode = new ProgramNode(
                Enumerable.Empty<ArgumentDeclarationNode>(),
                new[] { constantDeclarationNode },
                Enumerable.Empty<ModuleDeclarationNode>());

            var result = SymbolRegistry.Build(programNode);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Arguments, Is.Empty);
            Assert.That(result.Constants, Has.Count.EqualTo(1));
            Assert.That(result.Constants, Contains.Key("x"));
            Assert.That(result.Modules, Is.Empty);
            Assert.That(result.SymbolTables, Is.Empty);
        }

        [Test]
        public void Should_CreateSymbolRegistryWithModule_When_GivenProgramNodeWithModuleDeclaration()
        {
            var moduleDeclarationNode = new ModuleDeclarationNode(
                "x",
                Enumerable.Empty<ImportDeclarationNode>(),
                Enumerable.Empty<FormulaDeclarationNode>());
            var programNode = new ProgramNode(
                Enumerable.Empty<ArgumentDeclarationNode>(),
                Enumerable.Empty<ConstantDeclarationNode>(),
                new[] { moduleDeclarationNode });

            var result = SymbolRegistry.Build(programNode);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Arguments, Is.Empty);
            Assert.That(result.Constants, Is.Empty);
            Assert.That(result.Modules, Has.Count.EqualTo(1));
            Assert.That(result.Modules, Contains.Key("x"));
            Assert.That(result.SymbolTables, Has.Count.EqualTo(1));
            Assert.That(result.SymbolTables, Contains.Key(moduleDeclarationNode));
        }

        [Test]
        public void Should_CreateSymbolTableInRegistryWithSymbol_When_GivenProgramNodeWithModuleWithFormula()
        {
            var formulaDeclarationNode = new FormulaDeclarationNode("x", new IdentifierNode("y"), true);
            var moduleDeclarationNode = new ModuleDeclarationNode(
                "x",
                Enumerable.Empty<ImportDeclarationNode>(),
                new[] { formulaDeclarationNode });
            var programNode = new ProgramNode(
                Enumerable.Empty<ArgumentDeclarationNode>(),
                Enumerable.Empty<ConstantDeclarationNode>(),
                new[] { moduleDeclarationNode });

            var result = SymbolRegistry.Build(programNode).SymbolTables[moduleDeclarationNode];

            Assert.That(result, Is.Not.Null);
            Assert.That(result.TryResolve("x", out var resultSymbol), Is.True);
            Assert.That(resultSymbol, Is.Not.Null);
            Assert.That(resultSymbol.Name, Is.EqualTo("x"));
        }

        [Test]
        public void Should_CreateSymbolTableInRegistryWithSymbol_When_GivenProgramNodeWithModuleWithImport()
        {
            var importDeclarationNode = new ImportDeclarationNode("x", new[] { "y" }, ImportType.Constant);
            var moduleDeclarationNode = new ModuleDeclarationNode(
                "x",
                new[] { importDeclarationNode },
                Enumerable.Empty<FormulaDeclarationNode>());
            var programNode = new ProgramNode(
                Enumerable.Empty<ArgumentDeclarationNode>(),
                Enumerable.Empty<ConstantDeclarationNode>(),
                new[] { moduleDeclarationNode });

            var result = SymbolRegistry.Build(programNode).SymbolTables[moduleDeclarationNode];

            Assert.That(result, Is.Not.Null);
            Assert.That(result.TryResolve("x", out var resultSymbol), Is.True);
            Assert.That(resultSymbol, Is.Not.Null);
            Assert.That(resultSymbol.Name, Is.EqualTo("x"));
        }
    }
}