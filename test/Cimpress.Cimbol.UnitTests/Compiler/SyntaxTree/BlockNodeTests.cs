using System.Linq;
using Cimpress.Cimbol.Compiler.SyntaxTree;
using NUnit.Framework;

namespace Cimpress.Cimbol.UnitTests.Compiler.SyntaxTree
{
    [TestFixture]
    public class BlockNodeTests
    {
        [Test]
        public void Should_SerializeToString_When_Valid()
        {
            var node = new BlockNode(Enumerable.Empty<IExpressionNode>());
            Assert.AreEqual("{BlockNode}", node.ToString());
        }

        [Test]
        public void Should_ReturnChildrenInOrder_When_IteratingChildren()
        {
            var child1 = new LiteralNode(null);
            var child2 = new LiteralNode(null);
            var child3 = new LiteralNode(null);
            var node = new BlockNode(new[] { child1, child2, child3 });

            var expected = new[] { child1, child2, child3 };
            CollectionAssert.AreEqual(expected, node.Children());
        }

        [Test]
        public void Should_ReturnChildrenInOrder_When_IteratingChildrenReverse()
        {
            var child1 = new LiteralNode(null);
            var child2 = new LiteralNode(null);
            var child3 = new LiteralNode(null);
            var node = new BlockNode(new[] { child1, child2, child3 });

            var expected = new[] { child3, child2, child1 };
            CollectionAssert.AreEqual(expected, node.ChildrenReverse());
        }

        [Test]
        public void Should_BeAsync_When_AnyExpressionIsAsync()
        {
            var child1 = new UnaryOpNode(UnaryOpType.Await, new LiteralNode(null));
            var child2 = new LiteralNode(null);
            var child3 = new LiteralNode(null);
            var node = new BlockNode(new IExpressionNode[] { child1, child2, child3 });

            var result = node.IsAsynchronous;

            Assert.That(result, Is.True);
        }

        [Test]
        public void Should_BeAsync_When_AllExpressionsAreAsync()
        {
            var child1 = new UnaryOpNode(UnaryOpType.Await, new LiteralNode(null));
            var child2 = new UnaryOpNode(UnaryOpType.Await, new LiteralNode(null));
            var child3 = new UnaryOpNode(UnaryOpType.Await, new LiteralNode(null));
            var node = new BlockNode(new IExpressionNode[] { child1, child2, child3 });

            var result = node.IsAsynchronous;

            Assert.That(result, Is.True);
        }

        [Test]
        public void Should_BeAsync_When_NoExpressionsAreAsync()
        {
            var child1 = new LiteralNode(null);
            var child2 = new LiteralNode(null);
            var child3 = new LiteralNode(null);
            var node = new BlockNode(new IExpressionNode[] { child1, child2, child3 });

            var result = node.IsAsynchronous;

            Assert.That(result, Is.False);
        }
    }
}