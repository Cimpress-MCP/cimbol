using System;
using System.Reflection;
using System.Threading.Tasks;
using Cimpress.Cimbol.Exceptions;
using Cimpress.Cimbol.Runtime.Types;

namespace Cimpress.Cimbol.Runtime.Functions
{
    /// <summary>
    /// A collection of functions that drive evaluation.
    /// </summary>
    internal static class EvaluationFunctions
    {
        /// <summary>
        /// The method info for the <see cref="EvaluateAsynchronous"/> method.
        /// </summary>
        internal static MethodInfo EvaluateAsynchronousInfo { get; } = typeof(EvaluationFunctions).GetMethod(
            nameof(EvaluateAsynchronous),
            BindingFlags.NonPublic | BindingFlags.Static);

        /// <summary>
        /// The method info for the <see cref="EvaluateSynchronous"/> method.
        /// </summary>
        internal static MethodInfo EvaluateSynchronousInfo { get; } = typeof(EvaluationFunctions).GetMethod(
            nameof(EvaluateSynchronous),
            BindingFlags.NonPublic | BindingFlags.Static);

        /// <summary>
        /// The method info for the <see cref="ExportValue"/> method.
        /// </summary>
        internal static MethodInfo ExportValueInfo { get; } = typeof(EvaluationFunctions).GetMethod(
            nameof(ExportValue),
            BindingFlags.NonPublic | BindingFlags.Static);

        /// <summary>
        /// The method info for the <see cref="RunExecutionGroups"/> methods.
        /// </summary>
        internal static MethodInfo RunExecutionGroupsInfo { get; } = typeof(EvaluationFunctions).GetMethod(
            nameof(RunExecutionGroups),
            BindingFlags.NonPublic | BindingFlags.Static);

        /// <summary>
        /// Execute an asynchronous formula.
        /// </summary>
        /// <param name="id">The ID of the formula.</param>
        /// <param name="dependents">The collection of IDs of formulas dependent on this execution.</param>
        /// <param name="skips">The collection of formulas to not execute. </param>
        /// <param name="executeCallback">The callback to run to execute the formula.</param>
        /// <param name="resultCallback">The callback to run to save the result of the formula's execution.</param>
        /// <returns>A task the completes the formula is evaluated and saved.</returns>
        internal static async Task EvaluateAsynchronous(
            int id,
            int[] dependents,
            bool[] skips,
            Func<ILocalValue> executeCallback,
            Action<ILocalValue> resultCallback)
        {
            ILocalValue result;

            CimbolRuntimeException error;

            if (!skips[id])
            {
                error = null;
                goto evaluationFailure;
            }

            try
            {
                result = executeCallback();
            }
            catch (CimbolRuntimeException runtimeException)
            {
                error = runtimeException;
                goto evaluationFailure;
            }

            if (!(result is PromiseValue promiseValue))
            {
                error = CimbolRuntimeException.AwaitError(null);
                goto evaluationFailure;
            }

            try
            {
                var awaitedResult = await promiseValue.Value;
                resultCallback(awaitedResult);
            }
            catch (CimbolRuntimeException runtimeException)
            {
                error = runtimeException;
                goto evaluationFailure;
            }

            return;

            evaluationFailure:
            foreach (var dependent in dependents)
            {
                skips[dependent] = false;
            }

            resultCallback(new ErrorValue(error));
        }

        /// <summary>
        /// Execute a synchronous formula.
        /// </summary>
        /// <param name="id">The ID of the formula.</param>
        /// <param name="dependents">The collection of IDs of formulas dependent on this execution.</param>
        /// <param name="skips">The collection of formulas to not execute. </param>
        /// <param name="executeCallback">The callback to run to execute the formula.</param>
        /// <returns>The result of evaluating the formula.</returns>
        internal static ILocalValue EvaluateSynchronous(
            int id,
            int[] dependents,
            bool[] skips,
            Func<ILocalValue> executeCallback)
        {
            CimbolRuntimeException error;

            if (!skips[id])
            {
                error = null;
                goto evaluationFailure;
            }

            try
            {
                return executeCallback();
            }
            catch (CimbolRuntimeException runtimeException)
            {
                error = runtimeException;
                goto evaluationFailure;
            }

            evaluationFailure:
            foreach (var dependent in dependents)
            {
                skips[dependent] = false;
            }

            return new ErrorValue(error);
        }

        /// <summary>
        /// Export a computed value to a module.
        /// </summary>
        /// <param name="exportValue">The value to export.</param>
        /// <param name="exportName">The name of the value to export.</param>
        /// <param name="moduleValue">The module to export the value to.</param>
        internal static void ExportValue(ILocalValue exportValue, string exportName, ObjectValue moduleValue)
        {
            moduleValue.Assign(exportName, exportValue);
        }

        /// <summary>
        /// Run a series of execution groups.
        /// </summary>
        /// <param name="closer">The function to combine all of the outputs together.</param>
        /// <param name="executionGroups">The list of execution groups.</param>
        /// <returns>A task that resolves to the final program output.</returns>
        internal static async Task<ObjectValue> RunExecutionGroups(Func<ObjectValue> closer, Func<Task>[] executionGroups)
        {
            foreach (var executionGroup in executionGroups)
            {
                await executionGroup();
            }

            return closer();
        }
    }
}