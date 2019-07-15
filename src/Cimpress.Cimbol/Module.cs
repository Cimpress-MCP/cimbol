using System;
using System.Collections.Generic;

namespace Cimpress.Cimbol
{
    /// <summary>
    /// An unit of executable code that shares a scope within a <see cref="Program"/>.
    /// </summary>
    public class Module : IResource
    {
        private readonly IDictionary<string, Formula> _formulas;

        private readonly IDictionary<string, IResource> _references;

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

            _references = new Dictionary<string, IResource>(StringComparer.OrdinalIgnoreCase);
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
            if (_formulas.ContainsKey(formulaName) || _references.ContainsKey(formulaName))
            {
                // Disallow adding duplicate references to a module.
                throw new NotSupportedException();
            }

            var formula = new Formula(this, formulaName, formulaValue, flags);

            _formulas[formulaName] = formula;

            _references[formulaName] = formula;

            return formula;
        }

        /// <summary>
        /// Add a reference to another module to the module.
        /// </summary>
        /// <param name="referenceName">The name to reference the module contents by.</param>
        /// <param name="resource">The resource to reference.</param>
        public void AddReference(string referenceName, IResource resource)
        {
            if (referenceName == null)
            {
                // Reference names must not be null.
                throw new ArgumentNullException(nameof(referenceName));
            }

            if (resource == null)
            {
                throw new ArgumentNullException(nameof(resource));
            }

            if (!ReferenceEquals(Program, resource.Program))
            {
                // Disallow adding a reference to a resource outside of the program.
                throw new NotSupportedException();
            }

            if (ReferenceEquals(this, resource))
            {
                // Disallow adding a self-reference to a module within that module.
                throw new NotSupportedException();
            }

            if (resource is Formula formula)
            {
                if (ReferenceEquals(this, formula.Module))
                {
                    // Disallow adding a reference to a formula within the module that owns the formula.
                    throw new NotSupportedException();
                }

                if (!formula.IsReferenceable)
                {
                    // Disallow adding a reference to a formula when that formula is not referenceable.
                    throw new NotSupportedException();
                }
            }

            if (_references.ContainsKey(referenceName))
            {
                // Disallow adding duplicate references to a module.
                throw new NotSupportedException();
            }

            _references[referenceName] = resource;
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
    }
}
