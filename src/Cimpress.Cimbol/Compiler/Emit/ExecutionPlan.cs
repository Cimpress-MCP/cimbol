// Copyright 2021 Cimpress plc.
// Licensed under the Apache License, Version 2.0
// http://www.apache.org/licenses/LICENSE-2.0

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Cimpress.Cimbol.Compiler.SyntaxTree;

namespace Cimpress.Cimbol.Compiler.Emit
{
    /// <summary>
    /// Describes the order and grouping of evaluations of formulas and imports.
    /// </summary>
    public class ExecutionPlan
    {
        private readonly Dictionary<IDeclarationNode, ExecutionStep> _declarationMapping =
            new Dictionary<IDeclarationNode, ExecutionStep>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ExecutionPlan"/> from a dependency table.
        /// </summary>
        /// <param name="declarationHierarchy">The declaration hierarchy used by the execution plan.</param>
        /// <param name="dependencyTable">The dependency table used by the execution plan.</param>
        /// <param name="symbolRegistry">The symbol registry used by the execution plan.</param>
        internal ExecutionPlan(
            DeclarationHierarchy declarationHierarchy,
            DependencyTable dependencyTable,
            SymbolRegistry symbolRegistry)
        {
            if (declarationHierarchy == null)
            {
                throw new ArgumentNullException(nameof(declarationHierarchy));
            }

            if (dependencyTable == null)
            {
                throw new ArgumentNullException(nameof(dependencyTable));
            }

            if (symbolRegistry == null)
            {
                throw new ArgumentNullException(nameof(symbolRegistry));
            }

            var executionStepId = 0;

            var partialOrdering = dependencyTable.MinimalPartialOrder();

            var executionGroups = new ExecutionGroup[partialOrdering.Count];

            for (var i = 0; i < partialOrdering.Count; ++i)
            {
                var index = partialOrdering.Count - i - 1;

                // The execution groups are iterated in reverse order because the dependencies need to be processed
                // before the formula that depends on them.
                var declarationGroup = partialOrdering.ElementAt(index);

                var executionEvents = new List<ExecutionStep>(declarationGroup.Count);

                foreach (var declarationNode in declarationGroup)
                {
                    var currentId = executionStepId;

                    var dependents = dependencyTable.GetDependents(declarationNode);

                    var dependentExecutionSteps = dependents.Select(dependency => _declarationMapping[dependency]);

                    executionStepId += 1;

                    var isAsynchronous = (declarationNode as FormulaNode)?.IsAsynchronous ?? false;

                    var moduleNode = declarationHierarchy.GetParentModule(declarationNode);

                    var executionStep = new ExecutionStep(
                        currentId,
                        moduleNode,
                        declarationNode,
                        isAsynchronous ? ExecutionStepType.Asynchronous : ExecutionStepType.Synchronous,
                        dependentExecutionSteps);

                    executionEvents.Add(executionStep);

                    _declarationMapping.Add(declarationNode, executionStep);
                }

                var executionGroup = new ExecutionGroup(executionEvents);

                executionGroups[index] = executionGroup;
            }

            ExecutionGroups = ExecutionGroup.Merge(executionGroups).ToImmutableArray();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExecutionPlan"/> from a list of execution groups.
        /// This will optimize them by merging the synchronous groups together.
        /// </summary>
        /// <param name="executionGroups">The list of execution groups.</param>
        internal ExecutionPlan(IEnumerable<ExecutionGroup> executionGroups)
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