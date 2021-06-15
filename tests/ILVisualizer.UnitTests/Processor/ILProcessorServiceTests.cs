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
            CollectionAssert.Equal(new StatementBreak[] { new StatementBreak(0) }, result.Breaks);
        }

        [Fact]
        public void Process_PushThenSinglePop()
        {
            var service = new ILProcessorService();

            var result = service.Process(new List<ILInstruction>
            {
                // Push
                new ILInstruction()
                {
                    Type = ILInstructionType.Ldc_I4_2
                },

                // Pop
                new ILInstruction()
                {
                    Type = ILInstructionType.Ret
                }
            });

            var expectedSteps = new Step[]
            {
                new Step()
                {
                    ItemsPopped = 0,
                    HasMultiplePushed = false,
                    SinglePushed = new Int32ConstantEvalStackItem(2)
                    {
                        PoppedStepNo = 1
                    },
                },
                new Step()
                {
                    ItemsPopped = 1,
                    HasMultiplePushed = false
                }
            };

            CollectionAssert.Equal(expectedSteps, result.Steps);
            CollectionAssert.Equal(new StatementBreak[] { new StatementBreak(0) }, result.Breaks);
        }

        [Fact]
        public void Process_Folding_Int32WithInt32()
        {
            var service = new ILProcessorService();

            var result = service.Process(new List<ILInstruction>
            {
                // Setup
                new ILInstruction()
                {
                    Type = ILInstructionType.Ldc_I4_2
                },
                new ILInstruction()
                {
                    Type = ILInstructionType.Ldc_I4_5
                },

                // The instruction
                new ILInstruction()
                {
                    Type = ILInstructionType.Add
                }
            });

            var expectedSteps = new Step[]
            {
                new Step()
                {
                    ItemsPopped = 0,
                    HasMultiplePushed = false,
                    SinglePushed = new Int32ConstantEvalStackItem(2)
                    {
                        PoppedStepNo = 2
                    },
                },
                new Step()
                {
                    ItemsPopped = 0,
                    HasMultiplePushed = false,
                    SinglePushed = new Int32ConstantEvalStackItem(5)
                    {
                        PoppedStepNo = 2
                    },
                },
                new Step()
                {
                    ItemsPopped = 2,
                    HasMultiplePushed = false,
                    SinglePushed = new Int32ConstantEvalStackItem(7)
                }
            };

            CollectionAssert.Equal(expectedSteps, result.Steps);
            CollectionAssert.Equal(new StatementBreak[] { new StatementBreak(0) }, result.Breaks);
        }

        [Fact]
        public void Process_Folding_Int64WithInt32()
        {
            var service = new ILProcessorService();

            var result = service.Process(new List<ILInstruction>
            {
                // Setup
                new ILInstruction()
                {
                    Type = ILInstructionType.Ldc_I8,
                    LongArg = 80000000000000
                },
                new ILInstruction()
                {
                    Type = ILInstructionType.Ldc_I4_5
                },

                // The instruction
                new ILInstruction()
                {
                    Type = ILInstructionType.Add
                }
            });

            var expectedSteps = new Step[]
            {
                new Step()
                {
                    ItemsPopped = 0,
                    HasMultiplePushed = false,
                    SinglePushed = new Int64ConstantEvalStackItem(80000000000000)
                    {
                        PoppedStepNo = 2
                    },
                },
                new Step()
                {
                    ItemsPopped = 0,
                    HasMultiplePushed = false,
                    SinglePushed = new Int32ConstantEvalStackItem(5)
                    {
                        PoppedStepNo = 2
                    },
                },
                new Step()
                {
                    ItemsPopped = 2,
                    HasMultiplePushed = false,
                    SinglePushed = new Int64ConstantEvalStackItem(80000000000005)
                }
            };

            CollectionAssert.Equal(expectedSteps, result.Steps);
            CollectionAssert.Equal(new StatementBreak[] { new StatementBreak(0) }, result.Breaks);
        }

        [Fact]
        public void Process_Folding_Int32WithInt64()
        {
            var service = new ILProcessorService();

            var result = service.Process(new List<ILInstruction>
            {
                // Setup
                new ILInstruction()
                {
                    Type = ILInstructionType.Ldc_I4_7
                },
                new ILInstruction()
                {
                    Type = ILInstructionType.Ldc_I8,
                    LongArg = 80000000000000
                },

                // The instruction
                new ILInstruction()
                {
                    Type = ILInstructionType.Add
                }
            });

            var expectedSteps = new Step[]
            {
                new Step()
                {
                    ItemsPopped = 0,
                    HasMultiplePushed = false,
                    SinglePushed = new Int32ConstantEvalStackItem(7)
                    {
                        PoppedStepNo = 2
                    },
                },
                new Step()
                {
                    ItemsPopped = 0,
                    HasMultiplePushed = false,
                    SinglePushed = new Int64ConstantEvalStackItem(80000000000000)
                    {
                        PoppedStepNo = 2
                    },
                },
                new Step()
                {
                    ItemsPopped = 2,
                    HasMultiplePushed = false,
                    SinglePushed = new Int64ConstantEvalStackItem(80000000000007)
                }
            };

            CollectionAssert.Equal(expectedSteps, result.Steps);
            CollectionAssert.Equal(new StatementBreak[] { new StatementBreak(0) }, result.Breaks);
        }

        [Fact]
        public void Process_Folding_Int64WithInt64()
        {
            var service = new ILProcessorService();

            var result = service.Process(new List<ILInstruction>
            {
                // Setup
                new ILInstruction()
                {
                    Type = ILInstructionType.Ldc_I8,
                    LongArg = 13
                },
                new ILInstruction()
                {
                    Type = ILInstructionType.Ldc_I8,
                    LongArg = 80000000000000
                },

                // The instruction
                new ILInstruction()
                {
                    Type = ILInstructionType.Add
                }
            });

            var expectedSteps = new Step[]
            {
                new Step()
                {
                    ItemsPopped = 0,
                    HasMultiplePushed = false,
                    SinglePushed = new Int64ConstantEvalStackItem(13)
                    {
                        PoppedStepNo = 2
                    },
                },
                new Step()
                {
                    ItemsPopped = 0,
                    HasMultiplePushed = false,
                    SinglePushed = new Int64ConstantEvalStackItem(80000000000000)
                    {
                        PoppedStepNo = 2
                    },
                },
                new Step()
                {
                    ItemsPopped = 2,
                    HasMultiplePushed = false,
                    SinglePushed = new Int64ConstantEvalStackItem(80000000000013)
                }
            };

            CollectionAssert.Equal(expectedSteps, result.Steps);
            CollectionAssert.Equal(new StatementBreak[] { new StatementBreak(0) }, result.Breaks);
        }

        [Fact]
        public void Process_InvalidPop()
        {
            var service = new ILProcessorService();
            Assert.Throws<InvalidPopException>(() => service.Process(new List<ILInstruction>()
            {
                new ILInstruction()
                {
                    Type = ILInstructionType.Ret
                }
            }));
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
            CollectionAssert.Equal(new StatementBreak[] { new StatementBreak(0) }, result.Breaks);
        }
    }
}
