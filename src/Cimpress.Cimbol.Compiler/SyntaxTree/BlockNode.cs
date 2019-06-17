using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Cimpress.Cimbol.Compiler.SyntaxTree
{
    /// <summary>
    /// A syntax tree node representing a logical block of expressions that returns the value of the last expression.
    /// </summary>
    public sealed class BlockNode : INode, IEquatable<BlockNode>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BlockNode"/> class.
        /// </summary>
        /// <param name="expressions">The list of expressions contained in the block.</param>
        public BlockNode(IEnumerable<INode> expressions)
        {
            Expressions = expressions.ToImmutableArray();
        }

        /// <summary>
        /// The list of expressions contained in the block.
        /// </summary>
        public ImmutableArray<INode> Expressions { get; }

        /// <inheritdoc cref="INode.ChildrenReverse"/>
        public IEnumerable<INode> Children()
        {
            foreach (var expression in Expressions)
            {
                yield return expression;
            }
        }

        /// <inheritdoc cref="INode.ChildrenReverse"/>
        public IEnumerable<INode> ChildrenReverse()
        {
            foreach (var expression in Expressions.Reverse())
            {
                yield return expression;
            }
        }

        /// <inheritdoc cref="IEquatable{T}.Equals(T)"/>
        public bool Equals(BlockNode other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Expressions.SequenceEqual(other.Expressions);
        }

        /// <inheritdoc cref="object.Equals(object)"/>
        public override bool Equals(object obj)
        {
            return Equals(obj as BlockNode);
        }

        /// <inheritdoc cref="object.GetHashCode"/>
        public override int GetHashCode()
        {
            return Expressions.GetHashCode();
        }

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            return $"{{{nameof(BlockNode)}}}";
        }
    }
}