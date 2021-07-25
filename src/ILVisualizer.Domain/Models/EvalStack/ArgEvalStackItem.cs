using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILVisualizer.Domain.Models.EvalStack
{
	public class ArgEvalStackItem : EvalStackItem
	{
		public short Index { get; }

		public ArgEvalStackItem(short index) => Index = index;

		public override bool Equals(object obj) =>
			obj is ArgEvalStackItem other && BaseEquals(other) && other.Index == Index;

		public override int GetHashCode() => base.GetHashCode();
	}
}
