﻿using System;
using System.Linq;
using Cimpress.Cimbol.Compiler.SyntaxTree;
using NUnit.Framework;

namespace Cimpress.Cimbol.UnitTests.Compiler.SyntaxTree
{
    [TestFixture]
    public class UnaryOpTypeTests
    {
        [Test]
        public void Should_ReturnOperator_When_GivenValidOpType()
        {
            foreach (var opType in Enum.GetValues(typeof(UnaryOpType)).Cast<UnaryOpType>())
            {
                string result = null;
                Assert.DoesNotThrow(() => result = opType.GetOperator());
                Assert.IsNotNull(result);
            }
        }

        [Test]
        public void Should_ThrowException_When_GivenInvalidOpType()
        {
            var fakeOpType = (UnaryOpType)int.MaxValue;
            Assert.Throws<ArgumentOutOfRangeException>(() => fakeOpType.GetOperator());
        }
    }
}