using System;
using Cimpress.Cimbol.Compiler.SyntaxTree;
using NUnit.Framework;

namespace Cimpress.Cimbol.UnitTests.Compiler.Parse
{
    public static class ParserAssert
    {
        public static TNode Parses<TNode>(Func<INode> parserCallback)
        {
            Assert.That(parserCallback, Is.Not.Null);

            INode parseResult = null;
            Assert.That(() => parseResult = parserCallback(), Throws.Nothing);
            Assert.That(parseResult, Is.Not.Null);
            Assert.That(parseResult, Is.TypeOf<TNode>());

            return (TNode)parseResult;
        }

        public static TProperty HasProperty<TProperty>(INode node, string propertyName)
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