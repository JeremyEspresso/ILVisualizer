namespace ILVisualizer.Domain.Enums
{
    public enum ILInstructionType
    {
        // Parameterless
        Ldc_I4_0,
        Ldc_I4_1,
        Ldc_I4_2,
        Ldc_I4_3,
        Ldc_I4_4,
        Ldc_I4_5,
        Ldc_I4_6,
        Ldc_I4_7,
        Ldc_I4_8,
        Ldc_I4_M1,

        // One single "Int8" parameter
        Int8Parametered_Instructions,
        Ldc_I4_S,

        // One single "Int32" parameter
        Int32Parametered_Instructions,
        Ldc_I4
    }
}
