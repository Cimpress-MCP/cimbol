using System;
using System.Collections.Generic;
using System.Linq;
using Cimpress.Cimbol.Compiler.SyntaxTree;
using Cimpress.Cimbol.Exceptions;
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
        /// <param name="programNode">The program node to build the dependency table from.</param>
        public DependencyTable(ProgramNode programNode)
        {
            if (programNode == null)
            {
                throw new ArgumentNullException(nameof(programNode));
            }

            _dependencyGraph = BuildDependencyGraph(programNode);
        }

        /// <summary>
        /// Get the dependencies for a given declaration node.
        /// The dependencies of a declaration are the nodes that it requires for evaluation.
        /// For example, if a formula A exists and is equal to the expression "B + 1", B is a dependency of A.
        /// </summary>
        /// <param name="declarationNode">The declaration node to get the dependencies of.</param>
        /// <returns>Either a list of the dependencies of the given declaration node, or an empty collection.</returns>
        public IReadOnlyCollection<IDeclarationNode> GetDependencies(IDeclarationNode declarationNode)
        {
            return _dependencyGraph.AdjacentsIn(declarationNode);
        }

        /// <summary>
        /// Get the dependents for a given declaration node.
        /// The dependents of a declaration are the nodes that use this as a dependency.
        /// For example, if a formula A exists and is equal to the expression "B + 1", A is a dependent of B.
        /// </summary>
        /// <param name="declarationNode">The declaration node to get the dependents of.</param>
        /// <returns>Either a list of the dependents of the given declaration node, or an empty collection.</returns>
        public IReadOnlyCollection<IDeclarationNode> GetDependents(IDeclarationNode declarationNode)
        {
            return _dependencyGraph.AdjacentsOut(declarationNode);
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

        /// <summary>
        /// Determines the topological sort of the given declarations.
        /// A successful topological sort will have every declaration node positioned in the collection such that none of
        /// that node's dependents come before it and none of that node's dependencies come after it.
        /// </summary>
        /// <returns>
        /// A list of nodes that have been sorted based on a partial ordering.
        /// </returns>
        public IReadOnlyCollection<IDeclarationNode> TopologicalSort()
        {
            return _dependencyGraph.TopologicalSort();
        }

        private static Graph<IDeclarationNode> BuildDependencyGraph(ProgramNode programNode)
        {
            var dependencyTable = new Dictionary<IDeclarationNode, HashSet<IDeclarationNode>>();

            ModuleNode currentModule = null;
            FormulaNode currentFormula = null;

            var treeWalker = new TreeWalker(programNode);

            treeWalker.OnEnter<FormulaNode>(formulaNode =>
            {
                currentFormula = formulaNode;
            });

            treeWalker.OnExit<FormulaNode>(formulaNode =>
            {
                currentFormula = null;

                if (!dependencyTable.ContainsKey(formulaNode))
                {
                    dependencyTable[formulaNode] = new HashSet<IDeclarationNode>();
                }
            });

            treeWalker.OnExit<DefaultNode>(defaultNode =>
            {
                if (currentFormula == null || currentModule == null)
                {
                    throw new CimbolInternalException("An error occurred while generating the dependency tree.");
                }

                if (!dependencyTable.TryGetValue(currentFormula, out var dependencies))
                {
                    dependencies = new HashSet<IDeclarationNode>();
                    dependencyTable[currentFormula] = dependencies;
                }

                if (currentModule.TryGetFormula(defaultNode.Path.First(), out var formula))
                {
                    dependencies.Add(formula);
                }

                if (currentModule.TryGetImport(defaultNode.Path.First(), out var import))
                {
                    dependencies.Add(import);
                }
            });

            treeWalker.OnExit<IdentifierNode>(identifierNode =>
            {
                if (currentFormula == null || currentModule == null)
                {
                    throw new CimbolInternalException("An error occurred while generating the dependency tree.");
                }

                if (!dependencyTable.TryGetValue(currentFormula, out var dependencies))
                {
                    dependencies = new HashSet<IDeclarationNode>();
                    dependencyTable[currentFormula] = dependencies;
                }

                if (currentModule.TryGetFormula(identifierNode.Identifier, out var formula))
                {
                    dependencies.Add(formula);
                }

                if (currentModule.TryGetImport(identifierNode.Identifier, out var import))
                {
                    dependencies.Add(import);
                }
            });

            treeWalker.OnExit<ImportNode>(importNode =>
            {
                var moduleName = importNode.ImportPath.ElementAtOrDefault(0);

                var formulaName = importNode.ImportPath.ElementAtOrDefault(1);

                var dependencies = new HashSet<IDeclarationNode>();

                dependencyTable[importNode] = dependencies;

                if (importNode.ImportType == ImportType.Formula)
                {
                    if (moduleName == null || formulaName == null || importNode.ImportPath.Count != 2)
                    {
                        throw new CimbolInternalException("An error occurred while generating the dependency tree.");
                    }

                    if (!programNode.TryGetModule(moduleName, out var moduleNode))
                    {
                        // The imported module was not found in the program.
                        return;
                    }

                    if (!moduleNode.TryGetFormula(formulaName, out var formulaNode))
                    {
                        // The imported formula was not found in the module.
                        return;
                    }

                    dependencies.Add(formulaNode);
                }

                if (importNode.ImportType == ImportType.Module)
                {
                    if (moduleName == null || importNode.ImportPath.Count != 1)
                    {
                        throw new CimbolInternalException("An error occurred while generating the dependency tree.");
                    }

                    if (!programNode.TryGetModule(moduleName, out var moduleNode))
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

            treeWalker.OnEnter<ModuleNode>(moduleNode =>
            {
                currentModule = moduleNode;
            });

            treeWalker.OnExit<ModuleNode>(moduleNode =>
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
                // TODO: Log the formulas that form a cycle.
                throw CimbolCompilationException.CycleError(null);
            }

            return graph;
        }
    }
}