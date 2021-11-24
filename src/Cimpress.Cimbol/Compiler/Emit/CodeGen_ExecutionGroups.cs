// Copyright 2021 Cimpress plc.
// Licensed under the Apache License, Version 2.0
// http://www.apache.org/licenses/LICENSE-2.0

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Cimpress.Cimbol.Exceptions;
using Cimpress.Cimbol.Runtime;
using Cimpress.Cimbol.Runtime.Functions;
using Cimpress.Cimbol.Runtime.Types;

namespace Cimpress.Cimbol.Compiler.Emit
{
    /// <summary>
    /// A collection of methods for generating expression trees related to execution groups.
    /// </summary>
    internal static partial class CodeGen
    {
        /// <summary>
        /// Generates the expression tree for running an execution group.
        /// </summary>
        /// <param name="asynchronousSteps">The list of asynchronous steps to run.</param>
        /// <param name="synchronousSteps">The list of synchronous steps to run.</param>
        /// <returns>An expression that runs an execution group.</returns>
        internal static LambdaExpression ExecutionGroup(
            ICollection<Expression> asynchronousSteps,
            ICollection<Expression> synchronousSteps)
        {
            // Create the temporary variables for the asynchronous steps in this group

            var temporaryVariables = new List<ParameterExpression>(asynchronousSteps.Count);

            // Execute and assign all of the asynchronous steps to temporary variables

            var asynchronousExpressions = asynchronousSteps.Select(asynchronousStep =>
            {
                var temporaryVariable = Expression.Parameter(typeof(Task));

                temporaryVariables.Add(temporaryVariable);

                return Expression.Assign(temporaryVariable, asynchronousStep);
            }).ToArray();

            // Execute, assign, and optionally export all of the synchronous steps to variables

            var synchronousExpressions = synchronousSteps;

            // Put all of the temporary variables containing tasks in an array and return it

            var bodyReturnValues = temporaryVariables;

            var bodyReturnArray = Expression.NewArrayInit(typeof(Task), bodyReturnValues);

            var bodyReturn = Expression.Call(null, StandardFunctions.TaskWhenAllInfo, bodyReturnArray);

            // Concatenate the last four steps and add them to a block that defines the temporary variables

            // TODO: Switch these around later
            var executionGroupExpressions = synchronousExpressions
                .Concat(asynchronousExpressions)
                .Append(bodyReturn);

            var executionGroupBody = Expression.Block(temporaryVariables, executionGroupExpressions);

            // Wrap that in a lambda and return it.

            return Expression.Lambda(executionGroupBody);
        }

        /// <summary>
        /// Generates an expression tree that runs a series of execution groups in order.
        /// </summary>
        /// <param name="executionGroups">The list of execution groups to chain together.</param>
        /// <param name="programOutputBuilder">An expression that outputs the final results of the program.</param>
        /// <returns>An expression that builds the chain of execution groups.</returns>
        internal static Expression ExecutionGroupChain(
            IEnumerable<LambdaExpression> executionGroups,
            Expression programOutputBuilder)
        {
            var closer = Expression.Lambda(programOutputBuilder);

            return Expression.Call(
                EvaluationFunctions.RunExecutionGroupsInfo,
                closer,
                Expression.NewArrayInit(typeof(Func<Task>), executionGroups));
        }

        /// <summary>
        /// Generates the expression that evaluates an asynchronous execution step with partial evaluation.
        /// </summary>
        /// <param name="step">The expression tree for the execution step.</param>
        /// <param name="resultCallback">The lambda expression to run when the result is computed.</param>
        /// <param name="executionStepContext">The context of the execution step.</param>
        /// <param name="errorList">The list of errors encountered while running the program.</param>
        /// <param name="skipList">The list of execution steps to skip the evaluation of.</param>
        /// <param name="compilationProfile">The level of error reporting to use.</param>
        /// <returns>An expression that evaluates an asynchronous execution step.</returns>
        internal static Expression ExecutionStepAsync(
            Expression step,
            LambdaExpression resultCallback,
            ExecutionStepContext executionStepContext,
            ParameterExpression errorList,
            ParameterExpression skipList,
            CompilationProfile compilationProfile)
        {
            switch (compilationProfile)
            {
                case CompilationProfile.Minimal:
                    return Expression.Call(EvaluationFunctions.EvaluateAsynchronousInfo, step, resultCallback);

                case CompilationProfile.Trace:
                    return Expression.Call(
                        EvaluationFunctions.EvaluateAsynchronousTraceInfo,
                        Expression.Constant(executionStepContext, typeof(ExecutionStepContext)),
                        Expression.Lambda(step),
                        resultCallback);

                case CompilationProfile.Verbose:
                    return Expression.Call(
                        EvaluationFunctions.EvaluateAsynchronousVerboseInfo,
                        Expression.Constant(executionStepContext),
                        errorList,
                        skipList,
                        Expression.Lambda(step),
                        resultCallback);

                default:
                    throw new CimbolInternalException("Unrecognized compilation profile.");
            }
        }

