using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Cimpress.Cimbol.Compiler.SyntaxTree
{
    /// <summary>
    /// A syntax tree node representing a macro invocation.
    /// </summary>
    public class MacroNode : IEquatable<MacroNode>, INode
    {
        /// <summary>
        /// Initialize a new instance of the <see cref="MacroNode"/> class.
        /// </summary>
        /// <param name="macro">The macro to call.</param>
        /// <param name="arguments">The arguments to call the macro with.</param>
        public MacroNode(string macro, IEnumerable<IArgument> arguments)
        {
            Arguments = arguments.ToImmutableArray();

            Macro = macro;
        }

        /// <summary>
        /// The arguments to call the macro with.
        /// </summary>
        public ImmutableArray<IArgument> Arguments { get; }

        /// <summary>
        /// The macro to call.
        /// </summary>
        public string Macro { get; }

        /// <inheritdoc cref="INode.Children"/>
        public IEnumerable<INode> Children()
        {
            foreach (var argument in Arguments)
            {
                yield return argument.Value;
            }
        }

        /// <inheritdoc cref="INode.ChildrenReverse"/>
        public IEnumerable<INode> ChildrenReverse()
        {
            foreach (var argument in Arguments.Reverse())
            {
                yield return argument.Value;
            }
        }

        /// <inheritdoc cref="IEquatable{T}.Equals(T)"/>
        public bool Equals(MacroNode other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Arguments.SequenceEqual(other.Arguments) && string.Equals(Macro, other.Macro, StringComparison.OrdinalIgnoreCase);
        }

        /// <inheritdoc cref="object.Equals(object)"/>
        public override bool Equals(object obj)
        {
            return Equals(obj as MacroNode);
        }
        
        /// <inheritdoc cref="object.GetHashCode"/>
        public override int GetHashCode()
        {
            unchecked
            {
                return (Arguments.GetHashCode() * 397) ^ (Macro != null ? Macro.GetHashCode() : 0);
            }
        }

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            return $"{{{nameof(MacroNode)}}}";
        }
    }
}