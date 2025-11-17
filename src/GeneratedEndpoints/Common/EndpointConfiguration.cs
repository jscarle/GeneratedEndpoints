namespace GeneratedEndpoints.Common;

internal readonly record struct EndpointConfiguration
{
    public required string? DisplayName { get; init; }
    public required string? Summary { get; init; }
    public required string? Description { get; init; }
    public required EquatableImmutableArray<string>? Tags { get; init; }
    public required EquatableImmutableArray<AcceptsMetadata>? Accepts { get; init; }
    public required EquatableImmutableArray<ProducesMetadata>? Produces { get; init; }
    public required EquatableImmutableArray<ProducesProblemMetadata>? ProducesProblem { get; init; }
    public required EquatableImmutableArray<ProducesValidationProblemMetadata>? ProducesValidationProblem { get; init; }
    public required bool ExcludeFromDescription { get; init; }
    public required bool RequireAuthorization { get; init; }
    public required EquatableImmutableArray<string>? AuthorizationPolicies { get; init; }
    public required bool DisableAntiforgery { get; init; }
    public required bool AllowAnonymous { get; init; }
    public required bool RequireCors { get; init; }
    public required string? CorsPolicyName { get; init; }
    public required EquatableImmutableArray<string>? RequiredHosts { get; init; }
    public required bool RequireRateLimiting { get; init; }
    public required string? RateLimitingPolicyName { get; init; }
    public required EquatableImmutableArray<string>? EndpointFilterTypes { get; init; }
    public required bool ShortCircuit { get; init; }
    public required bool DisableValidation { get; init; }
    public required bool DisableRequestTimeout { get; init; }
    public required bool WithRequestTimeout { get; init; }
    public required string? RequestTimeoutPolicyName { get; init; }
    public required int? Order { get; init; }
    public required string? GroupIdentifier { get; init; }
    public required string? GroupPattern { get; init; }
    public required string? GroupName { get; init; }
}
