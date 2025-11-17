using System.Buffers;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;
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

        if (context.TargetSymbol is not IMethodSymbol requestHandlerMethodSymbol)
            return null;
        var attribute = context.Attributes[0];

        var requestHandlerClass = GetRequestHandlerClass(requestHandlerMethodSymbol, cancellationToken);
        if (requestHandlerClass is null)
            return null;

        var requestHandlerMethod = GetRequestHandlerMethod(requestHandlerMethodSymbol, cancellationToken);

        var (httpMethod, pattern, name) = GetRequestHandlerAttribute(requestHandlerMethodSymbol, attribute, cancellationToken);

        var methodConfiguration = EndpointConfigurationFactory.Create(requestHandlerMethodSymbol);

        var requestHandler = new RequestHandler(requestHandlerClass.Value, requestHandlerMethod, httpMethod, pattern, name, methodConfiguration);

        return requestHandler;
    }

    private static (string HttpMethod, string Pattern, string? Name) GetRequestHandlerAttribute(IMethodSymbol methodSymbol, AttributeData attribute, CancellationToken cancellationToken)
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

    private static RequestHandlerMethod GetRequestHandlerMethod(IMethodSymbol methodSymbol, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var name = methodSymbol.Name;
        var isStatic = methodSymbol.IsStatic;
        var parameters = methodSymbol.GetParameters(cancellationToken);

        var requestHandlerMethod = new RequestHandlerMethod(name, isStatic, parameters);

        return requestHandlerMethod;
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

    private static void GenerateSource(SourceProductionContext context, EquatableImmutableArray<RequestHandler> requestHandlers)
    {
        context.CancellationToken.ThrowIfCancellationRequested();

        var normalized = NormalizeRequestHandlers(requestHandlers);

        AddEndpointHandlersGenerator.GenerateSource(context, normalized);
        UseEndpointHandlersGenerator.GenerateSource(context, normalized);
    }

    private static ImmutableArray<RequestHandler> NormalizeRequestHandlers(EquatableImmutableArray<RequestHandler> requestHandlers)
    {
        var sorted = SortRequestHandlers(requestHandlers);
        sorted = EnsureUniqueEndpointNames(sorted);

        return sorted;
    }

    private static ImmutableArray<RequestHandler> SortRequestHandlers(EquatableImmutableArray<RequestHandler> requestHandlers)
    {
        var count = requestHandlers.Count;
        if (count == 0)
            return ImmutableArray<RequestHandler>.Empty;
        if (count == 1)
            return [requestHandlers[0]];

        var builder = ImmutableArray.CreateBuilder<RequestHandler>(count);
        builder.AddRange(requestHandlers);
        builder.Sort(RequestHandlerComparer.Instance);
        return builder.MoveToImmutable();
    }

    private static ImmutableArray<RequestHandler> EnsureUniqueEndpointNames(ImmutableArray<RequestHandler> requestHandlers)
    {
        var collidingHandlers = GetRequestHandlersWithNameCollisions(requestHandlers);
        if (collidingHandlers.IsEmpty)
            return requestHandlers;

        var builder = requestHandlers.ToBuilder();
        for (var i = 0; i < collidingHandlers.Length; i++)
        {
            var index = collidingHandlers[i];
            var handler = builder[index];
            var newHandler = handler with
            {
                Name = GetFullyQualifiedMethodDisplayName(handler),
            };
            builder[index] = newHandler;
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

        int[]? collidingArray = null;
        var collidingCount = 0;

        try
        {
            for (var index = 0; index < handlerCount; index++)
            {
                var handler = requestHandlers[index];
                var name = handler.Name;
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

            if (collidingCount == 0)
                return ImmutableArray<int>.Empty;

            Array.Sort(collidingArray!, 0, collidingCount);
            var sorted = new int[collidingCount];
            Array.Copy(collidingArray!, 0, sorted, 0, collidingCount);
            return [..sorted];
        }
        finally
        {
            ArrayPool<bool>.Shared.Return(collisionFlags, clearArray: false);
            if (collidingArray is not null)
                ArrayPool<int>.Shared.Return(collidingArray, clearArray: false);
        }

        void MarkCollision(int handlerIndex)
        {
            if (collisionFlags[handlerIndex])
                return;

            collisionFlags[handlerIndex] = true;
            collidingArray ??= ArrayPool<int>.Shared.Rent(handlerCount);
            collidingArray[collidingCount++] = handlerIndex;
        }
    }

    private static string GetFullyQualifiedMethodDisplayName(RequestHandler requestHandler)
    {
        var className = requestHandler.Class.Name;
        var methodName = requestHandler.Method.Name;

        var startIndex = className.StartsWith(GlobalPrefix, StringComparison.Ordinal)
            ? GlobalPrefix.Length
            : 0;
        var length = className.Length - startIndex;
        var containsNestedTypeSeparator = className.IndexOf('+', startIndex, length) >= 0;

        if (length == 0)
            return string.Concat(".", methodName);

        var totalLength = length + 1 + methodName.Length;
        var buffer = ArrayPool<char>.Shared.Rent(totalLength);
        try
        {
            var destinationIndex = 0;
            for (var i = 0; i < length; i++)
            {
                var character = className[startIndex + i];
                buffer[destinationIndex++] = containsNestedTypeSeparator && character == '+' ? '.' : character;
            }

            buffer[destinationIndex++] = '.';
            methodName.CopyTo(0, buffer, destinationIndex, methodName.Length);

            return new string(buffer, 0, totalLength);
        }
        finally
        {
            ArrayPool<char>.Shared.Return(buffer);
        }
    }
}
