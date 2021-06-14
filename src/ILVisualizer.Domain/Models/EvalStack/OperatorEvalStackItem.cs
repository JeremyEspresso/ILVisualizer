using ILVisualizer.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILVisualizer.Domain.Models.EvalStack
{
    public class OperatorEvalStackItem : EvalStackItem
    {
        public EvalStackOperatorType Type { get; }
        public EvalStackItem Left { get; }
        public EvalStackItem Right { get; }

        public OperatorEvalStackItem(EvalStackOperatorType type, EvalStackItem left, EvalStackItem right) =>
            (Type, Left, Right) = (type, left, right);
    }
}
