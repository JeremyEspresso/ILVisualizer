using System;

namespace ILVisualizer.Application.Common.Exceptions.Parser
{
    public class ParseFailedException : Exception
    {
        public ParseFailedException(string msg) : base(msg) { }
    }
}
