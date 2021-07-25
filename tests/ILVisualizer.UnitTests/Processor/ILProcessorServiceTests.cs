using ILVisualizer.Application.Common.Exceptions.Processor;
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
            var result = service.Process(new List<ParsedILInstruction>
            {
                new ParsedILInstruction(ILInstructionType.Ldc_I4, 13)
            });

            var expected = new Block[]
            {
				new Block()
				{
					FirstActionInstructionPos = -1,
					Instructions = new Step[]
					{
						new Step(ILInstructionType.Ldc_I4, new Int32ConstantEvalStackItem(13), null, false)						
					}
				}
            };

            CollectionAssert.Equal(expected, result);
        }

        [Fact]
        public void Process_ActionInstruction()
        {
            var service = new ILProcessorService();

            var result = service.Process(new List<ParsedILInstruction>
            {
                // Push
                new ParsedILInstruction(ILInstructionType.Ldc_I4_2),

                // Action Instruction - Pop
                new ParsedILInstruction(ILInstructionType.Ret)
            });

			var ldci42 = new Int32ConstantEvalStackItem(2)
			{
				PoppedByActionStepsCounts = 1
			};

			var expectedSteps = new Block[]
            {
				new Block()
				{
					Instructions = new Step[]
					{
						new Step(ILInstructionType.Ldc_I4_2, ldci42, null, false),
						new Step(ILInstructionType.Ret, null, ldci42, true)
					},
					FirstActionInstructionPos = 1
				}
            };

            CollectionAssert.Equal(expectedSteps, result);
        }

		public enum ConstantType
		{
			Int32,
			Int64
		}

		[Theory]
		[InlineData(ConstantType.Int32, ConstantType.Int32, ILInstructionType.Add, 7)]
		[InlineData(ConstantType.Int64, ConstantType.Int32, ILInstructionType.Add, 80000000000002)]
		[InlineData(ConstantType.Int32, ConstantType.Int64, ILInstructionType.Add, 14L)]
		[InlineData(ConstantType.Int64, ConstantType.Int64, ILInstructionType.Add, 80000000000009)]
		[InlineData(ConstantType.Int32, ConstantType.Int32, ILInstructionType.Sub, 3)]
		[InlineData(ConstantType.Int64, ConstantType.Int32, ILInstructionType.Sub, 79999999999998)]
		[InlineData(ConstantType.Int32, ConstantType.Int64, ILInstructionType.Sub, -4L)]
		[InlineData(ConstantType.Int64, ConstantType.Int64, ILInstructionType.Sub, 79999999999991)]
		[InlineData(ConstantType.Int32, ConstantType.Int32, ILInstructionType.Mul, 10)]
		[InlineData(ConstantType.Int64, ConstantType.Int32, ILInstructionType.Mul, 160000000000000)]
		[InlineData(ConstantType.Int32, ConstantType.Int64, ILInstructionType.Mul, 45L)]
		[InlineData(ConstantType.Int64, ConstantType.Int64, ILInstructionType.Mul, 720000000000000)]
		[InlineData(ConstantType.Int32, ConstantType.Int32, ILInstructionType.Div, 2)]
		[InlineData(ConstantType.Int64, ConstantType.Int32, ILInstructionType.Div, 40000000000000)]
		[InlineData(ConstantType.Int32, ConstantType.Int64, ILInstructionType.Div, 0L)]
		[InlineData(ConstantType.Int64, ConstantType.Int64, ILInstructionType.Div, 8888888888888)]
		public void Process_Operation_ConstantWithConstant(
			ConstantType leftType, ConstantType rightType, ILInstructionType operation, object expectedResult)
        {
            var service = new ILProcessorService();

			(ParsedILInstruction leftInstruction, EvalStackItem leftStackItem) = leftType switch
			{
				ConstantType.Int32 => (new ParsedILInstruction(ILInstructionType.Ldc_I4_5), (EvalStackItem)new Int32ConstantEvalStackItem(5)),
				ConstantType.Int64 => (new ParsedILInstruction(ILInstructionType.Ldc_I8, 80000000000000), new Int64ConstantEvalStackItem(80000000000000)),
				_ => throw new Exception()
			};

			(ParsedILInstruction rightInstruction, EvalStackItem rightStackItem) = rightType switch
			{
				ConstantType.Int32 => (new ParsedILInstruction(ILInstructionType.Ldc_I4_2), (EvalStackItem)new Int32ConstantEvalStackItem(2)),
				ConstantType.Int64 => (new ParsedILInstruction(ILInstructionType.Ldc_I8, 9), new Int64ConstantEvalStackItem(9)),
				_ => throw new Exception()
			};

			var expectedResultItem = expectedResult switch
			{
				int i => (EvalStackItem)new Int32ConstantEvalStackItem(i),
				long l => new Int64ConstantEvalStackItem(l),
				_ => throw new Exception()
			};

			var result = service.Process(new List<ParsedILInstruction>
            {
                // Setup
                leftInstruction,
                rightInstruction,

                // The operation
                new ParsedILInstruction(operation)
            });

			var expectedSteps = new Block[]
			{
				new Block()
				{
					FirstActionInstructionPos = -1,
					Instructions = new Step[]
					{
						new Step(leftInstruction.Type, leftStackItem, null, false),
						new Step(rightInstruction.Type, rightStackItem, null, false),
						new Step(operation, expectedResultItem, 
							new EvalStackItem[]
							{
								leftStackItem,
								rightStackItem
							}, 
						false)
					}
				}
            };

            CollectionAssert.Equal(expectedSteps, result);
        }

        [Fact]
        public void Process_InvalidPop()
        {
            var service = new ILProcessorService();
			_ = Assert.Throws<InvalidPopException>(() => service.Process(new List<ParsedILInstruction>()
			{
				new ParsedILInstruction(ILInstructionType.Ret)
			}));
        }
    }
}
