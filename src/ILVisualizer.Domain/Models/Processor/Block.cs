namespace ILVisualizer.Domain.Models.Processor
{
    /// <summary>
    /// Represents a block: The output is organised into these, and a new block is made each time the
	/// evaluation stack has zero items.
    /// </summary>
    public struct Block
    {
        /// <summary>
		/// The instructions within this block and the steps they perform.
		/// </summary>
        public Step[] Instructions;

		/// <summary>
		/// The index of the first "action instruction" present.
		/// </summary>
		public int FirstActionInstructionPos;
    }
}
