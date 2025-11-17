using GeneratedEndpoints.Tests.Common;
using SourceGeneratorTestHelpers.XUnit;

namespace GeneratedEndpoints.Tests;

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
        var source = FallbackScenario(true);
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
        var source = AuthorizationScenario(true);
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
    public async Task ClassDisableValidation()
    {
        var source = AuthorizationScenario(classDisableValidation: true);
        await VerifyIndividualAsync(source, nameof(ClassDisableValidation));
    }

    [Fact]
    public async Task MethodDisableValidation()
    {
        var source = AuthorizationScenario(methodDisableValidation: true);
        await VerifyIndividualAsync(source, nameof(MethodDisableValidation));
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
    public async Task ClassMapGroup()
    {
        var source = AuthorizationScenario(classRequireAuthorization: true, classTags: true, classHost: "*.individual.com", classRequireCors: true,
            classCorsPolicy: "ClassCors", applyShortCircuit: true, applyRequestTimeout: true, requestTimeoutPolicy: "ClassTimeout", orderValue: 2,
            groupName: "ClassGroup", excludeFromDescription: true, methodAllowAnonymous: true, methodTags: true, mapGroupPattern: "/individuals"
        );
        await VerifyIndividualAsync(source, nameof(ClassMapGroup));
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
        var source = ConfigureScenario(true);
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
        var source = HttpMethodScenario(true);
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
        var source = HttpMethodScenario(true, includeMethodNameCollision: true);
        await VerifyIndividualAsync(source, nameof(MethodNameCollision));
    }

    [Fact]
    public async Task MultipleEndpointNameCollisions()
    {
        var source = EndpointNameCollisionScenario();
        await VerifyIndividualAsync(source, nameof(MultipleEndpointNameCollisions));
    }

    [Fact]
    public async Task BindingNames()
    {
        var source = ContractScenario(true);
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
    public async Task GenericProducesResponseAttribute()
    {
        var source = ContractScenario(includeGenericProducesResponse: true, producesContentType1: "application/json");
        await VerifyIndividualAsync(source, nameof(GenericProducesResponseAttribute));
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

    [Fact]
    public async Task AsyncMethodVariants()
    {
        var source = AsyncHandlerScenario();
        await VerifyIndividualAsync(source, nameof(AsyncMethodVariants));
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
    {
        return SourceFactory.BuildFallbackSource(includeDefault, includeCustom, customRoute);
    }

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
        bool excludeFromDescription = false,
        string? mapGroupPattern = null,
        bool classDisableValidation = false,
        bool methodDisableValidation = false
    )
    {
        return SourceFactory.BuildAuthorizationMatrixSource(classAllowAnonymous, methodAllowAnonymous, classRequireAuthorization, methodRequireAuthorization,
            classTags, methodTags, classHost, methodHost, classRequireCors, classCorsPolicy, methodRequireCors, methodCorsPolicy, requireRateLimiting,
            rateLimitingPolicy, applyShortCircuit, applyRequestTimeout, requestTimeoutPolicy, disableRequestTimeout, orderValue, groupName,
            excludeFromDescription, mapGroupPattern, classDisableValidation, methodDisableValidation
        );
    }

    private static string ConfigureScenario(
        bool configureWithServiceProvider = false,
        bool configureAddsMetadata = false,
        bool includeClassLevelFilter = false,
        bool includeMethodLevelFilter = false,
        bool includeGenericFilter = false,
        bool configureRegistersFilter = false,
        string metadataValue = "Individual"
    )
    {
        return SourceFactory.BuildConfigureAndFiltersSource(configureWithServiceProvider, configureAddsMetadata, includeClassLevelFilter,
            includeMethodLevelFilter, includeGenericFilter, configureRegistersFilter, metadataValue
        );
    }

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
        bool includeMethodNameCollision = false
    )
    {
        return SourceFactory.BuildHttpMethodMatrixSource(includeGet, includePost, includePut, includeDelete, includeOptions, includeHead, includePatch,
            includeQuery, includeTrace, includeConnect, includeMethodNameCollision
        );
    }

    private static string EndpointNameCollisionScenario()
    {
        return SourceFactory.BuildEndpointNameCollisionSource();
    }

    private static string ContractScenario(
        bool includeBindingNames = false,
        bool includeAsParameters = false,
        bool includeFromServices = false,
        bool includeFromKeyedServices = false,
        bool includeAccepts = false,
        bool includeGenericAccepts = false,
        bool includeProducesResponse = false,
        bool includeGenericProducesResponse = false,
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
        string? producesContentType2 = null
    )
    {
        return SourceFactory.BuildContractsAndBindingSource(includeBindingNames, includeAsParameters, includeFromServices, includeFromKeyedServices,
            includeAccepts, includeGenericAccepts, includeProducesResponse, includeGenericProducesResponse, includeProducesProblem, includeProducesValidationProblem,
            includeSummaryAndDescription, includeDisplayName, includeTags, excludeFromDescription, allowAnonymous, methodRequiresAuthorization,
            acceptsContentType1, acceptsContentType2, producesContentType1, producesContentType2
        );
    }

    private static string AsyncHandlerScenario()
    {
        return """
               using System.Threading.Tasks;

               internal sealed class AsyncHandlerEndpoints
               {
                   [MapGet("/task")]
                   public async Task TaskOnly()
                   {
                       await Task.Yield();
                   }

                   [MapGet("/task-result")]
                   public async Task<Results<Ok<string>, NotFound>> TaskWithResult(int id)
                   {
                       await Task.Yield();
                       return id >= 0 ? TypedResults.Ok("task") : TypedResults.NotFound();
                   }

                   [MapPost("/valuetask")]
                   public async ValueTask ValueTaskOnly()
                   {
                       await Task.Yield();
                   }

                   [MapPost("/valuetask-result")]
                   public async ValueTask<Results<Ok<string>, NotFound>> ValueTaskWithResult(int id)
                   {
                       await Task.Yield();
                       return id >= 0 ? TypedResults.Ok("value") : TypedResults.NotFound();
                   }
               }
               """;
    }
}
