using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Cimpress.Cimbol.Compiler.Emit
{
    /// <summary>
    /// A symbol table.
    /// </summary>
    public class SymbolTable : IEnumerable<KeyValuePair<string, ParameterExpression>>
    {
        private readonly Dictionary<string, ParameterExpression> _table =
            new Dictionary<string, ParameterExpression>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Initializes a new instance of the <see cref="SymbolTable"/> class with no parent.
        /// </summary>
        public SymbolTable()
        {
            Parent = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SymbolTable"/> class with a parent.
        /// </summary>
        /// <param name="parent">The parent symbol table.</param>
        public SymbolTable(SymbolTable parent)
        {
            Parent = parent;
        }

        /// <summary>
        /// The parent symbol table.
        /// </summary>
        public SymbolTable Parent { get; }

        /// <inheritdoc cref="IReadOnlyCollection{T}.GetEnumerator"/>
        public IEnumerator<KeyValuePair<string, ParameterExpression>> GetEnumerator()
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

            var variable = Expression.Variable(symbolType, symbolName);

            _table[symbolName] = variable;

            return true;
        }

        /// <summary>
        /// Resolve a symbol by the name of the symbol.
        /// This traverses the scope's parent scopes, and returns the symbol that matches in the closest scope.
        /// </summary>
        /// <param name="symbolName">The name of the symbol.</param>
        /// <param name="variable">The variable corresponding to the symbol.</param>
        /// <returns>True if the symbol was resolved, false if it does not exist in this scope or any parent scope.</returns>
        public bool Resolve(string symbolName, out ParameterExpression variable)
        {
            var symbolTable = this;

            while (symbolTable != null)
            {
                if (symbolTable._table.TryGetValue(symbolName, out variable))
                {
                    return true;
                }

                symbolTable = symbolTable.Parent;
            }

            variable = null;

            return false;
        }
    }
}