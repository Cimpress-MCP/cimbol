using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Cimpress.Cimbol.Compiler.SyntaxTree
{
    /// <summary>
    /// The syntax tree node for declaring a module.
    /// </summary>
    public class ModuleDeclarationNode : IDeclarationNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleDeclarationNode"/> class.
        /// </summary>
        /// <param name="name">The name of the module.</param>
        /// <param name="imports">The list of imports in the module.</param>
        /// <param name="exports">The list of exports in the module.</param>
        /// <param name="formulas">The list of formulas in the module.</param>
        public ModuleDeclarationNode(
            string name,
            IEnumerable<ImportStatementNode> imports,
            IEnumerable<ExportStatementNode> exports,
            IEnumerable<FormulaDeclarationNode> formulas)
        {
            Exports = exports.ToImmutableArray();

            Formulas = formulas.ToImmutableArray();

            Imports = imports.ToImmutableArray();

            Name = name;
        }

        /// <summary>
        /// THe list of exports in the module.
        /// </summary>
        public IReadOnlyCollection<ExportStatementNode> Exports { get; }

        /// <summary>
        /// The list of formulas in the module.
        /// </summary>
        public IReadOnlyCollection<FormulaDeclarationNode> Formulas { get; }

        /// <summary>
        /// The list of imports in the module.
        /// </summary>
        public IReadOnlyCollection<ImportStatementNode> Imports { get; }

        /// <inheritdoc cref="IDeclarationNode.Name"/>
        public string Name { get; }

        /// <inheritdoc cref="ISyntaxNode.Children"/>
        public IEnumerable<ISyntaxNode> Children()
        {
            foreach (var import in Imports)
            {
                yield return import;
            }

            foreach (var export in Exports)
            {
                yield return export;
            }

            foreach (var formula in Formulas)
            {
                yield return formula;
            }
        }

        /// <inheritdoc cref="ISyntaxNode.ChildrenReverse"/>
        public IEnumerable<ISyntaxNode> ChildrenReverse()
        {
            foreach (var formula in Formulas.Reverse())
            {
                yield return formula;
            }

            foreach (var export in Exports.Reverse())
            {
                yield return export;
            }

            foreach (var import in Imports.Reverse())
            {
                yield return import;
            }
        }

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            return $"{{{nameof(ModuleDeclarationNode)} {Name}}}";
        }
    }
}