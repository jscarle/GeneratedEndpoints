using System.Linq;
using GeneratedEndpoints.Tests.Common;
using Microsoft.CodeAnalysis;
using Xunit;

namespace GeneratedEndpoints.Tests;

public class DiagnosticTests
{
    [Fact]
    public void Reports_InvalidEndpointContainer()
    {
        var source = """
            internal struct InvalidEndpoints
            {
                [MapGet("/invalid")]
                public void Handle()
                {
                }
            }
            """;

        var result = TestHelpers.RunGenerator(TestHelpers.GetSources(source, withNamespace: false));
        var diagnostic = Assert.Single(result.Diagnostics, d => d.Id == "GE0002");
        Assert.Equal(DiagnosticSeverity.Error, diagnostic.Severity);
    }

    [Fact]
    public void Reports_ConflictingAuthorizationAttributes()
    {
        var source = """
            internal sealed class AuthorizationEndpoints
            {
                [AllowAnonymous]
                [RequireAuthorization]
                [MapGet("/conflict")]
                public void Handle()
                {
                }
            }
            """;

        var result = TestHelpers.RunGenerator(TestHelpers.GetSources(source, withNamespace: false));
        var diagnostic = Assert.Single(result.Diagnostics, d => d.Id == "GE0003");
        Assert.Equal(DiagnosticSeverity.Error, diagnostic.Severity);
    }

    [Fact]
    public void Reports_InvalidBindingAnnotation()
    {
        var source = """
            internal sealed class BindingEndpoints
            {
                [MapGet("/items/{id}")]
                public void Get([FromRoute(Name = " ")] int id)
                {
                }
            }
            """;

        var result = TestHelpers.RunGenerator(TestHelpers.GetSources(source, withNamespace: false));
        var diagnostic = Assert.Single(result.Diagnostics, d => d.Id == "GE0005");
        Assert.Equal(DiagnosticSeverity.Error, diagnostic.Severity);
    }

    [Fact]
    public void Reports_DuplicateEndpointNames()
    {
        var source = """
            internal sealed class DuplicateEndpoints
            {
                [MapGet("/one", Name = "SharedName")]
                public void One()
                {
                }

                [MapPost("/two", Name = "SharedName")]
                public void Two()
                {
                }
            }
            """;

        var result = TestHelpers.RunGenerator(TestHelpers.GetSources(source, withNamespace: false));
        var duplicateDiagnostics = result.Diagnostics.Where(d => d.Id == "GE0004").ToList();
        Assert.Equal(2, duplicateDiagnostics.Count);
        Assert.All(duplicateDiagnostics, d => Assert.Equal(DiagnosticSeverity.Warning, d.Severity));
    }
}
