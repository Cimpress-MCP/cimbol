// Copyright 2021 Cimpress plc.
// Licensed under the Apache License, Version 2.0
// http://www.apache.org/licenses/LICENSE-2.0

using Cimpress.Cimbol.Exceptions;

namespace Cimpress.Cimbol.Compiler.Scan
{
    /// <summary>
    /// The set of methods to use with the <see cref="Scanner"/> for parsing strings.
    /// </summary>
    public partial class Scanner
    {
        /// <summary>
        /// Scans the <see cref="ScanningContext"/> and returns the next string <see cref="Token"/>.
        /// </summary>
        /// <returns>The next <see cref="Token"/> in the stream.</returns>
        public Token NextStringLiteral()
        {
            // Matches the regex /"/
            if (_context.Peek() != "\"")
            {
                // String literals must start with a double quote.
                throw CimbolCompilationException.StringQuoteStartError(FormulaName, _context.Start(), _context.End());
            }

            _context.Advance();

            while (!_context.EndOfFile)
            {
                switch (_context.Peek())
                {
                    case "\"":
                        _context.Advance();
                        return _context.Consume(TokenType.StringLiteral);

                    case "\\":
                        ScanEscapeSequence();
                        break;

                    case "\n":
                    case "\r":
                        // String literals cannot have new lines in them.
                        throw CimbolCompilationException.StringNewLineError(
                            FormulaName,
                            _context.Start(),
                            _context.End());

                    default:
                        _context.Advance();
                        break;
                }
            }

            // Unexpected end of file reached while lexing a string.
            throw CimbolCompilationException.UnexpectedEndOfFileError(
                FormulaName,
                _context.Start(),
                _context.End());
        }
    }
}