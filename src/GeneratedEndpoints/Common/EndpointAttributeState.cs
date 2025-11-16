namespace GeneratedEndpoints.Common;

internal struct EndpointAttributeState
{
    public EquatableImmutableArray<string>? Tags;
    public bool? RequireAuthorization;
    public EquatableImmutableArray<string>? AuthorizationPolicies;
    public bool? DisableAntiforgery;
    public bool? AllowAnonymous;
    public bool? ExcludeFromDescription;
    public List<AcceptsMetadata>? Accepts;
    public List<ProducesMetadata>? Produces;
    public List<ProducesProblemMetadata>? ProducesProblem;
    public List<ProducesValidationProblemMetadata>? ProducesValidationProblem;
    public bool? RequireCors;
    public string? CorsPolicyName;
    public EquatableImmutableArray<string>? RequiredHosts;
    public bool? RequireRateLimiting;
    public string? RateLimitingPolicyName;
    public List<string>? EndpointFilters;
    public HashSet<string>? EndpointFilterSet;
    public bool HasAllowAnonymousAttribute;
    public bool HasRequireAuthorizationAttribute;
    public bool? ShortCircuit;
    public bool? DisableValidation;
    public bool? DisableRequestTimeout;
    public bool? WithRequestTimeout;
    public string? RequestTimeoutPolicyName;
    public int? Order;
    public string? EndpointGroupName;
    public string? Summary;
}
