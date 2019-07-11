namespace Cimpress.Cimbol.Compiler.SyntaxTree
{
    /// <summary>
    /// The interface for different types of arguments.
    /// </summary>
    public interface IArgument
    {
        /// <summary>
        /// The value of the argument.
        /// </summary>
        INode Value { get; }
    }
}