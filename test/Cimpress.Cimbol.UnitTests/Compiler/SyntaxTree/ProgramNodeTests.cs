using System.Linq;
using Cimpress.Cimbol.Compiler.SyntaxTree;
using Cimpress.Cimbol.Runtime.Types;
using NUnit.Framework;

namespace Cimpress.Cimbol.UnitTests.Compiler.SyntaxTree
{
    [TestFixture]
    public class ProgramNodeTests
    {
        [Test]
        public void Should_SerializeToString_When_Valid()
        {
            var node = new ProgramNode(
                Enumerable.Empty<ArgumentDeclarationNode>(),
                Enumerable.Empty<ConstantDeclarationNode>(),
                Enumerable.Empty<ModuleDeclarationNode>());

            var result = node.ToString();

            Assert.That(result, Is.EqualTo("{ProgramNode}"));
        }

        [Test]
        public void Should_ReturnChildrenInOrder_When_IteratingChildren()
        {
            var child1 = new ArgumentDeclarationNode("a");
            var child2 = new ConstantDeclarationNode("b", BooleanValue.True);
            var child3 = new ModuleDeclarationNode(
                "c",
                Enumerable.Empty<ImportStatementNode>(),
                Enumerable.Empty<ExportStatementNode>(),
                Enumerable.Empty<FormulaDeclarationNode>());
            var node = new ProgramNode(new[] { child1 }, new[] { child2 }, new[] { child3 });

            var result = node.Children();

            var expected = new ISyntaxNode[] { child1, child2, child3 };
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void Should_ReturnChildrenInOrder_When_IteratingChildrenReverse()
        {
            var child1 = new ArgumentDeclarationNode("a");
            var child2 = new ConstantDeclarationNode("b", BooleanValue.True);
            var child3 = new ModuleDeclarationNode(
                "c",
                Enumerable.Empty<ImportStatementNode>(),
                Enumerable.Empty<ExportStatementNode>(),
                Enumerable.Empty<FormulaDeclarationNode>());
            var node = new ProgramNode(new[] { child1 }, new[] { child2 }, new[] { child3 });

            var result = node.ChildrenReverse();

            var expected = new ISyntaxNode[] { child3, child2, child1 };
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}