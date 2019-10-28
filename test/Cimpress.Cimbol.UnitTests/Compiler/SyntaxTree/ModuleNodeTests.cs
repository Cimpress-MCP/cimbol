using System;
using System.Linq;
using Cimpress.Cimbol.Compiler.SyntaxTree;
using NUnit.Framework;

namespace Cimpress.Cimbol.UnitTests.Compiler.SyntaxTree
{
    [TestFixture]
    public class ModuleNodeTests
    {
        [Test]
        public void Should_SerializeToString_When_Valid()
        {
            var node = new ModuleNode(
                "a",
                Array.Empty<ImportNode>(),
                Array.Empty<FormulaNode>());

            Assert.That(node.ToString(), Is.EqualTo("{ModuleNode a}"));
        }

        [Test]
        public void Should_ReturnChildrenInOrder_When_IteratingChildren()
        {
            var child1 = new ImportNode("a", new[] { "b" }, ImportType.Formula, false);
            var child2 = new FormulaNode("a", new LiteralNode(null), true);
            var node = new ModuleNode("a", new[] { child1 }, new[] { child2 });

            var expected = new ISyntaxNode[] { child1, child2 };
            Assert.That(node.Children(), Is.EqualTo(expected));
        }

        [Test]
        public void Should_ReturnChildrenInOrder_When_IteratingChildrenReverse()
        {
            var child1 = new ImportNode("a", new[] { "b" }, ImportType.Formula, false);
            var child2 = new FormulaNode("a", new LiteralNode(null), true);
            var node = new ModuleNode("a", new[] { child1 }, new[] { child2 });

            var expected = new ISyntaxNode[] { child2, child1 };
            Assert.That(node.ChildrenReverse(), Is.EqualTo(expected));
        }

        [Test]
        public void Should_RetrieveFormulaDeclaration_When_FormulaDeclarationExists()
        {
            var formula = new FormulaNode("x", new IdentifierNode("x"), true);
            var module = new ModuleNode("y", Enumerable.Empty<ImportNode>(), new[] { formula });

            var result = module.TryGetFormula("x", out var resultFormula);

            Assert.That(result, Is.True);
            Assert.That(resultFormula, Is.Not.Null);
            Assert.That(resultFormula.Name, Is.EqualTo("x"));
        }

        [Test]
        public void ShouldNot_RetrieveFormulaDeclaration_When_FormulaDeclarationDoesNotExist()
        {
            var formula = new FormulaNode("x", new IdentifierNode("x"), true);
            var module = new ModuleNode("y", Enumerable.Empty<ImportNode>(), new[] { formula });

            var result = module.TryGetFormula("z", out _);

            Assert.That(result, Is.False);
        }

        [Test]
        public void Should_RetrieveImportDeclaration_When_ImportDeclarationExists()
        {
            var import = new ImportNode("x", Enumerable.Empty<string>(), ImportType.Constant, false);
            var module = new ModuleNode("y", new[] { import }, Enumerable.Empty<FormulaNode>());

            var result = module.TryGetImport("x", out var resultImport);

            Assert.That(result, Is.True);
            Assert.That(resultImport, Is.Not.Null);
            Assert.That(resultImport.Name, Is.EqualTo("x"));
        }

        [Test]
        public void ShouldNot_RetrieveImportDeclaration_When_ImportDeclarationDoesNotExist()
        {
            var import = new ImportNode("x", Enumerable.Empty<string>(), ImportType.Constant, false);
            var module = new ModuleNode("y", new[] { import }, Enumerable.Empty<FormulaNode>());

            var result = module.TryGetImport("z", out _);

            Assert.That(result, Is.False);
        }
    }
}