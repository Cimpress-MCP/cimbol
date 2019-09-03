namespace Cimpress.Cimbol.Runtime
{
    /// <summary>
    /// Contains metadata about the context of the execution of an execution step.
    /// </summary>
    public class ExecutionStepContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExecutionStepContext"/> class.
        /// </summary>
        /// <param name="id">The ID of the execution step.</param>
        /// <param name="dependents">The list of dependents for the execution step.</param>
        /// <param name="formulaName">The name of the formula or import being executed.</param>
        /// <param name="moduleName">The name of the module containing the formula or import.</param>
        public ExecutionStepContext(int id, int[] dependents, string formulaName, string moduleName)
        {
            Dependents = dependents;

            FormulaName = formulaName;

            Id = id;

            ModuleName = moduleName;
        }

        /// <summary>
        /// The list of dependents for the execution step.
        /// </summary>
        #pragma warning disable CA1819
        public int[] Dependents { get; }
        #pragma warning enable CA1819

        /// <summary>
        /// The ID of the execution step.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// The name of the formula or import being executed.
        /// </summary>
        public string FormulaName { get; }

        /// <summary>
        /// The name of the module containing the execution step.
        /// </summary>
        public string ModuleName { get; }
    }
}