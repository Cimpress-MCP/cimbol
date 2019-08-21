using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Cimpress.Cimbol.Compiler.Emit;
using Cimpress.Cimbol.Compiler.SyntaxTree;
using Cimpress.Cimbol.Exceptions;
using Cimpress.Cimbol.Runtime.Functions;
using Cimpress.Cimbol.Runtime.Types;
using NUnit.Framework;

namespace Cimpress.Cimbol.UnitTests.Compiler.Emit
{
    public class EmitterTests
    {
        private Emitter _emitter;

        private SymbolTable _symbolTable;

        [SetUp]
        public void SetUp()
        {
            _emitter = new Emitter();

            _symbolTable = new SymbolTable();
        }

        [Test]
        public void ShouldNot_EmitAnything_When_GivenNullNode()
        {
            var node = (IExpressionNode)null;

            Assert.Throws<NotSupportedException>(() => _emitter.EmitExpression(node, _symbolTable));
        }

        [Test]
        public void ShouldNot_EmitAnything_When_GivenNullSymbolTable()
        {
            var node = new LiteralNode(null);

            Assert.Throws<ArgumentNullException>(() => _emitter.EmitExpression(node, null));
        }

        [Test]
        public void Should_EmitAccess_When_GivenAccessNode()
        {
            var value = new ObjectValue(new Dictionary<string, ILocalValue>
            {
                { "x", new NumberValue(5) },
            });
            var node = new AccessNode(new LiteralNode(value), "x");

            var expression = _emitter.EmitExpression(node, _symbolTable) as MethodCallExpression;

            Assert.That(expression, Is.Not.Null);
            Assert.That(expression.Method, Is.EqualTo(LocalValueFunctions.AccessInfo));
            Assert.That(expression, Is.InstanceOf(typeof(MethodCallExpression)));
            Assert.That(expression.Type, Is.EqualTo(typeof(ILocalValue)));
        }

        [Test]
        [TestCaseSource(typeof(EmitterTests), nameof(BinaryOpNodeTestCases))]
        public void Should_EmitMethodCallExpression_When_GivenBinaryOpNode(
            BinaryOpType opType,
            MethodInfo methodInfo,
            Type type)
        {
            var value1 = new NumberValue(2);
            var value2 = new NumberValue(3);
            var node = new BinaryOpNode(opType, new LiteralNode(value1), new LiteralNode(value2));

            var expression = _emitter.EmitExpression(node, _symbolTable) as MethodCallExpression;

            Assert.That(expression, Is.Not.Null);
            Assert.That(expression.Method, Is.EqualTo(methodInfo));
            Assert.That(expression, Is.InstanceOf(typeof(MethodCallExpression)));
            Assert.That(expression.Type, Is.EqualTo(type));
        }

        [Test]
        public void ShouldNot_EmitMethodCallExpression_When_GivenInvalidBinaryOpType()
        {
            var value1 = new NumberValue(2);
            var value2 = new NumberValue(3);
            var node = new BinaryOpNode((BinaryOpType)(-1), new LiteralNode(value1), new LiteralNode(value2));

            Assert.Throws<ArgumentOutOfRangeException>(() => _emitter.EmitExpression(node, _symbolTable));
        }

        [Test]
        public void Should_EmitBlock_When_GivenBlockNodeWithOneExpression()
        {
            var value = new NumberValue(5);
            var node = new BlockNode(new[] { new LiteralNode(value) });

            var expression = _emitter.EmitExpression(node, _symbolTable) as BlockExpression;

            Assert.That(expression, Is.Not.Null);
            Assert.That(expression, Is.InstanceOf(typeof(BlockExpression)));
            Assert.That(expression.Expressions, Has.Count.EqualTo(1));
            Assert.That(expression.Type, Is.EqualTo(typeof(NumberValue)));
        }

