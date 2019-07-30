namespace Cimpress.Cimbol.Compiler.SyntaxTree
{
    /// <summary>
    /// A positional argument.
    /// </summary>
    public class PositionalArgument : IArgument
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PositionalArgument"/> class.
        /// </summary>
        /// <param name="value">The value of the argument.</param>
        public PositionalArgument(IExpressionNode value)
        {
            Value = value;
        }

        /// <summary>
        /// The value of the argument.
        /// </summary>
        public IExpressionNode Value { get; }
    }
}