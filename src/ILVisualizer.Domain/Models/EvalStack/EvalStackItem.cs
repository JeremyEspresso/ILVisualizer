namespace ILVisualizer.Domain.Models.EvalStack
{
    public abstract class EvalStackItem
    {
        /// <summary>
		/// How many action instructions pop this off the stack.
		/// </summary>
        public int PoppedByActionStepsCounts;

		protected bool BaseEquals(EvalStackItem obj) =>
			PoppedByActionStepsCounts == obj.PoppedByActionStepsCounts;
    }
}
