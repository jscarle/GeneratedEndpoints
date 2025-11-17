namespace GeneratedEndpoints.Common;

internal readonly record struct EndpointConfiguration
{
    public string? Name { get; init; }
    public string? DisplayName { get; init; }
    public string? Summary { get; init; }
    public string? Description { get; init; }
    public EquatableImmutableArray<string>? Tags { get; init; }
    public EquatableImmutableArray<AcceptsMetadata>? Accepts { get; init; }
    public EquatableImmutableArray<ProducesMetadata>? Produces { get; init; }
    public EquatableImmutableArray<ProducesProblemMetadata>? ProducesProblem { get; init; }
    public EquatableImmutableArray<ProducesValidationProblemMetadata>? ProducesValidationProblem { get; init; }
    public bool ExcludeFromDescription { get; init; }
    public bool RequireAuthorization { get; init; }
    public EquatableImmutableArray<string>? AuthorizationPolicies { get; init; }
    public bool DisableAntiforgery { get; init; }
    public bool AllowAnonymous { get; init; }
    public bool RequireCors { get; init; }
    public string? CorsPolicyName { get; init; }
    public EquatableImmutableArray<string>? RequiredHosts { get; init; }
    public bool RequireRateLimiting { get; init; }
    public string? RateLimitingPolicyName { get; init; }
    public EquatableImmutableArray<string>? EndpointFilterTypes { get; init; }
    public bool ShortCircuit { get; init; }
    public bool DisableValidation { get; init; }
    public bool DisableRequestTimeout { get; init; }
    public bool WithRequestTimeout { get; init; }
    public string? RequestTimeoutPolicyName { get; init; }
    public int? Order { get; init; }
    public string? EndpointGroupName { get; init; }
}
