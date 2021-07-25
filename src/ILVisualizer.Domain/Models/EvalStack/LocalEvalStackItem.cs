using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILVisualizer.Domain.Models.EvalStack
{
	public class LocalEvalStackItem : EvalStackItem
	{
		public short Index { get; }

		public LocalEvalStackItem(short index) => Index = index;

		public override bool Equals(object obj) =>
			obj is LocalEvalStackItem other && BaseEquals(other) && other.Index == Index;

		public override int GetHashCode() => base.GetHashCode();
	}
}
