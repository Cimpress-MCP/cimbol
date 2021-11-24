// Copyright 2021 Cimpress plc.
// Licensed under the Apache License, Version 2.0
// http://www.apache.org/licenses/LICENSE-2.0

using System.Linq;
using Cimpress.Cimbol.Compiler.SyntaxTree;
using NUnit.Framework;

namespace Cimpress.Cimbol.UnitTests.Compiler.SyntaxTree
{
    [TestFixture]
    public class ImportNodeTests
    {
        [Test]
        public void Should_SerializeToString_When_Valid()
        {
            var node = new ImportNode("a", new[] { "x", "y", "z" }, ImportType.Formula, false);
            Assert.That(node.ToString(), Is.EqualTo("{ImportNode Formula a}"));
        }

        [Test]
        public void Should_ConvertPathToImmutableArray_When_GivenPath()
        {
            var node = new ImportNode("a", new[] { "x", "y", "z" }, ImportType.Formula, false);
            Assert.That(node.ImportPath, Is.EqualTo(new[] { "x", "y", "z" }));
        }

        [Test]
        public void Should_ReturnEmptyEnumerable_When_IteratingChildren()
        {
            var node = new ImportNode("a", new[] { "x", "y", "z" }, ImportType.Formula, false);
            Assert.That(node.Children(), Is.EqualTo(Enumerable.Empty<ISyntaxNode>()));
        }

        [Test]
        public void Should_ReturnEmptyEnumerable_When_IteratingChildrenReverse()
        {
            var node = new ImportNode("a", new[] { "x", "y", "z" }, ImportType.Formula, false);
            Assert.That(node.ChildrenReverse(), Is.EqualTo(Enumerable.Empty<ISyntaxNode>()));
        }

        [Test]
        [TestCase(ImportType.Argument)]
        [TestCase(ImportType.Constant)]
        [TestCase(ImportType.Formula)]
        [TestCase(ImportType.Module)]
        public void Should_ReturnCorrectImportType(ImportType expected)
        {
            var node = new ImportNode("a", new[] { "x", "y", "z" }, expected, false);

            var result = node.ImportType;

            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void Should_ReturnCorrectExportedStatus(bool expected)
        {
            var node = new ImportNode("a", new[] { "x", "y", "z" }, ImportType.Formula, expected);

            var result = node.IsExported;

            Assert.That(result, Is.EqualTo(expected));
        }
    }
}