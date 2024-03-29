﻿// Copyright 2021 Cimpress plc.
// Licensed under the Apache License, Version 2.0
// http://www.apache.org/licenses/LICENSE-2.0

using System.Linq;
using Cimpress.Cimbol.Compiler.Scan;
using Cimpress.Cimbol.Utilities;

namespace Cimpress.Cimbol.UnitTests.Compiler.Parse
{
    public static class ParseTestUtilities
    {
        public static TokenStream CreateTokenStream(params Token[] tokens)
        {
            var position = new Position(0, 0);
            var endOfFileToken = new Token(string.Empty, TokenType.EndOfFile, position, position);
            return new TokenStream(tokens.Append(endOfFileToken));
        }
    }
}