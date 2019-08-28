using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using BenchmarkDotNet.Attributes;
using Cimpress.Cimbol.Runtime.Types;

namespace Cimpress.Cimbol.PerformanceTests.Evaluation
{
    public class TinyErrorProgramBenchmark
    {
        private const int MaxErrors = 4;

        private Executable[] _executables;

        [ParamsSource(nameof(FailureCounts))]
        public int FailureCount { get; set; }

        public IEnumerable<int> FailureCounts => Enumerable.Range(0, MaxErrors + 1);

        [GlobalSetup]
        public void Setup()
        {
            const string failExpression = "1 / 0";

            const string successExpression = "1";

            _executables = new Executable[MaxErrors + 1];

            for (var i = 0; i <= MaxErrors; ++i)
            {
                var program = new Program();

                var module = program.AddModule("Main");

                for (var j = 0; j <= MaxErrors; ++j)
                {
                    var formulaName = string.Format(CultureInfo.InvariantCulture, "Result{0}", j);

                    module.AddFormula(formulaName, j < i ? failExpression : successExpression);
                }

                _executables[i] = program.Compile();
            }
        }

        [Benchmark]
        public ObjectValue Benchmark_TinyErrorProgram()
        {
            return _executables[FailureCount].Call().Result;
        }
    }
}