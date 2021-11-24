// Copyright 2021 Cimpress plc.
// Licensed under the Apache License, Version 2.0
// http://www.apache.org/licenses/LICENSE-2.0

namespace Cimpress.Cimbol.Compiler.Scan
{
    /// <summary>
    /// The set of methods to use with the <see cref="Scanner"/> for ignoring whitespace.
    /// </summary>
    public partial class Scanner
    {
        /// <summary>
        /// Scans the <see cref="ScanningContext"/> and ignores the next set of whitespace characters.
        /// </summary>
        public void IgnoreWhitespace()
        {
            while (!_context.EndOfFile)
            {
                if (IsWhitespace(_context.Peek()))
                {
                    _context.Advance();
                }
                else
                {
                    break;
                }
            }

            _context.Skip();
        }

        private bool IsWhitespace(string character)
        {
            switch (character)
            {
                case " ":
                case "\f":
                case "\n":
                case "\r":
                case "\t":
                    return true;

                default:
                    return false;
            }
        }
    }
}