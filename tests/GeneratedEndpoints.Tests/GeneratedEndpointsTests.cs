using GeneratedEndpoints.Tests.Common;
using SourceGeneratorTestHelpers.XUnit;

namespace GeneratedEndpoints.Tests;

[UsesVerify]
public class GeneratedEndpointsTests
{
    public GeneratedEndpointsTests()
    {
        ModuleInitializer.Initialize();
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task MapGet(bool withNamespace)
    {
        var sources = TestHelpers.GetSources(
            """

            """,
            withNamespace
        );
        var result = TestHelpers.RunGenerator(sources);

        //await result.VerifyAsync("GeneratedSource.g.cs").UseMethodName($"{nameof(UsingDbSets)}_With{(withNamespace ? "" : "out")}Namespace");
    }
}
