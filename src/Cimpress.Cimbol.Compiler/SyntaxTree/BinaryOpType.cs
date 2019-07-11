namespace Cimpress.Cimbol.Compiler.SyntaxTree
{
    /// <summary>
    /// The possible binary operation types.
    /// </summary>
    public enum BinaryOpType
    {
        /// <summary>
        /// Represents the add arithmetic binary operation.
        /// </summary>
        Add,

        /// <summary>
        /// Represents the and logical binary operation.
        /// </summary>
        And,

        /// <summary>
        /// Represents the concatenate string binary operation.
        /// </summary>
        Concatenate,

        /// <summary>
        /// Represents the divide arithmetic binary operation.
        /// </summary>
        Divide,

        /// <summary>
        /// Represents the equality comparison binary operation.
        /// </summary>
        Equal,

        /// <summary>
        /// Represents the greater than comparison binary operation.
        /// </summary>
        GreaterThan,

        /// <summary>
        /// Represents the greater than or equal comparison binary operation.
        /// </summary>
        GreaterThanOrEqual,

        /// <summary>
        /// Represents the less than comparison binary operation.
        /// </summary>
        LessThan,

        /// <summary>
        /// Represents the less than or equal comparison binary operation.
        /// </summary>
        LessThanOrEqual,

        /// <summary>
        /// Represents the multiply arithmetic binary operation.
        /// </summary>
        Multiply,

        /// <summary>
        /// Represents the not equal comparison binary operation.
        /// </summary>
        NotEqual,

        /// <summary>
        /// Represents the or logical binary operation.
        /// </summary>
        Or,

        /// <summary>
        /// Represents the power arithmetic operation.
        /// </summary>
        Power,

        /// <summary>
        /// Represents the modulo arithmetic binary operation.
        /// </summary>
        Remainder,

        /// <summary>
        /// Represents the subtract arithmetic operation.
        /// </summary>
        Subtract,
    }
}