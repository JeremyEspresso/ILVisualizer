using ILVisualizer.Application.Common.Entities.Parser;
using ILVisualizer.Application.Common.Exceptions.Parser;
using ILVisualizer.Application.Common.Interfaces;
using ILVisualizer.Domain.Enums;
using ILVisualizer.Domain.Models;
using System.Collections.Generic;

namespace ILVisualizer.Application.Common.Services
{
    public class ILParserService : Parser, IILParserService
    {
	    const char Space = ' ';

		List<ParsedILInstruction> _destination = new();

        public IList<ParsedILInstruction> Parse(string str)
        {
            Initialize(str);

            // Parse the first instruction
            AddInstruction();

            // Parse the rest
            bool isEnd = TryMoveToNextLine();
            while (!isEnd)
            {
                AddInstruction();
                isEnd = TryMoveToNextLine();
            }

            return _destination;
        }

        public void AddInstruction() => _destination.Add(ParseInstruction());

        public ParsedILInstruction ParseInstruction()
        {
            // Parse the opcode
            string opCodeStr = ReadToLineEndOrToChar(' ').ToLower();
         
			var res = new ParsedILInstruction(SelectInstruction(opCodeStr));

            // Parse the operands (parameters)
            if (res.Type > ILInstructionType.Int64Parametered_Instructions)
                res.Arg = ReadInt64Parameter(true);
            else if (res.Type > ILInstructionType.Int32Parametered_Instructions)
                res.Arg = ReadInt32Parameter(true);
            else if (res.Type > ILInstructionType.Int8Parametered_Instructions)
                res.Arg = ReadInt8Parameter(true);

            return res;
        }

		static ILInstructionType SelectInstruction(string opCodeStr) => opCodeStr switch
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
			"ldc.i8" => ILInstructionType.Ldc_I8,
			"ret" => ILInstructionType.Ret,
			"add" => ILInstructionType.Add,
			"sub" => ILInstructionType.Sub,
			"mul" => ILInstructionType.Mul,
			"div" => ILInstructionType.Div,
			"rem" => ILInstructionType.Rem,
			"ldloc" => ILInstructionType.Ldloc,
			"ldarg" => ILInstructionType.Ldarg,
			_ => throw new ParseFailedException($"Unrecognized instruction: {opCodeStr}")
		};

		long ReadInt64Parameter(bool isLastParameter)
        {
            string parameterData = isLastParameter ? ReadToLineEnd() : ReadToLineEndOrToChar(Space);

            if (!long.TryParse(parameterData, out long val))
                throw new ParseFailedException($"Invalid Int64 number in parameter at position {_currentPos}");

            return val;
        }

        int ReadInt32Parameter(bool isLastParameter)
        {
            string parameterData = isLastParameter ? ReadToLineEnd() : ReadToLineEndOrToChar(Space);

            if (!int.TryParse(parameterData, out int val))
                throw new ParseFailedException($"Invalid Int32 number in parameter at position {_currentPos}");

            return val;
        }

        sbyte ReadInt8Parameter(bool isLastParameter)
        {
            string parameterData = isLastParameter ? ReadToLineEnd() : ReadToLineEndOrToChar(Space);

            if (!sbyte.TryParse(parameterData, out sbyte val))
                throw new ParseFailedException($"Invalid Int8 number in parameter at position {_currentPos}");

            return val;
        }
    }
}