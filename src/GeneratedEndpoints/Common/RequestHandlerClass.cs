namespace GeneratedEndpoints.Common;

internal readonly record struct RequestHandlerClass : IComparable<RequestHandlerClass>, IComparable
{
    public required string Name { get; init; }
    public required bool IsStatic { get; init; }
    public required bool HasConfigureMethod { get; init; }
    public required bool ConfigureMethodAcceptsServiceProvider { get; init; }
    public required EndpointConfiguration Configuration { get; init; }

    public int CompareTo(RequestHandlerClass other)
    {
        return string.Compare(Name, other.Name, StringComparison.Ordinal);
    }

    public int CompareTo(object? obj)
    {
        if (obj is null)
            return 1;
        return obj is RequestHandlerClass other ? CompareTo(other) : throw new ArgumentException($"Object must be of type {nameof(RequestHandlerClass)}");
    }

    public static bool operator <(RequestHandlerClass left, RequestHandlerClass right)
    {
        return left.CompareTo(right) < 0;
    }

    public static bool operator >(RequestHandlerClass left, RequestHandlerClass right)
    {
        return left.CompareTo(right) > 0;
    }

    public static bool operator <=(RequestHandlerClass left, RequestHandlerClass right)
    {
        return left.CompareTo(right) <= 0;
    }

    public static bool operator >=(RequestHandlerClass left, RequestHandlerClass right)
    {
        return left.CompareTo(right) >= 0;
    }
}
