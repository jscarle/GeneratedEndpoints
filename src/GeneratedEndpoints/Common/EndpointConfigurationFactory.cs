using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using Microsoft.CodeAnalysis;
using static GeneratedEndpoints.Common.Constants;

namespace GeneratedEndpoints.Common;

internal static class EndpointConfigurationFactory
{
    private static readonly ConditionalWeakTable<INamedTypeSymbol, GeneratedAttributeKindCacheEntry> GeneratedAttributeKindCache = new();

    public static EndpointConfiguration Create(ISymbol symbol, string? name)
    {
        var attributes = symbol.GetAttributes();

        if (symbol is IMethodSymbol)
            name ??= RemoveAsyncSuffix(symbol.Name);

        string? displayName = null;
        string? description = null;
        EquatableImmutableArray<string>? tags = null;
        bool? requireAuthorization = null;
        EquatableImmutableArray<string>? authorizationPolicies = null;
        bool? disableAntiforgery = null;
        bool? allowAnonymous = null;
        bool? excludeFromDescription = null;
        List<AcceptsMetadata>? accepts = null;
        List<ProducesMetadata>? produces = null;
        List<ProducesProblemMetadata>? producesProblem = null;
        List<ProducesValidationProblemMetadata>? producesValidationProblem = null;
        bool? requireCors = null;
        string? corsPolicyName = null;
        EquatableImmutableArray<string>? requiredHosts = null;
        bool? requireRateLimiting = null;
        string? rateLimitingPolicyName = null;
        List<string>? endpointFilters = null;
        HashSet<string>? endpointFilterSet = null;
        bool? shortCircuit = null;
        bool? disableValidation = null;
        bool? disableRequestTimeout = null;
        bool? withRequestTimeout = null;
        string? requestTimeoutPolicyName = null;
        int? order = null;
        string? endpointGroupName = null;
        string? summary = null;

        foreach (var attribute in attributes)
        {
            var attributeClass = attribute.AttributeClass;
            if (attributeClass is null)
                continue;

            var attributeKind = GetGeneratedAttributeKind(attributeClass);
            switch (attributeKind)
            {
                case RequestHandlerAttributeKind.ShortCircuit:
                    shortCircuit = true;
                    continue;
                case RequestHandlerAttributeKind.DisableValidation:
                    disableValidation = true;
                    continue;
                case RequestHandlerAttributeKind.DisableRequestTimeout:
                    disableRequestTimeout = true;
                    continue;
                case RequestHandlerAttributeKind.RequestTimeout:
                    requestTimeoutPolicyName = attribute.GetConstructorStringValue();
                    withRequestTimeout = true;
                    continue;
                case RequestHandlerAttributeKind.Order:
                    order = attribute.GetConstructorIntValue();
                    continue;
                case RequestHandlerAttributeKind.MapGroup:
                    endpointGroupName = attribute.GetNamedStringValue(NameAttributeNamedParameter);
                    continue;
                case RequestHandlerAttributeKind.Summary:
                    summary = attribute.GetConstructorStringValue();
                    continue;
                case RequestHandlerAttributeKind.Accepts:
                    TryAddAcceptsMetadata(attribute, attributeClass, ref accepts);
                    continue;
                case RequestHandlerAttributeKind.ProducesResponse:
                    TryAddProducesMetadata(attribute, attributeClass, ref produces);
                    continue;
                case RequestHandlerAttributeKind.RequireAuthorization:
                    authorizationPolicies = attribute.GetConstructorStringArray();
                    requireAuthorization = true;
                    continue;
                case RequestHandlerAttributeKind.RequireCors:
                    corsPolicyName = attribute.GetConstructorStringValue();
                    requireCors = true;
                    continue;
                case RequestHandlerAttributeKind.RequireHost:
                    requiredHosts = attribute.GetConstructorStringArray();
                    continue;
                case RequestHandlerAttributeKind.RequireRateLimiting:
                    rateLimitingPolicyName = attribute.GetConstructorStringValue();
                    requireRateLimiting = rateLimitingPolicyName is not null;
                    continue;
                case RequestHandlerAttributeKind.EndpointFilter:
                    TryAddEndpointFilter(attribute, attributeClass, ref endpointFilters, ref endpointFilterSet);
                    continue;
                case RequestHandlerAttributeKind.DisableAntiforgery:
                    disableAntiforgery = true;
                    continue;
                case RequestHandlerAttributeKind.ProducesProblem:
                {
                    var statusCode = attribute.GetConstructorIntValue() ?? 500;
                    var contentType = attribute.GetConstructorStringValue(1);
                    var additionalContentTypes = attribute.GetConstructorStringArray(2);

                    var producesProblemList = producesProblem ??= [];
                    producesProblemList.Add(new ProducesProblemMetadata(statusCode, contentType, additionalContentTypes));
                    continue;
                }
                case RequestHandlerAttributeKind.ProducesValidationProblem:
                {
                    var statusCode = attribute.GetConstructorIntValue() ?? 400;
                    var contentType = attribute.GetConstructorStringValue(1);
                    var additionalContentTypes = attribute.GetConstructorStringArray(2);

                    var producesValidationProblemList = producesValidationProblem ??= [];
                    producesValidationProblemList.Add(new ProducesValidationProblemMetadata(statusCode, contentType, additionalContentTypes));
                    continue;
                }
                case RequestHandlerAttributeKind.DisplayName:
                    displayName = attribute.GetConstructorStringValue();
                    break;
                case RequestHandlerAttributeKind.Description:
                    description = attribute.GetConstructorStringValue();
                    break;
                case RequestHandlerAttributeKind.AllowAnonymous:
                    allowAnonymous = true;
                    break;
                case RequestHandlerAttributeKind.Tags:
                    tags = attribute.GetConstructorStringArray();
                    break;
                case RequestHandlerAttributeKind.ExcludeFromDescription:
                    excludeFromDescription = true;
                    break;
                case RequestHandlerAttributeKind.None:
                default:
                    break;
            }
        }

        return new EndpointConfiguration
        {
            Name = name,
            DisplayName = displayName,
            Summary = summary,
            Description = description,
            Tags = tags,
            Accepts = ToEquatableOrNull(accepts),
            Produces = ToEquatableOrNull(produces),
            ProducesProblem = ToEquatableOrNull(producesProblem),
            ProducesValidationProblem = ToEquatableOrNull(producesValidationProblem),
            ExcludeFromDescription = excludeFromDescription ?? false,
            RequireAuthorization = requireAuthorization ?? false,
            AuthorizationPolicies = authorizationPolicies,
            DisableAntiforgery = disableAntiforgery ?? false,
            AllowAnonymous = allowAnonymous ?? false,
            RequireCors = requireCors ?? false,
            CorsPolicyName = corsPolicyName,
            RequiredHosts = requiredHosts,
            RequireRateLimiting = requireRateLimiting ?? false,
            RateLimitingPolicyName = rateLimitingPolicyName,
            EndpointFilterTypes = ToEquatableOrNull(endpointFilters),
            ShortCircuit = shortCircuit ?? false,
            DisableValidation = disableValidation ?? false,
            DisableRequestTimeout = disableRequestTimeout ?? false,
            WithRequestTimeout = withRequestTimeout ?? false,
            RequestTimeoutPolicyName = requestTimeoutPolicyName,
            Order = order,
            EndpointGroupName = endpointGroupName,
        };
    }

