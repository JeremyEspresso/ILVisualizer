using ILVisualizer.Application.Common.Entities.Parser;
using ILVisualizer.Application.Common.Exceptions.Parser;
using Xunit;

namespace ILVisualizer.UnitTests.Parsing
{
    public class ParserTests
    {
        [Fact]
        public void Initialize()
        {
            var parser = new TestParser();
            parser.Initialize("abc\r\ndef");

            parser.AssertValues(0, 3);
        }

        [Fact]
        public void MoveToNextLine_LastLine()
        {
            var parser = new TestParser();
            parser.Initialize("abc");
            parser.JumpTo(3);
            
            Assert.True(parser.TryMoveToNextLine());
        }

        [Fact]
        public void MoveToNextLine_NotLastLine_OntoNotLastLine_NoR()
        {
            var parser = new TestParser();
            parser.Initialize("abc\ndef\nghi");
            parser.JumpTo(3);

            Assert.False(parser.TryMoveToNextLine());
            parser.AssertValues(4, 7);
        }

        [Fact]
        public void MoveToNextLine_NotLastLine_OntoLastLine_NoR()
        {
            var parser = new TestParser();
            parser.Initialize("abc\ndef");
            parser.JumpTo(3);

            Assert.False(parser.TryMoveToNextLine());
            parser.AssertValues(4, 7);
        }

        [Fact]
        public void MoveToNextLine_NotLastLine_OntoNotLastLine_WithR()
        {
            var parser = new TestParser();
            parser.Initialize("abc\r\ndef\r\nghi");
            parser.JumpTo(3);

            Assert.False(parser.TryMoveToNextLine());
            parser.AssertValues(5, 8);
        }

        [Fact]
        public void MoveToNextLine_NotLastLine_OntoLastLine_WithR()
        {
            var parser = new TestParser();
            parser.Initialize("abc\r\ndef");
            parser.JumpTo(3);

            Assert.False(parser.TryMoveToNextLine());
            parser.AssertValues(5, 8);
        }

        [Fact]
        public void ReadToLineEnd()
        {
            var parser = new TestParser();
            parser.Initialize("abc\r\ndef");

            Assert.Equal("abc", parser.ReadToLineEnd());
            parser.AssertValues(3, 3);
        }

        [Fact]
        public void ReadToLineEndOrToChar_ToLineEnd()
        {
            var parser = new TestParser();
            parser.Initialize("abc\r\nd e");

            Assert.Equal("abc", parser.ReadToLineEndOrToChar(' '));
            parser.AssertValues(3, 3);
        }

        [Fact]
        public void ReadToLineEndOrToChar_ToProvidedChar()
        {
            var parser = new TestParser();
            parser.Initialize("ab c\r\nd e");

            Assert.Equal("ab", parser.ReadToLineEndOrToChar(' '));
            parser.AssertValues(2, 4);
        }

        [Fact]
        public void ReadTo_NextLine()
        {
            var parser = new TestParser();
            parser.Initialize("ab c\r\nd .e");

            Assert.Throws<ParseFailedException>(() => parser.ReadTo('.'));
        }

        [Fact]
        public void ReadTo_EndOfText()
        {
            var parser = new TestParser();
            parser.Initialize("ab c\r\nd e");

            Assert.Throws<ParseFailedException>(() => parser.ReadTo('.'));
        }

        [Fact]
        public void ReadTo_Valid()
        {
            var parser = new TestParser();
            parser.Initialize("ab.c\r\nd e");

            Assert.Equal("ab", parser.ReadTo('.'));
        }
    }

    class TestParser : Parser
    {
        public void AssertValues(int currentPos, int currentLineEnd)
        {
            Assert.Equal(currentPos, _currentPos);
            Assert.Equal(currentLineEnd, _currentLineEnd);
        }

        public void JumpTo(int pos) => _currentPos = pos;
    }
}
