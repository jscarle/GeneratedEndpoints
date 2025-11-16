using System.Security.Cryptography;
using System.Text;
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
        var scenario = ScenarioNamer.Create(nameof(MapFallbackScenarios),
            ("Namespace", withNamespace),
            ("Default", includeDefaultFallback),
            ("Custom", includeCustomFallback),
            ("Route", customRoute ?? "default"));

        await result.VerifyAsync("AddEndpointHandlers.g.cs")
            .UseMethodName($"{scenario}_AddEndpointHandlers");

        await result.VerifyAsync("MapEndpointHandlers.g.cs")
            .UseMethodName($"{scenario}_MapEndpointHandlers");
    }

    [Theory]
    [InlineData(true, true, false, true, false, true, true, "*.contoso.com", "api.contoso.com", true, "NamedCorsPolicy", false, null, true, "RatePolicy", true, true, "TimeoutPolicy", false, 5, "Reporting", true)]
    [InlineData(false, false, true, false, true, false, true, null, "services.contoso.com", false, null, true, "MethodCors", true, null, false, false, null, true, -1, null, false)]
    [InlineData(true, true, true, true, true, true, false, "*.example.com", null, true, null, true, null, false, null, false, true, null, true, 0, "Operations", true)]
    [InlineData(false, false, false, true, false, true, false, null, "*.alt.com", false, "CorsDefault", false, null, false, null, true, false, null, false, 10, null, false)]
    [InlineData(true, false, true, false, true, false, true, "api.alt.com", null, true, null, true, "MethodCors", true, "BurstPolicy", false, true, "TimeoutPolicy", true, -5, "Docs", true)]
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
        bool excludeFromDescription)
    {
        var source = SourceFactory.BuildAuthorizationMatrixSource(
            classAllowAnonymous,
            methodAllowAnonymous,
            classRequireAuthorization,
            methodRequireAuthorization,
            classTags,
            methodTags,
            classHost,
            methodHost,
            classRequireCors,
            classCorsPolicy,
            methodRequireCors,
            methodCorsPolicy,
            requireRateLimiting,
            rateLimitingPolicy,
            applyShortCircuit,
            applyRequestTimeout,
            requestTimeoutPolicy,
            disableRequestTimeout,
            orderValue,
            groupName,
            excludeFromDescription);

        var sources = TestHelpers.GetSources(source, withNamespace);
        var result = TestHelpers.RunGenerator(sources);
        var scenario = ScenarioNamer.Create(nameof(AuthorizationAndMetadataMatrix),
            ("Namespace", withNamespace),
            ("ClassAnon", classAllowAnonymous),
            ("MethodAnon", methodAllowAnonymous),
            ("ClassAuth", classRequireAuthorization),
            ("MethodAuth", methodRequireAuthorization),
            ("ClassTags", classTags),
            ("MethodTags", methodTags),
            ("ClassHost", classHost ?? "none"),
            ("MethodHost", methodHost ?? "none"),
            ("ClassCors", classRequireCors),
            ("MethodCors", methodRequireCors),
            ("RateLimit", requireRateLimiting),
            ("ShortCircuit", applyShortCircuit),
            ("RequestTimeout", applyRequestTimeout),
            ("DisableTimeout", disableRequestTimeout),
            ("Order", orderValue),
            ("Group", groupName ?? "none"),
            ("Exclude", excludeFromDescription));

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
        string metadataValue)
    {
        var source = SourceFactory.BuildConfigureAndFiltersSource(
            configureWithServiceProvider,
            configureAddsMetadata,
            includeClassLevelFilter,
            includeMethodLevelFilter,
            includeGenericFilter,
            configureRegistersFilter,
            metadataValue);

        var sources = TestHelpers.GetSources(source, withNamespace);
        var result = TestHelpers.RunGenerator(sources);
        var scenario = ScenarioNamer.Create(nameof(ConfigureAndFiltersMatrix),
            ("Namespace", withNamespace),
            ("SvcProvider", configureWithServiceProvider),
            ("Metadata", configureAddsMetadata),
            ("ClassFilter", includeClassLevelFilter),
            ("MethodFilter", includeMethodLevelFilter),
            ("GenericFilter", includeGenericFilter),
            ("ConfigureFilter", configureRegistersFilter),
            ("Value", metadataValue));

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
        bool includeMethodNameCollision)
    {
        var source = SourceFactory.BuildHttpMethodMatrixSource(
            includeGet,
            includePost,
            includePut,
            includeDelete,
            includeOptions,
            includeHead,
            includePatch,
            includeQuery,
            includeTrace,
            includeConnect,
            includeMethodNameCollision);

        var sources = TestHelpers.GetSources(source, withNamespace);
        var result = TestHelpers.RunGenerator(sources);
        var scenario = ScenarioNamer.Create(nameof(HttpMethodMatrix),
            ("Namespace", withNamespace),
            ("Get", includeGet),
            ("Post", includePost),
            ("Put", includePut),
            ("Delete", includeDelete),
            ("Options", includeOptions),
            ("Head", includeHead),
            ("Patch", includePatch),
            ("Query", includeQuery),
            ("Trace", includeTrace),
            ("Connect", includeConnect),
            ("Collision", includeMethodNameCollision));

        await result.VerifyAsync("AddEndpointHandlers.g.cs")
            .UseMethodName($"{scenario}_AddEndpointHandlers");

        await result.VerifyAsync("MapEndpointHandlers.g.cs")
            .UseMethodName($"{scenario}_MapEndpointHandlers");
    }

    [Theory]
    [InlineData(true, true, true, true, true, true, true, true, true, true, true, true, true, false, true, false, "application/xml", "text/xml", "application/json", "text/json")]
    [InlineData(false, false, true, false, false, true, false, true, true, false, false, false, true, true, false, true, "application/custom", null, "application/problem+json", null)]
    [InlineData(true, true, false, true, true, false, true, false, false, true, true, true, false, false, true, true, null, null, null, null)]
    [InlineData(false, true, false, true, false, true, false, true, false, true, false, true, false, true, false, true, "application/xml", null, "application/json", null)]
    [InlineData(true, false, true, false, true, false, true, false, true, false, true, false, true, false, true, false, null, "text/plain", null, "text/plain")]
    public async Task ContractsAndBindingMatrix(
        bool withNamespace,
        bool includeBindingNames,
        bool includeAsParameters,
        bool includeFromServices,
        bool includeFromKeyedServices,
        bool includeAccepts,
        bool includeGenericAccepts,
        bool includeProducesResponse,
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
        string? producesContentType2)
    {
        var source = SourceFactory.BuildContractsAndBindingSource(
            includeBindingNames,
            includeAsParameters,
            includeFromServices,
            includeFromKeyedServices,
            includeAccepts,
            includeGenericAccepts,
            includeProducesResponse,
            includeProducesProblem,
            includeProducesValidationProblem,
            includeSummaryAndDescription,
            includeDisplayName,
            includeTags,
            excludeFromDescription,
            allowAnonymous,
            methodRequiresAuthorization,
            acceptsContentType1,
            acceptsContentType2,
            producesContentType1,
            producesContentType2);

        var sources = TestHelpers.GetSources(source, withNamespace);
        var result = TestHelpers.RunGenerator(sources);
        var scenario = ScenarioNamer.Create(nameof(ContractsAndBindingMatrix),
            ("Namespace", withNamespace),
            ("BindingNames", includeBindingNames),
            ("AsParameters", includeAsParameters),
            ("Services", includeFromServices),
            ("KeyedServices", includeFromKeyedServices),
            ("Accepts", includeAccepts),
            ("GenericAccepts", includeGenericAccepts),
            ("Produces", includeProducesResponse),
            ("Problem", includeProducesProblem),
            ("Validation", includeProducesValidationProblem),
            ("Summary", includeSummaryAndDescription),
            ("DisplayName", includeDisplayName),
            ("Tags", includeTags),
            ("Exclude", excludeFromDescription),
            ("AllowAnon", allowAnonymous),
            ("MethodAuth", methodRequiresAuthorization));

        await result.VerifyAsync("AddEndpointHandlers.g.cs")
            .UseMethodName($"{scenario}_AddEndpointHandlers");

        await result.VerifyAsync("MapEndpointHandlers.g.cs")
            .UseMethodName($"{scenario}_MapEndpointHandlers");
    }
}

