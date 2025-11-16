using Microsoft.CodeAnalysis;

namespace GeneratedEndpoints.Common;

internal sealed class CompilationTypeCache(Compilation compilation)
{
    public INamedTypeSymbol? EndpointConventionBuilderSymbol { get; } =
        compilation.GetTypeByMetadataName("Microsoft.AspNetCore.Builder.IEndpointConventionBuilder");

    public INamedTypeSymbol? ServiceProviderSymbol { get; } = compilation.GetTypeByMetadataName("System.IServiceProvider");
}
