using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using Microsoft.CodeAnalysis;
using static GeneratedEndpoints.Common.AttributeSymbolMatcher;
using static GeneratedEndpoints.Common.Constants;

namespace GeneratedEndpoints.Common;

internal static class EndpointConfigurationFactory
{
    private sealed class GeneratedAttributeKindCacheEntry(GeneratedAttributeKind kind)
    {
        public GeneratedAttributeKind Kind { get; } = kind;
    }

    private static readonly ConditionalWeakTable<INamedTypeSymbol, GeneratedAttributeKindCacheEntry> GeneratedAttributeKindCache = new();

    public static EndpointConfiguration Create(
        ImmutableArray<AttributeData> attributes,
        string? name,
        string? displayName,
        string? description,
        bool enforceMethodRequireAuthorizationRules
    )
    {
        var state = new EndpointAttributeState();
        PopulateAttributeState(attributes, ref state);

        if (enforceMethodRequireAuthorizationRules && state is { HasRequireAuthorizationAttribute: true, HasAllowAnonymousAttribute: false })
            state.AllowAnonymous = false;

        var metadata = new RequestHandlerMetadata(
            name,
            displayName,
            state.Summary,
            description,
            state.Tags,
            ToEquatableOrNull(state.Accepts),
            ToEquatableOrNull(state.Produces),
            ToEquatableOrNull(state.ProducesProblem),
            ToEquatableOrNull(state.ProducesValidationProblem),
            state.ExcludeFromDescription ?? false
        );

        var withRequestTimeout = state.WithRequestTimeout ?? false;
        var requestTimeoutPolicyName = withRequestTimeout ? state.RequestTimeoutPolicyName : null;

        return new EndpointConfiguration(
            metadata,
            state.RequireAuthorization ?? false,
            state.AuthorizationPolicies,
            state.DisableAntiforgery ?? false,
            state.AllowAnonymous ?? false,
            state.RequireCors ?? false,
            state.CorsPolicyName,
            state.RequiredHosts,
            state.RequireRateLimiting ?? false,
            state.RateLimitingPolicyName,
            ToEquatableOrNull(state.EndpointFilters),
            state.ShortCircuit ?? false,
            state.DisableValidation ?? false,
            state.DisableRequestTimeout ?? false,
            withRequestTimeout,
            requestTimeoutPolicyName,
            state.Order,
            state.EndpointGroupName
        );
    }

    public static GeneratedAttributeKind GetGeneratedAttributeKind(INamedTypeSymbol attributeClass)
    {
        var definition = attributeClass.OriginalDefinition;
        var cacheEntry = GeneratedAttributeKindCache.GetValue(
            definition,
            static def => new GeneratedAttributeKindCacheEntry(GetGeneratedAttributeKindCore(def))
        );

        return cacheEntry.Kind;
    }

    public static string? NormalizeOptionalString(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : value!.Trim();
    }

