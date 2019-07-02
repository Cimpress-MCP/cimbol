using System;

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
                // String literals must start with a single quote.
                throw new NotSupportedException();
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
                        throw new NotSupportedException();

                    default:
                        _context.Advance();
                        break;
                }
            }

            // Unexpected end of file reached while lexing a string.
            throw new NotSupportedException();
        }
    }
}