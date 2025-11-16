using System;
using System.Text;

namespace GeneratedEndpoints.Common;

/// <summary>Provides a simple per-thread cache for <see cref="StringBuilder"/> instances.</summary>
internal static class StringBuilderPool
{
    private const int MaxBuilderCapacity = 128 * 1024;

    [ThreadStatic]
    private static StringBuilder? _cachedInstance;

    /// <summary>Gets a <see cref="StringBuilder"/> with at least the requested capacity.</summary>
    public static StringBuilder Get(int capacity = 16)
    {
        var builder = _cachedInstance;
        if (builder is null)
            return new StringBuilder(capacity);

        _cachedInstance = null;
        builder.Clear();
        if (builder.Capacity < capacity)
            builder.EnsureCapacity(capacity);

        return builder;
    }

    /// <summary>Returns the <see cref="StringBuilder"/> to the pool if it is below the maximum retention size.</summary>
    public static void Return(StringBuilder builder)
    {
        if (builder.Capacity > MaxBuilderCapacity)
            return;

        builder.Clear();
        if (_cachedInstance is null)
            _cachedInstance = builder;
    }

    /// <summary>Converts the builder to a string and returns it to the pool.</summary>
    public static string ToStringAndReturn(StringBuilder builder)
    {
        var result = builder.ToString();
        Return(builder);
        return result;
    }
}
