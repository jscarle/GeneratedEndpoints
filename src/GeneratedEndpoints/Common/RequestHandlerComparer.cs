namespace GeneratedEndpoints.Common;

internal sealed class RequestHandlerComparer : IComparer<RequestHandler>
{
    public static RequestHandlerComparer Instance { get; } = new();

    public int Compare(RequestHandler x, RequestHandler y)
    {
        var comparison = string.Compare(x.Class.Name, y.Class.Name, StringComparison.Ordinal);
        if (comparison != 0)
            return comparison;

        comparison = string.Compare(x.Method.Name, y.Method.Name, StringComparison.Ordinal);
        if (comparison != 0)
            return comparison;

        comparison = string.Compare(x.HttpMethod, y.HttpMethod, StringComparison.Ordinal);
        if (comparison != 0)
            return comparison;

        return string.Compare(x.Pattern, y.Pattern, StringComparison.Ordinal);
    }
}
