namespace Cimpress.Cimbol.Compiler.SyntaxTree
{
    /// <summary>
    /// An interface for all syntax tree nodes that are expressions.
    /// </summary>
    public interface IExpressionNode : ISyntaxNode
    {
        /// <summary>
        /// Whether or not the expression is asynchronous.
        /// </summary>
        bool IsAsynchronous { get; }
    }
}