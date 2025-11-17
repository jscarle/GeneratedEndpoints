using System.Runtime.CompilerServices;

namespace GeneratedEndpoints.Common;

internal readonly struct HandlerNameKey(string name, string method) : IEquatable<HandlerNameKey>
{
    private readonly string _name = name;
    private readonly string _method = method;
    private readonly int _hashCode = CombineHashCodes(StringComparer.Ordinal.GetHashCode(name), StringComparer.Ordinal.GetHashCode(method));

    public bool Equals(HandlerNameKey other)
    {
        return ReferenceEquals(_name, other._name) && ReferenceEquals(_method, other._method)
               || (string.Equals(_name, other._name, StringComparison.Ordinal)
                   && string.Equals(_method, other._method, StringComparison.Ordinal));
    }

    public override bool Equals(object? obj)
    {
        return obj is HandlerNameKey other && Equals(other);
    }

    public override int GetHashCode()
    {
        return _hashCode;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int CombineHashCodes(int left, int right)
    {
        unchecked
        {
            return (left * 397) ^ right;
        }
    }
}
