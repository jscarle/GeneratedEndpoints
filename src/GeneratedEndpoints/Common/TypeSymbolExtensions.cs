using Microsoft.CodeAnalysis;

namespace GeneratedEndpoints.Common;

/// <summary>Provides extension methods for working with type symbols.</summary>
internal static class TypeSymbolExtensions
{
    public static bool IsFromRouteAttribute(this ITypeSymbol symbol)
    {
        return symbol is INamedTypeSymbol
        {
            MetadataName: "FromRouteAttribute",
            ContainingNamespace:
            {
                Name: "Mvc",
                ContainingNamespace:
                {
                    Name: "AspNetCore",
                    ContainingNamespace:
                    {
                        Name: "Microsoft",
                        ContainingNamespace.IsGlobalNamespace: true,
                    },
                },
            },
        };
    }

    public static bool IsFromQueryAttribute(this ITypeSymbol symbol)
    {
        return symbol is INamedTypeSymbol
        {
            MetadataName: "FromQueryAttribute",
            ContainingNamespace:
            {
                Name: "Mvc",
                ContainingNamespace:
                {
                    Name: "AspNetCore",
                    ContainingNamespace:
                    {
                        Name: "Microsoft",
                        ContainingNamespace.IsGlobalNamespace: true,
                    },
                },
            },
        };
    }

    public static bool IsFromHeaderAttribute(this ITypeSymbol symbol)
    {
        return symbol is INamedTypeSymbol
        {
            MetadataName: "FromHeaderAttribute",
            ContainingNamespace:
            {
                Name: "Mvc",
                ContainingNamespace:
                {
                    Name: "AspNetCore",
                    ContainingNamespace:
                    {
                        Name: "Microsoft",
                        ContainingNamespace.IsGlobalNamespace: true,
                    },
                },
            },
        };
    }

    public static bool IsFromBodyAttribute(this ITypeSymbol symbol)
    {
        return symbol is INamedTypeSymbol
        {
            MetadataName: "FromBodyAttribute",
            ContainingNamespace:
            {
                Name: "Mvc",
                ContainingNamespace:
                {
                    Name: "AspNetCore",
                    ContainingNamespace:
                    {
                        Name: "Microsoft",
                        ContainingNamespace.IsGlobalNamespace: true,
                    },
                },
            },
        };
    }

    public static bool IsFromFormAttribute(this ITypeSymbol symbol)
    {
        return symbol is INamedTypeSymbol
        {
            MetadataName: "FromFormAttribute",
            ContainingNamespace:
            {
                Name: "Mvc",
                ContainingNamespace:
                {
                    Name: "AspNetCore",
                    ContainingNamespace:
                    {
                        Name: "Microsoft",
                        ContainingNamespace.IsGlobalNamespace: true,
                    },
                },
            },
        };
    }

    public static bool IsFromServicesAttribute(this ITypeSymbol symbol)
    {
        return symbol is INamedTypeSymbol
        {
            MetadataName: "FromServicesAttribute",
            ContainingNamespace:
            {
                Name: "Mvc",
                ContainingNamespace:
                {
                    Name: "AspNetCore",
                    ContainingNamespace:
                    {
                        Name: "Microsoft",
                        ContainingNamespace.IsGlobalNamespace: true,
                    },
                },
            },
        };
    }

    public static bool IsFromKeyedServicesAttribute(this ITypeSymbol symbol)
    {
        return symbol is INamedTypeSymbol
        {
            MetadataName: "FromKeyedServicesAttribute",
            ContainingNamespace:
            {
                Name: "Mvc",
                ContainingNamespace:
                {
                    Name: "AspNetCore",
                    ContainingNamespace:
                    {
                        Name: "Microsoft",
                        ContainingNamespace.IsGlobalNamespace: true,
                    },
                },
            },
        };
    }

    public static bool IsAsParametersAttribute(this ITypeSymbol symbol)
    {
        return symbol is INamedTypeSymbol
        {
            MetadataName: "AsParametersAttribute",
            ContainingNamespace:
            {
                Name: "Http",
                ContainingNamespace:
                {
                    Name: "AspNetCore",
                    ContainingNamespace:
                    {
                        Name: "Microsoft",
                        ContainingNamespace.IsGlobalNamespace: true,
                    },
                },
            },
        };
    }

    public static bool IsCancellationToken(this ITypeSymbol symbol)
    {
        return symbol is INamedTypeSymbol
        {
            MetadataName: "CancellationToken",
            ContainingNamespace:
            {
                Name: "Threading",
                ContainingNamespace:
                {
                    Name: "System",
                    ContainingNamespace.IsGlobalNamespace: true,
                },
            },
        };
    }

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
