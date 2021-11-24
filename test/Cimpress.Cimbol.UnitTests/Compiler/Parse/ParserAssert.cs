// Copyright 2021 Cimpress plc.
// Licensed under the Apache License, Version 2.0
// http://www.apache.org/licenses/LICENSE-2.0

using System;
using Cimpress.Cimbol.Compiler.SyntaxTree;
using NUnit.Framework;

namespace Cimpress.Cimbol.UnitTests.Compiler.Parse
{
    public static class ParserAssert
    {
        internal static TNode Parses<TNode>(Func<ISyntaxNode> parserCallback)
        {
            Assert.That(parserCallback, Is.Not.Null);

            ISyntaxNode parseResult = null;
            Assert.That(() => parseResult = parserCallback(), Throws.Nothing);
            Assert.That(parseResult, Is.Not.Null);
            Assert.That(parseResult, Is.TypeOf<TNode>());

            return (TNode)parseResult;
        }

        internal static TProperty HasProperty<TProperty>(ISyntaxNode node, string propertyName)
        {
            Assert.That(node, Is.Not.Null);

            var propertyInfo = node.GetType().GetProperty(propertyName);
            Assert.That(propertyInfo, Is.Not.Null);

            var property = propertyInfo.GetMethod.Invoke(node, Array.Empty<object>());
            Assert.That(property, Is.InstanceOf<TProperty>());

            return (TProperty)property;
        }
    }
}