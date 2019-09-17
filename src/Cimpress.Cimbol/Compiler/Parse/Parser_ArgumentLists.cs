using System.Collections.Generic;
using Cimpress.Cimbol.Compiler.Scan;
using Cimpress.Cimbol.Compiler.SyntaxTree;
using Cimpress.Cimbol.Utilities;

namespace Cimpress.Cimbol.Compiler.Parse
{
    /// <summary>
    /// The set of methods to use with the <see cref="Parser"/> for parsing argument lists.
    /// </summary>
    public partial class Parser
    {
        /// <summary>
        /// Parse a series of <see cref="Token"/> objects into an argument list.
        /// </summary>
        /// <returns>The list of arguments as an array of <see cref="PositionalArgument"/> objects.</returns>
        public IEnumerable<PositionalArgument> ArgumentList()
        {
            Match(TokenType.LeftParenthesis);

            var first = true;

            while (Lookahead(0) != TokenType.RightParenthesis)
            {
                if (!first)
                {
                    Match(TokenType.Comma);
                }
                else
                {
                    first = false;
                }

                var argument = Expression();
                yield return new PositionalArgument(argument);
            }

            Match(TokenType.RightParenthesis);
        }

        /// <summary>
        /// Parse a series of <see cref="Token"/> objects into a named argument list.
        /// </summary>
        /// <returns>The list of arguments as an array of <see cref="IArgument"/> objects.</returns>
        public IEnumerable<IArgument> NamedArgumentList()
        {
            Match(TokenType.LeftParenthesis);

            var first = true;

            while (Lookahead(0) != TokenType.RightParenthesis)
            {
                if (!first)
                {
                    Match(TokenType.Comma);
                }
                else
                {
                    first = false;
                }

                if (Lookahead(0) == TokenType.Identifier && Lookahead(1) == TokenType.Assign)
                {
                    var name = Match(TokenType.Identifier);
                    Match(TokenType.Assign);
                    var argument = Expression();
                    yield return new NamedArgument(IdentifierSerializer.DeserializeIdentifier(name.Value), argument);
                }
                else
                {
                    var argument = Expression();
                    yield return new PositionalArgument(argument);
                }
            }

            Match(TokenType.RightParenthesis);
        }
    }
}