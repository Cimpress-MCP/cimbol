using System.Linq;
using Cimpress.Cimbol.Compiler.Emit;
using Cimpress.Cimbol.Compiler.SyntaxTree;
using NUnit.Framework;

namespace Cimpress.Cimbol.UnitTests.Compiler.Emit
{
    [TestFixture]
    public class ExecutionGroupTests
    {
        [Test]
        public void Should_BeMarkedAsAsynchronous_When_ContainsAsynchronousStep()
        {
            var formulaNode1 = new FormulaDeclarationNode("x", new LiteralNode(null), false);
            var executionStep1 = new ExecutionStep(formulaNode1, ExecutionStepType.Asynchronous);
            var executionGroup = new ExecutionGroup(new[] { executionStep1 });

            var result = executionGroup.IsAsynchronous;

            Assert.That(result, Is.True);
        }

        [Test]
        public void ShouldNot_BeMarkedAsAsynchronous_When_ContainsSynchronousStep()
        {
            var formulaNode1 = new FormulaDeclarationNode("x", new LiteralNode(null), false);
            var executionStep1 = new ExecutionStep(formulaNode1, ExecutionStepType.Synchronous);
            var executionGroup = new ExecutionGroup(new[] { executionStep1 });

            var result = executionGroup.IsAsynchronous;

            Assert.That(result, Is.False);
        }

        [Test]
        public void Should_MergeTwoExecutionGroups_When_NeitherAreAsynchronous()
        {
            var formulaNode1 = new FormulaDeclarationNode("x", new LiteralNode(null), false);
            var formulaNode2 = new FormulaDeclarationNode("y", new LiteralNode(null), false);
            var executionStep1 = new ExecutionStep(formulaNode1, ExecutionStepType.Synchronous);
            var executionStep2 = new ExecutionStep(formulaNode2, ExecutionStepType.Synchronous);
            var executionGroup1 = new ExecutionGroup(new[] { executionStep1 });
            var executionGroup2 = new ExecutionGroup(new[] { executionStep2 });

            var result = ExecutionGroup.Merge(new[] { executionGroup1, executionGroup2 }).ToArray();

            Assert.That(result, Has.Length.EqualTo(1));
            var resultGroup = result.ElementAt(0);
            Assert.That(resultGroup.ExecutionSteps.Count, Is.EqualTo(2));
            Assert.That(resultGroup.ExecutionSteps.Select(x => x.Node.Name), Is.EqualTo(new[] { "x", "y" }));
        }

        [Test]
        public void Should_MergeTwoExecutionGroups_When_SecondIsAsynchronous()
        {
            var formulaNode1 = new FormulaDeclarationNode("x", new LiteralNode(null), false);
            var formulaNode2 = new FormulaDeclarationNode("y", new LiteralNode(null), false);
            var executionStep1 = new ExecutionStep(formulaNode1, ExecutionStepType.Synchronous);
            var executionStep2 = new ExecutionStep(formulaNode2, ExecutionStepType.Asynchronous);
            var executionGroup1 = new ExecutionGroup(new[] { executionStep1 });
            var executionGroup2 = new ExecutionGroup(new[] { executionStep2 });

            var result = ExecutionGroup.Merge(new[] { executionGroup1, executionGroup2 }).ToArray();

            Assert.That(result, Has.Length.EqualTo(1));
            var resultGroup = result.ElementAt(0);
            Assert.That(resultGroup.ExecutionSteps.Count, Is.EqualTo(2));
            Assert.That(resultGroup.ExecutionSteps.Select(x => x.Node.Name), Is.EqualTo(new[] { "x", "y" }));
        }

        [Test]
        public void ShouldNot_MergeTwoExecutionGroups_When_FirstIsAsynchronous()
        {
            var formulaNode1 = new FormulaDeclarationNode("x", new LiteralNode(null), false);
            var formulaNode2 = new FormulaDeclarationNode("y", new LiteralNode(null), false);
            var executionStep1 = new ExecutionStep(formulaNode1, ExecutionStepType.Asynchronous);
            var executionStep2 = new ExecutionStep(formulaNode2, ExecutionStepType.Synchronous);
            var executionGroup1 = new ExecutionGroup(new[] { executionStep1 });
            var executionGroup2 = new ExecutionGroup(new[] { executionStep2 });

            var result = ExecutionGroup.Merge(new[] { executionGroup1, executionGroup2 }).ToArray();

            Assert.That(result, Has.Length.EqualTo(2));
            var resultGroup1 = result.ElementAt(0);
            Assert.That(resultGroup1.ExecutionSteps.Count, Is.EqualTo(1));
            Assert.That(resultGroup1.ExecutionSteps.Select(x => x.Node.Name), Is.EqualTo(new[] { "x" }));
            var resultGroup2 = result.ElementAt(1);
            Assert.That(resultGroup2.ExecutionSteps.Count, Is.EqualTo(1));
            Assert.That(resultGroup2.ExecutionSteps.Select(x => x.Node.Name), Is.EqualTo(new[] { "y" }));
        }

        [Test]
        public void ShouldNot_MergeTwoExecutionGroups_When_BothAreAsynchronous()
        {
            var formulaNode1 = new FormulaDeclarationNode("x", new LiteralNode(null), false);
            var formulaNode2 = new FormulaDeclarationNode("y", new LiteralNode(null), false);
            var executionStep1 = new ExecutionStep(formulaNode1, ExecutionStepType.Asynchronous);
            var executionStep2 = new ExecutionStep(formulaNode2, ExecutionStepType.Asynchronous);
            var executionGroup1 = new ExecutionGroup(new[] { executionStep1 });
            var executionGroup2 = new ExecutionGroup(new[] { executionStep2 });

            var result = ExecutionGroup.Merge(new[] { executionGroup1, executionGroup2 }).ToArray();

            Assert.That(result, Has.Length.EqualTo(2));
            var resultGroup1 = result.ElementAt(0);
            Assert.That(resultGroup1.ExecutionSteps.Count, Is.EqualTo(1));
            Assert.That(resultGroup1.ExecutionSteps.Select(x => x.Node.Name), Is.EqualTo(new[] { "x" }));
            var resultGroup2 = result.ElementAt(1);
            Assert.That(resultGroup2.ExecutionSteps.Count, Is.EqualTo(1));
            Assert.That(resultGroup2.ExecutionSteps.Select(x => x.Node.Name), Is.EqualTo(new[] { "y" }));
        }

        [Test]
        public void Should_ReturnEmptyEnumerable_When_MergingNullListOfExecutionGroups()
        {
            var result = ExecutionGroup.Merge(null).ToArray();

            Assert.That(result, Is.Empty);
        }
    }
}