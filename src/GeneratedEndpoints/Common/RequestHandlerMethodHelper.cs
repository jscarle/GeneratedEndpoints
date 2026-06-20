using Microsoft.CodeAnalysis;

namespace GeneratedEndpoints.Common;

internal static class RequestHandlerMethodHelper
{
    public static RequestHandlerMethod? Create(IMethodSymbol methodSymbol, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (methodSymbol.MethodKind != MethodKind.Ordinary || methodSymbol.TypeParameters.Length > 0 || !methodSymbol.IsAccessibleFromGeneratedCode())
            return null;

        var name = methodSymbol.Name;
        var isStatic = methodSymbol.IsStatic;
        var requiresDelegateWrapper = isStatic && HasOverloads(methodSymbol, cancellationToken);
        var parameters = methodSymbol.GetParameters(cancellationToken);
        var configuration = EndpointConfigurationFactory.Create(methodSymbol);
        var requestHandlerMethod = new RequestHandlerMethod
        {
            Name = name,
            IsStatic = isStatic,
            RequiresDelegateWrapper = requiresDelegateWrapper,
            Parameters = parameters,
            Configuration = configuration,
        };

        return requestHandlerMethod;
    }

    private static bool HasOverloads(IMethodSymbol methodSymbol, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        foreach (var member in methodSymbol.ContainingType.GetMembers(methodSymbol.Name))
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (member is IMethodSymbol otherMethod
                && otherMethod.MethodKind == MethodKind.Ordinary
                && !SymbolEqualityComparer.Default.Equals(methodSymbol, otherMethod))
                return true;
        }

        return false;
    }
}
