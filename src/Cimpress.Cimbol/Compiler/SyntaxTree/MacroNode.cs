using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Cimpress.Cimbol.Compiler.SyntaxTree
{
    /// <summary>
    /// A syntax tree node representing a macro invocation.
    /// </summary>
    public class MacroNode : IExpressionNode
    {
        /// <summary>
        /// Initialize a new instance of the <see cref="MacroNode"/> class.
        /// </summary>
        /// <param name="macro">The macro to invoke.</param>
        /// <param name="arguments">The arguments to invoke the macro with.</param>
        public MacroNode(string macro, IEnumerable<IArgument> arguments)
        {
            Arguments = arguments?.ToImmutableArray() ?? throw new ArgumentNullException(nameof(arguments));

            IsAsynchronous = Arguments.Any(argument => argument.Value.IsAsynchronous);

            Macro = macro;
        }

        /// <summary>
        /// The arguments to invoke the macro with.
        /// </summary>
        public ImmutableArray<IArgument> Arguments { get; }

        /// <inheritdoc cref="IExpressionNode.IsAsynchronous"/>
        public bool IsAsynchronous { get; }

        /// <summary>
        /// The macro to invoke.
        /// </summary>
        public string Macro { get; }

        /// <inheritdoc cref="ISyntaxNode.Children"/>
        public IEnumerable<ISyntaxNode> Children()
        {
            foreach (var argument in Arguments)
            {
                yield return argument.Value;
            }
        }

        /// <inheritdoc cref="ISyntaxNode.ChildrenReverse"/>
        public IEnumerable<ISyntaxNode> ChildrenReverse()
        {
            foreach (var argument in Arguments.Reverse())
            {
                yield return argument.Value;
            }
        }

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            return $"{{{nameof(MacroNode)} {Macro}}}";
        }
    }
}