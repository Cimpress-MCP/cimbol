using System;

namespace Cimpress.Cimbol.Compiler.SyntaxTree
{
    /// <summary>
    /// A keyword argument.
    /// </summary>
    public class NamedArgument : IArgument, IEquatable<NamedArgument>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PositionalArgument"/> class.
        /// </summary>
        /// <param name="name">The name of the argument.</param>
        /// <param name="value">The value of the argument.</param>
        public NamedArgument(string name, INode value)
        {
            Name = name;

            Value = value;
        }

        /// <summary>
        /// The name of the argument.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The value of the argument.
        /// </summary>
        public INode Value { get; }

        /// <inheritdoc cref="IEquatable{T}.Equals(T)"/>
        public bool Equals(NamedArgument other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return string.Equals(Name, other.Name, StringComparison.OrdinalIgnoreCase) &&
                   Equals(Value, other.Value);
        }

        /// <inheritdoc cref="object.Equals(object)"/>
        public override bool Equals(object obj)
        {
            return Equals(obj as NamedArgument);
        }

        /// <inheritdoc cref="object.GetHashCode"/>
        public override int GetHashCode()
        {
            unchecked
            {
                return ((Name != null ? Name.GetHashCode() : 0) * 397) ^ (Value != null ? Value.GetHashCode() : 0);
            }
        }
    }
}