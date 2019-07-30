using System;
using Cimpress.Cimbol.Compiler.SyntaxTree;
using NUnit.Framework;

namespace Cimpress.Cimbol.UnitTests.Compiler.SyntaxTree
{
    [TestFixture]
    public class ModuleDeclarationNodeTests
    {
        [Test]
        public void Should_SerializeToString_When_Valid()
        {
            var node = new ModuleDeclarationNode(
                "a",
                Array.Empty<ImportDeclarationNode>(),
                Array.Empty<ExportStatementNode>(),
                Array.Empty<FormulaDeclarationNode>());

            Assert.That(node.ToString(), Is.EqualTo("{ModuleDeclarationNode a}"));
        }

        [Test]
        public void Should_ReturnChildrenInOrder_When_IteratingChildren()
        {
            var child1 = new ImportDeclarationNode("a", new[] { "b" }, ImportType.Formula);
            var child2 = new ExportStatementNode("a");
            var child3 = new FormulaDeclarationNode("a", null);
            var node = new ModuleDeclarationNode("a", new[] { child1 }, new[] { child2 }, new[] { child3 });

            var expected = new ISyntaxNode[] { child1, child2, child3 };
            Assert.That(node.Children(), Is.EqualTo(expected));
        }

        [Test]
        public void Should_ReturnChildrenInOrder_When_IteratingChildrenReverse()
        {
            var child1 = new ImportDeclarationNode("a", new[] { "b" }, ImportType.Formula);
            var child2 = new ExportStatementNode("a");
            var child3 = new FormulaDeclarationNode("a", null);
            var node = new ModuleDeclarationNode("a", new[] { child1 }, new[] { child2 }, new[] { child3 });

            var expected = new ISyntaxNode[] { child3, child2, child1 };
            Assert.That(node.ChildrenReverse(), Is.EqualTo(expected));
        }
    }
}