[UsesVerify]
public class IndividualTests
{
    public IndividualTests()
    {
        ModuleInitializer.Initialize();
    }

    [Fact]
    public async Task DefaultFallbackOnly()
    {
        var source = FallbackScenario(includeDefault: true);
        await VerifyIndividualAsync(source, nameof(DefaultFallbackOnly));
    }

    [Fact]
    public async Task CustomFallbackRoute()
    {
        var source = FallbackScenario(includeCustom: true, customRoute: "/custom-individual");
        await VerifyIndividualAsync(source, nameof(CustomFallbackRoute));
    }

    [Fact]
    public async Task ClassAllowAnonymous()
    {
        var source = AuthorizationScenario(classAllowAnonymous: true);
        await VerifyIndividualAsync(source, nameof(ClassAllowAnonymous));
    }

    [Fact]
    public async Task MethodAllowAnonymous()
    {
        var source = AuthorizationScenario(methodAllowAnonymous: true);
        await VerifyIndividualAsync(source, nameof(MethodAllowAnonymous));
    }

    [Fact]
    public async Task ClassRequireAuthorization()
    {
        var source = AuthorizationScenario(classRequireAuthorization: true);
        await VerifyIndividualAsync(source, nameof(ClassRequireAuthorization));
    }

    [Fact]
    public async Task MethodRequireAuthorization()
    {
        var source = AuthorizationScenario(methodRequireAuthorization: true);
        await VerifyIndividualAsync(source, nameof(MethodRequireAuthorization));
    }

    [Fact]
    public async Task ClassTags()
    {
        var source = AuthorizationScenario(classTags: true);
        await VerifyIndividualAsync(source, nameof(ClassTags));
    }

