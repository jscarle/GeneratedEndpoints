using GeneratedEndpoints.Tests.Common;
using Microsoft.CodeAnalysis;
using SourceGeneratorTestHelpers.XUnit;

namespace GeneratedEndpoints.Tests;

[UsesVerify]
public class AttributeGenerationTests
{
    private const string AttributeTestSource = "internal static class AttributeTestEndpoints { }";
    private static readonly GeneratorDriverRunResult GeneratorResult =
        TestHelpers.RunGenerator(TestHelpers.GetSources(AttributeTestSource, withNamespace: true));

    public AttributeGenerationTests()
    {
        ModuleInitializer.Initialize();
    }

    [Fact]
    public Task MapGetAttribute()
        => VerifyAttributeAsync("Microsoft.AspNetCore.Generated.Attributes.MapGetAttribute.gs.cs");

    [Fact]
    public Task MapPostAttribute()
        => VerifyAttributeAsync("Microsoft.AspNetCore.Generated.Attributes.MapPostAttribute.gs.cs");

    [Fact]
    public Task MapPutAttribute()
        => VerifyAttributeAsync("Microsoft.AspNetCore.Generated.Attributes.MapPutAttribute.gs.cs");

    [Fact]
    public Task MapPatchAttribute()
        => VerifyAttributeAsync("Microsoft.AspNetCore.Generated.Attributes.MapPatchAttribute.gs.cs");

    [Fact]
    public Task MapDeleteAttribute()
        => VerifyAttributeAsync("Microsoft.AspNetCore.Generated.Attributes.MapDeleteAttribute.gs.cs");

    [Fact]
    public Task MapOptionsAttribute()
        => VerifyAttributeAsync("Microsoft.AspNetCore.Generated.Attributes.MapOptionsAttribute.gs.cs");

    [Fact]
    public Task MapHeadAttribute()
        => VerifyAttributeAsync("Microsoft.AspNetCore.Generated.Attributes.MapHeadAttribute.gs.cs");

    [Fact]
    public Task MapQueryAttribute()
        => VerifyAttributeAsync("Microsoft.AspNetCore.Generated.Attributes.MapQueryAttribute.gs.cs");

    [Fact]
    public Task MapTraceAttribute()
        => VerifyAttributeAsync("Microsoft.AspNetCore.Generated.Attributes.MapTraceAttribute.gs.cs");

    [Fact]
    public Task MapConnectAttribute()
        => VerifyAttributeAsync("Microsoft.AspNetCore.Generated.Attributes.MapConnectAttribute.gs.cs");

    [Fact]
    public Task MapFallbackAttribute()
        => VerifyAttributeAsync("Microsoft.AspNetCore.Generated.Attributes.MapFallbackAttribute.gs.cs");

    [Fact]
    public Task RequireAuthorizationAttribute()
        => VerifyAttributeAsync("Microsoft.AspNetCore.Generated.Attributes.RequireAuthorizationAttribute.gs.cs");

    [Fact]
    public Task RequireCorsAttribute()
        => VerifyAttributeAsync("Microsoft.AspNetCore.Generated.Attributes.RequireCorsAttribute.gs.cs");

    [Fact]
    public Task RequireRateLimitingAttribute()
        => VerifyAttributeAsync("Microsoft.AspNetCore.Generated.Attributes.RequireRateLimitingAttribute.gs.cs");

    [Fact]
    public Task RequireHostAttribute()
        => VerifyAttributeAsync("Microsoft.AspNetCore.Generated.Attributes.RequireHostAttribute.gs.cs");

    [Fact]
    public Task DisableAntiforgeryAttribute()
        => VerifyAttributeAsync("Microsoft.AspNetCore.Generated.Attributes.DisableAntiforgeryAttribute.gs.cs");

    [Fact]
    public Task ShortCircuitAttribute()
        => VerifyAttributeAsync("Microsoft.AspNetCore.Generated.Attributes.ShortCircuitAttribute.gs.cs");

    [Fact]
    public Task DisableRequestTimeoutAttribute()
        => VerifyAttributeAsync("Microsoft.AspNetCore.Generated.Attributes.DisableRequestTimeoutAttribute.gs.cs");

    [Fact]
    public Task DisableValidationAttribute()
        => VerifyAttributeAsync("Microsoft.AspNetCore.Generated.Attributes.DisableValidationAttribute.gs.cs");

    [Fact]
    public Task RequestTimeoutAttribute()
        => VerifyAttributeAsync("Microsoft.AspNetCore.Generated.Attributes.RequestTimeoutAttribute.gs.cs");

    [Fact]
    public Task OrderAttribute()
        => VerifyAttributeAsync("Microsoft.AspNetCore.Generated.Attributes.OrderAttribute.gs.cs");

    [Fact]
    public Task MapGroupAttribute()
        => VerifyAttributeAsync("Microsoft.AspNetCore.Generated.Attributes.MapGroupAttribute.gs.cs");

    [Fact]
    public Task SummaryAttribute()
        => VerifyAttributeAsync("Microsoft.AspNetCore.Generated.Attributes.SummaryAttribute.gs.cs");

    [Fact]
    public Task AcceptsAttribute()
        => VerifyAttributeAsync("Microsoft.AspNetCore.Generated.Attributes.AcceptsAttribute.gs.cs");

    [Fact]
    public Task EndpointFilterAttribute()
        => VerifyAttributeAsync("Microsoft.AspNetCore.Generated.Attributes.EndpointFilterAttribute.gs.cs");

    [Fact]
    public Task ProducesResponseAttribute()
        => VerifyAttributeAsync("Microsoft.AspNetCore.Generated.Attributes.ProducesResponseAttribute.gs.cs");

    [Fact]
    public Task ProducesProblemAttribute()
        => VerifyAttributeAsync("Microsoft.AspNetCore.Generated.Attributes.ProducesProblemAttribute.gs.cs");

    [Fact]
    public Task ProducesValidationProblemAttribute()
        => VerifyAttributeAsync("Microsoft.AspNetCore.Generated.Attributes.ProducesValidationProblemAttribute.gs.cs");

    private static Task VerifyAttributeAsync(string fileName)
        => GeneratorResult.VerifyAsync(fileName);
}
