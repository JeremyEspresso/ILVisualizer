using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILVisualizer.Domain.Models.EvalStack
{
    public class Int64ConstantEvalStackItem : EvalStackItem
    {
        public long Constant { get; }

        public Int64ConstantEvalStackItem(long constant) => Constant = constant;
    }
}
