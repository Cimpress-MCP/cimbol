using Cimpress.Cimbol.Compiler.SyntaxTree;
using Cimpress.Cimbol.Utilities;
using NUnit.Framework;

namespace Cimpress.Cimbol.UnitTests.Compiler.Utilities
{
    [TestFixture]
    public class TreeWalkerTests
    {
        [Test]
        public void Should_TraversePreOrder_When_GivenSingleNode()
        {
            var node1 = new LiteralNode(null);
            var traversal = new TreeWalker(node1);

            var expected = new ISyntaxNode[] { node1 };
            CollectionAssert.AreEqual(expected, traversal.TraversePreOrder());
        }

        [Test]
        public void Should_TraversePostOrder_When_GivenSingleNode()
        {
            var node1 = new LiteralNode(null);
            var traversal = new TreeWalker(node1);

            var expected = new ISyntaxNode[] { node1 };
            CollectionAssert.AreEqual(expected, traversal.TraversePostOrder());
        }

        [Test]
        public void Should_TraversePreOrder_When_GivenSingleLevel()
        {
            var node1 = new IdentifierNode("x");
            var argument1 = new PositionalArgument(new LiteralNode(null));
            var argument2 = new PositionalArgument(new LiteralNode(null));
            var argument3 = new PositionalArgument(new LiteralNode(null));
            var node2 = new InvokeNode(node1, new[] { argument1, argument2, argument3 });
            var traversal = new TreeWalker(node2);

            var expected = new[] { node2, node1, argument1.Value, argument2.Value, argument3.Value };
            CollectionAssert.AreEqual(expected, traversal.TraversePreOrder());
        }

        [Test]
        public void Should_TraversePostOrder_When_GivenSingleLevel()
        {
            var node1 = new IdentifierNode("x");
            var argument1 = new PositionalArgument(new LiteralNode(null));
            var argument2 = new PositionalArgument(new LiteralNode(null));
            var argument3 = new PositionalArgument(new LiteralNode(null));
            var node2 = new InvokeNode(node1, new[] { argument1, argument2, argument3 });
            var traversal = new TreeWalker(node2);

            var expected = new[] { node1, argument1.Value, argument2.Value, argument3.Value, node2 };
            CollectionAssert.AreEqual(expected, traversal.TraversePostOrder());
        }

        [Test]
        public void Should_TraversePreOrder_When_GivenManyLevels()
        {
            var node1 = new LiteralNode(null);
            var node2 = new LiteralNode(null);
            var node3 = new BinaryOpNode(BinaryOpType.Add, node1, node2);
            var node4 = new LiteralNode(null);
            var node5 = new BinaryOpNode(BinaryOpType.Add, node3, node4);
            var traversal = new TreeWalker(node5);

            var expected = new ISyntaxNode[] { node5, node3, node1, node2, node4 };
            CollectionAssert.AreEqual(expected, traversal.TraversePreOrder());
        }

        [Test]
        public void Should_TraversePostOrder_When_GivenManyLevels()
        {
            var node1 = new LiteralNode(null);
            var node2 = new LiteralNode(null);
            var node3 = new BinaryOpNode(BinaryOpType.Add, node1, node2);
            var node4 = new LiteralNode(null);
            var node5 = new BinaryOpNode(BinaryOpType.Add, node3, node4);
            var traversal = new TreeWalker(node5);

            var expected = new ISyntaxNode[] { node1, node2, node3, node4, node5 };
            CollectionAssert.AreEqual(expected, traversal.TraversePostOrder());
        }

        [Test]
        public void Should_CallAllEnterHandlers_When_TraversingGraph()
        {
            bool hitFirst = false, hitSecond = false;
            var node1 = new LiteralNode(null);
            var traversal = new TreeWalker(node1);
            traversal.OnEnter<LiteralNode>(node => hitFirst = true);
            traversal.OnEnter<LiteralNode>(node => hitSecond = true);

            traversal.Visit();

            Assert.That(hitFirst, Is.True);
            Assert.That(hitSecond, Is.True);
        }

        [Test]
        public void Should_CallAllExitHandlers_When_TraversingGraph()
        {
            bool hitFirst = false, hitSecond = false;
            var node1 = new LiteralNode(null);
            var traversal = new TreeWalker(node1);
            traversal.OnExit<LiteralNode>(node => hitFirst = true);
            traversal.OnExit<LiteralNode>(node => hitSecond = true);

            traversal.Visit();

            Assert.That(hitFirst, Is.True);
            Assert.That(hitSecond, Is.True);
        }
    }
}