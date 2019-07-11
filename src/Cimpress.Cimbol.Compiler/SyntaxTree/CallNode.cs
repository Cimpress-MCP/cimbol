using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Cimpress.Cimbol.Compiler.SyntaxTree
{
    /// <summary>
    /// A syntax tree node representing a function invocation.
    /// </summary>
    public sealed class CallNode : INode, IEquatable<CallNode>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CallNode"/> class.
        /// </summary>
        /// <param name="function">The function to call.</param>
        /// <param name="arguments">The arguments to call the function with.</param>
        public CallNode(INode function, IEnumerable<PositionalArgument> arguments)
        {
            Arguments = arguments.ToImmutableArray();

            Function = function;
        }

        /// <summary>
        /// The arguments to call the function with.
        /// </summary>
        public ImmutableArray<PositionalArgument> Arguments { get; }

        /// <summary>
        /// The function to call.
        /// </summary>
        public INode Function { get; }

        /// <inheritdoc cref="INode.Children"/>
        public IEnumerable<INode> Children()
        {
            yield return Function;

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

            yield return Function;
        }

        /// <inheritdoc cref="IEquatable{T}.Equals(T)"/>
        public bool Equals(CallNode other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Arguments.SequenceEqual(other.Arguments) && Equals(Function, other.Function);
        }

        /// <inheritdoc cref="object.Equals(object)"/>
        public override bool Equals(object obj)
        {
            return Equals(obj as CallNode);
        }

        /// <inheritdoc cref="object.GetHashCode"/>
        public override int GetHashCode()
        {
            unchecked
            {
                return (Arguments.GetHashCode() * 397) ^ (Function != null ? Function.GetHashCode() : 0);
            }
        }

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            return $"{{{nameof(CallNode)}}}";
        }
    }
}