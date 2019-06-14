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
            var node = new InvokeNode(null, new INode[] { null });
            Assert.AreEqual("{InvokeNode}", node.ToString());
        }

        [Test]
        public void Should_BeEqual_When_ComparedToSameReference()
        {
            var node1 = new InvokeNode(null, new INode[] { null });
            Assert.IsTrue(node1.Equals(node1));
        }

        [Test]
        public void Should_BeEqual_When_ComparedToSameValue()
        {
            var node1 = new InvokeNode(null, new INode[] { null });
            var node2 = new InvokeNode(null, new INode[] { null });
            Assert.IsTrue(node1.Equals(node2));
        }

        [Test]
        public void ShouldNot_BeEqual_When_ComparedToDifferentValue()
        {
            var node1 = new InvokeNode(null, new INode[] { null });
            var node2 = new InvokeNode(null, new INode[] { null, null });
            Assert.IsFalse(node1.Equals(node2));
        }

        [Test]
        public void ShouldNot_BeEqual_When_ComparedToDifferentType()
        {
            var node1 = new InvokeNode(null, new INode[] { null });
            var node2 = new ConstantNode(1);
            Assert.IsFalse(node1.Equals(node2));
        }

        [Test]
        public void ShouldNot_BeEqual_When_ComparedToNull()
        {
            var node1 = new InvokeNode(null, new INode[] { null });
            var node2 = null as InvokeNode;
            Assert.IsFalse(node1.Equals(node2));
        }

        [Test]
        public void Should_ReturnChildrenInOrder_When_IteratingChildren()
        {
            var child1 = new IdentifierNode("x");
            var child2 = new ConstantNode(1);
            var child3 = new ConstantNode(2);
            var node = new InvokeNode(child1, new[] { child2, child3 });

            var expected = new INode[] { child1, child2, child3 };
            CollectionAssert.AreEqual(expected, node.Children());
        }

        [Test]
        public void Should_ReturnChildrenInOrder_When_IteratingChildrenReverse()
        {
            var child1 = new IdentifierNode("x");
            var child2 = new ConstantNode(1);
            var child3 = new ConstantNode(2);
            var node = new InvokeNode(child1, new[] { child2, child3 });

            var expected = new INode[] { child3, child2, child1 };
            CollectionAssert.AreEqual(expected, node.ChildrenReverse());
        }
    }
}