using GeneratedEndpoints.Tests.Common;
using SourceGeneratorTestHelpers.XUnit;

namespace GeneratedEndpoints.Tests;

[UsesVerify]
public class GeneratedSourceTests
{
    public GeneratedSourceTests()
    {
        ModuleInitializer.Initialize();
    }

    [Theory]
    [InlineData(true, true, true, "/custom-fallback")]
    [InlineData(false, true, false, null)]
    [InlineData(true, false, true, "/alternate-fallback")]
    [InlineData(false, false, true, "/custom-only")]
    [InlineData(true, false, false, null)]
    public async Task MapFallbackScenarios(bool withNamespace, bool includeDefaultFallback, bool includeCustomFallback, string? customRoute)
    {
        var sources = TestHelpers.GetSources(SourceFactory.BuildFallbackSource(includeDefaultFallback, includeCustomFallback, customRoute), withNamespace);
        var result = TestHelpers.RunGenerator(sources);
        var scenario = ScenarioNamer.Create(nameof(MapFallbackScenarios), ("Namespace", withNamespace), ("Default", includeDefaultFallback),
            ("Custom", includeCustomFallback), ("Route", customRoute ?? "default")
        );

        await result.VerifyAsync("AddEndpointHandlers.g.cs")
            .UseMethodName($"{scenario}_AddEndpointHandlers");

        await result.VerifyAsync("MapEndpointHandlers.g.cs")
            .UseMethodName($"{scenario}_MapEndpointHandlers");
    }

    [Theory]
    [InlineData(true, true, false, true, false, true, true, "*.contoso.com", "api.contoso.com", true, "NamedCorsPolicy", false, null, true, "RatePolicy", true,
        true, "TimeoutPolicy", false, 5, "Reporting", true
    )]
    [InlineData(false, false, true, false, true, false, true, null, "services.contoso.com", false, null, true, "MethodCors", true, null, false, false, null,
        true, -1, null, false
    )]
    [InlineData(true, true, true, true, true, true, false, "*.example.com", null, true, null, true, null, false, null, false, true, null, true, 0, "Operations",
        true
    )]
    [InlineData(false, false, false, true, false, true, false, null, "*.alt.com", false, "CorsDefault", false, null, false, null, true, false, null, false, 10,
        null, false
    )]
    [InlineData(true, false, true, false, true, false, true, "api.alt.com", null, true, null, true, "MethodCors", true, "BurstPolicy", false, true,
        "TimeoutPolicy", true, -5, "Docs", true
    )]
    public async Task AuthorizationAndMetadataMatrix(
        bool withNamespace,
        bool classAllowAnonymous,
        bool methodAllowAnonymous,
        bool classRequireAuthorization,
        bool methodRequireAuthorization,
        bool classTags,
        bool methodTags,
        string? classHost,
        string? methodHost,
        bool classRequireCors,
        string? classCorsPolicy,
        bool methodRequireCors,
        string? methodCorsPolicy,
        bool requireRateLimiting,
        string? rateLimitingPolicy,
        bool applyShortCircuit,
        bool applyRequestTimeout,
        string? requestTimeoutPolicy,
        bool disableRequestTimeout,
        int orderValue,
        string? groupName,
        bool excludeFromDescription
    )
    {
        var source = SourceFactory.BuildAuthorizationMatrixSource(classAllowAnonymous, methodAllowAnonymous, classRequireAuthorization,
            methodRequireAuthorization, classTags, methodTags, classHost, methodHost, classRequireCors, classCorsPolicy, methodRequireCors, methodCorsPolicy,
            requireRateLimiting, rateLimitingPolicy, applyShortCircuit, applyRequestTimeout, requestTimeoutPolicy, disableRequestTimeout, orderValue, groupName,
            excludeFromDescription
        );

        var sources = TestHelpers.GetSources(source, withNamespace);
        var result = TestHelpers.RunGenerator(sources);
        var scenario = ScenarioNamer.Create(nameof(AuthorizationAndMetadataMatrix), ("Namespace", withNamespace), ("ClassAnon", classAllowAnonymous),
            ("MethodAnon", methodAllowAnonymous), ("ClassAuth", classRequireAuthorization), ("MethodAuth", methodRequireAuthorization),
            ("ClassTags", classTags), ("MethodTags", methodTags), ("ClassHost", classHost ?? "none"), ("MethodHost", methodHost ?? "none"),
            ("ClassCors", classRequireCors), ("MethodCors", methodRequireCors), ("RateLimit", requireRateLimiting), ("ShortCircuit", applyShortCircuit),
            ("RequestTimeout", applyRequestTimeout), ("DisableTimeout", disableRequestTimeout), ("Order", orderValue), ("Group", groupName ?? "none"),
            ("Exclude", excludeFromDescription)
        );

        await result.VerifyAsync("AddEndpointHandlers.g.cs")
            .UseMethodName($"{scenario}_AddEndpointHandlers");

        await result.VerifyAsync("MapEndpointHandlers.g.cs")
            .UseMethodName($"{scenario}_MapEndpointHandlers");
    }

