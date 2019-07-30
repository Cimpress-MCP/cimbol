using System;
using Cimpress.Cimbol.Compiler.SyntaxTree;
using Cimpress.Cimbol.Utilities;

namespace Cimpress.Cimbol
{
    /// <summary>
    /// A Cimbol expression to be run within the context of a <see cref="Module"/>.
    /// </summary>
    public class Formula : IResource
    {
        private readonly FormulaFlags _flags;

        /// <summary>
        /// Initializes a new instance of the <see cref="Formula"/> class.
        /// </summary>
        /// <param name="module">The <see cref="Module"/> that the formula belongs to.</param>
        /// <param name="name">The formula's name.</param>
        /// <param name="value">The formula's expression.</param>
        /// <param name="flags">The flags to apply to the formula to determine its visibility.</param>
        internal Formula(Module module, string name, string value, FormulaFlags flags = FormulaFlags.Public)
        {
            _flags = flags;

            Module = module ?? throw new ArgumentNullException(nameof(module));

            Name = name ?? throw new ArgumentNullException(nameof(name));

            Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// True if the formula is returned as a result of the evaluation.
        /// </summary>
        public bool IsExported => _flags.HasFlag(FormulaFlags.Exported);

        /// <summary>
        /// True if the formula can be referenced from other modules.
        /// </summary>
        public bool IsReferenceable => _flags.HasFlag(FormulaFlags.Referenceable);

        /// <summary>
        /// The <see cref="Module"/> that the formula belongs to.
        /// </summary>
        public Module Module { get; }

        /// <summary>
        /// The name of the formula.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The <see cref="Program"/> that the formula belongs to.
        /// </summary>
        public Program Program => Module.Program;

        /// <summary>
        /// The formula's expression.
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// Parse the formula and return an abstract syntax equivalent to the formula.
        /// </summary>
        /// <returns>An abstract syntax tree equivalent to the formula.</returns>
        internal FormulaDeclarationNode ToSyntaxTree()
        {
            var body = FormulaParser.ParseFormulaPart(Value);

            return new FormulaDeclarationNode(Name, body, IsExported);
        }
    }
}