        [Test]
        public void Should_EmitBlock_When_GivenBlockNodeWithTwoExpressions()
        {
            var value = new NumberValue(5);
            var node = new BlockNode(new[] { new LiteralNode(value), new LiteralNode(value) });

            var expression = _emitter.EmitExpression(node, _symbolTable) as BlockExpression;

            Assert.That(expression, Is.Not.Null);
            Assert.That(expression, Is.InstanceOf(typeof(BlockExpression)));
            Assert.That(expression.Expressions, Has.Count.EqualTo(2));
            Assert.That(expression.Type, Is.EqualTo(typeof(NumberValue)));
        }

        [Test]
        public void Should_EmitIdentifier_When_GivenIdentifierNodeWithIdentifierInScope()
        {
            var node = new IdentifierNode("x");
            var symbolTable = new SymbolTable();
            symbolTable.Define("x", typeof(BooleanValue));

            var expression = _emitter.EmitExpression(node, symbolTable) as ParameterExpression;

            Assert.That(expression, Is.Not.Null);
            Assert.That(expression, Is.InstanceOf(typeof(ParameterExpression)));
            Assert.That(expression.Type, Is.EqualTo(typeof(BooleanValue)));
        }

        [Test]
        public void Should_EmitIdentifier_When_GivenIdentifierNodeWithIdentifierInParentScope()
        {
            var node = new IdentifierNode("x");
            var symbolTable1 = new SymbolTable();
            var symbolTable2 = new SymbolTable(symbolTable1);
            symbolTable1.Define("x", typeof(BooleanValue));

            var expression = _emitter.EmitExpression(node, symbolTable2) as ParameterExpression;

            Assert.That(expression, Is.Not.Null);
            Assert.That(expression, Is.InstanceOf(typeof(ParameterExpression)));
            Assert.That(expression.Type, Is.EqualTo(typeof(BooleanValue)));
        }

        [Test]
        public void Should_EmitError_When_GivenIdentifierNodeWithIdentifierNotInScope()
        {
            var node = new IdentifierNode("x");
            var symbolTable = new SymbolTable();

            var expression = _emitter.EmitExpression(node, symbolTable) as UnaryExpression;

            Assert.That(expression, Is.Not.Null);
            Assert.That(expression, Is.InstanceOf(typeof(UnaryExpression)));
            Assert.That(expression.Type, Is.EqualTo(typeof(ILocalValue)));
        }

        [Test]
        public void Should_EmitInvoke_When_GivenInvokeNodeWithNoArguments()
        {
            NumberValue MyTestFunction()
            {
                return new NumberValue(3);
            }

            var value = new FunctionValue((Func<NumberValue>)MyTestFunction);
            var node = new InvokeNode(new LiteralNode(value), Array.Empty<PositionalArgument>());

            var expression = _emitter.EmitExpression(node, _symbolTable) as MethodCallExpression;

            Assert.That(expression, Is.Not.Null);
            Assert.That(expression, Is.InstanceOf(typeof(MethodCallExpression)));
            Assert.That(expression.Method, Is.EqualTo(LocalValueFunctions.InvokeInfo));
            Assert.That(expression.Type, Is.EqualTo(typeof(ILocalValue)));
        }

        [Test]
        public void Should_EmitInvoke_When_GivenInvokeNodeWithOneArgument()
        {
            NumberValue MyTestFunction(NumberValue argument1)
            {
                return argument1;
            }

            var value = new FunctionValue((Func<NumberValue, NumberValue>)MyTestFunction);
            var arguments = new[]
            {
                new PositionalArgument(new LiteralNode(new NumberValue(5))),
            };
            var node = new InvokeNode(new LiteralNode(value), arguments);

            var expression = _emitter.EmitExpression(node, _symbolTable) as MethodCallExpression;

            Assert.That(expression, Is.Not.Null);
            Assert.That(expression, Is.InstanceOf(typeof(MethodCallExpression)));
            Assert.That(expression.Method, Is.EqualTo(LocalValueFunctions.InvokeInfo));
            Assert.That(expression.Type, Is.EqualTo(typeof(ILocalValue)));
        }

