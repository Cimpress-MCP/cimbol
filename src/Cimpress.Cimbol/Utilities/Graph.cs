// Copyright 2021 Cimpress plc.
// Licensed under the Apache License, Version 2.0
// http://www.apache.org/licenses/LICENSE-2.0

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Cimpress.Cimbol.Utilities
{
    /// <summary>
    /// A directed graph.
    /// </summary>
    /// <typeparam name="T">The type of node in the graph.</typeparam>
    public class Graph<T>
    {
        private readonly IEqualityComparer<T> _comparer;

        private readonly ImmutableArray<Tuple<T, T>> _edges;

        private readonly ImmutableDictionary<T, ImmutableHashSet<T>> _edgesIn;

        private readonly ImmutableDictionary<T, ImmutableHashSet<T>> _edgesOut;

        private readonly ImmutableHashSet<T> _vertices;

        /// <summary>
        /// Initializes a new instance of the <see cref="Graph{T}"/> class from an adjacency list.
        /// </summary>
        /// <param name="vertices">The vertices in the graph.</param>
        /// <param name="edges">The edges in the graph.</param>
        public Graph(IEnumerable<T> vertices, IEnumerable<Tuple<T, T>> edges)
            : this(vertices, edges, EqualityComparer<T>.Default)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Graph{T}"/> class from an adjacency list.
        /// </summary>
        /// <param name="vertices">The vertices in the graph.</param>
        /// <param name="edges">The edges in the graph.</param>
        /// <param name="comparer">How to compare the vertices in the graph.</param>
        public Graph(IEnumerable<T> vertices, IEnumerable<Tuple<T, T>> edges, IEqualityComparer<T> comparer)
        {
            _comparer = comparer;

            _vertices = vertices.ToImmutableHashSet(_comparer);

            _edges = edges.ToImmutableArray();

            foreach (var edge in _edges)
            {
                if (!_vertices.Contains(edge.Item1) || !_vertices.Contains(edge.Item2))
                {
                    // All edges must reference vertices in the vertex set.
                    throw new ArgumentException(
                        "One or more edges connect vertices not in the list vertices.",
                        nameof(edges));
                }
            }

            var edgesInIndex = _edges
                .GroupBy(edge => edge.Item2)
                .ToImmutableDictionary(
                    edgeGroup => edgeGroup.Key,
                    edgeGroup => edgeGroup.Select(edge => edge.Item1).ToImmutableHashSet(_comparer),
                    comparer);

            var edgesOutIndex = _edges
                .GroupBy(edge => edge.Item1)
                .ToImmutableDictionary(
                    edgeGroup => edgeGroup.Key,
                    edgeGroup => edgeGroup.Select(edge => edge.Item2).ToImmutableHashSet(_comparer),
                    comparer);

            _edgesIn = _vertices.ToImmutableDictionary(
                vertex => vertex,
                vertex => edgesInIndex.TryGetValue(vertex, out var adjacents) ? adjacents : ImmutableHashSet<T>.Empty,
                comparer);

            _edgesOut = _vertices.ToImmutableDictionary(
                vertex => vertex,
                vertex => edgesOutIndex.TryGetValue(vertex, out var adjacents) ? adjacents : ImmutableHashSet<T>.Empty,
                comparer);
        }

        private Graph(
            ImmutableHashSet<T> vertices,
            ImmutableArray<Tuple<T, T>> edges,
            ImmutableDictionary<T, ImmutableHashSet<T>> edgesIn,
            ImmutableDictionary<T, ImmutableHashSet<T>> edgesOut,
            IEqualityComparer<T> comparer)
        {
            _comparer = comparer;

            _edges = edges;

            _edgesIn = edgesIn;

            _edgesOut = edgesOut;

            _vertices = vertices;
        }

        private enum VertexAction
        {
            Enter,

            Exit,
        }

        private enum VertexColor
        {
            White,

            Gray,

            Black,
        }

        /// <summary>
        /// The list of edges in the graph.
        /// </summary>
        public IReadOnlyCollection<Tuple<T, T>> Edges => _edges;

        /// <summary>
        /// The list of vertices in the graph.
        /// </summary>
        public IReadOnlyCollection<T> Vertices => _vertices;

        /// <summary>
        /// Return the list of adjacent vertices in to a given vertex.
        /// </summary>
        /// <param name="vertex">The vertex with which to retrieve the adjacent vertices.</param>
        /// <returns>The list of adjacent vertices in to the given vertex.</returns>
        public IReadOnlyCollection<T> AdjacentsIn(T vertex)
        {
            if (_edgesIn.TryGetValue(vertex, out var adjacents))
            {
                return adjacents;
            }

            return ImmutableHashSet.Create<T>();
        }

        /// <summary>
        /// Return the list of adjacent vertices out of a given vertex.
        /// </summary>
        /// <param name="vertex">The vertex with which to retrieve the adjacent vertices.</param>
        /// <returns>The list of adjacent vertices out of the given vertex.</returns>
        public IReadOnlyCollection<T> AdjacentsOut(T vertex)
        {
            if (_edgesOut.TryGetValue(vertex, out var adjacents))
            {
                return adjacents;
            }

            return ImmutableHashSet.Create<T>();
        }

        /// <summary>
        /// Check if the graph contains any cycles.
        /// </summary>
        /// <returns>True if the graph is cyclical, false otherwise.</returns>
        public bool IsCyclical()
        {
            var visited = _vertices.ToDictionary(vertex => vertex, vertex => VertexColor.White, _comparer);

            foreach (var rootVertex in _vertices)
            {
                if (visited[rootVertex] == VertexColor.Black)
                {
                    continue;
                }

                var frontier = new Stack<Tuple<T, VertexAction>>();
                frontier.Push(Tuple.Create(rootVertex, VertexAction.Enter));

                while (frontier.Count > 0)
                {
                    var next = frontier.Pop();
                    var vertex = next.Item1;
                    var vertexAction = next.Item2;

                    if (vertexAction == VertexAction.Exit)
                    {
                        visited[vertex] = VertexColor.Black;
                    }
                    else
                    {
                        visited[vertex] = VertexColor.Gray;

                        var adjacents = _edgesOut[vertex];

                        frontier.Push(Tuple.Create(vertex, VertexAction.Exit));

                        foreach (var adjacent in adjacents)
                        {
                            if (visited[adjacent] == VertexColor.Gray)
                            {
                                return true;
                            }

                            frontier.Push(Tuple.Create(adjacent, VertexAction.Enter));
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Determines the partial ordering of the given graph such that there are as few partial orderings as
        ///   possible. For example, given the graph:
        /// A   B   C   D
        ///  ╲ ╱     ╲ ╱
        ///   E       F
        ///    ╲     ╱
        ///     ╲   ╱
        ///      ╲ ╱
        ///       G
        /// Three partial orderings should be computed, containing (A, B, C, D), (E, F), and (G).
        /// </summary>
        /// <returns>
        /// A list of sets of nodes, where each set has elements less than the elements after it.
        /// </returns>
        public IReadOnlyCollection<ISet<T>> MinimalPartialOrder()
        {
            var topologicalSort = TopologicalSort();

            var depths = new Dictionary<T, int>(_comparer);

            foreach (var node in topologicalSort)
            {
                var nodesIn = _edgesIn[node];

                if (nodesIn.Count == 0)
                {
                    depths[node] = 0;
                }
                else
                {
                    var depth = nodesIn.Select(otherNode => depths[otherNode]).Aggregate(Math.Max) + 1;
                    depths[node] = depth;
                }
            }

            return depths
                .GroupBy(x => x.Value, x => x.Key)
                .OrderBy(x => x.Key)
                .Select(x => x.ToImmutableHashSet())
                .ToImmutableArray();
        }

        /// <summary>
        /// Perform a topological sort on the graph, returning a list of nodes such that all nodes less than a
        /// given node come before that node, and all nodes greater than the current node come after a given node.
        /// </summary>
        /// <returns>
        /// A list of nodes that have been sorted based on a partial ordering.
        /// </returns>
        public IReadOnlyCollection<T> TopologicalSort()
        {
            var edgeCounts = new Dictionary<T, int>(_comparer);

            var rootNodes = _vertices.Where(vertex => _edgesIn[vertex].Count == 0);

            var frontierNodes = new Queue<T>(rootNodes);

            var visitedNodes = new HashSet<T>(_comparer);

            var topologicalSort = new List<T>(_vertices.Count);

            while (frontierNodes.Count > 0)
            {
                var node = frontierNodes.Dequeue();

                if (visitedNodes.Contains(node))
                {
                    continue;
                }

                visitedNodes.Add(node);

                topologicalSort.Add(node);

                foreach (var otherNode in _edgesOut[node])
                {
                    if (!edgeCounts.ContainsKey(otherNode))
                    {
                        edgeCounts[otherNode] = 0;
                    }

                    edgeCounts[otherNode] += 1;

                    if (edgeCounts[otherNode] == _edgesIn[otherNode].Count)
                    {
                        frontierNodes.Enqueue(otherNode);
                    }
                }
            }

            return topologicalSort;
        }

        /// <summary>
        /// Return the transpose of the directed graph.
        /// </summary>
        /// <returns>The transpose of the directed graph.</returns>
        public Graph<T> Transpose()
        {
            return new Graph<T>(_vertices, _edges, _edgesOut, _edgesIn, _comparer);
        }
    }
}