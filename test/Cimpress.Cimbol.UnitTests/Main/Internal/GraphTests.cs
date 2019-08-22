using System;
using Cimpress.Cimbol.Utilities;
using NUnit.Framework;

namespace Cimpress.Cimbol.UnitTests.Main.Internal
{
    [TestFixture]
    public class GraphTests
    {
        [Test]
        public void Should_ThrowException_When_ConstructedWithEdgesToVerticesNotInTheGraph()
        {
            var vertices = new[] { 0 };
            var edges = new[] { Tuple.Create(0, 1) };

            Assert.Throws<ArgumentException>(() =>
            {
                var graph = new Graph<int>(vertices, edges);
            });
        }

        [Test]
        public void Should_GetEmptyArrayOfVertices_When_VerticesDoNotExist()
        {
            var graph = new Graph<string>(
                Array.Empty<string>(),
                Array.Empty<Tuple<string, string>>());

            var vertices = graph.Vertices;

            Assert.That(vertices, Has.Count.EqualTo(0));
        }

        [Test]
        public void Should_GetVertices_When_VerticesExist()
        {
            var graph = new Graph<string>(
                new[] { "x", "y" },
                new[] { Tuple.Create("x", "y") });

            var vertices = graph.Vertices;

            Assert.That(vertices, Has.Count.EqualTo(2));
            Assert.That(vertices, Has.Member("x"));
            Assert.That(vertices, Has.Member("y"));
        }

        [Test]
        public void Should_EmptyArrayOfGetEdges_When_EdgesDoNotExist()
        {
            var graph = new Graph<string>(
                Array.Empty<string>(),
                Array.Empty<Tuple<string, string>>());

            var edges = graph.Edges;

            Assert.That(edges, Has.Length.EqualTo(0));
        }

        [Test]
        public void Should_GetEdges_When_EdgesExist()
        {
            var graph = new Graph<string>(
                new[] { "x", "y" },
                new[] { Tuple.Create("x", "y") });

            var edges = graph.Edges;

            Assert.That(edges, Has.Length.EqualTo(1));
            Assert.That(edges, Has.Member(Tuple.Create("x", "y")));
        }

        [Test]
        public void Should_GetAdjacentVerticesIn_When_VerticesExist()
        {
            var graph = new Graph<string>(
                new[] { "x", "y" },
                new[] { Tuple.Create("x", "y") });

            var adjacentsIn = graph.AdjacentsIn("y");

            Assert.That(adjacentsIn, Has.Count.EqualTo(1));
            Assert.That(adjacentsIn, Has.Member("x"));
        }

        [Test]
        public void Should_GetAdjacentVerticesIn_When_VerticesDoNotExist()
        {
            var graph = new Graph<string>(
                new[] { "x", "y" },
                new[] { Tuple.Create("x", "y") });

            var adjacentsIn = graph.AdjacentsIn("z");
            Assert.That(adjacentsIn, Is.Empty);
        }

        [Test]
        public void Should_GetAdjacentVerticesOut_When_VerticesExist()
        {
            var graph = new Graph<string>(
                new[] { "x", "y" },
                new[] { Tuple.Create("x", "y") });

            var adjacentsOut = graph.AdjacentsOut("x");

            Assert.That(adjacentsOut, Has.Count.EqualTo(1));
            Assert.That(adjacentsOut, Has.Member("y"));
        }

        [Test]
        public void Should_GetAdjacentVerticesOut_When_VerticesDoNotExist()
        {
            var graph = new Graph<string>(
                new[] { "x", "y" },
                new[] { Tuple.Create("x", "y") });

            var adjacentsOut = graph.AdjacentsOut("z");

            Assert.That(adjacentsOut, Is.Empty);
        }

        [Test]
        public void Should_UseEqualityComparer_When_LookingUpNodes()
        {
            var graph = new Graph<string>(
                new[] { "x", "y" },
                new[] { Tuple.Create("x", "y") },
                StringComparer.OrdinalIgnoreCase);

            var adjacentsOut = graph.AdjacentsOut("X");

            Assert.That(adjacentsOut, Is.Not.Null);
        }

        [Test]
        public void Should_GetAdjacentsInAsAdjacentsOut_When_Transposed()
        {
            var graph = new Graph<string>(
                new[] { "x", "y" },
                new[] { Tuple.Create("x", "y") });
            var transposed = graph.Transpose();

            var adjacentsOut = transposed.AdjacentsOut("y");

            Assert.That(adjacentsOut, Has.Count.EqualTo(1));
            Assert.That(adjacentsOut, Has.Member("x"));
        }

        [Test]
        public void Should_GetAdjacentsOutAsAdjacentsIn_When_Transposed()
        {
            var graph = new Graph<string>(
                new[] { "x", "y" },
                new[] { Tuple.Create("x", "y") });
            var transposed = graph.Transpose();

            var adjacentsIn = transposed.AdjacentsIn("x");

            Assert.That(adjacentsIn, Has.Count.EqualTo(1));
            Assert.That(adjacentsIn, Has.Member("y"));
        }

        [Test]
        public void Should_GetSameEdgesOut_When_Transposed()
        {
            var graph = new Graph<string>(
                new[] { "x", "y" },
                new[] { Tuple.Create("x", "y") });
            var transposed = graph.Transpose();

            var edges = transposed.Edges;

            Assert.That(edges, Is.EquivalentTo(graph.Edges));
        }

        [Test]
        public void Should_GetSameVerticesOut_When_Transposed()
        {
            var graph = new Graph<string>(
                new[] { "x", "y" },
                new[] { Tuple.Create("x", "y") });
            var transposed = graph.Transpose();

            var vertices = transposed.Vertices;

            Assert.That(vertices, Is.EquivalentTo(graph.Vertices));
        }
    }
}