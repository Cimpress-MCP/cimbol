using System.Collections.Generic;
using System.Reflection;
using Cimpress.Cimbol.Runtime.Functions;
using NUnit.Framework;

namespace Cimpress.Cimbol.UnitTests.Runtime.Functions
{
    [TestFixture]
    public class LocalValueFunctionsTests
    {
        [Test]
        [TestCaseSource(typeof(LocalValueFunctionsTests), nameof(ConstructorInfoTestCases))]
        public void Should_HaveNonNullConstructorInfos_When_Accessed(ConstructorInfo constructorInfo)
        {
            Assert.That(constructorInfo, Is.Not.Null);
        }

        [Test]
        [TestCaseSource(typeof(LocalValueFunctionsTests), nameof(MethodInfoTestCases))]
        public void Should_HaveNonNullMethodInfos_When_Accessed(MethodInfo constructorInfo)
        {
            Assert.That(constructorInfo, Is.Not.Null);
        }

        private static IEnumerable<TestCaseData> ConstructorInfoTestCases()
        {
            yield return new TestCaseData(LocalValueFunctions.ListValueConstructorInfo);
            yield return new TestCaseData(LocalValueFunctions.ObjectValueConstructorInfo);
        }

        private static IEnumerable<TestCaseData> MethodInfoTestCases()
        {
            yield return new TestCaseData(LocalValueFunctions.AccessInfo);
            yield return new TestCaseData(LocalValueFunctions.CastBooleanInfo);
            yield return new TestCaseData(LocalValueFunctions.CastNumberInfo);
            yield return new TestCaseData(LocalValueFunctions.CastStringInfo);
            yield return new TestCaseData(LocalValueFunctions.InvokeInfo);
            yield return new TestCaseData(LocalValueFunctions.ObjectAssignInfo);
        }
    }
}