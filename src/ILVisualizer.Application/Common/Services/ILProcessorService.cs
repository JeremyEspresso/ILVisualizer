using ILVisualizer.Domain.Enums;
using ILVisualizer.Domain.Models;
using ILVisualizer.Domain.Models.EvalStack;
using ILVisualizer.Domain.Models.Processor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                CurrentStep = new Step();
                ProcessInstruction(instructions[i]);

                Result.Steps.Add(CurrentStep);

                if (CurrentEvalStack.Count == 0)
                    InsertStatementBreak();
            }

            return Result;
        }

        void Initialize()
        {
            Result.Steps = new List<Step>();
            Result.Breaks = new List<StatementBreak>();
        }

        public void ProcessInstruction(ILInstruction instruction)
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
            }
        }

        public void InsertStatementBreak()
        {
            Result.Breaks.Add(new StatementBreak(Result.Steps.Count));
        }

        public EvalStackItem Pop()
        {
            CurrentStep.ItemsPopped++;

            var item = CurrentEvalStack.Pop();
            item.PoppedStepNo = (ushort)Result.Steps.Count;
            return item;
        }

        public void PushOne(EvalStackItem item)
        {
            CurrentEvalStack.Push(item);

            CurrentStep.SinglePushed = item;
        }

        public void PushMany(EvalStackItem[] item)
        {
            // Push to the current step.
            CurrentStep.HasMultiplePushed = true;
            CurrentStep.MultiplePushed = item;

            // Push to the curreneval stack.
            for (int i = 0; i < item.Length; i++)
                CurrentEvalStack.Push(item[i]);
        }
    }
}
