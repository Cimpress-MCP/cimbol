namespace Cimpress.Cimbol.Compiler.SyntaxTree
{
    /// <summary>
    /// An interface for all syntax tree nodes that are declarations.
    /// </summary>
    public interface IDeclarationNode : ISyntaxNode
    {
        /// <summary>
        /// The name to assign to the declared value.
        /// </summary>
        string Name { get; }
    }
}