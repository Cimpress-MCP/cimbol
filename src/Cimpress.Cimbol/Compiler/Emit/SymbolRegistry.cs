using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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
        /// <summary>
        /// Initializes a new instance of the <see cref="SymbolRegistry"/> class.
        /// </summary>
        /// <param name="arguments">The arguments in the program.</param>
        /// <param name="constants">The constants in the program.</param>
        /// <param name="modules">The modules in the program.</param>
        /// <param name="symbolTables">The symbol tables in the program.</param>
        public SymbolRegistry(
            IDictionary<string, Symbol> arguments,
            IDictionary<string, Symbol> constants,
            IDictionary<string, Symbol> modules,
            IDictionary<IDeclarationNode, SymbolTable> symbolTables)
        {
            Arguments = arguments.ToImmutableDictionary(StringComparer.OrdinalIgnoreCase);

            Constants = constants.ToImmutableDictionary(StringComparer.OrdinalIgnoreCase);

            Modules = modules.ToImmutableDictionary(StringComparer.OrdinalIgnoreCase);

            SymbolTables = symbolTables.ToImmutableDictionary();
        }

        /// <summary>
        /// The arguments in the program.
        /// </summary>
        public IReadOnlyDictionary<string, Symbol> Arguments { get; }

        /// <summary>
        /// The constants in the program.
        /// </summary>
        public IReadOnlyDictionary<string, Symbol> Constants { get; }

        /// <summary>
        /// The modules in the program.
        /// </summary>
        public IReadOnlyDictionary<string, Symbol> Modules { get; }

        /// <summary>
        /// The symbol tables in the program.
        /// </summary>
        public IReadOnlyDictionary<IDeclarationNode, SymbolTable> SymbolTables { get; }

        /// <summary>
        /// Builds a <see cref="SymbolRegistry"/> object from an abstract syntax tree.
        /// </summary>
        /// <param name="rootNode">The root of the abstract syntax tree.</param>
        /// <returns>A complete set of symbols in the program.</returns>
        public static SymbolRegistry Build(ProgramNode rootNode)
        {
            var arguments = new Dictionary<string, Symbol>(StringComparer.OrdinalIgnoreCase);
            var constants = new Dictionary<string, Symbol>(StringComparer.OrdinalIgnoreCase);
            var modules = new Dictionary<string, Symbol>(StringComparer.OrdinalIgnoreCase);
            var symbolTables = new Dictionary<IDeclarationNode, SymbolTable>();

            var tableStack = new Stack<SymbolTable>();
            tableStack.Push(new SymbolTable());

            var treeWalker = new TreeWalker(rootNode);

            treeWalker
                .OnExit<ArgumentDeclarationNode>(argumentDeclarationNode =>
                {
                    var name = argumentDeclarationNode.Name;
                    arguments[name] = new Symbol(name, typeof(ILocalValue));
                })
                .OnExit<ConstantDeclarationNode>(constantDeclarationNode =>
                {
                    var name = constantDeclarationNode.Name;
                    constants[name] = new Symbol(name, typeof(ILocalValue));
                })
                .OnExit<FormulaDeclarationNode>(formulaDeclarationNode =>
                {
                    var symbolTable = tableStack.Peek();
                    symbolTable.Define(formulaDeclarationNode.Name, typeof(ILocalValue));
                })
                .OnExit<ImportDeclarationNode>(importStatementNode =>
                {
                    var symbolTable = tableStack.Peek();
                    symbolTable.Define(importStatementNode.Name, typeof(ILocalValue));
                })
                .OnEnter<ModuleDeclarationNode>(moduleDeclarationNode =>
                {
                    var parentSymbolTable = tableStack.Peek();

                    var symbolTable = new SymbolTable(parentSymbolTable);
                    tableStack.Push(symbolTable);

                    symbolTables[moduleDeclarationNode] = symbolTable;
                })
                .OnExit<ModuleDeclarationNode>(moduleDeclarationNode =>
                {
                    modules[moduleDeclarationNode.Name] =
                        new Symbol(moduleDeclarationNode.Name, typeof(IDictionary<string, ILocalValue>));

                    tableStack.Pop();
                });

            treeWalker.Visit();

            return new SymbolRegistry(arguments, constants, modules, symbolTables);
        }
    }
}