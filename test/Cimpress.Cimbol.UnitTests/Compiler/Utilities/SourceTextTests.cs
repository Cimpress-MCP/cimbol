using System;
using Cimpress.Cimbol.Compiler.Utilities;
using NUnit.Framework;

namespace Cimpress.Cimbol.UnitTests.Compiler.Utilities
{
    [TestFixture]
    public class SourceTextTests
    {
        [Test]
        public void Should_Initialize_With_ValidSource()
        {
            Assert.DoesNotThrow(() => new SourceText("test"));
        }

        [Test]
        public void Should_Initialize_With_EmptySource()
        {
            Assert.DoesNotThrow(() => new SourceText(string.Empty));
        }

        [Test]
        public void Should_ThrowException_With_NullSource()
        {
            Assert.Throws<ArgumentNullException>(() => new SourceText(null));
        }

        [Test]
        public void Should_TrackPosition_With_AsciiSource()
        {
            var source = "abc";
            var sourceText = new SourceText(source);

            Assert.AreEqual("a", sourceText.Peek());
            Assert.AreEqual(0, sourceText.Column);
            Assert.AreEqual(0, sourceText.Row);

            Assert.IsTrue(sourceText.Next());
            Assert.AreEqual("b", sourceText.Peek());
            Assert.AreEqual(1, sourceText.Column);
            Assert.AreEqual(0, sourceText.Row);

            Assert.IsTrue(sourceText.Next());
            Assert.AreEqual("c", sourceText.Peek());
            Assert.AreEqual(2, sourceText.Column);
            Assert.AreEqual(0, sourceText.Row);
            Assert.IsFalse(sourceText.EndOfFile);

            Assert.IsFalse(sourceText.Next());
            Assert.AreEqual(string.Empty, sourceText.Peek());
            Assert.AreEqual(3, sourceText.Column);
            Assert.AreEqual(0, sourceText.Row);
            Assert.IsTrue(sourceText.EndOfFile);
        }

        [Test]
        public void Should_TrackPosition_With_UnicodeSource()
        {
            var source = "👻👽🤖";
            var sourceText = new SourceText(source);

            Assert.AreEqual("\uD83D\uDC7B", sourceText.Peek());
            Assert.AreEqual(0, sourceText.Column);
            Assert.AreEqual(0, sourceText.Row);

            Assert.IsTrue(sourceText.Next());
            Assert.AreEqual("\uD83D\uDC7D", sourceText.Peek());
            Assert.AreEqual(1, sourceText.Column);
            Assert.AreEqual(0, sourceText.Row);

            Assert.IsTrue(sourceText.Next());
            Assert.AreEqual("\uD83E\uDD16", sourceText.Peek());
            Assert.AreEqual(2, sourceText.Column);
            Assert.AreEqual(0, sourceText.Row);
            Assert.IsFalse(sourceText.EndOfFile);

            Assert.IsFalse(sourceText.Next());
            Assert.AreEqual(string.Empty, sourceText.Peek());
            Assert.AreEqual(3, sourceText.Column);
            Assert.AreEqual(0, sourceText.Row);
            Assert.IsTrue(sourceText.EndOfFile);
        }

        [Test]
        public void Should_TrackPosition_With_EmptySource()
        {
            var source = string.Empty;
            var sourceText = new SourceText(source);

            Assert.AreEqual(string.Empty, sourceText.Peek());
            Assert.AreEqual(0, sourceText.Column);
            Assert.AreEqual(0, sourceText.Row);

            Assert.IsFalse(sourceText.Next());
            Assert.AreEqual(string.Empty, sourceText.Peek());
            Assert.AreEqual(0, sourceText.Column);
            Assert.AreEqual(0, sourceText.Row);
        }

        [Test]
        public void Should_ThrowException_When_HighSurrogateAtEndOfSource()
        {
            var source = "\uD83E";
            var sourceText = new SourceText(source);
            Assert.Throws<NotSupportedException>(() => sourceText.Peek());
        }

        [Test]
        public void Should_ThrowException_When_HighSurrogateAtStartOfSource()
        {
            var source = "\uD83Ea";
            var sourceText = new SourceText(source);
            Assert.Throws<NotSupportedException>(() => sourceText.Peek());
        }

        [Test]
        public void Should_ThrowException_When_LowSurrogateAtEndOfSource()
        {
            var source = "\uDD16";
            var sourceText = new SourceText(source);
            Assert.Throws<NotSupportedException>(() => sourceText.Peek());
        }

        [Test]
        public void Should_ThrowException_When_LowSurrogateAtStartOfSource()
        {
            var source = "\uDD16a";
            var sourceText = new SourceText(source);
            Assert.Throws<NotSupportedException>(() => sourceText.Peek());
        }

        [Test]
        public void Should_ThrowException_When_HighAndLowSurrogateSwappedInSource()
        {
            var source = "\uDD16\uD83E";
            var sourceText = new SourceText(source);
            Assert.Throws<NotSupportedException>(() => sourceText.Peek());
        }
    }
}