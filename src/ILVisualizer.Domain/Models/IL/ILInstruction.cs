using ILVisualizer.Domain.Enums.IL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILVisualizer.Domain.Models.IL
{
    public struct ILInstruction
    {
        public ILInstructionType Type;
        public int IntArg;
    }
}
