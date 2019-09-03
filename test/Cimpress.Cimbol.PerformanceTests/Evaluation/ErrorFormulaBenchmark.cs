using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using Cimpress.Cimbol.Runtime.Types;

namespace Cimpress.Cimbol.PerformanceTests.Evaluation
{
    public class ErrorFormulaBenchmark
    {
        private const int MaxErrors = 64;

        private Executable _executable;

        [ParamsSource(nameof(FailureCounts))]
        public int FailureCount { get; set; }

        [GlobalSetup]
        public void Setup()
        {
            const string failExpression = "1 / 0";

            const string successExpression = "1";

            var program = new Program();

            var module = program.AddModule("Main");

            for (var j = 0; j <= MaxErrors; ++j)
            {
                var formulaName = string.Format(CultureInfo.InvariantCulture, "Formula{0}", j);

                module.AddFormula(formulaName, j < FailureCount ? failExpression : successExpression);
            }

            _executable = program.Compile();
        }

        [Benchmark]
        public async Task<EvaluationResult> Benchmark_FormulaError()
        {
            return await _executable.Call();
        }

        public IEnumerable<int> FailureCounts()
        {
            for (var i = 0; i < MaxErrors;)
            {
                yield return i;

                i = i == 0 ? 1 : i * 2;
            }

            yield return MaxErrors;
        }
    }
}