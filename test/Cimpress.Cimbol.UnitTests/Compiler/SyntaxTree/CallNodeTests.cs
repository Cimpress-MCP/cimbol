using Cimpress.Cimbol.Compiler.SyntaxTree;
using NUnit.Framework;

namespace Cimpress.Cimbol.UnitTests.Compiler.SyntaxTree
{
    [TestFixture]
    public class CallNodeTests
    {
        [Test]
        public void Should_SerializeToString_When_Valid()
        {
            var node = new CallNode(null, new PositionalArgument[] { null });
            Assert.AreEqual("{CallNode}", node.ToString());
        }

        [Test]
        public void Should_BeEqual_When_ComparedToSameReference()
        {
            var node1 = new CallNode(null, new PositionalArgument[] { null });
            Assert.IsTrue(node1.Equals(node1));
        }

        [Test]
        public void Should_BeEqual_When_ComparedToSameValue()
        {
            var node1 = new CallNode(null, new PositionalArgument[] { null });
            var node2 = new CallNode(null, new PositionalArgument[] { null });
            Assert.IsTrue(node1.Equals(node2));
        }

        [Test]
        public void ShouldNot_BeEqual_When_ComparedToDifferentValue()
        {
            var node1 = new CallNode(null, new PositionalArgument[] { null });
            var node2 = new CallNode(null, new PositionalArgument[] { null, null });
            Assert.IsFalse(node1.Equals(node2));
        }

        [Test]
        public void ShouldNot_BeEqual_When_ComparedToDifferentType()
        {
            var node1 = new CallNode(null, new PositionalArgument[] { null });
            var node2 = new ConstantNode(1);
            Assert.IsFalse(node1.Equals(node2));
        }

        [Test]
        public void ShouldNot_BeEqual_When_ComparedToNull()
        {
            var node1 = new CallNode(null, new PositionalArgument[] { null });
            var node2 = null as CallNode;
            Assert.IsFalse(node1.Equals(node2));
        }

        [Test]
        public void Should_ReturnChildrenInOrder_When_IteratingChildren()
        {
            var child1 = new IdentifierNode("x");
            var child2 = new ConstantNode(1);
            var child3 = new ConstantNode(2);
            var node = new CallNode(child1, new[] { new PositionalArgument(child2), new PositionalArgument(child3) });

            var expected = new INode[] { child1, child2, child3 };
            CollectionAssert.AreEqual(expected, node.Children());
        }

        [Test]
        public void Should_ReturnChildrenInOrder_When_IteratingChildrenReverse()
        {
            var child1 = new IdentifierNode("x");
            var child2 = new ConstantNode(1);
            var child3 = new ConstantNode(2);
            var node = new CallNode(child1, new[] { new PositionalArgument(child2), new PositionalArgument(child3) });

            var expected = new INode[] { child3, child2, child1 };
            CollectionAssert.AreEqual(expected, node.ChildrenReverse());
        }
    }
}