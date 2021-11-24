// Copyright 2021 Cimpress plc.
// Licensed under the Apache License, Version 2.0
// http://www.apache.org/licenses/LICENSE-2.0

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Cimpress.Cimbol.Exceptions;
using Cimpress.Cimbol.Runtime;
using Cimpress.Cimbol.Runtime.Functions;
using Cimpress.Cimbol.Runtime.Types;
using NUnit.Framework;

namespace Cimpress.Cimbol.UnitTests.Runtime.Functions
{
    [TestFixture]
    public class EvaluationFunctionsTests
    {
        [Test]
        [TestCaseSource(typeof(EvaluationFunctionsTests), nameof(MethodInfoTestCases))]
        public void Should_HaveNonNullMethodInfos_When_Accessed(MethodInfo constructorInfo)
        {
            Assert.That(constructorInfo, Is.Not.Null);
        }

        [Test]
        public async Task Should_ReturnAsyncEvaluation_When_Successful()
        {
            var expected = new NumberValue(1);
            var taskValue = new PromiseValue(Task.FromResult((ILocalValue)expected));
            var dependencies = Array.Empty<int>();
            var errorList = new List<CimbolRuntimeException>();
            var skipList = new[] { true };
            var executionStepContext = new ExecutionStepContext(0, dependencies, "x", "y");

            ILocalValue result = null;
            await EvaluationFunctions.EvaluateAsynchronous_Verbose(
                executionStepContext,
                errorList,
                skipList,
                () => taskValue,
                x => result = x);

            Assert.That(result, Is.SameAs(expected));
            Assert.That(errorList, Is.Empty);
        }

        [Test]
        public async Task ShouldNot_ReturnAsyncEvaluation_When_IdIsInSkipList()
        {
            var expected = new NumberValue(1);
            var taskValue = new PromiseValue(Task.FromResult((ILocalValue)expected));
            var dependencies = Array.Empty<int>();
            var errorList = new List<CimbolRuntimeException>();
            var skipList = new[] { false };
            var executionStepContext = new ExecutionStepContext(0, dependencies, "x", "y");

            ILocalValue result = null;
            await EvaluationFunctions.EvaluateAsynchronous_Verbose(
                executionStepContext,
                errorList,
                skipList,
                () => taskValue,
                x => result = x);

            Assert.That(result, Is.Null);
            Assert.That(errorList, Has.Count.EqualTo(1));
            Assert.That(errorList[0], Is.InstanceOf<CimbolRuntimeException>());
        }

        [Test]
        public async Task ShouldNot_ReturnAsyncEvaluation_When_ExpressionThrows()
        {
            var expected = CimbolRuntimeException.IfConditionError();
            var dependencies = Array.Empty<int>();
            var errorList = new List<CimbolRuntimeException>();
            var skipList = new[] { true };
            var executionStepContext = new ExecutionStepContext(0, dependencies, "x", "y");

            ILocalValue result = null;
            await EvaluationFunctions.EvaluateAsynchronous_Verbose(
                executionStepContext,
                errorList,
                skipList,
                () => throw expected,
                x => result = x);

            Assert.That(result, Is.Null);
            Assert.That(errorList, Has.Count.EqualTo(1));
            Assert.That(errorList[0], Is.SameAs(expected));
        }

        [Test]
        public async Task ShouldNot_ReturnAsyncEvaluation_When_ResultIsNotPromiseValue()
        {
            var taskValue = new NumberValue(1);
            var dependencies = Array.Empty<int>();
            var errorList = new List<CimbolRuntimeException>();
            var skipList = new[] { true };
            var executionStepContext = new ExecutionStepContext(0, dependencies, "x", "y");

            ILocalValue result = null;
            await EvaluationFunctions.EvaluateAsynchronous_Verbose(
                executionStepContext,
                errorList,
                skipList,
                () => taskValue,
                x => result = x);

            Assert.That(result, Is.Null);
            Assert.That(errorList, Has.Count.EqualTo(1));
            Assert.That(errorList[0], Is.InstanceOf<CimbolRuntimeException>());
        }

        [Test]
        public async Task ShouldNot_ReturnAsyncEvaluation_When_PromiseResolutionThrows()
        {
            var expected = CimbolRuntimeException.IfConditionError();
            var taskValue = new PromiseValue(Task.FromException<ILocalValue>(expected));
            var dependencies = Array.Empty<int>();
            var errorList = new List<CimbolRuntimeException>();
            var skipList = new[] { true };
            var executionStepContext = new ExecutionStepContext(0, dependencies, "x", "y");

            ILocalValue result = null;
            await EvaluationFunctions.EvaluateAsynchronous_Verbose(
                executionStepContext,
                errorList,
                skipList,
                () => taskValue,
                x => result = x);

            Assert.That(result, Is.Null);
            Assert.That(errorList, Has.Count.EqualTo(1));
            Assert.That(errorList[0], Is.SameAs(expected));
        }

