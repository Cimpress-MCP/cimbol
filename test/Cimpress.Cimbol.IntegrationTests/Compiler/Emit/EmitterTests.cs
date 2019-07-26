using System.Collections.Generic;
using Cimpress.Cimbol.Compiler.Emit;
using Cimpress.Cimbol.Compiler.SyntaxTree;
using Cimpress.Cimbol.Runtime.Types;
using NUnit.Framework;

namespace Cimpress.Cimbol.IntegrationTests.Compiler.Emit
{
    [TestFixture]
    public class EmitterTests
    {
        [Test]
        public void Should_AccessMember_When_GivenObjectValue()
        {
            var value = new ObjectValue(new Dictionary<string, ILocalValue>
            {
                { "x", new NumberValue(5) },
            });
            var emitter = new Emitter();
            var node = new AccessNode(new LiteralNode(value), "x");

            var expression = emitter.EmitExpression(node, new SymbolTable());
            var function = EmitTestUtilities.WrapAndCompile(expression);
            var result = function() as NumberValue;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.EqualTo(5));
        }

        [Test]
        public void Should_EmitAdd_When_GivenTwoNumbers()
        {
            var value1 = new NumberValue(2);
            var value2 = new NumberValue(3);
            var emitter = new Emitter();
            var node = new BinaryOpNode(BinaryOpType.Add, new LiteralNode(value1), new LiteralNode(value2));

            var expression = emitter.EmitExpression(node, new SymbolTable());
            var function = EmitTestUtilities.WrapAndCompile(expression);
            var result = function() as NumberValue;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.EqualTo(5));
        }

        [Test]
        public void Should_ReturnLiteral_When_GivenLiteralNode()
        {
            var emitter = new Emitter();
            var node = new LiteralNode(new NumberValue(5));

            var expression = emitter.EmitExpression(node, new SymbolTable());
            var function = EmitTestUtilities.WrapAndCompile(expression);
            var result = function() as NumberValue;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.EqualTo(5));
        }
    }
}
