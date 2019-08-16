using System.Collections.Generic;
using System.Reflection;
using Cimpress.Cimbol.Runtime.Functions;
using NUnit.Framework;

namespace Cimpress.Cimbol.UnitTests.Runtime.Functions
{
    [TestFixture]
    public class StandardFunctionsTests
    {
        [Test]
        [TestCaseSource(typeof(StandardFunctionsTests), nameof(ConstructorInfoTestCases))]
        public void Should_HaveNonNullConstructorInfos_When_Accessed(ConstructorInfo constructorInfo)
        {
            Assert.That(constructorInfo, Is.Not.Null);
        }

        [Test]
        [TestCaseSource(typeof(StandardFunctionsTests), nameof(MethodInfoTestCases))]
        public void Should_HaveNonNullMethodInfos_When_Accessed(MethodInfo constructorInfo)
        {
            Assert.That(constructorInfo, Is.Not.Null);
        }

        private static IEnumerable<TestCaseData> ConstructorInfoTestCases()
        {
            yield return new TestCaseData(StandardFunctions.NotSupportedExceptionConstructorInfo);
            yield return new TestCaseData(StandardFunctions.DictionaryConstructorInfo);
        }

        private static IEnumerable<TestCaseData> MethodInfoTestCases()
        {
            yield return new TestCaseData(StandardFunctions.DictionaryAddInfo);
            yield return new TestCaseData(StandardFunctions.TaskWhenAllInfo);
        }
    }
}