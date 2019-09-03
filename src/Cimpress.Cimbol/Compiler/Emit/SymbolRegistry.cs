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
        private readonly Dictionary<IDeclarationNode, Symbol> _exportSymbols;

        private readonly Dictionary<ISyntaxNode, SymbolTable> _symbolTables;

        /// <summary>
        /// Initializes a new instance of the <see cref="SymbolRegistry"/> class.
        /// </summary>
        public SymbolRegistry()
        {
            Arguments = new SymbolTable(this);

            Constants = new SymbolTable(this);

            ErrorList = new Symbol("errorList", typeof(List<CimbolRuntimeException>));

            Modules = new SymbolTable(this);

            SkipList = new Symbol("skipList", typeof(bool[]));

            _exportSymbols = new Dictionary<IDeclarationNode, Symbol>();

            _symbolTables = new Dictionary<ISyntaxNode, SymbolTable>();
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

            ErrorList = new Symbol("errorList", typeof(List<CimbolRuntimeException>));

            Modules = new SymbolTable(this);

            SkipList = new Symbol("skipList", typeof(bool[]));

            _exportSymbols = new Dictionary<IDeclarationNode, Symbol>();

            _symbolTables = new Dictionary<ISyntaxNode, SymbolTable>();

            Symbol exportSymbol = null;

            var tableStack = new Stack<SymbolTable>();

            var treeWalker = new TreeWalker(programNode);

            tableStack.Push(new SymbolTable(this));

            treeWalker
                .OnEnter<ArgumentNode>(argumentNode =>
                {
                    var name = argumentNode.Name;

                    Arguments.Define(name, typeof(ILocalValue));
                })
                .OnEnter<ConstantNode>(constantNode =>
                {
                    var name = constantNode.Name;

                    Constants.Define(name, typeof(ILocalValue));
                })
                .OnEnter<FormulaNode>(formulaNode =>
                {
                    var symbolTable = tableStack.Peek();

                    symbolTable.Define(formulaNode.Name, typeof(ILocalValue));

                    _exportSymbols.Add(formulaNode, exportSymbol);

                    _symbolTables.Add(formulaNode, symbolTable);
                })
                .OnEnter<ImportNode>(importNode =>
                {
                    var symbolTable = tableStack.Peek();

                    symbolTable.Define(importNode.Name, typeof(ILocalValue));

                    _exportSymbols.Add(importNode, exportSymbol);

                    _symbolTables.Add(importNode, symbolTable);
                })
                .OnEnter<ModuleNode>(moduleNode =>
                {
                    var name = moduleNode.Name;

                    Modules.Define(name, typeof(ObjectValue));

                    var parentSymbolTable = tableStack.Peek();

                    var symbolTable = new SymbolTable(this, parentSymbolTable);

                    tableStack.Push(symbolTable);

                    exportSymbol = Modules.Resolve(name);

                    _symbolTables.Add(moduleNode, symbolTable);
                })
                .OnExit<ModuleNode>(moduleNode =>
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
        /// The symbol tables in the program.
        /// </summary>
        public IReadOnlyDictionary<ISyntaxNode, SymbolTable> SymbolTables => _symbolTables;

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
        public SymbolTable GetSymbolTable(ISyntaxNode declarationNode)
        {
            if (_symbolTables.TryGetValue(declarationNode, out var symbolTable))
            {
                return symbolTable;
            }

            throw new ArgumentException("The provided declaration node does not have a corresponding symbol table.");
        }
    }
}