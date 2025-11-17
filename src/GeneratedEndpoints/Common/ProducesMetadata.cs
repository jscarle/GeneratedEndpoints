namespace GeneratedEndpoints.Common;

internal readonly record struct ProducesMetadata(
    string ResponseType,
    int StatusCode,
    string? ContentType,
    EquatableImmutableArray<string>? AdditionalContentTypes
);
