using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Cimpress.Cimbol.Compiler.SyntaxTree;
using Cimpress.Cimbol.Runtime;

namespace Cimpress.Cimbol.Compiler.Emit
{
    /// <summary>
    /// A single expression evaluation step inside of an execution group.
    /// </summary>
    public class ExecutionStep
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExecutionStep"/> class.
        /// </summary>
        /// <param name="id">The ID of the execution step.</param>
        /// <param name="moduleNode">The module that the declaration node belongs to.</param>
        /// <param name="declarationNode">The declaration node to evaluate when this step is executed.</param>
        /// <param name="type">The type of the execution step.</param>
        /// <param name="dependents">The list of dependents for the execution step.</param>
        public ExecutionStep(
            int id,
            ModuleNode moduleNode,
            IDeclarationNode declarationNode,
            ExecutionStepType type,
            IEnumerable<ExecutionStep> dependents)
        {
            DeclarationNode = declarationNode ?? throw new ArgumentNullException(nameof(declarationNode));

            Dependents = dependents.ToImmutableArray();

            Id = id;

            ModuleNode = moduleNode ?? throw new ArgumentNullException(nameof(moduleNode));

            Type = type;

            ExecutionStepContext = new ExecutionStepContext(
                Id,
                Dependents.Select(dependent => dependent.Id).ToArray(),
                DeclarationNode.Name,
                ModuleNode.Name);
        }

        /// <summary>
        /// The declaration node to evaluate when this step is executed.
        /// </summary>
        public IDeclarationNode DeclarationNode { get; }

        /// <summary>
        /// The list of dependents for the execution step.
        /// </summary>
        public IReadOnlyCollection<ExecutionStep> Dependents { get; }

        /// <summary>
        /// The context of the execution step. Used when handling errors.
        /// </summary>
        public ExecutionStepContext ExecutionStepContext { get; }

        /// <summary>
        /// The ID of the execution step.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Whether or not the execution step is asynchronous.
        /// </summary>
        public bool IsAsynchronous => Type == ExecutionStepType.Asynchronous;

        /// <summary>
        /// The name of the module containing the execution step.
        /// </summary>
        public ModuleNode ModuleNode { get; }

        /// <summary>
        /// The type of the execution step.
        /// </summary>
        public ExecutionStepType Type { get; }
    }
}