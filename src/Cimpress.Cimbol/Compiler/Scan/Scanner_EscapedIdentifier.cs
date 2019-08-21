using Cimpress.Cimbol.Exceptions;

namespace Cimpress.Cimbol.Compiler.Scan
{
    /// <summary>
    /// The set of methods to use with the <see cref="Scanner"/> for parsing escaped identifiers.
    /// </summary>
    public partial class Scanner
    {
        /// <summary>
        /// Scans the <see cref="ScanningContext"/> and returns the next escaped identifier <see cref="Token"/>.
        /// </summary>
        /// <returns>The next <see cref="Token"/> in the stream.</returns>
        public Token NextEscapedIdentifier()
        {
            // Matches the regex /'/
            if (_context.Peek() != "'")
            {
                // Escaped identifiers must start with a single quote.
                throw CimbolCompilationException.IdentifierQuoteStartError(
                    FormulaName,
                    _context.Start(),
                    _context.End());
            }

            _context.Advance();

            while (!_context.EndOfFile)
            {
                switch (_context.Peek())
                {
                    case "'":
                        _context.Advance();
                        return _context.Consume(TokenType.Identifier);

                    case "\\":
                        ScanEscapeSequence();
                        break;

                    case "\n":
                    case "\r":
                        // Escaped identifiers cannot have new lines in them.
                        throw CimbolCompilationException.IdentifierNewLineError(
                            FormulaName,
                            _context.Start(),
                            _context.End());

                    default:
                        _context.Advance();
                        break;
                }
            }

            // Unexpected end of file reached while lexing a string.
            throw CimbolCompilationException.UnexpectedEndOfFileError(FormulaName, _context.Start(), _context.End());
        }
    }
}