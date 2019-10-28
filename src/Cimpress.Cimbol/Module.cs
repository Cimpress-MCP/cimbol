using System;
using System.Collections.Generic;
using System.Linq;
using Cimpress.Cimbol.Compiler.SyntaxTree;

namespace Cimpress.Cimbol
{
    /// <summary>
    /// An unit of executable code that shares a scope within a <see cref="Program"/>.
    /// </summary>
    public class Module : IResource
    {
        private readonly IDictionary<string, Formula> _formulas;

        private readonly IDictionary<string, Import> _imports;

        /// <summary>
        /// Initializes a new instance of the <see cref="Module"/> class.
        /// </summary>
        /// <param name="name">The name of the module.</param>
        /// <param name="program">The program that the module belongs to.</param>
        internal Module(Program program, string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));

            Program = program ?? throw new ArgumentNullException(nameof(program));

            _formulas = new Dictionary<string, Formula>(StringComparer.OrdinalIgnoreCase);

            _imports = new Dictionary<string, Import>(StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// The name of the module.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The program that the module belongs to.
        /// </summary>
        public Program Program { get; }

        /// <summary>
        /// Add a formula to execute within the scope of this module.
        /// </summary>
        /// <param name="formulaName">The name of the formula.</param>
        /// <param name="formulaValue">The formula's expression.</param>
        /// <param name="flags">The flags to apply to the formula to determine its visibility.</param>
        /// <returns>The newly created formula.</returns>
        public Formula AddFormula(string formulaName, string formulaValue, FormulaFlags flags = FormulaFlags.Public)
        {
            if (formulaName == null)
            {
                // Formula names cannot be null.
                throw new ArgumentNullException(nameof(formulaName));
            }

            if (formulaValue == null)
            {
                // Formulas cannot be null.
                throw new ArgumentNullException(nameof(formulaValue));
            }

            if (_formulas.ContainsKey(formulaName) || _imports.ContainsKey(formulaName))
            {
                // Disallow adding duplicate references to a module.
                throw new ArgumentException("Duplicate formula name added to module.", nameof(formulaName));
            }

            var formula = new Formula(this, formulaName, formulaValue, flags);

            _formulas[formulaName] = formula;

            return formula;
        }

        /// <summary>
        /// Add a reference to another module to the module.
        /// </summary>
        /// <param name="importName">The name to reference the module contents by.</param>
        /// <param name="importResource">The imported resource to reference.</param>
        /// <param name="isExported">Whether or not the import should be re-exported.</param>
        public void AddImport(string importName, IResource importResource, bool isExported = false)
        {
            if (importName == null)
            {
                // Reference names must not be null.
                throw new ArgumentNullException(nameof(importName));
            }

            if (importResource == null)
            {
                // Resources cannot be null.
                throw new ArgumentNullException(nameof(importResource));
            }

            var import = new Import(this, importName, importResource, isExported);

            if (!ReferenceEquals(Program, import.Value.Program))
            {
                // Disallow adding duplicate references to a module.
                throw new ArgumentException(
                    "Imported resources must belong to the same program as the importing module.",
                    nameof(importResource));
            }

            if (ReferenceEquals(this, import.Value))
            {
                // Disallow adding a self-reference to a module within that module.
                throw new ArgumentException("Cannot add a module as a reference to itself.", nameof(importResource));
            }

            if (import.Value is Formula formula)
            {
                if (ReferenceEquals(this, formula.Module))
                {
                    // Disallow adding a reference to a formula within the module that owns the formula.
                    throw new ArgumentException(
                        "Cannot add a reference to a formula within the module that owns the formula.",
                        nameof(importResource));
                }

                if (!formula.IsReferenceable)
                {
                    // Disallow adding a reference to a formula when that formula is not referenceable.
                    throw new ArgumentException(
                        "Cannot add a reference to a formula that is not referenceable.",
                        nameof(importResource));
                }
            }

            if (_formulas.ContainsKey(importName) || _imports.ContainsKey(import.Name))
            {
                // Disallow adding duplicate references to a module.
                throw new ArgumentException("Duplicate reference name added to module.", nameof(importName));
            }

            _imports[import.Name] = import;
        }

        /// <summary>
        /// Try to retrieve a formula from the module by name.
        /// </summary>
        /// <param name="formulaName">The formula name to look up by.</param>
        /// <param name="formula">The result of getting the formula out of the module.</param>
        /// <returns>True if the formula was retrieved, false otherwise.</returns>
        public bool TryGetFormula(string formulaName, out Formula formula)
        {
            return _formulas.TryGetValue(formulaName, out formula);
        }

        /// <summary>
        /// Compile the module into an abstract syntax tree.
        /// </summary>
        /// <returns>An abstract syntax tree.</returns>
        internal ModuleNode ToSyntaxTree()
        {
            var imports = _imports.Values.Select(import => import.ToSyntaxTree());

            var formulas = _formulas.Values.Select(formula => formula.ToSyntaxTree());

            return new ModuleNode(Name, imports, formulas);
        }
    }
}
