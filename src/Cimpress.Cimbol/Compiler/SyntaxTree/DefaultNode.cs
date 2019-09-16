using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Cimpress.Cimbol.Compiler.SyntaxTree
{
    /// <summary>
    /// A syntax tree node representing a defaulting access.
    /// </summary>
    public class DefaultNode : IExpressionNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultNode"/> class.
        /// </summary>
        /// <param name="path">The path to attempt to access.</param>
        /// <param name="fallback">The default value to use if the path does not evaluate.</param>
        public DefaultNode(IEnumerable<string> path, IExpressionNode fallback)
        {
            Fallback = fallback ?? throw new ArgumentNullException(nameof(fallback));

            IsAsynchronous = fallback?.IsAsynchronous ?? false;

            Path = path?.ToImmutableArray() ?? throw new ArgumentNullException(nameof(path));
        }

        /// <summary>
        /// The default value to use if the path does not evaluate.
        /// </summary>
        public IExpressionNode Fallback { get; }

        /// <inheritdoc cref="IExpressionNode.IsAsynchronous"/>
        public bool IsAsynchronous { get; }

        /// <summary>
        /// The path to attempt to access.
        /// </summary>
        public IReadOnlyCollection<string> Path { get; }

        /// <inheritdoc cref="ISyntaxNode.Children"/>
        public IEnumerable<ISyntaxNode> Children()
        {
            yield return Fallback;
        }

        /// <inheritdoc cref="ISyntaxNode.ChildrenReverse"/>
        public IEnumerable<ISyntaxNode> ChildrenReverse()
        {
            yield return Fallback;
        }

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            return $"{{{nameof(DefaultNode)}}}";
        }
    }
}