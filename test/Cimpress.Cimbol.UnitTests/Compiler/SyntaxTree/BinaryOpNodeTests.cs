using Cimpress.Cimbol.Compiler.SyntaxTree;
using NUnit.Framework;

namespace Cimpress.Cimbol.UnitTests.Compiler.SyntaxTree
{
    [TestFixture]
    public class BinaryOpNodeTests
    {
        [Test]
        public void Should_SerializeToString_When_Valid()
        {
            var child1 = new LiteralNode(null);
            var node = new BinaryOpNode(BinaryOpType.Add, child1, child1);
            Assert.AreEqual("{BinaryOpNode +}", node.ToString());
        }

        [Test]
        public void Should_ReturnChildrenInOrder_When_IteratingChildren()
        {
            var child1 = new LiteralNode(null);
            var child2 = new LiteralNode(null);
            var node = new BinaryOpNode(BinaryOpType.Add, child1, child2);

            var expected = new[] { child1, child2 };
            CollectionAssert.AreEqual(expected, node.Children());
        }

        [Test]
        public void Should_ReturnChildrenInOrder_When_IteratingChildrenReverse()
        {
            var child1 = new LiteralNode(null);
            var child2 = new LiteralNode(null);
            var node = new BinaryOpNode(BinaryOpType.Add, child1, child2);

            var expected = new[] { child2, child1 };
            CollectionAssert.AreEqual(expected, node.ChildrenReverse());
        }

        [Test]
        public void Should_BeAsync_When_GivenAsyncLeftChild()
        {
            var child1 = new UnaryOpNode(UnaryOpType.Await, new LiteralNode(null));
            var child2 = new LiteralNode(null);
            var node = new BinaryOpNode(BinaryOpType.Add, child1, child2);

            var result = node.IsAsynchronous;

            Assert.That(result, Is.True);
        }

        [Test]
        public void Should_BeAsync_When_GivenAsyncRightChild()
        {
            var child1 = new LiteralNode(null);
            var child2 = new UnaryOpNode(UnaryOpType.Await, new LiteralNode(null));
            var node = new BinaryOpNode(BinaryOpType.Add, child1, child2);

            var result = node.IsAsynchronous;

            Assert.That(result, Is.True);
        }

        [Test]
        public void ShouldNot_BeAsync_When_GivenNonAsyncChildren()
        {
            var child1 = new LiteralNode(null);
            var child2 = new LiteralNode(null);
            var node = new BinaryOpNode(BinaryOpType.Add, child1, child2);

            var result = node.IsAsynchronous;

            Assert.That(result, Is.False);
        }
    }
}