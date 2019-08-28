using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Cimpress.Cimbol.Exceptions;
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
            var skipList = new[] { true };

            ILocalValue result = null;
            await EvaluationFunctions.EvaluateAsynchronous(0, dependencies, skipList, () => taskValue, x => result = x);

            Assert.That(result, Is.SameAs(expected));
        }

        [Test]
        public async Task ShouldNot_ReturnAsyncEvaluation_When_IdIsInSkipList()
        {
            var expected = new NumberValue(1);
            var taskValue = new PromiseValue(Task.FromResult((ILocalValue)expected));
            var dependencies = Array.Empty<int>();
            var skipList = new[] { false };

            ILocalValue result = null;
            await EvaluationFunctions.EvaluateAsynchronous(0, dependencies, skipList, () => taskValue, x => result = x);

            Assert.That(result, Is.InstanceOf<ErrorValue>());
            Assert.That(result, Has.Property("Value").Null);
        }

        [Test]
        public async Task ShouldNot_ReturnAsyncEvaluation_When_ExpressionThrows()
        {
            var expected = CimbolRuntimeException.IfConditionError(null);
            var dependencies = Array.Empty<int>();
            var skipList = new[] { true };

            ILocalValue result = null;
            await EvaluationFunctions.EvaluateAsynchronous(
                0,
                dependencies,
                skipList,
                () => throw expected,
                x => result = x);

            Assert.That(result, Is.InstanceOf<ErrorValue>());
            Assert.That(result, Has.Property("Value").SameAs(expected));
        }

        [Test]
        public async Task ShouldNot_ReturnAsyncEvaluation_When_ResultIsNotPromiseValue()
        {
            var taskValue = new NumberValue(1);
            var dependencies = Array.Empty<int>();
            var skipList = new[] { true };

            ILocalValue result = null;
            await EvaluationFunctions.EvaluateAsynchronous(0, dependencies, skipList, () => taskValue, x => result = x);

            Assert.That(result, Is.InstanceOf<ErrorValue>());
            Assert.That(result, Has.Property("Value").Not.Null);
        }

        [Test]
        public async Task ShouldNot_ReturnAsyncEvaluation_When_PromiseResolutionThrows()
        {
            var expected = CimbolRuntimeException.IfConditionError(null);
            var taskValue = new PromiseValue(Task.FromException<ILocalValue>(expected));
            var dependencies = Array.Empty<int>();
            var skipList = new[] { true };

            ILocalValue result = null;
            await EvaluationFunctions.EvaluateAsynchronous(
                0,
                dependencies,
                skipList,
                () => taskValue,
                x => result = x);

            Assert.That(result, Is.InstanceOf<ErrorValue>());
            Assert.That(result, Has.Property("Value").SameAs(expected));
        }

        [Test]
        public async Task Should_MarkDependentsAsSkipped_When_AsyncEvaluationFails()
        {
            var expected = new NumberValue(1);
            var taskValue = new PromiseValue(Task.FromResult((ILocalValue)expected));
            var dependencies = new[] { 1, 2, 3 };
            var skipList = new[] { false, true, true, true };

            await EvaluationFunctions.EvaluateAsynchronous(
                0,
                dependencies,
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
            var skipList = new[] { true, true, true, true };

            await EvaluationFunctions.EvaluateAsynchronous(
                0,
                dependencies,
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
            var skipList = new[] { true };

            var result = EvaluationFunctions.EvaluateSynchronous(0, dependencies, skipList, () => expected);

            Assert.That(result, Is.SameAs(expected));
        }

        [Test]
        public void ShouldNot_ReturnSyncEvaluation_When_IdIsInSkipList()
        {
            var expected = new NumberValue(1);
            var dependencies = Array.Empty<int>();
            var skipList = new[] { false };

            var result = EvaluationFunctions.EvaluateSynchronous(0, dependencies, skipList, () => expected);

            Assert.That(result, Is.InstanceOf<ErrorValue>());
            Assert.That(result, Has.Property("Value").Null);
        }

        [Test]
        public void ShouldNot_ReturnSyncEvaluation_When_ExpressionThrows()
        {
            var expected = CimbolRuntimeException.IfConditionError(null);
            var dependencies = Array.Empty<int>();
            var skipList = new[] { true };

            var result = EvaluationFunctions.EvaluateSynchronous(0, dependencies, skipList, () => throw expected);

            Assert.That(result, Is.InstanceOf<ErrorValue>());
            Assert.That(result, Has.Property("Value").SameAs(expected));
        }

        [Test]
        public void Should_MarkDependentsAsSkipped_When_SyncEvaluationFails()
        {
            var expected = new NumberValue(1);
            var dependencies = new[] { 1, 2, 3 };
            var skipList = new[] { false, true, true, true };

            EvaluationFunctions.EvaluateSynchronous(0, dependencies, skipList, () => expected);

            Assert.That(skipList[1], Is.False);
            Assert.That(skipList[2], Is.False);
            Assert.That(skipList[3], Is.False);
        }

        [Test]
        public void ShouldNot_MarkDependentsAsSkipped_When_SyncEvaluationSucceeds()
        {
            var expected = new NumberValue(1);
            var dependencies = new[] { 1, 2, 3 };
            var skipList = new[] { true, true, true, true };

            EvaluationFunctions.EvaluateSynchronous(0, dependencies, skipList, () => expected);

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