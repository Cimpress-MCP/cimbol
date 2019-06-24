using System;
using BenchmarkDotNet.Attributes;
using Cimpress.Cimbol.Runtime.Types;

namespace Cimpress.Cimbol.PerformanceTests.Runtime.Types
{
    public class FunctionTypeBenchmarks
    {
        private FunctionValue _arithmeticFunction;

        private FunctionValue _identityFunction;

        private NumberValue _testNumber1;

        private NumberValue _testNumber2;

        private StringValue _testString1;

        private StringValue _testString2;

        [GlobalSetup]
        public void GlobalSetup()
        {
            _arithmeticFunction = new FunctionValue((Func<NumberValue, NumberValue, NumberValue>)ArithmeticFunction);

            _identityFunction = new FunctionValue((Func<NumberValue, NumberValue>)IdentityFunction);

            _testNumber1 = new NumberValue(3);

            _testNumber2 = new NumberValue(5);

            _testString1 = new StringValue("3");

            _testString2 = new StringValue("5");
        }

        [Benchmark]
        public ILocalValue Benchmark_IdentityFunctionInvocation()
        {
            return _identityFunction.Invoke(_testNumber1);
        }

        [Benchmark]
        public ILocalValue Benchmark_ArithmeticFunctionInvocation()
        {
            return _arithmeticFunction.Invoke(_testNumber1, _testNumber2);
        }

        private static NumberValue ArithmeticFunction(NumberValue number1, NumberValue number2)
        {
            return new NumberValue(number1.Value + number2.Value);
        }

        private static NumberValue IdentityFunction(NumberValue number)
        {
            return number;
        }
    }
}