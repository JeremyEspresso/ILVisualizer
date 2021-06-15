using System;
using ILVisualizer.Application.Common.Exceptions.Processor;
using ILVisualizer.Domain.Enums;
using ILVisualizer.Domain.Models;
using ILVisualizer.Domain.Models.EvalStack;
using ILVisualizer.Domain.Models.Processor;
using System.Collections.Generic;

namespace ILVisualizer.Application.Common.Services
{
    public class ILProcessorService
    {
        public Step CurrentStep = new();

        public ProcessorResult Result = new();
        public Stack<EvalStackItem> CurrentEvalStack = new();

        public ProcessorResult Process(IList<ILInstruction> instructions)
        {
            Initialize();

            for (int i = 0; i < instructions.Count; i++)
            {
                if (CurrentEvalStack.Count == 0)
                    InsertStatementBreak(i);

                CurrentStep = new Step();
                ProcessInstruction(instructions[i]);

                Result.Steps.Add(CurrentStep);
            }

            return Result;
        }

        void Initialize()
        {
            Result.Steps = new List<Step>();
            Result.Breaks = new List<StatementBreak>();
        }

        void ProcessInstruction(ILInstruction instruction)
        {
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
                    PushOne(new Int32ConstantEvalStackItem(instruction.IntArg));
                    break;
                case ILInstructionType.Ldc_I8:
                    PushOne(new Int64ConstantEvalStackItem(instruction.LongArg));
                    break;
                case ILInstructionType.Add:
                case ILInstructionType.Sub:
                case ILInstructionType.Mul:
                case ILInstructionType.Div:
                case ILInstructionType.Rem:
                    PerformOperation((EvalStackOperatorType)(instruction.Type - ILInstructionType.Add));
                    break;
                case ILInstructionType.Ret:
                    Pop();
                    break;
            }
        }

        void InsertStatementBreak(int i)
        {
            Result.Breaks.Add(new StatementBreak(i));
        }

        EvalStackItem Pop()
        {
            if (!CurrentEvalStack.TryPop(out var item)) throw new InvalidPopException();
            CurrentStep.ItemsPopped++;

            item.PoppedStepNo = (ushort)Result.Steps.Count;
            return item;
        }

        void PushOne(EvalStackItem item)
        {
            CurrentEvalStack.Push(item);

            CurrentStep.SinglePushed = item;
        }

        void PushMany(EvalStackItem[] item)
        {
            // Push to the current step.
            CurrentStep.HasMultiplePushed = true;
            CurrentStep.MultiplePushed = item;

            // Push to the curreneval stack.
            for (int i = 0; i < item.Length; i++)
                CurrentEvalStack.Push(item[i]);
        }

        enum FoldMode
        {
            None,
            Int32,
            Int64
        }

        void PerformOperation(EvalStackOperatorType opType)
        {
            var second = Pop();
            var first = Pop();

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
