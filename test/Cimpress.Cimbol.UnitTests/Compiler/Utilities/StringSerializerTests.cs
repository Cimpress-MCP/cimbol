// Copyright 2021 Cimpress plc.
// Licensed under the Apache License, Version 2.0
// http://www.apache.org/licenses/LICENSE-2.0

using System;
using Cimpress.Cimbol.Exceptions;
using Cimpress.Cimbol.Utilities;
using NUnit.Framework;

namespace Cimpress.Cimbol.UnitTests.Compiler.Utilities
{
    [TestFixture]
    public class StringSerializerTests
    {
        [Test]
        [TestCase("\"abc\"", "abc")]
        [TestCase("\"_xyz\"", "_xyz")]
        [TestCase("\"_123\"", "_123")]
        [TestCase("\"123\"", "123")]
        [TestCase("\"my dog\"", "my dog")]
        [TestCase("\"\\\"\"", "\"")]
        [TestCase("\"\\'\"", "'")]
        [TestCase("\"\\\\\"", "\\")]
        [TestCase("\"\\n\"", "\n")]
        [TestCase("\"\\r\"", "\r")]
        [TestCase("\"\\t\"", "\t")]
        [TestCase("\"\\u0041\"", "A")]
        [TestCase("\"\\U00000041\"", "A")]
        public void Should_DeserializeToString_When_String(string given, string expected)
        {
            var actual = StringSerializer.DeserializeString(given);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        [TestCase("123")]
        [TestCase("my dog")]
        [TestCase("\n")]
        [TestCase("\"123")]
        [TestCase("\"my dog")]
        [TestCase("\"\n")]
        public void ShouldNot_DeserializeToString_When_NotString(string given)
        {
            Assert.Throws<CimbolInternalException>(() => StringSerializer.DeserializeString(given));
        }

        [Test]
        [TestCase("test", "\"test\"")]
        [TestCase("test spaces", "\"test spaces\"")]
        [TestCase("\n", "\"\\n\"")]
        [TestCase("\r", "\"\\r\"")]
        [TestCase("\t", "\"\\t\"")]
        [TestCase("\"", "\"\\\"\"")]
        [TestCase("'", "\"\\'\"")]
        public void Should_SerializeToString_When_String(string given, string expected)
        {
            var actual = StringSerializer.SerializeString(given);
            Assert.AreEqual(expected, actual);
        }
    }
}