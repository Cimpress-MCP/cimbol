// Copyright 2021 Cimpress plc.
// Licensed under the Apache License, Version 2.0
// http://www.apache.org/licenses/LICENSE-2.0

using System;
using System.Collections.Generic;
using System.Linq;
using Cimpress.Cimbol.Exceptions;
using Cimpress.Cimbol.Runtime.Types;
using NUnit.Framework;

namespace Cimpress.Cimbol.UnitTests.Runtime.Types
{
    [TestFixture]
    public class FunctionValueTests
    {
        [Test]
        public void Should_ThrowException_When_AccessingAnElement()
        {
            var functionValue = new FunctionValue(new[] { (Func<NumberValue>)MockFunction });
            Assert.Throws<CimbolRuntimeException>(() => functionValue.Access("test"));
        }

        [Test]
        public void Should_ThrowException_When_ConvertingToBoolean()
        {
            var functionValue = new FunctionValue(new[] { (Func<NumberValue>)MockFunction });
            Assert.Throws<CimbolRuntimeException>(() => functionValue.CastBoolean());
        }

        [Test]
        public void Should_ThrowException_When_ConvertingToNumber()
        {
            var functionValue = new FunctionValue(new[] { (Func<NumberValue>)MockFunction });
            Assert.Throws<CimbolRuntimeException>(() => functionValue.CastNumber());
        }

        [Test]
        public void Should_ThrowException_When_ConvertingToString()
        {
            var functionValue = new FunctionValue(new[] { (Func<NumberValue>)MockFunction });
            Assert.Throws<CimbolRuntimeException>(() => functionValue.CastString());
        }

        [Test]
        public void Should_PassBooleans_When_BooleanFunctionInvokedWithBoolean()
        {
            BooleanValue TestFunction(BooleanValue booleanValue)
            {
                return booleanValue;
            }

            var functionValue = new FunctionValue(new[] { (Func<BooleanValue, BooleanValue>)TestFunction });

            var result = functionValue.Invoke(new BooleanValue(true)) as BooleanValue;
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result.Value);
        }

        [Test]
        public void Should_ThrowException_When_BooleanFunctionInvokedWithNumber()
        {
            BooleanValue TestFunction(BooleanValue booleanValue)
            {
                return booleanValue;
            }

            var functionValue = new FunctionValue(new[] { (Func<BooleanValue, BooleanValue>)TestFunction });

            Assert.Throws<CimbolRuntimeException>(() => functionValue.Invoke(new NumberValue(1)));
        }

        [Test]
        public void Should_ThrowException_When_BooleanFunctionInvokedWithString()
        {
            BooleanValue TestFunction(BooleanValue booleanValue)
            {
                return booleanValue;
            }

            var functionValue = new FunctionValue(new[] { (Func<BooleanValue, BooleanValue>)TestFunction });

            Assert.Throws<CimbolRuntimeException>(() => functionValue.Invoke(new StringValue("test")));
        }

        [Test]
        public void Should_PassFunctions_When_FunctionFunctionInvokedWithFunction()
        {
            FunctionValue TestFunction(FunctionValue otherFunctionValue)
            {
                return otherFunctionValue;
            }

            var innerFunctionValue = new FunctionValue(new[] { (Func<NumberValue>)MockFunction });
            var functionValue = new FunctionValue(new[] { (Func<FunctionValue, FunctionValue>)TestFunction });

            var functionResult = functionValue.Invoke(innerFunctionValue) as FunctionValue;
            Assert.IsNotNull(functionResult);
            Assert.AreEqual(innerFunctionValue.Value, functionResult.Value);
        }

        [Test]
        public void Should_ThrowException_When_FunctionFunctionInvokedWithNumber()
        {
            FunctionValue TestFunction(FunctionValue otherFunctionValue)
            {
                return otherFunctionValue;
            }

            var functionValue = new FunctionValue(new[] { (Func<FunctionValue, FunctionValue>)TestFunction });

            Assert.Throws<CimbolRuntimeException>(() => functionValue.Invoke(new NumberValue(1)));
        }

