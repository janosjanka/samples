// Copyright (c) DotNet Samples. All rights reserved.
// See License.txt in the project root for license information.

using System.IO;
using System.Runtime.Intrinsics.X86;

namespace Performance.Utilities;

/// <summary>
/// Writes to an output stream.
/// </summary>
internal class DefaultLogger
{
    private readonly TextWriter _writer;

    /// <summary>
    /// Initializes a new instance of the <see cref="DefaultLogger" /> struct.
    /// </summary>
    /// <param name="writer">The writer.</param>
    public DefaultLogger(TextWriter writer)
    {
        _writer = writer;
    }

    /// <summary>
    /// Writes start to <see cref="TextWriter" />.
    /// </summary>
    public virtual void WriteStart()
    {
        _writer.WriteLine("CPU Performance Benchmark");
    }

    /// <summary>
    /// Writes processor info to <see cref="TextWriter" />.
    /// </summary>
    public virtual void WriteInfo()
    {
        _writer.WriteLine($"{nameof(Sse),-8} {Sse.IsSupported}");
        _writer.WriteLine($"{nameof(Sse2),-8} {Sse2.IsSupported}");
        _writer.WriteLine($"{nameof(Sse3),-8} {Sse3.IsSupported}");
        _writer.WriteLine($"{nameof(Sse41),-8} {Sse41.IsSupported}");
        _writer.WriteLine($"{nameof(Sse42),-8} {Sse42.IsSupported}");

        _writer.WriteLine($"{nameof(Avx),-8} {Avx.IsSupported}");
        _writer.WriteLine($"{nameof(Avx2),-8} {Avx2.IsSupported}");
        _writer.WriteLine($"{nameof(AvxVnni),-8} {AvxVnni.IsSupported}");
    }

    /// <summary>
    /// Writes a line break to <see cref="TextWriter" />.
    /// </summary>
    public virtual void WriteLine()
    {
        _writer.WriteLine();
    }

    /// <summary>
    /// Writes a line break to <see cref="TextWriter" />.
    /// </summary>
    public virtual void WriteBreak()
    {
        _writer.WriteLine("-----------------------------------------------------------------------------------");
    }

    /// <summary>
    /// Writes caption.
    /// </summary>
    public virtual void WriteTitle(string value)
    {
        _writer.Write(value);
    }

    /// <summary>
    /// Writes result.
    /// </summary>
    public virtual void WriteValue(string result)
    {
        _writer.Write(result);
    }

    /// <summary>
    /// Writes the <paramref name="array" /> to <see cref="TextWriter" />.
    /// </summary>
    public virtual void WriteArray<T>(T[] array)
    {
        _writer.WriteLine(string.Join(',', array));
    }

    /// <summary>
    /// Writes end to <see cref="TextWriter" />.
    /// </summary>
    public virtual void WriteEnd()
    {
    }
}
