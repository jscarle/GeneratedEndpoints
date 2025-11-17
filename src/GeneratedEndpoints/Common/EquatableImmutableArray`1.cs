using System.Collections;
using System.Collections.Immutable;
using System.Runtime.InteropServices;

namespace GeneratedEndpoints.Common;

/// <summary>Represents an immutable array that implements <see cref="IEquatable{T}"/>.</summary>
/// <typeparam name="T">The type of elements in the array.</typeparam>
public readonly struct EquatableImmutableArray<T> : IEquatable<EquatableImmutableArray<T>>, IReadOnlyList<T>
{
    /// <summary>Gets an empty <see cref="EquatableImmutableArray{T}"/>.</summary>
    internal static EquatableImmutableArray<T> Empty { get; } = new(ImmutableArray<T>.Empty);

    /// <summary>Gets the number of elements in the array.</summary>
    public int Count => Array.Length;

    /// <summary>Gets the element at the specified index.</summary>
    /// <param name="index">The zero-based index of the element to get.</param>
    /// <returns>The element at the specified index.</returns>
    public T this[int index] => Array[index];

    private ImmutableArray<T> Array => _array ?? ImmutableArray<T>.Empty;
    private readonly ImmutableArray<T>? _array;

    internal EquatableImmutableArray(ImmutableArray<T>? array)
    {
        _array = array;
    }

    /// <summary>
    /// Gets the underlying array, or <see langword="null"/> if the array is empty. WARNING: This returns the underlying storage of the ImmutableArray and
    /// must only be used when you can guarantee no other code observes it as immutable.
    /// </summary>
    internal T[]? AsArray()
    {
        return ImmutableCollectionsMarshal.AsArray(Array);
    }

    /// <summary>
    /// Sorts the underlying array in place using the specified comparer. WARNING: This mutates the underlying storage of the ImmutableArray and must only be
    /// used when you can guarantee no other code observes it as immutable.
    /// </summary>
    internal void SortInPlace(IComparer<T>? comparer = null)
    {
        if (_array is null)
            return;

        var array = _array.Value;
        if (array.Length <= 1)
            return;

        comparer ??= Comparer<T>.Default;

        var raw = ImmutableCollectionsMarshal.AsArray(array);
        if (raw is not null)
            System.Array.Sort(raw, comparer);
    }

    /// <inheritdoc/>
    public bool Equals(EquatableImmutableArray<T> other)
    {
        return this.SequenceEqual(other);
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        return obj is EquatableImmutableArray<T> other && Equals(other);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        var hashCode = new HashCode();

        foreach (var item in Array)
            hashCode.Add(item);

        return hashCode.ToHashCode();
    }

    /// <summary>Determines whether two <see cref="EquatableImmutableArray{T}"/> instances are equal.</summary>
    /// <param name="left">The first <see cref="EquatableImmutableArray{T}"/> to compare.</param>
    /// <param name="right">The second <see cref="EquatableImmutableArray{T}"/> to compare.</param>
    /// <returns><see langword="true"/> if the two <see cref="EquatableImmutableArray{T}"/> instances are equal; otherwise, <see langword="false"/>.</returns>
    public static bool operator ==(EquatableImmutableArray<T> left, EquatableImmutableArray<T> right)
    {
        return left.Equals(right);
    }

    /// <summary>Determines whether two <see cref="EquatableImmutableArray{T}"/> instances are not equal.</summary>
    /// <param name="left">The first <see cref="EquatableImmutableArray{T}"/> to compare.</param>
    /// <param name="right">The second <see cref="EquatableImmutableArray{T}"/> to compare.</param>
    /// <returns><see langword="true"/> if the two <see cref="EquatableImmutableArray{T}"/> instances are not equal; otherwise, <see langword="false"/>.</returns>
    public static bool operator !=(EquatableImmutableArray<T> left, EquatableImmutableArray<T> right)
    {
        return !left.Equals(right);
    }

    /// <inheritdoc/>
    public IEnumerator<T> GetEnumerator()
    {
        // ReSharper disable once ForCanBeConvertedToForeach
        // ReSharper disable once LoopCanBeConvertedToQuery
        for (var index = 0; index < Array.Length; index++)
        {
            var item = Array[index];
            yield return item;
        }
    }

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