    public static RequestHandlerAttributeKind GetGeneratedAttributeKind(INamedTypeSymbol attributeClass)
    {
        var definition = attributeClass.OriginalDefinition;
        var cacheEntry = GeneratedAttributeKindCache.GetValue(
            definition, static def => new GeneratedAttributeKindCacheEntry(def.GetRequestHandlerAttributeKind())
        );

        return cacheEntry.Kind;
    }

    internal static EquatableImmutableArray<string> MergeUnion(EquatableImmutableArray<string>? existing, IEnumerable<string> values)
    {
        List<string>? list = null;
        HashSet<string>? seen = null;

        if (existing is { Count: > 0 })
        {
            var count = existing.Value.Count;
            list = new List<string>(count + 4);
            list.AddRange(existing.Value);
            seen = new HashSet<string>(existing.Value, StringComparer.OrdinalIgnoreCase);
        }

        foreach (var value in values)
        {
            var normalized = value.NormalizeOptionalString();
            if (normalized is not { Length: > 0 })
                continue;

            seen ??= new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            if (!seen.Add(normalized))
                continue;

            list ??= [];
            list.Add(normalized);
        }

        return list?.ToEquatableImmutableArray() ?? EquatableImmutableArray<string>.Empty;
    }

    private static string RemoveAsyncSuffix(string methodName)
    {
        if (methodName.EndsWith(AsyncSuffix, StringComparison.OrdinalIgnoreCase) && methodName.Length > AsyncSuffix.Length)
            return methodName[..^AsyncSuffix.Length];

        return methodName;
    }

    private static EquatableImmutableArray<T>? ToEquatableOrNull<T>(List<T>? values)
    {
        return values is { Count: > 0 } ? values.ToEquatableImmutableArray() : null;
    }

    private static EquatableImmutableArray<string>? GetStringArrayValues(TypedConstant typedConstant)
    {
        if (typedConstant.Kind != TypedConstantKind.Array || typedConstant.Values.IsDefaultOrEmpty)
            return null;

        var builder = ImmutableArray.CreateBuilder<string>(typedConstant.Values.Length);
        foreach (var value in typedConstant.Values)
        {
            if (value.Value is string s && !string.IsNullOrWhiteSpace(s))
                builder.Add(s.Trim());
        }

        return builder.Count > 0 ? builder.ToEquatableImmutable() : null;
    }

