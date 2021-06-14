using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILVisualizer.Domain.Models.EvalStack
{
    public class Int64ConstantEvalStackItem : EvalStackItem
    {
        public long Value { get; }

        public Int64ConstantEvalStackItem(long value) => Value = value;

        public override bool Equals(object obj) =>
            obj is Int64ConstantEvalStackItem other && BaseEquals(other) && other.Value == Value;

        public override int GetHashCode() => base.GetHashCode();        
    }
}
