using ILVisualizer.Application.Common.Exceptions.Parser;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILVisualizer.Application.Common.Entities.Parser
{
    /// <summary>
    /// Provides helpers to parse a string.
    /// </summary>
    public class Parser
    {
        protected string _source;
        protected int _currentPos;
        private int _currentLineEnd;

        public void Initialize(string source)
        {
            _source = source;
        }

        // Returns: Whether this is the last line
        public bool TryMoveToNextLine()
        {
            // We should be at the end of the line when calling this.
            Debug.Assert(_source[_currentPos] == '\r' || _source[_currentPos] == '\n');

            _currentPos += _source[_currentPos] == '\r' ? 2 : 1;
            _currentLineEnd = _source.IndexOf('\n', _currentPos);

            // If there is no more "\n", this is the last line.
            bool isLastLine = _currentLineEnd == -1;
            if (isLastLine) _currentLineEnd = _source.Length;

            // Put the line end on the '\r' if there is one.
            int oneBefore = _currentLineEnd - 1;
            if (_source[oneBefore] == '\r') _currentLineEnd = oneBefore;

            _currentLineEnd--;
            return isLastLine;
        }

        public string ReadToLineEnd()
        {
            return _source.Substring(_currentPos, _currentLineEnd - _currentPos);
        }

        public string ReadToLineEndOr(char ch)
        {
            int end = _source.IndexOf(ch, _currentPos, _currentLineEnd - _currentPos);
            return _source.Substring(_currentPos, end == -1 ? _currentLineEnd : end);
        }

        public string ReadTo(char ch)
        {
            int end = _source.IndexOf(ch);

            if (end == -1)
                throw new ParseFailedException("Unexpected end-of-text");
            if (end > _currentLineEnd)
                throw new ParseFailedException($"Unexpected end-of-line at position {end}");

            return _source.Substring(_currentPos, end);
        }
    }
}
