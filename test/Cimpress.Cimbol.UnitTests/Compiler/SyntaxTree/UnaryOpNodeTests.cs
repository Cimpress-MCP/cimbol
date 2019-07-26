using Cimpress.Cimbol.Compiler.SyntaxTree;
using NUnit.Framework;

namespace Cimpress.Cimbol.UnitTests.Compiler.SyntaxTree
{
    [TestFixture]
    public class UnaryOpNodeTests
    {
        [Test]
        public void Should_SerializeToString_When_Valid()
        {
            var node = new UnaryOpNode(UnaryOpType.Negate, null);
            Assert.AreEqual("{UnaryOpNode -}", node.ToString());
        }

        [Test]
        public void Should_ReturnChildrenInOrder_When_IteratingChildren()
        {
            var child1 = new LiteralNode(null);
            var node = new UnaryOpNode(UnaryOpType.Negate, child1);

            var expected = new[] { child1 };
            CollectionAssert.AreEqual(expected, node.Children());
        }

        [Test]
        public void Should_ReturnChildrenInOrder_When_IteratingChildrenReverse()
        {
            var child1 = new LiteralNode(null);
            var node = new UnaryOpNode(UnaryOpType.Negate, child1);

            var expected = new[] { child1 };
            CollectionAssert.AreEqual(expected, node.ChildrenReverse());
        }
    }
}