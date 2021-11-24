// Copyright 2021 Cimpress plc.
// Licensed under the Apache License, Version 2.0
// http://www.apache.org/licenses/LICENSE-2.0

using System;
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

        [Test]
        public void Should_CreateArgumentDeclarationNode_When_Valid()
        {
            var program = new Program();
            var argument = program.AddArgument("argument");

            var result = argument.ToSyntaxTree();

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo("argument"));
        }
    }
}