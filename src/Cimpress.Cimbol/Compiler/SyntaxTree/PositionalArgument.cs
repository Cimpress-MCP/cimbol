// Copyright 2021 Cimpress plc.
// Licensed under the Apache License, Version 2.0
// http://www.apache.org/licenses/LICENSE-2.0

using System;

namespace Cimpress.Cimbol.Compiler.SyntaxTree
{
    /// <summary>
    /// A positional argument.
    /// </summary>
    public class PositionalArgument : IArgument
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PositionalArgument"/> class.
        /// </summary>
        /// <param name="value">The value of the argument.</param>
        public PositionalArgument(IExpressionNode value)
        {
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// The value of the argument.
        /// </summary>
        public IExpressionNode Value { get; }
    }
}