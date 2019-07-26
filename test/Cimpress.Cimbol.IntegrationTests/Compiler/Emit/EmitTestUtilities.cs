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