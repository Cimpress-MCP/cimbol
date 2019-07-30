using System.Linq.Expressions;
using Cimpress.Cimbol.Compiler.Emit;
using Cimpress.Cimbol.Runtime.Types;
using NUnit.Framework;

namespace Cimpress.Cimbol.UnitTests.Compiler.Emit
{
    [TestFixture]
    public class SymbolTests
    {
        [Test]
        public void Should_InitializeParameterExpression_When_Constructed()
        {
            var symbol = new Symbol("test", typeof(ILocalValue));

            var result = symbol.Variable;

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<ParameterExpression>());
            Assert.That(result.Name, Is.EqualTo("test"));
            Assert.That(result.Type, Is.EqualTo(typeof(ILocalValue)));
        }
    }
}