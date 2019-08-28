using System.Threading.Tasks;
using Cimpress.Cimbol.Exceptions;
using Cimpress.Cimbol.Runtime.Types;
using NUnit.Framework;

namespace Cimpress.Cimbol.UnitTests.Runtime.Types
{
    [TestFixture]
    public class PromiseValueTests
    {
        [Test]
        public void Should_ThrowException_When_AccessingAnElement()
        {
            var value = new PromiseValue(Task.FromResult((ILocalValue)BooleanValue.True));

            Assert.Throws<CimbolRuntimeException>(() => value.Access("Key"));
        }

        [Test]
        public void Should_ThrowException_When_ConvertingToBoolean()
        {
            var value = new PromiseValue(Task.FromResult((ILocalValue)BooleanValue.True));

            Assert.Throws<CimbolRuntimeException>(() => value.CastBoolean());
        }

        [Test]
        public void Should_ThrowException_When_ConvertingToNumber()
        {
            var value = new PromiseValue(Task.FromResult((ILocalValue)BooleanValue.True));

            Assert.Throws<CimbolRuntimeException>(() => value.CastNumber());
        }

        [Test]
        public void Should_ThrowException_When_ConvertingToString()
        {
            var value = new PromiseValue(Task.FromResult((ILocalValue)BooleanValue.True));

            Assert.Throws<CimbolRuntimeException>(() => value.CastString());
        }

        [Test]
        public void Should_ThrowException_When_Invoking()
        {
            var value = new PromiseValue(Task.FromResult((ILocalValue)BooleanValue.True));

            Assert.Throws<CimbolRuntimeException>(() => value.Invoke());
        }

        [Test]
        public void Should_BeEqual_When_ComparedToSelf()
        {
            var value = new PromiseValue(Task.FromResult((ILocalValue)BooleanValue.True));

            var result = value.EqualTo(value);

            Assert.That(result, Is.True);
        }

        [Test]
        public void ShouldNot_BeEqual_When_ComparedToErrorValue()
        {
            var value = new PromiseValue(Task.FromResult((ILocalValue)BooleanValue.True));
            var otherValue = new PromiseValue(Task.FromResult((ILocalValue)BooleanValue.True));

            var result = value.EqualTo(otherValue);

            Assert.That(result, Is.False);
        }

        [Test]
        public void ShouldNot_BeEqual_When_ComparedToNonErrorValue()
        {
            var value = new PromiseValue(Task.FromResult((ILocalValue)BooleanValue.True));
            var otherValue = new NumberValue(1);

            var result = value.EqualTo(otherValue);

            Assert.That(result, Is.False);
        }
    }
}