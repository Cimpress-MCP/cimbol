using System;
using System.Collections.Generic;

namespace Cimpress.Cimbol.Compiler.Scan
{
    /// <summary>
    /// A stream of tokens that can store an arbitrary amount of lookahead tokens.
    /// </summary>
    public class TokenStream
    {
        private readonly Token[] _lookaheadBuffer;

        private readonly int _lookaheadSize;

        private readonly IEnumerator<Token> _tokenEnumerator;

        private int _lookaheadIndex;

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenStream"/> class.
        /// </summary>
        /// <param name="tokens">The <see cref="Token"/> enumerable.</param>
        /// <param name="lookaheadSize">The number of tokens to lookahead by. Defaults to 1.</param>
        public TokenStream(IEnumerable<Token> tokens, int lookaheadSize = 1)
        {
            if (tokens == null)
            {
                throw new ArgumentNullException(nameof(tokens));
            }

            _lookaheadIndex = 0;

            _lookaheadSize = lookaheadSize;

            // This is initialized with room for N + 1 tokens to accomodate all lookahead tokens plus the current token.
            _lookaheadBuffer = new Token[_lookaheadSize + 1];

            _tokenEnumerator = tokens.GetEnumerator();

            for (var i = 0; i < _lookaheadSize + 1; ++i)
            {
                if (!_tokenEnumerator.MoveNext())
                {
                    break;
                }

                _lookaheadBuffer[i] = _tokenEnumerator.Current;
            }
        }

        /// <summary>
        /// Get the current <see cref="Token"/> in the <see cref="TokenStream"/>.
        /// Analogous to calling <see cref="Lookahead"/> with an index of 0.
        /// </summary>
        /// <returns>
        /// The current <see cref="Token"/> in the <see cref="TokenStream"/>,
        /// or null if end of the stream was reached.
        /// </returns>
        public Token Current()
        {
            return _lookaheadBuffer[_lookaheadIndex];
        }

        /// <summary>
        /// Get one of the buffered lookahead <see cref="Token"/> in the <see cref="TokenStream"/>.
        /// </summary>
        /// <param name="amount">The number of tokens to lookahead.</param>
        /// <returns>
        /// The next lookahead <see cref="Token"/> in the <see cref="TokenStream"/>,
        /// or null if this extends past the end of the stream.
        /// </returns>
        public Token Lookahead(int amount)
        {
            if (amount > _lookaheadSize)
            {
                throw new ArgumentOutOfRangeException(nameof(amount));
            }

            // The buffer size is N + 1, where N is the number of lookahead tokens.
            // This means that to wrap properly, we need to wrap with N + 1 instead of N.
            var index = (_lookaheadIndex + amount) % (_lookaheadSize + 1);

            return _lookaheadBuffer[index];
        }

        /// <summary>
        /// Move the next <see cref="Token"/> in the <see cref="TokenStream"/>.
        /// </summary>
        /// <returns>True if the next <see cref="Token"/> is not null.</returns>
        public bool Next()
        {
            var next = _tokenEnumerator.MoveNext() ? _tokenEnumerator.Current : null;

            // The current index is actually the last in the ring buffer after being incremented.
            _lookaheadBuffer[_lookaheadIndex] = next;

            // The buffer size is N + 1, where N is the number of lookahead tokens.
            // This means that to wrap properly, we need to wrap with N + 1 instead of N.
            _lookaheadIndex = (_lookaheadIndex + 1) % (_lookaheadSize + 1);

            return _lookaheadBuffer[_lookaheadIndex] != null;
        }
    }
}