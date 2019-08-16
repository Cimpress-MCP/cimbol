using System;

namespace Cimpress.Cimbol.Compiler.Scan
{
    /// <summary>
    /// The set of methods to use with the <see cref="Scanner"/> for parsing numbers.
    /// </summary>
    public partial class Scanner
    {
        /// <summary>
        /// Scans the <see cref="ScanningContext"/> and returns the next identifier <see cref="Token"/>.
        /// </summary>
        /// <returns>The next <see cref="Token"/> in the stream.</returns>
        public Token NextNumberLiteral()
        {
            switch (_context.Peek())
            {
                case string character when IsNumber(character):
                    ScanIntegerPart();
                    return _context.Consume(TokenType.NumberLiteral);

                case ".":
                    ScanDecimalOnlyPart();
                    return _context.Consume(TokenType.NumberLiteral);

                default:
                    // Numbers must start with either a decimal digit or a period.
#pragma warning disable CA1303
                    throw new NotSupportedException("ErrorCode023");
#pragma warning restore CA1303
            }
        }

        private static bool IsExponentSign(string character)
        {
            return character == "e" || character == "E";
        }

        private static bool IsNumber(string character)
        {
            if (character.Length == 1)
            {
                var characterCode = char.ConvertToUtf32(character, 0);
                return characterCode >= '0' && characterCode <= '9';
            }

            return false;
        }

        private static bool IsSign(string character)
        {
            return character == "+" || character == "-";
        }

        private void ScanIntegerPart()
        {
            // Matches /[0-9]?/, implicitly matches [0-9]+
            ScanNumbers();

            // Matches /\.[0-9]*([eE][+-]?[0-9]{1-3})?/
            if (_context.Peek() == ".")
            {
                ScanDecimalPart();
            }

            // Matches /([eE][+-]?[0-9]{1-3})?/
            else if (IsExponentSign(_context.Peek()))
            {
                ScanExponentPart();
            }
        }

        private void ScanDecimalPart()
        {
            // Matches /./, implicitly matches /\./
            _context.Advance();

            // Matches regex /[0-9]*/
            if (IsNumber(_context.Peek()))
            {
                ScanNumbers();
            }

            // Matches regex /([eE][+-]?[0-9]{1-3})?/
            if (IsExponentSign(_context.Peek()))
            {
                ScanExponentPart();
            }
        }

        private void ScanDecimalOnlyPart()
        {
            // Assuming the period has already been checked for, this matches /\./
            _context.Advance();

            // Matches regex /[0-9]+/
            if (IsNumber(_context.Peek()))
            {
                ScanNumbers();
            }
            else
            {
                // DecimalOnlyPart needs at least one number, otherwise strings like "-.e2" will parse as a number.
#pragma warning disable CA1303
                throw new NotSupportedException("ErrorCode024");
#pragma warning restore CA1303
            }

            // Matches regex /([eE][+-]?[0-9]{1-3})?/
            if (IsExponentSign(_context.Peek()))
            {
                ScanExponentPart();
            }
        }

        private void ScanExponentPart()
        {
            _context.Advance();

            // Matches regex /[+-]?/
            if (IsSign(_context.Peek()))
            {
                _context.Advance();
            }

            // Matches regex /[0-9]/
            if (IsNumber(_context.Peek()))
            {
                _context.Advance();
            }
            else
            {
                // ExponentPart needs at least one number, otherwise strings like "-2.e" will parse as a number.
#pragma warning disable CA1303
                throw new NotSupportedException("ErrorCode025");
#pragma warning restore CA1303
            }

            // Matches regex /[0-9]?/
            if (IsNumber(_context.Peek()))
            {
                _context.Advance();
            }

            // Matches regex /[0-9]?/
            if (IsNumber(_context.Peek()))
            {
                _context.Advance();
            }

            // Supplements previous regex with error checking.
            if (IsNumber(_context.Peek()))
            {
                // Don't support four or more digits in the exponent.
#pragma warning disable CA1303
                throw new NotSupportedException("ErrorCode026");
#pragma warning restore CA1303
            }
        }

        private void ScanNumbers()
        {
            while (!_context.EndOfFile)
            {
                if (IsNumber(_context.Peek()))
                {
                    _context.Advance();
                }
                else
                {
                    break;
                }
            }
        }
    }
}