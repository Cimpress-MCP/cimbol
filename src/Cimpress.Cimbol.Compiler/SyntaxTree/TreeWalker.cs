using System.Collections.Generic;
using System.Linq;

namespace Cimpress.Cimbol.Compiler.SyntaxTree
{
    /// <summary>
    /// A utility class for working an abstract syntax tree in a particular order.
    /// </summary>
    public sealed class TreeWalker
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TreeWalker"/> class.
        /// </summary>
        /// <param name="rootNode">The node to start the traversal at.</param>
        public TreeWalker(INode rootNode)
        {
            RootNode = rootNode;
        }

        /// <summary>
        /// The node to start the traversal at.
        /// </summary>
        public INode RootNode { get; }

        /// <summary>
        /// Perform a pre-order walk of the tree, starting at the root node.
        /// A pre-order walk will return the parent node, and then each child node in forward order.
        /// </summary>
        /// <returns>A collection of nodes.</returns>
        public IEnumerable<INode> TraversePreOrder()
        {
            var nodeStack = new Stack<INode>();

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
        public IEnumerable<INode> TraversePostOrder()
        {
            var nodeStack = new Stack<INode>();

            var outStack = new Stack<INode>();

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
    }
}