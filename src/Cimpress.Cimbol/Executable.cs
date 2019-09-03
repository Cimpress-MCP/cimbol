using System;
using System.Linq;
using System.Threading.Tasks;
using Cimpress.Cimbol.Exceptions;
using Cimpress.Cimbol.Runtime.Types;

namespace Cimpress.Cimbol
{
    /// <summary>
    /// A wrapped around the compiled function.
    /// </summary>
    public class Executable
    {
        private readonly int _argumentCount;

        private readonly Delegate _function;

        /// <summary>
        /// Initializes a new instance of the <see cref="Executable"/> class.
        /// </summary>
        /// <param name="function">The compiled function to initialize the executable with.</param>
        internal Executable(Delegate function)
        {
            // The first argument of any dynamically compiled program is always a Closure, so we ignore that.
            _argumentCount = function.Method.GetParameters().Length - 1;

            _function = function;
        }

        /// <summary>
        /// Calls the executable with the given arguments.
        /// </summary>
        /// <param name="arguments">The list of arguments to call the executable with.</param>
        /// <returns>The result of evaluating the executable.</returns>
        public Task<EvaluationResult> Call(params ILocalValue[] arguments)
        {
            if (arguments == null)
            {
                throw new ArgumentNullException(nameof(arguments));
            }

            if (arguments.Length != _argumentCount)
            {
                throw new ArgumentCountException(_argumentCount, arguments.Length);
            }

            var castArguments = arguments.Cast<object>().ToArray();

            var returnValue = _function.DynamicInvoke(castArguments);

            return returnValue as Task<EvaluationResult>;
        }
    }
}