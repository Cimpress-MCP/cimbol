using System;
using System.Collections.Generic;

namespace Cimpress.Cimbol.Compiler.SyntaxTree
{
    /// <summary>
    /// A syntax tree node representing a binary operation.
    /// Binary operations include operations like add, greater than, and logical and.
    /// </summary>
    public sealed class BinaryOpNode : INode, IEquatable<BinaryOpNode>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryOpNode"/> class.
        /// </summary>
        /// <param name="opType">The type of binary operation.</param>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand.</param>
        public BinaryOpNode(BinaryOpType opType, INode left, INode right)
        {
            OpType = opType;

            Left = left;

            Right = right;
        }

        /// <summary>
        /// The type of binary operation.
        /// </summary>
        public BinaryOpType OpType { get; }

        /// <summary>
        /// The left operand.
        /// </summary>
        public INode Left { get; }

        /// <summary>
        /// The right operand.
        /// </summary>
        public INode Right { get; }

        /// <inheritdoc cref="INode.Children"/>
        public IEnumerable<INode> Children()
        {
            yield return Left;

            yield return Right;
        }

        /// <inheritdoc cref="INode.ChildrenReverse"/>
        public IEnumerable<INode> ChildrenReverse()
        {
            yield return Right;

            yield return Left;
        }

        /// <inheritdoc cref="IEquatable{T}.Equals(T)"/>
        public bool Equals(BinaryOpNode other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return OpType == other.OpType && Equals(Left, other.Left) && Equals(Right, other.Right);
        }

        /// <inheritdoc cref="object.Equals(object)"/>
        public override bool Equals(object obj)
        {
            return Equals(obj as BinaryOpNode);
        }

        /// <inheritdoc cref="object.GetHashCode"/>
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (int)OpType;
                hashCode = (hashCode * 397) ^ (Left != null ? Left.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Right != null ? Right.GetHashCode() : 0);
                return hashCode;
            }
        }

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            return $"{{{nameof(BinaryOpNode)} {OpType.GetOperator()}}}";
        }
    }
}