// Copyright 2021 Cimpress plc.
// Licensed under the Apache License, Version 2.0
// http://www.apache.org/licenses/LICENSE-2.0

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Cimpress.Cimbol.Exceptions;
using Cimpress.Cimbol.Runtime.Types;
using NUnit.Framework;

namespace Cimpress.Cimbol.UnitTests.Main
{
    [TestFixture]
    public class ExecutableTests
    {
        [Test]
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public async Task Should_RunToCompletion_When_InvokedCorrectly(int argumentCount)
        {
            var returnValue = new EvaluationResult(
                new Dictionary<string, ObjectValue>(),
                new List<CimbolRuntimeException>());
            var function = CreateMockFunction(Task.FromResult(returnValue), argumentCount);
            var executable = new Executable(function);
            var arguments = Enumerable.Range(0, argumentCount).Select(_ => (ILocalValue)BooleanValue.True).ToArray();

            var result = await executable.Call(arguments);

            Assert.That(result, Is.SameAs(returnValue));
        }

        [Test]
        [TestCase(0, 1)]
        [TestCase(0, 2)]
        [TestCase(0, 3)]
        [TestCase(1, 0)]
        [TestCase(1, 2)]
        [TestCase(1, 3)]
        [TestCase(2, 0)]
        [TestCase(2, 1)]
        [TestCase(2, 3)]
        [TestCase(3, 0)]
        [TestCase(3, 1)]
        [TestCase(3, 2)]
        public void ShouldNot_RunToCompletion_When_InvokedWithIncorrectNumberOfArguments(
            int argumentCount,
            int providedCount)
        {
            var returnValue = new EvaluationResult(
                new Dictionary<string, ObjectValue>(),
                new List<CimbolRuntimeException>());
            var function = CreateMockFunction(Task.FromResult(returnValue), argumentCount);
            var executable = new Executable(function);
            var arguments = Enumerable.Range(0, providedCount).Select(_ => (ILocalValue)BooleanValue.True).ToArray();

            Assert.That(async () => await executable.Call(arguments), Throws.InstanceOf<ArgumentCountException>());
        }

        [Test]
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void ShouldNot_RunToCompletion_When_GivenNullArguments(int argumentCount)
        {
            var returnValue = new EvaluationResult(
                new Dictionary<string, ObjectValue>(),
                new List<CimbolRuntimeException>());
            var function = CreateMockFunction(Task.FromResult(returnValue), argumentCount);
            var executable = new Executable(function);

            Assert.That(
                async () => await executable.Call((ILocalValue[])null),
                Throws.InstanceOf<ArgumentNullException>());
        }

        private Delegate CreateMockFunction(Task<EvaluationResult> returnValue, int argumentCount)
        {
            var parameters = new ParameterExpression[argumentCount];

            for (var i = 0; i < argumentCount; ++i)
            {
                parameters[i] = Expression.Parameter(typeof(ILocalValue));
            }

            var returnExpression = Expression.Constant(returnValue);

            var functionSource = Expression.Lambda(returnExpression, parameters);

            return functionSource.Compile();
        }
    }
}