using System;
using System.Collections.Generic;
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
        /// The method info for the <see cref="EvaluateAsynchronous_Trace"/> method.
        /// </summary>
        internal static MethodInfo EvaluateAsynchronousTraceInfo { get; } = typeof(EvaluationFunctions).GetMethod(
            nameof(EvaluateAsynchronous_Trace),
            BindingFlags.NonPublic | BindingFlags.Static);

        /// <summary>
        /// The method info for the <see cref="EvaluateAsynchronous_Verbose"/> method.
        /// </summary>
        internal static MethodInfo EvaluateAsynchronousVerboseInfo { get; } = typeof(EvaluationFunctions).GetMethod(
            nameof(EvaluateAsynchronous_Verbose),
            BindingFlags.NonPublic | BindingFlags.Static);

        /// <summary>
        /// The method info for the <see cref="EvaluateSynchronous_Trace"/> method.
        /// </summary>
        internal static MethodInfo EvaluateSynchronousTraceInfo { get; } = typeof(EvaluationFunctions).GetMethod(
            nameof(EvaluateSynchronous_Trace),
            BindingFlags.NonPublic | BindingFlags.Static);

        /// <summary>
        /// The method info for the <see cref="EvaluateSynchronous_Verbose"/> method.
        /// </summary>
        internal static MethodInfo EvaluateSynchronousVerboseInfo { get; } = typeof(EvaluationFunctions).GetMethod(
            nameof(EvaluateSynchronous_Verbose),
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
        /// Execute an asynchronous formula with no enhanced error reporting.
        /// </summary>
        /// <param name="executeValue">The result of executing the asynchronous execution step.</param>
        /// <param name="resultCallback">The callback to run to save the result of the formula's execution.</param>
        /// <returns>A task that completes when the formula is evaluated and saved.</returns>
        internal static async Task EvaluateAsynchronous(
            ILocalValue executeValue,
            Action<ILocalValue> resultCallback)
        {
            if (!(executeValue is PromiseValue promiseValue))
            {
                throw CimbolRuntimeException.AwaitError();
            }

            var awaitedResult = await promiseValue.Value;

            resultCallback(awaitedResult);
        }

        /// <summary>
        /// Execute an asynchronous formula with trace error reporting.
        /// </summary>
        /// <param name="executionStepContext">Metadata about the execution step.</param>
        /// <param name="executeCallback">The callback to run to execute the formula.</param>
        /// <param name="resultCallback">The callback to run to save the result of the formula's execution.</param>
        /// <returns>A task that completes when the formula is evaluated and saved.</returns>
        internal static async Task EvaluateAsynchronous_Trace(
            ExecutionStepContext executionStepContext,
            Func<ILocalValue> executeCallback,
            Action<ILocalValue> resultCallback)
        {
            ILocalValue result;

            CimbolRuntimeException error;

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
                error = CimbolRuntimeException.AwaitError();
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

            if (executionStepContext != null)
            {
                error.Formula = executionStepContext.FormulaName;
                error.Module = executionStepContext.ModuleName;
            }

            throw error;
        }

        /// <summary>
        /// Execute an asynchronous formula with verbose error reporting.
        /// </summary>
        /// <param name="executionStepContext">Metadata about the execution step.</param>
        /// <param name="errorList">The list of errors encountered in the program.</param>
        /// <param name="skipList">The collection of formulas to not execute. </param>
        /// <param name="executeCallback">The callback to run to execute the formula.</param>
        /// <param name="resultCallback">The callback to run to save the result of the formula's execution.</param>
        /// <returns>A task that completes when the formula is evaluated and saved.</returns>
        internal static async Task EvaluateAsynchronous_Verbose(
            ExecutionStepContext executionStepContext,
            List<CimbolRuntimeException> errorList,
            bool[] skipList,
            Func<ILocalValue> executeCallback,
            Action<ILocalValue> resultCallback)
        {
            ILocalValue result;

            CimbolRuntimeException error;

            if (!skipList[executionStepContext.Id])
            {
                error = new CimbolRuntimeSkipException();
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
                error = CimbolRuntimeException.AwaitError();
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

            foreach (var dependent in executionStepContext.Dependents)
            {
                skipList[dependent] = false;
            }

            error.Formula = executionStepContext.FormulaName;
            error.Module = executionStepContext.ModuleName;
            errorList.Add(error);

            resultCallback(null);
        }

        /// <summary>
        /// Execute a synchronous formula with trace error reporting.
        /// </summary>
        /// <param name="executionStepContext">Metadata about the execution step.</param>
        /// <param name="executeCallback">The callback to run to execute the formula.</param>
        /// <returns>The result of evaluating the formula.</returns>
        internal static ILocalValue EvaluateSynchronous_Trace(
            ExecutionStepContext executionStepContext,
            Func<ILocalValue> executeCallback)
        {
            try
            {
                return executeCallback();
            }
            catch (CimbolRuntimeException runtimeException)
            {
                if (executionStepContext != null)
                {
                    runtimeException.Formula = executionStepContext.FormulaName;
                    runtimeException.Module = executionStepContext.ModuleName;
                }

                throw;
            }
        }

        /// <summary>
        /// Execute a synchronous formula with verbose error reporting.
        /// </summary>
        /// <param name="executionStepContext">Metadata about the execution step.</param>
        /// <param name="errorList">The list of errors encountered in the program.</param>
        /// <param name="skipList">The collection of formulas to not execute. </param>
        /// <param name="executeCallback">The callback to run to execute the formula.</param>
        /// <returns>The result of evaluating the formula.</returns>
        internal static ILocalValue EvaluateSynchronous_Verbose(
            ExecutionStepContext executionStepContext,
            List<CimbolRuntimeException> errorList,
            bool[] skipList,
            Func<ILocalValue> executeCallback)
        {
            CimbolRuntimeException error;

            if (!skipList[executionStepContext.Id])
            {
                error = new CimbolRuntimeSkipException();
                goto evaluationFailure;
            }

            try
            {
                return executeCallback();
            }
            catch (CimbolRuntimeException runtimeException)
            {
                error = runtimeException;
            }

            evaluationFailure:

            foreach (var dependent in executionStepContext.Dependents)
            {
                skipList[dependent] = false;
            }

            error.Formula = executionStepContext.FormulaName;
            error.Module = executionStepContext.ModuleName;
            errorList.Add(error);

            return null;
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
        internal static async Task<EvaluationResult> RunExecutionGroups(
            Func<EvaluationResult> closer,
            Func<Task>[] executionGroups)
        {
            foreach (var executionGroup in executionGroups)
            {
                await executionGroup();
            }

            return closer();
        }
    }
}