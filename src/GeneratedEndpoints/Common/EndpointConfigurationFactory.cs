using System.Runtime.CompilerServices;
using Microsoft.CodeAnalysis;
using static GeneratedEndpoints.Common.Constants;

namespace GeneratedEndpoints.Common;

internal static class EndpointConfigurationFactory
{
    private static readonly ConditionalWeakTable<INamedTypeSymbol, GeneratedAttributeKindCacheEntry> GeneratedAttributeKindCache = new();

    public static EndpointConfiguration Create(ISymbol symbol)
    {
        var attributes = symbol.GetAttributes();

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
        string? groupIdentifier = null;
        string? groupPattern = null;
        string? groupName = null;
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
                    groupIdentifier = GetMapGroupIdentifier(symbol);
                    groupPattern = attribute.GetConstructorStringValue() ?? "";
                    groupName = attribute.GetNamedStringValue(NameAttributeNamedParameter);
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
                    var producesProblemMetadata = new ProducesProblemMetadata(statusCode, contentType, additionalContentTypes);

                    var producesProblemList = producesProblem ??= [];
                    producesProblemList.Add(producesProblemMetadata);
                    continue;
                }
                case RequestHandlerAttributeKind.ProducesValidationProblem:
                {
                    var statusCode = attribute.GetConstructorIntValue() ?? 400;
                    var contentType = attribute.GetConstructorStringValue(1);
                    var additionalContentTypes = attribute.GetConstructorStringArray(2);
                    var producesValidationProblemMetadata = new ProducesValidationProblemMetadata(statusCode, contentType, additionalContentTypes);

                    var producesValidationProblemList = producesValidationProblem ??= [];
                    producesValidationProblemList.Add(producesValidationProblemMetadata);
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
            Group = groupIdentifier is not null && groupPattern is not null
                ? new EndpointGroup
                {
                    Identifier = groupIdentifier,
                    Pattern = groupPattern,
                    Name = groupName,
                }
                : null,
        };
    }

    private static string? GetMapGroupIdentifier(ISymbol symbol)
    {
        if (symbol is not INamedTypeSymbol namedTypeSymbol)
            return null;

        var className = namedTypeSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);

        if (className.StartsWith(GlobalPrefix, StringComparison.Ordinal))
            className = className[GlobalPrefix.Length..];

        var builder = StringBuilderPool.Get(className.Length + 8);
        builder.Append('_');

        foreach (var character in className)
            builder.Append(char.IsLetterOrDigit(character) ? character : '_');

        builder.Append("_Group");
        return StringBuilderPool.ToStringAndReturn(builder);
    }

    private static RequestHandlerAttributeKind GetGeneratedAttributeKind(INamedTypeSymbol attributeClass)
    {
        var definition = attributeClass.OriginalDefinition;
        var cacheEntry = GeneratedAttributeKindCache.GetValue(
            definition, static def => new GeneratedAttributeKindCacheEntry(def.GetRequestHandlerAttributeKind())
        );

        return cacheEntry.Kind;
    }

    private static EquatableImmutableArray<T>? ToEquatableOrNull<T>(List<T>? values)
    {
        return values is { Count: > 0 } ? values.ToEquatableImmutableArray() : null;
    }

    private static void TryAddAcceptsMetadata(AttributeData attribute, INamedTypeSymbol attributeClass, ref List<AcceptsMetadata>? accepts)
    {
        string? requestType;
        if (attributeClass is { IsGenericType: true, TypeArguments.Length: 1 })
            requestType = attributeClass.TypeArguments[0]
                .ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
        else if (attribute.GetNamedTypeSymbol(RequestTypeAttributeNamedParameter) is { } requestTypeSymbol)
            requestType = requestTypeSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
        else
            return;

        var contentType = attribute.GetConstructorStringValue() ?? ApplicationJsonContentType;
        var additionalContentTypes = attribute.GetConstructorStringArray(1);
        var isOptional = attribute.GetNamedBoolValue(IsOptionalAttributeNamedParameter);

        var acceptMetadata = new AcceptsMetadata(requestType, contentType, additionalContentTypes, isOptional);

        var acceptsList = accepts ??= [];
        acceptsList.Add(acceptMetadata);
    }

    private static void TryAddProducesMetadata(AttributeData attribute, INamedTypeSymbol attributeClass, ref List<ProducesMetadata>? produces)
    {
        string? responseType;
        if (attributeClass is { IsGenericType: true, TypeArguments.Length: 1 })
            responseType = attributeClass.TypeArguments[0]
                .ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
        else if (attribute.GetNamedTypeSymbol(ResponseTypeAttributeNamedParameter) is { } responseTypeSymbol)
            responseType = responseTypeSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
        else
            return;

        var statusCode = attribute.GetConstructorIntValue() ?? 200;
        var contentType = attribute.GetConstructorStringValue(1);
        var additionalContentTypes = attribute.GetConstructorStringArray(2);

        var producesMetadata = new ProducesMetadata(responseType, statusCode, contentType, additionalContentTypes);

        var producesList = produces ??= [];
        producesList.Add(producesMetadata);
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
