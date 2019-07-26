using System;
using Cimpress.Cimbol.Compiler.SyntaxTree;
using NUnit.Framework;

namespace Cimpress.Cimbol.UnitTests.Compiler.SyntaxTree
{
    [TestFixture]
    public class MacroNodeTests
    {
        [Test]
        public void Should_SerializeToString_When_Valid()
        {
            var node = new MacroNode("if", Array.Empty<IArgument>());
            Assert.That(node.ToString(), Is.EqualTo("{MacroNode if}"));
        }

        [Test]
        public void Should_ReturnChildrenInOrder_When_IteratingChildren()
        {
            var child1 = new LiteralNode(null);
            var child2 = new LiteralNode(null);
            var node = new MacroNode("if", new[] { new PositionalArgument(child1), new PositionalArgument(child2) });

            var expected = new IExpressionNode[] { child1, child2 };
            Assert.That(node.Children(), Is.EqualTo(expected));
        }

        [Test]
        public void Should_ReturnChildrenInOrder_When_IteratingChildrenReverse()
        {
            var child1 = new LiteralNode(null);
            var child2 = new LiteralNode(null);
            var node = new MacroNode("if", new[] { new PositionalArgument(child1), new PositionalArgument(child2) });

            var expected = new IExpressionNode[] { child2, child1 };
            Assert.That(node.ChildrenReverse(), Is.EqualTo(expected));
        }
    }
}