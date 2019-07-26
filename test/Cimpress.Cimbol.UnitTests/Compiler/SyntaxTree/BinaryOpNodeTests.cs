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
            var node = new BinaryOpNode(BinaryOpType.Add, null, null);
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
    }
}