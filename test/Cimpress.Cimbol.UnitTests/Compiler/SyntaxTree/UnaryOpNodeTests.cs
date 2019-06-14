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
        public void Should_BeEqual_When_ComparedToSameReference()
        {
            var node1 = new UnaryOpNode(UnaryOpType.Negate, null);
            Assert.IsTrue(node1.Equals(node1));
        }

        [Test]
        public void Should_BeEqual_When_ComparedToSameValue()
        {
            var node1 = new UnaryOpNode(UnaryOpType.Negate, null);
            var node2 = new UnaryOpNode(UnaryOpType.Negate, null);
            Assert.IsTrue(node1.Equals(node2));
        }

        [Test]
        public void ShouldNot_BeEqual_When_ComparedToDifferentValue()
        {
            var node1 = new UnaryOpNode(UnaryOpType.Negate, null);
            var node2 = new UnaryOpNode(UnaryOpType.Not, null);
            Assert.IsFalse(node1.Equals(node2));
        }

        [Test]
        public void ShouldNot_BeEqual_When_ComparedToDifferentType()
        {
            var node1 = new UnaryOpNode(UnaryOpType.Negate, null);
            var node2 = new ConstantNode(1);
            Assert.IsFalse(node1.Equals(node2));
        }

        [Test]
        public void ShouldNot_BeEqual_When_ComparedToNull()
        {
            var node1 = new UnaryOpNode(UnaryOpType.Negate, null);
            var node2 = null as UnaryOpNode;
            Assert.IsFalse(node1.Equals(node2));
        }

        [Test]
        public void Should_ReturnChildrenInOrder_When_IteratingChildren()
        {
            var child1 = new ConstantNode(1);
            var node = new UnaryOpNode(UnaryOpType.Negate, child1);

            var expected = new[] { child1 };
            CollectionAssert.AreEqual(expected, node.Children());
        }

        [Test]
        public void Should_ReturnChildrenInOrder_When_IteratingChildrenReverse()
        {
            var child1 = new ConstantNode(1);
            var node = new UnaryOpNode(UnaryOpType.Negate, child1);

            var expected = new[] { child1 };
            CollectionAssert.AreEqual(expected, node.ChildrenReverse());
        }
    }
}