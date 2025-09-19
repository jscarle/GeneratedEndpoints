namespace GeneratedEndpoints.Common;

/// <summary>Represents a declaration.</summary>
public sealed record Declaration
{
    /// <summary>Gets the type of declaration.</summary>
    public DeclarationType Type { get; }

    /// <summary>Gets the name of the declaration.</summary>
    public string Name { get; }

    /// <summary>Gets a read-only list of generic parameter names for generic declarations, or an empty list otherwise.</summary>
    public EquatableImmutableArray<string> GenericParameters { get; }

    /// <summary>Initializes a new instance of the <see cref="Declaration"/> class with the specified type, name, and generic parameters.</summary>
    /// <param name="type">The type of declaration.</param>
    /// <param name="name">The name of the declaration.</param>
    /// <param name="genericParameters">A read-only list of generic parameter names, or an empty list if not generic.</param>
    internal Declaration(DeclarationType type, string name, EquatableImmutableArray<string> genericParameters)
    {
        Type = type;
        Name = name;
        GenericParameters = genericParameters;
    }

    /// <summary>Returns a string representation of the declaration in the appropriate format for its type.</summary>
    /// <returns>A string representation of the declaration.</returns>
    public override string ToString()
    {
        switch (Type)
        {
            case DeclarationType.Namespace:
                return $"namespace {Name}";
            case DeclarationType.Interface:
            case DeclarationType.Class:
            case DeclarationType.Record:
            case DeclarationType.Struct:
            case DeclarationType.RecordStruct:
                var keyword = ToKeyword(Type);
                return GenericParameters.Count == 0 ? $"{keyword} {Name}" : $"{keyword} {Name}<{string.Join(", ", GenericParameters)}>";
            default:
                return base.ToString();
        }
    }

    private static string ToKeyword(DeclarationType declarationType)
    {
        return declarationType switch
        {
            DeclarationType.Namespace => "namespace",
            DeclarationType.Interface => "interface",
            DeclarationType.Class => "class",
            DeclarationType.Record => "record",
            DeclarationType.Struct => "struct",
            DeclarationType.RecordStruct => "record struct",
            _ => throw new InvalidOperationException(),
        };
    }
}
