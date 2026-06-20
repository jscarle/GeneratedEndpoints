using Microsoft.CodeAnalysis;

namespace GeneratedEndpoints.Common;

internal static class AttributeDataExtensions
{
    public static string? GetConstructorStringValue(this AttributeData attribute, int position = 0)
    {
        if (attribute.ConstructorArguments.Length > position)
            return (attribute.ConstructorArguments[position].Value as string).NormalizeOptionalString();

        return null;
    }

    public static EquatableImmutableArray<string>? GetConstructorStringArray(this AttributeData attribute, int position = 0)
    {
        if (attribute.ConstructorArguments.Length <= position)
            return null;

        var arg = attribute.ConstructorArguments[position];
        if (arg.Kind == TypedConstantKind.Array)
        {
            if (arg.Values.Length == 0)
                return null;

            List<string>? normalized = null;
            foreach (var value in arg.Values)
            {
                if (value.Value is not string stringValue)
                    continue;

                var trimmed = stringValue.NormalizeOptionalString();
                if (trimmed is not { Length: > 0 })
                    continue;

                normalized ??= new List<string>(arg.Values.Length);
                normalized.Add(trimmed);
            }

            if (normalized is { Count: > 0 })
                return normalized.ToEquatableImmutableArray();
        }
        else if (arg.Value is string singleHost && !string.IsNullOrWhiteSpace(singleHost))
        {
            return new[] { singleHost.Trim() }.ToEquatableImmutableArray();
        }

        return null;
    }

    public static int? GetConstructorIntValue(this AttributeData attribute, int position = 0)
    {
        if (attribute.ConstructorArguments.Length > position && attribute.ConstructorArguments[position].Value is int value)
            return value;

        return null;
    }

    public static ITypeSymbol? GetConstructorTypeSymbol(this AttributeData attribute, int position = 0)
    {
        if (attribute.ConstructorArguments.Length > position && attribute.ConstructorArguments[position].Value is ITypeSymbol typeSymbol)
            return typeSymbol;

        return null;
    }

    public static ITypeSymbol? GetNamedTypeSymbol(this AttributeData attribute, string namedParameter)
    {
        foreach (var namedArg in attribute.NamedArguments)
        {
            if (namedArg.Key == namedParameter && namedArg.Value.Value is ITypeSymbol typeSymbol)
                return typeSymbol;
        }

        return null;
    }

    public static bool GetNamedBoolValue(this AttributeData attribute, string namedParameter, bool defaultValue = false)
    {
        foreach (var namedArg in attribute.NamedArguments)
        {
            if (namedArg.Key == namedParameter && namedArg.Value.Value is bool boolValue)
                return boolValue;
        }

        return defaultValue;
    }

    public static string? GetNamedStringValue(this AttributeData attribute, string namedParameter)
    {
        foreach (var namedArg in attribute.NamedArguments)
        {
            if (namedArg.Key == namedParameter && namedArg.Value.Value is string stringValue)
                return stringValue.NormalizeOptionalString();
        }

        return null;
    }

    public static string? GetNamedConstantValue(this AttributeData attribute, string namedParameter)
    {
        foreach (var namedArg in attribute.NamedArguments)
            if (namedArg.Key == namedParameter)
                return namedArg.Value.ToConstLiteral();

        return null;
    }
}
