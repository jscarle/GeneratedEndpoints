using System.Threading;
using GeneratedEndpoints;
using Microsoft.CodeAnalysis;

namespace GeneratedEndpoints.Common;

internal sealed class RequestHandlerClassCacheEntry
{
    private readonly object _lock = new();
    private RequestHandlerClass _value;
    private bool _initialized;

    public RequestHandlerClass GetOrCreate(
        INamedTypeSymbol classSymbol,
        CompilationTypeCache compilationCache,
        CancellationToken cancellationToken
    )
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
            var configureMethodDetails = MinimalApiGenerator.GetConfigureMethodDetails(
                classSymbol,
                compilationCache.EndpointConventionBuilderSymbol,
                compilationCache.ServiceProviderSymbol,
                cancellationToken
            );

            var mapGroupPattern = MinimalApiGenerator.GetMapGroupPattern(classSymbol);
            var mapGroupIdentifier = mapGroupPattern is null ? null : MinimalApiGenerator.GetMapGroupIdentifier(name);
            var classConfiguration = EndpointConfigurationFactory.Create(classSymbol.GetAttributes(), null, null, null, false);

            _value = new RequestHandlerClass(
                name,
                isStatic,
                configureMethodDetails.HasConfigureMethod,
                configureMethodDetails.ConfigureMethodAcceptsServiceProvider,
                mapGroupPattern,
                mapGroupIdentifier,
                classConfiguration
            );
            _initialized = true;
            return _value;
        }
    }
}
