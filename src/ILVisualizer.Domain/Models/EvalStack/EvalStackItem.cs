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

        protected bool BaseEquals(EvalStackItem obj) => 
            PushedStepNo == obj.PushedStepNo && PoppedStepNo == obj.PoppedStepNo;
    }
}
