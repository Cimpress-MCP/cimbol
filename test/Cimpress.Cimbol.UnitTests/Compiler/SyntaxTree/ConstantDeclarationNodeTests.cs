using System.Linq;
using Cimpress.Cimbol.Compiler.SyntaxTree;
using Cimpress.Cimbol.Runtime.Types;
using NUnit.Framework;

namespace Cimpress.Cimbol.UnitTests.Compiler.SyntaxTree
{
    [TestFixture]
    public class ConstantDeclarationNodeTests
    {
        [Test]
        public void Should_SerializeToString_When_Valid()
        {
            var node = new ConstantDeclarationNode("x", BooleanValue.True);

            var result = node.ToString();

            Assert.That(result, Is.EqualTo("{ConstantDeclarationNode x}"));
        }

        [Test]
        public void Should_ReturnEmptyEnumerable_When_IteratingChildren()
        {
            var node = new ConstantDeclarationNode("x", BooleanValue.True);

            var result = node.Children();

            Assert.That(result, Is.EqualTo(Enumerable.Empty<IExpressionNode>()));
        }

        [Test]
        public void Should_ReturnEmptyEnumerable_When_IteratingChildrenReverse()
        {
            var node = new ConstantDeclarationNode("x", BooleanValue.True);

            var result = node.ChildrenReverse();

            Assert.That(result, Is.EqualTo(Enumerable.Empty<IExpressionNode>()));
        }
    }
}