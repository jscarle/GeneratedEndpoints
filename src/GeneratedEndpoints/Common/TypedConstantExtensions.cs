using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Microsoft.CodeAnalysis;

namespace GeneratedEndpoints.Common;

internal static class TypedConstantExtensions
{
    [SuppressMessage("Globalization", "CA1308: Normalize strings to uppercase", Justification = "C# boolean literals must be lowercase.")]
    public static string ToConstLiteral(this TypedConstant tc)
    {
        if (tc.IsNull)
            return "null";

        if (tc.Kind == TypedConstantKind.Array)
            return ToArrayLiteral(tc);

        var v = tc.Value;
        var t = tc.Type;
        if (t is null)
            return "null";

        if (v is ITypeSymbol typeSymbol)
            return $"typeof({typeSymbol.ToDisplayString(SymbolExtensions.FullyQualifiedTypeDisplayFormat)})";

        if (t.TypeKind != TypeKind.Enum)
            return t.SpecialType switch
            {
                SpecialType.System_String => ((string?)v).ToStringLiteral(),
                SpecialType.System_Char => $"'{EscapeChar((char)v!)}'",
                SpecialType.System_Boolean => ((bool)v!).ToString()
                    .ToLowerInvariant(),
                SpecialType.System_Double => ((double)v!).ToString("R", CultureInfo.InvariantCulture),
                SpecialType.System_Single => ((float)v!).ToString("R", CultureInfo.InvariantCulture) + "f",
                SpecialType.System_Decimal => ((decimal)v!).ToString(CultureInfo.InvariantCulture) + "m",
                SpecialType.System_SByte => ((sbyte)v!).ToString(CultureInfo.InvariantCulture),
                SpecialType.System_Byte => ((byte)v!).ToString(CultureInfo.InvariantCulture),
                SpecialType.System_Int16 => ((short)v!).ToString(CultureInfo.InvariantCulture),
                SpecialType.System_UInt16 => ((ushort)v!).ToString(CultureInfo.InvariantCulture),
                SpecialType.System_Int32 => ((int)v!).ToString(CultureInfo.InvariantCulture),
                SpecialType.System_UInt32 => ((uint)v!).ToString(CultureInfo.InvariantCulture) + "u",
                SpecialType.System_Int64 => ((long)v!).ToString(CultureInfo.InvariantCulture) + "L",
                SpecialType.System_UInt64 => ((ulong)v!).ToString(CultureInfo.InvariantCulture) + "UL",
                _ => (v?.ToString()).ToStringLiteral(),
            };

        var field = t.GetMembers()
            .OfType<IFieldSymbol>()
            .FirstOrDefault(f => f.HasConstantValue && Equals(f.ConstantValue, v));

        if (field is not null)
            return $"{t.ToDisplayString(SymbolExtensions.FullyQualifiedTypeDisplayFormat)}.{field.Name}";

        var underlying = ((INamedTypeSymbol)t).EnumUnderlyingType!;
        var num = IntegralLiteral(v, underlying.SpecialType);
        return $"({t.ToDisplayString(SymbolExtensions.FullyQualifiedTypeDisplayFormat)}){num}";
    }

    private static string ToArrayLiteral(TypedConstant tc)
    {
        var arrayType = tc.Type?.ToDisplayString(SymbolExtensions.FullyQualifiedTypeDisplayFormat) ?? "object[]";
        var builder = StringBuilderPool.Get();
        builder.Append("new ");
        builder.Append(arrayType);
        builder.Append(" {");

        for (var index = 0; index < tc.Values.Length; index++)
        {
            if (index > 0)
                builder.Append(',');

            builder.Append(' ');
            builder.Append(tc.Values[index].ToConstLiteral());
        }

        if (tc.Values.Length > 0)
            builder.Append(' ');

        builder.Append('}');
        return StringBuilderPool.ToStringAndReturn(builder);
    }

    private static string EscapeChar(char c)
    {
        return c switch
        {
            '\'' => "\\'",
            '\\' => "\\\\",
            '\n' => "\\n",
            '\r' => "\\r",
            '\t' => "\\t",
            '\0' => "\\0",
            _ when char.IsControl(c) => "\\u" + ((int)c).ToString("x4", CultureInfo.InvariantCulture),
            _ => c.ToString(),
        };
    }

    private static string IntegralLiteral(object? value, SpecialType underlying)
    {
        return underlying switch
        {
            SpecialType.System_SByte => ((sbyte)value!).ToString(CultureInfo.InvariantCulture),
            SpecialType.System_Byte => ((byte)value!).ToString(CultureInfo.InvariantCulture),
            SpecialType.System_Int16 => ((short)value!).ToString(CultureInfo.InvariantCulture),
            SpecialType.System_UInt16 => ((ushort)value!).ToString(CultureInfo.InvariantCulture),
            SpecialType.System_Int32 => ((int)value!).ToString(CultureInfo.InvariantCulture),
            SpecialType.System_UInt32 => ((uint)value!).ToString(CultureInfo.InvariantCulture) + "u",
            SpecialType.System_Int64 => ((long)value!).ToString(CultureInfo.InvariantCulture) + "L",
            SpecialType.System_UInt64 => ((ulong)value!).ToString(CultureInfo.InvariantCulture) + "UL",
            _ => "0",
        };
    }
}
