﻿// Copyright 2021 Cimpress plc.
// Licensed under the Apache License, Version 2.0
// http://www.apache.org/licenses/LICENSE-2.0

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Cimpress.Cimbol.Compiler.SyntaxTree
{
    /// <summary>
    /// A syntax tree node representing a function invocation.
    /// </summary>
    public sealed class InvokeNode : IExpressionNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvokeNode"/> class.
        /// </summary>
        /// <param name="function">The function to invoke.</param>
        /// <param name="arguments">The arguments to invoke the function with.</param>
        public InvokeNode(IExpressionNode function, IEnumerable<PositionalArgument> arguments)
        {
            Arguments = arguments?.ToImmutableArray() ?? throw new ArgumentNullException(nameof(arguments));

            Function = function ?? throw new ArgumentNullException(nameof(function));

            IsAsynchronous = Function.IsAsynchronous || Arguments.Any(argument => argument.Value.IsAsynchronous);
        }

        /// <summary>
        /// The arguments to invoke the function with.
        /// </summary>
        public ImmutableArray<PositionalArgument> Arguments { get; }

        /// <summary>
        /// The function to invoke.
        /// </summary>
        public IExpressionNode Function { get; }

        /// <inheritdoc cref="IExpressionNode.IsAsynchronous"/>
        public bool IsAsynchronous { get; }

        /// <inheritdoc cref="ISyntaxNode.Children"/>
        public IEnumerable<ISyntaxNode> Children()
        {
            yield return Function;

            foreach (var argument in Arguments)
            {
                yield return argument.Value;
            }
        }

        /// <inheritdoc cref="ISyntaxNode.ChildrenReverse"/>
        public IEnumerable<ISyntaxNode> ChildrenReverse()
        {
            foreach (var argument in Arguments.Reverse())
            {
                yield return argument.Value;
            }

            yield return Function;
        }

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            return $"{{{nameof(InvokeNode)}}}";
        }
    }
}