using System;
using System.Collections.Generic;
using System.Linq;
using Cimpress.Cimbol.Compiler.Scan;
using Cimpress.Cimbol.Compiler.SyntaxTree;

namespace Cimpress.Cimbol.Compiler.Parse
{
    /// <summary>
    /// The set of methods to use with the <see cref="Parser"/> for parsing comparisons.
    /// </summary>
    public partial class Parser
    {
        /// <summary>
        /// Parses a series of comparison operations.
        /// </summary>
        /// <returns>A tree of comparison operations.</returns>
        public INode Comparison()
        {
            var head = Concatenate();

            while (true)
            {
                var lookahead = Lookahead(0);
                switch (lookahead)
                {
                    // Production rule for the equals operation.
                    // Comparison -> Concatenate ( "==" Concatenate )*
                    // Comparison -> Concatenate ( ">" Concatenate )*
                    // Comparison -> Concatenate ( ">=" Concatenate )*
                    // Comparison -> Concatenate ( "<" Concatenate )*
                    // Comparison -> Concatenate ( "<=" Concatenate )*
                    // Comparison -> Concatenate ( "!=" Concatenate )*
                    case TokenType.Equal:
                    case TokenType.GreaterThan:
                    case TokenType.GreaterThanEqual:
                    case TokenType.LessThan:
                    case TokenType.LessThanEqual:
                    case TokenType.NotEqual:
                        Match(lookahead);
                        var opType = GetComparisonOpType(lookahead);
                        head = new BinaryOpNode(opType, head, Concatenate());
                        break;

                    default:
                        return head;
                }
            }
        }

        /// <summary>
        /// Parses a series of logical and expressions.
        /// </summary>
        /// <returns>A tree of logical and expressions.</returns>
        public INode LogicalAnd()
        {
            var head = LogicalOr();

            while (true)
            {
                switch (Lookahead(0))
                {
                    // Production rule for getting the logical AND of two expressions.
                    // LogicalAnd -> LogicalOr ( "and" LogicalOr )*
                    case TokenType.And:
                        Match(TokenType.And);
                        head = new BinaryOpNode(BinaryOpType.And, head, LogicalOr());
                        break;

                    default:
                        return head;
                }
            }
        }

        /// <summary>
        /// Parses a series of logical not expressions.
        /// </summary>
        /// <returns>A tree of logical not expressions.</returns>
        public INode LogicalNot()
        {
            var operationStack = new Stack<UnaryOpType>();

            while (true)
            {
                if (Lookahead(0) == TokenType.Not)
                {
                    operationStack.Push(UnaryOpType.Not);

                    Match(TokenType.Not);
                }
                else
                {
                    break;
                }
            }

            var head = Comparison();

            while (operationStack.Any())
            {
                operationStack.Pop();

                head = new UnaryOpNode(UnaryOpType.Not, head);
            }

            return head;
        }

        /// <summary>
        /// Parses a series of logical or expressions.
        /// </summary>
        /// <returns>A tree of logical or expressions.</returns>
        public INode LogicalOr()
        {
            var head = LogicalNot();

            while (true)
            {
                switch (Lookahead(0))
                {
                    // Production rule for getting the logical OR of two expressions.
                    // LogicalOr -> LogicalNot ( "or" LogicalNot )*
                    case TokenType.Or:
                        Match(TokenType.Or);
                        head = new BinaryOpNode(BinaryOpType.Or, head, LogicalNot());
                        break;

                    default:
                        return head;
                }
            }
        }

        private static BinaryOpType GetComparisonOpType(TokenType tokenType)
        {
            switch (tokenType)
            {
                case TokenType.Equal:
                    return BinaryOpType.Equal;

                case TokenType.GreaterThan:
                    return BinaryOpType.GreaterThan;

                case TokenType.GreaterThanEqual:
                    return BinaryOpType.GreaterThanOrEqual;

                case TokenType.LessThan:
                    return BinaryOpType.LessThan;

                case TokenType.LessThanEqual:
                    return BinaryOpType.LessThanOrEqual;

                case TokenType.NotEqual:
                    return BinaryOpType.NotEqual;

                default:
                    throw new NotSupportedException();
            }
        }
    }
}