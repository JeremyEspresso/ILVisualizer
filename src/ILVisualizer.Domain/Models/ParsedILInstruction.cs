using ILVisualizer.Domain.Enums;

namespace ILVisualizer.Domain.Models
{
    public struct ParsedILInstruction
    {
        public ILInstructionType Type;
        public long Arg;

		public ParsedILInstruction(ILInstructionType type, long arg = 0)
		{
			Type = type;
			Arg = arg;
		}
	}
}
