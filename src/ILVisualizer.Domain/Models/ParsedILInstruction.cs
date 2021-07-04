using ILVisualizer.Domain.Enums;

namespace ILVisualizer.Domain.Models
{
    public struct ParsedILInstruction
    {
        public ILInstructionType Type;
        public long Arg;
    }
}
