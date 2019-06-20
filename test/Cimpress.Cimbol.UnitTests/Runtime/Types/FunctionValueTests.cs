using System;
using System.Collections.Generic;
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
            var functionValue = new FunctionValue((Func<NumberValue>)MockFunction);
            Assert.Throws<NotSupportedException>(() => functionValue.Access("test"));
        }

        [Test]
        public void Should_ThrowException_When_ConvertingToBoolean()
        {
            var functionValue = new FunctionValue((Func<NumberValue>)MockFunction);
            Assert.Throws<NotSupportedException>(() => functionValue.CastBoolean());
        }

        [Test]
        public void Should_ThrowException_When_ConvertingToNumber()
        {
            var functionValue = new FunctionValue((Func<NumberValue>)MockFunction);
            Assert.Throws<NotSupportedException>(() => functionValue.CastNumber());
        }

        [Test]
        public void Should_ThrowException_When_ConvertingToString()
        {
            var functionValue = new FunctionValue((Func<NumberValue>)MockFunction);
            Assert.Throws<NotSupportedException>(() => functionValue.CastString());
        }

        [Test]
        public void Should_PassBooleans_When_BooleanFunctionInvokedWithBoolean()
        {
            BooleanValue TestFunction(BooleanValue booleanValue)
            {
                return booleanValue;
            }

            var functionValue = new FunctionValue((Func<BooleanValue, BooleanValue>)TestFunction);

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

            var functionValue = new FunctionValue((Func<BooleanValue, BooleanValue>)TestFunction);

            Assert.Throws<NotSupportedException>(() => functionValue.Invoke(new NumberValue(1)));
        }

        [Test]
        public void Should_ThrowException_When_BooleanFunctionInvokedWithString()
        {
            BooleanValue TestFunction(BooleanValue booleanValue)
            {
                return booleanValue;
            }

            var functionValue = new FunctionValue((Func<BooleanValue, BooleanValue>)TestFunction);

            Assert.Throws<NotSupportedException>(() => functionValue.Invoke(new StringValue("test")));
        }

        [Test]
        public void Should_PassFunctions_When_FunctionFunctionInvokedWithFunction()
        {
            FunctionValue TestFunction(FunctionValue otherFunctionValue)
            {
                return otherFunctionValue;
            }

            var innerFunctionValue = new FunctionValue((Func<NumberValue>)MockFunction);
            var functionValue = new FunctionValue((Func<FunctionValue, FunctionValue>)TestFunction);

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

            var functionValue = new FunctionValue((Func<FunctionValue, FunctionValue>)TestFunction);

            Assert.Throws<NotSupportedException>(() => functionValue.Invoke(new NumberValue(1)));
        }

        [Test]
        public void Should_PassLists_When_ListFunctionInvokedWithList()
        {
            ListValue TestFunction(ListValue listValue)
            {
                return listValue;
            }

            var innerListValue = new ListValue(Array.Empty<ILocalValue>());
            var functionValue = new FunctionValue((Func<ListValue, ListValue>)TestFunction);

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

            var functionValue = new FunctionValue((Func<ListValue, ListValue>)TestFunction);

            Assert.Throws<NotSupportedException>(() => functionValue.Invoke(new NumberValue(1)));
        }

        [Test]
        public void Should_ThrowException_When_NumberFunctionInvokedWithBoolean()
        {
            NumberValue TestFunction(NumberValue numberValue)
            {
                return numberValue;
            }

            var functionValue = new FunctionValue((Func<NumberValue, NumberValue>)TestFunction);

            Assert.Throws<NotSupportedException>(() => functionValue.Invoke(new BooleanValue(true)));
        }

        [Test]
        public void Should_PassNumbers_When_NumberFunctionInvokedWithNumber()
        {
            NumberValue TestFunction(NumberValue numberValue)
            {
                return numberValue;
            }

            var functionValue = new FunctionValue((Func<NumberValue, NumberValue>)TestFunction);

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

            var functionValue = new FunctionValue((Func<NumberValue, NumberValue>)TestFunction);

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

            var functionValue = new FunctionValue((Func<ObjectValue, ObjectValue>)TestFunction);
            Assert.Throws<NotSupportedException>(() => functionValue.Invoke(new NumberValue(1)));
        }

        [Test]
        public void Should_PassObjects_When_ObjectFunctionInvokedWithObject()
        {
            ObjectValue TestFunction(ObjectValue objectValue)
            {
                return objectValue;
            }

            var innerObjectValue = new ObjectValue(new Dictionary<string, ILocalValue>());
            var functionValue = new FunctionValue((Func<ObjectValue, ObjectValue>)TestFunction);

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

            var functionValue = new FunctionValue((Func<StringValue, StringValue>)TestFunction);

            Assert.Throws<NotSupportedException>(() => functionValue.Invoke(new BooleanValue(true)));
        }

        [Test]
        public void Should_CastStrings_When_StringFunctionInvokedWithNumber()
        {
            StringValue TestFunction(StringValue stringValue)
            {
                return stringValue;
            }

            var functionValue = new FunctionValue((Func<StringValue, StringValue>)TestFunction);

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

            var functionValue = new FunctionValue((Func<StringValue, StringValue>)TestFunction);

            var result = functionValue.Invoke(new StringValue("1")) as StringValue;
            Assert.IsNotNull(result);
            Assert.AreEqual("1", result.Value);
        }

        [Test]
        public void Should_RetrieveValue_When_GivenValue()
        {
            var functionValue = new FunctionValue((Func<NumberValue>)MockFunction);
            Assert.AreEqual((Func<NumberValue>)MockFunction, functionValue.Value);
        }

        [Test]
        public void Should_ThrowException_When_InvokedWithTooFewArguments()
        {
            NumberValue TestFunction(NumberValue numberValue1, NumberValue numberValue2)
            {
                return new NumberValue(numberValue1.Value + numberValue2.Value);
            }

            var functionValue = new FunctionValue((Func<NumberValue, NumberValue, NumberValue>)TestFunction);

            Assert.Throws<NotSupportedException>(() => functionValue.Invoke(new NumberValue(1)));
        }

        [Test]
        public void Should_ThrowException_When_InvokedWithTooManyArguments()
        {
            NumberValue TestFunction(NumberValue numberValue1, NumberValue numberValue2)
            {
                return new NumberValue(numberValue1.Value + numberValue2.Value);
            }

            var functionValue = new FunctionValue((Func<NumberValue, NumberValue, NumberValue>)TestFunction);

            Assert.Throws<NotSupportedException>(() =>
                functionValue.Invoke(new NumberValue(1), new NumberValue(2), new NumberValue(3)));
        }

        private static NumberValue MockFunction()
        {
            return new NumberValue(1);
        }
    }
}