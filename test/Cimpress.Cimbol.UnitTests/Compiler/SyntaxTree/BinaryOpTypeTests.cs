// Copyright 2021 Cimpress plc.
// Licensed under the Apache License, Version 2.0
// http://www.apache.org/licenses/LICENSE-2.0

using System;
using System.Linq;
using Cimpress.Cimbol.Compiler.SyntaxTree;
using NUnit.Framework;

namespace Cimpress.Cimbol.UnitTests.Compiler.SyntaxTree
{
    [TestFixture]
    public class BinaryOpTypeTests
    {
        [Test]
        public void Should_ReturnOperator_When_GivenValidOpType()
        {
            foreach (var opType in Enum.GetValues(typeof(BinaryOpType)).Cast<BinaryOpType>())
            {
                string result = null;
                Assert.DoesNotThrow(() => result = opType.GetOperator());
                Assert.IsNotNull(result);
            }
        }

        [Test]
        public void Should_ThrowException_When_GivenInvalidOpType()
        {
            var fakeOpType = (BinaryOpType)int.MaxValue;
            Assert.Throws<ArgumentOutOfRangeException>(() => fakeOpType.GetOperator());
        }
    }
}