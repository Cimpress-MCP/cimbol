using Cimpress.Cimbol.Compiler.SyntaxTree;
using NUnit.Framework;

namespace Cimpress.Cimbol.UnitTests.Compiler.SyntaxTree
{
    [TestFixture]
    public class DefaultNodeTests
    {
        [Test]
        public void Should_SerializeToString_When_Valid()
        {
            var child = new LiteralNode(null);
            var node = new DefaultNode(new[] { "x", "y", "z" }, child);

            var result = node.ToString();

            Assert.That(result, Is.EqualTo("{DefaultNode}"));
        }

        [Test]
        public void Should_ReturnChildrenInOrder_When_IteratingChildren()
        {
            var child = new LiteralNode(null);
            var node = new DefaultNode(new[] { "x", "y", "z" }, child);

            var result = node.Children();

            Assert.That(result, Is.EqualTo(new IExpressionNode[] { child }));
        }

        [Test]
        public void Should_ReturnChildrenInOrder_When_IteratingChildrenReverse()
        {
            var child = new LiteralNode(null);
            var node = new DefaultNode(new[] { "x", "y", "z" }, child);

            var result = node.ChildrenReverse();

            Assert.That(result, Is.EqualTo(new IExpressionNode[] { child }));
        }

        [Test]
        public void Should_BeAsync_When_GivenAsyncChild()
        {
            var child = new UnaryOpNode(UnaryOpType.Await, new LiteralNode(null));
            var node = new DefaultNode(new[] { "x", "y", "z" }, child);

            var result = node.IsAsynchronous;

            Assert.That(result, Is.True);
        }

        [Test]
        public void ShouldNot_BeAsync_When_GivenNonAsyncChild()
        {
            var child = new UnaryOpNode(UnaryOpType.Negate, new LiteralNode(null));
            var node = new DefaultNode(new[] { "x", "y", "z" }, child);

            var result = node.IsAsynchronous;

            Assert.That(result, Is.False);
        }
    }
}