using System.Security.Cryptography;
using System.Text;

namespace GeneratedEndpoints.Tests.Common;

public static class ScenarioNamer
{
    public static string Create(string prefix, params (string Name, object? Value)[] parts)
    {
        var descriptor = new StringBuilder();

        foreach (var (name, value) in parts)
        {
            descriptor.Append(name);
            descriptor.Append('=');
            descriptor.Append(Sanitize(value));
            descriptor.Append(';');
        }

        using var sha256 = SHA256.Create();
        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(descriptor.ToString()));
        var hash = Convert.ToHexString(bytes.AsSpan(0, 6));
        return $"{prefix}_{hash}";
    }

    private static string Sanitize(object? value)
    {
        if (value is null)
            return "None";

        return value switch
        {
            bool b => b ? "On" : "Off",
            string s => s,
            _ => value.ToString() ?? "Value",
        };
    }
}
