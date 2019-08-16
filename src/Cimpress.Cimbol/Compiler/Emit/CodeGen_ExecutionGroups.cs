using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
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
        /// Generate the expression tree for evaluating and persisting the result of an asynchronous executions step.
        /// </summary>
        /// <param name="step">The execution step's expression.</param>
        /// <param name="stepVariable">The variable to assign the execution step to.</param>
        /// <returns>An expression that evaluates and persists an execution step.</returns>
        internal static Expression ExecutionStepAsync(Expression step, ParameterExpression stepVariable)
        {
            var assignmentParameter = Expression.Parameter(typeof(ILocalValue));

            var assignmentLambda = Expression.Lambda(
                Expression.Assign(stepVariable, assignmentParameter),
                assignmentParameter);

            return Expression.Call(EvaluationFunctions.AsyncEvalInfo, step, assignmentLambda);
        }

        /// <summary>
        /// Generate the expression tree for evaluating, persisting, and exporting an asynchronous execution step.
        /// </summary>
        /// <param name="step">The execution step's expression.</param>
        /// <param name="stepVariable">The variable to assign the execution step to.</param>
        /// <param name="stepName">The name of the execution step.</param>
        /// <param name="moduleVariable">The variable for the module to export the result to.</param>
        /// <returns>An expression that evaluates, persists, and exports an asynchronous execution step.</returns>
        internal static Expression ExecutionStepAsyncExported(
            Expression step,
            ParameterExpression stepVariable,
            string stepName,
            ParameterExpression moduleVariable)
        {
            var assignmentParameter = Expression.Parameter(typeof(ILocalValue));

            var assignmentLambda = Expression.Lambda(
                Expression.Call(
                    EvaluationFunctions.ExportValueInfo,
                    Expression.Assign(stepVariable, assignmentParameter),
                    Expression.Constant(stepName),
                    moduleVariable),
                assignmentParameter);

            return Expression.Call(EvaluationFunctions.AsyncEvalInfo, step, assignmentLambda);
        }

        /// <summary>
        /// Generate the expression tree for evaluating and persisting the result of a synchronous execution step.
        /// </summary>
        /// <param name="step">The execution step's expression.</param>
        /// <param name="stepVariable">The variable to assign the execution step to.</param>
        /// <returns>An expression that evaluates and persists an execution step.</returns>
        internal static Expression ExecutionStepSync(Expression step, ParameterExpression stepVariable)
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
    }
}