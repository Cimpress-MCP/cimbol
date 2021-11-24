// Copyright 2021 Cimpress plc.
// Licensed under the Apache License, Version 2.0
// http://www.apache.org/licenses/LICENSE-2.0

using System.Text.RegularExpressions;
using Cimpress.Cimbol.Exceptions;

namespace Cimpress.Cimbol.Compiler.Scan
{
    /// <summary>
    /// The set of methods to use with the <see cref="Scanner"/> for parsing escape sequences.
    /// </summary>
    public partial class Scanner
    {
        private static readonly Regex HexRegex = new Regex("[0-9A-Fa-f]");

        private void ScanEscapeSequence()
        {
            // Matches the regex /./, implicitly matches the regex /\\/
            _context.Advance();

            switch (_context.Peek())
            {
                // Matches the regex /[nrt\\"']/
                case "n":
                case "r":
                case "t":
                case "\\":
                case "\"":
                case "'":
                    _context.Advance();
                    break;

                // Matches the regex /u[A-Fa-f0-9]{4}/
                case "u":
                    ScanUnicodeEscapeSequence(4);
                    break;

                // Matches the regex /u[A-Fa-f0-9]{8}/
                case "U":
                    ScanUnicodeEscapeSequence(8);
                    break;

                default:
                    // Unrecognized escape sequence.
                    throw CimbolCompilationException.UnrecognizedEscapeSequenceError(
                        FormulaName,
                        _context.Start(),
                        _context.End(),
                        _context.Current);
            }
        }

        private void ScanUnicodeEscapeSequence(int count)
        {
            // Matches the regex /./, implicitly matches the regex /[uU]/
            _context.Advance();

            // Matches the regex /[0-9A-Fa-f]{count}
            for (var i = 0; i < count; ++i)
            {
                if (!HexRegex.IsMatch(_context.Peek()))
                {
                    // The encountered character is not valid hexadecimal.
                    throw CimbolCompilationException.InvalidHexadecimalSequenceError(
                        FormulaName,
                        _context.Start(),
                        _context.End());
                }

                _context.Advance();
            }
        }
    }
}