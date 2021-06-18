using System.Collections.Generic;

namespace ILVisualizer.Domain.Models.Processor
{
    public struct ProcessorResult
    {
        public IList<Step> Steps;
        public IList<Block> Breaks;
    }
}
