using System;
using System.Globalization;

namespace Cimpress.Cimbol.Compiler.Scan
{
    /// <summary>
    /// The set of methods to use with the <see cref="Scanner"/> for parsing bare words.
    /// A bare word is an identifier or keyword that has not been escaped.
    /// </summary>
    public partial class Scanner
    {
        /// <summary>
        /// Scans the <see cref="ScanningContext"/> and returns the next identifier or keyword <see cref="Token"/>.
        /// </summary>
        /// <returns>The next <see cref="Token"/> in the stream.</returns>
        public Token NextBareWord()
        {
            if (!IsBareWordStart(_context.Peek()))
            {
                throw new NotSupportedException();
            }

            ScanBareWord();

            switch (_context.Current.ToUpperInvariant())
            {
                case "AND":
                    return _context.Consume(TokenType.And);

                case "AWAIT":
                    return _context.Consume(TokenType.AwaitKeyword);

                case "FALSE":
                    return _context.Consume(TokenType.FalseKeyword);

                case "IF":
                    return _context.Consume(TokenType.IfKeyword);

                case "LIST":
                    return _context.Consume(TokenType.ListKeyword);

                case "NOT":
                    return _context.Consume(TokenType.Not);

                case "OBJECT":
                    return _context.Consume(TokenType.ObjectKeyword);

                case "OR":
                    return _context.Consume(TokenType.Or);

                case "TRUE":
                    return _context.Consume(TokenType.TrueKeyword);

                case "WHERE":
                    return _context.Consume(TokenType.WhereKeyword);

                case "XOR":
                    return _context.Consume(TokenType.Xor);

                default:
                    return _context.Consume(TokenType.Identifier);
            }
        }

        /// <summary>
        /// Checks if the given character is possibly part of the start of an identifier or keyword.
        /// </summary>
        /// <param name="character">The character to check.</param>
        /// <returns>True if the given character is possibly part of the start of an identifier.</returns>
        private static bool IsBareWordStart(string character)
        {
            var category = CharUnicodeInfo.GetUnicodeCategory(character, 0);
            return character == "_" || IsLClass(category);
        }

        /// <summary>
        /// Checks if the given character is possibly part of an identifier.
        /// </summary>
        /// <param name="character">The character to check.</param>
        /// <returns>True if the given character is possibly part of an identifier.</returns>
        private static bool IsBareWordNonStart(string character)
        {
            var category = CharUnicodeInfo.GetUnicodeCategory(character, 0);
            return character == "_" || IsLClass(category) || IsNClass(category);
        }

        private static bool IsLClass(UnicodeCategory category)
        {
            return category == UnicodeCategory.LowercaseLetter
                   || category == UnicodeCategory.ModifierLetter
                   || category == UnicodeCategory.OtherLetter
                   || category == UnicodeCategory.TitlecaseLetter
                   || category == UnicodeCategory.UppercaseLetter;
        }

        private static bool IsNClass(UnicodeCategory category)
        {
            return category == UnicodeCategory.DecimalDigitNumber
                   || category == UnicodeCategory.LetterNumber
                   || category == UnicodeCategory.OtherNumber;
        }

        private void ScanBareWord()
        {
            while (!_context.EndOfFile)
            {
                switch (_context.Peek())
                {
                    case string character when IsBareWordNonStart(character):
                        _context.Advance();
                        break;

                    default:
                        return;
                }
            }
        }
    }
}