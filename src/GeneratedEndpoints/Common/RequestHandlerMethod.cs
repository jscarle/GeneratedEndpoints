namespace GeneratedEndpoints.Common;

internal readonly record struct RequestHandlerMethod(
    string Name,
    bool IsStatic,
    bool IsAwaitable,
    EquatableImmutableArray<Parameter> Parameters
);
