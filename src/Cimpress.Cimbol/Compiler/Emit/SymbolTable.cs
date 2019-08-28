using System;
using System.Collections;
using System.Collections.Generic;

namespace Cimpress.Cimbol.Compiler.Emit
{
    /// <summary>
    /// A symbol table.
    /// </summary>
    public class SymbolTable : IReadOnlyCollection<KeyValuePair<string, Symbol>>
    {
        private readonly Dictionary<string, Symbol> _table =
            new Dictionary<string, Symbol>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Initializes a new instance of the <see cref="SymbolTable"/> class with no parent.
        /// </summary>
        /// <param name="registry">The registry that the symbol table belongs to.</param>
        internal SymbolTable(SymbolRegistry registry)
        {
            Parent = null;

            Registry = registry ?? throw new ArgumentNullException(nameof(registry));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SymbolTable"/> class with a parent.
        /// </summary>
        /// <param name="registry">The registry that the symbol table belongs to.</param>
        /// <param name="parent">The parent symbol table.</param>
        internal SymbolTable(SymbolRegistry registry, SymbolTable parent)
        {
            Parent = parent;

            Registry = registry ?? throw new ArgumentNullException(nameof(registry));
        }

        /// <summary>
        /// Get the number of symbols in the symbol table.
        /// </summary>
        public int Count => _table.Count;

        /// <summary>
        /// The parent symbol table.
        /// </summary>
        public SymbolTable Parent { get; }

        /// <summary>
        /// The registry that the symbol table belongs to.
        /// </summary>
        public SymbolRegistry Registry { get; }

        /// <inheritdoc cref="IReadOnlyCollection{T}.GetEnumerator"/>
        public IEnumerator<KeyValuePair<string, Symbol>> GetEnumerator()
        {
            return _table.GetEnumerator();
        }

        /// <inheritdoc cref="IEnumerable.GetEnumerator"/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
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
        /// This traverses the scope's parent scopes, and returns the symbol that matches in the closest scope.
        /// </summary>
        /// <param name="symbolName">The name of the symbol to look up.</param>
        /// <exception cref="NotSupportedException">The symbol could not be found in the symbol table.</exception>
        /// <returns>The symbol matching the specified symbol name.</returns>
        public Symbol Resolve(string symbolName)
        {
            var symbolTable = this;

            while (symbolTable != null)
            {
                if (symbolTable._table.TryGetValue(symbolName, out var symbol))
                {
                    return symbol;
                }

                symbolTable = symbolTable.Parent;
            }

            throw new KeyNotFoundException("Identifier not found in the symbol table.");
        }

        /// <summary>
        /// Try to resolve a symbol by the name of the symbol.
        /// This traverses the scope's parent scopes, and returns the symbol that matches in the closest scope.
        /// </summary>
        /// <param name="symbolName">The name of the symbol.</param>
        /// <param name="symbol">The variable corresponding to the symbol.</param>
        /// <returns>True if the symbol was resolved, false if it does not exist in this scope or any parent scope.</returns>
        public bool TryResolve(string symbolName, out Symbol symbol)
        {
            var symbolTable = this;

            while (symbolTable != null)
            {
                if (symbolTable._table.TryGetValue(symbolName, out symbol))
                {
                    return true;
                }

                symbolTable = symbolTable.Parent;
            }

            symbol = null;

            return false;
        }
    }
}