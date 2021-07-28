using System;

namespace ILVisualizer.Application.Common.Exceptions.Processor
{
    public abstract class InvalidILException : Exception
    {
        public InvalidILException(string message) : base(message) { }
    }
}
