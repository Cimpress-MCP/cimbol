using System;
using System.Linq;
using NUnit.Framework;

namespace Cimpress.Cimbol.UnitTests.Main.Internal
{
    [TestFixture]
    public class GraphMinimalPartialOrderingTests
    {
        [Test]
        public void Should_PartiallyOrder_When_GivenEmptyGraph()
        {
            var graph = GraphTestUtilities.BuildEmptyGraph();

            var partialOrder = graph.MinimalPartialOrder();

            Assert.That(partialOrder, Has.Length.EqualTo(0));
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(6)]
        public void Should_PartiallyOrder_When_GivenDiamondGraph(int n)
        {
            var height = (2 * n) - 1;
            var graph = GraphTestUtilities.BuildDiamondGraph(n);

            var partialOrder = graph.MinimalPartialOrder();

            Assert.That(partialOrder, Has.Length.EqualTo(height));
            for (var i = 0; i < height; ++i)
            {
                var row = partialOrder.ElementAtOrDefault(i);
                var count = Math.Min(i, height - i - 1) + 1;
                Assert.That(row, Has.Count.EqualTo(count));
            }
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(6)]
        public void Should_PartiallyOrder_When_GivenDisjointGraph(int n)
        {
            var graph = GraphTestUtilities.BuildDisjointGraph(n);

            var partialOrder = graph.MinimalPartialOrder();

            Assert.That(partialOrder, Has.Length.EqualTo(1));
            Assert.That(partialOrder.ElementAtOrDefault(0), Is.EquivalentTo(Enumerable.Range(0, n)));
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(6)]
        public void Should_PartiallyOrder_When_GivenPathGraph(int n)
        {
            var graph = GraphTestUtilities.BuildPathGraph(n);

            var partialOrder = graph.MinimalPartialOrder();

            Assert.That(partialOrder, Has.Length.EqualTo(n));
            for (var i = 0; i < n; ++i)
            {
                var row = partialOrder.ElementAtOrDefault(i);
                Assert.That(row, Is.EquivalentTo(new[] { i }));
            }
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(6)]
        public void Should_PartiallyOrder_When_GivenTreeGraph(int n)
        {
            var graph = GraphTestUtilities.BuildTreeGraph(n);

            var partialOrder = graph.MinimalPartialOrder();

            Assert.That(partialOrder, Has.Length.EqualTo(n));
            for (var i = 0; i < n; ++i)
            {
                var powerStart = (int)Math.Pow(2, i) - 1;
                var powerEnd = (int)Math.Pow(2, i + 1) - 1;
                var row = partialOrder.ElementAtOrDefault(i);
                var expected = Enumerable.Range(powerStart, powerEnd - powerStart);
                Assert.That(row, Is.EquivalentTo(expected));
            }
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(6)]
        public void ShouldNot_PartiallyOrder_When_GivenCompleteGraph(int n)
        {
            var graph = GraphTestUtilities.BuildCompleteGraph(n);

            Assert.Throws<NotSupportedException>(() => graph.MinimalPartialOrder());
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(6)]
        public void ShouldNot_PartiallyOrder_When_GivenCycleGraph(int n)
        {
            var graph = GraphTestUtilities.BuildCycleGraph(n);

            Assert.Throws<NotSupportedException>(() => graph.MinimalPartialOrder());
        }
    }
}