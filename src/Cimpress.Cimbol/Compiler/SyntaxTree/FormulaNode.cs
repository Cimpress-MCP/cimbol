﻿// Copyright 2021 Cimpress plc.
// Licensed under the Apache License, Version 2.0
// http://www.apache.org/licenses/LICENSE-2.0

using System;
using System.Collections.Generic;

namespace Cimpress.Cimbol.Compiler.SyntaxTree
{
    /// <summary>
    /// A syntax node representing an entire formula.
    /// </summary>
    public class FormulaNode : IDeclarationNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FormulaNode"/> class.
        /// </summary>
        /// <param name="name">The name to assign to the formula's result.</param>
        /// <param name="body">The body of the formula.</param>
        /// <param name="isExported">Whether or not the formula is exported.</param>
        public FormulaNode(string name, IExpressionNode body, bool isExported)
        {
            Body = body ?? throw new ArgumentNullException(nameof(body));

            IsAsynchronous = Body.IsAsynchronous;

            IsExported = isExported;

            Name = name;
        }

        /// <summary>
        /// The body of the formula.
        /// </summary>
        public IExpressionNode Body { get; }

        /// <summary>
        /// Whether or not the formula is asynchronous.
        /// </summary>
        public bool IsAsynchronous { get; }

        /// <summary>
        /// Whether or not the formula is exported.
        /// </summary>
        public bool IsExported { get; }

        /// <inheritdoc cref="IDeclarationNode.Name"/>
        public string Name { get; }

        /// <inheritdoc cref="ISyntaxNode.Children"/>
        public IEnumerable<ISyntaxNode> Children()
        {
            yield return Body;
        }

        /// <inheritdoc cref="ISyntaxNode.ChildrenReverse"/>
        public IEnumerable<ISyntaxNode> ChildrenReverse()
        {
            yield return Body;
        }

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            return $"{{{nameof(FormulaNode)} {Name}}}";
        }
    }
}