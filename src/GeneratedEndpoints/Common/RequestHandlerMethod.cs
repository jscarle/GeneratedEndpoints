namespace GeneratedEndpoints.Common;

internal readonly record struct RequestHandlerMethod : IComparable<RequestHandlerMethod>, IComparable
{
    public required string Name { get; init; }
    public required bool IsStatic { get; init; }
    public required bool RequiresDelegateWrapper { get; init; }
    public required EquatableImmutableArray<Parameter> Parameters { get; init; }
    public required EndpointConfiguration Configuration { get; init; }

    public int CompareTo(RequestHandlerMethod other)
    {
        return string.Compare(Name, other.Name, StringComparison.Ordinal);
    }

    public int CompareTo(object? obj)
    {
        if (obj is null)
            return 1;
        return obj is RequestHandlerMethod other ? CompareTo(other) : throw new ArgumentException($"Object must be of type {nameof(RequestHandlerMethod)}");
    }

    public static bool operator <(RequestHandlerMethod left, RequestHandlerMethod right)
    {
        return left.CompareTo(right) < 0;
    }

    public static bool operator >(RequestHandlerMethod left, RequestHandlerMethod right)
    {
        return left.CompareTo(right) > 0;
    }

    public static bool operator <=(RequestHandlerMethod left, RequestHandlerMethod right)
    {
        return left.CompareTo(right) <= 0;
    }

    public static bool operator >=(RequestHandlerMethod left, RequestHandlerMethod right)
    {
        return left.CompareTo(right) >= 0;
    }
}
