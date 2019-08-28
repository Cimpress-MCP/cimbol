using System;
using System.Collections.Generic;
using Cimpress.Cimbol.Compiler.SyntaxTree;
using Cimpress.Cimbol.Runtime.Types;
using Cimpress.Cimbol.Utilities;

namespace Cimpress.Cimbol.Compiler.Emit
{
    /// <summary>
    /// Holds all of the information about a program's symbols.
    /// </summary>
    public class SymbolRegistry
    {
        private readonly Dictionary<IDeclarationNode, Symbol> _exportSymbols;

        private readonly Dictionary<IDeclarationNode, SymbolTable> _symbolTables;

        /// <summary>
        /// Initializes a new instance of the <see cref="SymbolRegistry"/> class.
        /// </summary>
        public SymbolRegistry()
        {
            Arguments = new SymbolTable(this);

            Constants = new SymbolTable(this);

            Modules = new SymbolTable(this);

            SkipList = new Symbol("skipList", typeof(bool[]));

            _exportSymbols = new Dictionary<IDeclarationNode, Symbol>();

            _symbolTables = new Dictionary<IDeclarationNode, SymbolTable>();
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

            Arguments = new SymbolTable(this);

            Constants = new SymbolTable(this);

            Modules = new SymbolTable(this);

            SkipList = new Symbol("skipList", typeof(bool[]));

            _exportSymbols = new Dictionary<IDeclarationNode, Symbol>();

            _symbolTables = new Dictionary<IDeclarationNode, SymbolTable>();

            Symbol exportSymbol = null;

            var tableStack = new Stack<SymbolTable>();

            var treeWalker = new TreeWalker(programNode);

            tableStack.Push(new SymbolTable(this));

            treeWalker
                .OnEnter<ArgumentDeclarationNode>(argumentDeclarationNode =>
                {
                    var name = argumentDeclarationNode.Name;

                    Arguments.Define(name, typeof(ILocalValue));
                })
                .OnEnter<ConstantDeclarationNode>(constantDeclarationNode =>
                {
                    var name = constantDeclarationNode.Name;

                    Constants.Define(name, typeof(ILocalValue));
                })
                .OnEnter<FormulaDeclarationNode>(formulaDeclarationNode =>
                {
                    var symbolTable = tableStack.Peek();

                    symbolTable.Define(formulaDeclarationNode.Name, typeof(ILocalValue));

                    _exportSymbols.Add(formulaDeclarationNode, exportSymbol);

                    _symbolTables.Add(formulaDeclarationNode, symbolTable);
                })
                .OnEnter<ImportDeclarationNode>(importDeclarationNode =>
                {
                    var symbolTable = tableStack.Peek();

                    symbolTable.Define(importDeclarationNode.Name, typeof(ILocalValue));

                    _exportSymbols.Add(importDeclarationNode, exportSymbol);

                    _symbolTables.Add(importDeclarationNode, symbolTable);
                })
                .OnEnter<ModuleDeclarationNode>(moduleDeclarationNode =>
                {
                    var name = moduleDeclarationNode.Name;

                    Modules.Define(name, typeof(ObjectValue));

                    var parentSymbolTable = tableStack.Peek();

                    var symbolTable = new SymbolTable(this, parentSymbolTable);

                    tableStack.Push(symbolTable);

                    exportSymbol = Modules.Resolve(name);

                    _symbolTables.Add(moduleDeclarationNode, symbolTable);
                })
                .OnExit<ModuleDeclarationNode>(moduleDeclarationNode =>
                {
                    tableStack.Pop();
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
        /// The modules in the program.
        /// </summary>
        public SymbolTable Modules { get; }

        /// <summary>
        /// The symbol for the program's skip list.
        /// </summary>
        public Symbol SkipList { get; }

        /// <summary>
        /// The symbol tables in the program.
        /// </summary>
        public IReadOnlyDictionary<IDeclarationNode, SymbolTable> SymbolTables => _symbolTables;

        /// <summary>
        /// Get the export symbol for a given declaration node.
        /// </summary>
        /// <param name="declarationNode">The declaration node.</param>
        /// <returns>The export symbol for the declaration node.</returns>
        public Symbol GetExportSymbol(IDeclarationNode declarationNode)
        {
            if (_exportSymbols.TryGetValue(declarationNode, out var exportSymbol))
            {
                return exportSymbol;
            }

            throw new ArgumentException("The provided declaration node does not have a corresponding export symbol.");
        }

        /// <summary>
        /// Get the symbol table for a given declaration node.
        /// </summary>
        /// <param name="declarationNode">The declaration node.</param>
        /// <returns>The symbol table for the declaration node.</returns>
        public SymbolTable GetSymbolTable(IDeclarationNode declarationNode)
        {
            if (_symbolTables.TryGetValue(declarationNode, out var symbolTable))
            {
                return symbolTable;
            }

            throw new ArgumentException("The provided declaration node does not have a corresponding symbol table.");
        }

        /// <summary>
        /// Try to get an export symbol for a given declaration node.
        /// </summary>
        /// <param name="declarationNode">The declaration node.</param>
        /// <param name="exportSymbol">The export symbol for the declaration node.</param>
        /// <returns>True if the export symbol was retrieved, false otherwise.</returns>
        public bool TryGetExportSymbol(IDeclarationNode declarationNode, out Symbol exportSymbol)
        {
            return _exportSymbols.TryGetValue(declarationNode, out exportSymbol);
        }

        /// <summary>
        /// Try to get a symbol table for a given declaration node.
        /// </summary>
        /// <param name="declarationNode">The declaration node.</param>
        /// <param name="symbolTable">The symbol table for the declaration node.</param>
        /// <returns>True if the symbol table was retrieved, false otherwise.</returns>
        public bool TryGetSymbolTable(IDeclarationNode declarationNode, out SymbolTable symbolTable)
        {
            return _symbolTables.TryGetValue(declarationNode, out symbolTable);
        }
    }
}