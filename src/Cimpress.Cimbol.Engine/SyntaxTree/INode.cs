using System.Collections.Generic;

namespace Cimpress.Cimbol.Compiler.SyntaxTree
{
    /// <summary>
    /// The base interface for all abstract syntax tree nodes.
    /// </summary>
    public interface INode
    {
        /// <summary>
        /// Iterate over all of the children of this node from left to right.
        /// </summary>
        /// <returns>An enumerable containing this node's children.</returns>
        IEnumerable<INode> Children();

        /// <summary>
        /// Iterate over all of the children of this from right to left.
        /// </summary>
        /// <returns>An enumerable containing this node's children.</returns>
        IEnumerable<INode> ChildrenReverse();
    }
}