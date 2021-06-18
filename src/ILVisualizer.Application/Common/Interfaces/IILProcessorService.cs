using ILVisualizer.Domain.Models;
using ILVisualizer.Domain.Models.Processor;
using System.Collections.Generic;

namespace ILVisualizer.Application.Common.Interfaces
{
	public interface IILProcessorService
	{
		ProcessorResult Process(IList<ParsedILInstruction> instructions);
	}
}