    private static void PopulateAttributeState(ImmutableArray<AttributeData> attributes, ref EndpointAttributeState state)
    {
        ref var tags = ref state.Tags;
        ref var requireAuthorization = ref state.RequireAuthorization;
        ref var authorizationPolicies = ref state.AuthorizationPolicies;
        ref var disableAntiforgery = ref state.DisableAntiforgery;
        ref var allowAnonymous = ref state.AllowAnonymous;
        ref var excludeFromDescription = ref state.ExcludeFromDescription;
        ref var accepts = ref state.Accepts;
        ref var produces = ref state.Produces;
        ref var producesProblem = ref state.ProducesProblem;
        ref var producesValidationProblem = ref state.ProducesValidationProblem;
        ref var requireCors = ref state.RequireCors;
        ref var corsPolicyName = ref state.CorsPolicyName;
        ref var requiredHosts = ref state.RequiredHosts;
        ref var requireRateLimiting = ref state.RequireRateLimiting;
        ref var rateLimitingPolicyName = ref state.RateLimitingPolicyName;
        ref var endpointFilters = ref state.EndpointFilters;
        ref var endpointFilterSet = ref state.EndpointFilterSet;
        ref var hasAllowAnonymousAttribute = ref state.HasAllowAnonymousAttribute;
        ref var hasRequireAuthorizationAttribute = ref state.HasRequireAuthorizationAttribute;
        ref var shortCircuit = ref state.ShortCircuit;
        ref var disableValidation = ref state.DisableValidation;
        ref var disableRequestTimeout = ref state.DisableRequestTimeout;
        ref var withRequestTimeout = ref state.WithRequestTimeout;
        ref var requestTimeoutPolicyName = ref state.RequestTimeoutPolicyName;
        ref var order = ref state.Order;
        ref var endpointGroupName = ref state.EndpointGroupName;
        ref var summary = ref state.Summary;

        foreach (var attribute in attributes)
        {
            var attributeClass = attribute.AttributeClass;
            if (attributeClass is null)
                continue;

            switch (GetGeneratedAttributeKind(attributeClass))
            {
                case GeneratedAttributeKind.ShortCircuit:
                    shortCircuit = true;
                    continue;
                case GeneratedAttributeKind.DisableValidation:
                    disableValidation = true;
                    continue;
                case GeneratedAttributeKind.DisableRequestTimeout:
                    disableRequestTimeout = true;
                    withRequestTimeout = false;
                    requestTimeoutPolicyName = null;
                    continue;
                case GeneratedAttributeKind.RequestTimeout:
                {
                    disableRequestTimeout = false;
                    withRequestTimeout = true;

                    string? policyName = null;
                    if (attribute.ConstructorArguments.Length > 0)
                        policyName = attribute.ConstructorArguments[0].Value as string;

                    policyName ??= GetNamedStringValue(attribute, PolicyNameAttributeNamedParameter);
                    requestTimeoutPolicyName = NormalizeOptionalString(policyName);
                    continue;
                }
                case GeneratedAttributeKind.Order:
                    if (attribute.ConstructorArguments.Length > 0 && attribute.ConstructorArguments[0].Value is int orderValue)
                        order = orderValue;
                    continue;
                case GeneratedAttributeKind.MapGroup:
                {
                    var groupName = GetNamedStringValue(attribute, NameAttributeNamedParameter);
                    if (!string.IsNullOrEmpty(groupName))
                        endpointGroupName = groupName;
                    continue;
                }
                case GeneratedAttributeKind.Summary:
                    if (attribute.ConstructorArguments.Length > 0)
                    {
                        var summaryValue = NormalizeOptionalString(attribute.ConstructorArguments[0].Value as string);
                        if (!string.IsNullOrEmpty(summaryValue))
                            summary = summaryValue;
                    }
                    continue;
                case GeneratedAttributeKind.Accepts:
                    TryAddAcceptsMetadata(attribute, attributeClass, ref accepts);
                    continue;
                case GeneratedAttributeKind.ProducesResponse:
                    TryAddProducesMetadata(attribute, attributeClass, ref produces);
                    continue;
                case GeneratedAttributeKind.RequireAuthorization:
                    requireAuthorization = true;
                    hasRequireAuthorizationAttribute = true;
                    if (attribute.ConstructorArguments.Length == 1)
                    {
                        var arg = attribute.ConstructorArguments[0];
                        MergeInto(ref authorizationPolicies, arg.Values);
                    }

                    continue;
                case GeneratedAttributeKind.RequireCors:
                    requireCors = true;
                    corsPolicyName = attribute.ConstructorArguments.Length > 0
                        ? NormalizeOptionalString(attribute.ConstructorArguments[0].Value as string)
                        : null;
                    continue;
                case GeneratedAttributeKind.RequireHost:
                    if (attribute.ConstructorArguments.Length == 1)
                    {
                        var arg = attribute.ConstructorArguments[0];
                        if (arg is { Kind: TypedConstantKind.Array, Values.Length: > 0 })
                            MergeInto(ref requiredHosts, arg.Values);
                        else if (arg.Value is string singleHost && !string.IsNullOrWhiteSpace(singleHost))
                            MergeInto(ref requiredHosts, [singleHost.Trim()]);
                    }

                    continue;
                case GeneratedAttributeKind.RequireRateLimiting:
                {
                    var policyName = attribute.ConstructorArguments.Length > 0
                        ? NormalizeOptionalString(attribute.ConstructorArguments[0].Value as string)
                        : null;

                    if (!string.IsNullOrEmpty(policyName))
                    {
                        requireRateLimiting = true;
                        rateLimitingPolicyName = policyName;
                    }

                    continue;
                }
                case GeneratedAttributeKind.EndpointFilter:
                    TryAddEndpointFilter(attribute, attributeClass, ref endpointFilters, ref endpointFilterSet);
                    continue;
                case GeneratedAttributeKind.DisableAntiforgery:
                    disableAntiforgery = true;
                    continue;
                case GeneratedAttributeKind.ProducesProblem:
                {
                    var statusCode = attribute.ConstructorArguments.Length > 0 && attribute.ConstructorArguments[0].Value is int producesProblemStatusCode
                        ? producesProblemStatusCode
                        : 500;
                    var contentType = attribute.ConstructorArguments.Length > 1
                        ? NormalizeOptionalContentType(attribute.ConstructorArguments[1].Value as string)
                        : null;
                    var additionalContentTypes = attribute.ConstructorArguments.Length > 2 ? GetStringArrayValues(attribute.ConstructorArguments[2]) : null;

                    var producesProblemList = producesProblem ??= [];
                    producesProblemList.Add(new ProducesProblemMetadata(statusCode, contentType, additionalContentTypes));
                    continue;
                }
                case GeneratedAttributeKind.ProducesValidationProblem:
                {
                    var statusCode =
                        attribute.ConstructorArguments.Length > 0 && attribute.ConstructorArguments[0].Value is int producesValidationProblemStatusCode
                            ? producesValidationProblemStatusCode
                            : 400;
                    var contentType = attribute.ConstructorArguments.Length > 1
                        ? NormalizeOptionalContentType(attribute.ConstructorArguments[1].Value as string)
                        : null;
                    var additionalContentTypes = attribute.ConstructorArguments.Length > 2 ? GetStringArrayValues(attribute.ConstructorArguments[2]) : null;

                    var producesValidationProblemList = producesValidationProblem ??= [];
                    producesValidationProblemList.Add(new ProducesValidationProblemMetadata(statusCode, contentType, additionalContentTypes));
                    continue;
                }
            }

            if (IsAttribute(attributeClass, AllowAnonymousAttributeName, AspNetCoreAuthorizationNamespaceParts))
            {
                allowAnonymous = true;
                hasAllowAnonymousAttribute = true;
                continue;
            }

            if (IsAttribute(attributeClass, "TagsAttribute", AspNetCoreHttpNamespaceParts))
            {
                if (attribute.ConstructorArguments.Length > 0)
                {
                    var arg = attribute.ConstructorArguments[0];
                    MergeInto(ref tags, arg.Values);
                }

                continue;
            }

            if (IsAttribute(attributeClass, "ExcludeFromDescriptionAttribute", AspNetCoreRoutingNamespaceParts))
                excludeFromDescription = true;
        }
    }

