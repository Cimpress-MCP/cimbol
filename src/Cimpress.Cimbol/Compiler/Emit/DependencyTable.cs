using System;
using System.Collections.Generic;
using System.Linq;
using Cimpress.Cimbol.Compiler.SyntaxTree;
using Cimpress.Cimbol.Utilities;

namespace Cimpress.Cimbol.Compiler.Emit
{
    /// <summary>
    /// A tree that describes the process by which a series of expressions are evaluated.
    /// </summary>
    public class DependencyTable
    {
        private readonly Graph<IDeclarationNode> _dependencyGraph;

        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyTable"/> class.
        /// </summary>
        /// <param name="dependencyGraph">The graph of dependencies between declarations.</param>
        public DependencyTable(Graph<IDeclarationNode> dependencyGraph)
        {
            _dependencyGraph = dependencyGraph ?? throw new ArgumentNullException(nameof(dependencyGraph));
        }

        /// <summary>
        /// Builds a execution plan from an abstract syntax tree.
        /// </summary>
        /// <param name="programNode">The root of the abstract syntax tree.</param>
        /// <returns>An execution plan.</returns>
        public static DependencyTable Build(ProgramNode programNode)
        {
            var dependencyTable = new Dictionary<IDeclarationNode, HashSet<IDeclarationNode>>();

            ModuleDeclarationNode currentModule = null;
            FormulaDeclarationNode currentFormula = null;

            var treeWalker = new TreeWalker(programNode);

            treeWalker.OnEnter<FormulaDeclarationNode>(formulaDeclarationNode =>
            {
                currentFormula = formulaDeclarationNode;
            });

            treeWalker.OnExit<FormulaDeclarationNode>(formulaDeclarationNode =>
            {
                currentFormula = null;

                if (!dependencyTable.ContainsKey(formulaDeclarationNode))
                {
                    dependencyTable[formulaDeclarationNode] = new HashSet<IDeclarationNode>();
                }
            });

            treeWalker.OnExit<IdentifierNode>(identifierNode =>
            {
                if (currentFormula == null || currentModule == null)
                {
                    // If this is reached, something has gone wrong.
                    throw new NotSupportedException("ErrorCode001");
                }

                if (!dependencyTable.TryGetValue(currentFormula, out var dependencies))
                {
                    dependencies = new HashSet<IDeclarationNode>();
                    dependencyTable[currentFormula] = dependencies;
                }

                if (currentModule.TryGetFormulaDeclaration(identifierNode.Identifier, out var formula))
                {
                    dependencies.Add(formula);
                }

                if (currentModule.TryGetImportDeclaration(identifierNode.Identifier, out var import))
                {
                    dependencies.Add(import);
                }
            });

            treeWalker.OnExit<ImportDeclarationNode>(importDeclarationNode =>
            {
                var moduleName = importDeclarationNode.ImportPath.ElementAtOrDefault(0);

                var formulaName = importDeclarationNode.ImportPath.ElementAtOrDefault(1);

                var dependencies = new HashSet<IDeclarationNode>();

                dependencyTable[importDeclarationNode] = dependencies;

                if (importDeclarationNode.ImportType == ImportType.Formula)
                {
                    if (moduleName == null || formulaName == null || importDeclarationNode.ImportPath.Count != 2)
                    {
                        throw new NotSupportedException("ErrorCode002");
                    }

                    if (!programNode.TryGetModuleDeclaration(moduleName, out var moduleNode))
                    {
                        // The imported module was not found in the program.
                        return;
                    }

                    if (!moduleNode.TryGetFormulaDeclaration(formulaName, out var formulaNode))
                    {
                        // The imported formula was not found in the module.
                        return;
                    }

                    dependencies.Add(formulaNode);
                }

                if (importDeclarationNode.ImportType == ImportType.Module)
                {
                    if (moduleName == null || importDeclarationNode.ImportPath.Count != 1)
                    {
                        throw new NotSupportedException("ErrorCode003");
                    }

                    if (!programNode.TryGetModuleDeclaration(moduleName, out var moduleNode))
                    {
                        // The imported module was not found in the program.
                        return;
                    }

                    foreach (var formulaNode in moduleNode.Formulas)
                    {
                        if (formulaNode.IsExported)
                        {
                            dependencies.Add(formulaNode);
                        }
                    }
                }
            });

            treeWalker.OnEnter<ModuleDeclarationNode>(moduleDeclarationNode =>
            {
                currentModule = moduleDeclarationNode;
            });

            treeWalker.OnExit<ModuleDeclarationNode>(moduleDeclarationNode =>
            {
                currentModule = null;
            });

            treeWalker.Visit();

            var vertices = dependencyTable.Keys;

            var edges = dependencyTable.SelectMany(vertexEdges =>
            {
                var fromVertex = vertexEdges.Key;

                return vertexEdges.Value.Select(toVertex => Tuple.Create(toVertex, fromVertex));
            });

            var graph = new Graph<IDeclarationNode>(vertices, edges);

            if (graph.IsCyclical())
            {
                // Do not allow cycles between declarations.
#pragma warning disable CA1303
                throw new NotSupportedException("ErrorCode004");
#pragma warning restore CA1303
            }

            return new DependencyTable(graph);
        }

        /// <summary>
        /// Determines the partial ordering of the given declarations such that there are as few partial orderings as
        /// possible.
        /// A partial ordering of dependencies is a partial ordering where each declaration that is a dependent of
        /// another declaration is considered less than the declaration that depends on it.
        /// </summary>
        /// <returns>
        /// A list of sets of declarations, where each set has elements less than the elements after it.
        /// </returns>
        public IReadOnlyCollection<ISet<IDeclarationNode>> MinimalPartialOrder()
        {
            return _dependencyGraph.MinimalPartialOrder();
        }
    }
}