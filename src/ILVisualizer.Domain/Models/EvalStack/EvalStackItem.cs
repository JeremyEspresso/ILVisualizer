using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILVisualizer.Domain.Models.EvalStack
{
    public abstract class EvalStackItem
    {
        public EvalStackItemType ItemType;
        public EvalStackOperationType Type;

        /// <summary>
        /// The step this item was first pushed at
        /// </summary>
        public int PushedStepNo;
    }
}
