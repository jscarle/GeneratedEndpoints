using GeneratedEndpoints.Common;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace GeneratedEndpoints;

public sealed partial class MinimalApiGenerator
{
    private readonly record struct HttpAttributeDefinition(
        string Name,
        string FullyQualifiedName,
        string Hint,
        string Verb,
        SourceText SourceText
    );

    private readonly record struct RequestHandler(
        RequestHandlerClass Class,
        RequestHandlerMethod Method,
        string HttpMethod,
        string Pattern,
        EndpointConfiguration Configuration
    );

    private readonly record struct RequestHandlerClass(
        string Name,
        bool IsStatic,
        bool HasConfigureMethod,
        bool ConfigureMethodAcceptsServiceProvider,
        string? MapGroupPattern,
        string? MapGroupBuilderIdentifier,
        EndpointConfiguration Configuration
    );

    private readonly record struct EndpointConfiguration(
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

    private readonly record struct RequestHandlerMethod(string Name, bool IsStatic, bool IsAwaitable, EquatableImmutableArray<Parameter> Parameters);

    private readonly record struct RequestHandlerMetadata(
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

    private readonly record struct AcceptsMetadata(
        string RequestType,
        string ContentType,
        EquatableImmutableArray<string>? AdditionalContentTypes,
        bool IsOptional
    );

    private readonly record struct ProducesMetadata(
        string ResponseType,
        int StatusCode,
        string? ContentType,
        EquatableImmutableArray<string>? AdditionalContentTypes
    );

    private readonly record struct ProducesProblemMetadata(int StatusCode, string? ContentType, EquatableImmutableArray<string>? AdditionalContentTypes);

    private readonly record struct ProducesValidationProblemMetadata(
        int StatusCode,
        string? ContentType,
        EquatableImmutableArray<string>? AdditionalContentTypes
    );

    private readonly record struct Parameter(string Name, string Type, string BindingPrefix);

    private readonly record struct ConfigureMethodDetails(bool HasConfigureMethod, bool ConfigureMethodAcceptsServiceProvider);

    private struct EndpointAttributeState
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

    private enum GeneratedAttributeKind
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
    }

    private enum BindingSource
    {
        None = 0,
        FromRoute = 1,
        FromQuery = 2,
        FromHeader = 3,
        FromBody = 4,
        FromForm = 5,
        FromServices = 6,
        FromKeyedServices = 7,
        AsParameters = 8,
    }

    private sealed class RequestHandlerComparer : IComparer<RequestHandler>
    {
        public static RequestHandlerComparer Instance { get; } = new();

        public int Compare(RequestHandler x, RequestHandler y)
        {
            var comparison = string.Compare(x.Class.Name, y.Class.Name, StringComparison.Ordinal);
            if (comparison != 0)
                return comparison;

            comparison = string.Compare(x.Method.Name, y.Method.Name, StringComparison.Ordinal);
            if (comparison != 0)
                return comparison;

            comparison = string.Compare(x.HttpMethod, y.HttpMethod, StringComparison.Ordinal);
            if (comparison != 0)
                return comparison;

            return string.Compare(x.Pattern, y.Pattern, StringComparison.Ordinal);
        }
    }

    private sealed class CompilationTypeCache(Compilation compilation)
    {
        public INamedTypeSymbol? EndpointConventionBuilderSymbol { get; } =
            compilation.GetTypeByMetadataName("Microsoft.AspNetCore.Builder.IEndpointConventionBuilder");

        public INamedTypeSymbol? ServiceProviderSymbol { get; } = compilation.GetTypeByMetadataName("System.IServiceProvider");
    }

    private sealed class RequestHandlerClassCacheEntry
    {
        private readonly object _lock = new();
        private RequestHandlerClass _value;
        private bool _initialized;

        public RequestHandlerClass GetOrCreate(INamedTypeSymbol classSymbol, CompilationTypeCache compilationCache, CancellationToken cancellationToken)
        {
            if (_initialized)
                return _value;

            lock (_lock)
            {
                if (_initialized)
                    return _value;

                cancellationToken.ThrowIfCancellationRequested();

                var name = classSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
                var isStatic = classSymbol.IsStatic;
                var configureMethodDetails = GetConfigureMethodDetails(classSymbol, compilationCache.EndpointConventionBuilderSymbol,
                    compilationCache.ServiceProviderSymbol, cancellationToken
                );

                var mapGroupPattern = GetMapGroupPattern(classSymbol);
                var mapGroupIdentifier = mapGroupPattern is null ? null : GetMapGroupIdentifier(name);
                var classConfiguration = GetEndpointConfiguration(classSymbol.GetAttributes(), null, null, null, false);

                _value = new RequestHandlerClass(name, isStatic, configureMethodDetails.HasConfigureMethod,
                    configureMethodDetails.ConfigureMethodAcceptsServiceProvider, mapGroupPattern, mapGroupIdentifier, classConfiguration
                );
                _initialized = true;
                return _value;
            }
        }
    }

    private sealed class GeneratedAttributeKindCacheEntry(GeneratedAttributeKind kind)
    {
        public GeneratedAttributeKind Kind { get; } = kind;
    }
}
