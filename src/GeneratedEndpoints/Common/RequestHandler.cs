using static GeneratedEndpoints.Common.Constants;

namespace GeneratedEndpoints.Common;

internal record struct RequestHandler
{
    public required RequestHandlerClass Class { get; init; }
    public required RequestHandlerMethod Method { get; init; }
    public required string HttpMethod { get; init; }
    public required string Pattern { get; init; }

    public required string? Name
    {
        readonly get => _name;
        init => _name = value;
    }

    private string? _name;

    public void SetFullyQualifiedName(bool includeSignature = false)
    {
        ReadOnlySpan<char> className = Class.Name;
        ReadOnlySpan<char> methodName = Method.Name;

        if (className.StartsWith(GlobalPrefix))
            className = className[GlobalPrefix.Length..];

        if (includeSignature)
        {
            var builder = StringBuilderPool.Get(Class.Name.Length + Method.Name.Length + (Method.Parameters.Count * 32) + 4);
            builder.Append(className.ToString());
            builder.Append('.');
            builder.Append(methodName.ToString());
            builder.Append('(');

            for (var index = 0; index < Method.Parameters.Count; index++)
            {
                if (index > 0)
                    builder.Append(", ");

                builder.Append(Method.Parameters[index].Type.Replace(GlobalPrefix, ""));
            }

            builder.Append(')');
            _name = StringBuilderPool.ToStringAndReturn(builder);
            return;
        }

        var classLen = className.Length;
        var methodLen = methodName.Length;
        var total = classLen + 1 + methodLen;

        Span<char> buffer = stackalloc char[total];
        className.CopyTo(buffer);

        buffer[classLen] = '.';

        methodName.CopyTo(buffer[(classLen + 1)..]);

        _name = buffer.ToString();
    }
}
