using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Cimpress.Cimbol.Compiler.SyntaxTree
{
    /// <summary>
    /// A syntax tree node representing a logical block of expressions that returns the value of the last expression.
    /// </summary>
    public sealed class BlockNode : IExpressionNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BlockNode"/> class.
        /// </summary>
        /// <param name="expressions">The list of expressions contained in the block.</param>
        public BlockNode(IEnumerable<IExpressionNode> expressions)
        {
            Expressions = expressions?.ToImmutableArray() ?? throw new ArgumentNullException(nameof(expressions));

            IsAsynchronous = Expressions.Any(expression => expression.IsAsynchronous);
        }

        /// <summary>
        /// The list of expressions contained in the block.
        /// </summary>
        public ImmutableArray<IExpressionNode> Expressions { get; }

        /// <inheritdoc cref="IExpressionNode.IsAsynchronous"/>
        public bool IsAsynchronous { get; }

        /// <inheritdoc cref="ISyntaxNode.ChildrenReverse"/>
        public IEnumerable<ISyntaxNode> Children()
        {
            foreach (var expression in Expressions)
            {
                yield return expression;
            }
        }

        /// <inheritdoc cref="ISyntaxNode.ChildrenReverse"/>
        public IEnumerable<ISyntaxNode> ChildrenReverse()
        {
            foreach (var expression in Expressions.Reverse())
            {
                yield return expression;
            }
        }

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            return $"{{{nameof(BlockNode)}}}";
        }
    }
}