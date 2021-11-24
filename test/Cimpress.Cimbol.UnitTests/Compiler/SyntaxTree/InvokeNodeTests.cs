// Copyright 2021 Cimpress plc.
// Licensed under the Apache License, Version 2.0
// http://www.apache.org/licenses/LICENSE-2.0

using System.Linq;
using Cimpress.Cimbol.Compiler.SyntaxTree;
using NUnit.Framework;

namespace Cimpress.Cimbol.UnitTests.Compiler.SyntaxTree
{
    [TestFixture]
    public class InvokeNodeTests
    {
        [Test]
        public void Should_SerializeToString_When_Valid()
        {
            var child1 = new LiteralNode(null);
            var node = new InvokeNode(child1, Enumerable.Empty<PositionalArgument>());
            Assert.AreEqual("{InvokeNode}", node.ToString());
        }

        [Test]
        public void Should_ReturnChildrenInOrder_When_IteratingChildren()
        {
            var child1 = new IdentifierNode("x");
            var child2 = new LiteralNode(null);
            var child3 = new LiteralNode(null);
            var node = new InvokeNode(child1, new[] { new PositionalArgument(child2), new PositionalArgument(child3) });

            var expected = new IExpressionNode[] { child1, child2, child3 };
            CollectionAssert.AreEqual(expected, node.Children());
        }

        [Test]
        public void Should_ReturnChildrenInOrder_When_IteratingChildrenReverse()
        {
            var child1 = new IdentifierNode("x");
            var child2 = new LiteralNode(null);
            var child3 = new LiteralNode(null);
            var node = new InvokeNode(child1, new[] { new PositionalArgument(child2), new PositionalArgument(child3) });

            var expected = new IExpressionNode[] { child3, child2, child1 };
            CollectionAssert.AreEqual(expected, node.ChildrenReverse());
        }

        [Test]
        public void Should_BeAsync_When_FunctionIsAsync()
        {
            var child1 = new UnaryOpNode(UnaryOpType.Await, new IdentifierNode("x"));
            var child2 = new LiteralNode(null);
            var child3 = new LiteralNode(null);
            var node = new InvokeNode(child1, new[] { new PositionalArgument(child2), new PositionalArgument(child3) });

            var result = node.IsAsynchronous;

            Assert.That(result, Is.True);
        }

        [Test]
        public void Should_BeAsync_When_AnyArgumentIsAsync()
        {
            var child1 = new IdentifierNode("x");
            var child2 = new UnaryOpNode(UnaryOpType.Await, new LiteralNode(null));
            var child3 = new LiteralNode(null);
            var node = new InvokeNode(child1, new[] { new PositionalArgument(child2), new PositionalArgument(child3) });

            var result = node.IsAsynchronous;

            Assert.That(result, Is.True);
        }

        [Test]
        public void Should_BeAsync_When_AllArgumentsAreASync()
        {
            var child1 = new IdentifierNode("x");
            var child2 = new UnaryOpNode(UnaryOpType.Await, new LiteralNode(null));
            var child3 = new UnaryOpNode(UnaryOpType.Await, new LiteralNode(null));
            var node = new InvokeNode(child1, new[] { new PositionalArgument(child2), new PositionalArgument(child3) });

            var result = node.IsAsynchronous;

            Assert.That(result, Is.True);
        }

        [Test]
        public void ShouldNot_BeAsync_When_FunctionAndArgumentsAreNotAsync()
        {
            var child1 = new IdentifierNode("x");
            var child2 = new LiteralNode(null);
            var child3 = new LiteralNode(null);
            var node = new InvokeNode(child1, new[] { new PositionalArgument(child2), new PositionalArgument(child3) });

            var result = node.IsAsynchronous;

            Assert.That(result, Is.False);
        }
    }
}