    private static void MergeInto(ref EquatableImmutableArray<string>? target, IEnumerable<string> values)
    {
        var merged = MergeUnion(target, values);
        target = merged.Count > 0 ? merged : null;
    }

    private static void MergeInto(ref EquatableImmutableArray<string>? target, ImmutableArray<TypedConstant> values)
    {
        if (values.IsDefaultOrEmpty)
            return;

        List<string>? normalized = null;
        foreach (var value in values)
        {
            if (value.Value is not string stringValue)
                continue;

            var trimmed = NormalizeOptionalString(stringValue);
            if (trimmed is not { Length: > 0 })
                continue;

            normalized ??= new List<string>(values.Length);
            normalized.Add(trimmed);
        }

        if (normalized is { Count: > 0 })
            MergeInto(ref target, normalized);
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
            var normalized = NormalizeOptionalString(value);
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

    private static EquatableImmutableArray<T>? ToEquatableOrNull<T>(List<T>? values)
    {
        return values is { Count: > 0 } ? values.ToEquatableImmutableArray() : null;
    }

    private static string NormalizeRequiredContentType(string? contentType, string defaultValue)
    {
        return string.IsNullOrWhiteSpace(contentType) ? defaultValue : contentType!.Trim();
    }

    private static string? NormalizeOptionalContentType(string? contentType)
    {
        return string.IsNullOrWhiteSpace(contentType) ? null : contentType!.Trim();
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
        var isOptional = GetNamedBoolValue(attribute, IsOptionalAttributeNamedParameter);

        if (attributeClass is { IsGenericType: true, TypeArguments.Length: 1 })
        {
            requestType = attributeClass.TypeArguments[0]
                .ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            contentType = attribute.ConstructorArguments.Length > 0
                ? NormalizeRequiredContentType(attribute.ConstructorArguments[0].Value as string, "application/json")
                : "application/json";
            additionalContentTypes = attribute.ConstructorArguments.Length > 1 ? GetStringArrayValues(attribute.ConstructorArguments[1]) : null;
        }
        else if (GetNamedTypeSymbol(attribute, RequestTypeAttributeNamedParameter) is { } requestTypeSymbol)
        {
            requestType = requestTypeSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            contentType = attribute.ConstructorArguments.Length > 0
                ? NormalizeRequiredContentType(attribute.ConstructorArguments[0].Value as string, "application/json")
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
            contentType = attribute.ConstructorArguments.Length > 1 ? NormalizeOptionalContentType(attribute.ConstructorArguments[1].Value as string) : null;
            additionalContentTypes = attribute.ConstructorArguments.Length > 2 ? GetStringArrayValues(attribute.ConstructorArguments[2]) : null;
        }
        else if (GetNamedTypeSymbol(attribute, ResponseTypeAttributeNamedParameter) is { } responseTypeSymbol)
        {
            responseType = responseTypeSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            statusCode = attribute.ConstructorArguments.Length > 0 && attribute.ConstructorArguments[0].Value is int producesStatusCode
                ? producesStatusCode
                : 200;
            contentType = attribute.ConstructorArguments.Length > 1 ? NormalizeOptionalContentType(attribute.ConstructorArguments[1].Value as string) : null;
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
        ref HashSet<string>? endpointFilterSet)
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

    private static void TryAddEndpointFilterType(
        ITypeSymbol? typeSymbol,
        ref List<string>? endpointFilters,
        ref HashSet<string>? endpointFilterSet)
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

    private static ITypeSymbol? GetNamedTypeSymbol(AttributeData attribute, string namedParameter)
    {
        foreach (var namedArg in attribute.NamedArguments)
        {
            if (namedArg.Key == namedParameter && namedArg.Value.Value is ITypeSymbol typeSymbol)
                return typeSymbol;
        }

        return null;
    }

    private static bool GetNamedBoolValue(AttributeData attribute, string namedParameter, bool defaultValue = false)
    {
        foreach (var namedArg in attribute.NamedArguments)
        {
            if (namedArg.Key == namedParameter && namedArg.Value.Value is bool boolValue)
                return boolValue;
        }

        return defaultValue;
    }

    private static string? GetNamedStringValue(AttributeData attribute, string namedParameter)
    {
        foreach (var namedArg in attribute.NamedArguments)
        {
            if (namedArg.Key == namedParameter && namedArg.Value.Value is string stringValue)
                return NormalizeOptionalString(stringValue);
        }

        return null;
    }

    private static GeneratedAttributeKind GetGeneratedAttributeKindCore(INamedTypeSymbol definition)
    {
        if (!IsInNamespace(definition.ContainingNamespace, AttributesNamespaceParts))
            return GeneratedAttributeKind.None;

        return definition.Name switch
        {
            ShortCircuitAttributeName => GeneratedAttributeKind.ShortCircuit,
            DisableValidationAttributeName => GeneratedAttributeKind.DisableValidation,
            DisableRequestTimeoutAttributeName => GeneratedAttributeKind.DisableRequestTimeout,
            RequestTimeoutAttributeName => GeneratedAttributeKind.RequestTimeout,
            OrderAttributeName => GeneratedAttributeKind.Order,
            MapGroupAttributeName => GeneratedAttributeKind.MapGroup,
            SummaryAttributeName => GeneratedAttributeKind.Summary,
            AcceptsAttributeName => GeneratedAttributeKind.Accepts,
            ProducesResponseAttributeName => GeneratedAttributeKind.ProducesResponse,
            RequireAuthorizationAttributeName => GeneratedAttributeKind.RequireAuthorization,
            RequireCorsAttributeName => GeneratedAttributeKind.RequireCors,
            RequireHostAttributeName => GeneratedAttributeKind.RequireHost,
            RequireRateLimitingAttributeName => GeneratedAttributeKind.RequireRateLimiting,
            EndpointFilterAttributeName => GeneratedAttributeKind.EndpointFilter,
            DisableAntiforgeryAttributeName => GeneratedAttributeKind.DisableAntiforgery,
            ProducesProblemAttributeName => GeneratedAttributeKind.ProducesProblem,
            ProducesValidationProblemAttributeName => GeneratedAttributeKind.ProducesValidationProblem,
            _ => GeneratedAttributeKind.None,
        };
    }
}
