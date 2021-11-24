// Copyright 2021 Cimpress plc.
// Licensed under the Apache License, Version 2.0
// http://www.apache.org/licenses/LICENSE-2.0

using System;
using System.Collections.Generic;

namespace Cimpress.Cimbol.Compiler.SyntaxTree
{
    /// <summary>
    /// A syntax tree node representing a binary operation.
    /// Binary operations include operations like add, greater than, and logical and.
    /// </summary>
    public sealed class BinaryOpNode : IExpressionNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryOpNode"/> class.
        /// </summary>
        /// <param name="opType">The type of binary operation.</param>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand.</param>
        public BinaryOpNode(BinaryOpType opType, IExpressionNode left, IExpressionNode right)
        {
            OpType = opType;

            Left = left ?? throw new ArgumentNullException(nameof(left));

            Right = right ?? throw new ArgumentNullException(nameof(right));

            IsAsynchronous = Left.IsAsynchronous || Right.IsAsynchronous;
        }

        /// <inheritdoc cref="IExpressionNode.IsAsynchronous"/>
        public bool IsAsynchronous { get; }

        /// <summary>
        /// The type of binary operation.
        /// </summary>
        public BinaryOpType OpType { get; }

        /// <summary>
        /// The left operand.
        /// </summary>
        public IExpressionNode Left { get; }

        /// <summary>
        /// The right operand.
        /// </summary>
        public IExpressionNode Right { get; }

        /// <inheritdoc cref="ISyntaxNode.Children"/>
        public IEnumerable<ISyntaxNode> Children()
        {
            yield return Left;

            yield return Right;
        }

        /// <inheritdoc cref="ISyntaxNode.ChildrenReverse"/>
        public IEnumerable<ISyntaxNode> ChildrenReverse()
        {
            yield return Right;

            yield return Left;
        }

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            return $"{{{nameof(BinaryOpNode)} {OpType.GetOperator()}}}";
        }
    }
}