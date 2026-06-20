using System.Collections.Immutable;
using System.Globalization;
using System.Text;
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
            var parameterType = parameterSymbol.Type.ToDisplayString(SymbolExtensions.FullyQualifiedNullableTypeDisplayFormat);
            var attributePrefix = GetNonBindingAttributePrefix(parameterSymbol);
            var bindingPrefix = GetBindingPrefix(parameterSymbol);
            var defaultValue = GetDefaultValue(parameterSymbol);
            var parameter = new Parameter(parameterName, parameterType, attributePrefix, bindingPrefix, defaultValue);

            methodParameters.Add(parameter);
        }

        return methodParameters.ToEquatableImmutable();
    }

    internal static string EscapeIdentifier(string identifier)
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
            return CanUseNullDefaultLiteral(type) ? "null" : "default";

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

    private static bool CanUseNullDefaultLiteral(ITypeSymbol type)
    {
        if (!type.IsValueType)
            return true;

        return type.OriginalDefinition.SpecialType == SpecialType.System_Nullable_T;
    }

    private static string GetEnumDefaultValueLiteral(ITypeSymbol type, object? value)
    {
        var field = type.GetMembers()
            .OfType<IFieldSymbol>()
            .FirstOrDefault(f => f.HasConstantValue && Equals(f.ConstantValue, value));

        var typeName = type.ToDisplayString(SymbolExtensions.FullyQualifiedTypeDisplayFormat);
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
        string? emptyBodyBehavior = null;

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
                case BindingSource.FromBody:
                    emptyBodyBehavior = attribute.GetNamedConstantValue(EmptyBodyBehaviorAttributeNamedParameter);
                    break;
                case BindingSource.FromKeyedServices:
                    typedKey = attribute.ConstructorArguments.Length > 0 ? attribute.ConstructorArguments[0] : null;
                    break;
            }
        }

        var bindingPrefix = GetBindingSourceAttribute(source, typedKey, bindingName, emptyBodyBehavior);

        return bindingPrefix;
    }

    private static string GetNonBindingAttributePrefix(IParameterSymbol parameter)
    {
        StringBuilder? builder = null;

        foreach (var attribute in parameter.GetAttributes())
        {
            var attributeClass = attribute.AttributeClass;
            if (attributeClass is null)
                continue;

            if (attributeClass.GetBindingSource() != BindingSource.None)
                continue;

            if (!attributeClass.IsTypeAccessibleFromGeneratedCode())
                continue;

            builder ??= StringBuilderPool.Get();
            AppendAttribute(builder, attribute, attributeClass);
            builder.Append(' ');
        }

        return builder is null ? "" : StringBuilderPool.ToStringAndReturn(builder);
    }

    private static void AppendAttribute(StringBuilder builder, AttributeData attribute, INamedTypeSymbol attributeClass)
    {
        builder.Append('[');
        builder.Append(attributeClass.ToDisplayString(SymbolExtensions.FullyQualifiedTypeDisplayFormat));

        var hasConstructorArguments = attribute.ConstructorArguments.Length > 0;
        var hasNamedArguments = attribute.NamedArguments.Length > 0;
        if (hasConstructorArguments || hasNamedArguments)
        {
            builder.Append('(');

            var hasArgument = false;
            foreach (var argument in attribute.ConstructorArguments)
            {
                if (hasArgument)
                    builder.Append(", ");

                builder.Append(argument.ToConstLiteral());
                hasArgument = true;
            }

            foreach (var argument in attribute.NamedArguments)
            {
                if (hasArgument)
                    builder.Append(", ");

                builder.Append(EscapeIdentifier(argument.Key));
                builder.Append(" = ");
                builder.Append(argument.Value.ToConstLiteral());
                hasArgument = true;
            }

            builder.Append(')');
        }

        builder.Append(']');
    }

    private static string GetBindingSourceAttribute(BindingSource source, TypedConstant? typedKey, string? bindingName, string? emptyBodyBehavior)
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
                return FormatFromBodyAttribute(emptyBodyBehavior);
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

    private static string FormatFromBodyAttribute(string? emptyBodyBehavior)
    {
        if (emptyBodyBehavior is null)
            return "[FromBody] ";

        return $"[FromBody(EmptyBodyBehavior = {emptyBodyBehavior})] ";
    }
}
