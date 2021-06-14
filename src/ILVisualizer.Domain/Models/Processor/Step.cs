using ILVisualizer.Domain.Models.EvalStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ILVisualizer.Domain.Models.Processor
{
    [StructLayout(LayoutKind.Explicit)]
    public struct Step
    {
        [FieldOffset(0)]
        public byte ItemsPopped;

        [FieldOffset(1)]
        public bool HasMultiplePushed;

        // If only one item was pushed on:
        [FieldOffset(8)]
        public EvalStackItem SinglePushed;

        // If multiple items were pushed on:
        [FieldOffset(8)]
        public EvalStackItem[] MultiplePushed;

        public override bool Equals(object obj)
        {
            if (obj is Step step)
            {
                if (ItemsPopped != step.ItemsPopped) return false;
                if (HasMultiplePushed != step.HasMultiplePushed) return false;

                if (HasMultiplePushed && !MultiplePushed.SequenceEqual(step.MultiplePushed)) return false;
                if (!HasMultiplePushed && !SinglePushed.Equals(step.SinglePushed)) return false;

                return true;
            }

            return false;
        }

        // (Never used)
        public override int GetHashCode() => 0;

        public static bool operator ==(Step left, Step right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Step left, Step right)
        {
            return !(left == right);
        }
    }
}
