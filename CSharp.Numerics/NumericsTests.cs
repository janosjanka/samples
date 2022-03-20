// Copyright (c) DotNet Samples. All rights reserved.
// See License.txt in the project root for license information.

using System;
using System.Numerics;
using System.Runtime.Intrinsics.X86;
using Performance.Utilities;

namespace Performance;

internal sealed class NumericsTests
{
    private const int ArrayLength = 16;
    private const int ArrayBlock4 = ArrayLength & ~3;
    private const int ArrayBlock8 = ArrayLength & ~7;

    private readonly int _repeats;
    private readonly DefaultLogger _logger;

    private readonly int[] _array1 = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };
    private readonly int[] _array2 = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };
    private TimeSpan _compareTime = TimeSpan.Zero;

    /// <summary>
    /// Initializes a new instance of the <see cref="NumericsTests" /> class.
    /// </summary>
    public NumericsTests(DefaultLogger logger, int repeats)
    {
        _logger = logger;
        _repeats = repeats;
    }

    /// <summary>
    /// Multiplies two arrays with bound checks using a simple for loop.
    /// The compiler elides bound checks for Release builds so it will perform
    /// similarly to the <see cref="Multiply_For_BoundChecksOff" />.
    /// </summary>
    internal void Multiply_Arrays_BoundChecksOn()
    {
        var result = new int[ArrayLength];
        var array1 = _array1;
        var array2 = _array2;

        using (Benchmark benchmark = new(_logger))
        {
            for (var it = 0; it < _repeats; it++)
            {
                for (var i = 0; i < ArrayLength; i++)
                {
                    result[i] = array1[i] * array2[i];
                }
            }

            benchmark.Stop();
            _compareTime = benchmark.Elapsed;
        }

        _logger.WriteArray(result);
    }

    /// <summary>
    /// Multiplies two arrays with bound checks using a simple for loop.
    /// In Release build, there is no difference between it and <see cref="Multiply_Arrays_BoundChecksOn" />.
    /// </summary>
    internal unsafe void Multiply_Arrays_BoundChecksOff()
    {
        var result = new int[ArrayLength];
        var array1 = _array1;
        var array2 = _array2;

        fixed (int* resultPtr = result)
        {
            using Benchmark benchmark = new(_logger, compare: _compareTime);

            for (var it = 0; it < _repeats; it++)
            {
                for (var i = 0; i < ArrayLength; i++)
                {
                    resultPtr[i] = array1[i] * array2[i];
                }
            }
        }

        _logger.WriteArray(result);
    }

    /// <summary>
    /// Multiplies two arrays with bound checks using an unrolled loop.
    /// </summary>
    internal void Multiply_Arrays_BoundChecksOn_Partially_Unrolled()
    {
        var result = new int[ArrayLength];
        var array1 = _array1;
        var array2 = _array2;

        using (Benchmark benchmark = new(_logger, compare: _compareTime))
        {
            for (var it = 0; it < _repeats; it++)
            {
                for (var i = 0; i < ArrayBlock8; i += 8)
                {
                    result[i + 0] = array1[i + 0] * array2[i + 0];
                    result[i + 1] = array1[i + 1] * array2[i + 1];
                    result[i + 2] = array1[i + 2] * array2[i + 2];
                    result[i + 3] = array1[i + 3] * array2[i + 3];
                    result[i + 4] = array1[i + 4] * array2[i + 4];
                    result[i + 5] = array1[i + 5] * array2[i + 5];
                    result[i + 6] = array1[i + 6] * array2[i + 6];
                    result[i + 7] = array1[i + 7] * array2[i + 7];
                }
            }
        }

        _logger.WriteArray(result);
    }

    /// <summary>
    /// Multiplies two arrays without overhead of extra bound checks
    /// emitted for each index access.
    /// </summary>
    internal unsafe void Multiply_Arrays_BoundChecksOff_Partially_Unrolled()
    {
        var result = new int[ArrayLength];
        var array1 = _array1;
        var array2 = _array2;

        // Get interior pointers to the managed heap data to eliminate array bound checks
        // for each index access. It radically increases performance by removing
        // both compare (cmp) and conditional jump (ja/jb) instructions.
        fixed (int* resultPtr = result)
        {
            using Benchmark benchmark = new(_logger, compare: _compareTime);

            for (var it = 0; it < _repeats; it++)
            {
                for (var i = 0; i < ArrayBlock8; i += 8)
                {
                    resultPtr[i + 0] = array1[i + 0] * array2[i + 0];
                    resultPtr[i + 1] = array1[i + 1] * array2[i + 1];
                    resultPtr[i + 2] = array1[i + 2] * array2[i + 2];
                    resultPtr[i + 3] = array1[i + 3] * array2[i + 3];
                    resultPtr[i + 4] = array1[i + 4] * array2[i + 4];
                    resultPtr[i + 5] = array1[i + 5] * array2[i + 5];
                    resultPtr[i + 6] = array1[i + 6] * array2[i + 6];
                    resultPtr[i + 7] = array1[i + 7] * array2[i + 7];
                }
            }
        }

        _logger.WriteArray(result);
    }

    /// <summary>
    /// Multiplies two arrays with bound checks using an unrolled loop.
    /// </summary>
    internal void Multiply_Arrays_BoundChecksOn_Unrolled()
    {
        var result = new int[ArrayLength];
        var array1 = _array1;
        var array2 = _array2;

        using (Benchmark benchmark = new(_logger, compare: _compareTime))
        {
            for (var it = 0; it < _repeats; it++)
            {
                result[0] = array1[0] * array2[0];
                result[1] = array1[1] * array2[1];
                result[2] = array1[2] * array2[2];
                result[3] = array1[3] * array2[3];
                result[4] = array1[4] * array2[4];
                result[5] = array1[5] * array2[5];
                result[6] = array1[6] * array2[6];
                result[7] = array1[7] * array2[7];
                result[8] = array1[8] * array2[8];
                result[9] = array1[9] * array2[9];
                result[10] = array1[10] * array2[10];
                result[11] = array1[11] * array2[11];
                result[12] = array1[12] * array2[12];
                result[13] = array1[13] * array2[13];
                result[14] = array1[14] * array2[14];
                result[15] = array1[15] * array2[15];
            }
        }

        _logger.WriteArray(result);
    }

    /// <summary>
    /// Multiplies two arrays without overhead of extra bound checks
    /// emitted for each index access.
    /// </summary>
    internal unsafe void Multiply_Arrays_BoundChecksOff_Unrolled()
    {
        var result = new int[ArrayLength];
        var array1 = _array1;
        var array2 = _array2;

        // Get interior pointers to the managed heap data to eliminate array bound checks
        // for each index access. It radically increases performance by removing
        // both compare (cmp) and conditional jump (ja/jb) instructions.
        fixed (int* resultPtr = result)
        {
            using Benchmark benchmark = new(_logger, compare: _compareTime);

            for (var it = 0; it < _repeats; it++)
            {
                resultPtr[0] = array1[0] * array2[0];
                resultPtr[1] = array1[1] * array2[1];
                resultPtr[2] = array1[2] * array2[2];
                resultPtr[3] = array1[3] * array2[3];
                resultPtr[4] = array1[4] * array2[4];
                resultPtr[5] = array1[5] * array2[5];
                resultPtr[6] = array1[6] * array2[6];
                resultPtr[7] = array1[7] * array2[7];
                resultPtr[8] = array1[8] * array2[8];
                resultPtr[9] = array1[9] * array2[9];
                resultPtr[10] = array1[10] * array2[10];
                resultPtr[11] = array1[11] * array2[11];
                resultPtr[12] = array1[12] * array2[12];
                resultPtr[13] = array1[13] * array2[13];
                resultPtr[14] = array1[14] * array2[14];
                resultPtr[15] = array1[15] * array2[15];
            }
        }

        _logger.WriteArray(result);
    }

    /// <summary>
    /// Multiplies two arrays using CPU vector instructions.
    /// </summary>
    internal void Multiply_Arrays_Vector_Auto()
    {
        var result = new int[ArrayLength];
        var array1 = _array1;
        var array2 = _array2;

        var unitSize = Vector<int>.Count;
        var blockSize = ArrayLength & ~(unitSize - 1);

        using (Benchmark benchmark = new(_logger, compare: _compareTime))
        {
            for (var it = 0; it < _repeats; it++)
            {
                for (var i = 0; i < blockSize; i += unitSize)
                {
                    (new Vector<int>(array1, i) * new Vector<int>(array2, i)).CopyTo(result, i);
                }
            }
        }

        _logger.WriteArray(result);
    }

    /// <summary>
    /// Multiplies two arrays using CPU vector instructions.
    /// </summary>
    internal void Multiply_Arrays_Vector_Auto_Unrolled()
    {
        var result = new int[ArrayLength];
        var array1 = _array1;
        var array2 = _array2;

        var unitSize = Vector<int>.Count;

        using (Benchmark benchmark = new(_logger, compare: _compareTime))
        {
            for (var it = 0; it < _repeats; it++)
            {
                (new Vector<int>(array1, 0) * new Vector<int>(array2, 0)).CopyTo(result, 0);
                (new Vector<int>(array1, unitSize) * new Vector<int>(array2, unitSize)).CopyTo(result, unitSize);
            }
        }

        _logger.WriteArray(result);
    }

    /// <summary>
    /// Multiplies two arrays using CPU vector instructions.
    /// </summary>
    internal unsafe void Multiply_Arrays_Vector128_Sse4()
    {
        using AlignedArray<int> result = new(ArrayLength, 16);
        using AlignedArray<int> array1 = new(_array1, 16);
        using AlignedArray<int> array2 = new(_array2, 16);

        using (Benchmark benchmark = new(_logger, compare: _compareTime))
        {
            for (var it = 0; it < _repeats; it++)
            {
                for (var i = 0; i < ArrayBlock4; i += 4)
                {
                    Sse2.StoreAligned(result + i, Sse41.MultiplyLow(Sse2.LoadAlignedVector128(array1 + i), Sse2.LoadAlignedVector128(array2 + i)));
                }
            }
        }

        _logger.WriteArray((int[])result);
    }

    /// <summary>
    /// Multiplies two arrays using CPU vector instructions.
    /// </summary>
    internal unsafe void Multiply_Arrays_Vector128_Sse4_Unrolled()
    {
        using AlignedArray<int> result = new(ArrayLength, 16);
        using AlignedArray<int> array1 = new(_array1, 16);
        using AlignedArray<int> array2 = new(_array2, 16);

        using (Benchmark benchmark = new(_logger, compare: _compareTime))
        {
            for (var it = 0; it < _repeats; it++)
            {
                Sse2.StoreAligned(result + 0, Sse41.MultiplyLow(Sse2.LoadAlignedVector128(array1 + 0), Sse2.LoadAlignedVector128(array2 + 0)));
                Sse2.StoreAligned(result + 4, Sse41.MultiplyLow(Sse2.LoadAlignedVector128(array1 + 4), Sse2.LoadAlignedVector128(array2 + 4)));
                Sse2.StoreAligned(result + 8, Sse41.MultiplyLow(Sse2.LoadAlignedVector128(array1 + 8), Sse2.LoadAlignedVector128(array2 + 8)));
                Sse2.StoreAligned(result + 12, Sse41.MultiplyLow(Sse2.LoadAlignedVector128(array1 + 12), Sse2.LoadAlignedVector128(array2 + 12)));
            }
        }

        _logger.WriteArray((int[])result);
    }

    /// <summary>
    /// Multiplies two arrays using CPU vector instructions.
    /// </summary>
    internal unsafe void Multiply_Arrays_Vector256_Avx2()
    {
        using AlignedArray<int> result = new(ArrayLength, 32);
        using AlignedArray<int> array1 = new(_array1, 32);
        using AlignedArray<int> array2 = new(_array2, 32);

        using (Benchmark benchmark = new(_logger, compare: _compareTime))
        {
            for (var it = 0; it < _repeats; it++)
            {
                for (var i = 0; i < ArrayBlock8; i += 8)
                {
                    Avx.StoreAligned(result + i, Avx2.MultiplyLow(Avx.LoadAlignedVector256(array1 + i), Avx.LoadAlignedVector256(array2 + i)));
                }
            }
        }

        _logger.WriteArray((int[])result);
    }

    /// <summary>
    /// Multiplies two arrays using CPU vector instructions.
    /// </summary>
    internal unsafe void Multiply_Arrays_Vector256_Avx2_Unrolled_Unaligned()
    {
        var result = new int[ArrayLength];

        fixed (int* resultPtr = result, array1 = _array1, array2 = _array2)
        {
            using Benchmark benchmark = new(_logger, nameof(Multiply_Arrays_Vector256_Avx2_Unrolled_Unaligned), _compareTime);

            for (var it = 0; it < _repeats; it++)
            {
                Avx.Store(resultPtr + 0, Avx2.MultiplyLow(Avx.LoadVector256(array1 + 0), Avx.LoadVector256(array2 + 0)));
                Avx.Store(resultPtr + 8, Avx2.MultiplyLow(Avx.LoadVector256(array1 + 8), Avx.LoadVector256(array2 + 8)));
            }
        }

        _logger.WriteArray(result);
    }

    /// <summary>
    /// Multiplies two arrays using CPU vector instructions.
    /// </summary>
    internal unsafe void Multiply_Arrays_Vector256_Avx2_Unrolled_Aligned()
    {
        using AlignedArray<int> result = new(ArrayLength, 32);
        using AlignedArray<int> array1 = new(_array1, 32);
        using AlignedArray<int> array2 = new(_array2, 32);

        using (Benchmark benchmark = new(_logger, nameof(Multiply_Arrays_Vector256_Avx2_Unrolled_Aligned), _compareTime))
        {
            for (var it = 0; it < _repeats; it++)
            {
                Avx.StoreAligned(result + 0, Avx2.MultiplyLow(Avx.LoadAlignedVector256(array1 + 0), Avx.LoadAlignedVector256(array2 + 0)));
                Avx.StoreAligned(result + 8, Avx2.MultiplyLow(Avx.LoadAlignedVector256(array1 + 8), Avx.LoadAlignedVector256(array2 + 8)));
            }
        }

        _logger.WriteArray((int[])result);
    }
}