    [Fact]
    public async Task MethodTags()
    {
        var source = AuthorizationScenario(methodTags: true);
        await VerifyIndividualAsync(source, nameof(MethodTags));
    }

    [Fact]
    public async Task ClassRequireHost()
    {
        var source = AuthorizationScenario(classHost: "*.individual.com");
        await VerifyIndividualAsync(source, nameof(ClassRequireHost));
    }

    [Fact]
    public async Task MethodRequireHost()
    {
        var source = AuthorizationScenario(methodHost: "api.individual.com");
        await VerifyIndividualAsync(source, nameof(MethodRequireHost));
    }

    [Fact]
    public async Task ClassRequireCors()
    {
        var source = AuthorizationScenario(classRequireCors: true);
        await VerifyIndividualAsync(source, nameof(ClassRequireCors));
    }

    [Fact]
    public async Task ClassRequireCorsWithPolicy()
    {
        var source = AuthorizationScenario(classRequireCors: true, classCorsPolicy: "ClassPolicy");
        await VerifyIndividualAsync(source, nameof(ClassRequireCorsWithPolicy));
    }

    [Fact]
    public async Task MethodRequireCors()
    {
        var source = AuthorizationScenario(methodRequireCors: true);
        await VerifyIndividualAsync(source, nameof(MethodRequireCors));
    }

    [Fact]
    public async Task MethodRequireCorsWithPolicy()
    {
        var source = AuthorizationScenario(methodRequireCors: true, methodCorsPolicy: "MethodPolicy");
        await VerifyIndividualAsync(source, nameof(MethodRequireCorsWithPolicy));
    }

    [Fact]
    public async Task RequireRateLimiting()
    {
        var source = AuthorizationScenario(requireRateLimiting: true);
        await VerifyIndividualAsync(source, nameof(RequireRateLimiting));
    }

    [Fact]
    public async Task RequireRateLimitingWithPolicy()
    {
        var source = AuthorizationScenario(requireRateLimiting: true, rateLimitingPolicy: "BurstPolicy");
        await VerifyIndividualAsync(source, nameof(RequireRateLimitingWithPolicy));
    }

    [Fact]
    public async Task ShortCircuit()
    {
        var source = AuthorizationScenario(applyShortCircuit: true);
        await VerifyIndividualAsync(source, nameof(ShortCircuit));
    }

    [Fact]
    public async Task RequestTimeout()
    {
        var source = AuthorizationScenario(applyRequestTimeout: true);
        await VerifyIndividualAsync(source, nameof(RequestTimeout));
    }

    [Fact]
    public async Task RequestTimeoutWithPolicy()
    {
        var source = AuthorizationScenario(applyRequestTimeout: true, requestTimeoutPolicy: "TimeoutPolicy");
        await VerifyIndividualAsync(source, nameof(RequestTimeoutWithPolicy));
    }

    [Fact]
    public async Task DisableRequestTimeout()
    {
        var source = AuthorizationScenario(disableRequestTimeout: true);
        await VerifyIndividualAsync(source, nameof(DisableRequestTimeout));
    }

    [Fact]
    public async Task OrderMetadata()
    {
        var source = AuthorizationScenario(orderValue: 7);
        await VerifyIndividualAsync(source, nameof(OrderMetadata));
    }

    [Fact]
    public async Task GroupName()
    {
        var source = AuthorizationScenario(groupName: "IndividualGroup");
        await VerifyIndividualAsync(source, nameof(GroupName));
    }

    [Fact]
    public async Task ExcludeFromDescription()
    {
        var source = AuthorizationScenario(excludeFromDescription: true);
        await VerifyIndividualAsync(source, nameof(ExcludeFromDescription));
    }

    [Fact]
    public async Task ClassEndpointFilter()
    {
        var source = ConfigureScenario(includeClassLevelFilter: true);
        await VerifyIndividualAsync(source, nameof(ClassEndpointFilter));
    }

    [Fact]
    public async Task MethodEndpointFilter()
    {
        var source = ConfigureScenario(includeMethodLevelFilter: true);
        await VerifyIndividualAsync(source, nameof(MethodEndpointFilter));
    }

    [Fact]
    public async Task GenericEndpointFilter()
    {
        var source = ConfigureScenario(includeGenericFilter: true);
        await VerifyIndividualAsync(source, nameof(GenericEndpointFilter));
    }

    [Fact]
    public async Task ConfigureWithServiceProvider()
    {
        var source = ConfigureScenario(configureWithServiceProvider: true);
        await VerifyIndividualAsync(source, nameof(ConfigureWithServiceProvider));
    }

    [Fact]
    public async Task ConfigureAddsMetadata()
    {
        var source = ConfigureScenario(configureAddsMetadata: true, metadataValue: "IndividualMetadata");
        await VerifyIndividualAsync(source, nameof(ConfigureAddsMetadata));
    }

    [Fact]
    public async Task ConfigureRegistersFilter()
    {
        var source = ConfigureScenario(configureRegistersFilter: true);
        await VerifyIndividualAsync(source, nameof(ConfigureRegistersFilter));
    }

