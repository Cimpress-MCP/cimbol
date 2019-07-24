using System.Linq;
using Cimpress.Cimbol.Compiler.SyntaxTree;
using NUnit.Framework;

namespace Cimpress.Cimbol.UnitTests.Compiler.SyntaxTree
{
    [TestFixture]
    public class LiteralNodeTests
    {
        [Test]
        public void Should_SerializeToString_When_Valid()
        {
            var node = new LiteralNode(1);
            Assert.AreEqual("{LiteralNode 1}", node.ToString());
        }

        [Test]
        public void Should_ReturnEmptyEnumerable_When_IteratingChildren()
        {
            var node = new LiteralNode(1);
            CollectionAssert.AreEqual(Enumerable.Empty<IExpressionNode>(), node.Children());
        }

        [Test]
        public void Should_ReturnEmptyEnumerable_When_IteratingChildrenReverse()
        {
            var node = new LiteralNode(1);
            CollectionAssert.AreEqual(Enumerable.Empty<IExpressionNode>(), node.ChildrenReverse());
        }
    }
}