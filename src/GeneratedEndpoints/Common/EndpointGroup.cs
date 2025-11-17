namespace GeneratedEndpoints.Common;

internal readonly record struct EndpointGroup
{
    public required string Identifier { get; init; }
    public required string Pattern { get; init; }
    public required string? Name { get; init; }
}
