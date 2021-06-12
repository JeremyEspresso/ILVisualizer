using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILVisualizer.Application.Common.Exceptions.Parser
{
    public class ParseFailedException : Exception
    {
        public ParseFailedException(string msg) : base(msg) { }
    }
}