        [Test]
        public async Task Should_MarkDependentsAsSkipped_When_AsyncEvaluationFails()
        {
            var expected = new NumberValue(1);
            var taskValue = new PromiseValue(Task.FromResult((ILocalValue)expected));
            var dependencies = new[] { 1, 2, 3 };
            var errorList = new List<CimbolRuntimeException>();
            var skipList = new[] { false, true, true, true };
            var executionStepContext = new ExecutionStepContext(0, dependencies, "x", "y");

            await EvaluationFunctions.EvaluateAsynchronous_Verbose(
                executionStepContext,
                errorList,
                skipList,
                () => taskValue,
                x => { });

            Assert.That(skipList[1], Is.False);
            Assert.That(skipList[2], Is.False);
            Assert.That(skipList[3], Is.False);
        }

        [Test]
        public async Task ShouldNot_MarkDependentsAsSkipped_When_AsyncEvaluationSucceeds()
        {
            var expected = new NumberValue(1);
            var taskValue = new PromiseValue(Task.FromResult((ILocalValue)expected));
            var dependencies = new[] { 1, 2, 3 };
            var errorList = new List<CimbolRuntimeException>();
            var skipList = new[] { true, true, true, true };
            var executionStepContext = new ExecutionStepContext(0, dependencies, "x", "y");

            await EvaluationFunctions.EvaluateAsynchronous_Verbose(
                executionStepContext,
                errorList,
                skipList,
                () => taskValue,
                x => { });

            Assert.That(skipList[1], Is.True);
            Assert.That(skipList[2], Is.True);
            Assert.That(skipList[3], Is.True);
        }

        [Test]
        public void Should_ReturnSyncEvaluation_When_Successful()
        {
            var expected = new NumberValue(1);
            var dependencies = Array.Empty<int>();
            var errorList = new List<CimbolRuntimeException>();
            var skipList = new[] { true };
            var executionStepContext = new ExecutionStepContext(0, dependencies, "x", "y");

            var result = EvaluationFunctions.EvaluateSynchronous_Verbose(
                executionStepContext,
                errorList,
                skipList,
                () => expected);

            Assert.That(result, Is.SameAs(expected));
            Assert.That(errorList, Is.Empty);
        }

        [Test]
        public void ShouldNot_ReturnSyncEvaluation_When_IdIsInSkipList()
        {
            var expected = new NumberValue(1);
            var dependencies = Array.Empty<int>();
            var errorList = new List<CimbolRuntimeException>();
            var skipList = new[] { false };
            var executionStepContext = new ExecutionStepContext(0, dependencies, "x", "y");

            var result = EvaluationFunctions.EvaluateSynchronous_Verbose(
                executionStepContext,
                errorList,
                skipList,
                () => expected);

            Assert.That(result, Is.Null);
            Assert.That(errorList, Has.Count.EqualTo(1));
            Assert.That(errorList[0], Is.InstanceOf<CimbolRuntimeException>());
        }

        [Test]
        public void ShouldNot_ReturnSyncEvaluation_When_ExpressionThrows()
        {
            var expected = CimbolRuntimeException.IfConditionError();
            var dependencies = Array.Empty<int>();
            var errorList = new List<CimbolRuntimeException>();
            var skipList = new[] { true };
            var executionStepContext = new ExecutionStepContext(0, dependencies, "x", "y");

            var result = EvaluationFunctions.EvaluateSynchronous_Verbose(
                executionStepContext,
                errorList,
                skipList,
                () => throw expected);

            Assert.That(result, Is.Null);
            Assert.That(errorList, Has.Count.EqualTo(1));
            Assert.That(errorList[0], Is.SameAs(expected));
        }

        [Test]
        public void Should_MarkDependentsAsSkipped_When_SyncEvaluationFails()
        {
            var expected = new NumberValue(1);
            var dependencies = new[] { 1, 2, 3 };
            var errorList = new List<CimbolRuntimeException>();
            var skipList = new[] { false, true, true, true };
            var executionStepContext = new ExecutionStepContext(0, dependencies, "x", "y");

            EvaluationFunctions.EvaluateSynchronous_Verbose(
                executionStepContext,
                errorList,
                skipList,
                () => expected);

            Assert.That(skipList[1], Is.False);
            Assert.That(skipList[2], Is.False);
            Assert.That(skipList[3], Is.False);
        }

        [Test]
        public void ShouldNot_MarkDependentsAsSkipped_When_SyncEvaluationSucceeds()
        {
            var expected = new NumberValue(1);
            var dependencies = new[] { 1, 2, 3 };
            var errorList = new List<CimbolRuntimeException>();
            var skipList = new[] { true, true, true, true };
            var executionStepContext = new ExecutionStepContext(0, dependencies, "x", "y");

            EvaluationFunctions.EvaluateSynchronous_Verbose(
                executionStepContext,
                errorList,
                skipList,
                () => expected);

            Assert.That(skipList[1], Is.True);
            Assert.That(skipList[2], Is.True);
            Assert.That(skipList[3], Is.True);
        }

        [Test]
        public void Should_AssignLocalValueToObjectValue_When_ExportingValue()
        {
            var moduleValue = new ObjectValue(new Dictionary<string, ILocalValue>(StringComparer.OrdinalIgnoreCase));
            var localValue = new NumberValue(5);

            EvaluationFunctions.ExportValue(localValue, "value", moduleValue);

            Assert.That(moduleValue.Value["value"], Is.SameAs(localValue));
        }

        private static IEnumerable<TestCaseData> MethodInfoTestCases()
        {
            yield return new TestCaseData(EvaluationFunctions.ExportValueInfo);
        }
    }
}