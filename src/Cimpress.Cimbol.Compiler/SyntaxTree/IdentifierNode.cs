using System;
using System.Collections.Generic;

namespace Cimpress.Cimbol.Compiler.SyntaxTree
{
    /// <summary>
    /// A syntax tree node that references a named variable with an identifier.
    /// </summary>
    public sealed class IdentifierNode : INode, IEquatable<IdentifierNode>
    {
        /// <summary>
        /// Initializes a new instance of the new <see cref="IdentifierNode"/> class.
        /// </summary>
        /// <param name="identifier">The identifier that this node references.</param>
        public IdentifierNode(string identifier)
        {
            Identifier = identifier;
        }

        /// <summary>
        /// The identifier that this node references.
        /// </summary>
        public string Identifier { get; }

        /// <inheritdoc cref="INode.Children"/>
        public IEnumerable<INode> Children()
        {
            yield break;
        }

        /// <inheritdoc cref="INode.ChildrenReverse"/>
        public IEnumerable<INode> ChildrenReverse()
        {
            yield break;
        }

        /// <inheritdoc cref="IEquatable{T}.Equals(T)"/>
        public bool Equals(IdentifierNode other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return string.Equals(Identifier, other.Identifier, StringComparison.OrdinalIgnoreCase);
        }

        /// <inheritdoc cref="object.Equals(object)"/>
        public override bool Equals(object obj)
        {
            return Equals(obj as IdentifierNode);
        }

        /// <inheritdoc cref="object.GetHashCode"/>
        public override int GetHashCode()
        {
            return Identifier != null ? Identifier.GetHashCode() : 0;
        }

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            return $"{{{nameof(IdentifierNode)} \"{Identifier}\"}}";
        }
    }
}