using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using Cimpress.Cimbol.Runtime.Types;

namespace Cimpress.Cimbol.PerformanceTests.Evaluation
{
    public class TinyAsyncProgramBenchmark
    {
        private Executable _executable;

        [GlobalSetup]
        public void Setup()
        {
            var program = new Program();

            var constantValue1 = new FunctionValue((Func<NumberValue, PromiseValue>)AsyncFunction);
            var constant1 = program.AddConstant("AsyncFunction", constantValue1);

            var module = program.AddModule("main");

            module.AddReference("AsyncFunction", constant1);

            module.AddFormula("ResultA", "await AsyncFunction(2)");
            module.AddFormula("ResultB", "await AsyncFunction(4)");
            module.AddFormula("ResultC", "await AsyncFunction(ResultA + ResultB)");
            module.AddFormula("ResultD", "ResultA + ResultB + ResultC");

            _executable = program.Compile();
        }

        [Benchmark]
        public ObjectValue Benchmark_TinyProgram()
        {
            return _executable.Call().Result;
        }

        private static PromiseValue AsyncFunction(NumberValue numberValue)
        {
            var result = new NumberValue(numberValue.Value * 2);

            return new PromiseValue(Task.FromResult<ILocalValue>(result));
        }
    }
}