using Cimpress.Cimbol.Compiler.Scan;
using Cimpress.Cimbol.Compiler.SyntaxTree;
using Cimpress.Cimbol.Utilities;

namespace Cimpress.Cimbol.Compiler.Parse
{
    /// <summary>
    /// The set of methods to use with the <see cref="Parser"/> for parsing invocations.
    /// Note that member accesses as well as macro invocations also fall under this set of productions.
    /// </summary>
    public partial class Parser
    {
        /// <summary>
        /// Parse a series of <see cref="Token"/> objects into either a function invocation or member access.
        /// </summary>
        /// <returns>Either a <see cref="AccessNode"/>, <see cref="InvokeNode"/>, or <see cref="MacroNode"/>.</returns>
        public IExpressionNode Invoke()
        {
            IExpressionNode head;

            switch (Lookahead(0))
            {
                // Production rule for an if expression.
                // Invoke -> "if" KeyArgumentList InvokePart*
                case TokenType.IfKeyword:
                {
                    Match(TokenType.IfKeyword);
                    var argumentList = NamedArgumentList();
                    head = new MacroNode("if", argumentList);
                    break;
                }

                // Production rule for a list constructor.
                // Invoke -> "list" KeyArgumentList InvokePart*
                case TokenType.ListKeyword:
                {
                    Match(TokenType.ListKeyword);
                    var argumentList = NamedArgumentList();
                    head = new MacroNode("list", argumentList);
                    break;
                }

                // Production rule for an object constructor.
                // Invoke -> "object" KeyArgumentList InvokePart*
                case TokenType.ObjectKeyword:
                {
                    Match(TokenType.ObjectKeyword);
                    var argumentList = NamedArgumentList();
                    head = new MacroNode("object", argumentList);
                    break;
                }

                // Production rule for a where expression.
                // Invoke -> "where" KeyArgumentList InvokePart*
                case TokenType.WhereKeyword:
                {
                    Match(TokenType.WhereKeyword);
                    var argumentList = NamedArgumentList();
                    head = new MacroNode("where", argumentList);
                    break;
                }

                // Production rule for any generic function invocation or member access.
                // Invoke -> Atom InvokePart*
                default:
                {
                    head = Atom();
                    break;
                }
            }

            // Production rule for trailing member accesses and function invocations.
            // Invoke -> Macro ArgumentList InvokePart*
            // Invoke -> Parentheses InvokePart*
            while (true)
            {
                var invokePart = InvokePart(head);

                if (invokePart == null)
                {
                    break;
                }

                head = invokePart;
            }

            return head;
        }

        private IExpressionNode InvokePart(IExpressionNode inner)
        {
            switch (Lookahead(0))
            {
                // Production rule for argument lists.
                // InvokePart -> ArgumentList
                case TokenType.LeftParenthesis:
                {
                    var argumentList = ArgumentList();
                    return new InvokeNode(inner, argumentList);
                }

                // Production rule for member accesses.
                // InvokePart -> "." Identifier
                case TokenType.Period:
                {
                    Match(TokenType.Period);
                    var identifier = Match(TokenType.Identifier);
                    return new AccessNode(inner, IdentifierSerializer.DeserializeIdentifier(identifier.Value));
                }

                default:
                    return null;
            }
        }
    }
}