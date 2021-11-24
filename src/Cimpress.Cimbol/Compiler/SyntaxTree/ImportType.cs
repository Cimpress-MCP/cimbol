// Copyright 2021 Cimpress plc.
// Licensed under the Apache License, Version 2.0
// http://www.apache.org/licenses/LICENSE-2.0

namespace Cimpress.Cimbol.Compiler.SyntaxTree
{
    /// <summary>
    /// The possible import types.
    /// </summary>
    public enum ImportType
    {
        /// <summary>
        /// Represents the import of an argument.
        /// </summary>
        Argument,

        /// <summary>
        /// Represents the import of a constant.
        /// </summary>
        Constant,

        /// <summary>
        /// Represents the import of a formula.
        /// </summary>
        Formula,

        /// <summary>
        /// Represents the import of a module.
        /// </summary>
        Module,
    }
}