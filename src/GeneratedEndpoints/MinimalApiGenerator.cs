using System.Buffers;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using GeneratedEndpoints.Common;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace GeneratedEndpoints;

[Generator]
public sealed partial class MinimalApiGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(RegisterAttributes);

        var requestHandlerProviders = ImmutableArray.CreateBuilder<IncrementalValueProvider<ImmutableArray<RequestHandler>>>(HttpAttributeDefinitions.Length);

        foreach (var definition in HttpAttributeDefinitions)
        {
            var handlers = context.SyntaxProvider
                .ForAttributeWithMetadataName(definition.FullyQualifiedName, RequestHandlerFilter, RequestHandlerTransform)
                .WhereNotNull()
                .Collect();

            requestHandlerProviders.Add(handlers);
        }

        var requestHandlers = CombineRequestHandlers(requestHandlerProviders.MoveToImmutable());

        context.RegisterSourceOutput(requestHandlers, GenerateSource);
    }

    private static HttpAttributeDefinition CreateHttpAttributeDefinition(string attributeName, string verb, bool allowEmptyPattern = false)
    {
        var fullyQualifiedName = $"{AttributesNamespace}.{attributeName}";
        var hint = $"{fullyQualifiedName}.gs.cs";
        var summaryVerb = verb == FallbackHttpMethod ? "fallback" : verb;
        var source = GenerateHttpAttributeSource(AttributesNamespace, attributeName, summaryVerb, allowEmptyPattern);
        return new HttpAttributeDefinition(attributeName, fullyQualifiedName, hint, verb, allowEmptyPattern, SourceText.From(source, Encoding.UTF8));
    }

    private static IncrementalValueProvider<ImmutableArray<RequestHandler>> CombineRequestHandlers(
        ImmutableArray<IncrementalValueProvider<ImmutableArray<RequestHandler>>> handlerProviders
    )
    {
        if (handlerProviders.IsDefaultOrEmpty)
            throw new InvalidOperationException("No HTTP attribute definitions were provided.");

        var combined = handlerProviders[0];
        for (var i = 1; i < handlerProviders.Length; i++)
        {
            combined = combined.Combine(handlerProviders[i])
                .Select(static (x, _) => x.Left.AddRange(x.Right));
        }

        return combined;
    }

    private static void RegisterAttributes(IncrementalGeneratorPostInitializationContext context)
    {
        foreach (var definition in HttpAttributeDefinitions)
            context.AddSource(definition.Hint, definition.SourceText);

        context.AddSource(RequireAuthorizationAttributeHint, RequireAuthorizationAttributeSourceText);
        context.AddSource(RequireCorsAttributeHint, RequireCorsAttributeSourceText);
        context.AddSource(RequireRateLimitingAttributeHint, RequireRateLimitingAttributeSourceText);
        context.AddSource(RequireHostAttributeHint, RequireHostAttributeSourceText);
        context.AddSource(DisableAntiforgeryAttributeHint, DisableAntiforgeryAttributeSourceText);
        context.AddSource(ShortCircuitAttributeHint, ShortCircuitAttributeSourceText);
        context.AddSource(DisableRequestTimeoutAttributeHint, DisableRequestTimeoutAttributeSourceText);
        context.AddSource(DisableValidationAttributeHint, DisableValidationAttributeSourceText);
        context.AddSource(RequestTimeoutAttributeHint, RequestTimeoutAttributeSourceText);
        context.AddSource(OrderAttributeHint, OrderAttributeSourceText);
        context.AddSource(MapGroupAttributeHint, MapGroupAttributeSourceText);
        context.AddSource(SummaryAttributeHint, SummaryAttributeSourceText);
        context.AddSource(AcceptsAttributeHint, AcceptsAttributeSourceText);
        context.AddSource(EndpointFilterAttributeHint, EndpointFilterAttributeSourceText);
        context.AddSource(ProducesResponseAttributeHint, ProducesResponseAttributeSourceText);
        context.AddSource(ProducesProblemAttributeHint, ProducesProblemAttributeSourceText);
        context.AddSource(ProducesValidationProblemAttributeHint, ProducesValidationProblemAttributeSourceText);
    }

    private static string GenerateHttpAttributeSource(string attributesNamespace, string attributeName, string summaryVerb, bool allowEmptyPattern)
    {
        var patternDefaultValue = allowEmptyPattern ? " = \"\"" : string.Empty;
        return $$"""
                 {{FileHeader}}

                 namespace {{attributesNamespace}};

                 /// <summary>
                 /// Identifies a method as an HTTP {{summaryVerb}} minimal API endpoint with the specified route pattern.
                 /// </summary>
                 [global::System.AttributeUsage(global::System.AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
                 internal sealed class {{attributeName}} : global::System.Attribute
                 {
                     /// <summary>
                     /// Gets the route pattern for the endpoint.
                     /// </summary>
                     public string Pattern { get; }

                     /// <summary>
                     /// Gets or sets the endpoint name.
                     /// </summary>
                     public string? Name { get; init; }

                     /// <summary>
                     /// Initializes a new instance of the <see cref="{{attributeName}}"/> class.
                     /// </summary>
                     /// <param name="pattern">The route pattern for the endpoint.</param>
                     public {{attributeName}}([global::System.Diagnostics.CodeAnalysis.StringSyntax("Route")] string pattern{{patternDefaultValue}})
                      {
                          Pattern = pattern;
                      }
                 }
                 """;
    }

    private static bool RequestHandlerFilter(SyntaxNode syntaxNode, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return syntaxNode is MethodDeclarationSyntax;
    }

    private static RequestHandler? RequestHandlerTransform(GeneratorAttributeSyntaxContext context, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (context.TargetSymbol is not IMethodSymbol requestHandlerMethodSymbol)
            return null;
        var attribute = context.Attributes[0];

        var requestHandlerClass = GetRequestHandlerClass(requestHandlerMethodSymbol, context.SemanticModel.Compilation, cancellationToken);
        if (requestHandlerClass is null)
            return null;

        var requestHandlerMethod = GetRequestHandlerMethod(requestHandlerMethodSymbol, cancellationToken);

        var (httpMethod, pattern, name) = GetRequestHandlerAttribute(attribute, cancellationToken);

        var (displayName, description) = GetDisplayAndDescriptionAttributes(requestHandlerMethodSymbol);

        name ??= RemoveAsyncSuffix(requestHandlerMethod.Name);

        var methodConfiguration = GetEndpointConfiguration(requestHandlerMethodSymbol.GetAttributes(), name, displayName, description, true);

        var requestHandler = new RequestHandler(requestHandlerClass.Value, requestHandlerMethod, httpMethod, pattern, methodConfiguration);

        return requestHandler;
    }

    private static string RemoveAsyncSuffix(string methodName)
    {
        if (methodName.EndsWith(AsyncSuffix, StringComparison.OrdinalIgnoreCase) && methodName.Length > AsyncSuffix.Length)
            return methodName[..^AsyncSuffix.Length];

        return methodName;
    }

    private static ( string HttpMethod, string Pattern, string? Name ) GetRequestHandlerAttribute(AttributeData attribute, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var attributeName = attribute.AttributeClass?.Name ?? "";

        var httpMethod = HttpAttributeDefinitionsByName.TryGetValue(attributeName, out var definition) ? definition.Verb : "";

        var pattern = (attribute.ConstructorArguments.Length > 0 ? attribute.ConstructorArguments[0].Value as string : "") ?? "";

        string? name = null;
        foreach (var namedArg in attribute.NamedArguments)
        {
            switch (namedArg.Key)
            {
                case NameAttributeNamedParameter:
                {
                    var value = namedArg.Value.Value as string;
                    name = string.IsNullOrWhiteSpace(value) ? null : value!.Trim();
                    break;
                }
            }
        }

        return (httpMethod, pattern, name);
    }

    private static (string? DisplayName, string? Description) GetDisplayAndDescriptionAttributes(IMethodSymbol methodSymbol)
    {
        string? displayName = null;
        string? description = null;

        foreach (var attribute in methodSymbol.GetAttributes())
        {
            var attributeClass = attribute.AttributeClass;
            if (attributeClass is null)
                continue;

            if (IsAttribute(attributeClass, nameof(DisplayNameAttribute), ComponentModelNamespaceParts))
            {
                displayName = NormalizeOptionalString(attribute.ConstructorArguments.Length > 0 ? attribute.ConstructorArguments[0].Value as string : null);

                continue;
            }

            if (IsAttribute(attributeClass, nameof(DescriptionAttribute), ComponentModelNamespaceParts))
                description = NormalizeOptionalString(attribute.ConstructorArguments.Length > 0 ? attribute.ConstructorArguments[0].Value as string : null);
        }

        return (displayName, description);
    }

    private static EndpointConfiguration GetEndpointConfiguration(
        ImmutableArray<AttributeData> attributes,
        string? name,
        string? displayName,
        string? description,
        bool enforceMethodRequireAuthorizationRules
    )
    {
        var state = new EndpointAttributeState();

        GetAdditionalRequestHandlerAttributeValues(attributes, ref state);

        if (enforceMethodRequireAuthorizationRules && state is { HasRequireAuthorizationAttribute: true, HasAllowAnonymousAttribute: false })
            state.AllowAnonymous = false;

        var metadata = new RequestHandlerMetadata(name, displayName, state.Summary, description, state.Tags, ToEquatableOrNull(state.Accepts),
            ToEquatableOrNull(state.Produces), ToEquatableOrNull(state.ProducesProblem), ToEquatableOrNull(state.ProducesValidationProblem),
            state.ExcludeFromDescription ?? false
        );

        var withRequestTimeout = state.WithRequestTimeout ?? false;
        var requestTimeoutPolicyName = withRequestTimeout ? state.RequestTimeoutPolicyName : null;

        return new EndpointConfiguration(metadata, state.RequireAuthorization ?? false, state.AuthorizationPolicies, state.DisableAntiforgery ?? false,
            state.AllowAnonymous ?? false, state.RequireCors ?? false, state.CorsPolicyName, state.RequiredHosts, state.RequireRateLimiting ?? false,
            state.RateLimitingPolicyName, ToEquatableOrNull(state.EndpointFilters), state.ShortCircuit ?? false, state.DisableValidation ?? false,
            state.DisableRequestTimeout ?? false, withRequestTimeout, requestTimeoutPolicyName, state.Order, state.EndpointGroupName
        );
    }

    private static void GetAdditionalRequestHandlerAttributeValues(ImmutableArray<AttributeData> attributes, ref EndpointAttributeState state)
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

    private static string? NormalizeOptionalString(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : value!.Trim();
    }

    [SuppressMessage("Major Code Smell", "S3398:Move this method into a class of its own", Justification = "Shared helper for multiple caching paths.")]
    private static string? GetMapGroupPattern(INamedTypeSymbol classSymbol)
    {
        foreach (var attribute in classSymbol.GetAttributes())
        {
            var attributeClass = attribute.AttributeClass;
            if (attributeClass is null)
                continue;

            if (GetGeneratedAttributeKind(attributeClass) != GeneratedAttributeKind.MapGroup)
                continue;

            if (attribute.ConstructorArguments.Length > 0 && attribute.ConstructorArguments[0].Value is string pattern)
                return pattern.Trim();
        }

        return null;
    }

    [SuppressMessage("Major Code Smell", "S3398:Move this method into a class of its own", Justification = "Shared helper for multiple caching paths.")]
    private static string GetMapGroupIdentifier(string className)
    {
        if (className.StartsWith(GlobalPrefix, StringComparison.Ordinal))
            className = className.Substring(GlobalPrefix.Length);

        var builder = StringBuilderPool.Get(className.Length + 8);
        builder.Append('_');

        foreach (var character in className)
            builder.Append(char.IsLetterOrDigit(character) ? character : '_');

        builder.Append("_Group");
        return StringBuilderPool.ToStringAndReturn(builder);
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

    private static GeneratedAttributeKind GetGeneratedAttributeKind(INamedTypeSymbol attributeClass)
    {
        var definition = attributeClass.OriginalDefinition;
        var cacheEntry =
            GeneratedAttributeKindCache.GetValue(definition, static def => new GeneratedAttributeKindCacheEntry(GetGeneratedAttributeKindCore(def)));

        return cacheEntry.Kind;
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

    private static bool IsAttribute(INamedTypeSymbol attributeClass, string attributeName, string[] namespaceParts)
    {
        var definition = attributeClass.OriginalDefinition;
        return definition.Name == attributeName && IsInNamespace(definition.ContainingNamespace, namespaceParts);
    }

    private static bool IsInNamespace(INamespaceSymbol? namespaceSymbol, string[] namespaceParts)
    {
        for (var i = namespaceParts.Length - 1; i >= 0; i--)
        {
            if (namespaceSymbol is null || namespaceSymbol.Name != namespaceParts[i])
                return false;

            namespaceSymbol = namespaceSymbol.ContainingNamespace;
        }

        return namespaceSymbol is null || namespaceSymbol.IsGlobalNamespace;
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

    private static EquatableImmutableArray<string> MergeUnion(EquatableImmutableArray<string>? existing, IEnumerable<string> values)
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

            if (list is null)
                list = [];

            list.Add(normalized);
        }

        return list?.ToEquatableImmutableArray() ?? EquatableImmutableArray<string>.Empty;
    }

    private static RequestHandlerMethod GetRequestHandlerMethod(IMethodSymbol methodSymbol, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var name = methodSymbol.Name;
        var isStatic = methodSymbol.IsStatic;
        var isAwaitable = methodSymbol.ReturnType.IsTask(out _) || methodSymbol.ReturnType.IsValueTask(out _);
        var parameters = GetRequestHandlerParameters(methodSymbol, cancellationToken);

        var requestHandlerMethod = new RequestHandlerMethod(name, isStatic, isAwaitable, parameters);

        return requestHandlerMethod;
    }

    private static RequestHandlerClass? GetRequestHandlerClass(IMethodSymbol methodSymbol, Compilation compilation, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var classSymbol = methodSymbol.ContainingType;
        if (classSymbol.TypeKind != TypeKind.Class)
            return null;

        var typeCache = GetCompilationTypeCache(compilation);
        var cacheEntry = RequestHandlerClassCache.GetValue(classSymbol, static _ => new RequestHandlerClassCacheEntry());
        var requestHandlerClass = cacheEntry.GetOrCreate(classSymbol, typeCache, cancellationToken);
        return requestHandlerClass;
    }

    private static CompilationTypeCache GetCompilationTypeCache(Compilation compilation)
    {
        return CompilationTypeCaches.GetValue(compilation, static c => new CompilationTypeCache(c));
    }

    [SuppressMessage("Major Code Smell", "S3398:Move this method into a class of its own", Justification = "Shared helper reused by caching infrastructure.")]
    private static ConfigureMethodDetails GetConfigureMethodDetails(
        INamedTypeSymbol classSymbol,
        INamedTypeSymbol? endpointConventionBuilderSymbol,
        INamedTypeSymbol? serviceProviderSymbol,
        CancellationToken cancellationToken
    )
    {
        cancellationToken.ThrowIfCancellationRequested();

        var hasConfigureMethod = false;
        var acceptsServiceProvider = false;
        foreach (var member in classSymbol.GetMembers(ConfigureMethodName))
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (member is not IMethodSymbol methodSymbol)
                continue;

            if (IsConfigureMethod(methodSymbol, endpointConventionBuilderSymbol, serviceProviderSymbol, out var methodAcceptsServiceProvider))
            {
                hasConfigureMethod = true;
                if (methodAcceptsServiceProvider)
                {
                    acceptsServiceProvider = true;
                    break;
                }
            }
        }

        return new ConfigureMethodDetails(hasConfigureMethod, acceptsServiceProvider);
    }

    private static bool IsConfigureMethod(
        IMethodSymbol methodSymbol,
        INamedTypeSymbol? endpointConventionBuilderSymbol,
        INamedTypeSymbol? serviceProviderSymbol,
        out bool acceptsServiceProvider
    )
    {
        acceptsServiceProvider = false;

        if (!methodSymbol.IsStatic)
            return false;

        if (methodSymbol.TypeParameters.Length != 1)
            return false;

        if (methodSymbol.Parameters.Length is < 1 or > 2)
            return false;

        var builderTypeParameter = methodSymbol.TypeParameters[0];
        var builderParameter = methodSymbol.Parameters[0];

        if (!SymbolEqualityComparer.Default.Equals(builderParameter.Type, builderTypeParameter))
            return false;

        if (methodSymbol.Parameters.Length == 2)
        {
            var serviceProviderParameter = methodSymbol.Parameters[1];
            if (!IsServiceProviderParameter(serviceProviderParameter.Type, serviceProviderSymbol))
                return false;

            acceptsServiceProvider = true;
        }

        if (!methodSymbol.ReturnsVoid)
            return false;

        if (!HasEndpointConventionBuilderConstraint(builderTypeParameter, methodSymbol, endpointConventionBuilderSymbol))
            return false;

        return true;
    }

    private static bool IsServiceProviderParameter(ITypeSymbol typeSymbol, INamedTypeSymbol? serviceProviderSymbol)
    {
        if (serviceProviderSymbol is not null)
            return SymbolEqualityComparer.Default.Equals(typeSymbol, serviceProviderSymbol);

        return MatchesServiceProvider(typeSymbol);
    }

    private static bool HasEndpointConventionBuilderConstraint(
        ITypeParameterSymbol builderTypeParameter,
        IMethodSymbol methodSymbol,
        INamedTypeSymbol? endpointConventionBuilderSymbol
    )
    {
        var symbolMatches = builderTypeParameter.ConstraintTypes.Any(constraint =>
            endpointConventionBuilderSymbol is not null
                ? SymbolEqualityComparer.Default.Equals(constraint, endpointConventionBuilderSymbol)
                : MatchesEndpointConventionBuilder(constraint)
        );

        if (symbolMatches)
            return true;

        return methodSymbol.DeclaringSyntaxReferences
            .Select(reference => reference.GetSyntax())
            .OfType<MethodDeclarationSyntax>()
            .SelectMany(methodSyntax => methodSyntax.ConstraintClauses)
            .Where(clause => string.Equals(clause.Name.Identifier.ValueText, builderTypeParameter.Name, StringComparison.Ordinal))
            .SelectMany(clause => clause.Constraints.OfType<TypeConstraintSyntax>())
            .Any(constraint => IsEndpointConventionBuilderIdentifier(constraint.Type));
    }

    private static bool IsEndpointConventionBuilderIdentifier(TypeSyntax typeSyntax)
    {
        return typeSyntax switch
        {
            QualifiedNameSyntax qualified => IsEndpointConventionBuilderIdentifier(qualified.Right),
            AliasQualifiedNameSyntax alias => IsEndpointConventionBuilderIdentifier(alias.Name),
            SimpleNameSyntax simple => string.Equals(simple.Identifier.ValueText, "IEndpointConventionBuilder", StringComparison.Ordinal),
            _ => false,
        };
    }

    private static bool MatchesEndpointConventionBuilder(ITypeSymbol typeSymbol)
    {
        if (typeSymbol is not INamedTypeSymbol namedType)
            return false;

        if (!string.Equals(namedType.Name, "IEndpointConventionBuilder", StringComparison.Ordinal))
            return false;

        var containingNamespace = namedType.ContainingNamespace?.ToDisplayString() ?? string.Empty;
        return string.Equals(containingNamespace, "Microsoft.AspNetCore.Builder", StringComparison.Ordinal);
    }

    private static bool MatchesServiceProvider(ITypeSymbol typeSymbol)
    {
        if (typeSymbol is not INamedTypeSymbol namedType)
            return false;

        if (!string.Equals(namedType.Name, "IServiceProvider", StringComparison.Ordinal))
            return false;

        var containingNamespace = namedType.ContainingNamespace?.ToDisplayString() ?? string.Empty;
        return string.Equals(containingNamespace, "System", StringComparison.Ordinal);
    }

    private static EquatableImmutableArray<Parameter> GetRequestHandlerParameters(IMethodSymbol methodSymbol, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var methodParameters = new List<Parameter>(methodSymbol.Parameters.Length);
        foreach (var parameter in methodSymbol.Parameters)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var source = BindingSource.None;
            TypedConstant? typedKey = null;
            string? bindingName = null;

            var attributes = parameter.GetAttributes();
            foreach (var attribute in attributes)
            {
                var attributeClass = attribute.AttributeClass;
                if (attributeClass is null)
                    continue;

                if (attributeClass.IsFromRouteAttribute())
                {
                    source = BindingSource.FromRoute;
                    bindingName = GetBindingAttributeName(attribute) ?? bindingName;
                }
                if (attributeClass.IsFromQueryAttribute())
                {
                    source = BindingSource.FromQuery;
                    bindingName = GetBindingAttributeName(attribute) ?? bindingName;
                }
                if (attributeClass.IsFromHeaderAttribute())
                {
                    source = BindingSource.FromHeader;
                    bindingName = GetBindingAttributeName(attribute) ?? bindingName;
                }
                if (attributeClass.IsFromBodyAttribute())
                    source = BindingSource.FromBody;
                if (attributeClass.IsFromFormAttribute())
                {
                    source = BindingSource.FromForm;
                    bindingName = GetBindingAttributeName(attribute) ?? bindingName;
                }
                if (attributeClass.IsFromServicesAttribute())
                    source = BindingSource.FromServices;
                if (attributeClass.IsFromKeyedServicesAttribute())
                {
                    source = BindingSource.FromKeyedServices;
                    typedKey = attribute.ConstructorArguments.Length > 0 ? attribute.ConstructorArguments[0] : null;
                }
                if (attributeClass.IsAsParametersAttribute())
                    source = BindingSource.AsParameters;
            }

            var parameterName = parameter.Name;
            var parameterType = parameter.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            var key = typedKey.HasValue ? ConstLiteral(typedKey.Value) : null;
            var bindingPrefix = GetBindingSourceAttribute(source, key, bindingName);
            methodParameters.Add(new Parameter(parameterName, parameterType, bindingPrefix));
        }

        return methodParameters.ToEquatableImmutableArray();
    }

    private static string? GetBindingAttributeName(AttributeData attribute)
    {
        foreach (var namedArg in attribute.NamedArguments)
        {
            if (string.Equals(namedArg.Key, NameAttributeNamedParameter, StringComparison.Ordinal) && namedArg.Value.Value is string namedValue)
            {
                var normalized = NormalizeBindingName(namedValue);
                if (normalized is not null)
                    return normalized;
            }
        }

        if (attribute.ConstructorArguments.Length > 0 && attribute.ConstructorArguments[0].Value is string constructorName)
            return NormalizeBindingName(constructorName);

        return null;
    }

    private static string? NormalizeBindingName(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;

        var trimmed = value!.Trim();
        return trimmed.Length > 0 ? trimmed : null;
    }

    private static void GenerateSource(SourceProductionContext context, ImmutableArray<RequestHandler> requestHandlers)
    {
        var sorted = SortRequestHandlers(requestHandlers);
        sorted = EnsureUniqueEndpointNames(sorted);

        GenerateAddEndpointHandlersClass(context, sorted);
        GenerateUseEndpointHandlersClass(context, sorted);
    }

    private static ImmutableArray<RequestHandler> SortRequestHandlers(ImmutableArray<RequestHandler> requestHandlers)
    {
        if (requestHandlers.Length <= 1)
            return requestHandlers;

        var array = requestHandlers.ToArray();
        Array.Sort(array, RequestHandlerComparer.Instance);
        return [..array];
    }

    private static ImmutableArray<RequestHandler> EnsureUniqueEndpointNames(ImmutableArray<RequestHandler> requestHandlers)
    {
        var collidingHandlers = GetRequestHandlersWithNameCollisions(requestHandlers);
        if (collidingHandlers.IsEmpty)
            return requestHandlers;

        var builder = requestHandlers.ToBuilder();
        foreach (var index in collidingHandlers)
        {
            var handler = builder[index];
            var configuration = handler.Configuration;
            var metadata = configuration.Metadata with
            {
                Name = GetFullyQualifiedMethodDisplayName(handler),
            };
            configuration = configuration with
            {
                Metadata = metadata,
            };
            builder[index] = handler with
            {
                Configuration = configuration,
            };
        }

        return builder.MoveToImmutable();
    }

    private static ImmutableArray<int> GetRequestHandlersWithNameCollisions(ImmutableArray<RequestHandler> requestHandlers)
    {
        if (requestHandlers.IsDefaultOrEmpty)
            return ImmutableArray<int>.Empty;

        var handlerCount = requestHandlers.Length;
        var nameToFirstIndex = new Dictionary<HandlerNameKey, int>(handlerCount);
        var collisionFlags = ArrayPool<bool>.Shared.Rent(handlerCount);
        Array.Clear(collisionFlags, 0, handlerCount);
        List<int>? collidingIndices = null;

        try
        {
            for (var index = 0; index < handlerCount; index++)
            {
                var handler = requestHandlers[index];
                var name = handler.Configuration.Metadata.Name;
                if (string.IsNullOrEmpty(name))
                    continue;

                var key = new HandlerNameKey(name!, handler.Method.Name);

                if (nameToFirstIndex.TryGetValue(key, out var firstIndex))
                {
                    MarkCollision(firstIndex);
                    MarkCollision(index);
                }
                else
                {
                    nameToFirstIndex.Add(key, index);
                }
            }

            if (collidingIndices is null || collidingIndices.Count == 0)
                return ImmutableArray<int>.Empty;

            collidingIndices.Sort();
            var builder = ImmutableArray.CreateBuilder<int>(collidingIndices.Count);
            builder.AddRange(collidingIndices);
            return builder.MoveToImmutable();
        }
        finally
        {
            ArrayPool<bool>.Shared.Return(collisionFlags);
        }

        void MarkCollision(int handlerIndex)
        {
            if (collisionFlags[handlerIndex])
                return;

            collisionFlags[handlerIndex] = true;
            collidingIndices ??= new List<int>();
            collidingIndices.Add(handlerIndex);
        }
    }

    private static string GetFullyQualifiedMethodDisplayName(RequestHandler requestHandler)
    {
        var className = requestHandler.Class.Name;
        if (className.StartsWith(GlobalPrefix, StringComparison.Ordinal))
            className = className.Substring(GlobalPrefix.Length);

        if (className.IndexOf('+') >= 0)
            className = className.Replace('+', '.');

        return string.Concat(className, ".", requestHandler.Method.Name);
    }

    private static void GenerateAddEndpointHandlersClass(SourceProductionContext context, ImmutableArray<RequestHandler> requestHandlers)
    {
        var nonStaticClassNames = GetDistinctNonStaticClassNames(requestHandlers);
        var source = GetAddEndpointHandlersStringBuilder(nonStaticClassNames);
        source.AppendLine(FileHeader);

        source.AppendLine();

        source.AppendLine("using Microsoft.Extensions.DependencyInjection;");
        source.AppendLine("using Microsoft.Extensions.DependencyInjection.Extensions;");
        source.AppendLine();

        source.Append("namespace ");
        source.Append(RoutingNamespace);
        source.AppendLine(";");

        source.AppendLine();

        source.Append("internal static class ");
        source.Append(AddEndpointHandlersClassName);
        source.AppendLine();

        source.AppendLine("{");

        source.Append("    internal static void ");
        source.Append(AddEndpointHandlersMethodName);
        source.AppendLine("(this IServiceCollection services)");

        source.AppendLine("    {");

        foreach (var className in nonStaticClassNames)
        {
            source.Append("        services.TryAddScoped<");
            source.Append(className);
            source.Append(">();");
            source.AppendLine();
        }

        source.AppendLine("""
                              }
                          }
                          """
        );

        var sourceText = StringBuilderPool.ToStringAndReturn(source);
        context.AddSource(AddEndpointHandlersMethodHint, SourceText.From(sourceText, Encoding.UTF8));
    }

    [SuppressMessage("Major Code Smell", "S3267:Loops should be simplified by calling the \"Select\" LINQ method",
        Justification = "Manual loops avoid repeated allocations in the source generator."
    )]
    private static List<string> GetDistinctNonStaticClassNames(ImmutableArray<RequestHandler> requestHandlers)
    {
        var classNames = new List<string>();
        if (requestHandlers.IsDefaultOrEmpty)
            return classNames;

        var seen = new HashSet<string>(StringComparer.Ordinal);
        foreach (var requestHandler in requestHandlers)
        {
            if (requestHandler.Class.IsStatic)
                continue;

            var className = requestHandler.Class.Name;
            if (seen.Add(className))
                classNames.Add(className);
        }

        return classNames;
    }

    private static StringBuilder GetAddEndpointHandlersStringBuilder(List<string> nonStaticClassNames)
    {
        var estimate = 512L;
        foreach (var className in nonStaticClassNames)
            estimate += 36 + className.Length;

        estimate += Math.Max(256, nonStaticClassNames.Count * 12);
        estimate = (long)(estimate * 1.10);

        if (estimate < 512)
            estimate = 512;
        else if (estimate > int.MaxValue)
            estimate = int.MaxValue;

        return StringBuilderPool.Get((int)estimate);
    }

    private static void GenerateUseEndpointHandlersClass(SourceProductionContext context, ImmutableArray<RequestHandler> requestHandlers)
    {
        var source = GetUseEndpointHandlersStringBuilder(requestHandlers);
        source.AppendLine(FileHeader);

        source.AppendLine();

        source.AppendLine("using Microsoft.AspNetCore.Builder;");
        source.AppendLine("using Microsoft.AspNetCore.Http;");
        source.AppendLine("using Microsoft.AspNetCore.Mvc;");
        source.AppendLine("using Microsoft.AspNetCore.Routing;");
        if (HasRateLimitedHandlers(requestHandlers))
            source.AppendLine("using Microsoft.AspNetCore.RateLimiting;");
        source.AppendLine("using Microsoft.Extensions.DependencyInjection;");
        source.AppendLine();

        source.Append("namespace ");
        source.Append(RoutingNamespace);
        source.AppendLine(";");

        source.AppendLine();

        source.Append("internal static class ");
        source.Append(UseEndpointHandlersClassName);
        source.AppendLine();

        source.AppendLine("{");

        source.Append("    internal static IEndpointRouteBuilder ");
        source.Append(UseEndpointHandlersMethodName);
        source.AppendLine("(this IEndpointRouteBuilder builder)");

        source.AppendLine("    {");

        var groupedClasses = GetClassesWithMapGroups(requestHandlers);

        foreach (var groupedClass in groupedClasses)
        {
            source.Append("        var ");
            source.Append(groupedClass.MapGroupBuilderIdentifier);
            source.Append(" = builder.MapGroup(");
            source.Append(StringLiteral(groupedClass.MapGroupPattern!));
            source.Append(')');
            AppendEndpointConfiguration(source, "            ", groupedClass.Configuration, false);
            source.AppendLine(";");
        }

        if (groupedClasses.Count > 0)
            source.AppendLine();

        for (var index = 0; index < requestHandlers.Length; index++)
        {
            if (index > 0)
                source.AppendLine();

            var requestHandler = requestHandlers[index];
            GenerateMapRequestHandler(source, requestHandler);
        }

        source.AppendLine("""

                                  return builder;
                              }
                          }
                          """
        );

        var sourceText = StringBuilderPool.ToStringAndReturn(source);
        context.AddSource(UseEndpointHandlersMethodHint, SourceText.From(sourceText, Encoding.UTF8));
    }

    private static bool HasRateLimitedHandlers(ImmutableArray<RequestHandler> requestHandlers)
    {
        foreach (var handler in requestHandlers)
        {
            if (handler.Configuration.RequireRateLimiting)
                return true;
        }

        return false;
    }

    [SuppressMessage("Major Code Smell", "S3267:Loops should be simplified by calling the \"Select\" LINQ method",
        Justification = "Manual loops avoid repeated allocations in the source generator."
    )]
    private static List<RequestHandlerClass> GetClassesWithMapGroups(ImmutableArray<RequestHandler> requestHandlers)
    {
        var groupedClasses = new List<RequestHandlerClass>();
        if (requestHandlers.IsDefaultOrEmpty)
            return groupedClasses;

        var seen = new HashSet<string>(StringComparer.Ordinal);
        foreach (var handler in requestHandlers)
        {
            var handlerClass = handler.Class;
            if (handlerClass.MapGroupPattern is null)
                continue;

            if (seen.Add(handlerClass.Name))
                groupedClasses.Add(handlerClass);
        }

        return groupedClasses;
    }

    private static void GenerateMapRequestHandler(StringBuilder source, RequestHandler requestHandler)
    {
        var wrapWithConfigure = requestHandler.Class.HasConfigureMethod;
        var configureAcceptsServiceProvider = requestHandler.Class.ConfigureMethodAcceptsServiceProvider;
        var indent = wrapWithConfigure ? "            " : "        ";
        var continuationIndent = indent + "    ";
        var routeBuilderIdentifier = requestHandler.Class.MapGroupBuilderIdentifier ?? "builder";

        if (wrapWithConfigure)
        {
            source.Append("        ");
            source.Append(requestHandler.Class.Name);
            source.Append('.');
            source.Append(ConfigureMethodName);
            source.AppendLine("(");
        }

        var isFallback = string.Equals(requestHandler.HttpMethod, FallbackHttpMethod, StringComparison.Ordinal);
        var mapMethodSuffix = isFallback ? null : GetMapMethodSuffix(requestHandler.HttpMethod);

        source.Append(indent);
        if (isFallback)
        {
            source.Append(routeBuilderIdentifier);
            source.Append(".MapFallback(");
            if (!string.IsNullOrEmpty(requestHandler.Pattern))
            {
                source.Append(StringLiteral(requestHandler.Pattern));
                source.Append(", ");
            }
        }
        else
        {
            source.Append(routeBuilderIdentifier);
            source.Append(".Map");
            source.Append(mapMethodSuffix ?? "Methods");
            source.Append('(');
            source.Append(StringLiteral(requestHandler.Pattern));
            source.Append(", ");
            if (mapMethodSuffix is null)
            {
                source.Append("new[] { \"");
                source.Append(requestHandler.HttpMethod);
                source.Append("\" }, ");
            }
        }
        if (requestHandler.Method.IsStatic)
        {
            source.Append(requestHandler.Class.Name);
            source.Append('.');
            source.Append(requestHandler.Method.Name);
        }
        else
        {
            source.Append("static ");
            if (requestHandler.Method.IsAwaitable)
                source.Append("async ");
            source.Append("([FromServices] ");
            source.Append(requestHandler.Class.Name);
            source.Append(" handler");
            foreach (var parameter in requestHandler.Method.Parameters)
            {
                source.Append(", ");
                source.Append(parameter.BindingPrefix);
                source.Append(parameter.Type);
                source.Append(' ');
                source.Append(parameter.Name);
            }
            source.Append(") => ");
            if (requestHandler.Method.IsAwaitable)
                source.Append("await ");
            source.Append("handler.");
            source.Append(requestHandler.Method.Name);
            source.Append('(');
            for (var index = 0; index < requestHandler.Method.Parameters.Count; index++)
            {
                if (index > 0)
                    source.Append(", ");
                var parameter = requestHandler.Method.Parameters[index];
                source.Append(parameter.Name);
            }
            source.Append(')');
        }
        source.Append(')');

        var configuration = requestHandler.Configuration;
        if (requestHandler.Class.MapGroupPattern is null)
            configuration = MergeEndpointConfigurations(requestHandler.Class.Configuration, configuration);

        AppendEndpointConfiguration(source, continuationIndent, configuration, true);

        if (wrapWithConfigure && configureAcceptsServiceProvider)
        {
            source.AppendLine(",");
            source.Append(indent);
            source.Append("builder.ServiceProvider");
        }

        if (wrapWithConfigure)
        {
            source.AppendLine();
            source.Append("        );");
            source.AppendLine();
        }
        else
        {
            source.AppendLine(";");
        }
    }

    private static void AppendEndpointConfiguration(StringBuilder source, string indent, EndpointConfiguration configuration, bool includeNameAndDisplayName)
    {
        var metadata = configuration.Metadata;

        if (includeNameAndDisplayName && !string.IsNullOrEmpty(metadata.Name))
        {
            source.AppendLine();
            source.Append(indent);
            source.Append(".WithName(");
            source.Append(StringLiteral(metadata.Name));
            source.Append(')');
        }

        if (includeNameAndDisplayName && !string.IsNullOrEmpty(metadata.DisplayName))
        {
            source.AppendLine();
            source.Append(indent);
            source.Append(".WithDisplayName(");
            source.Append(StringLiteral(metadata.DisplayName));
            source.Append(')');
        }

        if (!string.IsNullOrEmpty(metadata.Summary))
        {
            source.AppendLine();
            source.Append(indent);
            source.Append(".WithSummary(");
            source.Append(StringLiteral(metadata.Summary));
            source.Append(')');
        }

        if (!string.IsNullOrEmpty(metadata.Description))
        {
            source.AppendLine();
            source.Append(indent);
            source.Append(".WithDescription(");
            source.Append(StringLiteral(metadata.Description));
            source.Append(')');
        }

        if (!string.IsNullOrEmpty(configuration.EndpointGroupName))
        {
            source.AppendLine();
            source.Append(indent);
            source.Append(".WithGroupName(");
            source.Append(StringLiteral(configuration.EndpointGroupName));
            source.Append(')');
        }

        if (configuration.Order is { } orderValue)
        {
            source.AppendLine();
            source.Append(indent);
            source.Append(".WithOrder(");
            source.Append(orderValue);
            source.Append(')');
        }

        if (metadata.ExcludeFromDescription)
        {
            source.AppendLine();
            source.Append(indent);
            source.Append(".ExcludeFromDescription()");
        }

        if (metadata.Tags is { Count: > 0 })
        {
            source.AppendLine();
            source.Append(indent);
            source.Append(".WithTags(");
            AppendCommaSeparatedLiterals(source, metadata.Tags.Value);
            source.Append(')');
        }

        if (metadata.Accepts is { Count: > 0 })
            foreach (var accepts in metadata.Accepts.Value)
            {
                source.AppendLine();
                source.Append(indent);
                source.Append(".Accepts<");
                source.Append(accepts.RequestType);
                source.Append('>');
                source.Append('(');
                if (accepts.IsOptional)
                    source.Append("isOptional: true, ");
                source.Append(StringLiteral(accepts.ContentType));
                AppendAdditionalContentTypes(source, accepts.AdditionalContentTypes);
                source.Append(')');
            }

        if (metadata.Produces is { Count: > 0 })
            foreach (var produces in metadata.Produces.Value)
            {
                source.AppendLine();
                source.Append(indent);
                source.Append(".Produces<");
                source.Append(produces.ResponseType);
                source.Append('>');
                source.Append('(');
                source.Append(produces.StatusCode);
                AppendOptionalContentTypes(source, produces.ContentType, produces.AdditionalContentTypes);
                source.Append(')');
            }

        if (metadata.ProducesProblem is { Count: > 0 })
            foreach (var producesProblem in metadata.ProducesProblem.Value)
            {
                source.AppendLine();
                source.Append(indent);
                source.Append(".ProducesProblem(");
                source.Append(producesProblem.StatusCode);
                AppendOptionalContentTypes(source, producesProblem.ContentType, producesProblem.AdditionalContentTypes);
                source.Append(')');
            }

        if (metadata.ProducesValidationProblem is { Count: > 0 })
            foreach (var producesValidationProblem in metadata.ProducesValidationProblem.Value)
            {
                source.AppendLine();
                source.Append(indent);
                source.Append(".ProducesValidationProblem(");
                source.Append(producesValidationProblem.StatusCode);
                AppendOptionalContentTypes(source, producesValidationProblem.ContentType, producesValidationProblem.AdditionalContentTypes);
                source.Append(')');
            }

        if (configuration.RequireAuthorization)
        {
            source.AppendLine();
            if (configuration.AuthorizationPolicies is { Count: > 0 })
            {
                source.Append(indent);
                source.Append(".RequireAuthorization(");
                AppendCommaSeparatedLiterals(source, configuration.AuthorizationPolicies.Value);
                source.Append(')');
            }
            else
            {
                source.Append(indent);
                source.Append(".RequireAuthorization()");
            }
        }

        if (configuration.RequireCors)
        {
            source.AppendLine();
            source.Append(indent);
            if (!string.IsNullOrEmpty(configuration.CorsPolicyName))
            {
                source.Append(".RequireCors(");
                source.Append(StringLiteral(configuration.CorsPolicyName));
                source.Append(')');
            }
            else
            {
                source.Append(".RequireCors()");
            }
        }

        if (configuration.RequiredHosts is { Count: > 0 })
        {
            source.AppendLine();
            source.Append(indent);
            source.Append(".RequireHost(");
            AppendCommaSeparatedLiterals(source, configuration.RequiredHosts.Value);
            source.Append(')');
        }

        if (configuration.RequireRateLimiting && !string.IsNullOrEmpty(configuration.RateLimitingPolicyName))
        {
            source.AppendLine();
            source.Append(indent);
            source.Append(".RequireRateLimiting(");
            source.Append(StringLiteral(configuration.RateLimitingPolicyName));
            source.Append(')');
        }

        if (configuration.DisableAntiforgery)
        {
            source.AppendLine();
            source.Append(indent);
            source.Append(".DisableAntiforgery()");
        }

        if (configuration.AllowAnonymous)
        {
            source.AppendLine();
            source.Append(indent);
            source.Append(".AllowAnonymous()");
        }

        if (configuration.ShortCircuit)
        {
            source.AppendLine();
            source.Append(indent);
            source.Append(".ShortCircuit()");
        }

        if (configuration.DisableValidation)
        {
            source.AppendLine();
            source.Append(indent);
            source.Append(".DisableValidation()");
            source.AppendLine();
        }

        if (configuration.DisableRequestTimeout)
        {
            source.AppendLine();
            source.Append(indent);
            source.Append(".DisableRequestTimeout()");
        }
        else if (configuration.WithRequestTimeout)
        {
            source.AppendLine();
            source.Append(indent);
            if (!string.IsNullOrEmpty(configuration.RequestTimeoutPolicyName))
            {
                source.Append(".WithRequestTimeout(");
                source.Append(StringLiteral(configuration.RequestTimeoutPolicyName));
                source.Append(')');
            }
            else
            {
                source.Append(".WithRequestTimeout()");
            }
        }

        if (configuration.EndpointFilterTypes is { Count: > 0 })
            foreach (var filterType in configuration.EndpointFilterTypes.Value)
            {
                source.AppendLine();
                source.Append(indent);
                source.Append(".AddEndpointFilter<");
                source.Append(filterType);
                source.Append(">()");
            }
    }

    private static EndpointConfiguration MergeEndpointConfigurations(EndpointConfiguration classConfiguration, EndpointConfiguration methodConfiguration)
    {
        var metadata = MergeRequestHandlerMetadata(classConfiguration.Metadata, methodConfiguration.Metadata);
        var authorizationPolicies = MergeDistinctStrings(classConfiguration.AuthorizationPolicies, methodConfiguration.AuthorizationPolicies);
        var requiredHosts = MergeDistinctStrings(classConfiguration.RequiredHosts, methodConfiguration.RequiredHosts);
        var endpointFilterTypes = ConcatEquatable(classConfiguration.EndpointFilterTypes, methodConfiguration.EndpointFilterTypes);
        var requireAuthorization = classConfiguration.RequireAuthorization || methodConfiguration.RequireAuthorization;
        var disableAntiforgery = classConfiguration.DisableAntiforgery || methodConfiguration.DisableAntiforgery;
        var allowAnonymous = classConfiguration.AllowAnonymous || methodConfiguration.AllowAnonymous;
        var requireCors = classConfiguration.RequireCors || methodConfiguration.RequireCors;
        var corsPolicyName = methodConfiguration.CorsPolicyName ?? classConfiguration.CorsPolicyName;
        var requireRateLimiting = classConfiguration.RequireRateLimiting || methodConfiguration.RequireRateLimiting;
        var rateLimitingPolicyName = methodConfiguration.RateLimitingPolicyName ?? classConfiguration.RateLimitingPolicyName;
        var shortCircuit = classConfiguration.ShortCircuit || methodConfiguration.ShortCircuit;
        var disableValidation = classConfiguration.DisableValidation || methodConfiguration.DisableValidation;
        var disableRequestTimeout = classConfiguration.DisableRequestTimeout || methodConfiguration.DisableRequestTimeout;
        var withRequestTimeout = classConfiguration.WithRequestTimeout || methodConfiguration.WithRequestTimeout;
        string? requestTimeoutPolicyName = null;
        if (methodConfiguration.WithRequestTimeout)
            requestTimeoutPolicyName = methodConfiguration.RequestTimeoutPolicyName;
        else if (classConfiguration.WithRequestTimeout)
            requestTimeoutPolicyName = classConfiguration.RequestTimeoutPolicyName;

        if (disableRequestTimeout)
        {
            withRequestTimeout = false;
            requestTimeoutPolicyName = null;
        }

        var order = methodConfiguration.Order ?? classConfiguration.Order;
        var endpointGroupName = methodConfiguration.EndpointGroupName ?? classConfiguration.EndpointGroupName;

        return new EndpointConfiguration(metadata, requireAuthorization, authorizationPolicies, disableAntiforgery, allowAnonymous, requireCors, corsPolicyName,
            requiredHosts, requireRateLimiting, rateLimitingPolicyName, endpointFilterTypes, shortCircuit, disableValidation, disableRequestTimeout,
            withRequestTimeout, requestTimeoutPolicyName, order, endpointGroupName
        );
    }

    private static RequestHandlerMetadata MergeRequestHandlerMetadata(RequestHandlerMetadata classMetadata, RequestHandlerMetadata methodMetadata)
    {
        return new RequestHandlerMetadata(methodMetadata.Name ?? classMetadata.Name, methodMetadata.DisplayName ?? classMetadata.DisplayName,
            methodMetadata.Summary ?? classMetadata.Summary, methodMetadata.Description ?? classMetadata.Description,
            MergeDistinctStrings(classMetadata.Tags, methodMetadata.Tags), ConcatEquatable(classMetadata.Accepts, methodMetadata.Accepts),
            ConcatEquatable(classMetadata.Produces, methodMetadata.Produces), ConcatEquatable(classMetadata.ProducesProblem, methodMetadata.ProducesProblem),
            ConcatEquatable(classMetadata.ProducesValidationProblem, methodMetadata.ProducesValidationProblem),
            classMetadata.ExcludeFromDescription || methodMetadata.ExcludeFromDescription
        );
    }

    private static EquatableImmutableArray<string>? MergeDistinctStrings(EquatableImmutableArray<string>? first, EquatableImmutableArray<string>? second)
    {
        if (first is not { Count: > 0 })
            return second;
        if (second is not { Count: > 0 })
            return first;

        var merged = MergeUnion(first, second.Value);
        return merged.Count > 0 ? merged : null;
    }

    private static EquatableImmutableArray<T>? ConcatEquatable<T>(EquatableImmutableArray<T>? first, EquatableImmutableArray<T>? second)
    {
        if (first is not { Count: > 0 })
            return second;
        if (second is not { Count: > 0 })
            return first;

        var builder = ImmutableArray.CreateBuilder<T>(first.Value.Count + second.Value.Count);
        builder.AddRange(first.Value);
        builder.AddRange(second.Value);
        return builder.ToEquatableImmutableArray();
    }

    private static string? GetMapMethodSuffix(string httpMethod)
    {
        return httpMethod switch
        {
            "GET" => "Get",
            "POST" => "Post",
            "PUT" => "Put",
            "DELETE" => "Delete",
            "PATCH" => "Patch",
            _ => null,
        };
    }

    private static string GetBindingSourceAttribute(BindingSource source, string? key, string? bindingName)
    {
        return source switch
        {
            BindingSource.None => "",
            BindingSource.FromRoute => FormatBindingAttribute("FromRoute", bindingName),
            BindingSource.FromQuery => FormatBindingAttribute("FromQuery", bindingName),
            BindingSource.FromHeader => FormatBindingAttribute("FromHeader", bindingName),
            BindingSource.FromBody => FormatBindingAttribute("FromBody", bindingName),
            BindingSource.FromForm => FormatBindingAttribute("FromForm", bindingName),
            BindingSource.FromServices => "[FromServices] ",
            BindingSource.FromKeyedServices => $"[FromKeyedServices({key})] ",
            BindingSource.AsParameters => "[AsParameters] ",
            _ => throw new NotImplementedException(),
        };
    }

    private static string FormatBindingAttribute(string attributeName, string? bindingName)
    {
        if (bindingName is null)
            return $"[{attributeName}] ";

        return $"[{attributeName}(Name = {StringLiteral(bindingName)})] ";
    }

    private static StringBuilder GetUseEndpointHandlersStringBuilder(ImmutableArray<RequestHandler> requestHandlers)
    {
        const int baseSize = 4096;
        const int perHandler = 512;

        var handlerCount = Math.Max(requestHandlers.Length, 0);
        var estimate = baseSize + (long)perHandler * handlerCount;
        estimate = (long)(estimate * 1.10);

        if (estimate > int.MaxValue)
            estimate = int.MaxValue;

        return StringBuilderPool.Get((int)Math.Max(baseSize, estimate));
    }

    [SuppressMessage("Globalization", "CA1308: Normalize strings to uppercase", Justification = "C# boolean literals must be lowercase.")]
    private static string ConstLiteral(TypedConstant tc)
    {
        if (tc.IsNull)
            return "null";
        var v = tc.Value;
        var t = tc.Type;
        if (t is null)
            return "null";

        if (t.TypeKind != TypeKind.Enum)
            return t.SpecialType switch
            {
                SpecialType.System_String => StringLiteral((string?)v),
                SpecialType.System_Char => $"'{EscapeChar((char)v!)}'",
                SpecialType.System_Boolean => ((bool)v!).ToString()
                    .ToLowerInvariant(),
                SpecialType.System_Double => ((double)v!).ToString("R", CultureInfo.InvariantCulture),
                SpecialType.System_Single => ((float)v!).ToString("R", CultureInfo.InvariantCulture) + "f",
                SpecialType.System_Decimal => ((decimal)v!).ToString(CultureInfo.InvariantCulture) + "m",
                SpecialType.System_SByte => ((sbyte)v!).ToString(CultureInfo.InvariantCulture),
                SpecialType.System_Byte => ((byte)v!).ToString(CultureInfo.InvariantCulture),
                SpecialType.System_Int16 => ((short)v!).ToString(CultureInfo.InvariantCulture),
                SpecialType.System_UInt16 => ((ushort)v!).ToString(CultureInfo.InvariantCulture),
                SpecialType.System_Int32 => ((int)v!).ToString(CultureInfo.InvariantCulture),
                SpecialType.System_UInt32 => ((uint)v!).ToString(CultureInfo.InvariantCulture) + "u",
                SpecialType.System_Int64 => ((long)v!).ToString(CultureInfo.InvariantCulture) + "L",
                SpecialType.System_UInt64 => ((ulong)v!).ToString(CultureInfo.InvariantCulture) + "UL",
                _ => StringLiteral(v?.ToString()),
            };

        var field = t.GetMembers()
            .OfType<IFieldSymbol>()
            .FirstOrDefault(f => f.HasConstantValue && Equals(f.ConstantValue, v));

        if (field is not null)
            return $"{t.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)}.{field.Name}";

        var underlying = ((INamedTypeSymbol)t).EnumUnderlyingType!;
        var num = IntegralLiteral(v, underlying.SpecialType);
        return $"({t.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)}){num}";
    }

    private static string IntegralLiteral(object? value, SpecialType underlying)
    {
        return underlying switch
        {
            SpecialType.System_SByte => ((sbyte)value!).ToString(CultureInfo.InvariantCulture),
            SpecialType.System_Byte => ((byte)value!).ToString(CultureInfo.InvariantCulture),
            SpecialType.System_Int16 => ((short)value!).ToString(CultureInfo.InvariantCulture),
            SpecialType.System_UInt16 => ((ushort)value!).ToString(CultureInfo.InvariantCulture),
            SpecialType.System_Int32 => ((int)value!).ToString(CultureInfo.InvariantCulture),
            SpecialType.System_UInt32 => ((uint)value!).ToString(CultureInfo.InvariantCulture) + "u",
            SpecialType.System_Int64 => ((long)value!).ToString(CultureInfo.InvariantCulture) + "L",
            SpecialType.System_UInt64 => ((ulong)value!).ToString(CultureInfo.InvariantCulture) + "UL",
            _ => "0",
        };
    }

    private static string StringLiteral(string? value)
    {
        if (value is null)
            return "null";

        var firstEscapeIndex = -1;
        for (var i = 0; i < value.Length; i++)
        {
            var c = value[i];
            if (c == '\"' || c == '\\' || c == '\n' || c == '\r' || c == '\t' || c == '\0' || char.IsControl(c))
            {
                firstEscapeIndex = i;
                break;
            }
        }

        if (firstEscapeIndex < 0)
            return string.Concat("\"", value, "\"");

        var sb = StringBuilderPool.Get(value.Length + 2);
        sb.Append('"');
        if (firstEscapeIndex > 0)
            sb.Append(value, 0, firstEscapeIndex);

        for (var i = firstEscapeIndex; i < value.Length; i++)
        {
            var c = value[i];
            switch (c)
            {
                case '\"':
                    sb.Append("\\\"");
                    break;
                case '\\':
                    sb.Append("\\\\");
                    break;
                case '\n':
                    sb.Append("\\n");
                    break;
                case '\r':
                    sb.Append("\\r");
                    break;
                case '\t':
                    sb.Append("\\t");
                    break;
                case '\0':
                    sb.Append("\\0");
                    break;
                default:
                    if (char.IsControl(c))
                        sb.Append("\\u")
                            .Append(((int)c).ToString("x4", CultureInfo.InvariantCulture));
                    else
                        sb.Append(c);

                    break;
            }
        }

        sb.Append('"');
        return StringBuilderPool.ToStringAndReturn(sb);
    }

    private static void AppendAdditionalContentTypes(StringBuilder source, EquatableImmutableArray<string>? additionalContentTypes)
    {
        if (additionalContentTypes is not { Count: > 0 })
            return;

        foreach (var additional in additionalContentTypes.Value)
        {
            source.Append(", ");
            source.Append(StringLiteral(additional));
        }
    }

    private static void AppendCommaSeparatedLiterals(StringBuilder source, EquatableImmutableArray<string> values)
    {
        if (values.Count == 0)
            return;

        source.Append(StringLiteral(values[0]));
        for (var i = 1; i < values.Count; i++)
        {
            source.Append(", ");
            source.Append(StringLiteral(values[i]));
        }
    }

    private static void AppendOptionalContentTypes(StringBuilder source, string? contentType, EquatableImmutableArray<string>? additionalContentTypes)
    {
        if (string.IsNullOrEmpty(contentType) && additionalContentTypes is not { Count: > 0 })
            return;

        source.Append(", ");
        source.Append(contentType is { Length: > 0 } ? StringLiteral(contentType) : "null");
        AppendAdditionalContentTypes(source, additionalContentTypes);
    }

    private static string EscapeChar(char c)
    {
        return c switch
        {
            '\'' => "\\'",
            '\\' => "\\\\",
            '\n' => "\\n",
            '\r' => "\\r",
            '\t' => "\\t",
            '\0' => "\\0",
            _ when char.IsControl(c) => "\\u" + ((int)c).ToString("x4", CultureInfo.InvariantCulture),
            _ => c.ToString(),
        };
    }
}
