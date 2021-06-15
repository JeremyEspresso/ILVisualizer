using ILVisualizer.Domain.Enums;

namespace ILVisualizer.Domain.Models
{
    public struct ILInstruction
    {
        public ILInstructionType Type;
        public int IntArg;
        public long LongArg;
    }
}
