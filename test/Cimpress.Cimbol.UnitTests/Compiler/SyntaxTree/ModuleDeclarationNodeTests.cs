using System;
using System.Linq;
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
                Array.Empty<FormulaDeclarationNode>());

            Assert.That(node.ToString(), Is.EqualTo("{ModuleDeclarationNode a}"));
        }

        [Test]
        public void Should_ReturnChildrenInOrder_When_IteratingChildren()
        {
            var child1 = new ImportDeclarationNode("a", new[] { "b" }, ImportType.Formula);
            var child2 = new FormulaDeclarationNode("a", null, true);
            var node = new ModuleDeclarationNode("a", new[] { child1 }, new[] { child2 });

            var expected = new ISyntaxNode[] { child1, child2 };
            Assert.That(node.Children(), Is.EqualTo(expected));
        }

        [Test]
        public void Should_ReturnChildrenInOrder_When_IteratingChildrenReverse()
        {
            var child1 = new ImportDeclarationNode("a", new[] { "b" }, ImportType.Formula);
            var child2 = new FormulaDeclarationNode("a", null, true);
            var node = new ModuleDeclarationNode("a", new[] { child1 }, new[] { child2 });

            var expected = new ISyntaxNode[] { child2, child1 };
            Assert.That(node.ChildrenReverse(), Is.EqualTo(expected));
        }

        [Test]
        public void Should_RetrieveFormulaDeclaration_When_FormulaDeclarationExists()
        {
            var formula = new FormulaDeclarationNode("x", new IdentifierNode("x"), true);
            var module = new ModuleDeclarationNode("y", Enumerable.Empty<ImportDeclarationNode>(), new[] { formula });

            var result = module.TryGetFormulaDeclaration("x", out var resultFormula);

            Assert.That(result, Is.True);
            Assert.That(resultFormula, Is.Not.Null);
            Assert.That(resultFormula.Name, Is.EqualTo("x"));
        }

        [Test]
        public void ShouldNot_RetrieveFormulaDeclaration_When_FormulaDeclarationDoesNotExist()
        {
            var formula = new FormulaDeclarationNode("x", new IdentifierNode("x"), true);
            var module = new ModuleDeclarationNode("y", Enumerable.Empty<ImportDeclarationNode>(), new[] { formula });

            var result = module.TryGetFormulaDeclaration("z", out _);

            Assert.That(result, Is.False);
        }

        [Test]
        public void Should_RetrieveImportDeclaration_When_ImportDeclarationExists()
        {
            var import = new ImportDeclarationNode("x", Enumerable.Empty<string>(), ImportType.Constant);
            var module = new ModuleDeclarationNode("y", new[] { import }, Enumerable.Empty<FormulaDeclarationNode>());

            var result = module.TryGetImportDeclaration("x", out var resultImport);

            Assert.That(result, Is.True);
            Assert.That(resultImport, Is.Not.Null);
            Assert.That(resultImport.Name, Is.EqualTo("x"));
        }

        [Test]
        public void ShouldNot_RetrieveImportDeclaration_When_ImportDeclarationDoesNotExist()
        {
            var import = new ImportDeclarationNode("x", Enumerable.Empty<string>(), ImportType.Constant);
            var module = new ModuleDeclarationNode("y", new[] { import }, Enumerable.Empty<FormulaDeclarationNode>());

            var result = module.TryGetImportDeclaration("z", out _);

            Assert.That(result, Is.False);
        }
    }
}