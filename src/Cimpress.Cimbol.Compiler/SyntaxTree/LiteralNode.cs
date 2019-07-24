using System.Collections.Generic;

namespace Cimpress.Cimbol.Compiler.SyntaxTree
{
    /// <summary>
    /// A syntax tree node representing a literal value.
    /// </summary>
    public sealed class LiteralNode : IExpressionNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LiteralNode"/> class.
        /// </summary>
        /// <param name="value">The value of the node.</param>
        public LiteralNode(object value)
        {
            Value = value;
        }

        /// <summary>
        /// The value of the node.
        /// </summary>
        public object Value { get; }

        /// <inheritdoc cref="ISyntaxNode.Children"/>
        public IEnumerable<ISyntaxNode> Children()
        {
            yield break;
        }

        /// <inheritdoc cref="ISyntaxNode.ChildrenReverse"/>
        public IEnumerable<ISyntaxNode> ChildrenReverse()
        {
            yield break;
        }

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            return $"{{{nameof(LiteralNode)} {Value}}}";
        }
    }
}