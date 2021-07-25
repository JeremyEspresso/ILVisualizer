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
				case ILInstructionType.Ldloc_0:
				case ILInstructionType.Ldloc_1:
				case ILInstructionType.Ldloc_2:
				case ILInstructionType.Ldloc_3:
					PushOne(new LocalEvalStackItem((short)(instruction.Type - ILInstructionType.Ldloc_0)));
					break;
				case ILInstructionType.Ldloc_S:
				case ILInstructionType.Ldloc:
					PushOne(new LocalEvalStackItem((short)instruction.Arg));
					break;
				case ILInstructionType.Ldarg_0:
				case ILInstructionType.Ldarg_1:
				case ILInstructionType.Ldarg_2:
				case ILInstructionType.Ldarg_3:
					PushOne(new ArgEvalStackItem((short)(instruction.Type - ILInstructionType.Ldarg_0)));
					break;
				case ILInstructionType.Ldarg_S:
				case ILInstructionType.Ldarg:
					PushOne(new ArgEvalStackItem((short)instruction.Arg));
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

		#region Operator Handling

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
            var mode = (FoldMode)Math.Max((int)GetFoldMode(first), (int)GetFoldMode(second));

			EvalStackItem toPush = mode switch
			{
				FoldMode.Int32 => Fold32(GetConstantValueInt(first), GetConstantValueInt(second)),
				FoldMode.Int64 => Fold64(GetConstantValueLong(first), GetConstantValueLong(second)),
				_ => new OperatorEvalStackItem(opType, first, second)
			};

			PushOne(toPush);

            FoldMode GetFoldMode(EvalStackItem itm)
            {
				if (itm is Int32ConstantEvalStackItem) return FoldMode.Int32;
				else if (itm is Int64ConstantEvalStackItem) return FoldMode.Int64;
				else return FoldMode.None;
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

		public static int GetConstantValueInt(EvalStackItem item) =>
			((Int32ConstantEvalStackItem)item).Value;

		public static long GetConstantValueLong(EvalStackItem item) =>
			item is Int32ConstantEvalStackItem item32 ? item32.Value : ((Int64ConstantEvalStackItem)item).Value;

		#endregion

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
    }
}
