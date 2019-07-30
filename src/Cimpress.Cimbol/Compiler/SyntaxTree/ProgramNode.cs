using System;
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
        private readonly ImmutableDictionary<string, ArgumentDeclarationNode> _argumentTable;

        private readonly ImmutableDictionary<string, ConstantDeclarationNode> _constantTable;

        private readonly ImmutableDictionary<string, ModuleDeclarationNode> _moduleTable;

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

            _argumentTable = Arguments.ToImmutableDictionary(
                argument => argument.Name,
                argument => argument,
                StringComparer.OrdinalIgnoreCase);

            Constants = constants.ToImmutableArray();

            _constantTable = Constants.ToImmutableDictionary(
                constant => constant.Name,
                constant => constant,
                StringComparer.OrdinalIgnoreCase);

            Modules = modules.ToImmutableArray();

            _moduleTable = Modules.ToImmutableDictionary(
                module => module.Name,
                module => module,
                StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// The list of arguments in the program.
        /// </summary>
        public IEnumerable<ArgumentDeclarationNode> Arguments { get; }

        /// <summary>
        /// The list of constants in the program.
        /// </summary>
        public IEnumerable<ConstantDeclarationNode> Constants { get; }

        /// <summary>
        /// The list of modules in the program.
        /// </summary>
        public IEnumerable<ModuleDeclarationNode> Modules { get; }

        /// <inheritdoc cref="ISyntaxNode.Children"/>
        public IEnumerable<ISyntaxNode> Children()
        {
            foreach (var argument in _argumentTable.Values)
            {
                yield return argument;
            }

            foreach (var constant in _constantTable.Values)
            {
                yield return constant;
            }

            foreach (var module in _moduleTable.Values)
            {
                yield return module;
            }
        }

        /// <inheritdoc cref="ISyntaxNode.ChildrenReverse"/>
        public IEnumerable<ISyntaxNode> ChildrenReverse()
        {
            foreach (var module in _moduleTable.Values.Reverse())
            {
                yield return module;
            }

            foreach (var constant in _constantTable.Values.Reverse())
            {
                yield return constant;
            }

            foreach (var argument in _argumentTable.Values.Reverse())
            {
                yield return argument;
            }
        }

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            return $"{{{nameof(ProgramNode)}}}";
        }

        /// <summary>
        /// Try and retrieve an argument declaration by name from the program.
        /// </summary>
        /// <param name="argumentName">The name of the argument to retrieve.</param>
        /// <param name="argument">The retrieved argument.</param>
        /// <returns>Whether or not the argument was retrieved.</returns>
        public bool TryGetArgumentDeclaration(string argumentName, out ArgumentDeclarationNode argument)
        {
            return _argumentTable.TryGetValue(argumentName, out argument);
        }

        /// <summary>
        /// Try and retrieve a constant declaration by name from the program.
        /// </summary>
        /// <param name="constantName">The name of the constant to retrieve.</param>
        /// <param name="constant">The retrieved constant.</param>
        /// <returns>Whether or not the argument was retrieved.</returns>
        public bool TryGetConstantDeclaration(string constantName, out ConstantDeclarationNode constant)
        {
            return _constantTable.TryGetValue(constantName, out constant);
        }

        /// <summary>
        /// Try and retrieve a module declaration by name from the program.
        /// </summary>
        /// <param name="moduleName">The name of the module to retrieve.</param>
        /// <param name="module">The retrieved module.</param>
        /// <returns>Whether or not the module was retrieved.</returns>
        public bool TryGetModuleDeclaration(string moduleName, out ModuleDeclarationNode module)
        {
            return _moduleTable.TryGetValue(moduleName, out module);
        }
    }
}