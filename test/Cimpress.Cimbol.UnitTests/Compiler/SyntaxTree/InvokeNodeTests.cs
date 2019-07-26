using Cimpress.Cimbol.Compiler.SyntaxTree;
using NUnit.Framework;

namespace Cimpress.Cimbol.UnitTests.Compiler.SyntaxTree
{
    [TestFixture]
    public class InvokeNodeTests
    {
        [Test]
        public void Should_SerializeToString_When_Valid()
        {
            var node = new InvokeNode(null, new PositionalArgument[] { null });
            Assert.AreEqual("{InvokeNode}", node.ToString());
        }

        [Test]
        public void Should_ReturnChildrenInOrder_When_IteratingChildren()
        {
            var child1 = new IdentifierNode("x");
            var child2 = new LiteralNode(null);
            var child3 = new LiteralNode(null);
            var node = new InvokeNode(child1, new[] { new PositionalArgument(child2), new PositionalArgument(child3) });

            var expected = new IExpressionNode[] { child1, child2, child3 };
            CollectionAssert.AreEqual(expected, node.Children());
        }

        [Test]
        public void Should_ReturnChildrenInOrder_When_IteratingChildrenReverse()
        {
            var child1 = new IdentifierNode("x");
            var child2 = new LiteralNode(null);
            var child3 = new LiteralNode(null);
            var node = new InvokeNode(child1, new[] { new PositionalArgument(child2), new PositionalArgument(child3) });

            var expected = new IExpressionNode[] { child3, child2, child1 };
            CollectionAssert.AreEqual(expected, node.ChildrenReverse());
        }
    }
}