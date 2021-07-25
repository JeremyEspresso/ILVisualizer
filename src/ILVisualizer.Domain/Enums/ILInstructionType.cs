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

        // Parameterless - Basic maths operators
        Add = 32,
        Sub = 33,
        Mul = 34,
        Div = 35,
        Rem = 36,

        // Parameterless - Control flow
        Ret = 64,

        // One single "Int8" parameter
        Int8Parametered_Instructions = 4096,
        Ldc_I4_S = 4097,

		// One single "Int16" parameter
		Int16Parametered_Instructions = 4096,
		Ldloc = 4097,
		Ldarg = 4098,

		// One single "Int32" parameter
		Int32Parametered_Instructions = 16384,
        Ldc_I4 = 16385,

        // One single "Int64" parameter
        Int64Parametered_Instructions = 32768,
        Ldc_I8 = 32769,
    }
}
