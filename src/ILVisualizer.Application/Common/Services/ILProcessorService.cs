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

        public ProcessorResult Result = new ProcessorResult();
        public Stack<EvalStackItem> CurrentEvalStack = new Stack<EvalStackItem>();

        public ProcessorResult Process(IList<ILInstruction> instructions)
        {
            Initialize();

            for (int i = 0; i < instructions.Count; i++)
            {
                CurrentStep = new Step();
                ProcessInstruction(instructions[i]);

                if (CurrentEvalStack.Count == 0)
                    InsertStatementBreak();

                Result.Steps.Add(CurrentStep);
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
                case ILInstructionType.Ldc_I4_0:
                case ILInstructionType.Ldc_I4_1:
                case ILInstructionType.Ldc_I4_2:
                case ILInstructionType.Ldc_I4_3:
                case ILInstructionType.Ldc_I4_4:
                case ILInstructionType.Ldc_I4_5:
                case ILInstructionType.Ldc_I4_6:
                case ILInstructionType.Ldc_I4_7:
                case ILInstructionType.Ldc_I4_8:
                    CurrentStep.SinglePushed = new ConstantEvalStackItem((int)instruction.Type);
                    break;
                case ILInstructionType.Ldc_I4_S:
                case ILInstructionType.Ldc_I4:
                    CurrentStep.SinglePushed = new ConstantEvalStackItem(instruction.IntArg);
                    break;
            }
        }

        public void InsertStatementBreak()
        {
            Result.Steps.Add(new Step());
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