    [Fact]
    public async Task MapGetEndpoint()
    {
        var source = HttpMethodScenario(includeGet: true);
        await VerifyIndividualAsync(source, nameof(MapGetEndpoint));
    }

    [Fact]
    public async Task MapPostEndpoint()
    {
        var source = HttpMethodScenario(includePost: true);
        await VerifyIndividualAsync(source, nameof(MapPostEndpoint));
    }

    [Fact]
    public async Task MapPutEndpoint()
    {
        var source = HttpMethodScenario(includePut: true);
        await VerifyIndividualAsync(source, nameof(MapPutEndpoint));
    }

    [Fact]
    public async Task MapDeleteEndpoint()
    {
        var source = HttpMethodScenario(includeDelete: true);
        await VerifyIndividualAsync(source, nameof(MapDeleteEndpoint));
    }

    [Fact]
    public async Task MapOptionsEndpoint()
    {
        var source = HttpMethodScenario(includeOptions: true);
        await VerifyIndividualAsync(source, nameof(MapOptionsEndpoint));
    }

    [Fact]
    public async Task MapHeadEndpoint()
    {
        var source = HttpMethodScenario(includeHead: true);
        await VerifyIndividualAsync(source, nameof(MapHeadEndpoint));
    }

    [Fact]
    public async Task MapPatchEndpoint()
    {
        var source = HttpMethodScenario(includePatch: true);
        await VerifyIndividualAsync(source, nameof(MapPatchEndpoint));
    }

    [Fact]
    public async Task MapQueryEndpoint()
    {
        var source = HttpMethodScenario(includeQuery: true);
        await VerifyIndividualAsync(source, nameof(MapQueryEndpoint));
    }

    [Fact]
    public async Task MapTraceEndpoint()
    {
        var source = HttpMethodScenario(includeTrace: true);
        await VerifyIndividualAsync(source, nameof(MapTraceEndpoint));
    }

    [Fact]
    public async Task MapConnectEndpoint()
    {
        var source = HttpMethodScenario(includeConnect: true);
        await VerifyIndividualAsync(source, nameof(MapConnectEndpoint));
    }

    [Fact]
    public async Task MethodNameCollision()
    {
        var source = HttpMethodScenario(includeGet: true, includeMethodNameCollision: true);
        await VerifyIndividualAsync(source, nameof(MethodNameCollision));
    }

    [Fact]
    public async Task BindingNames()
    {
        var source = ContractScenario(includeBindingNames: true);
        await VerifyIndividualAsync(source, nameof(BindingNames));
    }

    [Fact]
    public async Task AsParameters()
    {
        var source = ContractScenario(includeAsParameters: true);
        await VerifyIndividualAsync(source, nameof(AsParameters));
    }

    [Fact]
    public async Task FromServices()
    {
        var source = ContractScenario(includeFromServices: true);
        await VerifyIndividualAsync(source, nameof(FromServices));
    }

    [Fact]
    public async Task FromKeyedServices()
    {
        var source = ContractScenario(includeFromKeyedServices: true);
        await VerifyIndividualAsync(source, nameof(FromKeyedServices));
    }

    [Fact]
    public async Task AcceptsAttribute()
    {
        var source = ContractScenario(includeAccepts: true, acceptsContentType1: "application/custom");
        await VerifyIndividualAsync(source, nameof(AcceptsAttribute));
    }

    [Fact]
    public async Task AcceptsMultipleContentTypes()
    {
        var source = ContractScenario(includeAccepts: true, acceptsContentType1: "application/json", acceptsContentType2: "text/json");
        await VerifyIndividualAsync(source, nameof(AcceptsMultipleContentTypes));
    }

    [Fact]
    public async Task GenericAcceptsAttribute()
    {
        var source = ContractScenario(includeGenericAccepts: true, acceptsContentType1: "application/vnd.generic");
        await VerifyIndividualAsync(source, nameof(GenericAcceptsAttribute));
    }

    [Fact]
    public async Task ProducesResponseAttribute()
    {
        var source = ContractScenario(includeProducesResponse: true, producesContentType1: "application/json");
        await VerifyIndividualAsync(source, nameof(ProducesResponseAttribute));
    }

    [Fact]
    public async Task ProducesResponseMultipleContentTypes()
    {
        var source = ContractScenario(includeProducesResponse: true, producesContentType1: "application/json", producesContentType2: "text/json");
        await VerifyIndividualAsync(source, nameof(ProducesResponseMultipleContentTypes));
    }

    [Fact]
    public async Task ProducesProblemAttribute()
    {
        var source = ContractScenario(includeProducesProblem: true, producesContentType1: "application/problem+json");
        await VerifyIndividualAsync(source, nameof(ProducesProblemAttribute));
    }

    [Fact]
    public async Task ProducesValidationProblemAttribute()
    {
        var source = ContractScenario(includeProducesValidationProblem: true);
        await VerifyIndividualAsync(source, nameof(ProducesValidationProblemAttribute));
    }

    [Fact]
    public async Task SummaryAndDescriptionAttributes()
    {
        var source = ContractScenario(includeSummaryAndDescription: true);
        await VerifyIndividualAsync(source, nameof(SummaryAndDescriptionAttributes));
    }

