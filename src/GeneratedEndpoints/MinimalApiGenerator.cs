using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using GeneratedEndpoints.Common;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static GeneratedEndpoints.Common.Constants;

namespace GeneratedEndpoints;

// ReSharper disable ForCanBeConvertedToForeach
// ReSharper disable LoopCanBeConvertedToQuery
// Do not refactor, use for loop to avoid allocations.

[Generator]
public sealed class MinimalApiGenerator : IIncrementalGenerator
{
    private static readonly ConditionalWeakTable<INamedTypeSymbol, RequestHandlerClassCacheEntry> RequestHandlerClassCache = new();

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(RegisterAttributes);

        var requestHandlerProviders = ImmutableArray.CreateBuilder<IncrementalValueProvider<ImmutableArray<RequestHandler>>>(HttpAttributeDefinitions.Length);

        for (var index = 0; index < HttpAttributeDefinitions.Length; index++)
        {
            var definition = HttpAttributeDefinitions[index];
            var handlers = context.SyntaxProvider
                .ForAttributeWithMetadataName(definition.FullyQualifiedName, RequestHandlerFilter, RequestHandlerTransform)
                .WhereNotNull()
                .Collect();

            requestHandlerProviders.Add(handlers);
        }

        var requestHandlers = CombineRequestHandlers(requestHandlerProviders.MoveToImmutable())
            .Select((x, _) => x.ToEquatableImmutableArray());

        context.RegisterSourceOutput(requestHandlers, GenerateSource);
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

