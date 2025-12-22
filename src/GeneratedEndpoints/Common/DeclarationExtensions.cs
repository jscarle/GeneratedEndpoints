namespace GeneratedEndpoints.Common;

/// <summary>Provides extension methods for working with declarations.</summary>
internal static class DeclarationExtensions
{
    /// <summary>Converts a list of declarations to their corresponding namespace.</summary>
    /// <param name="declarations">The list of declarations to convert.</param>
    /// <returns>The namespace represented by the declarations.</returns>
    public static string ToNamespace(this EquatableImmutableArray<Declaration> declarations)
    {
        var builder = StringBuilderPool.Get();

        for (var index = 0; index < declarations.Count; index++)
        {
            var declaration = declarations[index];
            if (declaration.Type != DeclarationType.Namespace)
                continue;

            if (builder.Length > 0)
                builder.Append('.');
            builder.Append(declaration.Name);
        }

        return StringBuilderPool.ToStringAndReturn(builder);
    }

    /// <summary>Converts a list of declarations to their fully qualified name.</summary>
    /// <param name="declarations">The list of declarations to convert.</param>
    /// <returns>The fully qualified name represented by the declarations.</returns>
    public static string ToFullyQualifiedName(this EquatableImmutableArray<Declaration> declarations)
    {
        var builder = StringBuilderPool.Get();

        for (var index = 0; index < declarations.Count; index++)
        {
            var declaration = declarations[index];

            if (builder.Length > 0)
                builder.Append('.');
            builder.Append(declaration.Name);

            if (declaration.GenericParameters.Count == 0)
                continue;

            builder.Append('`');
            builder.Append(declaration.GenericParameters.Count);
        }

        return StringBuilderPool.ToStringAndReturn(builder);
    }

    public static EquatableImmutableArray<Declaration> Where(this EquatableImmutableArray<Declaration> declarations, Func<Declaration, bool> predicate)
    {
        var containingTypes = new List<Declaration>();

        for (var index = 0; index < declarations.Count; index++)
        {
            var containingDeclaration = declarations[index];
            if (predicate(containingDeclaration))
                containingTypes.Add(containingDeclaration);
        }

        return containingTypes.ToEquatableImmutableArray();
    }
}
