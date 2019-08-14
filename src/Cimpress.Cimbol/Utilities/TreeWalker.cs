using System;
using System.Collections.Generic;
using System.Linq;
using Cimpress.Cimbol.Compiler.SyntaxTree;

namespace Cimpress.Cimbol.Utilities
{
    /// <summary>
    /// A utility class for working an abstract syntax tree in a particular order.
    /// </summary>
    public class TreeWalker
    {
        private readonly IDictionary<Type, Action<ISyntaxNode>> _enterFunctions;

        private readonly IDictionary<Type, Action<ISyntaxNode>> _exitFunctions;

        /// <summary>
        /// Initializes a new instance of the <see cref="TreeWalker"/> class.
        /// </summary>
        /// <param name="rootNode">The node to start the traversal at.</param>
        public TreeWalker(ISyntaxNode rootNode)
        {
            _enterFunctions = new Dictionary<Type, Action<ISyntaxNode>>();

            _exitFunctions = new Dictionary<Type, Action<ISyntaxNode>>();

            RootNode = rootNode;
        }

        private enum TreeAction
        {
            Enter,

            Exit,
        }

        /// <summary>
        /// The node to start the traversal at.
        /// </summary>
        public ISyntaxNode RootNode { get; }

        /// <summary>
        /// Perform a pre-order walk of the tree, starting at the root node.
        /// A pre-order walk will return the parent node, and then each child node in forward order.
        /// </summary>
        /// <returns>A collection of nodes.</returns>
        public IEnumerable<ISyntaxNode> TraversePreOrder()
        {
            var nodeStack = new Stack<ISyntaxNode>();

            nodeStack.Push(RootNode);

            while (nodeStack.Any())
            {
                var currentNode = nodeStack.Pop();

                yield return currentNode;

                foreach (var childNode in currentNode.ChildrenReverse())
                {
                    nodeStack.Push(childNode);
                }
            }
        }

        /// <summary>
        /// Perform a post-order walk of the tree, starting at the root node.
        /// A post-order walk will return each child node in forward order, and then the parent node.
        /// </summary>
        /// <returns>A collection of nodes.</returns>
        public IEnumerable<ISyntaxNode> TraversePostOrder()
        {
            var nodeStack = new Stack<ISyntaxNode>();

            var outStack = new Stack<ISyntaxNode>();

            nodeStack.Push(RootNode);

            while (nodeStack.Any())
            {
                var currentNode = nodeStack.Pop();

                outStack.Push(currentNode);

                foreach (var childNode in currentNode.Children())
                {
                    nodeStack.Push(childNode);
                }
            }

            return outStack.AsEnumerable();
        }

        /// <summary>
        /// Register a handler for entering an <see cref="ISyntaxNode"/>.
        /// </summary>
        /// <typeparam name="T">The type of <see cref="ISyntaxNode"/> to handle.</typeparam>
        /// <param name="enterFunction">The callback to run when the given node is entered.</param>
        /// <returns>The tree walker.</returns>
        public TreeWalker OnEnter<T>(Action<T> enterFunction)
            where T : class, ISyntaxNode
        {
            var nodeType = typeof(T);

            if (_enterFunctions.ContainsKey(nodeType))
            {
                // Disallow adding duplicate handlers for the same node type.
                throw new NotSupportedException("ErrorCode084");
            }

            _enterFunctions[nodeType] = node => enterFunction(node as T);

            return this;
        }

        /// <summary>
        /// Register a handler for exiting an <see cref="ISyntaxNode"/>.
        /// </summary>
        /// <typeparam name="T">The type of <see cref="ISyntaxNode"/> to handle.</typeparam>
        /// <param name="exitFunction">The callback to run when the given node is exited.</param>
        /// <returns>The tree walker.</returns>
        public TreeWalker OnExit<T>(Action<T> exitFunction)
            where T : class, ISyntaxNode
        {
            var nodeType = typeof(T);

            if (_exitFunctions.ContainsKey(nodeType))
            {
                // Disallow adding duplicate handlers for the same node type.
                throw new NotSupportedException("ErrorCode085");
            }

            _exitFunctions[nodeType] = node => exitFunction(node as T);

            return this;
        }

        /// <summary>
        /// Visit each node in the syntax tree, calling functions whenever a node is entered or exited.
        /// </summary>
        /// <param name="onEnter">The callback for when a node is entered.</param>
        /// <param name="onExit">The callback for when a node is exited.</param>
        public void Visit()
        {
            var nodeStack = new Stack<Tuple<ISyntaxNode, TreeAction>>();

            nodeStack.Push(Tuple.Create(RootNode, TreeAction.Exit));
            nodeStack.Push(Tuple.Create(RootNode, TreeAction.Enter));

            while (nodeStack.Any())
            {
                var (node, action) = nodeStack.Pop();

                var nodeType = node.GetType();

                if (action == TreeAction.Enter)
                {
                    if (_enterFunctions.TryGetValue(nodeType, out var handler))
                    {
                        handler(node);
                    }

                    foreach (var childNode in node.ChildrenReverse())
                    {
                        nodeStack.Push(Tuple.Create(childNode, TreeAction.Exit));
                        nodeStack.Push(Tuple.Create(childNode, TreeAction.Enter));
                    }
                }
                else if (_exitFunctions.TryGetValue(nodeType, out var handler))
                {
                    handler(node);
                }
            }
        }
    }
}