        [Test]
        public void Should_PassLists_When_ListFunctionInvokedWithList()
        {
            ListValue TestFunction(ListValue listValue)
            {
                return listValue;
            }

            var innerListValue = new ListValue(Array.Empty<ILocalValue>());
            var functionValue = new FunctionValue(new[] { (Func<ListValue, ListValue>)TestFunction });

            var listResult = functionValue.Invoke(innerListValue) as ListValue;
            Assert.IsNotNull(listResult);
            Assert.AreEqual(innerListValue.Value, listResult.Value);
        }

        [Test]
        public void Should_ThrowException_When_ListFunctionInvokedWithNumber()
        {
            ListValue TestFunction(ListValue listValue)
            {
                return listValue;
            }

            var functionValue = new FunctionValue(new[] { (Func<ListValue, ListValue>)TestFunction });

            Assert.Throws<CimbolRuntimeException>(() => functionValue.Invoke(new NumberValue(1)));
        }

        [Test]
        public void Should_ThrowException_When_NumberFunctionInvokedWithBoolean()
        {
            NumberValue TestFunction(NumberValue numberValue)
            {
                return numberValue;
            }

            var functionValue = new FunctionValue(new[] { (Func<NumberValue, NumberValue>)TestFunction });

            Assert.Throws<CimbolRuntimeException>(() => functionValue.Invoke(new BooleanValue(true)));
        }

