namespace ILVisualizer.Application.Common.Exceptions.Processor
{
    public class InvalidPopException : InvalidILException
    {
        public InvalidPopException() 
            : base("The IL is invalid! It attempted to pop off an empty evaluation stack.") { }
    }
}
