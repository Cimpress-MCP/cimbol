﻿// Copyright 2021 Cimpress plc.
// Licensed under the Apache License, Version 2.0
// http://www.apache.org/licenses/LICENSE-2.0

using System;
using System.Collections.Generic;
using System.Linq;
using Cimpress.Cimbol.Compiler.Scan;
using Cimpress.Cimbol.Compiler.SyntaxTree;

namespace Cimpress.Cimbol.Compiler.Parse
{
    /// <summary>
    /// The set of methods to use with the <see cref="Parser"/> for parsing terms.
    /// </summary>
    public partial class Parser
    {
        /// <summary>
        /// Parse a series of <see cref="Token"/> objects into a series of addition and subtraction operations.
        /// </summary>
        /// <returns>The result of parsing the addition and subtraction operations.</returns>
        public IExpressionNode Add()
        {
            var head = Multiply();

            while (true)
            {
                switch (Lookahead(0))
                {
                    // Production rule for adding two expressions.
                    // Add -> Multiply ( "+" Multiply )*
                    case TokenType.Add:
                        Match(TokenType.Add);
                        head = new BinaryOpNode(BinaryOpType.Add, head, Multiply());
                        break;

                    // Production rule for subtracting two expressions.
                    // Add -> Multiply ( "-" Multiply )*
                    case TokenType.Subtract:
                        Match(TokenType.Subtract);
                        head = new BinaryOpNode(BinaryOpType.Subtract, head, Multiply());
                        break;

                    default:
                        return head;
                }
            }
        }

        /// <summary>
        /// Parse a series of <see cref="Token"/> objects into a series of concatenation operations.
        /// </summary>
        /// <returns>The result of parsing the concatenation operations.</returns>
        public IExpressionNode Concatenate()
        {
            var head = Add();

            while (true)
            {
                switch (Lookahead(0))
                {
                    // Production rule for concatenating two expressions.
                    // Concatenate -> Add ( "+" Add )*
                    case TokenType.Concatenate:
                        Match(TokenType.Concatenate);
                        head = new BinaryOpNode(BinaryOpType.Concatenate, head, Add());
                        break;

                    default:
                        return head;
                }
            }
        }

        /// <summary>
        /// Parse a series of <see cref="Token"/> objects into a unary expression.
        /// A factor is an expression with a leading unary arithmetic operator like "+" or "-", or the "await" operator.
        /// This does not include the "not" keyword because of precedence issues.
        /// </summary>
        /// <returns>The result of parsing the factor.</returns>
        public IExpressionNode Unary()
        {
            var operationStack = new Stack<UnaryOpType>();

            while (true)
            {
                if (Lookahead(0) == TokenType.Add)
                {
                    Match(TokenType.Add);
                }
                else if (Lookahead(0) == TokenType.Subtract)
                {
                    Match(TokenType.Subtract);

                    if (operationStack.Count > 0 && operationStack.Peek() == UnaryOpType.Negate)
                    {
                        operationStack.Pop();
                    }
                    else
                    {
                        operationStack.Push(UnaryOpType.Negate);
                    }
                }
                else
                {
                    break;
                }
            }

            var head = Power();

            while (operationStack.Any())
            {
                switch (operationStack.Pop())
                {
                    case UnaryOpType.Negate:
                        head = new UnaryOpNode(UnaryOpType.Negate, head);
                        break;

                    default:
                        throw new NotSupportedException();
                }
            }

            return head;
        }

        /// <summary>
        /// Parse a series of <see cref="Token"/> objects into a series of multiplication and division operations.
        /// </summary>
        /// <returns>The result of parsing the multiplication and division operations.</returns>
        public IExpressionNode Multiply()
        {
            var head = Unary();

            while (true)
            {
                switch (Lookahead(0))
                {
                    // Production rule for dividing two expressions.
                    // Multiply -> Factor ( "/" Factor )*
                    case TokenType.Divide:
                        Match(TokenType.Divide);
                        head = new BinaryOpNode(BinaryOpType.Divide, head, Unary());
                        break;

                    // Production rule for multiplying two expressions.
                    // Multiply -> Factor ( "*" Factor )*
                    case TokenType.Multiply:
                        Match(TokenType.Multiply);
                        head = new BinaryOpNode(BinaryOpType.Multiply, head, Unary());
                        break;

                    // Production rule for get the remainder between two expressions.
                    // Multiply -> Factor ( "%" Factor )*
                    case TokenType.Remainder:
                        Match(TokenType.Remainder);
                        head = new BinaryOpNode(BinaryOpType.Remainder, head, Unary());
                        break;

                    default:
                        return head;
                }
            }
        }

        /// <summary>
        /// Parse a series of <see cref="Token"/> objects into a series of power operations.
        /// </summary>
        /// <returns>The result of parsing the power operations.</returns>
        public IExpressionNode Power()
        {
            // The exponentiation operation is right-associative.
            // That means that x ^ y ^ z is equivalent to x ^ (y ^ z) instead of (x ^ y) ^ z.
            // Iteration, however, occurs in a left-to-right fashion.
            // This means that the syntax tree nodes can't be built until the entire power term is parsed.
            var expressionStack = new Stack<IExpressionNode>();
            expressionStack.Push(Invoke());

            // Production rule for get the remainder between two expressions.
            // Power -> Invoke ( "%" Invoke )*
            while (true)
            {
                if (Lookahead(0) == TokenType.Power)
                {
                    Match(TokenType.Power);
                    expressionStack.Push(Invoke());
                }
                else
                {
                    break;
                }
            }

            IExpressionNode head = null;

            while (expressionStack.Any())
            {
                var top = expressionStack.Pop();

                head = head == null ? top : new BinaryOpNode(BinaryOpType.Power, top, head);
            }

            return head;
        }
    }
}