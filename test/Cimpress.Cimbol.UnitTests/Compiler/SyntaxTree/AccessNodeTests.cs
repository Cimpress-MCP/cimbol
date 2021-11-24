// Copyright 2021 Cimpress plc.
// Licensed under the Apache License, Version 2.0
// http://www.apache.org/licenses/LICENSE-2.0

using Cimpress.Cimbol.Compiler.SyntaxTree;
using NUnit.Framework;

namespace Cimpress.Cimbol.UnitTests.Compiler.SyntaxTree
{
    [TestFixture]
    public class AccessNodeTests
    {
        [Test]
        public void Should_SerializeToString_When_Valid()
        {
            var child1 = new LiteralNode(null);
            var node = new AccessNode(child1, "x");
            Assert.That(node.ToString(), Is.EqualTo("{AccessNode x}"));
        }

        [Test]
        public void Should_ReturnChildrenInOrder_When_IteratingChildren()
        {
            var child1 = new LiteralNode(null);
            var node = new AccessNode(child1, "x");

            var expected = new IExpressionNode[] { child1 };
            Assert.That(node.Children(), Is.EqualTo(expected));
        }

        [Test]
        public void Should_ReturnChildrenInOrder_When_IteratingChildrenReverse()
        {
            var child1 = new LiteralNode(null);
            var node = new AccessNode(child1, "x");

            var expected = new IExpressionNode[] { child1 };
            Assert.That(node.ChildrenReverse(), Is.EqualTo(expected));
        }

        [Test]
        public void Should_BeAsync_When_GivenAsyncChild()
        {
            var child1 = new UnaryOpNode(UnaryOpType.Await, new LiteralNode(null));
            var node = new AccessNode(child1, "x");

            var result = node.IsAsynchronous;

            Assert.That(result, Is.True);
        }

        [Test]
        public void ShouldNot_BeAsync_When_GivenNonAsyncChild()
        {
            var child1 = new UnaryOpNode(UnaryOpType.Negate, new LiteralNode(null));
            var node = new AccessNode(child1, "x");

            var result = node.IsAsynchronous;

            Assert.That(result, Is.False);
        }
    }
}