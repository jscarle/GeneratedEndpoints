using Microsoft.CodeAnalysis;

namespace GeneratedEndpoints.Common;

/// <summary>Provides extension methods for working with named type symbols.</summary>
internal static class NamedTypeSymbolExtensions
{
    public static bool HasTypeArgument(this INamedTypeSymbol namedTypeSymbol, out ITypeSymbol typeParameter)
    {
        if (namedTypeSymbol.TypeArguments.Length == 1)
        {
            typeParameter = namedTypeSymbol.TypeArguments[0];
            return true;
        }

        typeParameter = null!;
        return false;
    }
}
