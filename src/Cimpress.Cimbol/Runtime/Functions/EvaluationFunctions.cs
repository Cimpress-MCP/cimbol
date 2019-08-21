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
        /// The method info for the <see cref="AsyncEval"/> method.
        /// </summary>
        internal static MethodInfo AsyncEvalInfo { get; } = typeof(EvaluationFunctions).GetMethod(
            nameof(AsyncEval),
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
        /// Take a promise value, await it, and then execute the associated callback.
        /// </summary>
        /// <param name="awaitedValue">The promise value to await.</param>
        /// <param name="callback">The callback to call on success.</param>
        /// <returns>A task that resolves when the callback has been handled.</returns>
        internal static async Task AsyncEval(ILocalValue awaitedValue, Action<ILocalValue> callback)
        {
            if (!(awaitedValue is PromiseValue promiseValue))
            {
                throw CimbolRuntimeException.AwaitError(null);
            }

            var taskResult = await promiseValue.Value;

            callback(taskResult);
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