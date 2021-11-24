// Copyright 2021 Cimpress plc.
// Licensed under the Apache License, Version 2.0
// http://www.apache.org/licenses/LICENSE-2.0

using Cimpress.Cimbol.Compiler.Source;
using Cimpress.Cimbol.Exceptions;

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
        /// <param name="formulaName">The name of the formula being operated on.</param>
        /// <param name="sourceText">The <see cref="SourceText"/> to scan.</param>
        public Scanner(string formulaName, SourceText sourceText)
        {
            FormulaName = formulaName;

            _context = new ScanningContext(formulaName, sourceText);
        }

        /// <summary>
        /// The name of the formula being operator on.
        /// </summary>
        public string FormulaName { get; }

        /// <summary>
        /// Scans the source and returns the next <see cref="Token"/>.
        /// </summary>
        /// <returns>The next <see cref="Token"/> in the stream.</returns>
        public Token Next()
        {
            if (IsWhitespace(_context.Peek()))
            {
                IgnoreWhitespace();
            }

            if (_context.EndOfFile)
            {
                return _context.ConsumeEndOfFile();
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

            throw CimbolCompilationException.UnrecognizedCharacterError(
                FormulaName,
                _context.Start(),
                _context.End(),
                _context.Peek());
        }
    }
}