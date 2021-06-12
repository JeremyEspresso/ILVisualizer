using ILVisualizer.Application.Common.Entities.Parser;
using ILVisualizer.Application.Common.Exceptions.Parser;
using ILVisualizer.Application.Common.Interfaces;
using ILVisualizer.Domain.Enums.IL;
using ILVisualizer.Domain.Models.IL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILVisualizer.Application.Common.Services
{
    public class ILParserService : Parser, IILParserService
    {
        List<ILInstruction> _destination = new();

        public IList<ILInstruction> Parse(string str)
        {
            Initialize(str);

            // Parse the first instruction
            AddInstruction();

            // Parse the rest
            bool isLastLine = TryMoveToNextLine();
            while (!isLastLine)
            {
                AddInstruction();
                isLastLine = TryMoveToNextLine();
            }

            return _destination;
        }

        public void AddInstruction() => _destination.Add(ParseInstruction());

        public ILInstruction ParseInstruction()
        {
            var res = new ILInstruction();

            // Parse the opcode
            string opCodeStr = ReadToLineEndOr(' ').ToLower();
            res.Type = SelectInstruction(opCodeStr);

            // Parse the operands (parameters)
            if (res.Type > ILInstructionType.Int32Parametered_Instructions)            
                res.IntArg = ReadInt32Parameter(true);
            else if (res.Type > ILInstructionType.Int8Parametered_Instructions)
                res.IntArg = ReadInt8Parameter(true);

            return res;
        }

        static ILInstructionType SelectInstruction(string opCodeStr)
        {
            return opCodeStr switch
            {
                "ldc.i4.0" => ILInstructionType.Ldc_I4_0,
                "ldc.i4.1" => ILInstructionType.Ldc_I4_1,
                "ldc.i4.2" => ILInstructionType.Ldc_I4_2,
                "ldc.i4.3" => ILInstructionType.Ldc_I4_3,
                "ldc.i4.4" => ILInstructionType.Ldc_I4_4,
                "ldc.i4.5" => ILInstructionType.Ldc_I4_5,
                "ldc.i4.6" => ILInstructionType.Ldc_I4_6,
                "ldc.i4.7" => ILInstructionType.Ldc_I4_7,
                "ldc.i4.8" => ILInstructionType.Ldc_I4_8,
                "ldc.i4.m1" => ILInstructionType.Ldc_I4_M1,
                "ldc.i4.s" => ILInstructionType.Ldc_I4_S,
                "ldc.i4" => ILInstructionType.Ldc_I4,
                _ => throw new ParseFailedException($"Unrecognized instruction: {opCodeStr}")
            };
        }

        int ReadInt32Parameter(bool isLastParameter)
        {
            string parameterData = isLastParameter ? ReadToLineEnd() : ReadToLineEndOr(' ');

            if (!int.TryParse(parameterData, out int val))
                throw new ParseFailedException($"Invalid Int32 number in parameter at position {_currentPos}");

            return val;
        }

        sbyte ReadInt8Parameter(bool isLastParameter)
        {
            string parameterData = isLastParameter ? ReadToLineEnd() : ReadToLineEndOr(' ');

            if (!sbyte.TryParse(parameterData, out sbyte val))
                throw new ParseFailedException($"Invalid Int8 number in parameter at position {_currentPos}");

            return val;
        }
    }
}
