using System;
using ILVisualizer.Application.Common.Exceptions.Processor;
using ILVisualizer.Application.Common.Interfaces;
using ILVisualizer.Domain.Enums;
using ILVisualizer.Domain.Models;
using ILVisualizer.Domain.Models.EvalStack;
using ILVisualizer.Domain.Models.Processor;
using System.Collections.Generic;

namespace ILVisualizer.Application.Common.Services
{
    public class ILProcessorService : IILProcessorService
	{
		public Block CurrentBlock;
		public List<Block> Result = new(8);

		public List<Step> CurrentSteps = new(8);
        public Step CurrentStep = new();
        public Stack<EvalStackItem> CurrentEvalStack = new();

        public IList<Block> Process(IList<ParsedILInstruction> instructions)
        {
            Initialize();

            for (int i = 0; i < instructions.Count; i++)
            {
                CurrentStep = new Step();
                ProcessInstruction(instructions[i]);

                CurrentSteps.Add(CurrentStep);

				if (CurrentEvalStack.Count == 0)
					FinishCurrentBlock();
			}

			// End the current block if there is one.
			if (CurrentSteps.Count > 0) FinishCurrentBlock();

			return Result;
        }

		void Initialize()
		{
			CurrentSteps = new List<Step>();
			SetupNewBlock();
		}

		void ProcessInstruction(ParsedILInstruction instruction)
        {
			CurrentStep.InstructionType = instruction.Type;

            switch (instruction.Type)
            {
                case ILInstructionType.Ldc_I4_M1:
                case ILInstructionType.Ldc_I4_0:
                case ILInstructionType.Ldc_I4_1:
                case ILInstructionType.Ldc_I4_2:
                case ILInstructionType.Ldc_I4_3:
                case ILInstructionType.Ldc_I4_4:
                case ILInstructionType.Ldc_I4_5:
                case ILInstructionType.Ldc_I4_6:
                case ILInstructionType.Ldc_I4_7:
                case ILInstructionType.Ldc_I4_8:
                    PushOne(new Int32ConstantEvalStackItem((int)instruction.Type));
                    break;
                case ILInstructionType.Ldc_I4_S:
                case ILInstructionType.Ldc_I4:
                    PushOne(new Int32ConstantEvalStackItem((int)instruction.Arg));
                    break;
                case ILInstructionType.Ldc_I8:
                    PushOne(new Int64ConstantEvalStackItem(instruction.Arg));
                    break;
                case ILInstructionType.Add:
                case ILInstructionType.Sub:
                case ILInstructionType.Mul:
                case ILInstructionType.Div:
                case ILInstructionType.Rem:
                    PerformOperation((EvalStackOperatorType)(instruction.Type - ILInstructionType.Add));
                    break;
                case ILInstructionType.Ret:
					MarkAsActionInstruction();
					_ = Pop();
                    break;
            }
        }

		void SetupNewBlock() => CurrentBlock = new Block() { FirstActionInstructionPos = -1 };

        void FinishCurrentBlock()
        {
			CurrentBlock.Instructions = CurrentSteps.ToArray();
			CurrentSteps.Clear();
			Result.Add(CurrentBlock);
			SetupNewBlock();
        }

		void MarkAsActionInstruction()
		{
			CurrentStep.IsActionInstruction = true;
			if (CurrentBlock.FirstActionInstructionPos == -1) 
				CurrentBlock.FirstActionInstructionPos = CurrentSteps.Count;
		}

		EvalStackItem Pop()
        {
            if (!CurrentEvalStack.TryPop(out var item)) throw new InvalidPopException();
			CurrentStep.Popped = item;

			if (CurrentStep.IsActionInstruction) item.PoppedByActionStepsCounts++;
            return item;
        }

        EvalStackItem[] PopMany(int count)
        {
            var popped = new EvalStackItem[count];

			for (int i = popped.Length - 1; i >= 0; i--)
			{
                if (!CurrentEvalStack.TryPop(out var item)) throw new InvalidPopException();
                popped[i] = item;
            }

			CurrentStep.Popped = popped;
			return popped;
        }

        void PushOne(EvalStackItem item)
        {
            CurrentEvalStack.Push(item);
            CurrentStep.Pushed = item;
        }

        enum FoldMode
        {
            None,
            Int32,
            Int64
        }

        void PerformOperation(EvalStackOperatorType opType)
        {
            var parts = PopMany(2);
            var first = parts[0];
            var second = parts[1];

            // Try to do any constant folding if both the first and second are constants.
            // (e.g. 3 + 4 can become 7)
            var mode = GetFoldMode();

            if (mode == FoldMode.Int32)
            {
                var firstConstant = GetConstantValueInt(first);
                var secondConstant = GetConstantValueInt(second);

                PushOne(Fold32(firstConstant, secondConstant));
            }
            else if (mode == FoldMode.Int64)
            {
                var firstConstant = GetConstantValueLong(first);
                var secondConstant = GetConstantValueLong(second);

                PushOne(Fold64(firstConstant, secondConstant));
            }
            else
            {
                PushOne(new OperatorEvalStackItem(opType, first, second));
            }

            FoldMode GetFoldMode()
            {
                if (first is Int32ConstantEvalStackItem first32)
                {
                    if (second is Int32ConstantEvalStackItem)
                        return FoldMode.Int32;
                    else if (second is Int64ConstantEvalStackItem)
                        return FoldMode.Int64;
                }
                else if (first is Int64ConstantEvalStackItem first64)
                {
                    if (second is Int32ConstantEvalStackItem or Int64ConstantEvalStackItem)
                        return FoldMode.Int64;
                }

                return FoldMode.None;
            }

            Int32ConstantEvalStackItem Fold32(int a, int b)
            {
                int folded = opType switch
                {
                    EvalStackOperatorType.Add => a + b,
                    EvalStackOperatorType.Subtract => a - b,
                    EvalStackOperatorType.Multiply => a * b,
                    EvalStackOperatorType.Divide => a / b,
                    EvalStackOperatorType.Modular => a % b,
                    _ => throw new Exception("Unsupported operator type in converter")
                };

                return new(folded);
            }

            Int64ConstantEvalStackItem Fold64(long a, long b)
            {
                long folded = opType switch
                {
                    EvalStackOperatorType.Add => a + b,
                    EvalStackOperatorType.Subtract => a - b,
                    EvalStackOperatorType.Multiply => a * b,
                    EvalStackOperatorType.Divide => a / b,
                    EvalStackOperatorType.Modular => a % b,
                    _ => throw new Exception("Unsupported operator type in converter")
                };

                return new(folded);
            }
        }

        public int GetConstantValueInt(EvalStackItem item) =>
            ((Int32ConstantEvalStackItem)item).Value;

        public long GetConstantValueLong(EvalStackItem item) =>
            item is Int32ConstantEvalStackItem item32 ? item32.Value : ((Int64ConstantEvalStackItem)item).Value;
    }
}
