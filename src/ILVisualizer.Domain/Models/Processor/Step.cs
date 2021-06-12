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
    }
}
