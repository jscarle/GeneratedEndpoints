namespace GeneratedEndpoints.Common;

internal readonly record struct EndpointConfiguration(
    RequestHandlerMetadata Metadata,
    bool RequireAuthorization,
    EquatableImmutableArray<string>? AuthorizationPolicies,
    bool DisableAntiforgery,
    bool AllowAnonymous,
    bool RequireCors,
    string? CorsPolicyName,
    EquatableImmutableArray<string>? RequiredHosts,
    bool RequireRateLimiting,
    string? RateLimitingPolicyName,
    EquatableImmutableArray<string>? EndpointFilterTypes,
    bool ShortCircuit,
    bool DisableValidation,
    bool DisableRequestTimeout,
    bool WithRequestTimeout,
    string? RequestTimeoutPolicyName,
    int? Order,
    string? EndpointGroupName
);
