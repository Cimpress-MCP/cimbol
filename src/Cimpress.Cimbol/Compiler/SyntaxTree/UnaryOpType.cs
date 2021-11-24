// Copyright 2021 Cimpress plc.
// Licensed under the Apache License, Version 2.0
// http://www.apache.org/licenses/LICENSE-2.0

namespace Cimpress.Cimbol.Compiler.SyntaxTree
{
    /// <summary>
    /// The possible unary operation types.
    /// </summary>
    public enum UnaryOpType
    {
        /// <summary>
        /// Represents the await operation.
        /// </summary>
        Await,

        /// <summary>
        /// Represents the negate arithmetic unary operation.
        /// </summary>
        Negate,

        /// <summary>
        /// Represents the not logical binary operation.
        /// </summary>
        Not,
    }
}