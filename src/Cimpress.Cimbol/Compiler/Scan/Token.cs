using Cimpress.Cimbol.Utilities;

namespace Cimpress.Cimbol.Compiler.Scan
{
    /// <summary>
    /// A token resulting from a scan of some source text.
    /// </summary>
    public class Token
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Token"/> class.
        /// </summary>
        /// <param name="value">The contents of the token.</param>
        /// <param name="type">The type of the token.</param>
        /// <param name="start">The starting position of the token.</param>
        /// <param name="end">The ending position of the token.</param>
        public Token(string value, TokenType type, Position start, Position end)
        {
            End = end;

            Type = type;

            Start = start;

            Value = value;
        }

        /// <summary>
        /// The ending position of the token.
        /// </summary>
        public Position End { get; }

        /// <summary>
        /// The type of the token.
        /// </summary>
        public TokenType Type { get; }

        /// <summary>
        /// The starting position of the token.
        /// </summary>
        public Position Start { get; }

        /// <summary>
        /// The textual value of the token.
        /// </summary>
        public string Value { get; }
    }
}