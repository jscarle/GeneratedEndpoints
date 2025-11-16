using Microsoft.CodeAnalysis;

namespace GeneratedEndpoints.Common;

internal static class TypeSymbolExtensions
{
    public static bool IsValueTask(this ITypeSymbol symbol, out INamedTypeSymbol valueTaskSymbol)
    {
        if (symbol is INamedTypeSymbol
            {
                MetadataName: "ValueTask`1",
                ContainingNamespace:
                {
                    Name: "Tasks",
                    ContainingNamespace:
                    {
                        Name: "Threading",
                        ContainingNamespace:
                        {
                            Name: "System",
                            ContainingNamespace.IsGlobalNamespace: true,
                        },
                    },
                },
            } namedTypeSymbol)
        {
            valueTaskSymbol = namedTypeSymbol;
            return true;
        }

        valueTaskSymbol = null!;
        return false;
    }

    public static bool IsTask(this ITypeSymbol symbol, out INamedTypeSymbol valueTaskSymbol)
    {
        if (symbol is INamedTypeSymbol
            {
                MetadataName: "Task`1",
                ContainingNamespace:
                {
                    Name: "Tasks",
                    ContainingNamespace:
                    {
                        Name: "Threading",
                        ContainingNamespace:
                        {
                            Name: "System",
                            ContainingNamespace.IsGlobalNamespace: true,
                        },
                    },
                },
            } namedTypeSymbol)
        {
            valueTaskSymbol = namedTypeSymbol;
            return true;
        }

        valueTaskSymbol = null!;
        return false;
    }

    public static bool IsLightResults(this ITypeSymbol symbol, out INamedTypeSymbol lightResultsSymbol)
    {
        if (symbol is INamedTypeSymbol
            {
                Name: "Result",
                ContainingNamespace:
                {
                    Name: "LightResults",
                    ContainingNamespace.IsGlobalNamespace: true,
                },
            } namedTypeSymbol)
        {
            lightResultsSymbol = namedTypeSymbol;
            return true;
        }

        lightResultsSymbol = null!;
        return false;
    }
}
