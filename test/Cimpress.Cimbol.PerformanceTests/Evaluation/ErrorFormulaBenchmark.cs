﻿using System.Globalization;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;

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

            _executable = program.Compile(CompilationProfile.Verbose);
        }

        [Benchmark]
        public async Task<EvaluationResult> Benchmark_FormulaError()
        {
            return await _executable.Call();
        }

        public int[] FailureCounts()
        {
            return new[] { 0, 1, 2, 4, 8, 16, 32, 48, 64 };
        }
    }
}