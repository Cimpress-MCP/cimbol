using System.Collections.Generic;
using Cimpress.Cimbol.Runtime.Types;

namespace Cimpress.Cimbol.Compiler.SyntaxTree
{
    /// <summary>
    /// A syntax node that declares a constant value in a program.
    /// </summary>
    public class ConstantDeclarationNode : IDeclarationNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConstantDeclarationNode"/> class.
        /// </summary>
        /// <param name="name">The name of the constant.</param>
        /// <param name="value">The value of the constant.</param>
        public ConstantDeclarationNode(string name, ILocalValue value)
        {
            Name = name;

            Value = value;
        }

        /// <inheritdoc cref="IDeclarationNode"/>
        public string Name { get; }

        /// <summary>
        /// The value of the declared constant.
        /// </summary>
        public ILocalValue Value { get; }

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
            return $"{{{nameof(ConstantDeclarationNode)} {Name}}}";
        }
    }
}