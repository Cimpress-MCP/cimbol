using System;

namespace Cimpress.Cimbol.Compiler.Utilities
{
    /// <summary>
    /// A name to refer to program-level constructs by.
    /// If constructed with a name, this will match any other reference name with that same string.
    /// If constructed without a name, this will only match when the two reference names have reference equality.
    /// </summary>
    public class ReferenceName : IEquatable<ReferenceName>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReferenceName"/> class with no name.
        /// </summary>
        public ReferenceName()
        {
            Name = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferenceName"/> class with a name.
        /// </summary>
        /// <param name="name">The name of the reference.</param>
        public ReferenceName(string name)
        {
            Name = name;
        }

        /// <summary>
        /// The name of the reference.
        /// </summary>
        public string Name { get; }

        /// <inheritdoc cref="IEquatable{T}.Equals(T)"/>
        public bool Equals(ReferenceName other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            // Don't return true if either name is null.
            // If the name of the reference is null, then the reference acts as a symbol.
            if (Name == null || other.Name == null)
            {
                return false;
            }

            return string.Equals(Name, other.Name, StringComparison.OrdinalIgnoreCase);
        }

        /// <inheritdoc cref="object.Equals(object)"/>
        public override bool Equals(object other)
        {
            return Equals(other as ReferenceName);
        }

        /// <inheritdoc cref="object.GetHashCode"/>
        public override int GetHashCode()
        {
            return Name != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(Name) : base.GetHashCode();
        }
    }
}