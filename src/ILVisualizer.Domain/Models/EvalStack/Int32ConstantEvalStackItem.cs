using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILVisualizer.Domain.Models.EvalStack
{
    public class Int32ConstantEvalStackItem : EvalStackItem
    {
        public int Value { get; }

        public Int32ConstantEvalStackItem(int value) => Value = value;
    }
}
