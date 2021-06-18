using ILVisualizer.Domain.Enums;
using ILVisualizer.Domain.Models.EvalStack;
using System.Linq;
using System.Runtime.InteropServices;

namespace ILVisualizer.Domain.Models.Processor
{
    public struct Step
    {
        public ILInstructionType Type;

        /// <summary>
		/// Whether this is an instruction that performs an action (has side effects beyond the eval stack).
		/// </summary>
        public bool IsActionInstruction;

        public EvalStackItem Pushed;

        // EvalStackItem[] when multiple are popped, EvalStackItem when one is popped.
        public object Popped;

        public override bool Equals(object obj)
        {
            if (obj is Step step)
            {
				if (Pushed == null)
					return step.Pushed == null;
				else
					if (!Pushed.Equals(step.Pushed)) return false;

				if (Popped == null)
					return step.Popped == null;
				else if (Popped is EvalStackItem singleItm)
					return singleItm.Equals(step.Popped);
				else if (Popped is EvalStackItem[] multiple)
					return step.Popped is EvalStackItem[] stepMultiple && multiple.SequenceEqual(stepMultiple);
            }

            return false;
        }

        // (Never used)
        public override int GetHashCode() => 0;

        public static bool operator ==(Step left, Step right) => left.Equals(right);

        public static bool operator !=(Step left, Step right) => !(left == right);
    }
}