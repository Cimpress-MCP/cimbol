// Copyright 2021 Cimpress plc.
// Licensed under the Apache License, Version 2.0
// http://www.apache.org/licenses/LICENSE-2.0

using System;
using System.Linq.Expressions;
using Cimpress.Cimbol.Runtime.Types;

namespace Cimpress.Cimbol.IntegrationTests.Compiler.Emit
{
    public class EmitTestUtilities
    {
        public static Func<ILocalValue> WrapAndCompile(Expression expression)
        {
            var lambda = Expression.Lambda<Func<ILocalValue>>(expression);

            return lambda.Compile();
        }
    }
}