        /// <summary>
        /// Generates the expression that handles the result of an asynchronous execution step evaluation.
        /// </summary>
        /// <param name="stepVariable">The variable to assign the result to.</param>
        /// <returns>An expression that handles the result of an asynchronous evaluation.</returns>
        internal static LambdaExpression ExecutionStepAsyncHandler(ParameterExpression stepVariable)
        {
            var assignmentParameter = Expression.Parameter(typeof(ILocalValue));

            return Expression.Lambda(
                Expression.Assign(stepVariable, assignmentParameter),
                assignmentParameter);
        }

        /// <summary>
        /// Generates the expression that handles the result of an asynchronous execution step evaluation with exporting.
        /// </summary>
        /// <param name="stepVariable">The variable to assign the result to.</param>
        /// <param name="stepName">The name of the execution step.</param>
        /// <param name="moduleVariable">The variable for the module to export the result to.</param>
        /// <returns>An expression that handles the result of an asynchronous evaluation.</returns>
        internal static LambdaExpression ExecutionStepAsyncHandlerExported(
            ParameterExpression stepVariable,
            string stepName,
            ParameterExpression moduleVariable)
        {
            var assignmentParameter = Expression.Parameter(typeof(ILocalValue));

            return Expression.Lambda(
                Expression.Call(
                    EvaluationFunctions.ExportValueInfo,
                    Expression.Assign(stepVariable, assignmentParameter),
                    Expression.Constant(stepName),
                    moduleVariable),
                assignmentParameter);
        }

        /// <summary>
        /// Generate the expression tree for re-exporting a symbol.
        /// </summary>
        /// <param name="stepVariable">The variable containing the value to re-export.</param>
        /// <param name="stepName">The name of the execution step.</param>
        /// <param name="moduleVariable">The variable for the module to export the result to.</param>
        /// <returns>An expression that evaluates, persists, and exports a synchronous execution step.</returns>
        internal static Expression ExecutionStepExport(
            ParameterExpression stepVariable,
            string stepName,
            ParameterExpression moduleVariable)
        {
            return Expression.Call(
                EvaluationFunctions.ExportValueInfo,
                stepVariable,
                Expression.Constant(stepName),
                moduleVariable);
        }

        /// <summary>
        /// Generate the expression tree for evaluating and persisting the result of a synchronous execution step.
        /// </summary>
        /// <param name="step">The execution step's expression.</param>
        /// <param name="stepVariable">The variable to assign the execution step to.</param>
        /// <returns>An expression that evaluates and persists an execution step.</returns>
        internal static Expression ExecutionStepSync(
            Expression step,
            ParameterExpression stepVariable)
        {
            return Expression.Assign(stepVariable, step);
        }

        /// <summary>
        /// Generate the expression tree for evaluating, persisting, and exporting a synchronous execution step.
        /// </summary>
        /// <param name="step">The execution step's expression.</param>
        /// <param name="stepVariable">The variable to assign the execution step to.</param>
        /// <param name="stepName">The name of the execution step.</param>
        /// <param name="moduleVariable">The variable for the module to export the result to.</param>
        /// <returns>An expression that evaluates, persists, and exports a synchronous execution step.</returns>
        internal static Expression ExecutionStepSyncExported(
            Expression step,
            ParameterExpression stepVariable,
            string stepName,
            ParameterExpression moduleVariable)
        {
            return Expression.Call(
                EvaluationFunctions.ExportValueInfo,
                Expression.Assign(stepVariable, step),
                Expression.Constant(stepName),
                moduleVariable);
        }

        /// <summary>
        /// Generates the expression tree for evaluating a synchronous execution step with partial evaluation.
        /// </summary>
        /// <param name="step">The execution step's expression.</param>
        /// <param name="executionStepContext">The context of the execution step.</param>
        /// <param name="errorList">The list of errors encountered while running the program.</param>
        /// <param name="skipList">The list of execution steps to skip the evaluation of.</param>
        /// <param name="compilationProfile">The level of error reporting to use.</param>
        /// <returns>An expression that evaluates a synchronous execution step.</returns>
        internal static Expression ExecutionStepSyncEvaluation(
            Expression step,
            ExecutionStepContext executionStepContext,
            ParameterExpression errorList,
            ParameterExpression skipList,
            CompilationProfile compilationProfile)
        {
            switch (compilationProfile)
            {
                case CompilationProfile.Minimal:
                    return step;

                case CompilationProfile.Trace:
                    return Expression.Call(
                        EvaluationFunctions.EvaluateSynchronousTraceInfo,
                        Expression.Constant(executionStepContext),
                        Expression.Lambda(step));

                case CompilationProfile.Verbose:
                    return Expression.Call(
                        EvaluationFunctions.EvaluateSynchronousVerboseInfo,
                        Expression.Constant(executionStepContext),
                        errorList,
                        skipList,
                        Expression.Lambda(step));

                default:
                    throw new CimbolInternalException("Unrecognized compilation profile.");
            }
        }
    }
}