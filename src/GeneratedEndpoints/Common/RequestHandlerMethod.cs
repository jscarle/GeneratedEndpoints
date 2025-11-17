namespace GeneratedEndpoints.Common;

internal readonly record struct RequestHandlerMethod(
    string Name,
    bool IsStatic,
    EquatableImmutableArray<Parameter> Parameters,
    EndpointConfiguration Configuration
);
