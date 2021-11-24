// Copyright 2021 Cimpress plc.
// Licensed under the Apache License, Version 2.0
// http://www.apache.org/licenses/LICENSE-2.0

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
        IExpressionNode Value { get; }
    }
}