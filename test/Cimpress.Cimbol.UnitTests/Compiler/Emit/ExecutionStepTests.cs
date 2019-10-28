using System;
using System.Linq;
using Cimpress.Cimbol.Compiler.Emit;
using Cimpress.Cimbol.Compiler.SyntaxTree;
using Cimpress.Cimbol.Runtime.Types;
using NUnit.Framework;

namespace Cimpress.Cimbol.UnitTests.Compiler.Emit
{
    [TestFixture]
    public class ExecutionStepTests
    {
        [Test]
        public void Should_ThrowArgumentNullException_When_ConstructedWithNullDeclarationNode()
        {
            var moduleNode = new ModuleNode("Module01", Array.Empty<ImportNode>(), Array.Empty<FormulaNode>());

            Assert.Throws<ArgumentNullException>(() => new ExecutionStep(
                0,
                moduleNode,
                null,
                ExecutionStepType.Synchronous,
                Array.Empty<ExecutionStep>()));
        }

        [Test]
        public void Should_ThrowArgumentNullException_When_ConstructedWithNullModuleNode()
        {
            var declarationNode = new FormulaNode("Formula01", new LiteralNode(BooleanValue.True), false);

            Assert.Throws<ArgumentNullException>(() => new ExecutionStep(
                0,
                null,
                declarationNode,
                ExecutionStepType.Synchronous,
                Array.Empty<ExecutionStep>()));
        }

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
                Enumerable.Empty<ExecutionStep>());

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
                Enumerable.Empty<ExecutionStep>());

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
                Enumerable.Empty<ExecutionStep>());

            var result = executionStep.IsAsynchronous;

            Assert.That(result, Is.False);
        }
    }
}