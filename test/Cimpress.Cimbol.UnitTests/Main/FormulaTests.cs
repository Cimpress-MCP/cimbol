using System;
using NUnit.Framework;

namespace Cimpress.Cimbol.UnitTests.Main
{
    [TestFixture]
    public class FormulaTests
    {
        [Test]
        public void Should_ThrowArgumentNullException_When_InitializedWithNullModule()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var formula = new Formula(null, "formula", "x");
            });
        }

        [Test]
        public void Should_ThrowException_When_InitializedWithNullNameWhenVisible()
        {
            Assert.Throws<NotSupportedException>(() =>
            {
                var formula = new Formula(new Module(new Program(), "module"), null, "x", FormulaFlags.Visible);
            });
        }

        [Test]
        public void Should_ThrowException_When_InitializedWithNameWhenInvisible()
        {
            Assert.Throws<NotSupportedException>(() =>
            {
                var formula = new Formula(new Module(new Program(), "module"), "formula", "x", FormulaFlags.None);
            });
        }

        [Test]
        public void Should_ThrowArgumentNullException_When_InitializedWithNullValue()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var formula = new Formula(new Module(new Program(), "module"), "formula", null);
            });
        }

        [Test]
        public void Should_RetrieveSameProgram_When_InitializedWithModule()
        {
            var program = new Program();
            var module = new Module(program, "module");
            var formula = new Formula(module, "formula", "x");
            Assert.That(formula.Program, Is.SameAs(program));
        }

        [Test]
        public void Should_RetrieveSameModule_When_InitializedWithModule()
        {
            var module = new Module(new Program(), "module");
            var formula = new Formula(module, "formula", "x");
            Assert.That(formula.Module, Is.SameAs(module));
        }

        [Test]
        public void Should_RetrieveSameName_When_InitializedWithName()
        {
            var name = "formula";
            var formula = new Formula(new Module(new Program(), "module"), name, "x");
            Assert.That(formula.Name, Is.SameAs(name));
        }

        [Test]
        public void Should_RetrieveSameValue_When_InitializedWithValue()
        {
            var value = "x";
            var formula = new Formula(new Module(new Program(), "module"), "formula", value);
            Assert.That(formula.Value, Is.SameAs(value));
        }

        [Test]
        [TestCase(FormulaFlags.None, false, false, false)]
        [TestCase(FormulaFlags.Visible, false, false, true)]
        [TestCase(FormulaFlags.Private, false, false, true)]
        [TestCase(FormulaFlags.Referenceable, false, true, false)]
        [TestCase(FormulaFlags.Internal, false, true, true)]
        [TestCase(FormulaFlags.Exported, true, false, false)]
        [TestCase(FormulaFlags.Public, true, true, true)]
        public void Should_RetrieveSameFlags_When_InitializedWithFlags(
            FormulaFlags flags,
            bool exported,
            bool referenceable,
            bool visible)
        {
            var name = flags.HasFlag(FormulaFlags.Visible) ? "formula" : null;
            var formula = new Formula(new Module(new Program(), "module"), name, "x", flags);
            Assert.That(formula.IsExported, Is.EqualTo(exported));
            Assert.That(formula.IsReferenceable, Is.EqualTo(referenceable));
            Assert.That(formula.IsVisible, Is.EqualTo(visible));
        }
    }
}
