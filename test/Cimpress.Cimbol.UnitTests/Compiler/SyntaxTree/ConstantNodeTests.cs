// Copyright 2021 Cimpress plc.
// Licensed under the Apache License, Version 2.0
// http://www.apache.org/licenses/LICENSE-2.0

using System.Linq;
using Cimpress.Cimbol.Compiler.SyntaxTree;
using Cimpress.Cimbol.Runtime.Types;
using NUnit.Framework;

namespace Cimpress.Cimbol.UnitTests.Compiler.SyntaxTree
{
    [TestFixture]
    public class ConstantNodeTests
    {
        [Test]
        public void Should_SerializeToString_When_Valid()
        {
            var node = new ConstantNode("x", BooleanValue.True);

            var result = node.ToString();

            Assert.That(result, Is.EqualTo("{ConstantNode x}"));
        }

        [Test]
        public void Should_ReturnEmptyEnumerable_When_IteratingChildren()
        {
            var node = new ConstantNode("x", BooleanValue.True);

            var result = node.Children();

            Assert.That(result, Is.EqualTo(Enumerable.Empty<IExpressionNode>()));
        }

        [Test]
        public void Should_ReturnEmptyEnumerable_When_IteratingChildrenReverse()
        {
            var node = new ConstantNode("x", BooleanValue.True);

            var result = node.ChildrenReverse();

            Assert.That(result, Is.EqualTo(Enumerable.Empty<IExpressionNode>()));
        }
    }
}