using Cimpress.Cimbol.Compiler.SyntaxTree;
using NUnit.Framework;

namespace Cimpress.Cimbol.UnitTests.Compiler.SyntaxTree
{
    [TestFixture]
    public class TreeWalkerTests
    {
        [Test]
        public void Should_TraversePreOrder_When_GivenSingleNode()
        {
            var node1 = new ConstantNode(1);
            var traversal = new TreeWalker(node1);

            var expected = new INode[] { node1 };
            CollectionAssert.AreEqual(expected, traversal.TraversePreOrder());
        }

        [Test]
        public void Should_TraversePostOrder_When_GivenSingleNode()
        {
            var node1 = new ConstantNode(1);
            var traversal = new TreeWalker(node1);

            var expected = new INode[] { node1 };
            CollectionAssert.AreEqual(expected, traversal.TraversePostOrder());
        }

        [Test]
        public void Should_TraversePreOrder_When_GivenSingleLevel()
        {
            var node1 = new IdentifierNode("x");
            var node2 = new ConstantNode(1);
            var node3 = new ConstantNode(2);
            var node4 = new ConstantNode(3);
            var node5 = new InvokeNode(node1, new[] { node2, node3, node4 });
            var traversal = new TreeWalker(node5);

            var expected = new INode[] { node5, node1, node2, node3, node4 };
            CollectionAssert.AreEqual(expected, traversal.TraversePreOrder());
        }

        [Test]
        public void Should_TraversePostOrder_When_GivenSingleLevel()
        {
            var node1 = new IdentifierNode("x");
            var node2 = new ConstantNode(1);
            var node3 = new ConstantNode(2);
            var node4 = new ConstantNode(3);
            var node5 = new InvokeNode(node1, new[] { node2, node3, node4 });
            var traversal = new TreeWalker(node5);

            var expected = new INode[] { node1, node2, node3, node4, node5 };
            CollectionAssert.AreEqual(expected, traversal.TraversePostOrder());
        }

        [Test]
        public void Should_TraversePreOrder_When_GivenManyLevels()
        {
            var node1 = new ConstantNode(1);
            var node2 = new ConstantNode(2);
            var node3 = new BinaryOpNode(BinaryOpType.Add, node1, node2);
            var node4 = new ConstantNode(3);
            var node5 = new BinaryOpNode(BinaryOpType.Add, node3, node4);
            var traversal = new TreeWalker(node5);

            var expected = new INode[] { node5, node3, node1, node2, node4 };
            CollectionAssert.AreEqual(expected, traversal.TraversePreOrder());
        }

        [Test]
        public void Should_TraversePostOrder_When_GivenManyLevels()
        {
            var node1 = new ConstantNode(1);
            var node2 = new ConstantNode(2);
            var node3 = new BinaryOpNode(BinaryOpType.Add, node1, node2);
            var node4 = new ConstantNode(3);
            var node5 = new BinaryOpNode(BinaryOpType.Add, node3, node4);
            var traversal = new TreeWalker(node5);

            var expected = new INode[] { node1, node2, node3, node4, node5 };
            CollectionAssert.AreEqual(expected, traversal.TraversePostOrder());
        }
    }
}