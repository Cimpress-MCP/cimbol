using System.Collections.Generic;

namespace Cimpress.Cimbol.Compiler.SyntaxTree
{
    /// <summary>
    /// A syntax node that declares an argument to the program.
    /// </summary>
    public class ArgumentDeclarationNode : IDeclarationNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ArgumentDeclarationNode"/> class.
        /// </summary>
        /// <param name="name">The name of the argument.</param>
        public ArgumentDeclarationNode(string name)
        {
            Name = name;
        }

        /// <inheritdoc cref="IDeclarationNode.Name"/>
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
            return $"{{{nameof(ArgumentDeclarationNode)} {Name}}}";
        }
    }
}