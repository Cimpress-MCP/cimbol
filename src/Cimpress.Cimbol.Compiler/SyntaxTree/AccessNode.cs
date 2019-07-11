using System;
using System.Collections.Generic;

namespace Cimpress.Cimbol.Compiler.SyntaxTree
{
    /// <summary>
    /// A syntax tree node representing member access.
    /// </summary>
    public class AccessNode : IEquatable<AccessNode>, INode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AccessNode"/> class.
        /// </summary>
        /// <param name="value">The value to access a member of.</param>
        /// <param name="member">The name of the member to access.</param>
        public AccessNode(INode value, string member)
        {
            Member = member;

            Value = value;
        }

        /// <summary>
        /// The name of the member to access.
        /// </summary>
        public string Member { get; }

        /// <summary>
        /// The value to access a member of.
        /// </summary>
        public INode Value { get; }

        /// <inheritdoc cref="INode.Children"/>
        public IEnumerable<INode> Children()
        {
            yield return Value;
        }

        /// <inheritdoc cref="INode.ChildrenReverse"/>
        public IEnumerable<INode> ChildrenReverse()
        {
            yield return Value;
        }

        /// <inheritdoc cref="IEquatable{T}.Equals(T)"/>
        public bool Equals(AccessNode other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return string.Equals(Member, other.Member, StringComparison.OrdinalIgnoreCase) &&
                   Equals(Value, other.Value);
        }

        /// <inheritdoc cref="object.Equals(object)"/>
        public override bool Equals(object obj)
        {
            return Equals(obj as AccessNode);
        }

        /// <inheritdoc cref="object.GetHashCode"/>
        public override int GetHashCode()
        {
            unchecked
            {
                return (Member.GetHashCode() * 397) ^ (Value != null ? Value.GetHashCode() : 0);
            }
        }

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            return $"{{{nameof(AccessNode)}}}";
        }
    }
}