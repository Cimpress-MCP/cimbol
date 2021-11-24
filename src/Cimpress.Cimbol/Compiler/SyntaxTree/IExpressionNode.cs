// Copyright 2021 Cimpress plc.
// Licensed under the Apache License, Version 2.0
// http://www.apache.org/licenses/LICENSE-2.0

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