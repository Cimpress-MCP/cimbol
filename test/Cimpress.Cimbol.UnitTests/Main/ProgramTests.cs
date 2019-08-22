using System;
using System.Linq;
using Cimpress.Cimbol.Runtime.Types;
using NUnit.Framework;

namespace Cimpress.Cimbol.UnitTests.Main
{
    [TestFixture]
    public class ProgramTests
    {
        [Test]
        public void Should_Initialize_When_Constructed()
        {
            Assert.DoesNotThrow(() =>
            {
                var program = new Program();
            });
        }

        [Test]
        public void Should_CreateArgument_When_GivenName()
        {
            var program = new Program();

            Argument argument = null;
            Assert.DoesNotThrow(() => argument = program.AddArgument("argument"));
            Assert.That(argument, Is.Not.Null);
            Assert.That(argument.Name, Is.EqualTo("argument"));
            Assert.That(argument.Program, Is.SameAs(program));
        }

        [Test]
        public void ShouldNot_CreateArgument_When_GivenNullName()
        {
            var program = new Program();

            Assert.Throws<ArgumentNullException>(() => program.AddArgument(null));
        }

        [Test]
        [TestCase("argument")]
        [TestCase("ARGUMENT")]
        [TestCase("aRgUmEnT")]
        public void Should_CreateArgument_When_GivenNameOfExistingConstant(string resourceName)
        {
            var program = new Program();
            program.AddConstant("argument", new NumberValue(1));

            Argument argument = null;
            Assert.DoesNotThrow(() => argument = program.AddArgument(resourceName));
            Assert.That(argument, Is.Not.Null);
            Assert.That(argument.Name, Is.EqualTo(resourceName));
            Assert.That(argument.Program, Is.SameAs(program));
        }

        [Test]
        [TestCase("argument")]
        [TestCase("ARGUMENT")]
        [TestCase("aRgUmEnT")]
        public void Should_CreateArgument_When_GivenNameOfExistingModule(string resourceName)
        {
            var program = new Program();
            program.AddModule("argument");

            Argument argument = null;
            Assert.DoesNotThrow(() => argument = program.AddArgument(resourceName));
            Assert.That(argument, Is.Not.Null);
            Assert.That(argument.Name, Is.EqualTo(resourceName));
            Assert.That(argument.Program, Is.SameAs(program));
        }

        [Test]
        [TestCase("argument")]
        [TestCase("ARGUMENT")]
        [TestCase("aRgUmEnT")]
        public void ShouldNot_CreateArgument_When_GivenNameOfExistingArgument(string resourceName)
        {
            var program = new Program();
            program.AddArgument("argument");

            Assert.Throws<ArgumentException>(() => program.AddArgument(resourceName));
        }

        [Test]
        public void Should_CreateConstant_When_GivenNameAndValue()
        {
            var program = new Program();

            Constant constant = null;
            Assert.DoesNotThrow(() => constant = program.AddConstant("constant", new NumberValue(1)));
            Assert.That(constant, Is.Not.Null);
            Assert.That(constant.Name, Is.EqualTo("constant"));
            Assert.That(constant.Program, Is.SameAs(program));
            Assert.That(constant.Value, Is.InstanceOf<NumberValue>());
        }

        [Test]
        public void ShouldNot_CreateConstant_When_GivenNullName()
        {
            var program = new Program();

            Assert.Throws<ArgumentNullException>(() => program.AddConstant(null, new NumberValue(1)));
        }

        [Test]
        public void ShouldNot_CreateConstant_When_GivenNullValue()
        {
            var program = new Program();

            Assert.Throws<ArgumentNullException>(() => program.AddConstant("constant", null));
        }

        [Test]
        [TestCase("constant")]
        [TestCase("CONSTANT")]
        [TestCase("cOnStAnT")]
        public void Should_CreateConstant_When_GivenNameOfExistingArgument(string resourceName)
        {
            var program = new Program();
            program.AddArgument("constant");

            Constant constant = null;
            Assert.DoesNotThrow(() => constant = program.AddConstant(resourceName, new NumberValue(1)));
            Assert.That(constant, Is.Not.Null);
            Assert.That(constant.Name, Is.EqualTo(resourceName));
            Assert.That(constant.Program, Is.SameAs(program));
            Assert.That(constant.Value, Is.InstanceOf<NumberValue>());
        }

