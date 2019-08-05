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
        public void Should_ConstructSymbolTable_When_NotGivenParent()
        {
            var symbolTable = new SymbolTable();

            Assert.That(symbolTable.Parent, Is.Null);
        }

        [Test]
        public void Should_ConstructSymbolTable_When_GivenParent()
        {
            var parentTable = new SymbolTable();

            var symbolTable = new SymbolTable(parentTable);

            Assert.That(symbolTable.Parent, Is.SameAs(parentTable));
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
        public void Should_DefineSymbol_When_SymbolInParent()
        {
            var parentTable = new SymbolTable();
            parentTable.Define("x", typeof(ILocalValue));
            var symbolTable = new SymbolTable(parentTable);
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
        public void Should_RetrieveParameter_When_SymbolInSelf()
        {
            var symbolTable = new SymbolTable();
            symbolTable.Define("x", typeof(ILocalValue));

            var result = symbolTable.TryResolve("x", out var parameter);

            Assert.That(result, Is.True);
            Assert.That(parameter, Is.Not.Null);
        }

        [Test]
        public void Should_RetrieveParameter_When_SymbolInParent()
        {
            var parentSymbolTable = new SymbolTable();
            parentSymbolTable.Define("x", typeof(ILocalValue));
            var symbolTable = new SymbolTable(parentSymbolTable);

            var result = symbolTable.TryResolve("x", out var parameter);

            Assert.That(result, Is.True);
            Assert.That(parameter, Is.Not.Null);
        }

        [Test]
        public void ShouldNot_RetrieveParameter_When_SymbolNotInSelf()
        {
            var symbolTable = new SymbolTable();
            symbolTable.Define("y", typeof(ILocalValue));

            var result = symbolTable.TryResolve("x", out var parameter);

            Assert.That(result, Is.False);
            Assert.That(parameter, Is.Null);
        }

        [Test]
        public void Should_ListSymbols_When_Enumerating()
        {
            var symbolTable = new SymbolTable();
            symbolTable.Define("x", typeof(ILocalValue));
            symbolTable.Define("y", typeof(ILocalValue));
            symbolTable.Define("z", typeof(ILocalValue));

            var symbols = symbolTable.ToArray();

            Assert.That(symbols, Has.Length.EqualTo(3));
        }
    }
}