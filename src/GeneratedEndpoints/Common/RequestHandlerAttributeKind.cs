namespace GeneratedEndpoints.Common;

internal enum RequestHandlerAttributeKind
{
    None = 0,
    ShortCircuit,
    DisableValidation,
    DisableRequestTimeout,
    RequestTimeout,
    Order,
    MapGroup,
    Summary,
    Accepts,
    ProducesResponse,
    RequireAuthorization,
    RequireCors,
    RequireHost,
    RequireRateLimiting,
    EndpointFilter,
    DisableAntiforgery,
    ProducesProblem,
    ProducesValidationProblem,
    DisplayName,
    Description,
    AllowAnonymous,
    Tags,
    ExcludeFromDescription
}
