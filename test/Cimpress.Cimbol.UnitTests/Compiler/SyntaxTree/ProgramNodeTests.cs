﻿// Copyright 2021 Cimpress plc.
// Licensed under the Apache License, Version 2.0
// http://www.apache.org/licenses/LICENSE-2.0

using System.Linq;
using Cimpress.Cimbol.Compiler.SyntaxTree;
using Cimpress.Cimbol.Runtime.Types;
using NUnit.Framework;

namespace Cimpress.Cimbol.UnitTests.Compiler.SyntaxTree
{
    [TestFixture]
    public class ProgramNodeTests
    {
        [Test]
        public void Should_SerializeToString_When_Valid()
        {
            var node = new ProgramNode(
                Enumerable.Empty<ArgumentNode>(),
                Enumerable.Empty<ConstantNode>(),
                Enumerable.Empty<ModuleNode>());

            var result = node.ToString();

            Assert.That(result, Is.EqualTo("{ProgramNode}"));
        }

        [Test]
        public void Should_ReturnChildrenInOrder_When_IteratingChildren()
        {
            var child1 = new ArgumentNode("a");
            var child2 = new ConstantNode("b", BooleanValue.True);
            var child3 = new ModuleNode(
                "c",
                Enumerable.Empty<ImportNode>(),
                Enumerable.Empty<FormulaNode>());
            var node = new ProgramNode(new[] { child1 }, new[] { child2 }, new[] { child3 });

            var result = node.Children();

            var expected = new ISyntaxNode[] { child1, child2, child3 };
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void Should_ReturnChildrenInOrder_When_IteratingChildrenReverse()
        {
            var child1 = new ArgumentNode("a");
            var child2 = new ConstantNode("b", BooleanValue.True);
            var child3 = new ModuleNode(
                "c",
                Enumerable.Empty<ImportNode>(),
                Enumerable.Empty<FormulaNode>());
            var node = new ProgramNode(new[] { child1 }, new[] { child2 }, new[] { child3 });

            var result = node.ChildrenReverse();

            var expected = new ISyntaxNode[] { child3, child2, child1 };
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}