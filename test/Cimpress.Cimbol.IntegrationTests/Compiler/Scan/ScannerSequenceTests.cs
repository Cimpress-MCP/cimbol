// Copyright 2021 Cimpress plc.
// Licensed under the Apache License, Version 2.0
// http://www.apache.org/licenses/LICENSE-2.0

using System.Collections;
using System.Collections.Generic;
using Cimpress.Cimbol.Compiler.Scan;
using Cimpress.Cimbol.Compiler.Source;
using NUnit.Framework;

namespace Cimpress.Cimbol.IntegrationTests.Compiler.Scan
{
    [TestFixture]
    public class ScannerSequenceTests
    {
        public static IEnumerable TestCases
        {
            get
            {
                yield return new TestCaseData(
                    "abc + 11 * 'xyz'",
                    Tokens(
                        TokenType.Identifier,
                        TokenType.Add,
                        TokenType.NumberLiteral,
                        TokenType.Multiply,
                        TokenType.Identifier,
                        TokenType.EndOfFile));

                yield return new TestCaseData(
                    "if(3+5, then = \"cat\", else = \"dog\")",
                    Tokens(
                        TokenType.IfKeyword,
                        TokenType.LeftParenthesis,
                        TokenType.NumberLiteral,
                        TokenType.Add,
                        TokenType.NumberLiteral,
                        TokenType.Comma,
                        TokenType.Identifier,
                        TokenType.Assign,
                        TokenType.StringLiteral,
                        TokenType.Comma,
                        TokenType.Identifier,
                        TokenType.Assign,
                        TokenType.StringLiteral,
                        TokenType.RightParenthesis,
                        TokenType.EndOfFile));

                yield return new TestCaseData(
                    "list('order'.'quantity', 500)",
                    Tokens(
                        TokenType.ListKeyword,
                        TokenType.LeftParenthesis,
                        TokenType.Identifier,
                        TokenType.Period,
                        TokenType.Identifier,
                        TokenType.Comma,
                        TokenType.NumberLiteral,
                        TokenType.RightParenthesis,
                        TokenType.EndOfFile));
            }
        }

        [Test]
        [TestCaseSource(typeof(ScannerSequenceTests), nameof(TestCases))]
        public void Should_MatchSequence_When_GivenSource(string source, TokenType[] types)
        {
            var scanner = new Scanner("formula", new SourceText("formula", source));

            var nextToken = scanner.Next();
            var tokens = new List<TokenType> { nextToken.Type };

            while (nextToken.Type != TokenType.EndOfFile)
            {
                nextToken = scanner.Next();
                tokens.Add(nextToken.Type);
            }

            Assert.That(types, Is.EqualTo(tokens).AsCollection);
        }

        private static TokenType[] Tokens(params TokenType[] types)
        {
            return types;
        }
    }
}