    [Fact]
    public async Task DisplayNameAttribute()
    {
        var source = ContractScenario(includeDisplayName: true);
        await VerifyIndividualAsync(source, nameof(DisplayNameAttribute));
    }

    [Fact]
    public async Task ContractTags()
    {
        var source = ContractScenario(includeTags: true);
        await VerifyIndividualAsync(source, nameof(ContractTags));
    }

    [Fact]
    public async Task ContractExcludeFromDescription()
    {
        var source = ContractScenario(excludeFromDescription: true);
        await VerifyIndividualAsync(source, nameof(ContractExcludeFromDescription));
    }

    [Fact]
    public async Task ContractAllowAnonymous()
    {
        var source = ContractScenario(allowAnonymous: true);
        await VerifyIndividualAsync(source, nameof(ContractAllowAnonymous));
    }

    [Fact]
    public async Task ContractRequireAuthorization()
    {
        var source = ContractScenario(methodRequiresAuthorization: true);
        await VerifyIndividualAsync(source, nameof(ContractRequireAuthorization));
    }

    private static async Task VerifyIndividualAsync(string source, string scenario, bool withNamespace = true)
    {
        var sources = TestHelpers.GetSources(source, withNamespace);
        var result = TestHelpers.RunGenerator(sources);

        await result.VerifyAsync("AddEndpointHandlers.g.cs")
            .UseMethodName($"{scenario}_AddEndpointHandlers");

        await result.VerifyAsync("MapEndpointHandlers.g.cs")
            .UseMethodName($"{scenario}_MapEndpointHandlers");
    }

    private static string FallbackScenario(bool includeDefault = false, bool includeCustom = false, string? customRoute = null)
        => SourceFactory.BuildFallbackSource(includeDefault, includeCustom, customRoute);

    private static string AuthorizationScenario(
        bool classAllowAnonymous = false,
        bool methodAllowAnonymous = false,
        bool classRequireAuthorization = false,
        bool methodRequireAuthorization = false,
        bool classTags = false,
        bool methodTags = false,
        string? classHost = null,
        string? methodHost = null,
        bool classRequireCors = false,
        string? classCorsPolicy = null,
        bool methodRequireCors = false,
        string? methodCorsPolicy = null,
        bool requireRateLimiting = false,
        string? rateLimitingPolicy = null,
        bool applyShortCircuit = false,
        bool applyRequestTimeout = false,
        string? requestTimeoutPolicy = null,
        bool disableRequestTimeout = false,
        int orderValue = 0,
        string? groupName = null,
        bool excludeFromDescription = false)
        => SourceFactory.BuildAuthorizationMatrixSource(
            classAllowAnonymous,
            methodAllowAnonymous,
            classRequireAuthorization,
            methodRequireAuthorization,
            classTags,
            methodTags,
            classHost,
            methodHost,
            classRequireCors,
            classCorsPolicy,
            methodRequireCors,
            methodCorsPolicy,
            requireRateLimiting,
            rateLimitingPolicy,
            applyShortCircuit,
            applyRequestTimeout,
            requestTimeoutPolicy,
            disableRequestTimeout,
            orderValue,
            groupName,
            excludeFromDescription);

    private static string ConfigureScenario(
        bool configureWithServiceProvider = false,
        bool configureAddsMetadata = false,
        bool includeClassLevelFilter = false,
        bool includeMethodLevelFilter = false,
        bool includeGenericFilter = false,
        bool configureRegistersFilter = false,
        string metadataValue = "Individual")
        => SourceFactory.BuildConfigureAndFiltersSource(
            configureWithServiceProvider,
            configureAddsMetadata,
            includeClassLevelFilter,
            includeMethodLevelFilter,
            includeGenericFilter,
            configureRegistersFilter,
            metadataValue);

    private static string HttpMethodScenario(
        bool includeGet = false,
        bool includePost = false,
        bool includePut = false,
        bool includeDelete = false,
        bool includeOptions = false,
        bool includeHead = false,
        bool includePatch = false,
        bool includeQuery = false,
        bool includeTrace = false,
        bool includeConnect = false,
        bool includeMethodNameCollision = false)
        => SourceFactory.BuildHttpMethodMatrixSource(
            includeGet,
            includePost,
            includePut,
            includeDelete,
            includeOptions,
            includeHead,
            includePatch,
            includeQuery,
            includeTrace,
            includeConnect,
            includeMethodNameCollision);

