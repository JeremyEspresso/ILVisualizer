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

        public override bool Equals(object obj) => 
            obj is Int32ConstantEvalStackItem other && BaseEquals(other) && other.Value == Value;

        public override int GetHashCode() => base.GetHashCode();
    }
}
