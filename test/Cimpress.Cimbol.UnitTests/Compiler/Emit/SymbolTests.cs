// Copyright 2021 Cimpress plc.
// Licensed under the Apache License, Version 2.0
// http://www.apache.org/licenses/LICENSE-2.0

using System;
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
        public void Should_ThrowArgumentNullException_When_ConstructedWithNullParentSymbol()
        {
            Assert.Throws<ArgumentNullException>(() => new Symbol("Symbol01", (Symbol)null));
        }

        [Test]
        public void Should_InitializeParameterExpression_When_ConstructedWithType()
        {
            var symbol = new Symbol("test", typeof(ILocalValue));

            var result = symbol.Variable;

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<ParameterExpression>());
            Assert.That(result.Name, Is.EqualTo("test"));
            Assert.That(result.Type, Is.EqualTo(typeof(ILocalValue)));
        }

        [Test]
        public void Should_InitializeAsOriginal_When_ConstructedWithType()
        {
            var result = new Symbol("test", typeof(ILocalValue));

            Assert.That(result.IsReference, Is.False);
        }

        [Test]
        public void Should_InitializeParameterExpression_When_ConstructedWithSymbol()
        {
            var original = new Symbol("test1", typeof(ILocalValue));
            var symbol = new Symbol("test2", original);

            var result = symbol.Variable;

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<ParameterExpression>());
            Assert.That(result, Is.SameAs(original.Variable));
            Assert.That(result.Name, Is.EqualTo("test1"));
            Assert.That(result.Type, Is.EqualTo(typeof(ILocalValue)));
        }

        [Test]
        public void Should_InitializeAsOriginal_When_ConstructedWithSymbol()
        {
            var original = new Symbol("test", typeof(ILocalValue));
            var result = new Symbol("test", original);

            Assert.That(result.IsReference, Is.True);
        }
    }
}