        [Test]
        [TestCase("constant")]
        [TestCase("CONSTANT")]
        [TestCase("cOnStAnT")]
        public void Should_CreateConstant_When_GivenNameOfExistingModule(string resourceName)
        {
            var program = new Program();
            program.AddModule("constant");

            Constant constant = null;
            Assert.DoesNotThrow(() => constant = program.AddConstant(resourceName, new NumberValue(1)));
            Assert.That(constant, Is.Not.Null);
            Assert.That(constant.Name, Is.EqualTo(resourceName));
            Assert.That(constant.Program, Is.SameAs(program));
            Assert.That(constant.Value, Is.InstanceOf<NumberValue>());
        }

        [Test]
        [TestCase("constant")]
        [TestCase("CONSTANT")]
        [TestCase("cOnStAnT")]
        public void ShouldNot_CreateConstant_When_GivenNameOfExistingConstant(string resourceName)
        {
            var program = new Program();
            program.AddConstant("constant", new NumberValue(1));

            Assert.Throws<ArgumentException>(() => program.AddConstant(resourceName, new NumberValue(1)));
        }

        [Test]
        public void Should_CreateModule_When_GivenName()
        {
            var program = new Program();

            Module module = null;
            Assert.DoesNotThrow(() => module = program.AddModule("module"));
            Assert.That(module, Is.Not.Null);
            Assert.That(module.Name, Is.EqualTo("module"));
            Assert.That(module.Program, Is.SameAs(program));
        }

        [Test]
        public void ShouldNot_CreateModule_When_GivenNullName()
        {
            var program = new Program();

            Assert.Throws<ArgumentNullException>(() => program.AddModule(null));
        }

        [Test]
        [TestCase("module")]
        [TestCase("MODULE")]
        [TestCase("mOdUlE")]
        public void Should_CreateModule_When_GivenNameOfExistingArgument(string resourceName)
        {
            var program = new Program();
            program.AddArgument("module");

            Module module = null;
            Assert.DoesNotThrow(() => module = program.AddModule(resourceName));
            Assert.That(module, Is.Not.Null);
            Assert.That(module.Name, Is.EqualTo(resourceName));
            Assert.That(module.Program, Is.SameAs(program));
        }

        [Test]
        [TestCase("module")]
        [TestCase("MODULE")]
        [TestCase("mOdUlE")]
        public void Should_CreateModule_When_GivenNameOfExistingConstant(string resourceName)
        {
            var program = new Program();
            program.AddConstant("module", new NumberValue(1));

            Module module = null;
            Assert.DoesNotThrow(() => module = program.AddModule(resourceName));
            Assert.That(module, Is.Not.Null);
            Assert.That(module.Name, Is.EqualTo(resourceName));
            Assert.That(module.Program, Is.SameAs(program));
        }

        [Test]
        [TestCase("module")]
        [TestCase("MODULE")]
        [TestCase("mOdUlE")]
        public void ShouldNot_CreateModule_When_GivenNameOfExistingModule(string resourceName)
        {
            var program = new Program();
            program.AddModule("module");

            Assert.Throws<ArgumentException>(() => program.AddModule(resourceName));
        }

        [Test]
        [TestCase("argument")]
        [TestCase("ARGUMENT")]
        [TestCase("aRgUmEnT")]
        public void Should_ReturnTrueWithArgument_When_RetrievingArgumentByName(string resourceName)
        {
            var program = new Program();
            var expected = program.AddArgument("argument");

            var success = program.TryGetArgument(resourceName, out var resource);

            Assert.That(success, Is.True);
            Assert.That(resource, Is.SameAs(expected));
        }

        [Test]
        public void ShouldNot_ReturnTrueWithArgument_When_RetrievingArgumentByWrongName()
        {
            var program = new Program();
            program.AddArgument("argument");

            var success = program.TryGetArgument("wrong name", out var resource);

            Assert.That(success, Is.False);
            Assert.That(resource, Is.Null);
        }

