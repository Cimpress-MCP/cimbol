using System;

namespace Cimpress.Cimbol
{
    /// <summary>
    /// Flags for formulas to determine the visibility of the formula.
    /// </summary>
    [Flags]
    public enum FormulaFlags
    {
        /// <summary>
        /// Shorthand for no flags set.
        /// </summary>
        None = 0,

        /// <summary>
        /// If set, the formula is returned when the program finishes executing.
        /// </summary>
        Exported = 1,

        /// <summary>
        /// If set, the formula is referenceable from other module.
        /// </summary>
        Referenceable = 2,

        /// <summary>
        /// If set, the formula cannot be referenced from other modules and is not return as result of the evaluation.
        /// </summary>
        Private = None,

        /// <summary>
        /// If set, the formula can be referenced from other modules but is not returned as a result of the evaluation.
        /// </summary>
        Internal = Referenceable,

        /// <summary>
        /// If set, the formula can be referenced from other modules but is not returned as a result of the evaluation.
        /// </summary>
        Public = Exported | Referenceable,
    }
}