    private static string ContractScenario(
        bool includeBindingNames = false,
        bool includeAsParameters = false,
        bool includeFromServices = false,
        bool includeFromKeyedServices = false,
        bool includeAccepts = false,
        bool includeGenericAccepts = false,
        bool includeProducesResponse = false,
        bool includeProducesProblem = false,
        bool includeProducesValidationProblem = false,
        bool includeSummaryAndDescription = false,
        bool includeDisplayName = false,
        bool includeTags = false,
        bool excludeFromDescription = false,
        bool allowAnonymous = false,
        bool methodRequiresAuthorization = false,
        string? acceptsContentType1 = null,
        string? acceptsContentType2 = null,
        string? producesContentType1 = null,
        string? producesContentType2 = null)
        => SourceFactory.BuildContractsAndBindingSource(
            includeBindingNames,
            includeAsParameters,
            includeFromServices,
            includeFromKeyedServices,
            includeAccepts,
            includeGenericAccepts,
            includeProducesResponse,
            includeProducesProblem,
            includeProducesValidationProblem,
            includeSummaryAndDescription,
            includeDisplayName,
            includeTags,
            excludeFromDescription,
            allowAnonymous,
            methodRequiresAuthorization,
            acceptsContentType1,
            acceptsContentType2,
            producesContentType1,
            producesContentType2);
}