        [Test]
        public void Should_EmitInvoke_When_GivenInvokeNodeWithTwoArguments()
        {
            NumberValue MyTestFunction(NumberValue argument1, NumberValue argument2)
            {
                return new NumberValue(argument1.Value + argument2.Value);
            }

            var value = new FunctionValue((Func<NumberValue, NumberValue, NumberValue>)MyTestFunction);
            var arguments = new[]
            {
                new PositionalArgument(new LiteralNode(new NumberValue(2))),
                new PositionalArgument(new LiteralNode(new NumberValue(3))),
            };
            var node = new InvokeNode(new LiteralNode(value), arguments);

            var expression = _emitter.EmitExpression(node, _symbolTable) as MethodCallExpression;

            Assert.That(expression, Is.Not.Null);
            Assert.That(expression, Is.InstanceOf(typeof(MethodCallExpression)));
            Assert.That(expression.Method, Is.EqualTo(LocalValueFunctions.InvokeInfo));
            Assert.That(expression.Type, Is.EqualTo(typeof(ILocalValue)));
        }

        [Test]
        public void Should_EmitBooleanValueConstantExpression_When_GivenBooleanLiteralNode()
        {
            var value = new BooleanValue(true);
            var node = new LiteralNode(value);

            var expression = _emitter.EmitExpression(node, _symbolTable) as ConstantExpression;

            Assert.That(expression, Is.Not.Null);
            Assert.That(expression, Is.InstanceOf(typeof(ConstantExpression)));
            Assert.That(expression.Type, Is.EqualTo(typeof(BooleanValue)));
            Assert.That(expression.Value, Is.EqualTo(value));
        }

        [Test]
        public void Should_EmitNumberValueConstantExpression_When_GivenNumberLiteralNode()
        {
            var value = new NumberValue(5);
            var node = new LiteralNode(value);

            var expression = _emitter.EmitExpression(node, _symbolTable) as ConstantExpression;

            Assert.That(expression, Is.Not.Null);
            Assert.That(expression, Is.InstanceOf(typeof(ConstantExpression)));
            Assert.That(expression.Type, Is.EqualTo(typeof(NumberValue)));
            Assert.That(expression.Value, Is.EqualTo(value));
        }

        [Test]
        public void Should_EmitStringValueConstantExpression_When_GivenStringLiteralNode()
        {
            var value = new StringValue("cat");
            var node = new LiteralNode(value);

            var expression = _emitter.EmitExpression(node, _symbolTable) as ConstantExpression;

            Assert.That(expression, Is.Not.Null);
            Assert.That(expression, Is.InstanceOf(typeof(ConstantExpression)));
            Assert.That(expression.Type, Is.EqualTo(typeof(StringValue)));
            Assert.That(expression.Value, Is.EqualTo(value));
        }

        [Test]
        public void Should_EmitConditionExpression_When_GivenMacroIfElseNode()
        {
            var arguments = new IArgument[]
            {
                new PositionalArgument(new LiteralNode(BooleanValue.True)),
                new NamedArgument("else", new LiteralNode(BooleanValue.True)),
            };
            var node = new MacroNode("if", arguments);

            var expression = _emitter.EmitExpression(node, _symbolTable) as ConditionalExpression;

            Assert.That(expression, Is.Not.Null);
            Assert.That(expression, Is.InstanceOf(typeof(ConditionalExpression)));
            Assert.That(expression.Type, Is.EqualTo(typeof(ILocalValue)));
            Assert.That(expression.IfFalse, Is.InstanceOf(typeof(ConstantExpression)));
            Assert.That(expression.IfTrue, Is.InstanceOf(typeof(UnaryExpression)));
        }