    private static void TryAddAcceptsMetadata(AttributeData attribute, INamedTypeSymbol attributeClass, ref List<AcceptsMetadata>? accepts)
    {
        string? requestType;
        string contentType;
        EquatableImmutableArray<string>? additionalContentTypes;
        var isOptional = attribute.GetNamedBoolValue(IsOptionalAttributeNamedParameter);

        if (attributeClass is { IsGenericType: true, TypeArguments.Length: 1 })
        {
            requestType = attributeClass.TypeArguments[0]
                .ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            contentType = attribute.ConstructorArguments.Length > 0
                ? (attribute.ConstructorArguments[0].Value as string).NormalizeOrDefaultString("application/json")
                : "application/json";
            additionalContentTypes = attribute.ConstructorArguments.Length > 1 ? GetStringArrayValues(attribute.ConstructorArguments[1]) : null;
        }
        else if (attribute.GetNamedTypeSymbol(RequestTypeAttributeNamedParameter) is { } requestTypeSymbol)
        {
            requestType = requestTypeSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            contentType = attribute.ConstructorArguments.Length > 0
                ? (attribute.ConstructorArguments[0].Value as string).NormalizeOrDefaultString("application/json")
                : "application/json";
            additionalContentTypes = attribute.ConstructorArguments.Length > 1 ? GetStringArrayValues(attribute.ConstructorArguments[1]) : null;
        }
        else
        {
            return;
        }

        var acceptsList = accepts ??= [];
        acceptsList.Add(new AcceptsMetadata(requestType, contentType, additionalContentTypes, isOptional));
    }

    private static void TryAddProducesMetadata(AttributeData attribute, INamedTypeSymbol attributeClass, ref List<ProducesMetadata>? produces)
    {
        string? responseType;
        int statusCode;
        string? contentType;
        EquatableImmutableArray<string>? additionalContentTypes;

        if (attributeClass is { IsGenericType: true, TypeArguments.Length: 1 })
        {
            responseType = attributeClass.TypeArguments[0]
                .ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            statusCode = attribute.ConstructorArguments.Length > 0 && attribute.ConstructorArguments[0].Value is int producesStatusCode
                ? producesStatusCode
                : 200;
            contentType = attribute.ConstructorArguments.Length > 1 ? (attribute.ConstructorArguments[1].Value as string).NormalizeOptionalString() : null;
            additionalContentTypes = attribute.ConstructorArguments.Length > 2 ? GetStringArrayValues(attribute.ConstructorArguments[2]) : null;
        }
        else if (attribute.GetNamedTypeSymbol(ResponseTypeAttributeNamedParameter) is { } responseTypeSymbol)
        {
            responseType = responseTypeSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            statusCode = attribute.ConstructorArguments.Length > 0 && attribute.ConstructorArguments[0].Value is int producesStatusCode
                ? producesStatusCode
                : 200;
            contentType = attribute.ConstructorArguments.Length > 1 ? (attribute.ConstructorArguments[1].Value as string).NormalizeOptionalString() : null;
            additionalContentTypes = attribute.ConstructorArguments.Length > 2 ? GetStringArrayValues(attribute.ConstructorArguments[2]) : null;
        }
        else
        {
            return;
        }

        var producesList = produces ??= [];
        producesList.Add(new ProducesMetadata(responseType, statusCode, contentType, additionalContentTypes));
    }

    private static void TryAddEndpointFilter(
        AttributeData attribute,
        INamedTypeSymbol attributeClass,
        ref List<string>? endpointFilters,
        ref HashSet<string>? endpointFilterSet
    )
    {
        if (attributeClass is { IsGenericType: true, TypeArguments.Length: 1 })
        {
            TryAddEndpointFilterType(attributeClass.TypeArguments[0], ref endpointFilters, ref endpointFilterSet);
            return;
        }

        if (attribute.ConstructorArguments.Length == 0)
            return;

        if (attribute.ConstructorArguments[0].Value is ITypeSymbol filterTypeSymbol)
            TryAddEndpointFilterType(filterTypeSymbol, ref endpointFilters, ref endpointFilterSet);
    }

    private static void TryAddEndpointFilterType(ITypeSymbol? typeSymbol, ref List<string>? endpointFilters, ref HashSet<string>? endpointFilterSet)
    {
        if (typeSymbol is null or ITypeParameterSymbol or IErrorTypeSymbol)
            return;

        var displayString = typeSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
        if (string.IsNullOrWhiteSpace(displayString))
            return;

        endpointFilterSet ??= new HashSet<string>(StringComparer.Ordinal);
        if (!endpointFilterSet.Add(displayString))
            return;

        endpointFilters ??= [];
        endpointFilters.Add(displayString);
    }

    private sealed class GeneratedAttributeKindCacheEntry(RequestHandlerAttributeKind kind)
    {
        public RequestHandlerAttributeKind Kind { get; } = kind;
    }
}
