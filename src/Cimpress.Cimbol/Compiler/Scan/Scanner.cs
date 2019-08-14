using System;
using Cimpress.Cimbol.Compiler.Source;

namespace Cimpress.Cimbol.Compiler.Scan
{
    /// <summary>
    /// The lexer responsible for converting a <see cref="SourceText"/> object into a stream of <see cref="Token"/> objects.
    /// </summary>
    public partial class Scanner
    {
        private readonly ScanningContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="Scanner"/> class.
        /// </summary>
        /// <param name="sourceText">The <see cref="SourceText"/> to scan.</param>
        public Scanner(SourceText sourceText)
        {
            _context = new ScanningContext(sourceText);
        }

        /// <summary>
        /// Scans the source and returns the next <see cref="Token"/>.
        /// </summary>
        /// <returns>The next <see cref="Token"/> in the stream.</returns>
        public Token Next()
        {
            if (_context.EndOfFile)
            {
                return _context.ConsumeEndOfFile();
            }

            if (IsWhitespace(_context.Peek()))
            {
                IgnoreWhitespace();
            }

            switch (_context.Peek())
            {
                case "(":
                    _context.Advance();
                    return _context.Consume(TokenType.LeftParenthesis);

                case ")":
                    _context.Advance();
                    return _context.Consume(TokenType.RightParenthesis);

                case ".":
                    _context.Advance();
                    return _context.Consume(TokenType.Period);

                case ",":
                    _context.Advance();
                    return _context.Consume(TokenType.Comma);

                case "'":
                    return NextEscapedIdentifier();

                case "\"":
                    return NextStringLiteral();

                case string character when IsOperatorCharacter(character):
                    return NextOperator();

                case string character when IsBareWordStart(character):
                    return NextBareWord();

                case string character when IsNumber(character):
                    return NextNumberLiteral();
            }

            throw new NotSupportedException("ErrorCode016");
        }
    }
}