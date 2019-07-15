using System;
using Cimpress.Cimbol.Runtime.Types;
using NUnit.Framework;

namespace Cimpress.Cimbol.UnitTests.Main
{
    [TestFixture]
    public class ConstantTests
    {
        [Test]
        public void Should_ThrowArgumentNullException_When_InitializingWithNullProgram()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var constant = new Constant(null, "constant", new NumberValue(5));
            });
        }

        [Test]
        public void Should_ThrowArgumentNullException_When_InitializingWithNullName()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var constant = new Constant(new Program(), null, new NumberValue(5));
            });
        }

        [Test]
        public void Should_ThrowArgumentNullException_When_InitializingWithNullValue()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var constant = new Constant(new Program(), "constant", null);
            });
        }

        [Test]
        public void Should_RetrieveSameProgram_When_InitializedWithProgram()
        {
            var program = new Program();
            var constant = new Constant(program, "constant", new NumberValue(5));
            Assert.That(constant.Program, Is.SameAs(program));
        }

        [Test]
        public void Should_RetrieveSameName_When_InitializedWithName()
        {
            var name = "constant";
            var constant = new Constant(new Program(), name, new NumberValue(5));
            Assert.That(constant.Name, Is.SameAs(name));
        }

        [Test]
        public void Should_RetrieveSameValue_When_InitializedWithValue()
        {
            var value = new NumberValue(5);
            var constant = new Constant(new Program(), "constant", value);
            Assert.That(constant.Value, Is.SameAs(value));
        }
    }
}