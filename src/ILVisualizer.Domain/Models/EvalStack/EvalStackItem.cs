using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILVisualizer.Domain.Models.EvalStack
{
    public abstract class EvalStackItem
    {
        /// <summary>
        /// The step this item was pushed on
        /// </summary>
        public int PushedStepNo;

        /// <summary>
        /// The step this item was popped off
        /// </summary>
        public int PoppedStepNo;
    }
}
