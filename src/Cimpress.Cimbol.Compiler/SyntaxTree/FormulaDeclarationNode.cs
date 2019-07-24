using System.Collections.Generic;

namespace Cimpress.Cimbol.Compiler.SyntaxTree
{
    /// <summary>
    /// A syntax node representing an entire formula.
    /// </summary>
    public class FormulaDeclarationNode : IDeclarationNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FormulaDeclarationNode"/> class.
        /// </summary>
        /// <param name="name">The name to assign to the formula's result.</param>
        /// <param name="body">The body of the formula.</param>
        public FormulaDeclarationNode(string name, IExpressionNode body)
        {
            Body = body;

            Name = name;
        }

        /// <summary>
        /// The body of the formula.
        /// </summary>
        public IExpressionNode Body { get; }

        /// <summary>
        /// The name to assign to the formula's result.
        /// </summary>
        public string Name { get; }

        /// <inheritdoc cref="ISyntaxNode.Children"/>
        public IEnumerable<ISyntaxNode> Children()
        {
            yield return Body;
        }

        /// <inheritdoc cref="ISyntaxNode.ChildrenReverse"/>
        public IEnumerable<ISyntaxNode> ChildrenReverse()
        {
            yield return Body;
        }

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            return $"{{{nameof(FormulaDeclarationNode)} {Name}}}";
        }
    }
}