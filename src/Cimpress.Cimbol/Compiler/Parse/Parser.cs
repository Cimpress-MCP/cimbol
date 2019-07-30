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
            if (tokenStream == null)
            {
                throw new ArgumentNullException(nameof(tokenStream));
            }

            _tokenStream = tokenStream;
        }

        /// <summary>
        /// Parse a series of <see cref="Token"/> objects into an expression.
        /// </summary>
        /// <returns>A syntax tree node representing an expression.</returns>
        public IExpressionNode Expression()
        {
            var expression = LogicalAnd();

            return expression;
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
                throw new NotSupportedException();
            }

            if (current.Type != tokenType)
            {
                // Wrong token type.
                throw new NotSupportedException();
            }

            _tokenStream.Next();

            return current;
        }
    }
}
