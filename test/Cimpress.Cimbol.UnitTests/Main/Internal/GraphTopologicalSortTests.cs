// Copyright 2021 Cimpress plc.
// Licensed under the Apache License, Version 2.0
// http://www.apache.org/licenses/LICENSE-2.0

using System;
using System.Linq;
using NUnit.Framework;

namespace Cimpress.Cimbol.UnitTests.Main.Internal
{
    [TestFixture]
    public class GraphTopologicalSortTests
    {
        [Test]
        public void Should_TopologicallySort_When_GivenEmptyGraph()
        {
            var graph = GraphTestUtilities.BuildEmptyGraph();

            var sort = graph.TopologicalSort();

            Assert.That(sort, Is.EqualTo(Enumerable.Empty<int>()));
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(6)]
        public void Should_TopologicallySort_When_GivenDisjointGraph(int n)
        {
            var graph = GraphTestUtilities.BuildDisjointGraph(n);

            var sort = graph.TopologicalSort();

            Assert.That(sort, Is.EquivalentTo(Enumerable.Range(0, n)));
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(6)]
        public void Should_TopologicallySort_When_GivenPathGraph(int n)
        {
            var graph = GraphTestUtilities.BuildPathGraph(n);

            var sort = graph.TopologicalSort();

            Assert.That(sort, Is.EqualTo(Enumerable.Range(0, n)));
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(6)]
        public void Should_TopologicallySort_When_GivenTreeGraph(int n)
        {
            var graph = GraphTestUtilities.BuildTreeGraph(n);

            var sort = graph.TopologicalSort();

            for (var i = 0; i < n; ++i)
            {
                var powerStart = (int)Math.Pow(2, i) - 1;
                var powerEnd = (int)Math.Pow(2, i + 1) - 1;
                var slice = sort.Skip(powerStart).Take(powerEnd - powerStart);
                var expected = Enumerable.Range(powerStart, powerEnd - powerStart);
                Assert.That(slice, Is.EquivalentTo(expected));
            }
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(6)]
        public void ShouldNot_TopologicallySort_When_GivenCompleteGraph(int n)
        {
            var graph = GraphTestUtilities.BuildCompleteGraph(n);

            var result = graph.TopologicalSort();

            Assert.That(result, Has.Count.EqualTo(0));
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(6)]
        public void ShouldNot_TopologicallySort_When_GivenCycleGraph(int n)
        {
            var graph = GraphTestUtilities.BuildCycleGraph(n);

            var result = graph.TopologicalSort();

            Assert.That(result, Has.Count.EqualTo(0));
        }
    }
}