using System.Collections.Immutable;
using System.Globalization;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using static GeneratedEndpoints.Common.Constants;

namespace GeneratedEndpoints.Common;

// ReSharper disable ForCanBeConvertedToForeach
// ReSharper disable LoopCanBeConvertedToQuery
// Do not refactor, use for loop to avoid allocations.

internal static class MethodSymbolExtensions
{
    public static EquatableImmutableArray<Parameter> GetParameters(this IMethodSymbol methodSymbol, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var methodParameters = ImmutableArray.CreateBuilder<Parameter>(methodSymbol.Parameters.Length);

        for (var index = 0; index < methodSymbol.Parameters.Length; index++)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var parameterSymbol = methodSymbol.Parameters[index];
            var parameterName = EscapeIdentifier(parameterSymbol.Name);
            var parameterType = parameterSymbol.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            var bindingPrefix = GetBindingPrefix(parameterSymbol);
            var defaultValue = GetDefaultValue(parameterSymbol);
            var parameter = new Parameter(parameterName, parameterType, bindingPrefix, defaultValue);

            methodParameters.Add(parameter);
        }

        return methodParameters.ToEquatableImmutable();
    }

    private static string EscapeIdentifier(string identifier)
    {
        return SyntaxFacts.GetKeywordKind(identifier) == SyntaxKind.None ? identifier : $"@{identifier}";
    }

    private static string GetDefaultValue(IParameterSymbol parameter)
    {
        if (!parameter.HasExplicitDefaultValue)
            return "";

        return $" = {GetDefaultValueLiteral(parameter.Type, parameter.ExplicitDefaultValue)}";
    }

    private static string GetDefaultValueLiteral(ITypeSymbol type, object? value)
    {
        if (value is null)
            return "null";

        if (type.TypeKind == TypeKind.Enum)
            return GetEnumDefaultValueLiteral(type, value);

        return type.SpecialType switch
        {
            SpecialType.System_String => ((string?)value).ToStringLiteral(),
            SpecialType.System_Char => $"'{EscapeChar((char)value)}'",
            SpecialType.System_Boolean => (bool)value ? "true" : "false",
            SpecialType.System_Double => GetDoubleLiteral((double)value),
            SpecialType.System_Single => GetSingleLiteral((float)value),
            SpecialType.System_Decimal => ((decimal)value).ToString(CultureInfo.InvariantCulture) + "m",
            SpecialType.System_SByte => ((sbyte)value).ToString(CultureInfo.InvariantCulture),
            SpecialType.System_Byte => ((byte)value).ToString(CultureInfo.InvariantCulture),
            SpecialType.System_Int16 => ((short)value).ToString(CultureInfo.InvariantCulture),
            SpecialType.System_UInt16 => ((ushort)value).ToString(CultureInfo.InvariantCulture),
            SpecialType.System_Int32 => ((int)value).ToString(CultureInfo.InvariantCulture),
            SpecialType.System_UInt32 => ((uint)value).ToString(CultureInfo.InvariantCulture) + "u",
            SpecialType.System_Int64 => ((long)value).ToString(CultureInfo.InvariantCulture) + "L",
            SpecialType.System_UInt64 => ((ulong)value).ToString(CultureInfo.InvariantCulture) + "UL",
            _ => "default",
        };
    }

    private static string GetEnumDefaultValueLiteral(ITypeSymbol type, object? value)
    {
        var field = type.GetMembers()
            .OfType<IFieldSymbol>()
            .FirstOrDefault(f => f.HasConstantValue && Equals(f.ConstantValue, value));

        var typeName = type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
        if (field is not null)
            return $"{typeName}.{field.Name}";

        if (type is not INamedTypeSymbol namedTypeSymbol)
            return $"default({typeName})";

        var underlying = namedTypeSymbol.EnumUnderlyingType;
        if (underlying is null)
            return $"default({typeName})";

        var literal = underlying.SpecialType switch
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

        return $"({typeName}){literal}";
    }

    private static string GetDoubleLiteral(double value)
    {
        if (double.IsNaN(value))
            return "global::System.Double.NaN";
        if (double.IsPositiveInfinity(value))
            return "global::System.Double.PositiveInfinity";
        if (double.IsNegativeInfinity(value))
            return "global::System.Double.NegativeInfinity";

        return value.ToString("R", CultureInfo.InvariantCulture);
    }

    private static string GetSingleLiteral(float value)
    {
        if (float.IsNaN(value))
            return "global::System.Single.NaN";
        if (float.IsPositiveInfinity(value))
            return "global::System.Single.PositiveInfinity";
        if (float.IsNegativeInfinity(value))
            return "global::System.Single.NegativeInfinity";

        return value.ToString("R", CultureInfo.InvariantCulture) + "f";
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

    private static string GetBindingPrefix(IParameterSymbol parameter)
    {
        var source = BindingSource.None;
        TypedConstant? typedKey = null;
        string? bindingName = null;

        foreach (var attribute in parameter.GetAttributes())
        {
            var attributeClass = attribute.AttributeClass;
            if (attributeClass is null)
                continue;

            var attributeSource = attributeClass.GetBindingSource();
            if (attributeSource == BindingSource.None)
                continue;

            source = attributeSource;
            switch (attributeSource)
            {
                case BindingSource.FromRoute:
                case BindingSource.FromQuery:
                case BindingSource.FromHeader:
                case BindingSource.FromForm:
                    bindingName = attribute.GetNamedStringValue(NameAttributeNamedParameter);
                    break;
                case BindingSource.FromKeyedServices:
                    typedKey = attribute.ConstructorArguments.Length > 0 ? attribute.ConstructorArguments[0] : null;
                    break;
            }
        }

        var bindingPrefix = GetBindingSourceAttribute(source, typedKey, bindingName);

        return bindingPrefix;
    }

    private static string GetBindingSourceAttribute(BindingSource source, TypedConstant? typedKey, string? bindingName)
    {
        switch (source)
        {
            case BindingSource.None:
                return "";
            case BindingSource.FromRoute:
                return FormatBindingAttribute("FromRoute", bindingName);
            case BindingSource.FromQuery:
                return FormatBindingAttribute("FromQuery", bindingName);
            case BindingSource.FromHeader:
                return FormatBindingAttribute("FromHeader", bindingName);
            case BindingSource.FromBody:
                return FormatBindingAttribute("FromBody", bindingName);
            case BindingSource.FromForm:
                return FormatBindingAttribute("FromForm", bindingName);
            case BindingSource.FromServices:
                return "[FromServices] ";
            case BindingSource.FromKeyedServices:
                var key = typedKey?.ToConstLiteral();
                return $"[FromKeyedServices({key})] ";
            case BindingSource.AsParameters:
                return "[AsParameters] ";
            default:
                return "";
        }
    }

    private static string FormatBindingAttribute(string attributeName, string? bindingName)
    {
        if (bindingName is null)
            return $"[{attributeName}] ";

        return $"[{attributeName}(Name = {bindingName.ToStringLiteral()})] ";
    }
}
