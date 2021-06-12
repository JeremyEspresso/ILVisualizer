using ILVisualizer.Domain.Models.IL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILVisualizer.Application.Common.Interfaces
{
    public interface IILParserService
    {
        public IList<ILInstruction> Parse(string str);
    }
}
