using Microsoft.CodeAnalysis;

namespace GeneratedEndpoints.Common;

internal static class TypeSymbolExtensions
{
    public static bool IsIEndpointConventionBuilder(this ITypeSymbol symbol)
    {
        return symbol is
        {
            MetadataName: "IEndpointConventionBuilder",
            ContainingNamespace:
            {
                Name: "Builder",
                ContainingNamespace: { Name: "AspNetCore", ContainingNamespace: { Name: "Microsoft", ContainingNamespace.IsGlobalNamespace: true } },
            },
        };
    }

    public static bool IsIServiceProvider(this ITypeSymbol symbol)
    {
        return symbol is
        {
            MetadataName: "IServiceProvider",
            ContainingNamespace:
            {
                Name: "System", ContainingNamespace.IsGlobalNamespace: true,
            },
        };
    }

    public static bool IsAwaitable(this ITypeSymbol symbol)
    {
        return symbol switch
        {
            {
                    MetadataName: "ValueTask`1",
                    ContainingNamespace:
                    {
                        Name: "Tasks",
                        ContainingNamespace: { Name: "Threading", ContainingNamespace: { Name: "System", ContainingNamespace.IsGlobalNamespace: true } },
                    },
                }
                or
                {
                    MetadataName: "Task`1",
                    ContainingNamespace:
                    {
                        Name: "Tasks",
                        ContainingNamespace: { Name: "Threading", ContainingNamespace: { Name: "System", ContainingNamespace.IsGlobalNamespace: true } },
                    },
                }
                or
                {
                    MetadataName: "ValueTask",
                    ContainingNamespace:
                    {
                        Name: "Tasks",
                        ContainingNamespace: { Name: "Threading", ContainingNamespace: { Name: "System", ContainingNamespace.IsGlobalNamespace: true } },
                    },
                }
                or
                {
                    MetadataName: "Task",
                    ContainingNamespace:
                    {
                        Name: "Tasks",
                        ContainingNamespace: { Name: "Threading", ContainingNamespace: { Name: "System", ContainingNamespace.IsGlobalNamespace: true } },
                    },
                } => true,
            _ => false,
        };
    }
}
