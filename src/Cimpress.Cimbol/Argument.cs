using System;
using Cimpress.Cimbol.Compiler.SyntaxTree;

namespace Cimpress.Cimbol
{
    /// <summary>
    /// An argument that can be passed in to the <see cref="Program"/>.
    /// </summary>
    public class Argument : IResource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Argument"/> class.
        /// </summary>
        /// <param name="program">The <see cref="Program"/> that the argument belongs to.</param>
        /// <param name="name">The name of the argument.</param>
        internal Argument(Program program, string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));

            Program = program ?? throw new ArgumentNullException(nameof(program));
        }

        /// <summary>
        /// The name of the argument.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The <see cref="Program"/> that the argument belongs to.
        /// </summary>
        public Program Program { get; }

        /// <summary>
        /// Compile the argument into an abstract syntax tree.
        /// </summary>
        /// <returns>An abstract syntax tree.</returns>
        internal ArgumentDeclarationNode ToSyntaxTree()
        {
            return new ArgumentDeclarationNode(Name);
        }
    }
}