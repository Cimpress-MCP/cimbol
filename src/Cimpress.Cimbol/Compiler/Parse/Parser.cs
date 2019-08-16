using System;
using Cimpress.Cimbol.Compiler.Scan;
using Cimpress.Cimbol.Compiler.SyntaxTree;

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
        /// <param name="tokenStream">The stream of tokens to parse.</param>
        public Parser(TokenStream tokenStream)
        {
            _tokenStream = tokenStream ?? throw new ArgumentNullException(nameof(tokenStream));
        }

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
                // Unexpected end of file.
#pragma warning disable CA1303
                throw new NotSupportedException("ErrorCode012");
#pragma warning restore CA1303
            }

            if (current.Type != tokenType)
            {
                // Wrong token type.
#pragma warning disable CA1303
                throw new NotSupportedException("ErrorCode013");
#pragma warning restore CA1303
            }

            _tokenStream.Next();

            return current;
        }
    }
}