        [Test]
        public void Should_EmitConditionExpression_When_GivenMacroIfElseThenNode()
        {
            var arguments = new IArgument[]
            {
                new PositionalArgument(new LiteralNode(BooleanValue.True)),
                new NamedArgument("else", new LiteralNode(BooleanValue.True)),
                new NamedArgument("then", new LiteralNode(BooleanValue.True)),
            };
            var node = new MacroNode("if", arguments);

            var expression = _emitter.EmitExpression(node, _symbolTable) as ConditionalExpression;

            Assert.That(expression, Is.Not.Null);
            Assert.That(expression, Is.InstanceOf(typeof(ConditionalExpression)));
            Assert.That(expression.Type, Is.EqualTo(typeof(ILocalValue)));
            Assert.That(expression.IfFalse, Is.InstanceOf(typeof(ConstantExpression)));
            Assert.That(expression.IfTrue, Is.InstanceOf(typeof(ConstantExpression)));
        }

        [Test]
        public void Should_EmitConditionExpression_When_GivenMacroIfThenNode()
        {
            var arguments = new IArgument[]
            {
                new PositionalArgument(new LiteralNode(BooleanValue.True)),
                new NamedArgument("then", new LiteralNode(BooleanValue.True)),
            };
            var node = new MacroNode("if", arguments);

            var expression = _emitter.EmitExpression(node, _symbolTable) as ConditionalExpression;

            Assert.That(expression, Is.Not.Null);
            Assert.That(expression, Is.InstanceOf(typeof(ConditionalExpression)));
            Assert.That(expression.Type, Is.EqualTo(typeof(ILocalValue)));
            Assert.That(expression.IfFalse, Is.InstanceOf(typeof(UnaryExpression)));
            Assert.That(expression.IfTrue, Is.InstanceOf(typeof(ConstantExpression)));
        }

        [Test]
        public void Should_EmitConditionExpression_When_GivenMacroIfThenElseNode()
        {
            var arguments = new IArgument[]
            {
                new PositionalArgument(new LiteralNode(BooleanValue.True)),
                new NamedArgument("then", new LiteralNode(BooleanValue.True)),
                new NamedArgument("else", new LiteralNode(BooleanValue.True)),
            };
            var node = new MacroNode("if", arguments);

            var expression = _emitter.EmitExpression(node, _symbolTable) as ConditionalExpression;

            Assert.That(expression, Is.Not.Null);
            Assert.That(expression, Is.InstanceOf(typeof(ConditionalExpression)));
            Assert.That(expression.Type, Is.EqualTo(typeof(ILocalValue)));
            Assert.That(expression.IfFalse, Is.InstanceOf(typeof(ConstantExpression)));
            Assert.That(expression.IfTrue, Is.InstanceOf(typeof(ConstantExpression)));
        }

        [Test]
        public void ShouldNot_EmitConditionExpression_When_GivenMacroIfWithNoBranches()
        {
            var arguments = new IArgument[]
            {
                new PositionalArgument(new LiteralNode(BooleanValue.True)),
            };
            var node = new MacroNode("if", arguments);

            Assert.Throws<CimbolCompilationException>(() => _emitter.EmitExpression(node, _symbolTable));
        }

        [Test]
        public void ShouldNot_EmitConditionExpression_When_GivenMacroIfWithAbnormalBranches()
        {
            var arguments = new IArgument[]
            {
                new PositionalArgument(new LiteralNode(BooleanValue.True)),
                new NamedArgument("cat", new LiteralNode(BooleanValue.True)),
            };
            var node = new MacroNode("if", arguments);

            Assert.Throws<NotSupportedException>(() => _emitter.EmitExpression(node, _symbolTable));
        }

        [Test]
        public void Should_EmitConstructor_When_GivenMacroListWithNoItems()
        {
            var arguments = Array.Empty<IArgument>();
            var node = new MacroNode("list", arguments);

            var expression = _emitter.EmitExpression(node, _symbolTable) as NewExpression;

            Assert.That(expression, Is.Not.Null);
            Assert.That(expression, Is.InstanceOf(typeof(NewExpression)));
            Assert.That(expression.Type, Is.EqualTo(typeof(ListValue)));
        }

