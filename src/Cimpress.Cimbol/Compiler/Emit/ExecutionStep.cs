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
        /// <param name="node">The declaration node to evaluate when this step is executed.</param>
        /// <param name="type">The type of the execution step.</param>
        public ExecutionStep(IDeclarationNode node, ExecutionStepType type)
        {
            Node = node;

            Type = type;
        }

        /// <summary>
        /// Whether or not the execution step is asynchronous.
        /// </summary>
        public bool IsAsynchronous => Type == ExecutionStepType.Asynchronous;

        /// <summary>
        /// The declaration node to evaluate when this step is executed.
        /// </summary>
        public IDeclarationNode Node { get; }

        /// <summary>
        /// The type of the execution step.
        /// </summary>
        public ExecutionStepType Type { get; }
    }
}