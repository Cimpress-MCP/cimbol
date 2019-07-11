using System;

namespace Cimpress.Cimbol.Compiler.SyntaxTree
{
    /// <summary>
    /// A positional argument.
    /// </summary>
    public class PositionalArgument : IArgument, IEquatable<PositionalArgument>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PositionalArgument"/> class.
        /// </summary>
        /// <param name="value">The value of the argument.</param>
        public PositionalArgument(INode value)
        {
            Value = value;
        }

        /// <summary>
        /// The value of the argument.
        /// </summary>
        public INode Value { get; }

        /// <inheritdoc cref="IEquatable{T}.Equals(T)"/>
        public bool Equals(PositionalArgument other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Equals(Value, other.Value);
        }

        /// <inheritdoc cref="object.Equals(object)"/>
        public override bool Equals(object obj)
        {
            return Equals(obj as PositionalArgument);
        }

        /// <inheritdoc cref="object.GetHashCode"/>
        public override int GetHashCode()
        {
            return Value != null ? Value.GetHashCode() : 0;
        }
    }
}