namespace GeneratedEndpoints.Common;

internal readonly record struct ProducesProblemMetadata(
    int StatusCode,
    string? ContentType,
    EquatableImmutableArray<string>? AdditionalContentTypes
);
