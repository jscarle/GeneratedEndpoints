using System.Collections.Immutable;
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

        var requestHandlers = CombineRequestHandlers(requestHandlerProviders);

        context.RegisterSourceOutput(requestHandlers, GenerateSource);
    }

    private static IncrementalValueProvider<EquatableImmutableArray<RequestHandler>> CombineRequestHandlers(
        ImmutableArray<IncrementalValueProvider<ImmutableArray<RequestHandler>>>.Builder builder
    )
    {
        var handlerProvidersArray = builder.MoveToImmutable();

        if (handlerProvidersArray.IsDefaultOrEmpty)
            throw new InvalidOperationException("No HTTP attribute definitions were provided.");

        var combined = handlerProvidersArray[0];
        for (var i = 1; i < handlerProvidersArray.Length; i++)
        {
            combined = combined.Combine(handlerProvidersArray[i])
                .Select(static (x, _) => x.Left.AddRange(x.Right));
        }

        return combined
            .Select((x, _) => x.ToEquatableImmutableArray());
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

        var requestHandlerClass = RequestHandlerClassHelper.Create(methodSymbol, cancellationToken);
        if (requestHandlerClass is null)
            return null;

        var requestHandlerMethod = RequestHandlerMethodHelper.Create(methodSymbol, cancellationToken);

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
        name ??= methodSymbol.Name.RemoveAsyncSuffix();

        return (httpMethod, pattern, name);
    }

    private static void GenerateSource(SourceProductionContext context, EquatableImmutableArray<RequestHandler> requestHandlers)
    {
        context.CancellationToken.ThrowIfCancellationRequested();

        var normalized = NormalizeRequestHandlers(requestHandlers);

        var grouped = normalized.GroupBy(x => x.Class).OrderBy(x => x.Key)
            .ToImmutableSortedDictionary(x => x.Key, x => x.OrderBy(y => y.Method).ToImmutableArray());

        AddEndpointHandlersGenerator.GenerateSource(context, grouped);
        UseEndpointHandlersGenerator.GenerateSource(context, grouped);
    }

    private static EquatableImmutableArray<RequestHandler> NormalizeRequestHandlers(EquatableImmutableArray<RequestHandler> requestHandlers)
    {
        if (requestHandlers.Count <= 1)
            return requestHandlers;

        ResolveEndpointNameCollisions(requestHandlers);
#pragma warning disable S125
        //requestHandlers.SortInPlace(RequestHandlerComparer.Instance);
#pragma warning restore S125

        return requestHandlers;
    }

    private static void ResolveEndpointNameCollisions(EquatableImmutableArray<RequestHandler> requestHandlers)
    {
        var raw = requestHandlers.AsArray();
        if (raw is null)
            return;

        var span = raw.AsSpan();
        var seen = new Dictionary<string, int>(span.Length, StringComparer.Ordinal);

        for (var index = 0; index < span.Length; index++)
        {
            ref var handler = ref span[index];
            var name = handler.Name;
            if (string.IsNullOrEmpty(name))
                continue;
            var nonEmptyName = name!;

            if (!seen.TryGetValue(nonEmptyName, out var state))
            {
                seen.Add(nonEmptyName, index);
                continue;
            }

            var firstIndex = state >= 0 ? state : ~state;
            if (state >= 0)
            {
                ref var firstHandler = ref span[firstIndex];
                firstHandler.SetFullyQualifiedName();
                seen[nonEmptyName] = ~firstIndex;
            }

            handler.SetFullyQualifiedName();
        }
    }
}
