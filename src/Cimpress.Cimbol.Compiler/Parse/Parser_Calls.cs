using Cimpress.Cimbol.Compiler.Scan;
using Cimpress.Cimbol.Compiler.SyntaxTree;

namespace Cimpress.Cimbol.Compiler.Parse
{
    /// <summary>
    /// The set of methods to use with the <see cref="Parser"/> for parsing calls.
    /// </summary>
    public partial class Parser
    {
        /// <summary>
        /// Parse a series of <see cref="Token"/> objects into either a function call or member access.
        /// </summary>
        /// <returns>Either a <see cref="AccessNode"/>, <see cref="CallNode"/>, or <see cref="MacroNode"/>.</returns>
        public INode Call()
        {
            INode head;

            switch (Lookahead(0))
            {
                // Production rule for an if expression.
                // Call -> "if" KeyArgumentList CallPart+
                case TokenType.IfKeyword:
                {
                    Match(TokenType.IfKeyword);
                    var argumentList = NamedArgumentList();
                    head = new MacroNode("if", argumentList);
                    break;
                }

                // Production rule for a list constructor.
                // Call -> "list" KeyArgumentList CallPart+
                case TokenType.ListKeyword:
                {
                    Match(TokenType.ListKeyword);
                    var argumentList = NamedArgumentList();
                    head = new MacroNode("list", argumentList);
                    break;
                }

                // Production rule for an object constructor.
                // Call -> "object" KeyArgumentList CallPart+
                case TokenType.ObjectKeyword:
                {
                    Match(TokenType.ObjectKeyword);
                    var argumentList = NamedArgumentList();
                    head = new MacroNode("object", argumentList);
                    break;
                }

                // Production rule for a where expression.
                // Call -> "where" KeyArgumentList CallPart+
                case TokenType.WhereKeyword:
                {
                    Match(TokenType.WhereKeyword);
                    var argumentList = NamedArgumentList();
                    head = new MacroNode("where", argumentList);
                    break;
                }

                // Production rule for any generic call or member access.
                // Call -> Atom CallPart+
                default:
                {
                    head = Atom();
                    break;
                }
            }

            // Production rule for trailing member accesses and function calls.
            // Call -> Macro ArgumentList ( CallPart )*
            // Call -> Parentheses ( CallPart )*
            while (true)
            {
                var callPart = CallPart(head);

                if (callPart == null)
                {
                    break;
                }

                head = callPart;
            }

            return head;
        }

        private INode CallPart(INode inner)
        {
            switch (Lookahead(0))
            {
                // Production rule for argument lists.
                // CallPart -> ArgumentList
                case TokenType.LeftParenthesis:
                {
                    var argumentList = ArgumentList();
                    return new CallNode(inner, argumentList);
                }

                // Production rule for member accesses.
                // CallPart -> "." Identifier
                case TokenType.Period:
                {
                    Match(TokenType.Period);
                    var identifier = Match(TokenType.Identifier);
                    return new AccessNode(inner, identifier.Value);
                }

                default:
                    return null;
            }
        }
    }
}