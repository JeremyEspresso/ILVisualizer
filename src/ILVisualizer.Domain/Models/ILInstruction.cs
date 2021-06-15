using ILVisualizer.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ILVisualizer.Domain.Models
{
    public struct ILInstruction
    {
        public ILInstructionType Type;
        public int IntArg;
        public long LongArg;
    }
}
