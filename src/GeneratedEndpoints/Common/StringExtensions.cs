using System.Globalization;

namespace GeneratedEndpoints.Common;

internal static class StringExtensions
{
    public static string ToStringLiteral(this string? value)
    {
        if (value is null)
            return "null";

        var firstEscapeIndex = -1;
        for (var i = 0; i < value.Length; i++)
        {
            var c = value[i];
            if (c == '\"' || c == '\\' || c == '\n' || c == '\r' || c == '\t' || c == '\0' || char.IsControl(c))
            {
                firstEscapeIndex = i;
                break;
            }
        }

        if (firstEscapeIndex < 0)
            return string.Concat("\"", value, "\"");

        var sb = StringBuilderPool.Get(value.Length + 2);
        sb.Append('"');
        if (firstEscapeIndex > 0)
            sb.Append(value, 0, firstEscapeIndex);

        for (var i = firstEscapeIndex; i < value.Length; i++)
        {
            var c = value[i];
            switch (c)
            {
                case '\"':
                    sb.Append("\\\"");
                    break;
                case '\\':
                    sb.Append("\\\\");
                    break;
                case '\n':
                    sb.Append("\\n");
                    break;
                case '\r':
                    sb.Append("\\r");
                    break;
                case '\t':
                    sb.Append("\\t");
                    break;
                case '\0':
                    sb.Append("\\0");
                    break;
                default:
                    if (char.IsControl(c))
                        sb.Append("\\u")
                            .Append(((int)c).ToString("x4", CultureInfo.InvariantCulture));
                    else
                        sb.Append(c);

                    break;
            }
        }

        sb.Append('"');
        return StringBuilderPool.ToStringAndReturn(sb);
    }
}
