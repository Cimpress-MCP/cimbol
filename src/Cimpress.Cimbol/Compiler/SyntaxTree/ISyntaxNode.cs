// Copyright 2021 Cimpress plc.
// Licensed under the Apache License, Version 2.0
// http://www.apache.org/licenses/LICENSE-2.0

using System.Collections.Generic;

namespace Cimpress.Cimbol.Compiler.SyntaxTree
{
    /// <summary>
    /// The base interface for all abstract syntax tree nodes.
    /// </summary>
    public interface ISyntaxNode
    {
        /// <summary>
        /// Iterate over all of the children of this syntax node from left to right.
        /// </summary>
        /// <returns>An enumerable containing this syntax node's children.</returns>
        IEnumerable<ISyntaxNode> Children();

        /// <summary>
        /// Iterate over all of the children of this syntax node from right to left.
        /// </summary>
        /// <returns>An enumerable containing this syntax node's children.</returns>
        IEnumerable<ISyntaxNode> ChildrenReverse();
    }
}