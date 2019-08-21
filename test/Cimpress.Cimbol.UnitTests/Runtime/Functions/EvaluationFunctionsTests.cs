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
        public void Should_Await_When_GivenCompletedPromiseValue()
        {
            var innerValue = new NumberValue(5);
            var promiseValue = new PromiseValue(Task.FromResult((ILocalValue)innerValue));

            ILocalValue result = null;
            var resultTask = EvaluationFunctions.AsyncEval(promiseValue, localValue => result = localValue);
            
            Assert.DoesNotThrowAsync(async () => await resultTask);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.SameAs(innerValue));
            Assert.That(resultTask.Status, Is.EqualTo(TaskStatus.RanToCompletion));
        }

        [Test]
        public void Should_Fault_When_GivenFaultedPromiseValue()
        {
            var error = new NotSupportedException();
            var promiseValue = new PromiseValue(Task.FromException<ILocalValue>(error));

            ILocalValue result = null;
            var resultTask = EvaluationFunctions.AsyncEval(promiseValue, localValue => result = localValue);

            var resultError = Assert.ThrowsAsync<NotSupportedException>(async () => await resultTask);
            Assert.That(resultError, Is.SameAs(error));
            Assert.That(result, Is.Null);
            Assert.That(resultTask.Status, Is.EqualTo(TaskStatus.Faulted));
        }

        [Test]
        public void Should_Fault_When_GivenNonPromiseValueToAwait()
        {
            var promiseValue = new NumberValue(5);

            ILocalValue result = null;
            var resultTask = EvaluationFunctions.AsyncEval(promiseValue, localValue => result = localValue);

            var resultError = Assert.ThrowsAsync<CimbolRuntimeException>(async () => await resultTask);
            Assert.That(result, Is.Null);
            Assert.That(resultTask.Status, Is.EqualTo(TaskStatus.Faulted));
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
            yield return new TestCaseData(EvaluationFunctions.AsyncEvalInfo);
            yield return new TestCaseData(EvaluationFunctions.ExportValueInfo);
        }
    }
}