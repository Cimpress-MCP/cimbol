// Copyright 2021 Cimpress plc.
// Licensed under the Apache License, Version 2.0
// http://www.apache.org/licenses/LICENSE-2.0

using System;
using Cimpress.Cimbol.Compiler.Scan;
using Cimpress.Cimbol.Utilities;
using NUnit.Framework;

namespace Cimpress.Cimbol.UnitTests.Compiler.Scan
{
    [TestFixture]
    public class TokenStreamTests
    {
        [Test]
        public void Should_Initialize_When_LookaheadSizeSetToZero()
        {
            var tokens = new[] { Token(TokenType.Add) };

            Assert.DoesNotThrow(() =>
            {
                var tokenStream = new TokenStream(tokens, 0);
                Assert.AreEqual(TokenType.Add, tokenStream.Current().Type);
            });
        }

        [Test]
        public void Should_Initialize_When_LookaheadSizeSetToOne()
        {
            var tokens = new[] { Token(TokenType.Add), Token(TokenType.Subtract) };

            Assert.DoesNotThrow(() =>
            {
                var tokenStream = new TokenStream(tokens, 1);
                Assert.AreEqual(TokenType.Add, tokenStream.Current().Type);
            });
        }

        [Test]
        public void Should_Initialize_When_LookaheadSizeSetToTwo()
        {
            var tokens = new[] { Token(TokenType.Add), Token(TokenType.Subtract), Token(TokenType.Subtract) };

            Assert.DoesNotThrow(() =>
            {
                var tokenStream = new TokenStream(tokens, 2);
                Assert.AreEqual(TokenType.Add, tokenStream.Current().Type);
            });
        }

        [Test]
        public void Should_Initialize_When_LookaheadLargerThanStream()
        {
            var tokens = new[] { Token(TokenType.Add) };

            Assert.DoesNotThrow(() =>
            {
                var tokenStream = new TokenStream(tokens, 10);
                Assert.AreEqual(TokenType.Add, tokenStream.Current().Type);
            });
        }

        [Test]
        public void Should_Lookahead_When_NotEndOfStream()
        {
            var tokens = new[] { Token(TokenType.Add), Token(TokenType.Subtract) };

            var tokenStream = new TokenStream(tokens, 1);
            Assert.AreEqual(TokenType.Subtract, tokenStream.Lookahead(1).Type);
        }

        [Test]
        public void Should_Lookahead_When_EndOfStream()
        {
            var tokens = new[] { Token(TokenType.Add) };

            var tokenStream = new TokenStream(tokens, 1);
            Assert.AreEqual(null, tokenStream.Lookahead(1));
        }

        [Test]
        public void Should_Advance_When_NotEndOfStream()
        {
            var tokens = new[] { Token(TokenType.Add), Token(TokenType.Subtract), Token(TokenType.Multiply) };

            var tokenStream = new TokenStream(tokens, 0);
            Assert.AreEqual(TokenType.Add, tokenStream.Current().Type);
            Assert.AreEqual(true, tokenStream.Next());
            Assert.AreEqual(TokenType.Subtract, tokenStream.Current().Type);
        }

        [Test]
        public void Should_Advance_When_EndOfStream()
        {
            var tokens = new[] { Token(TokenType.Add) };

            var tokenStream = new TokenStream(tokens, 0);
            Assert.AreEqual(TokenType.Add, tokenStream.Current().Type);
            Assert.AreEqual(false, tokenStream.Next());
            Assert.AreEqual(null, tokenStream.Current());
        }

        [Test]
        public void ShouldNot_Lookahead_When_OutsideLookaheadRange()
        {
            var tokens = new[] { Token(TokenType.Add) };

            var tokenStream = new TokenStream(tokens, 0);
            Assert.Throws<ArgumentOutOfRangeException>(() => tokenStream.Lookahead(1));
        }

        private Token Token(TokenType tokenType)
        {
            return new Token(string.Empty, tokenType, new Position(0, 0), new Position(0, 1));
        }
    }
}
