using System;
using BenchmarkDotNet.Attributes;
using Cimpress.Cimbol.Runtime.Types;

namespace Cimpress.Cimbol.PerformanceTests.Runtime.Types
{
    public class FunctionTypeBenchmarks
    {
        private FunctionValue _arithmeticFunction;

        private FunctionValue _identityFunction;

        private FunctionValue _variadicFunction;

        private NumberValue _testNumber1;

        private NumberValue _testNumber2;

        private NumberValue _testNumber3;

        [GlobalSetup]
        public void GlobalSetup()
        {
            _arithmeticFunction = new FunctionValue(new[] { (Func<NumberValue, NumberValue, NumberValue>)ArithmeticFunction });

            _identityFunction = new FunctionValue(new[] { (Func<NumberValue, NumberValue>)IdentityFunction });

            _variadicFunction = new FunctionValue(new[] { (Func<NumberValue[], NumberValue>)VariadicFunction });

            _testNumber1 = new NumberValue(3);

            _testNumber2 = new NumberValue(5);

            _testNumber3 = new NumberValue(7);
        }

        [Benchmark]
        public ILocalValue Benchmark_ArithmeticFunctionInvocation()
        {
            return _arithmeticFunction.Invoke(_testNumber1, _testNumber2);
        }

        [Benchmark]
        public ILocalValue Benchmark_IdentityFunctionInvocation()
        {
            return _identityFunction.Invoke(_testNumber1);
        }

        [Benchmark]
        public ILocalValue Benchmark_VariadicFunctionInvocation()
        {
            return _variadicFunction.Invoke(_testNumber1, _testNumber2, _testNumber3);
        }

        private static NumberValue ArithmeticFunction(NumberValue number1, NumberValue number2)
        {
            return new NumberValue(number1.Value + number2.Value);
        }

        private static NumberValue IdentityFunction(NumberValue number)
        {
            return number;
        }

        private static NumberValue VariadicFunction(NumberValue[] numberValues)
        {
            return numberValues[0];
        }
    }
}