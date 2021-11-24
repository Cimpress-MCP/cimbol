// Copyright 2021 Cimpress plc.
// Licensed under the Apache License, Version 2.0
// http://www.apache.org/licenses/LICENSE-2.0

using System;
using System.Collections.Generic;
using System.Linq;
using Cimpress.Cimbol.Compiler.SyntaxTree;
using Cimpress.Cimbol.Exceptions;
using Cimpress.Cimbol.Runtime.Types;
using Cimpress.Cimbol.Utilities;

namespace Cimpress.Cimbol.Compiler.Emit
{
    /// <summary>
    /// Holds all of the information about a program's symbols.
    /// </summary>
    public class SymbolRegistry
    {
        private readonly Dictionary<string, SymbolTable> _scopes;

        /// <summary>
        /// Initializes a new instance of the <see cref="SymbolRegistry"/> class.
        /// </summary>
        public SymbolRegistry()
        {
            Arguments = new SymbolTable();

            Constants = new SymbolTable();

            Modules = new SymbolTable();

            ErrorList = new Symbol("errorList", typeof(List<CimbolRuntimeException>));

            SkipList = new Symbol("skipList", typeof(bool[]));

            _scopes = new Dictionary<string, SymbolTable>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SymbolRegistry"/> class.
        /// </summary>
        /// <param name="programNode">The program node to initialize the symbol registry from.</param>
        /// <param name="declarationHierarchy">The program node's declaration hierarchy.</param>
        /// <param name="dependencyTable">The program node's dependency table.</param>
        public SymbolRegistry(
            ProgramNode programNode,
            DeclarationHierarchy declarationHierarchy,
            DependencyTable dependencyTable)
        {
            if (programNode == null)
            {
                throw new ArgumentNullException(nameof(programNode));
            }

            if (declarationHierarchy == null)
            {
                throw new ArgumentNullException(nameof(declarationHierarchy));
            }

            if (dependencyTable == null)
            {
                throw new ArgumentNullException(nameof(dependencyTable));
            }

            Arguments = new SymbolTable();

            Constants = new SymbolTable();

            Modules = new SymbolTable();

            ErrorList = new Symbol("errorList", typeof(List<CimbolRuntimeException>));

            SkipList = new Symbol("skipList", typeof(bool[]));

            _scopes = new Dictionary<string, SymbolTable>();

            // Build the global symbol tables for arguments, constants and modules.
            // Because these do not re-use symbols like imports do, they can be added in any order.
            var treeWalker = new TreeWalker(programNode);

            treeWalker
                .OnEnter<ArgumentNode>(argumentNode =>
                {
                    Arguments.Define(argumentNode.Name, typeof(ILocalValue));
                })
                .OnEnter<ConstantNode>(constantNode =>
                {
                    Constants.Define(constantNode.Name, constantNode.Value.GetType());
                })
                .OnEnter<ModuleNode>(moduleNode =>
                {
                    Modules.Define(moduleNode.Name, typeof(ObjectValue));

                    _scopes.Add(moduleNode.Name, new SymbolTable());
                });

            treeWalker.Visit();

            // Build the module symbol tables.
            // This is done via a topological sort because an import node's symbol cannot be determined
            // until the node that it imports has its symbol determined.
            // This dependency chain necessitates using the dependency table.
            // The formula node symbol construction could be moved up to the tree walker, however either location works.
            foreach (var declarationNode in dependencyTable.TopologicalSort())
            {
                if (declarationNode is FormulaNode formulaNode)
                {
                    var moduleNode = declarationHierarchy.GetParentModule(formulaNode);
                    if (moduleNode == null || !_scopes.TryGetValue(moduleNode.Name, out var symbolTable))
                    {
                        throw new CimbolInternalException("An error occurred while generating the symbol registry.");
                    }

                    symbolTable.Define(formulaNode.Name, typeof(ILocalValue));
                }
                else if (declarationNode is ImportNode importNode)
                {
                    var moduleNode = declarationHierarchy.GetParentModule(importNode);
                    if (moduleNode == null || !_scopes.TryGetValue(moduleNode.Name, out var symbolTable))
                    {
                        throw new CimbolInternalException("An error occurred while generating the symbol registry.");
                    }

                    var referencedSymbol = ResolveImportNode(importNode);
                    if (referencedSymbol == null)
                    {
                        throw new CimbolInternalException("An error occurred while generating the symbol registry.");
                    }

                    symbolTable.Define(importNode.Name, referencedSymbol);
                }
            }
        }

        /// <summary>
        /// The arguments in the program.
        /// </summary>
        public SymbolTable Arguments { get; }

        /// <summary>
        /// The constants in the program.
        /// </summary>
        public SymbolTable Constants { get; }

        /// <summary>
        /// The symbol for the list of errors encountered while evaluating the program.
        /// </summary>
        public Symbol ErrorList { get; }

        /// <summary>
        /// The modules in the program.
        /// </summary>
        public SymbolTable Modules { get; }

        /// <summary>
        /// The symbol for the program's skip list.
        /// A skip list is an array of booleans that are true if the corresponding formula should be evaluated
        /// Similarly, the entry in the array for a formula is false if it should be skipped.
        /// </summary>
        public Symbol SkipList { get; }

        /// <summary>
        /// The scopes in the program.
        /// </summary>
        public IReadOnlyDictionary<string, SymbolTable> Scopes => _scopes;

        /// <summary>
        /// Get the symbol table for a given module node.
        /// </summary>
        /// <param name="moduleNode">The module node.</param>
        /// <returns>The symbol table for the module node.</returns>
        public SymbolTable GetModuleScope(ModuleNode moduleNode)
        {
            if (moduleNode == null)
            {
                return null;
            }

            if (_scopes.TryGetValue(moduleNode.Name, out var scope))
            {
                return scope;
            }

            return null;
        }

        /// <summary>
        /// Resolve an <see cref="ImportNode"/> to the symbol that it references.
        /// </summary>
        /// <param name="importNode">The import node to resolve.</param>
        /// <returns>The <see cref="Symbol"/> that the import node is importing, or null if the import is invalid.</returns>
        public Symbol ResolveImportNode(ImportNode importNode)
        {
            if (importNode == null)
            {
                return null;
            }

            var path1 = importNode.ImportPath.ElementAtOrDefault(0);
            var path2 = importNode.ImportPath.ElementAtOrDefault(1);

            if (path1 == null || (importNode.ImportType == ImportType.Formula && path2 == null))
            {
                throw new CimbolInternalException("An error occurred while generating the symbol registry.");
            }

            if (importNode.ImportType == ImportType.Argument)
            {
                return Arguments.Resolve(path1);
            }

            if (importNode.ImportType == ImportType.Constant)
            {
                return Constants.Resolve(path1);
            }

            if (importNode.ImportType == ImportType.Formula)
            {
                if (_scopes.TryGetValue(path1, out var symbolTable))
                {
                    return symbolTable.Resolve(path2);
                }

                return null;
            }

            if (importNode.ImportType == ImportType.Module)
            {
                return Modules.Resolve(path1);
            }

            return null;
        }
    }
}