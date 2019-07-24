using System.Collections.Generic;

namespace Cimpress.Cimbol.Compiler.SyntaxTree
{
    /// <summary>
    /// A syntax node representing the exporting of a result from another module.
    /// </summary>
    public class ExportStatementNode : IStatementNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExportStatementNode"/> class.
        /// </summary>
        /// <param name="identifier">The identifier that this node exports from a module.</param>
        public ExportStatementNode(string identifier)
        {
            Identifier = identifier;
        }

        /// <summary>
        /// The identifier that this node exports from a module.
        /// </summary>
        public string Identifier { get; }

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
            return $"{{{nameof(ExportStatementNode)} {Identifier}}}";
        }
    }
}