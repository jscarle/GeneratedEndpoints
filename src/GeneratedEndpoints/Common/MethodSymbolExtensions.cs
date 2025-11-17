using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using static GeneratedEndpoints.Common.AttributeSymbolMatcher;
using static GeneratedEndpoints.Common.Constants;

namespace GeneratedEndpoints.Common;

// ReSharper disable ForCanBeConvertedToForeach
// ReSharper disable LoopCanBeConvertedToQuery
// Do not refactor, use for loop to avoid allocations.

internal static class MethodSymbolExtensions
{
    public static EquatableImmutableArray<Parameter> GetParameters(this IMethodSymbol methodSymbol, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var methodParameters = ImmutableArray.CreateBuilder<Parameter>(methodSymbol.Parameters.Length);

        for (var index = 0; index < methodSymbol.Parameters.Length; index++)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var parameterSymbol = methodSymbol.Parameters[index];
            var parameterName = parameterSymbol.Name;
            var parameterType = parameterSymbol.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            var bindingPrefix = GetBindingPrefix(parameterSymbol);
            var parameter = new Parameter(parameterName, parameterType, bindingPrefix);

            methodParameters.Add(parameter);
        }

        return methodParameters.ToEquatableImmutable();
    }

    private static string GetBindingPrefix(IParameterSymbol parameter)
    {
        var source = BindingSource.None;
        TypedConstant? typedKey = null;
        string? bindingName = null;

        foreach (var attribute in parameter.GetAttributes())
        {
            var attributeClass = attribute.AttributeClass;
            if (attributeClass is null)
                continue;

            var attributeSource = GetBindingSourceFromAttributeClass(attributeClass);
            if (attributeSource == BindingSource.None)
                continue;

            source = attributeSource;
            switch (attributeSource)
            {
                case BindingSource.FromRoute:
                case BindingSource.FromQuery:
                case BindingSource.FromHeader:
                case BindingSource.FromForm:
                    bindingName = attribute.GetNamedStringValue(NameAttributeNamedParameter);
                    break;
                case BindingSource.FromKeyedServices:
                    typedKey = attribute.ConstructorArguments.Length > 0 ? attribute.ConstructorArguments[0] : null;
                    break;
            }
        }

        var bindingPrefix = GetBindingSourceAttribute(source, typedKey, bindingName);

        return bindingPrefix;
    }

    private static BindingSource GetBindingSourceFromAttributeClass(INamedTypeSymbol attributeClass)
    {
        var definition = attributeClass.OriginalDefinition;
        var namespaceSymbol = definition.ContainingNamespace;

        return definition.Name switch
        {
            "FromRouteAttribute" when IsInNamespace(namespaceSymbol, AspNetCoreMvcNamespaceParts) => BindingSource.FromRoute,
            "FromQueryAttribute" when IsInNamespace(namespaceSymbol, AspNetCoreMvcNamespaceParts) => BindingSource.FromQuery,
            "FromHeaderAttribute" when IsInNamespace(namespaceSymbol, AspNetCoreMvcNamespaceParts) => BindingSource.FromHeader,
            "FromBodyAttribute" when IsInNamespace(namespaceSymbol, AspNetCoreMvcNamespaceParts) => BindingSource.FromBody,
            "FromFormAttribute" when IsInNamespace(namespaceSymbol, AspNetCoreMvcNamespaceParts) => BindingSource.FromForm,
            "FromServicesAttribute" when IsInNamespace(namespaceSymbol, AspNetCoreMvcNamespaceParts) => BindingSource.FromServices,
            "FromKeyedServicesAttribute" when IsInNamespace(namespaceSymbol, ExtensionsDependencyInjectionNamespaceParts) => BindingSource.FromKeyedServices,
            "AsParametersAttribute" when IsInNamespace(namespaceSymbol, AspNetCoreHttpNamespaceParts) => BindingSource.AsParameters,
            _ => BindingSource.None,
        };
    }

    private static string GetBindingSourceAttribute(BindingSource source, TypedConstant? typedKey, string? bindingName)
    {
        switch (source)
        {
            case BindingSource.None:
                return "";
            case BindingSource.FromRoute:
                return FormatBindingAttribute("FromRoute", bindingName);
            case BindingSource.FromQuery:
                return FormatBindingAttribute("FromQuery", bindingName);
            case BindingSource.FromHeader:
                return FormatBindingAttribute("FromHeader", bindingName);
            case BindingSource.FromBody:
                return FormatBindingAttribute("FromBody", bindingName);
            case BindingSource.FromForm:
                return FormatBindingAttribute("FromForm", bindingName);
            case BindingSource.FromServices:
                return "[FromServices] ";
            case BindingSource.FromKeyedServices:
                var key = typedKey?.ToConstLiteral();
                return $"[FromKeyedServices({key})] ";
            case BindingSource.AsParameters:
                return "[AsParameters] ";
            default:
                return "";
        }
    }

    private static string FormatBindingAttribute(string attributeName, string? bindingName)
    {
        if (bindingName is null)
            return $"[{attributeName}] ";

        return $"[{attributeName}(Name = {bindingName.ToStringLiteral()})] ";
    }
}
