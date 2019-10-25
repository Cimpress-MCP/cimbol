using System;
using System.Collections.Generic;

namespace Cimpress.Cimbol.Compiler.Emit
{
    /// <summary>
    /// A symbol table.
    /// </summary>
    public class SymbolTable
    {
        private readonly Dictionary<string, Symbol> _table;

        /// <summary>
        /// Initializes a new instance of the <see cref="SymbolTable"/> class with no parent.
        /// </summary>
        internal SymbolTable()
        {
            _table = new Dictionary<string, Symbol>(StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Return the collection of symbols in the symbol table.
        /// </summary>
        public IReadOnlyCollection<Symbol> Symbols => _table.Values;

        /// <summary>
        /// Add an existing symbol to the symbol table.
        /// </summary>
        /// <param name="symbolName">The symbol name to define.</param>
        /// <param name="otherSymbol">The symbol that this symbol is a clone of.</param>
        /// <returns>True if the symbol was defined, false if it could not be defined.</returns>
        public bool Define(string symbolName, Symbol otherSymbol)
        {
            if (_table.ContainsKey(symbolName))
            {
                return false;
            }

            var symbol = new Symbol(symbolName, otherSymbol);

            _table[symbol.Name] = symbol;

            return true;
        }

        /// <summary>
        /// Define a new symbol in the symbol table.
        /// </summary>
        /// <param name="symbolName">The symbol name to define.</param>
        /// <param name="symbolType">The type of the symbol.</param>
        /// <returns>True if the symbol was defined, false if it could not be defined.</returns>
        public bool Define(string symbolName, Type symbolType)
        {
            if (_table.ContainsKey(symbolName))
            {
                return false;
            }

            var symbol = new Symbol(symbolName, symbolType);

            _table[symbolName] = symbol;

            return true;
        }

        /// <summary>
        /// Resolve a symbol by the name of the symbol.
        /// </summary>
        /// <param name="symbolName">The name of the symbol to look up.</param>
        /// <exception cref="NotSupportedException">The symbol could not be found in the symbol table.</exception>
        /// <returns>The symbol matching the specified symbol name.</returns>
        public Symbol Resolve(string symbolName)
        {
            return _table.TryGetValue(symbolName, out var symbol) ? symbol : null;
        }
    }
}