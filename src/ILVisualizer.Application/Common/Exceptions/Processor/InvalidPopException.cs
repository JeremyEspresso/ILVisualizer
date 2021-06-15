using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILVisualizer.Application.Common.Exceptions.Processor
{
    public class InvalidPopException : Exception
    {
        public InvalidPopException() 
            : base("The IL is invalid! It attempted to pop off an empty evaluation stack.") { }
    }
}
