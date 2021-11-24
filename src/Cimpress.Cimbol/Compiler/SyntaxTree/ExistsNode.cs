// Copyright 2021 Cimpress plc.
// Licensed under the Apache License, Version 2.0
// http://www.apache.org/licenses/LICENSE-2.0

using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Cimpress.Cimbol.Compiler.SyntaxTree
{
    /// <summary>
    /// An expression tree representing usage of the exists function.
    /// </summary>
    /// <seealso cref="Cimpress.Cimbol.Compiler.SyntaxTree.IExpressionNode" />
    public class ExistsNode : IExpressionNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExistsNode"/> class.
        /// </summary>
        /// <param name="path">The path to attempt to access.</param>
        public ExistsNode(IEnumerable<string> path)
        {
            IsAsynchronous = false;

            Path = path?.ToImmutableArray() ?? throw new ArgumentNullException(nameof(path));
        }

        /// <inheritdoc cref="IExpressionNode.IsAsynchronous"/>
        public bool IsAsynchronous { get; }

        /// <summary>
        /// The path to attempt to access.
        /// </summary>
        public IReadOnlyCollection<string> Path { get; }

        /// <inheritdoc cref="ISyntaxNode.Children"/>
        public IEnumerable<ISyntaxNode> Children()
        {
            return new List<ISyntaxNode>();
        }

        /// <inheritdoc cref="ISyntaxNode.ChildrenReverse"/>
        public IEnumerable<ISyntaxNode> ChildrenReverse()
        {
            return new List<ISyntaxNode>();
        }

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            return $"{{{nameof(ExistsNode)}}}";
        }
    }
}
