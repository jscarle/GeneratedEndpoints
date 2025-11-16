namespace GeneratedEndpoints.Common;

internal readonly record struct RequestHandlerMetadata(
    string? Name,
    string? DisplayName,
    string? Summary,
    string? Description,
    EquatableImmutableArray<string>? Tags,
    EquatableImmutableArray<AcceptsMetadata>? Accepts,
    EquatableImmutableArray<ProducesMetadata>? Produces,
    EquatableImmutableArray<ProducesProblemMetadata>? ProducesProblem,
    EquatableImmutableArray<ProducesValidationProblemMetadata>? ProducesValidationProblem,
    bool ExcludeFromDescription
);
