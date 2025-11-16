namespace GeneratedEndpoints.Common;

internal readonly record struct ProducesValidationProblemMetadata(
    int StatusCode,
    string? ContentType,
    EquatableImmutableArray<string>? AdditionalContentTypes
);
