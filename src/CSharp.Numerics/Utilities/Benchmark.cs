// Copyright (c) DotNet Samples. All rights reserved.
// See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Performance.Utilities;

internal readonly ref struct Benchmark
{
    private readonly DefaultLogger _logger;
    private readonly string? _name;
    private readonly TimeSpan? _compare;
    private readonly Stopwatch _watch;

    /// <summary>
    /// Gets the elapsed time.
    /// </summary>
    public TimeSpan Elapsed => _watch.Elapsed;

    /// <summary>
    /// Initializes a new instance of the <see cref="Benchmark" /> struct.
    /// </summary>
    public Benchmark(DefaultLogger logger, [CallerMemberName] string? name = default, TimeSpan? compare = default)
    {
        _logger = logger;
        _name = name;
        _compare = compare;
        _watch = Stopwatch.StartNew();
    }

    /// <summary>
    /// Stops watch.
    /// </summary>
    public void Stop()
    {
        _watch.Stop();
    }

    /// <summary>
    /// Releases unmanaged and - optionally - managed resources.
    /// </summary>
    public void Dispose()
    {
        Stop();

        var perf = _compare.GetValueOrDefault(_watch.Elapsed).TotalMilliseconds / _watch.Elapsed.TotalMilliseconds;

        _logger.WriteTitle($"{_name,-50}");
        _logger.WriteValue($"{_watch.ElapsedMilliseconds,8} ms ({perf:0.0}x)");
        _logger.WriteLine();
    }
}
