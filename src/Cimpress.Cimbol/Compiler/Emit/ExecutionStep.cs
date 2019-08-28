using System.Collections.Generic;
using System.Collections.Immutable;
using Cimpress.Cimbol.Compiler.SyntaxTree;

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
        /// <param name="node">The declaration node to evaluate when this step is executed.</param>
        /// <param name="type">The type of the execution step.</param>
        /// <param name="dependencies">The list of dependencies for the execution step.</param>
        /// <param name="symbolTable">The symbol table used by the execution step.</param>
        public ExecutionStep(
            int id,
            IDeclarationNode node,
            ExecutionStepType type,
            IEnumerable<ExecutionStep> dependencies,
            SymbolTable symbolTable)
        {
            Dependencies = dependencies.ToImmutableArray();

            Id = id;

            Node = node;

            SymbolTable = symbolTable;

            Type = type;
        }

        /// <summary>
        /// The list of dependencies for the execution step.
        /// </summary>
        public IReadOnlyCollection<ExecutionStep> Dependencies { get; }

        /// <summary>
        /// The ID of the execution step.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Whether or not the execution step is asynchronous.
        /// </summary>
        public bool IsAsynchronous => Type == ExecutionStepType.Asynchronous;

        /// <summary>
        /// The declaration node to evaluate when this step is executed.
        /// </summary>
        public IDeclarationNode Node { get; }

        /// <summary>
        /// The symbol table that the execution step should use.
        /// </summary>
        public SymbolTable SymbolTable { get; }

        /// <summary>
        /// The type of the execution step.
        /// </summary>
        public ExecutionStepType Type { get; }
    }
}