        [Test]
        public void Should_EmitConstructor_When_GivenMacroListWithOneItem()
        {
            var arguments = new IArgument[]
            {
                new PositionalArgument(new LiteralNode(BooleanValue.True)),
            };
            var node = new MacroNode("list", arguments);

            var expression = _emitter.EmitExpression(node, _symbolTable) as NewExpression;

            Assert.That(expression, Is.Not.Null);
            Assert.That(expression, Is.InstanceOf(typeof(NewExpression)));
            Assert.That(expression.Type, Is.EqualTo(typeof(ListValue)));
        }

        [Test]
        public void Should_EmitConstructor_When_GivenMacroListWithTwoItems()
        {
            var arguments = new IArgument[]
            {
                new PositionalArgument(new LiteralNode(BooleanValue.True)),
                new PositionalArgument(new LiteralNode(BooleanValue.True)),
            };
            var node = new MacroNode("list", arguments);

            var expression = _emitter.EmitExpression(node, _symbolTable) as NewExpression;

            Assert.That(expression, Is.Not.Null);
            Assert.That(expression, Is.InstanceOf(typeof(NewExpression)));
            Assert.That(expression.Type, Is.EqualTo(typeof(ListValue)));
        }

        [Test]
        public void Should_EmitConstructor_When_GivenMacroObjectWithNoItems()
        {
            var arguments = Array.Empty<IArgument>();
            var node = new MacroNode("object", arguments);

            var expression = _emitter.EmitExpression(node, _symbolTable) as NewExpression;

            Assert.That(expression, Is.Not.Null);
            Assert.That(expression, Is.InstanceOf(typeof(NewExpression)));
            Assert.That(expression.Type, Is.EqualTo(typeof(ObjectValue)));
        }

        [Test]
        public void Should_EmitConstructor_When_GivenMacroWhereWithOneItem()
        {
            var arguments = new IArgument[]
            {
                new NamedArgument("x", new LiteralNode(BooleanValue.True)), 
            };
            var node = new MacroNode("object", arguments);

            var expression = _emitter.EmitExpression(node, _symbolTable) as NewExpression;

            Assert.That(expression, Is.Not.Null);
            Assert.That(expression, Is.InstanceOf(typeof(NewExpression)));
            Assert.That(expression.Type, Is.EqualTo(typeof(ObjectValue)));
        }

        [Test]
        public void Should_EmitConstructor_When_GivenMacroWhereWithTwoItems()
        {
            var arguments = new IArgument[]
            {
                new NamedArgument("x", new LiteralNode(BooleanValue.True)), 
                new NamedArgument("y", new LiteralNode(BooleanValue.True)), 
            };
            var node = new MacroNode("object", arguments);

            var expression = _emitter.EmitExpression(node, _symbolTable) as NewExpression;

            Assert.That(expression, Is.Not.Null);
            Assert.That(expression, Is.InstanceOf(typeof(NewExpression)));
            Assert.That(expression.Type, Is.EqualTo(typeof(ObjectValue)));
        }

        [Test]
        public void Should_EmitLiteralExpression_When_GivenMacroWhereWithDefault()
        {
            var arguments = new IArgument[]
            {
                new NamedArgument("default", new LiteralNode(BooleanValue.True)),
            };
            var node = new MacroNode("where", arguments);

            var expression = _emitter.EmitExpression(node, _symbolTable) as ConstantExpression;

            Assert.That(expression, Is.Not.Null);
            Assert.That(expression, Is.InstanceOf(typeof(ConstantExpression)));
            Assert.That(expression.Type, Is.EqualTo(typeof(BooleanValue)));
        }

        [Test]
        public void Should_EmitConditionExpression_When_GivenMacroWhereWithOneCase()
        {
            var arguments = new IArgument[]
            {
                new NamedArgument("case", new LiteralNode(BooleanValue.True)),
                new NamedArgument("do", new LiteralNode(BooleanValue.True)),
            };
            var node = new MacroNode("where", arguments);

            var expression = _emitter.EmitExpression(node, _symbolTable) as ConditionalExpression;

            Assert.That(expression, Is.Not.Null);
            Assert.That(expression, Is.InstanceOf(typeof(ConditionalExpression)));
            Assert.That(expression.Type, Is.EqualTo(typeof(ILocalValue)));
            Assert.That(expression.IfFalse, Is.InstanceOf(typeof(UnaryExpression)));
            Assert.That(expression.IfTrue, Is.InstanceOf(typeof(ConstantExpression)));
        }