    [Theory]
    [InlineData(true, true, true, true, true, true, true, "configured")]
    [InlineData(false, false, true, false, true, false, true, "timed")]
    [InlineData(true, true, false, true, false, true, false, "metadata")]
    [InlineData(false, true, false, false, false, true, false, "analytics")]
    [InlineData(true, false, false, true, false, false, true, "telemetry")]
    public async Task ConfigureAndFiltersMatrix(
        bool withNamespace,
        bool configureWithServiceProvider,
        bool configureAddsMetadata,
        bool includeClassLevelFilter,
        bool includeMethodLevelFilter,
        bool includeGenericFilter,
        bool configureRegistersFilter,
        string metadataValue
    )
    {
        var source = SourceFactory.BuildConfigureAndFiltersSource(configureWithServiceProvider, configureAddsMetadata, includeClassLevelFilter,
            includeMethodLevelFilter, includeGenericFilter, configureRegistersFilter, metadataValue
        );

        var sources = TestHelpers.GetSources(source, withNamespace);
        var result = TestHelpers.RunGenerator(sources);
        var scenario = ScenarioNamer.Create(nameof(ConfigureAndFiltersMatrix), ("Namespace", withNamespace), ("SvcProvider", configureWithServiceProvider),
            ("Metadata", configureAddsMetadata), ("ClassFilter", includeClassLevelFilter), ("MethodFilter", includeMethodLevelFilter),
            ("GenericFilter", includeGenericFilter), ("ConfigureFilter", configureRegistersFilter), ("Value", metadataValue)
        );

        await result.VerifyAsync("AddEndpointHandlers.g.cs")
            .UseMethodName($"{scenario}_AddEndpointHandlers");

        await result.VerifyAsync("MapEndpointHandlers.g.cs")
            .UseMethodName($"{scenario}_MapEndpointHandlers");
    }

    [Theory]
    [InlineData(true, true, true, true, true, true, true, true, true, true, true, false)]
    [InlineData(false, true, false, false, true, false, true, false, true, false, true, true)]
    [InlineData(true, false, true, true, false, true, false, true, false, true, false, true)]
    [InlineData(false, false, true, true, false, true, false, true, false, true, false, false)]
    [InlineData(true, true, false, false, true, false, true, false, true, false, true, true)]
    public async Task HttpMethodMatrix(
        bool withNamespace,
        bool includeGet,
        bool includePost,
        bool includePut,
        bool includeDelete,
        bool includeOptions,
        bool includeHead,
        bool includePatch,
        bool includeQuery,
        bool includeTrace,
        bool includeConnect,
        bool includeMethodNameCollision
    )
    {
        var source = SourceFactory.BuildHttpMethodMatrixSource(includeGet, includePost, includePut, includeDelete, includeOptions, includeHead, includePatch,
            includeQuery, includeTrace, includeConnect, includeMethodNameCollision
        );

        var sources = TestHelpers.GetSources(source, withNamespace);
        var result = TestHelpers.RunGenerator(sources);
        var scenario = ScenarioNamer.Create(nameof(HttpMethodMatrix), ("Namespace", withNamespace), ("Get", includeGet), ("Post", includePost),
            ("Put", includePut), ("Delete", includeDelete), ("Options", includeOptions), ("Head", includeHead), ("Patch", includePatch),
            ("Query", includeQuery), ("Trace", includeTrace), ("Connect", includeConnect), ("Collision", includeMethodNameCollision)
        );

        await result.VerifyAsync("AddEndpointHandlers.g.cs")
            .UseMethodName($"{scenario}_AddEndpointHandlers");

        await result.VerifyAsync("MapEndpointHandlers.g.cs")
            .UseMethodName($"{scenario}_MapEndpointHandlers");
    }

