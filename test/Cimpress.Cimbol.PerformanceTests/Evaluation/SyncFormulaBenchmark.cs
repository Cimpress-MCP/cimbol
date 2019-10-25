using System.Globalization;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using Cimpress.Cimbol.Runtime.Types;

namespace Cimpress.Cimbol.PerformanceTests.Evaluation
{
    public class SyncFormulaBenchmark
    {
        private Executable _executable;

        [ParamsSource(nameof(CompilationProfiles))]
        public CompilationProfile CompilationProfile { get; set; }

        [ParamsSource(nameof(FormulaCounts))]
        public int FormulaCount { get; set; }

        [GlobalSetup]
        public void GlobalSetup()
        {
            var program = new Program();

            var constant = program.AddConstant("Constant1", new NumberValue(1));

            var module = program.AddModule("Main");

            module.AddReference("Constant1", constant);

            for (var i = 0; i < FormulaCount; ++i)
            {
                var formulaName = string.Format(CultureInfo.InvariantCulture, "Formula{0}", i);

                module.AddFormula(formulaName, "Constant1");
            }

            _executable = program.Compile(CompilationProfile);
        }

        [Benchmark]
        public async Task<EvaluationResult> Benchmark_FormulaSync()
        {
            return await _executable.Call();
        }

        public CompilationProfile[] CompilationProfiles()
        {
            return new[] { CompilationProfile.Minimal, CompilationProfile.Trace, CompilationProfile.Verbose };
        }

        public int[] FormulaCounts()
        {
            return new[] { 16, 32, 48, 64 };
        }
    }
}