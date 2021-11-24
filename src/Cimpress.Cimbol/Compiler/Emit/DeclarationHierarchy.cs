// Copyright 2021 Cimpress plc.
// Licensed under the Apache License, Version 2.0
// http://www.apache.org/licenses/LICENSE-2.0

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
        private readonly Dictionary<IDeclarationNode, ModuleNode> _hierarchy;

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
        internal ModuleNode GetParentModule(IDeclarationNode declarationNode)
        {
            return _hierarchy.TryGetValue(declarationNode, out var moduleDeclarationNode)
                ? moduleDeclarationNode
                : null;
        }

        private static Dictionary<IDeclarationNode, ModuleNode> BuildHierarchy(ProgramNode programNode)
        {
            ModuleNode parentModule = null;

            var hierarchy = new Dictionary<IDeclarationNode, ModuleNode>();

            var treeWalker = new TreeWalker(programNode);

            treeWalker.OnEnter<FormulaNode>(formulaNode =>
            {
                hierarchy[formulaNode] = parentModule;
            });

            treeWalker.OnEnter<ImportNode>(importNode =>
            {
                hierarchy[importNode] = parentModule;
            });

            treeWalker.OnEnter<ModuleNode>(moduleNode =>
            {
                parentModule = moduleNode;
            });

            treeWalker.OnExit<ModuleNode>(moduleNode =>
            {
                parentModule = null;
            });

            treeWalker.Visit();

            return hierarchy;
        }
    }
}