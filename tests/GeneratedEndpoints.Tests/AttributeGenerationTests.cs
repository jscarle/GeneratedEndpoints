using GeneratedEndpoints.Tests.Common;
using Microsoft.CodeAnalysis;
using SourceGeneratorTestHelpers.XUnit;

namespace GeneratedEndpoints.Tests;

public class AttributeGenerationTests
{
    private const string AttributeTestSource = "internal static class AttributeTestEndpoints { }";

    private static readonly GeneratorDriverRunResult GeneratorResult = TestHelpers.RunGenerator(TestHelpers.GetSources(AttributeTestSource, true));

    public AttributeGenerationTests()
    {
        ModuleInitializer.Initialize();
    }

    [Fact]
    public Task MapGetAttribute()
    {
        return VerifyAttributeAsync("Microsoft.AspNetCore.Generated.Attributes.MapGetAttribute.gs.cs");
    }

    [Fact]
    public Task MapPostAttribute()
    {
        return VerifyAttributeAsync("Microsoft.AspNetCore.Generated.Attributes.MapPostAttribute.gs.cs");
    }

    [Fact]
    public Task MapPutAttribute()
    {
        return VerifyAttributeAsync("Microsoft.AspNetCore.Generated.Attributes.MapPutAttribute.gs.cs");
    }

    [Fact]
    public Task MapPatchAttribute()
    {
        return VerifyAttributeAsync("Microsoft.AspNetCore.Generated.Attributes.MapPatchAttribute.gs.cs");
    }

    [Fact]
    public Task MapDeleteAttribute()
    {
        return VerifyAttributeAsync("Microsoft.AspNetCore.Generated.Attributes.MapDeleteAttribute.gs.cs");
    }

    [Fact]
    public Task MapOptionsAttribute()
    {
        return VerifyAttributeAsync("Microsoft.AspNetCore.Generated.Attributes.MapOptionsAttribute.gs.cs");
    }

    [Fact]
    public Task MapHeadAttribute()
    {
        return VerifyAttributeAsync("Microsoft.AspNetCore.Generated.Attributes.MapHeadAttribute.gs.cs");
    }

    [Fact]
    public Task MapQueryAttribute()
    {
        return VerifyAttributeAsync("Microsoft.AspNetCore.Generated.Attributes.MapQueryAttribute.gs.cs");
    }

    [Fact]
    public Task MapTraceAttribute()
    {
        return VerifyAttributeAsync("Microsoft.AspNetCore.Generated.Attributes.MapTraceAttribute.gs.cs");
    }

    [Fact]
    public Task MapConnectAttribute()
    {
        return VerifyAttributeAsync("Microsoft.AspNetCore.Generated.Attributes.MapConnectAttribute.gs.cs");
    }

    [Fact]
    public Task MapFallbackAttribute()
    {
        return VerifyAttributeAsync("Microsoft.AspNetCore.Generated.Attributes.MapFallbackAttribute.gs.cs");
    }

    [Fact]
    public Task RequireAuthorizationAttribute()
    {
        return VerifyAttributeAsync("Microsoft.AspNetCore.Generated.Attributes.RequireAuthorizationAttribute.gs.cs");
    }

    [Fact]
    public Task RequireCorsAttribute()
    {
        return VerifyAttributeAsync("Microsoft.AspNetCore.Generated.Attributes.RequireCorsAttribute.gs.cs");
    }

    [Fact]
    public Task RequireRateLimitingAttribute()
    {
        return VerifyAttributeAsync("Microsoft.AspNetCore.Generated.Attributes.RequireRateLimitingAttribute.gs.cs");
    }

    [Fact]
    public Task RequireHostAttribute()
    {
        return VerifyAttributeAsync("Microsoft.AspNetCore.Generated.Attributes.RequireHostAttribute.gs.cs");
    }

    [Fact]
    public Task DisableAntiforgeryAttribute()
    {
        return VerifyAttributeAsync("Microsoft.AspNetCore.Generated.Attributes.DisableAntiforgeryAttribute.gs.cs");
    }

    [Fact]
    public Task ShortCircuitAttribute()
    {
        return VerifyAttributeAsync("Microsoft.AspNetCore.Generated.Attributes.ShortCircuitAttribute.gs.cs");
    }

    [Fact]
    public Task DisableRequestTimeoutAttribute()
    {
        return VerifyAttributeAsync("Microsoft.AspNetCore.Generated.Attributes.DisableRequestTimeoutAttribute.gs.cs");
    }

    [Fact]
    public Task DisableValidationAttribute()
    {
        return VerifyAttributeAsync("Microsoft.AspNetCore.Generated.Attributes.DisableValidationAttribute.gs.cs");
    }

    [Fact]
    public Task RequestTimeoutAttribute()
    {
        return VerifyAttributeAsync("Microsoft.AspNetCore.Generated.Attributes.RequestTimeoutAttribute.gs.cs");
    }

    [Fact]
    public Task OrderAttribute()
    {
        return VerifyAttributeAsync("Microsoft.AspNetCore.Generated.Attributes.OrderAttribute.gs.cs");
    }

    [Fact]
    public Task MapGroupAttribute()
    {
        return VerifyAttributeAsync("Microsoft.AspNetCore.Generated.Attributes.MapGroupAttribute.gs.cs");
    }

    [Fact]
    public Task SummaryAttribute()
    {
        return VerifyAttributeAsync("Microsoft.AspNetCore.Generated.Attributes.SummaryAttribute.gs.cs");
    }

    [Fact]
    public Task AcceptsAttribute()
    {
        return VerifyAttributeAsync("Microsoft.AspNetCore.Generated.Attributes.AcceptsAttribute.gs.cs");
    }

    [Fact]
    public Task EndpointFilterAttribute()
    {
        return VerifyAttributeAsync("Microsoft.AspNetCore.Generated.Attributes.EndpointFilterAttribute.gs.cs");
    }

    [Fact]
    public Task ProducesResponseAttribute()
    {
        return VerifyAttributeAsync("Microsoft.AspNetCore.Generated.Attributes.ProducesResponseAttribute.gs.cs");
    }

    [Fact]
    public Task ProducesProblemAttribute()
    {
        return VerifyAttributeAsync("Microsoft.AspNetCore.Generated.Attributes.ProducesProblemAttribute.gs.cs");
    }

    [Fact]
    public Task ProducesValidationProblemAttribute()
    {
        return VerifyAttributeAsync("Microsoft.AspNetCore.Generated.Attributes.ProducesValidationProblemAttribute.gs.cs");
    }

    private static Task VerifyAttributeAsync(string fileName)
    {
        return GeneratorResult.VerifyAsync(fileName);
    }
}
