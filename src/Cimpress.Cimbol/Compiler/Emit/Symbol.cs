using System;
using System.Linq.Expressions;

namespace Cimpress.Cimbol.Compiler.Emit
{
    /// <summary>
    /// A single symbol in a symbol table.
    /// </summary>
    public class Symbol
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Symbol"/> class.
        /// </summary>
        /// <param name="name">The name of the symbol.</param>
        /// <param name="type">The type of the symbol.</param>
        public Symbol(string name, Type type)
        {
            IsReference = false;

            Name = name;

            Type = type;

            Variable = Expression.Variable(type, name);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Symbol"/> class.
        /// </summary>
        /// <param name="name">The name of the symbol.</param>
        /// <param name="otherSymbol">The symbol that this symbol is a clone of.</param>
        public Symbol(string name, Symbol otherSymbol)
        {
            if (otherSymbol == null)
            {
                throw new ArgumentNullException(nameof(otherSymbol));
            }

            IsReference = true;

            Name = name;

            Type = otherSymbol.Type;

            Variable = otherSymbol.Variable;
        }

        /// <summary>
        /// Whether or not this symbol is a reference to another symbol.
        /// </summary>
        public bool IsReference { get; }

        /// <summary>
        /// The name of the symbol.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The type of the symbol.
        /// </summary>
        public Type Type { get; }

        /// <summary>
        /// The variable corresponding to this symbol.
        /// </summary>
        public ParameterExpression Variable { get; }
    }
}