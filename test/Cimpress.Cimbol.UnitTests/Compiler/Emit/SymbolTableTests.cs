using System.Linq;
using Cimpress.Cimbol.Compiler.Emit;
using Cimpress.Cimbol.Runtime.Types;
using NUnit.Framework;

namespace Cimpress.Cimbol.UnitTests.Compiler.Emit
{
    [TestFixture]
    public class SymbolTableTests
    {
        [Test]
        public void Should_BeEmpty_When_Constructed()
        {
            var result = new SymbolTable();

            Assert.That(result.Symbols, Is.Empty);
        }

        [Test]
        public void Should_DefineSymbol_When_SymbolNotInSelf()
        {
            var symbolTable = new SymbolTable();
            symbolTable.Define("y", typeof(ILocalValue));

            var result = symbolTable.Define("x", typeof(ILocalValue));

            Assert.That(result, Is.True);
        }

        [Test]
        public void ShouldNot_DefineSymbol_When_SymbolInSelf()
        {
            var symbolTable = new SymbolTable();
            symbolTable.Define("x", typeof(ILocalValue));

            var result = symbolTable.Define("x", typeof(ILocalValue));

            Assert.That(result, Is.False);
        }

        [Test]
        public void Should_DefineSymbolFromReference_When_SymbolNotInSelf()
        {
            var original = new Symbol("a", typeof(ILocalValue));
            var symbolTable = new SymbolTable();
            symbolTable.Define("y", typeof(ILocalValue));

            var result = symbolTable.Define("x", original);

            Assert.That(result, Is.True);
        }

        [Test]
        public void ShouldNot_DefineSymbolFromReference_When_SymbolInSelf()
        {
            var original = new Symbol("a", typeof(ILocalValue));
            var symbolTable = new SymbolTable();
            symbolTable.Define("x", typeof(ILocalValue));

            var result = symbolTable.Define("x", original);

            Assert.That(result, Is.False);
        }

        [Test]
        public void Should_RetrieveParameter_When_SymbolInSelf()
        {
            var symbolTable = new SymbolTable();
            symbolTable.Define("x", typeof(ILocalValue));

            var result = symbolTable.Resolve("x");

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void ShouldNot_RetrieveParameter_When_SymbolNotInSelf()
        {
            var symbolTable = new SymbolTable();
            symbolTable.Define("x", typeof(ILocalValue));

            var result = symbolTable.Resolve("y");

            Assert.That(result, Is.Null);
        }

        [Test]
        public void Should_ListSymbols_When_Enumerating()
        {
            var symbolTable = new SymbolTable();
            symbolTable.Define("x", typeof(ILocalValue));
            symbolTable.Define("y", typeof(ILocalValue));
            symbolTable.Define("z", typeof(ILocalValue));

            var symbols = symbolTable.Symbols.ToArray();

            Assert.That(symbols, Has.Length.EqualTo(3));
        }
    }
}