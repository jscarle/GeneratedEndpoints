namespace GeneratedEndpoints.Common;

internal readonly record struct AcceptsMetadata(
    string RequestType,
    string ContentType,
    EquatableImmutableArray<string>? AdditionalContentTypes,
    bool IsOptional
);
