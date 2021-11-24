// Copyright 2021 Cimpress plc.
// Licensed under the Apache License, Version 2.0
// http://www.apache.org/licenses/LICENSE-2.0

using System.Linq;
using Cimpress.Cimbol.Compiler.SyntaxTree;
using NUnit.Framework;

namespace Cimpress.Cimbol.UnitTests.Compiler.SyntaxTree
{
    [TestFixture]
    public class LiteralNodeTests
    {
        [Test]
        public void Should_SerializeToString_When_Valid()
        {
            var node = new LiteralNode(null);
            Assert.AreEqual("{LiteralNode}", node.ToString());
        }

        [Test]
        public void Should_ReturnEmptyEnumerable_When_IteratingChildren()
        {
            var node = new LiteralNode(null);
            CollectionAssert.AreEqual(Enumerable.Empty<IExpressionNode>(), node.Children());
        }

        [Test]
        public void Should_ReturnEmptyEnumerable_When_IteratingChildrenReverse()
        {
            var node = new LiteralNode(null);
            CollectionAssert.AreEqual(Enumerable.Empty<IExpressionNode>(), node.ChildrenReverse());
        }

        [Test]
        public void ShouldNot_BeAsync_When_Initialized()
        {
            var node = new LiteralNode(null);

            var result = node.IsAsynchronous;

            Assert.That(result, Is.False);
        }
    }
}