// Copyright 2021 Cimpress plc.
// Licensed under the Apache License, Version 2.0
// http://www.apache.org/licenses/LICENSE-2.0

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Cimpress.Cimbol.Compiler.SyntaxTree
{
    /// <summary>
    /// The syntax tree node for declaring a module.
    /// </summary>
    public class ModuleNode : ISyntaxNode
    {
        private readonly ImmutableDictionary<string, FormulaNode> _formulaTable;

        private readonly ImmutableDictionary<string, ImportNode> _importTable;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleNode"/> class.
        /// </summary>
        /// <param name="name">The name of the module.</param>
        /// <param name="imports">The list of imports in the module.</param>
        /// <param name="formulas">The list of formulas in the module.</param>
        public ModuleNode(
            string name,
            IEnumerable<ImportNode> imports,
            IEnumerable<FormulaNode> formulas)
        {
            Formulas = formulas?.ToImmutableArray() ?? throw new ArgumentNullException(nameof(formulas));

            _formulaTable = Formulas.ToImmutableDictionary(
                formula => formula.Name,
                formula => formula,
                StringComparer.OrdinalIgnoreCase);

            Imports = imports?.ToImmutableArray() ?? throw new ArgumentNullException(nameof(imports));

            _importTable = Imports.ToImmutableDictionary(
                import => import.Name,
                import => import,
                StringComparer.OrdinalIgnoreCase);

            Name = name;
        }

        /// <summary>
        /// The list of formulas in the module.
        /// </summary>
        public IEnumerable<FormulaNode> Formulas { get; }

        /// <summary>
        /// The list of imports in the module.
        /// </summary>
        public IEnumerable<ImportNode> Imports { get; }

        /// <summary>
        /// The name to assign to the module.
        /// </summary>
        public string Name { get; }

        /// <inheritdoc cref="ISyntaxNode.Children"/>
        public IEnumerable<ISyntaxNode> Children()
        {
            foreach (var import in _importTable.Values)
            {
                yield return import;
            }

            foreach (var formula in _formulaTable.Values)
            {
                yield return formula;
            }
        }

        /// <inheritdoc cref="ISyntaxNode.ChildrenReverse"/>
        public IEnumerable<ISyntaxNode> ChildrenReverse()
        {
            foreach (var formula in _formulaTable.Values.Reverse())
            {
                yield return formula;
            }

            foreach (var import in _importTable.Values.Reverse())
            {
                yield return import;
            }
        }

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            return $"{{{nameof(ModuleNode)} {Name}}}";
        }

        /// <summary>
        /// Try and retrieve a formula by name from the module.
        /// </summary>
        /// <param name="formulaName">The name of the formula to retrieve.</param>
        /// <param name="formula">The retrieved formula.</param>
        /// <returns>Whether or not the formula was retrieved.</returns>
        public bool TryGetFormula(string formulaName, out FormulaNode formula)
        {
            return _formulaTable.TryGetValue(formulaName, out formula);
        }

        /// <summary>
        /// Try and retrieve an import by name from the module.
        /// </summary>
        /// <param name="importName">The name of the import to retrieve.</param>
        /// <param name="import">The retrieved import.</param>
        /// <returns>Whether or not the import was retrieved.</returns>
        public bool TryGetImport(string importName, out ImportNode import)
        {
            return _importTable.TryGetValue(importName, out import);
        }
    }
}