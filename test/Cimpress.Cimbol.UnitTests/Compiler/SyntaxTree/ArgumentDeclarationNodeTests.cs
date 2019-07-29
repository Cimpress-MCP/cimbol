using System.Linq;
using Cimpress.Cimbol.Compiler.SyntaxTree;
using NUnit.Framework;

namespace Cimpress.Cimbol.UnitTests.Compiler.SyntaxTree
{
    [TestFixture]
    public class ArgumentDeclarationNodeTests
    {
        [Test]
        public void Should_SerializeToString_When_Valid()
        {
            var node = new ArgumentDeclarationNode("x");

            var result = node.ToString();

            Assert.That(result, Is.EqualTo("{ArgumentDeclarationNode x}"));
        }

        [Test]
        public void Should_ReturnEmptyEnumerable_When_IteratingChildren()
        {
            var node = new ArgumentDeclarationNode("x");

            var result = node.Children();

            Assert.That(result, Is.EqualTo(Enumerable.Empty<IExpressionNode>()));
        }

        [Test]
        public void Should_ReturnEmptyEnumerable_When_IteratingChildrenReverse()
        {
            var node = new ArgumentDeclarationNode("x");

            var result = node.ChildrenReverse();

            Assert.That(result, Is.EqualTo(Enumerable.Empty<IExpressionNode>()));
        }
    }
}