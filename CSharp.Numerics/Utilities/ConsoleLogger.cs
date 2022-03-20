// Copyright (c) DotNet Samples. All rights reserved.
// See License.txt in the project root for license information.

using System;
using System.IO;

namespace Performance.Utilities;

/// <summary>
/// Writes to console out.
/// </summary>
/// <seealso cref="DefaultLogger" />
internal sealed class ConsoleLogger : DefaultLogger
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ConsoleLogger" /> class.
    /// </summary>
    /// <param name="writer">The writer.</param>
    public ConsoleLogger()
        : base(Console.Out)
    {
    }

    /// <summary>
    /// Writes caption.
    /// </summary>
    public override void WriteTitle(string value)
    {
        Console.ForegroundColor = ConsoleColor.Magenta;
        base.WriteTitle(value);
        Console.ResetColor();
    }

    /// <summary>
    /// Writes result.
    /// </summary>
    public override void WriteValue(string value)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        base.WriteTitle(value);
        Console.ResetColor();
    }

    /// <summary>
    /// Writes the <paramref name="array" /> to <see cref="TextWriter" />.
    /// </summary>
    public override void WriteArray<T>(T[] array)
    {
        Console.ForegroundColor = ConsoleColor.DarkGray;
        base.WriteArray(array);
        Console.ResetColor();
    }
}