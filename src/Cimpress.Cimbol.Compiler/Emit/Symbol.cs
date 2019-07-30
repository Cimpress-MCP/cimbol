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
            Name = name;

            Type = type;

            Variable = Expression.Variable(type, name);
        }

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