file static class SourceFactory
{
    public static string BuildFallbackSource(bool includeDefault, bool includeCustom, string? customRoute)
    {
        var builder = new StringBuilder();
        builder.AppendLine("internal static class FallbackEndpoints");
        builder.AppendLine("{");

        if (includeDefault)
        {
            builder.AppendLine("    [MapFallback]");
            builder.AppendLine("    public static Ok Default() => TypedResults.Ok();");
            builder.AppendLine();
        }

        if (includeCustom)
        {
            var route = customRoute ?? "/custom";
            builder.AppendLine($"    [MapFallback(\"{route}\")]");
            builder.AppendLine("    public static Ok Custom() => TypedResults.Ok();");
            builder.AppendLine();
        }

        builder.AppendLine("}");
        return builder.ToString();
    }

    public static string BuildAuthorizationMatrixSource(
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
        bool excludeFromDescription)
    {
        var builder = new StringBuilder();

        if (classAllowAnonymous)
        {
            builder.AppendLine("[AllowAnonymous]");
        }

        if (classRequireAuthorization)
        {
            builder.AppendLine("[RequireAuthorization(\"ClassPolicy\")]");
        }

        if (classTags)
        {
            builder.AppendLine("[Tags(\"Class\", \"Matrix\")]");
        }

        if (!string.IsNullOrWhiteSpace(classHost))
        {
            builder.AppendLine($"[RequireHost(\"{classHost}\")]");
        }

        if (classRequireCors)
        {
            var cors = string.IsNullOrWhiteSpace(classCorsPolicy) ? "" : $"(\"{classCorsPolicy}\")";
            builder.AppendLine($"[RequireCors{cors}]");
        }

        if (!string.IsNullOrWhiteSpace(groupName))
        {
            builder.AppendLine($"[GroupName(\"{groupName}\")]");
        }

        if (applyShortCircuit)
        {
            builder.AppendLine("[ShortCircuit]");
        }

        if (applyRequestTimeout)
        {
            var timeoutArgument = string.IsNullOrWhiteSpace(requestTimeoutPolicy)
                ? string.Empty
                : $"(\"{requestTimeoutPolicy}\")";
            builder.AppendLine($"[RequestTimeout{timeoutArgument}]");
        }

        if (disableRequestTimeout)
        {
            builder.AppendLine("[DisableRequestTimeout]");
        }

        if (orderValue != 0)
        {
            builder.AppendLine($"[Order({orderValue})]");
        }

        if (excludeFromDescription)
        {
            builder.AppendLine("[ExcludeFromDescription]");
        }

        builder.AppendLine("internal sealed class AuthorizationMatrixEndpoints");
        builder.AppendLine("{");
        builder.AppendLine("    [MapGet(\"/matrix/{id:int}\", Name = \"GetMatrix\")]");

        if (methodAllowAnonymous)
        {
            builder.AppendLine("    [AllowAnonymous]");
        }

        if (methodRequireAuthorization)
        {
            builder.AppendLine("    [RequireAuthorization(\"MethodPolicy\")]");
        }

        if (methodTags)
        {
            builder.AppendLine("    [Tags(\"Method\", \"Matrix\")]");
        }

        if (!string.IsNullOrWhiteSpace(methodHost))
        {
            builder.AppendLine($"    [RequireHost(\"{methodHost}\", \"contoso.com\")]");
        }

        if (methodRequireCors)
        {
            var methodCors = string.IsNullOrWhiteSpace(methodCorsPolicy) ? string.Empty : $"(\"{methodCorsPolicy}\")";
            builder.AppendLine($"    [RequireCors{methodCors}]");
        }

        if (requireRateLimiting)
        {
            var rateLimit = string.IsNullOrWhiteSpace(rateLimitingPolicy) ? string.Empty : $"(\"{rateLimitingPolicy}\")";
            builder.AppendLine($"    [RequireRateLimiting{rateLimit}]");
        }

        builder.AppendLine("    public static Ok Handle(int id) => id >= 0 ? TypedResults.Ok() : TypedResults.Ok();");
        builder.AppendLine("}");
        return builder.ToString();
    }

    public static string BuildConfigureAndFiltersSource(
        bool configureWithServiceProvider,
        bool configureAddsMetadata,
        bool includeClassLevelFilter,
        bool includeMethodLevelFilter,
        bool includeGenericFilter,
        bool configureRegistersFilter,
        string metadataValue)
    {
        var builder = new StringBuilder();
        builder.AppendLine("using Microsoft.AspNetCore.Builder;");
        builder.AppendLine();

        if (includeClassLevelFilter)
        {
            builder.AppendLine("[EndpointFilter(typeof(TimingFilter))]");
        }

        builder.AppendLine("internal static class ConfigureFilterEndpoints");
        builder.AppendLine("{");
        builder.AppendLine("    [MapGet(\"/configure-filters\")]");

        if (includeMethodLevelFilter)
        {
            builder.AppendLine("    [EndpointFilter(typeof(ValidationFilter))]");
        }

        if (includeGenericFilter)
        {
            builder.AppendLine("    [EndpointFilter<ValidationFilter>]");
        }

        builder.AppendLine("    public static Ok Handle() => TypedResults.Ok();");
        builder.AppendLine();
        builder.AppendLine("    public static void Configure<TBuilder>(TBuilder builder" + (configureWithServiceProvider ? ", IServiceProvider services" : string.Empty) + ")");
        builder.AppendLine("        where TBuilder : IEndpointConventionBuilder");
        builder.AppendLine("    {");
        builder.AppendLine("        _ = builder;");

        if (configureWithServiceProvider)
        {
            builder.AppendLine("        _ = services;");
        }

        if (configureAddsMetadata)
        {
            builder.AppendLine($"        builder.WithMetadata(\"{metadataValue}\");");
        }

        if (configureRegistersFilter)
        {
            builder.AppendLine("        builder.AddEndpointFilterFactory((context, next) => next);");
        }

        builder.AppendLine("    }");
        builder.AppendLine("}");
        builder.AppendLine();
        builder.AppendLine("internal sealed class TimingFilter : IEndpointFilter");
        builder.AppendLine("{");
        builder.AppendLine("    public ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next) => next(context);");
        builder.AppendLine("}");
        builder.AppendLine();
        builder.AppendLine("internal sealed class ValidationFilter : IEndpointFilter");
        builder.AppendLine("{");
        builder.AppendLine("    public ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next) => next(context);");
        builder.AppendLine("}");

        return builder.ToString();
    }

    public static string BuildHttpMethodMatrixSource(
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
        bool includeMethodNameCollision)
    {
        var builder = new StringBuilder();
        builder.AppendLine("using Microsoft.AspNetCore.Mvc;");
        builder.AppendLine();
        builder.AppendLine("internal static class HttpMethodEndpoints");
        builder.AppendLine("{");

        if (includeGet)
        {
            builder.AppendLine("    [MapGet(\"/matrix\")] public static Ok Get() => TypedResults.Ok();");
        }

        if (includePost)
        {
            builder.AppendLine("    [MapPost(\"/matrix\")] public static Created<string> Post() => TypedResults.Created(\"/matrix/1\", \"Created\");");
        }

        if (includePut)
        {
            builder.AppendLine("    [MapPut(\"/matrix/{id:int}\")] public static Results<NoContent, NotFound> Put(int id) => id > 0 ? TypedResults.NoContent() : TypedResults.NotFound();");
        }

        if (includeDelete)
        {
            builder.AppendLine("    [MapDelete(\"/matrix/{id:int}\")] public static IResult Delete(int id) => TypedResults.Ok();");
        }

        if (includeOptions)
        {
            builder.AppendLine("    [MapOptions(\"/matrix\")] public static IResult Options() => TypedResults.Ok();");
        }

        if (includeHead)
        {
            builder.AppendLine("    [MapHead(\"/matrix\")] public static IResult Head() => TypedResults.Ok();");
        }

        if (includePatch)
        {
            builder.AppendLine("    [MapPatch(\"/matrix/{id:int}\")] public static IResult Patch(int id) => TypedResults.Ok();");
        }

        if (includeQuery)
        {
            builder.AppendLine("    [MapQuery(\"/matrix/query\")] public static IResult Query([FromQuery] string value) => TypedResults.Ok(value);");
        }

        if (includeTrace)
        {
            builder.AppendLine("    [MapTrace(\"/matrix\")] public static IResult Trace() => TypedResults.Ok();");
        }

        if (includeConnect)
        {
            builder.AppendLine("    [MapConnect(\"/matrix\")] public static IResult Connect() => TypedResults.Ok();");
        }

        builder.AppendLine("}");

        if (includeMethodNameCollision)
        {
            builder.AppendLine();
            builder.AppendLine("internal static class AlternateEndpoints");
            builder.AppendLine("{");
            builder.AppendLine("    [MapGet(\"/alternate\")] public static Ok Get() => TypedResults.Ok();");
            builder.AppendLine("    [MapPost(\"/alternate\")] public static IResult Post() => TypedResults.Ok();");
            builder.AppendLine("}");
        }

        return builder.ToString();
    }

    public static string BuildContractsAndBindingSource(
        bool includeBindingNames,
        bool includeAsParameters,
        bool includeFromServices,
        bool includeFromKeyedServices,
        bool includeAccepts,
        bool includeGenericAccepts,
        bool includeProducesResponse,
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
        string? producesContentType2)
    {
        var builder = new StringBuilder();
        builder.AppendLine("using Microsoft.AspNetCore.Mvc;");
        builder.AppendLine("using Microsoft.Extensions.DependencyInjection;");
        builder.AppendLine();
        builder.AppendLine("internal sealed class ContractEndpoints");
        builder.AppendLine("{");

        if (includeSummaryAndDescription)
        {
            builder.AppendLine("    [Summary(\"Gets detailed content.\")]");
            builder.AppendLine("    [Description(\"Shows binding and contract combinations.\")]");
        }

        if (includeDisplayName)
        {
            builder.AppendLine("    [DisplayName(\"Contract endpoint\")]");
        }

        if (includeTags)
        {
            builder.AppendLine("    [Tags(\"Contracts\", \"Bindings\")]");
        }

        if (excludeFromDescription)
        {
            builder.AppendLine("    [ExcludeFromDescription]");
        }

        if (allowAnonymous)
        {
            builder.AppendLine("    [AllowAnonymous]");
        }

        if (methodRequiresAuthorization)
        {
            builder.AppendLine("    [RequireAuthorization(\"ContractsPolicy\")]");
        }

        builder.AppendLine("    [MapGet(\"/contracts/{id:int}\")]");

        if (includeAccepts)
        {
            var secondContentType = string.IsNullOrWhiteSpace(acceptsContentType2) ? string.Empty : $", \"{acceptsContentType2}\"";
            builder.AppendLine($"    [Accepts(\"{acceptsContentType1 ?? "application/json"}\"{secondContentType})]");
        }

        if (includeGenericAccepts)
        {
            builder.AppendLine($"    [Accepts<RequestRecord>(\"{acceptsContentType1 ?? "application/json"}\")]");
        }

        if (includeProducesResponse)
        {
            var secondProduces = string.IsNullOrWhiteSpace(producesContentType2) ? string.Empty : $", \"{producesContentType2}\"";
            builder.AppendLine($"    [ProducesResponse(200, \"{producesContentType1 ?? "application/json"}\"{secondProduces}, ResponseType = typeof(ResponseRecord))]");
        }

        if (includeProducesProblem)
        {
            builder.AppendLine($"    [ProducesProblem(500, \"{producesContentType1 ?? "application/problem+json"}\")]");
        }

        if (includeProducesValidationProblem)
        {
            builder.AppendLine($"    [ProducesValidationProblem(422, \"{producesContentType1 ?? "application/problem+json"}\")]");
        }

        builder.AppendLine("    public static async Task<Results<Ok<ResponseRecord>, NotFound>> Handle(");
        builder.AppendLine(includeBindingNames
            ? "        [FromRoute(Name = \"route-id\")] int id,"
            : "        [FromRoute] int id,");
        builder.AppendLine(includeBindingNames
            ? "        [FromQuery(Name = \"filter-term\")] string? filter,"
            : "        [FromQuery] string? filter,");
        builder.AppendLine(includeBindingNames
            ? "        [FromHeader(Name = \"x-trace-id\")] string? traceId,"
            : "        [FromHeader] string? traceId,");
        builder.AppendLine("        [FromBody] RequestRecord request,");

        if (includeAsParameters)
        {
            builder.AppendLine("        [AsParameters] AdditionalParameters parameters,");
        }

        if (includeFromServices)
        {
            builder.AppendLine("        [FromServices] IServiceProvider services,");
        }

        if (includeFromKeyedServices)
        {
            builder.AppendLine("        [FromKeyedServices(\"special\")] object keyed,");
        }

        builder.AppendLine("        CancellationToken cancellationToken)");
        builder.AppendLine("    {");
        builder.AppendLine("        await Task.Yield();");
        builder.AppendLine("        cancellationToken.ThrowIfCancellationRequested();");
        builder.AppendLine("        return id > 0 ? TypedResults.Ok(new ResponseRecord(id)) : TypedResults.NotFound();");
        builder.AppendLine("    }");
        builder.AppendLine("}");
        builder.AppendLine();
        builder.AppendLine("internal sealed record RequestRecord(int Value);");
        builder.AppendLine("internal sealed record ResponseRecord(int Value);");

        if (includeAsParameters)
        {
            builder.AppendLine("internal sealed record AdditionalParameters(string? Search, int? Page);");
        }

        return builder.ToString();
    }
}

file static class ScenarioNamer
{
    public static string Create(string prefix, params (string Name, object? Value)[] parts)
    {
        var descriptor = new StringBuilder();

        foreach (var (name, value) in parts)
        {
            descriptor.Append(name);
            descriptor.Append('=');
            descriptor.Append(Sanitize(value));
            descriptor.Append(';');
        }

        using var sha256 = SHA256.Create();
        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(descriptor.ToString()));
        var hash = Convert.ToHexString(bytes.AsSpan(0, 6));
        return $"{prefix}_{hash}";
    }

    private static string Sanitize(object? value)
    {
        if (value is null)
        {
            return "None";
        }

        return value switch
        {
            bool b => b ? "On" : "Off",
            string s => s,
            _ => value.ToString() ?? "Value"
        };
    }
}
