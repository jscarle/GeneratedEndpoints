using System.Collections.Immutable;
using System.Threading;
using Microsoft.CodeAnalysis;
using static GeneratedEndpoints.Common.AttributeSymbolMatcher;
using static GeneratedEndpoints.Common.Constants;
using static GeneratedEndpoints.MinimalApiGenerator;

namespace GeneratedEndpoints.Common;

internal static class RequestHandlerParameterHelper
{
    public static EquatableImmutableArray<Parameter> Build(IMethodSymbol methodSymbol, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var methodParameters = ImmutableArray.CreateBuilder<Parameter>(methodSymbol.Parameters.Length);
        foreach (var parameter in methodSymbol.Parameters)
        {
            cancellationToken.ThrowIfCancellationRequested();

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
                        bindingName = GetBindingAttributeName(attribute) ?? bindingName;
                        break;
                    case BindingSource.FromKeyedServices:
                        typedKey = attribute.ConstructorArguments.Length > 0 ? attribute.ConstructorArguments[0] : null;
                        break;
                }
            }

            var parameterName = parameter.Name;
            var parameterType = parameter.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            var key = typedKey.HasValue ? ConstLiteral(typedKey.Value) : null;
            var bindingPrefix = GetBindingSourceAttribute(source, key, bindingName);
            methodParameters.Add(new Parameter(parameterName, parameterType, bindingPrefix));
        }

        return methodParameters.ToEquatableImmutable();
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
            "FromKeyedServicesAttribute" when IsInNamespace(namespaceSymbol, ExtensionsDependencyInjectionNamespaceParts)
                => BindingSource.FromKeyedServices,
            "AsParametersAttribute" when IsInNamespace(namespaceSymbol, AspNetCoreHttpNamespaceParts) => BindingSource.AsParameters,
            _ => BindingSource.None,
        };
    }

    private static string GetBindingSourceAttribute(BindingSource source, string? key, string? bindingName)
    {
        return source switch
        {
            BindingSource.None => string.Empty,
            BindingSource.FromRoute => FormatBindingAttribute("FromRoute", bindingName),
            BindingSource.FromQuery => FormatBindingAttribute("FromQuery", bindingName),
            BindingSource.FromHeader => FormatBindingAttribute("FromHeader", bindingName),
            BindingSource.FromBody => FormatBindingAttribute("FromBody", bindingName),
            BindingSource.FromForm => FormatBindingAttribute("FromForm", bindingName),
            BindingSource.FromServices => "[FromServices] ",
            BindingSource.FromKeyedServices => $"[FromKeyedServices({key})] ",
            BindingSource.AsParameters => "[AsParameters] ",
            _ => string.Empty,
        };
    }

    private static string FormatBindingAttribute(string attributeName, string? bindingName)
    {
        if (bindingName is null)
            return $"[{attributeName}] ";

        return $"[{attributeName}(Name = {StringLiteral(bindingName)})] ";
    }
}
