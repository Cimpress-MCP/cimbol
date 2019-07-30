using System;
using Cimpress.Cimbol.Utilities;
using NUnit.Framework;

namespace Cimpress.Cimbol.UnitTests.Main.Internal
{
    [TestFixture]
    public class GraphCycleTests
    {
        [Test]
        public void Should_FindNoCycles_When_GivenNullGraph()
        {
            var vertices = Array.Empty<int>();
            var edges = Array.Empty<Tuple<int, int>>();
            var graph = new Graph<int>(vertices, edges);

            var hasCycles = graph.IsCyclical();

            Assert.That(hasCycles, Is.False);
        }

        [Test]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(6)]
        public void Should_FindNoCycles_When_GivenDiamondGraph(int n)
        {
            var graph = GraphTestUtilities.BuildDiamondGraph(n);

            var hasCycles = graph.IsCyclical();

            Assert.That(hasCycles, Is.False);
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(6)]
        public void Should_FindNoCycles_When_GivenDisjointGraphs(int n)
        {
            var graph = GraphTestUtilities.BuildDisjointGraph(n);

            var hasCycles = graph.IsCyclical();

            Assert.That(hasCycles, Is.False);
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(6)]
        public void Should_FindNoCycles_When_GivenPathGraph(int n)
        {
            var graph = GraphTestUtilities.BuildPathGraph(n);

            var hasCycles = graph.IsCyclical();

            Assert.That(hasCycles, Is.False);
        }

        [Test]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(6)]
        public void Should_FindNoCycles_When_GivenTree(int n)
        {
            var graph = GraphTestUtilities.BuildTreeGraph(n);

            var hasCycles = graph.IsCyclical();

            Assert.That(hasCycles, Is.False);
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(6)]
        public void Should_FindCycles_When_GivenCompleteGraph(int n)
        {
            var graph = GraphTestUtilities.BuildCompleteGraph(n);

            var hasCycles = graph.IsCyclical();

            Assert.That(hasCycles, Is.True);
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(6)]
        public void Should_FindCycles_When_GivenCycleGraph(int n)
        {
            var graph = GraphTestUtilities.BuildCycleGraph(n);

            var hasCycles = graph.IsCyclical();

            Assert.That(hasCycles, Is.True);
        }
    }
}