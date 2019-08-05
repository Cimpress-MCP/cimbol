using System;
using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using Cimpress.Cimbol.Runtime.Types;

namespace Cimpress.Cimbol.PerformanceTests.Evaluation
{
    public class TinyProgramBenchmark
    {
        private Executable _executable;

        [GlobalSetup]
        public void Setup()
        {
            var program = new Program();

            var constantValue1 = new ObjectValue(new Dictionary<string, ILocalValue>(StringComparer.OrdinalIgnoreCase)
            {
                { "FieldA", new NumberValue(1m) },
                { "FieldB", new NumberValue(3.99m) },
            });
            var constant1 = program.AddConstant("RowA", constantValue1);

            var constantValue2 = new ObjectValue(new Dictionary<string, ILocalValue>(StringComparer.OrdinalIgnoreCase)
            {
                { "FieldA", BooleanValue.True },
                { "FieldB", new NumberValue(1m) },
            });
            var constant2 = program.AddConstant("RowB", constantValue2);

            var constantValue3 = new ObjectValue(new Dictionary<string, ILocalValue>(StringComparer.OrdinalIgnoreCase)
            {
                { "FieldA", BooleanValue.False },
            });
            var constant3 = program.AddConstant("RowC", constantValue3);

            var module = program.AddModule("main");

            module.AddReference("ReferenceA", constant1);
            module.AddReference("ReferenceB", constant2);
            module.AddReference("ReferenceC", constant3);

            module.AddFormula("ResultA", "ResultB * ResultC * ResultD * (ResultE - 1)");
            module.AddFormula("ResultB", "ReferenceA.FieldA * ReferenceA.FieldB * 5");
            module.AddFormula("ResultC", "if(ReferenceB.FieldA == false, then = 1.1, else = ReferenceB.FieldB)");
            module.AddFormula("ResultD", "if(ReferenceC.FieldA == false, then = 1.2, else = ReferenceC.FieldB)");
            module.AddFormula("ResultE", "0");

            _executable = program.Compile();
        }

        [Benchmark]
        public ObjectValue Benchmark_TinyProgram()
        {
            return _executable.Call();
        }
    }
}