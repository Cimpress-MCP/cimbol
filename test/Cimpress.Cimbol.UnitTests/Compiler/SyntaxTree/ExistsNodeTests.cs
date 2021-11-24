// Copyright 2021 Cimpress plc.
// Licensed under the Apache License, Version 2.0
// http://www.apache.org/licenses/LICENSE-2.0

using Cimpress.Cimbol.Compiler.SyntaxTree;
using NUnit.Framework;

namespace Cimpress.Cimbol.UnitTests.Compiler.SyntaxTree
{
    [TestFixture]
    public class ExistsNodeTests
    {
        [Test]
        public void Should_SerializeToString_When_Valid()
        {
            var node = new ExistsNode(new[] { "x", "y", "z" });
            var result = node.ToString();

            Assert.That(result, Is.EqualTo("{ExistsNode}"));
        }

        [Test]
        public void Should_ReturnEmptyCollection_When_IteratingChildren()
        {
            var node = new ExistsNode(new[] { "x", "y", "z" });

            var result = node.Children();

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void Should_ReturnChildrenInOrder_When_IteratingChildrenReverse()
        {
            var node = new ExistsNode(new[] { "x", "y", "z" });

            var result = node.ChildrenReverse();

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void ShouldNot_BeAsync()
        {
            var node = new ExistsNode(new[] { "x", "y", "z" });

            var result = node.IsAsynchronous;

            Assert.That(result, Is.False);
        }
    }
}
