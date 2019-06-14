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
            var node = new BlockNode(new INode[] { null });
            Assert.AreEqual("{BlockNode}", node.ToString());
        }

        [Test]
        public void Should_BeEqual_When_ComparedToSameReference()
        {
            var node1 = new BlockNode(new INode[] { null });
            Assert.IsTrue(node1.Equals(node1));
        }

        [Test]
        public void Should_BeEqual_When_ComparedToSameValue()
        {
            var node1 = new BlockNode(new INode[] { null });
            var node2 = new BlockNode(new INode[] { null });
            Assert.IsTrue(node1.Equals(node2));
        }

        [Test]
        public void ShouldNot_BeEqual_When_ComparedToDifferentValue()
        {
            var node1 = new BlockNode(new INode[] { null });
            var node2 = new BlockNode(new INode[] { null, null });
            Assert.IsFalse(node1.Equals(node2));
        }

        [Test]
        public void ShouldNot_BeEqual_When_ComparedToDifferentType()
        {
            var node1 = new BlockNode(new INode[] { null });
            var node2 = new ConstantNode(1);
            Assert.IsFalse(node1.Equals(node2));
        }

        [Test]
        public void ShouldNot_BeEqual_When_ComparedToNull()
        {
            var node1 = new BlockNode(new INode[] { null });
            var node2 = null as BlockNode;
            Assert.IsFalse(node1.Equals(node2));
        }

        [Test]
        public void Should_ReturnChildrenInOrder_When_IteratingChildren()
        {
            var child1 = new ConstantNode(1);
            var child2 = new ConstantNode(2);
            var child3 = new ConstantNode(3);
            var node = new BlockNode(new[] { child1, child2, child3 });

            var expected = new[] { child1, child2, child3 };
            CollectionAssert.AreEqual(expected, node.Children());
        }

        [Test]
        public void Should_ReturnChildrenInOrder_When_IteratingChildrenReverse()
        {
            var child1 = new ConstantNode(1);
            var child2 = new ConstantNode(2);
            var child3 = new ConstantNode(3);
            var node = new BlockNode(new[] { child1, child2, child3 });

            var expected = new[] { child3, child2, child1 };
            CollectionAssert.AreEqual(expected, node.ChildrenReverse());
        }
    }
}