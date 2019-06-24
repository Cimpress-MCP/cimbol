using System;
using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using Cimpress.Cimbol.Runtime.Types;

namespace Cimpress.Cimbol.PerformanceTests.Runtime.Types
{
    public class ObjectTypeBenchmarks
    {
        private ObjectValue _object;

        [GlobalSetup]
        public void GlobalSetup()
        {
            var objectContents = new Dictionary<string, ILocalValue>(StringComparer.OrdinalIgnoreCase)
            {
                { "x", new NumberValue(3) },
                { "y", new StringValue("test") },
                { "z", new BooleanValue(true) },
            };

            _object = new ObjectValue(objectContents);
        }

        [Benchmark]
        public ILocalValue Benchmark_ObjectAccess()
        {
            return _object.Access("y");
        }
    }
}