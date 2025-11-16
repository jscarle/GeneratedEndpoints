using System.Collections.Immutable;

namespace GeneratedEndpoints.Common;

/// <summary>Provides extension methods to convert various collections to an <see cref="EquatableImmutableArray{T}"/>.</summary>
internal static class EquatableImmutableArray
{
    /// <summary>Converts an <see cref="ImmutableArray{T}.Builder"/> to an <see cref="EquatableImmutableArray{T}"/>.</summary>
    /// <param name="builder">The <see cref="ImmutableArray{T}.Builder"/> to convert.</param>
    /// <returns>An <see cref="EquatableImmutableArray{T}"/> containing the same elements as the original builder.</returns>
    public static EquatableImmutableArray<T> ToEquatableImmutable<T>(this ImmutableArray<T>.Builder builder)
    {
        return new EquatableImmutableArray<T>(builder.ToImmutable());
    }

    /// <summary>Converts an <see cref="ImmutableArray{T}"/> to an <see cref="EquatableImmutableArray{T}"/>.</summary>
    /// <param name="array">The <see cref="ImmutableArray{T}"/> to convert.</param>
    /// <returns>An <see cref="EquatableImmutableArray{T}"/> containing the same elements as the original array.</returns>
    public static EquatableImmutableArray<T> ToEquatableImmutableArray<T>(this ImmutableArray<T> array)
    {
        return new EquatableImmutableArray<T>(array);
    }

    /// <summary>Converts an <see cref="IEnumerable{T}"/> to an <see cref="EquatableImmutableArray{T}"/>.</summary>
    /// <param name="enumerable">The <see cref="IEnumerable{T}"/> to convert.</param>
    /// <returns>An <see cref="EquatableImmutableArray{T}"/> containing the same elements as the original enumerable.</returns>
    public static EquatableImmutableArray<T> ToEquatableImmutableArray<T>(this IEnumerable<T> enumerable)
    {
        return new EquatableImmutableArray<T>([..enumerable]);
    }
}
