using Microsoft.CodeAnalysis;

namespace GeneratedEndpoints.Common;

internal static class AttributeSymbolMatcher
{
    public static bool IsAttribute(INamedTypeSymbol attributeClass, string attributeName, string[] namespaceParts)
    {
        var definition = attributeClass.OriginalDefinition;
        return definition.Name == attributeName && IsInNamespace(definition.ContainingNamespace, namespaceParts);
    }

    public static bool IsInNamespace(INamespaceSymbol? namespaceSymbol, string[] namespaceParts)
    {
        for (var i = namespaceParts.Length - 1; i >= 0; i--)
        {
            if (namespaceSymbol is null || namespaceSymbol.Name != namespaceParts[i])
                return false;

            namespaceSymbol = namespaceSymbol.ContainingNamespace;
        }

        return namespaceSymbol is null || namespaceSymbol.IsGlobalNamespace;
    }
}
