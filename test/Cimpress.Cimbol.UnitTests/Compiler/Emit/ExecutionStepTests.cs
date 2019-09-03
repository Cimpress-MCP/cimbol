using System;
using System.Linq;
using Cimpress.Cimbol.Compiler.Emit;
using Cimpress.Cimbol.Compiler.SyntaxTree;
using NUnit.Framework;

namespace Cimpress.Cimbol.UnitTests.Compiler.Emit
{
    [TestFixture]
    public class ExecutionStepTests
    {
        [Test]
        public void Should_Initialize_When_GivenValidParameters()
        {
            var formulaDeclarationNode = new FormulaNode("x", new IdentifierNode("y"), false);
            var moduleDeclarationNode = new ModuleNode(
                "a",
                Array.Empty<ImportNode>(),
                new[] { formulaDeclarationNode });

            var result = new ExecutionStep(
                0,
                moduleDeclarationNode,
                formulaDeclarationNode,
                ExecutionStepType.Synchronous,
                Enumerable.Empty<ExecutionStep>(),
                new SymbolTable(new SymbolRegistry()));

            Assert.That(result.DeclarationNode, Is.SameAs(formulaDeclarationNode));
            Assert.That(result.Type, Is.EqualTo(ExecutionStepType.Synchronous));
        }

        [Test]
        public void Should_BeMarkedAsAsynchronous_When_GivenAsynchronousType()
        {
            var formulaDeclarationNode = new FormulaNode("x", new IdentifierNode("y"), false);
            var moduleDeclarationNode = new ModuleNode(
                "a",
                Array.Empty<ImportNode>(),
                new[] { formulaDeclarationNode });
            var executionStep = new ExecutionStep(
                0,
                moduleDeclarationNode,
                formulaDeclarationNode,
                ExecutionStepType.Asynchronous,
                Enumerable.Empty<ExecutionStep>(),
                new SymbolTable(new SymbolRegistry()));

            var result = executionStep.IsAsynchronous;

            Assert.That(result, Is.True);
        }

        [Test]
        public void ShouldNot_BeMarkedAsAsynchronous_When_GivenSynchronousType()
        {
            var formulaDeclarationNode = new FormulaNode("x", new IdentifierNode("y"), false);
            var moduleDeclarationNode = new ModuleNode(
                "a",
                Array.Empty<ImportNode>(),
                new[] { formulaDeclarationNode });
            var executionStep = new ExecutionStep(
                0,
                moduleDeclarationNode,
                formulaDeclarationNode,
                ExecutionStepType.Synchronous,
                Enumerable.Empty<ExecutionStep>(),
                new SymbolTable(new SymbolRegistry()));

            var result = executionStep.IsAsynchronous;

            Assert.That(result, Is.False);
        }
    }
}