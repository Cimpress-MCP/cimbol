using System.Collections.Generic;
using Cimpress.Cimbol.Compiler.Parse;
using Cimpress.Cimbol.Compiler.Scan;
using Cimpress.Cimbol.Compiler.Source;
using Cimpress.Cimbol.Compiler.SyntaxTree;

namespace Cimpress.Cimbol.Compiler.Utilities
{
    /// <summary>
    /// A collection of static methods to orchestrate parsing formulas.
    /// </summary>
    public static class FormulaParser
    {
        /// <summary>
        /// Turn part of a formula into an abstract syntax tree.
        /// </summary>
        /// <param name="formulaPart">The formula to parse.</param>
        /// <returns>An abstract syntax tree representing that part of the formula.</returns>
        public static IExpressionNode ParseFormulaPart(string formulaPart)
        {
            var sourceText = new SourceText(formulaPart);

            var scanner = new Scanner(sourceText);

            var parser = new Parser(new TokenStream(GetTokens(scanner), 2));

            return parser.Expression();
        }

        private static IEnumerable<Token> GetTokens(Scanner scanner)
        {
            var nextToken = scanner.Next();

            while (nextToken.Type != TokenType.EndOfFile)
            {
                yield return nextToken;
                nextToken = scanner.Next();
            }

            yield return nextToken;
        }
    }
}