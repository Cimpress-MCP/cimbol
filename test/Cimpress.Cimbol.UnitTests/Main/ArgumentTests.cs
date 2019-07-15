﻿using System;
using NUnit.Framework;

namespace Cimpress.Cimbol.UnitTests.Main
{
    [TestFixture]
    public class ArgumentTests
    {
        [Test]
        public void Should_ThrowArgumentNullException_When_InitializedWithNullProgram()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var argument = new Argument(null, "argument");
            });
        }

        [Test]
        public void Should_ThrowArgumentNullException_When_InitializedWithNullName()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var argument = new Argument(new Program(), null);
            });
        }

        [Test]
        public void Should_RetrieveSameProgram_When_InitializedWithProgram()
        {
            var program = new Program();
            var argument = new Argument(program, "argument");
            Assert.That(argument.Program, Is.SameAs(program));
        }

        [Test]
        public void Should_RetrieveSameName_When_InitializedWithName()
        {
            var name = "argument";
            var argument = new Argument(new Program(), name);
            Assert.That(argument.Name, Is.SameAs(name));
        }
    }
}