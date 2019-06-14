using System.Linq;
using Cimpress.Cimbol.Compiler.SyntaxTree;
using NUnit.Framework;

namespace Cimpress.Cimbol.UnitTests.Compiler.SyntaxTree
{
    [TestFixture]
    public class ConstantNodeTests
    {
        [Test]
        public void Should_SerializeToString_When_Valid()
        {
            var node = new ConstantNode(1);
            Assert.AreEqual("{ConstantNode 1}", node.ToString());
        }

        [Test]
        public void Should_BeEqual_When_ComparedToSameReference()
        {
            var node1 = new ConstantNode(1);
            Assert.IsTrue(node1.Equals(node1));
        }

        [Test]
        public void Should_BeEqual_When_ComparedToSameValue()
        {
            var node1 = new ConstantNode(1);
            var node2 = new ConstantNode(1);
            Assert.IsTrue(node1.Equals(node2));
        }

        [Test]
        public void ShouldNot_BeEqual_When_ComparedToDifferentValue()
        {
            var node1 = new ConstantNode(1);
            var node2 = new ConstantNode(2);
            Assert.IsFalse(node1.Equals(node2));
        }

        [Test]
        public void ShouldNot_BeEqual_When_ComparedToDifferentType()
        {
            var node1 = new ConstantNode(1);
            var node2 = new IdentifierNode("x");
            Assert.IsFalse(node1.Equals(node2));
        }

        [Test]
        public void ShouldNot_BeEqual_When_ComparedToNull()
        {
            var node1 = new ConstantNode(1);
            var node2 = null as ConstantNode;
            Assert.IsFalse(node1.Equals(node2));
        }

        [Test]
        public void Should_ReturnEmptyEnumerable_When_IteratingChildren()
        {
            var node = new ConstantNode(1);
            CollectionAssert.AreEqual(Enumerable.Empty<INode>(), node.Children());
        }

        [Test]
        public void Should_ReturnEmptyEnumerable_When_IteratingChildrenReverse()
        {
            var node = new ConstantNode(1);
            CollectionAssert.AreEqual(Enumerable.Empty<INode>(), node.ChildrenReverse());
        }
    }
}