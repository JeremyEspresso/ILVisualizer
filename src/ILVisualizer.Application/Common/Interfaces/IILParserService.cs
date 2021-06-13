using ILVisualizer.Domain.Models;
using System.Collections.Generic;

namespace ILVisualizer.Application.Common.Interfaces
{
    public interface IILParserService
    {
        public IList<ILInstruction> Parse(string str);
    }
}
