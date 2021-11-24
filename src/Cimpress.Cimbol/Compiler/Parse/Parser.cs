// Copyright 2021 Cimpress plc.
// Licensed under the Apache License, Version 2.0
// http://www.apache.org/licenses/LICENSE-2.0

using System;
using Cimpress.Cimbol.Compiler.Scan;
using Cimpress.Cimbol.Compiler.SyntaxTree;
using Cimpress.Cimbol.Exceptions;

namespace Cimpress.Cimbol.Compiler.Parse
{
    /// <summary>
    /// The parser responsible for converting a stream of <see cref="Token"/> objects into a tree of <see cref="ISyntaxNode"/> objects.
    /// </summary>
    public partial class Parser
    {
        private readonly TokenStream _tokenStream;

        /// <summary>
        /// Initializes a new instance of the <see cref="Parser"/> class.
        /// </summary>
        /// <param name="formulaName">The name of the formula being parsed.</param>
        /// <param name="tokenStream">The stream of tokens to parse.</param>
        public Parser(string formulaName, TokenStream tokenStream)
        {
            FormulaName = formulaName;

            _tokenStream = tokenStream ?? throw new ArgumentNullException(nameof(tokenStream));
        }

        /// <summary>
        /// The name of the formula being parsed.
        /// </summary>
        public string FormulaName { get; }

        /// <summary>
        /// Parse a series of <see cref="Token"/> objects into an expression.
        /// </summary>
        /// <returns>A syntax tree node representing an expression.</returns>
        public IExpressionNode Expression()
        {
            if (Lookahead(0) == TokenType.AwaitKeyword)
            {
                Match(TokenType.AwaitKeyword);

                var expression = LogicalAnd();

                return new UnaryOpNode(UnaryOpType.Await, expression);
            }
            else
            {
                var expression = LogicalAnd();

                return expression;
            }
        }

        /// <summary>
        /// Parse of a series of <see cref="Token"/> objects into an expression tree.
        /// </summary>
        /// <returns>A syntax tree node representing an expression.</returns>
        public IExpressionNode Root()
        {
            var expression = Expression();

            Match(TokenType.EndOfFile);

            return expression;
        }

        private TokenType Lookahead(int lookahead)
        {
            var current = _tokenStream.Lookahead(lookahead);

            if (current == null)
            {
                return TokenType.EndOfFile;
            }

            return current.Type;
        }

        private Token Match(TokenType tokenType, string errorMessage = null)
        {
            var current = _tokenStream.Lookahead(0);

            if (current == null)
            {
                var last = _tokenStream.Current();

                throw CimbolCompilationException.UnexpectedEndOfFileError(FormulaName, last.Start, last.End);
            }

            if (current.Type != tokenType)
            {
                throw errorMessage != null
                    ? new CimbolCompilationException(errorMessage, FormulaName, current.Start, current.End)
                    : CimbolCompilationException.TokenMismatchError(
                        FormulaName,
                        current.Start,
                        current.End,
                        tokenType,
                        current.Type);
            }

            _tokenStream.Next();

            return current;
        }

        private Token Next()
        {
            var current = _tokenStream.Lookahead(0);

            if (current == null)
            {
                var last = _tokenStream.Current();

                throw CimbolCompilationException.UnexpectedEndOfFileError(FormulaName, last.Start, last.End);
            }

            _tokenStream.Next();

            return current;
        }

        private void Reject(TokenType tokenType, string errorMessage = null)
        {
            var current = _tokenStream.Lookahead(0);

            if (current.Type == tokenType)
            {
                throw errorMessage != null
                    ? new CimbolCompilationException(errorMessage, FormulaName, current.Start, current.End)
                    : CimbolCompilationException.TokenMismatchError(
                        FormulaName,
                        current.Start,
                        current.End,
                        tokenType,
                        current.Type);
            }
        }
    }
}
