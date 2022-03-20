// Copyright (c) DotNet Samples. All rights reserved.
// See License.txt in the project root for license information.

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Performance;

/// <summary>
/// Represents an aligned array where elements are padded to a fix length.
/// SRI methods require memory to be aligned to either 16 (Vector128) or 32 bytes (Vector256) boundary.
/// </summary>
/// <typeparam name="T">The type of the elements.</typeparam>
internal readonly unsafe ref struct AlignedArray<T> where T : unmanaged
{
    private readonly IntPtr _mem;
    private readonly T* _ptr;
    private readonly int _len;

    /// <summary>
    /// Creates an aligned array.
    /// </summary>
    public AlignedArray(int length, int alignment = 16)
    {
        var offs = alignment - 1;
        _mem = Marshal.AllocHGlobal(length * sizeof(T) + offs);
        _ptr = (T*)(alignment * (((long)_mem + offs) / alignment));
        _len = length;
    }

    /// <summary>
    /// Creates an aligned array.
    /// </summary>
    public AlignedArray(T[] elements, int alignment)
        : this(elements.Length, alignment)
    {
        var it = _ptr;
        for (var i = 0; i < elements.Length; i++)
        {
            *it++ = elements[i];
        }
    }

    /// <summary>
    /// Gets the <see cref="T*" /> at the specified <paramref name="index" />.
    /// </summary>
    public T this[int index]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _ptr[index];
    }

    /// <summary>
    /// Performs an explicit conversion from <see cref="AlignedArray{T}"/> to <see cref="T"/>.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>
    /// The result of the conversion.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator T*(AlignedArray<T> value)
    {
        return value._ptr;
    }

    /// <summary>
    /// Implements the operator +.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="offset">The offset.</param>
    /// <returns>
    /// The result of the operator.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T* operator +(AlignedArray<T> value, int offset)
    {
        return value._ptr + offset;
    }

    /// <summary>
    /// Performs an explicit conversion from <see cref="AlignedArray{T}" /> to <see cref="T[]" />.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>
    /// The result of the conversion.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator T[](AlignedArray<T> value)
    {
        var result = new T[value._len];

        var it = value._ptr;
        for (var i = 0; i < value._len; i++)
        {
            result[i] = *it++;
        }

        return result;
    }

    /// <summary>
    /// Releases unmanaged and - optionally - managed resources.
    /// </summary>
    public void Dispose()
    {
        Marshal.FreeHGlobal(_mem);
    }
}
