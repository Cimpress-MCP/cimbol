using System.Collections.Generic;
using System.Collections.Immutable;

namespace Cimpress.Cimbol.Compiler.SyntaxTree
{
    /// <summary>
    /// A syntax node representing the importing of a result or set of results from another module.
    /// </summary>
    public class ImportNode : IDeclarationNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImportNode"/> class.
        /// </summary>
        /// <param name="name">The identifier that this node assigns to.</param>
        /// <param name="importPath">The path to import from.</param>
        /// <param name="importType">The import type.</param>
        /// <param name="isExported">Whether or not the formula is exported.</param>
        public ImportNode(string name, IEnumerable<string> importPath, ImportType importType, bool isExported)
        {
            ImportPath = importPath.ToImmutableArray();

            ImportType = importType;

            IsExported = isExported;

            Name = name;
        }

        /// <summary>
        /// The path to import from.
        /// </summary>
        public IReadOnlyCollection<string> ImportPath { get; }

        /// <summary>
        /// The type of import.
        /// </summary>
        public ImportType ImportType { get; }

        /// <summary>
        /// Whether or not the formula is exported.
        /// </summary>
        public bool IsExported { get; }

        /// <summary>
        /// The identifier that this node assigns to.
        /// </summary>
        public string Name { get; }

        /// <inheritdoc cref="ISyntaxNode.Children"/>
        public IEnumerable<ISyntaxNode> Children()
        {
            yield break;
        }

        /// <inheritdoc cref="ISyntaxNode.ChildrenReverse"/>
        public IEnumerable<ISyntaxNode> ChildrenReverse()
        {
            yield break;
        }

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            return $"{{{nameof(ImportNode)} {ImportType} {Name}}}";
        }
    }
}