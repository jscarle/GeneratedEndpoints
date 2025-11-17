using static GeneratedEndpoints.Common.Constants;

namespace GeneratedEndpoints.Common;

internal record struct RequestHandler
{
    public required RequestHandlerClass Class { get; init; }
    public required RequestHandlerMethod Method { get; init; }
    public required string HttpMethod { get; init; }
    public required string Pattern { get; init; }
    public required string? Name { get; set; }

    public string GetFullyQualifiedMethodDisplayName()
    {
        ReadOnlySpan<char> className = Class.Name;
        ReadOnlySpan<char> methodName = Method.Name;

        if (className.StartsWith(GlobalPrefix))
            className = className[GlobalPrefix.Length..];

        var classLen = className.Length;
        var methodLen = methodName.Length;
        var total = classLen + 1 + methodLen;

        Span<char> buffer = stackalloc char[total];
        className.CopyTo(buffer);

        buffer[classLen] = '.';

        methodName.CopyTo(buffer[(classLen + 1)..]);

        return buffer.ToString();
    }
}
