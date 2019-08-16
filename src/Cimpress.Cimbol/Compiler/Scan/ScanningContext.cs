using System;
using System.Text;
using Cimpress.Cimbol.Compiler.Source;
using Cimpress.Cimbol.Utilities;

namespace Cimpress.Cimbol.Compiler.Scan
{
    /// <summary>
    /// Keeps track of the current position in the <see cref="SourceText"/>.
    /// </summary>
    public class ScanningContext
    {
        private const int InitialBuilderCapacity = 64;

        private readonly StringBuilder _builder;

        private readonly SourceText _sourceText;

        private Position _mark;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScanningContext"/> class.
        /// </summary>
        /// <param name="sourceText">The <see cref="SourceText"/> to scan.</param>
        public ScanningContext(SourceText sourceText)
        {
            if (sourceText == null)
            {
                throw new ArgumentNullException(nameof(sourceText));
            }

            _builder = new StringBuilder(InitialBuilderCapacity);

            _mark = new Position(sourceText.Row, sourceText.Column);

            _sourceText = sourceText;
        }

        /// <summary>
        /// Get the text of the token being built.
        /// </summary>
        public string Current => _builder.ToString();

        /// <summary>
        /// Whether or no there is any source text left to read.
        /// </summary>
        public bool EndOfFile => _sourceText.EndOfFile;

        /// <summary>
        /// Advance to the next character.
        /// </summary>
        public void Advance()
        {
            _builder.Append(_sourceText.Peek());
            _sourceText.Next();
        }

        /// <summary>
        /// Consume all the characters from the last point consumption happened up until the current position.
        /// </summary>
        /// <param name="type">The type of <see cref="Token"/> to return.</param>
        /// <returns>A <see cref="Token"/> of the given type.</returns>
        public Token Consume(TokenType type)
        {
            var value = _builder.ToString();
            _builder.Clear();

            var end = new Position(_sourceText.Row, _sourceText.Column);
            var token = new Token(value, type, _mark, end);

            _mark = end;
            return token;
        }

        /// <summary>
        /// Ensure that there are no left over characters, and then build an end-of-file <see cref="Token"/>.
        /// </summary>
        /// <returns>An end-of-file <see cref="Token"/>.</returns>
        public Token ConsumeEndOfFile()
        {
            if (_mark.Row != _sourceText.Row || _mark.Column != _sourceText.Column)
            {
                // Unexpected end of file reached.
                // Currently still processing a token.
                // This is an error in the lexer, not in the input string.
#pragma warning disable CA1303
                throw new NotSupportedException("ErrorCode032");
#pragma warning restore CA1303
            }

            return Consume(TokenType.EndOfFile);
        }

        /// <summary>
        /// Peeks the next character in the <see cref="SourceText"/>.
        /// </summary>
        /// <returns>The character at the current read position.</returns>
        public string Peek()
        {
            return _sourceText.Peek();
        }

        /// <summary>
        /// Ignore all the characters from the last point consumption happened up until the current position.
        /// </summary>
        public void Skip()
        {
            _builder.Clear();

            var end = new Position(_sourceText.Row, _sourceText.Column);

            _mark = end;
        }
    }
}