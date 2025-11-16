using Microsoft.CodeAnalysis;

namespace GeneratedEndpoints.Common;

internal static class TypeSymbolExtensions
{
    public static bool IsAwaitable(this ITypeSymbol symbol)
    {
        return symbol switch
        {
            INamedTypeSymbol
                {
                    MetadataName: "ValueTask`1",
                    ContainingNamespace:
                    {
                        Name: "Tasks",
                        ContainingNamespace: { Name: "Threading", ContainingNamespace: { Name: "System", ContainingNamespace.IsGlobalNamespace: true } },
                    },
                }
                or INamedTypeSymbol
                {
                    MetadataName: "Task`1",
                    ContainingNamespace:
                    {
                        Name: "Tasks",
                        ContainingNamespace: { Name: "Threading", ContainingNamespace: { Name: "System", ContainingNamespace.IsGlobalNamespace: true } },
                    },
                }
                or INamedTypeSymbol
                {
                    MetadataName: "ValueTask",
                    ContainingNamespace:
                    {
                        Name: "Tasks",
                        ContainingNamespace: { Name: "Threading", ContainingNamespace: { Name: "System", ContainingNamespace.IsGlobalNamespace: true } },
                    },
                }
                or INamedTypeSymbol
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
