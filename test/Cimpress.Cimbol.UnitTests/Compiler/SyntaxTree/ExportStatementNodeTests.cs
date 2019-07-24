using System.Linq;
using Cimpress.Cimbol.Compiler.SyntaxTree;
using NUnit.Framework;

namespace Cimpress.Cimbol.UnitTests.Compiler.SyntaxTree
{
    [TestFixture]
    public class ExportStatementNodeTests
    {
        [Test]
        public void Should_SerializeToString_When_Valid()
        {
            var node = new ExportStatementNode("a");
            Assert.That(node.ToString(), Is.EqualTo("{ExportStatementNode a}"));
        }

        [Test]
        public void Should_ReturnEmptyEnumerable_When_IteratingChildren()
        {
            var node = new ExportStatementNode("a");
            Assert.That(node.Children(), Is.EqualTo(Enumerable.Empty<ISyntaxNode>()));
        }

        [Test]
        public void Should_ReturnEmptyEnumerable_When_IteratingChildrenReverse()
        {
            var node = new ExportStatementNode("a");
            Assert.That(node.ChildrenReverse(), Is.EqualTo(Enumerable.Empty<ISyntaxNode>()));
        }
    }
}