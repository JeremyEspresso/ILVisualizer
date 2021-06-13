using ILVisualizer.Application.Common.Exceptions.Parser;
using System.Diagnostics;

namespace ILVisualizer.Application.Common.Entities.Parser
{
    /// <summary>
    /// Provides helpers to parse a string.
    /// </summary>
    public class Parser
    {
        protected string _source;
        protected int _currentPos;
        protected int _currentLineEnd;

        public void Initialize(string source)
        {
            _source = source;
            TryUpdateLineEnd();
        }

        const char CarriageReturn = '\r';
        const char NewLine = '\n';

        public bool TryMoveToNextLine()
        {
            if (_currentPos == _source.Length) return true;

            char currentChar = _source[_currentPos];

            // We should be at the end of the line when calling this.
            Debug.Assert(currentChar is CarriageReturn or NewLine);

            _currentPos += currentChar == CarriageReturn ? 2 : 1;
            if (!TryUpdateLineEnd()) return false;

            return false;
        }

        private bool TryUpdateLineEnd()
        {
            // Try to get the next '\n'
            _currentLineEnd = _source.IndexOf(NewLine, _currentPos);
            if (_currentLineEnd == -1)
            {
                _currentLineEnd = _source.Length;
                return false;
            }

            // Ensure we're behind the '\r'
            int oneBefore = _currentLineEnd - 1;
            if (_source[oneBefore] == CarriageReturn) _currentLineEnd = oneBefore;

            return true;
        }

        public string ReadToLineEnd()
        {
            string res = _source[_currentPos.._currentLineEnd];
            _currentPos = _currentLineEnd;
            return res;
        }

        public string ReadToLineEndOrToChar(char ch)
        {
            int end = _source.IndexOf(ch, _currentPos, _currentLineEnd - _currentPos);
            if (end == -1) end = _currentLineEnd;

            string res = _source[_currentPos..end];

            _currentPos = end;
            return res;
        }

        public string ReadTo(char ch)
        {
            int end = _source.IndexOf(ch);

            if (end > _currentLineEnd)
                throw new ParseFailedException($"Unexpected end-of-line at position {end}");
            if (end == -1)
                throw new ParseFailedException($"Expected {ch}");

            return _source.Substring(_currentPos, end);
        }
    }
}
