using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Cimpress.Cimbol.Compiler.Emit
{
    /// <summary>
    /// Describes the order and grouping of evaluations of formulas and imports.
    /// </summary>
    public class ExecutionPlan
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExecutionPlan"/> from a dependency table.
        /// </summary>
        /// <param name="dependencyTable">The dependency table.</param>
        public ExecutionPlan(DependencyTable dependencyTable)
        {
            if (dependencyTable == null)
            {
                throw new ArgumentNullException(nameof(dependencyTable));
            }

            var partialOrdering = dependencyTable.MinimalPartialOrder();

            var executionGroups = partialOrdering.Select(declarationGroup =>
            {
                var executionEvents = declarationGroup.Select(declaration =>
                    new ExecutionStep(declaration, ExecutionStepType.Synchronous));

                return new ExecutionGroup(executionEvents);
            });

            ExecutionGroups = ExecutionGroup.Merge(executionGroups).ToImmutableArray();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExecutionPlan"/> from a list of execution groups.
        /// This will optimize them by merging the synchronous groups together.
        /// </summary>
        /// <param name="executionGroups">The list of execution groups.</param>
        public ExecutionPlan(IEnumerable<ExecutionGroup> executionGroups)
        {
            if (executionGroups == null)
            {
                throw new ArgumentNullException(nameof(executionGroups));
            }

            ExecutionGroups = ExecutionGroup.Merge(executionGroups).ToImmutableArray();
        }

        /// <summary>
        /// The execution groups.
        /// </summary>
        public IReadOnlyCollection<ExecutionGroup> ExecutionGroups { get; }
    }
}