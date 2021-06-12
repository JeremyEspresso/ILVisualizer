using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILVisualizer.Domain.Models.EvalStack
{
    public class ConstantEvalStackItem : EvalStackItem
    {
        public long Value;

        public ConstantEvalStackItem(long value) => Value = value;
    }
}
