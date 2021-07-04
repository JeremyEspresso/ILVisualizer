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
                new ParsedILInstruction() 
                {
                    Type = ILInstructionType.Ldc_I4, 
                    Arg = 13
                }
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
                new ParsedILInstruction()
                {
                    Type = ILInstructionType.Ldc_I4_2
                },

                // Action Instruction - Pop
                new ParsedILInstruction()
                {
                    Type = ILInstructionType.Ret
                }
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

        [Fact]
        public void Process_Folding_Int32WithInt32()
        {
            var service = new ILProcessorService();

            var result = service.Process(new List<ParsedILInstruction>
            {
                // Setup
                new ParsedILInstruction()
                {
                    Type = ILInstructionType.Ldc_I4_2
                },
                new ParsedILInstruction()
                {
                    Type = ILInstructionType.Ldc_I4_5
                },

                // The instruction
                new ParsedILInstruction()
                {
                    Type = ILInstructionType.Add
                }
            });

			var ldcI42 = new Int32ConstantEvalStackItem(2);
			var ldcI45 = new Int32ConstantEvalStackItem(5);

			var expectedSteps = new Block[]
			{
				new Block()
				{
					FirstActionInstructionPos = -1,
					Instructions = new Step[]
					{
						new Step(ILInstructionType.Ldc_I4_2, ldcI42, null, false),
						new Step(ILInstructionType.Ldc_I4_5, ldcI45, null, false),
						new Step(ILInstructionType.Add, new Int32ConstantEvalStackItem(7), 
							new EvalStackItem[]
							{
								ldcI42,
								ldcI45
							}, 
						false)
					}
				}
            };

            CollectionAssert.Equal(expectedSteps, result);
        }

        [Fact]
        public void Process_Folding_Int64WithInt32()
        {
            var service = new ILProcessorService();

            var result = service.Process(new List<ParsedILInstruction>
            {
                // Setup
                new ParsedILInstruction()
                {
                    Type = ILInstructionType.Ldc_I8,
                    Arg = 80000000000000
                },
                new ParsedILInstruction()
                {
                    Type = ILInstructionType.Ldc_I4_5
                },

                // The instruction
                new ParsedILInstruction()
                {
                    Type = ILInstructionType.Add
                }
            });

			var ldci8 = new Int64ConstantEvalStackItem(80000000000000);
			var ldci45 = new Int32ConstantEvalStackItem(5);

			var expectedSteps = new Block[]
            {
				new Block()
				{
					FirstActionInstructionPos = -1,
					Instructions = new Step[]
					{
						new Step(ILInstructionType.Ldc_I8, ldci8, null, false),
						new Step(ILInstructionType.Ldc_I4_5, ldci45, null, false),
						new Step(ILInstructionType.Add, new Int64ConstantEvalStackItem(80000000000005),
							new EvalStackItem[]
							{
								ldci8,
								ldci45
							},
						false)						
					}
				}
            };

            CollectionAssert.Equal(expectedSteps, result);
        }

        [Fact]
        public void Process_Folding_Int32WithInt64()
        {
            var service = new ILProcessorService();

            var result = service.Process(new List<ParsedILInstruction>
            {
                // Setup
                new ParsedILInstruction()
                {
                    Type = ILInstructionType.Ldc_I4_7
                },
                new ParsedILInstruction()
                {
                    Type = ILInstructionType.Ldc_I8,
                    Arg = 80000000000000
                },

                // The instruction
                new ParsedILInstruction()
                {
                    Type = ILInstructionType.Add
                }
            });

			var ldci47 = new Int32ConstantEvalStackItem(7);
			var ldci8 = new Int64ConstantEvalStackItem(80000000000000);

			var expectedSteps = new Block[]
            {
				new Block()
				{
					FirstActionInstructionPos = -1,
					Instructions = new Step[]
					{
						new Step(ILInstructionType.Ldc_I4_7, ldci47, null, false),
						new Step(ILInstructionType.Ldc_I8, ldci8, null, false),
						new Step(ILInstructionType.Add, new Int64ConstantEvalStackItem(80000000000007), new EvalStackItem[] { ldci47, ldci8 }, false)
					}
				}    
            };

            CollectionAssert.Equal(expectedSteps, result);
        }

        [Fact]
        public void Process_Folding_Int64WithInt64()
        {
            var service = new ILProcessorService();

            var result = service.Process(new List<ParsedILInstruction>
            {
                // Setup
                new ParsedILInstruction()
                {
                    Type = ILInstructionType.Ldc_I8,
                    Arg = 13
                },
                new ParsedILInstruction()
                {
                    Type = ILInstructionType.Ldc_I8,
                    Arg = 80000000000000
                },

                // The instruction
                new ParsedILInstruction()
                {
                    Type = ILInstructionType.Add
                }
            });

			var ldci8 = new Int64ConstantEvalStackItem(13);
			var ldci82 = new Int64ConstantEvalStackItem(80000000000000);

			var expectedSteps = new Block[]
			{
				new Block()
				{
					FirstActionInstructionPos = -1,
					Instructions = new Step[]
					{
						new Step(ILInstructionType.Ldc_I8, ldci8, null, false),
						new Step(ILInstructionType.Ldc_I8, ldci82, null, false),
						new Step(ILInstructionType.Add, new Int64ConstantEvalStackItem(80000000000013), new EvalStackItem[] { ldci8, ldci82 }, false)
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
				new ParsedILInstruction()
				{
					Type = ILInstructionType.Ret
				}
			}));
        }
    }
}