        [Test]
        public void Should_PassNumbers_When_NumberFunctionInvokedWithNumber()
        {
            NumberValue TestFunction(NumberValue numberValue)
            {
                return numberValue;
            }

            var functionValue = new FunctionValue(new[] { (Func<NumberValue, NumberValue>)TestFunction });

            var result = functionValue.Invoke(new NumberValue(1)) as NumberValue;
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Value);
        }

        [Test]
        public void Should_CastNumbers_When_NumberFunctionInvokedWithString()
        {
            NumberValue TestFunction(NumberValue numberValue)
            {
                return numberValue;
            }

            var functionValue = new FunctionValue(new[] { (Func<NumberValue, NumberValue>)TestFunction });

            var result = functionValue.Invoke(new StringValue("1")) as NumberValue;
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Value);
        }

        [Test]
        public void Should_ThrowException_When_ObjectFunctionInvokedWithNumber()
        {
            ObjectValue TestFunction(ObjectValue objectValue)
            {
                return objectValue;
            }

            var functionValue = new FunctionValue(new[] { (Func<ObjectValue, ObjectValue>)TestFunction });
            Assert.Throws<CimbolRuntimeException>(() => functionValue.Invoke(new NumberValue(1)));
        }

        [Test]
        public void Should_PassObjects_When_ObjectFunctionInvokedWithObject()
        {
            ObjectValue TestFunction(ObjectValue objectValue)
            {
                return objectValue;
            }

            var innerObjectValue = new ObjectValue(new Dictionary<string, ILocalValue>());
            var functionValue = new FunctionValue(new[] { (Func<ObjectValue, ObjectValue>)TestFunction });

            var result = functionValue.Invoke(innerObjectValue) as ObjectValue;
            Assert.IsNotNull(result);
            Assert.AreEqual(innerObjectValue.Value, result.Value);
        }

        [Test]
        public void Should_ThrowException_When_StringFunctionInvokedWithBoolean()
        {
            StringValue TestFunction(StringValue stringValue)
            {
                return stringValue;
            }

            var functionValue = new FunctionValue(new[] { (Func<StringValue, StringValue>)TestFunction });

            Assert.Throws<CimbolRuntimeException>(() => functionValue.Invoke(new BooleanValue(true)));
        }

        [Test]
        public void Should_CastStrings_When_StringFunctionInvokedWithNumber()
        {
            StringValue TestFunction(StringValue stringValue)
            {
                return stringValue;
            }

            var functionValue = new FunctionValue(new[] { (Func<StringValue, StringValue>)TestFunction });

            var result = functionValue.Invoke(new NumberValue(1)) as StringValue;
            Assert.IsNotNull(result);
            Assert.AreEqual("1", result.Value);
        }

        [Test]
        public void Should_PassStrings_When_StringFunctionInvokedWithString()
        {
            StringValue TestFunction(StringValue stringValue)
            {
                return stringValue;
            }

            var functionValue = new FunctionValue(new[] { (Func<StringValue, StringValue>)TestFunction });

            var result = functionValue.Invoke(new StringValue("1")) as StringValue;
            Assert.IsNotNull(result);
            Assert.AreEqual("1", result.Value);
        }

        [Test]
        [TestCase(1, 1)]
        [TestCase(2, 2)]
        [TestCase(3, 3)]
        [TestCase(4, 3)]
        [TestCase(5, 3)]
        public void Should_CallOverloadWithVariadicArguments_When_GivenNArguments(int argumentCount, int expected)
        {
            NumberValue TestFunction1(NumberValue numberValue1)
            {
                return new NumberValue(1);
            }

            NumberValue TestFunction2(NumberValue numberValue1, NumberValue numberValue2)
            {
                return new NumberValue(2);
            }

            NumberValue TestFunctionN(
                NumberValue numberValue1,
                NumberValue numberValue2,
                NumberValue numberValue,
                NumberValue[] numberValues)
            {
                return new NumberValue(3);
            }

            var functionValue = new FunctionValue(new Delegate[]
            {
                (Func<NumberValue, NumberValue>)TestFunction1,
                (Func<NumberValue, NumberValue, NumberValue>)TestFunction2,
                (Func<NumberValue, NumberValue, NumberValue, NumberValue[], NumberValue>)TestFunctionN,
            });

            var arguments = Enumerable.Range(1, argumentCount)
                .Select(index => new NumberValue(index))
                .Cast<ILocalValue>()
                .ToArray();

            var result = functionValue.Invoke(arguments) as NumberValue;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.EqualTo(expected));
        }

        [Test]
        [TestCase(1, 1)]
        [TestCase(2, 2)]
        [TestCase(3, 3)]
        public void Should_CallOverloadWithFixedArguments_When_GivenNArguments(int argumentCount, int expected)
        {
            NumberValue TestFunction1(NumberValue numberValue1)
            {
                return new NumberValue(1);
            }

            NumberValue TestFunction2(NumberValue numberValue1, NumberValue numberValue2)
            {
                return new NumberValue(2);
            }

            NumberValue TestFunction3(NumberValue numberValue1, NumberValue numberValue2, NumberValue numberValue)
            {
                return new NumberValue(3);
            }

            var functionValue = new FunctionValue(new Delegate[]
            {
                (Func<NumberValue, NumberValue>)TestFunction1,
                (Func<NumberValue, NumberValue, NumberValue>)TestFunction2,
                (Func<NumberValue, NumberValue, NumberValue, NumberValue>)TestFunction3,
            });

            var arguments = Enumerable.Range(1, argumentCount)
                .Select(index => new NumberValue(index))
                .Cast<ILocalValue>()
                .ToArray();

            var result = functionValue.Invoke(arguments) as NumberValue;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.EqualTo(expected));
        }

        [Test]
        [TestCase(0, 0)]
        [TestCase(1, 1)]
        [TestCase(2, 2)]
        [TestCase(3, 3)]
        [TestCase(4, 4)]
        [TestCase(5, 5)]
        public void Should_PassAllArgumentsToPureVariadicFunction_When_GivenNArguments(int argumentCount, int expected)
        {
            NumberValue TestFunction(NumberValue[] numberValues)
            {
                return new NumberValue(numberValues.Length);
            }

            var functionValue = new FunctionValue(new[] { (Func<NumberValue[], NumberValue>)TestFunction });

            var arguments = Enumerable.Range(1, argumentCount)
                .Select(index => new NumberValue(index))
                .Cast<ILocalValue>()
                .ToArray();

            var result = functionValue.Invoke(arguments) as NumberValue;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.EqualTo(expected));
        }

        [Test]
        [TestCase(2, 0)]
        [TestCase(3, 1)]
        [TestCase(4, 2)]
        [TestCase(5, 3)]
        [TestCase(6, 4)]
        public void Should_PassSomeArgumentsToMixedVariadicFunction_When_GivenNArguments(int argumentCount, int expected)
        {
            NumberValue TestFunction(NumberValue numberValue1, NumberValue numberValue2, NumberValue[] numberValues)
            {
                return new NumberValue(numberValues.Length);
            }

            var functionValue = new FunctionValue(new[]
            {
                (Func<NumberValue, NumberValue, NumberValue[], NumberValue>)TestFunction,
            });

            var arguments = Enumerable.Range(1, argumentCount)
                .Select(index => new NumberValue(index))
                .Cast<ILocalValue>()
                .ToArray();

            var result = functionValue.Invoke(arguments) as NumberValue;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.EqualTo(expected));
        }

        [Test]
        public void Should_RetrieveValue_When_GivenValue()
        {
            var functionValue = new FunctionValue(new[] { (Func<NumberValue>)MockFunction });
            Assert.That(functionValue.Value, Is.InstanceOf<Func<ILocalValue[], ILocalValue>>());
        }

        [Test]
        public void Should_ThrowException_When_InvokedWithTooFewArguments()
        {
            NumberValue TestFunction(NumberValue numberValue1, NumberValue numberValue2)
            {
                return new NumberValue(numberValue1.Value + numberValue2.Value);
            }

            var functionValue = new FunctionValue(new[] { (Func<NumberValue, NumberValue, NumberValue>)TestFunction });

            Assert.Throws<CimbolRuntimeException>(() => functionValue.Invoke(new NumberValue(1)));
        }

        [Test]
        public void Should_ThrowException_When_VariadicFunctionInvokedWithTooFewArguments()
        {
            NumberValue TestFunction(NumberValue numberValue1, NumberValue[] numberValues)
            {
                return new NumberValue(numberValue1.Value + numberValues.Length);
            }

            var functionValue = new FunctionValue(new[] { (Func<NumberValue, NumberValue[], NumberValue>)TestFunction });

            Assert.Throws<CimbolRuntimeException>(() => functionValue.Invoke());
        }

        [Test]
        public void Should_ThrowException_When_OverloadFunctionInvokedWithTooFewArguments()
        {
            NumberValue TestFunction1(NumberValue numberValue1, NumberValue numberValue2)
            {
                return new NumberValue(numberValue1.Value + numberValue2.Value);
            }

            NumberValue TestFunction2(NumberValue numberValue1, NumberValue numberValue2, NumberValue numberValue3)
            {
                return new NumberValue(numberValue1.Value + numberValue2.Value);
            }

            var functionValue = new FunctionValue(new Delegate[]
            {
                (Func<NumberValue, NumberValue, NumberValue>)TestFunction1,
                (Func<NumberValue, NumberValue, NumberValue, NumberValue>)TestFunction2,
            });

            Assert.Throws<CimbolRuntimeException>(() => functionValue.Invoke(new NumberValue(1)));
        }

        [Test]
        public void Should_ThrowException_When_OverloadVariadicFunctionInvokedWithTooFewArguments()
        {
            NumberValue TestFunction1(NumberValue numberValue1)
            {
                return new NumberValue(numberValue1.Value);
            }

            NumberValue TestFunction2(NumberValue numberValue1, NumberValue numberValue2, NumberValue[] numberValues)
            {
                return new NumberValue(numberValue1.Value + numberValue2.Value + numberValues.Length);
            }

            var functionValue = new FunctionValue(new Delegate[]
            {
                (Func<NumberValue, NumberValue>)TestFunction1,
                (Func<NumberValue, NumberValue, NumberValue[], NumberValue>)TestFunction2,
            });

            Assert.Throws<CimbolRuntimeException>(() => functionValue.Invoke());
        }

        [Test]
        public void Should_ThrowException_When_InvokedWithTooManyArguments()
        {
            NumberValue TestFunction(NumberValue numberValue1, NumberValue numberValue2)
            {
                return new NumberValue(numberValue1.Value + numberValue2.Value);
            }

            var functionValue = new FunctionValue(new[] { (Func<NumberValue, NumberValue, NumberValue>)TestFunction });

            Assert.Throws<CimbolRuntimeException>(() =>
                functionValue.Invoke(new NumberValue(1), new NumberValue(2), new NumberValue(3)));
        }

        [Test]
        public void Should_ThrowArgumentNullException_When_ConstructedWithNullMethodList()
        {
            var result = Assert.Throws<ArgumentNullException>(() =>
            {
                var functionValue = new FunctionValue((Delegate[])null);
            });

            Assert.That(result.ParamName, Is.EqualTo("methods"));
        }

        [Test]
        public void Should_ThrowException_When_ConstructedWithFunctionThatDoesNotReturnILocalValue()
        {
            decimal TestFunction(NumberValue numberValue)
            {
                return numberValue.Value;
            }

            var result = Assert.Throws<ArgumentException>(() =>
            {
                var functionValue = new FunctionValue(new[] { (Func<NumberValue, decimal>)TestFunction });
            });
        }

        [Test]
        public void Should_ThrowException_When_ConstructedWithFunctionThatDoesNotAcceptILocalValue()
        {
            NumberValue TestFunction(decimal numberValue)
            {
                return new NumberValue(numberValue);
            }

            var result = Assert.Throws<ArgumentException>(() =>
            {
                var functionValue = new FunctionValue(new[] { (Func<decimal, NumberValue>)TestFunction });
            });
        }

        [Test]
        public void Should_ThrowException_When_ArrayParameterIsNotLast()
        {
            NumberValue TestFunction(NumberValue[] numberValueArray, NumberValue numberValue)
            {
                return numberValue;
            }

            var result = Assert.Throws<ArgumentException>(() =>
            {
                var functionValue = new FunctionValue(new[] { (Func<NumberValue[], NumberValue, NumberValue>)TestFunction });
            });
        }

        [Test]
        public void Should_ConstructFunction_When_ArrayParameterIsOnlyArgument()
        {
            NumberValue TestFunction(NumberValue[] numberValueArray)
            {
                return new NumberValue(numberValueArray.Length);
            }

            var result = new FunctionValue(new[] { (Func<NumberValue[], NumberValue>)TestFunction });

            Assert.That(result.Value, Is.Not.Null);
        }

        [Test]
        public void Should_ConstructFunction_When_ArrayParameterIsLastArgument()
        {
            NumberValue TestFunction(NumberValue numberValue, NumberValue[] numberValueArray)
            {
                return new NumberValue(numberValueArray.Length);
            }

            var result = new FunctionValue(new[] { (Func<NumberValue, NumberValue[], NumberValue>)TestFunction });

            Assert.That(result.Value, Is.Not.Null);
        }
            
        [Test]
        public void Should_BeEqual_When_ComparedToSelf()
        {
            var value = new FunctionValue(new[] { (Func<NumberValue>)MockFunction });

            var result = value.EqualTo(value);

            Assert.That(result, Is.True);
        }

        [Test]
        public void ShouldNot_BeEqual_When_ComparedToErrorValue()
        {
            var value = new FunctionValue(new[] { (Func<NumberValue>)MockFunction });
            var otherValue = new FunctionValue(new[] { (Func<NumberValue>)MockFunction });

            var result = value.EqualTo(otherValue);

            Assert.That(result, Is.False);
        }

        [Test]
        public void ShouldNot_BeEqual_When_ComparedToNonErrorValue()
        {
            var value = new FunctionValue(new[] { (Func<NumberValue>)MockFunction });
            var otherValue = new NumberValue(1);

            var result = value.EqualTo(otherValue);

            Assert.That(result, Is.False);
        }

        private static NumberValue MockFunction()
        {
            return new NumberValue(1);
        }
    }
}