    [Theory]
    [InlineData(true, true, true, true, true, true, true, true, true, true, true, true, true, true, false, true, false, "application/xml", "text/xml",
        "application/json", "text/json"
    )]
    [InlineData(false, false, true, false, false, true, false, true, false, true, false, false, true, true, false, true, false, "application/custom", null,
        "application/problem+json", null
    )]
    [InlineData(true, true, false, true, true, false, true, false, false, false, true, true, true, false, false, true, true, null, null, null, null)]
    [InlineData(false, true, false, true, false, true, false, true, false, true, false, false, true, false, true, false, true, "application/xml", null,
        "application/json", null
    )]
    [InlineData(true, false, true, false, true, false, true, false, true, false, true, false, true, true, false, true, false, null, "text/plain", null,
        "text/plain"
    )]
    public async Task ContractsAndBindingMatrix(
        bool withNamespace,
        bool includeBindingNames,
        bool includeAsParameters,
        bool includeFromServices,
        bool includeFromKeyedServices,
        bool includeAccepts,
        bool includeGenericAccepts,
        bool includeProducesResponse,
        bool includeGenericProducesResponse,
        bool includeProducesProblem,
        bool includeProducesValidationProblem,
        bool includeSummaryAndDescription,
        bool includeDisplayName,
        bool includeTags,
        bool excludeFromDescription,
        bool allowAnonymous,
        bool methodRequiresAuthorization,
        string? acceptsContentType1,
        string? acceptsContentType2,
        string? producesContentType1,
        string? producesContentType2
    )
    {
        var source = SourceFactory.BuildContractsAndBindingSource(includeBindingNames, includeAsParameters, includeFromServices, includeFromKeyedServices,
            includeAccepts, includeGenericAccepts, includeProducesResponse, includeGenericProducesResponse, includeProducesProblem,
            includeProducesValidationProblem, includeSummaryAndDescription, includeDisplayName, includeTags, excludeFromDescription, allowAnonymous,
            methodRequiresAuthorization, acceptsContentType1, acceptsContentType2, producesContentType1, producesContentType2
        );

        var sources = TestHelpers.GetSources(source, withNamespace);
        var result = TestHelpers.RunGenerator(sources);
        var scenario = ScenarioNamer.Create(nameof(ContractsAndBindingMatrix), ("Namespace", withNamespace), ("BindingNames", includeBindingNames),
            ("AsParameters", includeAsParameters), ("Services", includeFromServices), ("KeyedServices", includeFromKeyedServices), ("Accepts", includeAccepts),
            ("GenericAccepts", includeGenericAccepts), ("Produces", includeProducesResponse), ("Problem", includeProducesProblem),
            ("Validation", includeProducesValidationProblem), ("Summary", includeSummaryAndDescription), ("DisplayName", includeDisplayName),
            ("Tags", includeTags), ("Exclude", excludeFromDescription), ("AllowAnon", allowAnonymous), ("MethodAuth", methodRequiresAuthorization)
        );

        await result.VerifyAsync("AddEndpointHandlers.g.cs")
            .UseMethodName($"{scenario}_AddEndpointHandlers");

        await result.VerifyAsync("MapEndpointHandlers.g.cs")
            .UseMethodName($"{scenario}_MapEndpointHandlers");
    }
}
