namespace Cimpress.Cimbol.Compiler.Scan
{
    /// <summary>
    /// The list of token types.
    /// </summary>
    public enum TokenType
    {
        /// <summary>
        /// The token type for the "+" operator.
        /// </summary>
        Add,

        /// <summary>
        /// The token type for the "-" operator.
        /// </summary>
        Subtract,

        /// <summary>
        /// The token type for the "*" operator.
        /// </summary>
        Multiply,

        /// <summary>
        /// The token type for the "/" operator.
        /// </summary>
        Divide,

        /// <summary>
        /// The token type for the "^" operator.
        /// </summary>
        Power,

        /// <summary>
        /// The token type for the "%" operator.
        /// </summary>
        Remainder,

        /// <summary>
        /// The token type for the "++" operator.
        /// </summary>
        Concatenate,

        /// <summary>
        /// The token type for the "==" operator.
        /// </summary>
        Equal,

        /// <summary>
        /// The token type for the "!=" operator.
        /// </summary>
        NotEqual,

        /// <summary>
        /// The token type for the "<" operator.
        /// </summary>
        LessThan,

        /// <summary>
        /// The token type for the "<=" operator.
        /// </summary>
        LessThanEqual,

        /// <summary>
        /// The token type for the ">" operator.
        /// </summary>
        GreaterThan,

        /// <summary>
        /// The token type for the ">=" operator.
        /// </summary>
        GreaterThanEqual,

        /// <summary>
        /// The token type for the "and" operator.
        /// </summary>
        And,

        /// <summary>
        /// The token type for the "not" operator.
        /// </summary>
        Not,

        /// <summary>
        /// The token type for the "or" operator.
        /// </summary>
        Or,

        /// <summary>
        /// The token type for the "xor" operator.
        /// </summary>
        Xor,

        /// <summary>
        /// The token type for the operator "=".
        /// </summary>
        Assign,

        /// <summary>
        /// The token type for the grouping delimiter "(".
        /// </summary>
        LeftParenthesis,

        /// <summary>
        /// The token type for the grouping delimiter ")".
        /// </summary>
        RightParenthesis,

        /// <summary>
        /// The token type for the delimiter ".".
        /// </summary>
        Period,

        /// <summary>
        /// The token type for the delimiter ",".
        /// </summary>
        Comma,

        /// <summary>
        /// The token type for an identifier.
        /// </summary>
        Identifier,

        /// <summary>
        /// The token type for the "await" keyword.
        /// </summary>
        AwaitKeyword,

        /// <summary>
        /// The token type for the "if" keyword.
        /// </summary>
        IfKeyword,

        /// <summary>
        /// The token type for the "list" keyword.
        /// </summary>
        ListKeyword,

        /// <summary>
        /// The token type for the "object" keyword.
        /// </summary>
        ObjectKeyword,

        /// <summary>
        /// The token type for the "where" keyword.
        /// </summary>
        WhereKeyword,

        /// <summary>
        /// The token type for a boolean literal.
        /// </summary>
        BooleanLiteral,

        /// <summary>
        /// The token type for a number literal.
        /// </summary>
        NumberLiteral,

        /// <summary>
        /// The token type for a string literal.
        /// </summary>
        StringLiteral,

        /// <summary>
        /// The token type for the end of the file.
        /// </summary>
        EndOfFile,
    }
}