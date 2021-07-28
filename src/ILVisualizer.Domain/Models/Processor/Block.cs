using System.Linq;

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
		/// (-1 represents none)
		/// </summary>
		public int FirstActionInstructionPos;

		public override bool Equals(object obj) => obj is Block blk
				&& FirstActionInstructionPos == blk.FirstActionInstructionPos
				&& Instructions.SequenceEqual(blk.Instructions);

		public static bool operator ==(Block left, Block right) => left.Equals(right);
		public static bool operator !=(Block left, Block right) => !(left == right);

		public override int GetHashCode() => base.GetHashCode();
	}
}
