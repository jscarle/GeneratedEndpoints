namespace GeneratedEndpoints.Common;

internal readonly record struct RequestHandlerClass(
    string Name,
    bool IsStatic,
    bool HasConfigureMethod,
    bool ConfigureMethodAcceptsServiceProvider,
    string? MapGroupPattern,
    string? MapGroupBuilderIdentifier,
    EndpointConfiguration Configuration
);
