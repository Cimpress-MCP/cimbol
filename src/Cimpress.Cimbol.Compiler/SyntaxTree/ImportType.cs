namespace Cimpress.Cimbol.Compiler.SyntaxTree
{
    /// <summary>
    /// The possible import types.
    /// </summary>
    public enum ImportType
    {
        /// <summary>
        /// Represents the import of an argument.
        /// </summary>
        Argument,

        /// <summary>
        /// Represents the import of a constant.
        /// </summary>
        Constant,

        /// <summary>
        /// Represents the import of a formula.
        /// </summary>
        Formula,

        /// <summary>
        /// Represents the import of a module.
        /// </summary>
        Module,
    }
}