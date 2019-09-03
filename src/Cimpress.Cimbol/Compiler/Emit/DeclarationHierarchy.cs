using System.Collections.Generic;
using Cimpress.Cimbol.Compiler.SyntaxTree;
using Cimpress.Cimbol.Utilities;

namespace Cimpress.Cimbol.Compiler.Emit
{
    /// <summary>
    /// A utility class that maintains information related to the hierarchy of a program.
    /// This stores which declarations belong to which modules.
    /// </summary>
    public class DeclarationHierarchy
    {
        private readonly Dictionary<IDeclarationNode, ModuleDeclarationNode> _hierarchy;

        /// <summary>
        /// Initialize a new instance of the <see cref="DeclarationHierarchy"/> class.
        /// </summary>
        /// <param name="programNode">The program node to initialize the declaration hierarchy from.</param>
        internal DeclarationHierarchy(ProgramNode programNode)
        {
            _hierarchy = BuildHierarchy(programNode);
        }

        /// <summary>
        /// Get the parent module of a given declaration node.
        /// </summary>
        /// <param name="declarationNode">The declaration node to get the parent module declaration of.</param>
        /// <returns>The parent module of the given declaration node if it exists, or null otherwise.</returns>
        internal ModuleDeclarationNode GetParentModule(IDeclarationNode declarationNode)
        {
            return _hierarchy.TryGetValue(declarationNode, out var moduleDeclarationNode)
                ? moduleDeclarationNode
                : null;
        }

        private static Dictionary<IDeclarationNode, ModuleDeclarationNode> BuildHierarchy(ProgramNode programNode)
        {
            ModuleDeclarationNode parentModule = null;

            var hierarchy = new Dictionary<IDeclarationNode, ModuleDeclarationNode>();

            var treeWalker = new TreeWalker(programNode);

            treeWalker.OnEnter<FormulaDeclarationNode>(formulaDeclarationNode =>
            {
                hierarchy[formulaDeclarationNode] = parentModule;
            });

            treeWalker.OnEnter<ImportDeclarationNode>(importDeclarationNode =>
            {
                hierarchy[importDeclarationNode] = parentModule;
            });

            treeWalker.OnEnter<ModuleDeclarationNode>(moduleDeclarationNode =>
            {
                parentModule = moduleDeclarationNode;
            });

            treeWalker.OnExit<ModuleDeclarationNode>(moduleDeclarationNode =>
            {
                parentModule = null;
            });

            treeWalker.Visit();

            return hierarchy;
        }
    }
}