using Cimpress.Cimbol.Compiler.SyntaxTree;
using NUnit.Framework;

namespace Cimpress.Cimbol.UnitTests.Compiler.SyntaxTree
{
    [TestFixture]
    public class AccessNodeTests
    {
        [Test]
        public void Should_SerializeToString_When_Valid()
        {
            var node = new AccessNode(null, "x");
            Assert.That(node.ToString(), Is.EqualTo("{AccessNode x}"));
        }

        [Test]
        public void Should_ReturnChildrenInOrder_When_IteratingChildren()
        {
            var child1 = new LiteralNode(1);
            var node = new AccessNode(child1, "x");

            var expected = new IExpressionNode[] { child1 };
            Assert.That(node.Children(), Is.EqualTo(expected));
        }

        [Test]
        public void Should_ReturnChildrenInOrder_When_IteratingChildrenReverse()
        {
            var child1 = new LiteralNode(1);
            var node = new AccessNode(child1, "x");

            var expected = new IExpressionNode[] { child1 };
            Assert.That(node.ChildrenReverse(), Is.EqualTo(expected));
        }
    }
}