        [Test]
        public void Should_EmitConditionExpression_When_GivenMacroWhereWithOneCaseAndDefault()
        {
            var arguments = new IArgument[]
            {
                new NamedArgument("case", new LiteralNode(BooleanValue.True)),
                new NamedArgument("do", new LiteralNode(BooleanValue.True)),
                new NamedArgument("default", new LiteralNode(BooleanValue.True)),
            };
            var node = new MacroNode("where", arguments);

            var expression = _emitter.EmitExpression(node, _symbolTable) as ConditionalExpression;

            Assert.That(expression, Is.Not.Null);
            Assert.That(expression, Is.InstanceOf(typeof(ConditionalExpression)));
            Assert.That(expression.Type, Is.EqualTo(typeof(ILocalValue)));
            Assert.That(expression.IfFalse, Is.InstanceOf(typeof(ConstantExpression)));
            Assert.That(expression.IfTrue, Is.InstanceOf(typeof(ConstantExpression)));
        }

        [Test]
        public void Should_EmitConditionExpression_When_GivenMacroWhereWithTwoCases()
        {
            var arguments = new IArgument[]
            {
                new NamedArgument("case", new LiteralNode(BooleanValue.True)),
                new NamedArgument("do", new LiteralNode(BooleanValue.True)),
                new NamedArgument("case", new LiteralNode(BooleanValue.True)),
                new NamedArgument("do", new LiteralNode(BooleanValue.True)),
            };
            var node = new MacroNode("where", arguments);

            var expression = _emitter.EmitExpression(node, _symbolTable) as ConditionalExpression;

            Assert.That(expression, Is.Not.Null);
            Assert.That(expression, Is.InstanceOf(typeof(ConditionalExpression)));
            Assert.That(expression.Type, Is.EqualTo(typeof(ILocalValue)));
            Assert.That(expression.IfFalse, Is.InstanceOf(typeof(ConditionalExpression)));
            Assert.That(expression.IfTrue, Is.InstanceOf(typeof(ConstantExpression)));
        }

        [Test]
        public void Should_EmitConditionExpression_When_GivenMacroWhereWithTwoCasesAndDefault()
        {
            var arguments = new IArgument[]
            {
                new NamedArgument("case", new LiteralNode(BooleanValue.True)),
                new NamedArgument("do", new LiteralNode(BooleanValue.True)),
                new NamedArgument("case", new LiteralNode(BooleanValue.True)),
                new NamedArgument("do", new LiteralNode(BooleanValue.True)),
                new NamedArgument("default", new LiteralNode(BooleanValue.True)),
            };
            var node = new MacroNode("where", arguments);

            var expression = _emitter.EmitExpression(node, _symbolTable) as ConditionalExpression;

            Assert.That(expression, Is.Not.Null);
            Assert.That(expression, Is.InstanceOf(typeof(ConditionalExpression)));
            Assert.That(expression.Type, Is.EqualTo(typeof(ILocalValue)));
            Assert.That(expression.IfFalse, Is.InstanceOf(typeof(ConditionalExpression)));
            Assert.That(expression.IfTrue, Is.InstanceOf(typeof(ConstantExpression)));
        }

        [Test]
        public void ShouldNot_EmitConditionExpression_When_GivenNoCasesOrDefault()
        {
            var arguments = Array.Empty<IArgument>();
            var node = new MacroNode("where", arguments);

            Assert.Throws<CimbolCompilationException>(() => _emitter.EmitExpression(node, _symbolTable));
        }

