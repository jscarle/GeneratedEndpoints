using Microsoft.CodeAnalysis;

namespace GeneratedEndpoints.Common;

internal static class RequestHandlerMethodHelper
{
    public static RequestHandlerMethod Create(IMethodSymbol methodSymbol, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var name = methodSymbol.Name;
        var isStatic = methodSymbol.IsStatic;
        var parameters = methodSymbol.GetParameters(cancellationToken);
        var configuration = EndpointConfigurationFactory.Create(methodSymbol);
        var requestHandlerMethod = new RequestHandlerMethod
        {
            Name = name,
            IsStatic = isStatic,
            Parameters = parameters,
            Configuration = configuration,
        };

        return requestHandlerMethod;
    }
}
