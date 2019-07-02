using System;
using System.Globalization;

namespace Cimpress.Cimbol.Compiler.Utilities
{
    /// <summary>
    /// The source text for a snippet of Cimbol code.
    /// </summary>
    public class SourceText
    {
        private readonly string _source;

        private string _character;

        private int _index;

        /// <summary>
        /// Initializes a new instance of the <see cref="TextWindow"/> class.
        /// </summary>
        /// <param name="source">The source string.</param>
        public SourceText(string source)
        {
            _character = null;

            _index = 0;

            _source = source ?? throw new ArgumentNullException(nameof(source));

            Column = 0;

            Row = 0;
        }

        /// <summary>
        /// The current column position.
        /// </summary>
        public int Column { get; private set; }

        /// <summary>
        /// Whether or no there is any source text left to read.
        /// </summary>
        public bool EndOfFile => _index >= _source.Length;

        /// <summary>
        /// The current row position.
        /// </summary>
        public int Row { get; private set; }

        /// <summary>
        /// Advances the pointer in the source text to the next valid character.
        /// </summary>
        /// <returns>True if the pointer has not yet reached the end of the source text.</returns>
        public bool Next()
        {
            if (_index >= _source.Length)
            {
                return false;
            }

            _index = GetNextIndex();

            if (_index >= _source.Length)
            {
                _character = GetCurrentCharacter();
                Column += 1;
                return false;
            }

            if (!IsValid())
            {
                throw new NotSupportedException();
            }

            _character = GetCurrentCharacter();

            if (IsNewline())
            {
                Row += 1;
                Column = 0;
            }
            else
            {
                Column += 1;
            }

            return true;
        }

        /// <summary>
        /// Get the character at the current read position.
        /// Note that this is a UTF-16 encoded string, and can be up to two <see cref="char"/> long.
        /// </summary>
        /// <returns>The character at the current read position.</returns>
        public string Peek()
        {
            if (_character == null)
            {
                Start();
            }

            return _character;
        }

        private string GetCurrentCharacter()
        {
            if (_index >= _source.Length)
            {
                return string.Empty;
            }

            var offset = char.IsSurrogatePair(_source, _index) ? 2 : 1;

            return _source.Substring(_index, offset);
        }

        private int GetNextIndex()
        {
            if (_index >= _source.Length)
            {
                return _source.Length;
            }

            var offset = char.IsSurrogatePair(_source, _index) ? 2 : 1;

            return _index + offset;
        }

        private bool IsNewline()
        {
            var isSpace = CharUnicodeInfo.GetUnicodeCategory(_source, _index) == UnicodeCategory.SpaceSeparator;
            var isWhiteSpace = char.IsWhiteSpace(_source, _index);
            return isWhiteSpace && !isSpace;
        }

        private bool IsValid()
        {
            if (_source.Length - _index >= 2)
            {
                var isSurrogate = char.IsSurrogate(_source, _index);
                var isSurrogatePair = char.IsSurrogatePair(_source, _index);
                return !isSurrogate || isSurrogatePair;
            }

            var isBase = !char.IsSurrogate(_source, _index);
            return isBase;
        }

        private void Start()
        {
            if (_index >= _source.Length)
            {
                _character = string.Empty;
                return;
            }

            if (!IsValid())
            {
                throw new NotSupportedException();
            }

            _character = GetCurrentCharacter();

            Column = 0;

            Row = 0;
        }
    }
}