using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILVisualizer.Domain.Models.Processor
{
    /// <summary>
    /// Represents a statement break: Which is a point where you move onto the next statement
    /// (the evaluation stack has zero items).
    /// Shown as a "===" in the output.
    /// </summary>
    public struct StatementBreak
    {
        public int StepStartNo;
    }
}
