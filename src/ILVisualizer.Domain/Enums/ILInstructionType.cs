using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILVisualizer.Domain.Enums
{
    public enum ILInstructionType
    {
        // Parameterless - Constants
        Ldc_I4_M1 = -1,
        Ldc_I4_0 = 0,
        Ldc_I4_1 = 1,
        Ldc_I4_2 = 2,
        Ldc_I4_3 = 3,
        Ldc_I4_4 = 4,
        Ldc_I4_5 = 5,
        Ldc_I4_6 = 6,
        Ldc_I4_7 = 7,
        Ldc_I4_8 = 8,
        Ldc_I8 = 9,

        // Parameterless - Basic maths operators
        Add = 32,
        Sub = 33,
        Mul = 34,
        Div = 35,
        Rem = 36,

        // One single "Int8" parameter
        Int8Parametered_Instructions = 8192,
        Ldc_I4_S = 8193,

        // One single "Int32" parameter
        Int32Parametered_Instructions = 16384,
        Ldc_I4 = 16385
    }
}
