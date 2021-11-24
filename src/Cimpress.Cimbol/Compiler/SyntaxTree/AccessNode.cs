// Copyright 2021 Cimpress plc.
// Licensed under the Apache License, Version 2.0
// http://www.apache.org/licenses/LICENSE-2.0

using System;
using System.Collections.Generic;

namespace Cimpress.Cimbol.Compiler.SyntaxTree
{
    /// <summary>
    /// A syntax tree node representing member access.
    /// </summary>
    public class AccessNode : IExpressionNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AccessNode"/> class.
        /// </summary>
        /// <param name="value">The value to access a member of.</param>
        /// <param name="member">The name of the member to access.</param>
        public AccessNode(IExpressionNode value, string member)
        {
            Member = member;

            Value = value ?? throw new ArgumentNullException(nameof(value));

            IsAsynchronous = Value.IsAsynchronous;
        }

        /// <inheritdoc cref="IExpressionNode.IsAsynchronous"/>
        public bool IsAsynchronous { get; }

        /// <summary>
        /// The name of the member to access.
        /// </summary>
        public string Member { get; }

        /// <summary>
        /// The value to access a member of.
        /// </summary>
        public IExpressionNode Value { get; }

        /// <inheritdoc cref="ISyntaxNode.Children"/>
        public IEnumerable<ISyntaxNode> Children()
        {
            yield return Value;
        }

        /// <inheritdoc cref="ISyntaxNode.ChildrenReverse"/>
        public IEnumerable<ISyntaxNode> ChildrenReverse()
        {
            yield return Value;
        }

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            return $"{{{nameof(AccessNode)} {Member}}}";
        }
    }
}