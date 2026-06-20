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

        var requestHandlers = GetRequestHandlers(context);

        context.RegisterSourceOutput(requestHandlers, GenerateSource);
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

    private static IncrementalValueProvider<EquatableImmutableArray<RequestHandler>> GetRequestHandlers(IncrementalGeneratorInitializationContext context)
    {
        var list = new List<IncrementalValueProvider<ImmutableArray<RequestHandler>>>(HttpAttributeDefinitions.Length);

        for (var index = 0; index < HttpAttributeDefinitions.Length; index++)
        {
            var definition = HttpAttributeDefinitions[index];
            var handlers = context.SyntaxProvider
                .ForAttributeWithMetadataName(definition.FullyQualifiedName, RequestHandlerFilter, RequestHandlerTransform)
                .WhereNotNull()
                .Collect();

            list.Add(handlers);
        }

        if (list.Count == 0)
            throw new InvalidOperationException("No HTTP attribute definitions were provided.");

        var combined = list[0];
        for (var i = 1; i < list.Count; i++)
        {
            combined = combined.Combine(list[i])
                .Select(static (x, ct) =>
                    {
                        ct.ThrowIfCancellationRequested();
                        return x.Left.AddRange(x.Right);
                    }
                );
        }

        return combined.Select((x, ct) =>
            {
                ct.ThrowIfCancellationRequested();
                return x.ToEquatableImmutableArray();
            }
        );
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

        if (requestHandlerClass.Value.IsAbstract && !requestHandlerMethod.IsStatic)
            return null;

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

        ResolveEndpointNameCollisions(requestHandlers);

        var grouped = requestHandlers.GroupBy(x => x.Class)
            .OrderBy(x => x.Key)
            .ToImmutableSortedDictionary(x => x.Key, x => x.OrderBy(y => y.Method)
                .ToImmutableArray()
            );

        AddEndpointHandlersGenerator.GenerateSource(context, grouped);
        UseEndpointHandlersGenerator.GenerateSource(context, grouped);
    }

    private static void ResolveEndpointNameCollisions(EquatableImmutableArray<RequestHandler> requestHandlers)
    {
        if (requestHandlers.Count <= 1)
            return;

        var raw = requestHandlers.AsArray();
        if (raw is null)
            return;

        var span = raw.AsSpan();
        QualifyDuplicateNames(span, includeSignature: false);
        QualifyDuplicateNames(span, includeSignature: true);
    }

    private static void QualifyDuplicateNames(Span<RequestHandler> handlers, bool includeSignature)
    {
        var seen = new Dictionary<string, int>(handlers.Length, StringComparer.Ordinal);

        for (var index = 0; index < handlers.Length; index++)
        {
            ref var handler = ref handlers[index];
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
                ref var firstHandler = ref handlers[firstIndex];
                firstHandler.SetFullyQualifiedName(includeSignature);
                seen[nonEmptyName] = ~firstIndex;
            }

            handler.SetFullyQualifiedName(includeSignature);
        }
    }
}
