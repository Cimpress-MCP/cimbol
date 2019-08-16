using System.Collections.Generic;

namespace Cimpress.Cimbol.Compiler.SyntaxTree
{
    /// <summary>
    /// A syntax tree node that references a named variable with an identifier.
    /// </summary>
    public sealed class IdentifierNode : IExpressionNode
    {
        /// <summary>
        /// Initializes a new instance of the new <see cref="IdentifierNode"/> class.
        /// </summary>
        /// <param name="identifier">The identifier that this node references.</param>
        public IdentifierNode(string identifier)
        {
            Identifier = identifier;

            IsAsynchronous = false;
        }

        /// <summary>
        /// The identifier that this node references.
        /// </summary>
        public string Identifier { get; }

        /// <inheritdoc cref="IExpressionNode.IsAsynchronous"/>
        public bool IsAsynchronous { get; }

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
            return $"{{{nameof(IdentifierNode)} \"{Identifier}\"}}";
        }
    }
}