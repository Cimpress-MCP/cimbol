// Copyright 2021 Cimpress plc.
// Licensed under the Apache License, Version 2.0
// http://www.apache.org/licenses/LICENSE-2.0

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Cimpress.Cimbol.Compiler.Emit
{
    /// <summary>
    /// A collection of execution steps in order of execution.
    /// </summary>
    public class ExecutionGroup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExecutionGroup"/> class.
        /// </summary>
        /// <param name="executionSteps">The list of execution steps in the execution group.</param>
        public ExecutionGroup(IEnumerable<ExecutionStep> executionSteps)
        {
            ExecutionSteps = executionSteps
                .OrderBy(executionStep => executionStep.IsAsynchronous ? 1 : 0)
                .ToImmutableArray();

            IsAsynchronous = ExecutionSteps.Any(executionStep => executionStep.IsAsynchronous);
        }

        /// <summary>
        /// The execution steps in the execution group.
        /// </summary>
        public IReadOnlyCollection<ExecutionStep> ExecutionSteps { get; }

        /// <summary>
        /// Whether or not the execution group has asynchronous execution steps.
        /// </summary>
        public bool IsAsynchronous { get; }

        /// <summary>
        /// Merge a series of execution groups into possibly fewer groups.
        /// In a chain of execution groups, if the previous execution group does not have any asynchronous steps, it
        /// can be merged into the current execution group successfully.
        /// </summary>
        /// <param name="executionGroups">The list of execution groups to merge.</param>
        /// <returns>A merged list of execution groups.</returns>
        public static IEnumerable<ExecutionGroup> Merge(IEnumerable<ExecutionGroup> executionGroups)
        {
            if (executionGroups == null)
            {
                yield break;
            }

            ExecutionGroup accumulator = null;

            foreach (var executionGroup in executionGroups)
            {
                if (accumulator == null)
                {
                    accumulator = executionGroup;
                }
                else if (accumulator.IsAsynchronous)
                {
                    yield return accumulator;
                    accumulator = executionGroup;
                }
                else
                {
                    var executionSteps = accumulator.ExecutionSteps.Concat(executionGroup.ExecutionSteps);
                    accumulator = new ExecutionGroup(executionSteps);
                }
            }

            if (accumulator != null)
            {
                yield return accumulator;
            }
        }
    }
}