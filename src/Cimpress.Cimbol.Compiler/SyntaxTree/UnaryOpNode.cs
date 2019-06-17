using System;
using System.Collections.Generic;

namespace Cimpress.Cimbol.Compiler.SyntaxTree
{
    /// <summary>
    /// A syntax tree node representing a unary operation.
    /// Unary operations include operations like not and negate.
    /// </summary>
    public sealed class UnaryOpNode : INode, IEquatable<UnaryOpNode>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnaryOpNode"/> class.
        /// </summary>
        /// <param name="opType">The type of unary operation.</param>
        /// <param name="operand">The operand.</param>
        public UnaryOpNode(UnaryOpType opType, INode operand)
        {
            OpType = opType;

            Operand = operand;
        }

        /// <summary>
        /// The type of unary operation.
        /// </summary>
        public UnaryOpType OpType { get; }

        /// <summary>
        /// The operand.
        /// </summary>
        public INode Operand { get; }

        /// <inheritdoc cref="INode.Children"/>
        public IEnumerable<INode> Children()
        {
            yield return Operand;
        }

        /// <inheritdoc cref="INode.ChildrenReverse"/>
        public IEnumerable<INode> ChildrenReverse()
        {
            yield return Operand;
        }

        /// <inheritdoc cref="IEquatable{T}.Equals(T)"/>
        public bool Equals(UnaryOpNode other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return OpType == other.OpType && Equals(Operand, other.Operand);
        }

        /// <inheritdoc cref="object.Equals(object)"/>
        public override bool Equals(object obj)
        {
            return Equals(obj as UnaryOpNode);
        }

        /// <inheritdoc cref="object.GetHashCode"/>
        public override int GetHashCode()
        {
            unchecked
            {
                return ((int)OpType * 397) ^ (Operand != null ? Operand.GetHashCode() : 0);
            }
        }

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            return $"{{{nameof(UnaryOpNode)} {OpType.GetOperator()}}}";
        }
    }
}