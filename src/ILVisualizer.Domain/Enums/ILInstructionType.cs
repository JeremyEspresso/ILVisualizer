using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILVisualizer.Domain.Enums
{
    public enum ILInstructionType
    {
        // Parameterless
        Ldc_I4_0 = 0,
        Ldc_I4_1 = 1,
        Ldc_I4_2 = 2,
        Ldc_I4_3 = 3,
        Ldc_I4_4 = 4,
        Ldc_I4_5 = 5,
        Ldc_I4_6 = 6,
        Ldc_I4_7 = 7,
        Ldc_I4_8 = 8,
        Ldc_I4_M1 = -1,

        // One single "Int8" parameter
        Int8Parametered_Instructions = 9,
        Ldc_I4_S = 10,

        // One single "Int32" parameter
        Int32Parametered_Instructions = 11,
        Ldc_I4 = 12
    }
}