        [Test]
        public void ShouldNot_ReturnTrueWithArgument_When_RetrievingArgumentByOtherResourceName()
        {
            var program = new Program();
            program.AddConstant("argument", new NumberValue(1));

            var success = program.TryGetArgument("argument", out var resource);

            Assert.That(success, Is.False);
            Assert.That(resource, Is.Null);
        }

        [Test]
        [TestCase("constant")]
        [TestCase("CONSTANT")]
        [TestCase("cOnStAnT")]
        public void Should_ReturnTrueWithConstant_When_RetrievingConstantByName(string resourceName)
        {
            var program = new Program();
            var expected = program.AddConstant("constant", new NumberValue(1));

            var success = program.TryGetConstant(resourceName, out var resource);

            Assert.That(success, Is.True);
            Assert.That(resource, Is.SameAs(expected));
        }

        [Test]
        public void ShouldNot_ReturnTrueWithConstant_When_RetrievingConstantByWrongName()
        {
            var program = new Program();
            program.AddConstant("constant", new NumberValue(1));

            var success = program.TryGetConstant("wrong name", out var resource);

            Assert.That(success, Is.False);
            Assert.That(resource, Is.Null);
        }

        [Test]
        public void ShouldNot_ReturnTrueWithConstant_When_RetrievingConstantByOtherResourceName()
        {
            var program = new Program();
            program.AddModule("constant");

            var success = program.TryGetConstant("constant", out var constant);

            Assert.That(success, Is.False);
            Assert.That(constant, Is.Null);
        }

        [Test]
        [TestCase("module")]
        [TestCase("MODULE")]
        [TestCase("mOdUlE")]
        public void Should_ReturnTrueWithModule_When_RetrievingModuleByName(string resourceName)
        {
            var program = new Program();
            var expected = program.AddModule("module");

            var success = program.TryGetModule(resourceName, out var resource);

            Assert.That(success, Is.True);
            Assert.That(resource, Is.SameAs(expected));
        }

        [Test]
        public void ShouldNot_ReturnTrueWithModule_When_RetrievingModuleByWrongName()
        {
            var program = new Program();
            program.AddModule("module");

            var success = program.TryGetModule("wrong name", out var resource);

            Assert.That(success, Is.False);
            Assert.That(resource, Is.Null);
        }

        [Test]
        public void ShouldNot_ReturnTrueWithModule_When_RetrievingModuleByOtherResourceName()
        {
            var program = new Program();
            program.AddArgument("module");

            var success = program.TryGetModule("module", out var resource);

            Assert.That(success, Is.False);
            Assert.That(resource, Is.Null);
        }

        [Test]
        public void Should_CreateProgramNode_When_Empty()
        {
            var program = new Program();

            var result = program.ToSyntaxTree();

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Arguments, Is.Empty);
            Assert.That(result.Constants, Is.Empty);
            Assert.That(result.Modules, Is.Empty);
        }

        [Test]
        public void Should_CreateProgramNode_When_GivenSingleArgument()
        {
            var program = new Program();
            program.AddArgument("argument");

            var result = program.ToSyntaxTree();

            Assert.That(result.Arguments, Has.Length.EqualTo(1));
            var argument = result.Arguments.Single();
            Assert.That(argument.Name, Is.EqualTo("argument"));
        }

        [Test]
        public void Should_CreateProgramNode_When_GivenSingleConstant()
        {
            var program = new Program();
            program.AddConstant("constant", BooleanValue.True);

            var result = program.ToSyntaxTree();

            Assert.That(result.Constants, Has.Length.EqualTo(1));
            var constant = result.Constants.Single();
            Assert.That(constant.Name, Is.EqualTo("constant"));
        }

        [Test]
        public void Should_CreateProgramNode_When_GivenSingleModule()
        {
            var program = new Program();
            program.AddModule("module");

            var result = program.ToSyntaxTree();

            Assert.That(result.Modules, Has.Length.EqualTo(1));
            var module = result.Modules.Single();
            Assert.That(module.Name, Is.EqualTo("module"));
        }
    }
}
