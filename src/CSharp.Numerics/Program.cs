// Copyright (c) DotNet Samples. All rights reserved.
// See License.txt in the project root for license information.

using Performance.Utilities;

namespace Performance;

public sealed class Program
{
#if DEBUG
    private const int Repeats = 1_000_000;
#else
    private const int Repeats = 20_000_000;
#endif

    public static void Main()
    {
        DefaultLogger logger = new ConsoleLogger();
        NumericsTests tests = new(logger, repeats: Repeats);

        logger.WriteStart();
        logger.WriteBreak();

        logger.WriteInfo();
        logger.WriteBreak();

        tests.Multiply_Arrays_BoundChecksOn();
        tests.Multiply_Arrays_BoundChecksOff();
        logger.WriteBreak();

        tests.Multiply_Arrays_BoundChecksOn_Partially_Unrolled();
        tests.Multiply_Arrays_BoundChecksOff_Partially_Unrolled();
        tests.Multiply_Arrays_BoundChecksOn_Unrolled();
        tests.Multiply_Arrays_BoundChecksOff_Unrolled();
        logger.WriteBreak();

        tests.Multiply_Arrays_Vector_Auto();
        tests.Multiply_Arrays_Vector_Auto_Unrolled();
        tests.Multiply_Arrays_Vector128_Sse4();
        tests.Multiply_Arrays_Vector128_Sse4_Unrolled();
        tests.Multiply_Arrays_Vector256_Avx2();
        tests.Multiply_Arrays_Vector256_Avx2_Unrolled_Unaligned();
        tests.Multiply_Arrays_Vector256_Avx2_Unrolled_Aligned();

        logger.WriteBreak();
        logger.WriteEnd();
    }
}
