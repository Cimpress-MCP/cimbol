// Copyright 2021 Cimpress plc.
// Licensed under the Apache License, Version 2.0
// http://www.apache.org/licenses/LICENSE-2.0

using System;
using System.Collections.Generic;
using System.Linq;
using Cimpress.Cimbol.Utilities;

namespace Cimpress.Cimbol.UnitTests.Main.Internal
{
    public static class GraphTestUtilities
    {
        public static Graph<int> BuildCompleteGraph(int n)
        {
            var vertices = Enumerable.Range(0, n).ToArray();
            var edges = Enumerable.Range(0, n)
                .SelectMany(l => Enumerable.Range(0, n).Select(r => Tuple.Create(l, r)))
                .ToArray();

            return new Graph<int>(vertices, edges);
        }

        public static Graph<int> BuildCycleGraph(int n)
        {
            var vertices = Enumerable.Range(0, n).ToArray();
            var edges = Enumerable.Range(0, n - 1)
                .Zip(Enumerable.Range(1, n), Tuple.Create)
                .Append(Tuple.Create(n - 1, 0))
                .ToArray();

            return new Graph<int>(vertices, edges);
        }

        public static Graph<int> BuildDiamondGraph(int n)
        {
            IEnumerable<Tuple<int, int>> CreateEdges(int i)
            {
                var x = i % n;
                var y = i / n;

                if (x + 1 < n)
                {
                    var j = x + 1 + (y * n);
                    yield return Tuple.Create(i, j);
                }

                if (y + 1 < n)
                {
                    var j = x + ((y + 1) * n);
                    yield return Tuple.Create(i, j);
                }
            }

            var vertices = Enumerable.Range(0, n * n).ToArray();
            var edges = Enumerable.Range(0, n * n).SelectMany(CreateEdges).ToArray();

            return new Graph<int>(vertices, edges);
        }

        public static Graph<int> BuildDisjointGraph(int n)
        {
            var vertices = Enumerable.Range(0, n).ToArray();
            var edges = Array.Empty<Tuple<int, int>>();

            return new Graph<int>(vertices, edges);
        }

        public static Graph<int> BuildEmptyGraph()
        {
            var vertices = Enumerable.Empty<int>().ToArray();
            var edges = Enumerable.Empty<Tuple<int, int>>().ToArray();

            return new Graph<int>(vertices, edges);
        }

        public static Graph<int> BuildPathGraph(int n)
        {
            var vertices = Enumerable.Range(0, n).ToArray();
            var edges = Enumerable.Range(0, n - 1).Zip(Enumerable.Range(1, n), Tuple.Create).ToArray();

            return new Graph<int>(vertices, edges);
        }

        public static Graph<int> BuildTreeGraph(int n)
        {
            var power = (int)Math.Pow(2, n) - 1;

            IEnumerable<Tuple<int, int>> CreateEdges(int i)
            {
                if ((i * 2) + 1 < power)
                {
                    yield return Tuple.Create(i, (i * 2) + 1);
                }

                if ((i * 2) + 2 < power)
                {
                    yield return Tuple.Create(i, (i * 2) + 2);
                }
            }

            var vertices = Enumerable.Range(0, power).ToArray();
            var edges = Enumerable.Range(0, power).SelectMany(CreateEdges).ToArray();

            return new Graph<int>(vertices, edges);
        }
    }
}