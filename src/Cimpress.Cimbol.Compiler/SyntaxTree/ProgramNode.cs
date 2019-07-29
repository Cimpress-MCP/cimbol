using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Cimpress.Cimbol.Compiler.SyntaxTree
{
    /// <summary>
    /// The root node for a program.
    /// </summary>
    public class ProgramNode : ISyntaxNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProgramNode"/> class.
        /// </summary>
        /// <param name="arguments">The list of arguments in the program.</param>
        /// <param name="constants">The list of constants in the program.</param>
        /// <param name="modules">The list of modules in the program.</param>
        public ProgramNode(
            IEnumerable<ArgumentDeclarationNode> arguments,
            IEnumerable<ConstantDeclarationNode> constants,
            IEnumerable<ModuleDeclarationNode> modules)
        {
            Arguments = arguments.ToImmutableArray();

            Constants = constants.ToImmutableArray();

            Modules = modules.ToImmutableArray();
        }

        /// <summary>
        /// The list of arguments in the program.
        /// </summary>
        public IReadOnlyCollection<ArgumentDeclarationNode> Arguments { get; }

        /// <summary>
        /// The list of constants in the program.
        /// </summary>
        public IReadOnlyCollection<ConstantDeclarationNode> Constants { get; }

        /// <summary>
        /// The list of modules in the program.
        /// </summary>
        public IReadOnlyCollection<ModuleDeclarationNode> Modules { get; }

        /// <inheritdoc cref="ISyntaxNode.Children"/>
        public IEnumerable<ISyntaxNode> Children()
        {
            foreach (var argument in Arguments)
            {
                yield return argument;
            }

            foreach (var constant in Constants)
            {
                yield return constant;
            }

            foreach (var module in Modules)
            {
                yield return module;
            }
        }

        /// <inheritdoc cref="ISyntaxNode.ChildrenReverse"/>
        public IEnumerable<ISyntaxNode> ChildrenReverse()
        {
            foreach (var module in Modules.Reverse())
            {
                yield return module;
            }

            foreach (var constant in Constants.Reverse())
            {
                yield return constant;
            }

            foreach (var argument in Arguments.Reverse())
            {
                yield return argument;
            }
        }

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            return $"{{{nameof(ProgramNode)}}}";
        }
    }
}