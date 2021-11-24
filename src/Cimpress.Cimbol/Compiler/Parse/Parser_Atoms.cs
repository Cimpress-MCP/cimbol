// Copyright 2021 Cimpress plc.
// Licensed under the Apache License, Version 2.0
// http://www.apache.org/licenses/LICENSE-2.0

using Cimpress.Cimbol.Compiler.Scan;
using Cimpress.Cimbol.Compiler.SyntaxTree;
using Cimpress.Cimbol.Exceptions;
using Cimpress.Cimbol.Runtime.Types;
using Cimpress.Cimbol.Utilities;

namespace Cimpress.Cimbol.Compiler.Parse
{
    /// <summary>
    /// The set of methods to use with the <see cref="Parser"/> for parsing atoms.
    /// </summary>
    public partial class Parser
    {
        /// <summary>
        /// Parse a series of <see cref="Token"/> objects into a terminal syntax tree node.
        /// </summary>
        /// <returns>Either a <see cref="LiteralNode"/> or <see cref="IdentifierNode"/>.</returns>
        public IExpressionNode Atom()
        {
            var current = Lookahead(0);
            switch (current)
            {
                // Production rule for a true keyword.
                // Atom -> "true"
                case TokenType.TrueKeyword:
                {
                    Match(TokenType.TrueKeyword);
                    return new LiteralNode(new BooleanValue(true));
                }

                // Production rule for a false keyword.
                // Atom -> "false"
                case TokenType.FalseKeyword:
                {
                    Match(TokenType.FalseKeyword);
                    return new LiteralNode(new BooleanValue(false));
                }

                // Production rule for a number literal.
                // Atom -> NumberLiteral
                case TokenType.NumberLiteral:
                {
                    var match = Match(TokenType.NumberLiteral);
                    return new LiteralNode(new NumberValue(NumberSerializer.DeserializeNumber(match.Value)));
                }

                // Production rule for a string literal.
                // Atom -> StringLiteral
                case TokenType.StringLiteral:
                {
                    var match = Match(TokenType.StringLiteral);
                    return new LiteralNode(new StringValue(StringSerializer.DeserializeString(match.Value)));
                }

                // Production rule for an identifier.
                // Atom -> Identifier
                case TokenType.Identifier:
                {
                    var match = Match(TokenType.Identifier);
                    return new IdentifierNode(IdentifierSerializer.DeserializeIdentifier(match.Value));
                }

                // Production rule for a parenthesized expression.
                // Atom -> LeftParenthesis Expression RightParenthesis
                case TokenType.LeftParenthesis:
                {
                    Match(TokenType.LeftParenthesis);
                    var expression = Expression();
                    Match(TokenType.RightParenthesis);
                    return expression;
                }

                // Expected a terminal, found something else.
                default:
                {
                    var currentToken = Next();
                    throw CimbolCompilationException.ExpectedTerminalError(
                        FormulaName,
                        currentToken.Start,
                        currentToken.End);
                }
            }
        }
    }
}