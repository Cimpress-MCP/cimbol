using Cimpress.Cimbol.Compiler.SyntaxTree;
using NUnit.Framework;

namespace Cimpress.Cimbol.UnitTests.Compiler.SyntaxTree
{
    [TestFixture]
    public class FormulaDeclarationNodeTests
    {
        [Test]
        public void Should_SerializeToString_When_Valid()
        {
            var node = new FormulaDeclarationNode("a", null, true);

            Assert.That(node.ToString(), Is.EqualTo("{FormulaDeclarationNode a}"));
        }

        [Test]
        public void Should_ReturnChildrenInOrder_When_IteratingChildren()
        {
            var child1 = new LiteralNode(null);
            var node = new FormulaDeclarationNode("a", child1, true);

            var expected = new[] { child1 };
            CollectionAssert.AreEqual(expected, node.Children());
        }

        [Test]
        public void Should_ReturnChildrenInOrder_When_IteratingChildrenReverse()
        {
            var child1 = new LiteralNode(null);
            var node = new FormulaDeclarationNode("a", child1, true);

            var expected = new[] { child1 };
            CollectionAssert.AreEqual(expected, node.ChildrenReverse());
        }
    }
}