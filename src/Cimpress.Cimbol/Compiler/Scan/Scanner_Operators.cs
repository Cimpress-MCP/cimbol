// Copyright 2021 Cimpress plc.
// Licensed under the Apache License, Version 2.0
// http://www.apache.org/licenses/LICENSE-2.0

using System.Collections.Immutable;
using Cimpress.Cimbol.Exceptions;

namespace Cimpress.Cimbol.Compiler.Scan
{
    /// <summary>
    /// The set of methods to use with the <see cref="Scanner"/> for parsing operators.
    /// </summary>
    public partial class Scanner
    {
        private static readonly IImmutableSet<string> OperatorCharacters = new[]
        {
            "+", "-", "*", "/", "^", "%", "=", "!", "<", ">",
        }.ToImmutableHashSet();

        /// <summary>
        /// Scans the <see cref="ScanningContext"/> and returns the next operator <see cref="Token"/>.
        /// </summary>
        /// <returns>The next <see cref="Token"/> in the stream.</returns>
        public Token NextOperator()
        {
            switch (_context.Peek())
            {
                case "+":
                    return ScanOperatorPlus();

                case "-":
                    _context.Advance();
                    return _context.Consume(TokenType.Subtract);

                case "*":
                    _context.Advance();
                    return _context.Consume(TokenType.Multiply);

                case "/":
                    _context.Advance();
                    return _context.Consume(TokenType.Divide);

                case "^":
                    _context.Advance();
                    return _context.Consume(TokenType.Power);

                case "%":
                    _context.Advance();
                    return _context.Consume(TokenType.Remainder);

                case "=":
                    return ScanOperatorEqual();

                case "!":
                    return ScanOperatorNot();

                case "<":
                    return ScanOperatorLessThan();

                case ">":
                    return ScanOperatorGreaterThan();

                default:
                    throw CimbolCompilationException.UnrecognizedOperatorError(
                        FormulaName,
                        _context.Start(),
                        _context.End(),
                        _context.Current);
            }
        }

        /// <summary>
        /// Checks if the given character is possibly part of an operator.
        /// </summary>
        /// <param name="character">The character to check.</param>
        /// <returns>True if the given character is possibly part of an operator.</returns>
        private static bool IsOperatorCharacter(string character)
        {
            return OperatorCharacters.Contains(character);
        }

        private Token ScanOperatorPlus()
        {
            _context.Advance();

            switch (_context.Peek())
            {
                case "+":
                    _context.Advance();
                    return _context.Consume(TokenType.Concatenate);

                default:
                    return _context.Consume(TokenType.Add);
            }
        }

        private Token ScanOperatorEqual()
        {
            _context.Advance();

            switch (_context.Peek())
            {
                case "=":
                    _context.Advance();
                    return _context.Consume(TokenType.Equal);

                default:
                    return _context.Consume(TokenType.Assign);
            }
        }

        private Token ScanOperatorNot()
        {
            _context.Advance();

            switch (_context.Peek())
            {
                case "=":
                    _context.Advance();
                    return _context.Consume(TokenType.NotEqual);

                default:
                    throw CimbolCompilationException.UnrecognizedOperatorError(
                        FormulaName,
                        _context.Start(),
                        _context.End(),
                        _context.Current);
            }
        }

        private Token ScanOperatorLessThan()
        {
            _context.Advance();

            switch (_context.Peek())
            {
                case "=":
                    _context.Advance();
                    return _context.Consume(TokenType.LessThanEqual);

                default:
                    return _context.Consume(TokenType.LessThan);
            }
        }

        private Token ScanOperatorGreaterThan()
        {
            _context.Advance();

            switch (_context.Peek())
            {
                case "=":
                    _context.Advance();
                    return _context.Consume(TokenType.GreaterThanEqual);

                default:
                    return _context.Consume(TokenType.GreaterThan);
            }
        }
    }
}