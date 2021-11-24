// Copyright 2021 Cimpress plc.
// Licensed under the Apache License, Version 2.0
// http://www.apache.org/licenses/LICENSE-2.0

using System;
using Cimpress.Cimbol.Compiler.SyntaxTree;
using Cimpress.Cimbol.Exceptions;

namespace Cimpress.Cimbol
{
    /// <summary>
    /// An import of an argument, constant, formula or module into another module.
    /// </summary>
    public class Import : IResource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Import"/> class.
        /// </summary>
        /// <param name="module">The <see cref="Module"/> that the import belongs to.</param>
        /// <param name="name">The import's name.</param>
        /// <param name="value">The import's expression.</param>
        /// <param name="isExported">Whether or not the import should be re-exported.</param>
        internal Import(Module module, string name, IResource value, bool isExported)
        {
            IsExported = isExported;

            Module = module ?? throw new ArgumentNullException(nameof(module));

            Name = name ?? throw new ArgumentNullException(nameof(name));

            if (value is Import import)
            {
                Value = import.Value ?? throw new ArgumentNullException(nameof(value));
            }
            else
            {
                Value = value ?? throw new ArgumentNullException(nameof(value));
            }
        }

        /// <summary>
        /// True if the import is returned as a result of the evaluation.
        /// </summary>
        public bool IsExported { get; }

        /// <summary>
        /// The <see cref="Module"/> that the import belongs to.
        /// </summary>
        public Module Module { get; }

        /// <summary>
        /// The name of the import.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The <see cref="Program"/> that the import belongs to.
        /// </summary>
        public Program Program => Module.Program;

        /// <summary>
        /// The <see cref="IResource"/> that the import is importing.
        /// </summary>
        public IResource Value { get; }

        /// <summary>
        /// Parse the formula and return an abstract syntax equivalent to the formula.
        /// </summary>
        /// <returns>An abstract syntax tree equivalent to the formula.</returns>
        internal ImportNode ToSyntaxTree()
        {
            switch (Value)
            {
                case Argument argument:
                    return new ImportNode(Name, new[] { argument.Name }, ImportType.Argument, IsExported);

                case Constant constant:
                    return new ImportNode(Name, new[] { constant.Name }, ImportType.Constant, IsExported);

                case Formula formula:
                    return new ImportNode(
                        Name,
                        new[] { formula.Module.Name, formula.Name },
                        ImportType.Formula,
                        IsExported);

                case Module module:
                    return new ImportNode(Name, new[] { module.Name }, ImportType.Module, IsExported);

                default:
                    throw new CimbolInternalException("Error constructing syntax tree import.");
            }
        }
    }
}