// Copyright 2021 Cimpress plc.
// Licensed under the Apache License, Version 2.0
// http://www.apache.org/licenses/LICENSE-2.0

using Cimpress.Cimbol.Compiler.SyntaxTree;
using NUnit.Framework;

namespace Cimpress.Cimbol.UnitTests.Compiler.SyntaxTree
{
    [TestFixture]
    public class UnaryOpNodeTests
    {
        [Test]
        public void Should_SerializeToString_When_Valid()
        {
            var child1 = new LiteralNode(null);
            var node = new UnaryOpNode(UnaryOpType.Negate, child1);
            Assert.AreEqual("{UnaryOpNode -}", node.ToString());
        }

        [Test]
        public void Should_ReturnChildrenInOrder_When_IteratingChildren()
        {
            var child1 = new LiteralNode(null);
            var node = new UnaryOpNode(UnaryOpType.Negate, child1);

            var expected = new[] { child1 };
            CollectionAssert.AreEqual(expected, node.Children());
        }

        [Test]
        public void Should_ReturnChildrenInOrder_When_IteratingChildrenReverse()
        {
            var child1 = new LiteralNode(null);
            var node = new UnaryOpNode(UnaryOpType.Negate, child1);

            var expected = new[] { child1 };
            CollectionAssert.AreEqual(expected, node.ChildrenReverse());
        }

        [Test]
        public void Should_BeAsync_When_ChildIsAsync()
        {
            var child1 = new UnaryOpNode(UnaryOpType.Await, new LiteralNode(null));
            var node = new UnaryOpNode(UnaryOpType.Negate, child1);

            var result = node.IsAsynchronous;

            Assert.That(result, Is.True);
        }

        [Test]
        public void Should_BeAsync_When_OperationTypeIsAwait()
        {
            var child1 = new LiteralNode(null);
            var node = new UnaryOpNode(UnaryOpType.Await, child1);

            var result = node.IsAsynchronous;

            Assert.That(result, Is.True);
        }

        [Test]
        public void ShouldNot_BeAsync_WhenOperationTypeIsNotAwaitAndChildIsNotAsync()
        {
            var child1 = new LiteralNode(null);
            var node = new UnaryOpNode(UnaryOpType.Negate, child1);

            var result = node.IsAsynchronous;

            Assert.That(result, Is.False);
        }
    }
}