        context.AddSource(AcceptsAttributeHint, AcceptsAttributeSourceText);
        context.AddSource(DisableAntiforgeryAttributeHint, DisableAntiforgeryAttributeSourceText);
        context.AddSource(DisableRequestTimeoutAttributeHint, DisableRequestTimeoutAttributeSourceText);
        context.AddSource(DisableValidationAttributeHint, DisableValidationAttributeSourceText);
        context.AddSource(EndpointFilterAttributeHint, EndpointFilterAttributeSourceText);
        context.AddSource(MapGroupAttributeHint, MapGroupAttributeSourceText);
        context.AddSource(OrderAttributeHint, OrderAttributeSourceText);
        context.AddSource(ProducesProblemAttributeHint, ProducesProblemAttributeSourceText);
        context.AddSource(ProducesResponseAttributeHint, ProducesResponseAttributeSourceText);
        context.AddSource(ProducesValidationProblemAttributeHint, ProducesValidationProblemAttributeSourceText);
        context.AddSource(RequestTimeoutAttributeHint, RequestTimeoutAttributeSourceText);
        context.AddSource(RequireAuthorizationAttributeHint, RequireAuthorizationAttributeSourceText);
        context.AddSource(RequireCorsAttributeHint, RequireCorsAttributeSourceText);
        context.AddSource(RequireHostAttributeHint, RequireHostAttributeSourceText);
        context.AddSource(RequireRateLimitingAttributeHint, RequireRateLimitingAttributeSourceText);
        context.AddSource(ShortCircuitAttributeHint, ShortCircuitAttributeSourceText);
        context.AddSource(SummaryAttributeHint, SummaryAttributeSourceText);
    }

    private static bool RequestHandlerFilter(SyntaxNode syntaxNode, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return syntaxNode is MethodDeclarationSyntax;
    }

    private static RequestHandler? RequestHandlerTransform(GeneratorAttributeSyntaxContext context, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (context.TargetSymbol is not IMethodSymbol methodSymbol)
            return null;
        var attribute = context.Attributes[0];

        var requestHandlerClass = GetRequestHandlerClass(methodSymbol, cancellationToken);
        if (requestHandlerClass is null)
            return null;

        var requestHandlerMethod = GetRequestHandlerMethod(methodSymbol, cancellationToken);

        var (httpMethod, pattern, name) = GetRequestHandlerAttribute(methodSymbol, attribute, cancellationToken);

        var requestHandler = new RequestHandler
        {
            Class = requestHandlerClass.Value,
            Method = requestHandlerMethod,
            HttpMethod = httpMethod,
            Pattern = pattern,
            Name = name,
        };

        return requestHandler;
    }

    private static (string HttpMethod, string Pattern, string? Name) GetRequestHandlerAttribute(
        IMethodSymbol methodSymbol,
        AttributeData attribute,
        CancellationToken cancellationToken
    )
    {
        cancellationToken.ThrowIfCancellationRequested();

        var attributeName = attribute.AttributeClass?.Name ?? "";
        var httpMethod = HttpAttributeDefinitionsByName.TryGetValue(attributeName, out var definition) ? definition.Verb : "";
        var pattern = attribute.GetConstructorStringValue() ?? "";
        var name = attribute.GetNamedStringValue(NameAttributeNamedParameter);
        name ??= RemoveAsyncSuffix(methodSymbol.Name);

        return (httpMethod, pattern, name);
    }

    private static string RemoveAsyncSuffix(string methodName)
    {
        if (methodName.EndsWith(AsyncSuffix, StringComparison.OrdinalIgnoreCase) && methodName.Length > AsyncSuffix.Length)
            return methodName[..^AsyncSuffix.Length];

        return methodName;
    }

    private static RequestHandlerClass? GetRequestHandlerClass(IMethodSymbol methodSymbol, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var classSymbol = methodSymbol.ContainingType;
        if (classSymbol.TypeKind != TypeKind.Class)
            return null;

        var cacheEntry = RequestHandlerClassCache.GetValue(classSymbol, static _ => new RequestHandlerClassCacheEntry());
        var requestHandlerClass = cacheEntry.GetOrCreate(classSymbol, cancellationToken);

        return requestHandlerClass;
    }

    private static RequestHandlerMethod GetRequestHandlerMethod(IMethodSymbol methodSymbol, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var name = methodSymbol.Name;
        var isStatic = methodSymbol.IsStatic;
        var parameters = methodSymbol.GetParameters(cancellationToken);
        var configuration = EndpointConfigurationFactory.Create(methodSymbol);
        var requestHandlerMethod = new RequestHandlerMethod(name, isStatic, parameters, configuration);

        return requestHandlerMethod;
    }

    private static void GenerateSource(SourceProductionContext context, EquatableImmutableArray<RequestHandler> requestHandlers)
    {
        context.CancellationToken.ThrowIfCancellationRequested();

        var normalized = NormalizeRequestHandlers(requestHandlers);

        AddEndpointHandlersGenerator.GenerateSource(context, normalized);
        UseEndpointHandlersGenerator.GenerateSource(context, normalized);
    }

    private static EquatableImmutableArray<RequestHandler> NormalizeRequestHandlers(EquatableImmutableArray<RequestHandler> requestHandlers)
    {
        if (requestHandlers.Count <= 1)
            return requestHandlers;

        var sorted = requestHandlers.SortInPlace(RequestHandlerComparer.Instance);
        var unique = EnsureUniqueEndpointNames(sorted);

        return unique;
    }

    private static EquatableImmutableArray<RequestHandler> EnsureUniqueEndpointNames(EquatableImmutableArray<RequestHandler> requestHandlers)
    {
        ResolveEndpointNameCollisions(requestHandlers);
        return requestHandlers;
    }

    private static void ResolveEndpointNameCollisions(EquatableImmutableArray<RequestHandler> requestHandlers)
    {
        if (requestHandlers.Count == 0)
            return;

        var handlers = requestHandlers.AsImmutableArray();
        var raw = ImmutableCollectionsMarshal.AsArray(handlers);
        if (raw is null)
            return;

        var span = raw.AsSpan();

        for (var outerIndex = 0; outerIndex < span.Length - 1; outerIndex++)
        {
            ref var outer = ref span[outerIndex];
            var outerName = outer.Name;
            if (string.IsNullOrEmpty(outerName))
                continue;

            var collisionHandled = false;
            for (var innerIndex = outerIndex + 1; innerIndex < span.Length; innerIndex++)
            {
                ref var inner = ref span[innerIndex];
                var innerName = inner.Name;
                if (innerName is null)
                    continue;

                if (!string.Equals(outerName, innerName, StringComparison.Ordinal))
                    continue;

                if (!collisionHandled)
                {
                    outer.Name = outer.GetFullyQualifiedMethodDisplayName();
                    collisionHandled = true;
                }

                inner.Name = inner.GetFullyQualifiedMethodDisplayName();
            }
        }
    }
}
