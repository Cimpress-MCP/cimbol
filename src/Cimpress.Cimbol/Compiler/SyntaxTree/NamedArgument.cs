// Copyright 2021 Cimpress plc.
// Licensed under the Apache License, Version 2.0
// http://www.apache.org/licenses/LICENSE-2.0

using System;

namespace Cimpress.Cimbol.Compiler.SyntaxTree
{
    /// <summary>
    /// A keyword argument.
    /// </summary>
    public class NamedArgument : IArgument
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PositionalArgument"/> class.
        /// </summary>
        /// <param name="name">The name of the argument.</param>
        /// <param name="value">The value of the argument.</param>
        public NamedArgument(string name, IExpressionNode value)
        {
            Name = name;

            Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// The name of the argument.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The value of the argument.
        /// </summary>
        public IExpressionNode Value { get; }
    }
}