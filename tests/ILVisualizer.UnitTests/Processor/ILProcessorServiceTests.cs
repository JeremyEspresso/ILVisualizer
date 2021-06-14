using ILVisualizer.Application.Common.Services;
using ILVisualizer.Domain.Enums;
using ILVisualizer.Domain.Models;
using ILVisualizer.Domain.Models.EvalStack;
using ILVisualizer.Domain.Models.Processor;
using ILVisualizer.UnitTests.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ILVisualizer.UnitTests.Processor
{
    public class ILProcessorServiceTests
    {
        [Fact]
        public void Process_SinglePush()
        {
            var service = new ILProcessorService();
            var result = service.Process(new List<ILInstruction>
            {
                new ILInstruction() 
                {
                    Type = ILInstructionType.Ldc_I4, 
                    IntArg = 13
                }
            });

            var expectedSteps = new Step[]
            {
                new Step() 
                {
                    ItemsPopped = 0,
                    HasMultiplePushed = false,
                    SinglePushed = new Int32ConstantEvalStackItem(13),
                }
            };

            CollectionAssert.Equal(expectedSteps, result.Steps);
        }

        [Fact]
        public void Process_MultipleInstructions_AllPushing()
        {
            var service = new ILProcessorService();
            var result = service.Process(new List<ILInstruction>
            {
                new ILInstruction()
                {
                    Type = ILInstructionType.Ldc_I4_2
                },
                new ILInstruction()
                {
                    Type = ILInstructionType.Ldc_I4_5                    
                }
            });

            var expectedSteps = new Step[]
            {
                new Step()
                {
                    ItemsPopped = 0,
                    HasMultiplePushed = false,
                    SinglePushed = new Int32ConstantEvalStackItem(2),
                },
                new Step()
                {
                    ItemsPopped = 0,
                    HasMultiplePushed = false,
                    SinglePushed = new Int32ConstantEvalStackItem(5),
                }
            };

            CollectionAssert.Equal(expectedSteps, result.Steps);
        }
    }
}
