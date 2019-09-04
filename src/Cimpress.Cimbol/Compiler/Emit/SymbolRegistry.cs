using System;
using System.Collections.Generic;
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
        private readonly Dictionary<ModuleNode, SymbolTable> _scopes;

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

            _scopes = new Dictionary<ModuleNode, SymbolTable>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SymbolRegistry"/> class.
        /// </summary>
        /// <param name="programNode">The program node to initialize the symbol registry from.</param>
        public SymbolRegistry(ProgramNode programNode)
        {
            if (programNode == null)
            {
                throw new ArgumentNullException(nameof(programNode));
            }

            Arguments = new SymbolTable();

            Constants = new SymbolTable();

            Modules = new SymbolTable();

            ErrorList = new Symbol("errorList", typeof(List<CimbolRuntimeException>));

            SkipList = new Symbol("skipList", typeof(bool[]));

            _scopes = new Dictionary<ModuleNode, SymbolTable>();

            SymbolTable symbolTable = null;

            var treeWalker = new TreeWalker(programNode);

            treeWalker
                .OnEnter<ArgumentNode>(argumentNode =>
                {
                    Arguments.Define(argumentNode.Name, typeof(ILocalValue));
                })
                .OnEnter<ConstantNode>(constantNode =>
                {
                    Constants.Define(constantNode.Name, typeof(ILocalValue));
                })
                .OnEnter<FormulaNode>(formulaNode =>
                {
                    symbolTable.Define(formulaNode.Name, typeof(ILocalValue));
                })
                .OnEnter<ImportNode>(importNode =>
                {
                    symbolTable.Define(importNode.Name, typeof(ILocalValue));
                })
                .OnEnter<ModuleNode>(moduleNode =>
                {
                    Modules.Define(moduleNode.Name, typeof(ObjectValue));

                    symbolTable = new SymbolTable();
                })
                .OnExit<ModuleNode>(moduleNode =>
                {
                    _scopes.Add(moduleNode, symbolTable);

                    symbolTable = null;
                });

            treeWalker.Visit();
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
        public IReadOnlyDictionary<ModuleNode, SymbolTable> Scopes => _scopes;

        /// <summary>
        /// Get the symbol table for a given module node.
        /// </summary>
        /// <param name="moduleNode">The module node.</param>
        /// <returns>The symbol table for the module node.</returns>
        public SymbolTable GetModuleScope(ModuleNode moduleNode)
        {
            if (_scopes.TryGetValue(moduleNode, out var scope))
            {
                return scope;
            }

            return null;
        }
    }
}