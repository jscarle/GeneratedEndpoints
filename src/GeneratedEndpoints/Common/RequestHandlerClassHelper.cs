using Microsoft.CodeAnalysis;
using static GeneratedEndpoints.Common.Constants;

namespace GeneratedEndpoints.Common;

internal static class RequestHandlerClassHelper
{
    public static RequestHandlerClass? Create(IMethodSymbol methodSymbol, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var classSymbol = methodSymbol.ContainingType;
        if (classSymbol.TypeKind != TypeKind.Class)
            return null;

        if (!classSymbol.IsTypeAccessibleFromGeneratedCode() || classSymbol.HasGenericContainingType())
            return null;

        var name = classSymbol.ToDisplayString(SymbolExtensions.FullyQualifiedTypeDisplayFormat);
        var isStatic = classSymbol.IsStatic;
        var isAbstract = classSymbol.IsAbstract;
        var configureMethodDetails = GetConfigureMethodDetails(classSymbol, cancellationToken);
        var classConfiguration = EndpointConfigurationFactory.Create(classSymbol);
        var interfaceName = GetImplementedInterface(classSymbol, methodSymbol)?.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);

        var requestHandlerClass = new RequestHandlerClass
        {
            Name = name,
            IsStatic = isStatic,
            IsAbstract = isAbstract,
            HasConfigureMethod = configureMethodDetails.HasConfigureMethod,
            ConfigureMethodAcceptsServiceProvider = configureMethodDetails.ConfigureMethodAcceptsServiceProvider,
            Configuration = classConfiguration,
            InterfaceName = interfaceName,
        };

        return requestHandlerClass;
    }

    private static ConfigureMethodDetails GetConfigureMethodDetails(INamedTypeSymbol classSymbol, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var hasConfigureMethod = false;
        var acceptsServiceProvider = false;
        foreach (var member in classSymbol.GetMembers(ConfigureMethodName))
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (member is not IMethodSymbol methodSymbol)
                continue;

            if (IsConfigureMethod(methodSymbol, out var methodAcceptsServiceProvider))
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

    private static bool IsConfigureMethod(IMethodSymbol methodSymbol, out bool acceptsServiceProvider)
    {
        acceptsServiceProvider = false;

        if (!methodSymbol.IsAccessibleFromGeneratedCode())
            return false;

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
            if (!serviceProviderParameter.Type.IsIServiceProvider())
                return false;

            acceptsServiceProvider = true;
        }

        if (!methodSymbol.ReturnsVoid)
            return false;

        if (!builderTypeParameter.ConstraintTypes.Any(x => x.IsIEndpointConventionBuilder()))
            return false;

        return true;
    }

    private static INamedTypeSymbol? GetImplementedInterface(
        INamedTypeSymbol classSymbol,
        IMethodSymbol methodSymbol)
    {
        foreach (var candidate in classSymbol.AllInterfaces)
        {
            foreach (var member in candidate.GetMembers().OfType<IMethodSymbol>())
            {
                var implementation = classSymbol.FindImplementationForInterfaceMember(member);

                if (SymbolEqualityComparer.Default.Equals(implementation, methodSymbol))
                    return candidate;
            }
        }

        return null;
    }
}