        [Test]
        public void ShouldNot_EmitAnything_When_GivenInvalidMacroName()
        {
            var arguments = Array.Empty<IArgument>();
            var node = new MacroNode("cat", arguments);

            Assert.Throws<NotSupportedException>(() => _emitter.EmitExpression(node, _symbolTable));
        }

        [Test]
        [TestCaseSource(typeof(EmitterTests), nameof(UnaryOpNodeTestCases))]
        public void Should_EmitMethodCallExpression_When_GivenUnaryOpNode(
            UnaryOpType opType,
            MethodInfo methodInfo,
            Type type)
        {
            var value1 = new NumberValue(5);
            var node = new UnaryOpNode(opType, new LiteralNode(value1));

            var expression = _emitter.EmitExpression(node, _symbolTable) as MethodCallExpression;

            Assert.That(expression, Is.Not.Null);
            Assert.That(expression.Method, Is.EqualTo(methodInfo));
            Assert.That(expression, Is.InstanceOf(typeof(MethodCallExpression)));
            Assert.That(expression.Type, Is.EqualTo(type));
        }

        [Test]
        public void ShouldNot_EmitMethodCallExpression_When_GivenInvalidUnaryOpType()
        {
            var value1 = new NumberValue(5);
            var node = new UnaryOpNode((UnaryOpType)(-1), new LiteralNode(value1));

            Assert.Throws<ArgumentOutOfRangeException>(() => _emitter.EmitExpression(node, _symbolTable));
        }

        private static IEnumerable<TestCaseData> BinaryOpNodeTestCases()
        {
            yield return new TestCaseData(BinaryOpType.Add, RuntimeFunctions.MathAddInfo, typeof(NumberValue));
            yield return new TestCaseData(BinaryOpType.And, RuntimeFunctions.BooleanAndInfo, typeof(BooleanValue));
            yield return new TestCaseData(BinaryOpType.Concatenate, RuntimeFunctions.StringConcatenateInfo, typeof(StringValue));
            yield return new TestCaseData(BinaryOpType.Divide, RuntimeFunctions.MathDivideInfo, typeof(NumberValue));
            yield return new TestCaseData(BinaryOpType.Equal, RuntimeFunctions.EqualToInfo, typeof(BooleanValue));
            yield return new TestCaseData(BinaryOpType.GreaterThan, RuntimeFunctions.CompareGreaterThanInfo, typeof(BooleanValue));
            yield return new TestCaseData(BinaryOpType.GreaterThanOrEqual, RuntimeFunctions.CompareGreaterThanOrEqualInfo, typeof(BooleanValue));
            yield return new TestCaseData(BinaryOpType.LessThan, RuntimeFunctions.CompareLessThanInfo, typeof(BooleanValue));
            yield return new TestCaseData(BinaryOpType.LessThanOrEqual, RuntimeFunctions.CompareLessThanOrEqualInfo, typeof(BooleanValue));
            yield return new TestCaseData(BinaryOpType.Multiply, RuntimeFunctions.MathMultiplyInfo, typeof(NumberValue));
            yield return new TestCaseData(BinaryOpType.NotEqual, RuntimeFunctions.NotEqualToInfo, typeof(BooleanValue));
            yield return new TestCaseData(BinaryOpType.Or, RuntimeFunctions.BooleanOrInfo, typeof(BooleanValue));
            yield return new TestCaseData(BinaryOpType.Power, RuntimeFunctions.MathPowerInfo, typeof(NumberValue));
            yield return new TestCaseData(BinaryOpType.Remainder, RuntimeFunctions.MathRemainderInfo, typeof(NumberValue));
            yield return new TestCaseData(BinaryOpType.Subtract, RuntimeFunctions.MathSubtractInfo, typeof(NumberValue));
        }

        private static IEnumerable<TestCaseData> UnaryOpNodeTestCases()
        {
            yield return new TestCaseData(UnaryOpType.Negate, RuntimeFunctions.MathNegateInfo, typeof(NumberValue));
            yield return new TestCaseData(UnaryOpType.Not, RuntimeFunctions.BooleanNotInfo, typeof(BooleanValue));
        }
    }
}