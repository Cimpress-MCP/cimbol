using System.Collections.Generic;

namespace Cimpress.Cimbol.Compiler.SyntaxTree
{
    /// <summary>
    /// A syntax node that declares an argument to the program.
    /// </summary>
    public class ArgumentNode : ISyntaxNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ArgumentNode"/> class.
        /// </summary>
        /// <param name="name">The name of the argument.</param>
        public ArgumentNode(string name)
        {
            Name = name;
        }

        /// <summary>
        /// The name to assign to the argument.
        /// </summary>
        public string Name { get; }

        /// <inheritdoc cref="ISyntaxNode.Children"/>
        public IEnumerable<ISyntaxNode> Children()
        {
            yield break;
        }

        /// <inheritdoc cref="ISyntaxNode.ChildrenReverse"/>
        public IEnumerable<ISyntaxNode> ChildrenReverse()
        {
            yield break;
        }

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            return $"{{{nameof(ArgumentNode)} {Name}}}";
        }
    }
}