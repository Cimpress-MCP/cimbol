using System.Collections.Generic;
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
        private const string Default_ArgumentCount = "The default function takes only two arguments.";

        private const string Default_IdentifierArgument = "The default function's first argument must be a name.";

        private const string Exists_ArgumentCount = "The exists function takes one argument.";

        private const string Exists_IdentifierArgument = "The exists function's first argument must be a name.";

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
                // Invoke -> "default" "(" Identifier ("." Identifier)+ "," Expression ")" InvokePart*
                case TokenType.DefaultKeyword:
                    head = Default();
                    break;

                // Product rule for an exists expression
                // Invoke -> "exists" "(" Identifier ("." Identifier)* ")" InvokePart*
                case TokenType.ExistsKeyword:
                    head = Exists();
                    break;

                // Production rule for an if expression.
                // Invoke -> "if" "(" Expression "," "then" "=" Expression ("," "else" "=" Expression)* ")" InvokePart*
                case TokenType.IfKeyword:
                    head = If();
                    break;

                // Production rule for a list constructor.
                // Invoke -> "list" "(" Expression ("," Expression)* ")" InvokePart*
                case TokenType.ListKeyword:
                    head = List();
                    break;

                // Production rule for an object constructor.
                // Invoke -> "object" "(" Identifier "=" Identifier ("," Identifier "=" Identifier)* ")" InvokePart*
                case TokenType.ObjectKeyword:
                    head = Object();
                    break;

                // Production rule for a where expression.
                // Invoke -> "where" "(" WhereCase ("," WhereCase)* ("," WhereDefault)? ")"
                // Invoke -> "where" "(" WhereDefault ")"
                // WhereCase -> "case" "=" Expression "," "do" "=" Expression
                // WhereDefault -> "default" = "Expression"
                case TokenType.WhereKeyword:
                    head = Where();
                    break;

                // Production rule for any generic function invocation or member access.
                // Invoke -> Atom InvokePart*
                default:
                    head = Atom();
                    break;
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

        private IExpressionNode Default()
        {
            // A default expression needs to start with a default keyword.
            Match(TokenType.DefaultKeyword);

            // Parse the opening parenthesis.
            Match(TokenType.LeftParenthesis);

            // Reject when no arguments have been provided.
            Reject(TokenType.RightParenthesis, Default_ArgumentCount);

            // Parse the first argument.
            var path = new List<string>();

            // Parse the required first identifier of the first argument.
            {
                var identifier = Match(TokenType.Identifier, Default_IdentifierArgument);

                path.Add(identifier.Value);
            }

            // Parse every following member access that is a part of the first argument.
            while (Lookahead(0) != TokenType.Comma && Lookahead(0) != TokenType.RightParenthesis)
            {
                Match(TokenType.Period);

                var identifier = Match(TokenType.Identifier);

                path.Add(identifier.Value);
            }

            // Reject when only one argument has been provided.
            Reject(TokenType.RightParenthesis, Default_ArgumentCount);

            // Parse the second argument.
            Match(TokenType.Comma);

            var fallback = Expression();

            // Parse the closing parenthesis, rejecting with an error about argument count if not found.
            Match(TokenType.RightParenthesis, Default_ArgumentCount);

            return new DefaultNode(path, fallback);
        }

        private IExpressionNode Exists()
        {
            // A default expression needs to start with a default keyword.
            Match(TokenType.ExistsKeyword);

            // Parse the opening parenthesis.
            Match(TokenType.LeftParenthesis);

            // Reject when no arguments have been provided.
            Reject(TokenType.RightParenthesis, Exists_ArgumentCount);

            // Parse the first argument.
            var path = new List<string>();

            // Parse the required first identifier of the first argument.
            {
                var identifier = Match(TokenType.Identifier, Exists_IdentifierArgument);

                path.Add(identifier.Value);
            }

            // Parse every following member access that is a part of the first argument.
            while (Lookahead(0) != TokenType.Comma && Lookahead(0) != TokenType.RightParenthesis)
            {
                Match(TokenType.Period);

                var identifier = Match(TokenType.Identifier);

                path.Add(identifier.Value);
            }

            // Parse the closing parenthesis, rejecting with an error about argument count if not found.
            Match(TokenType.RightParenthesis, Exists_ArgumentCount);

            return new ExistsNode(path);
        }

        private IExpressionNode If()
        {
            Match(TokenType.IfKeyword);
            var argumentList = NamedArgumentList();
            return new MacroNode("if", argumentList);
        }

        private MacroNode List()
        {
            Match(TokenType.ListKeyword);
            var argumentList = NamedArgumentList();
            return new MacroNode("list", argumentList);
        }

        private MacroNode Object()
        {
            Match(TokenType.ObjectKeyword);
            var argumentList = NamedArgumentList();
            return new MacroNode("object", argumentList);
        }

        private MacroNode Where()
        {
            Match(TokenType.WhereKeyword);
            var argumentList = NamedArgumentList();
            return new MacroNode("where", argumentList);
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