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
        var sources = TestHelpers.GetSources("""
                                             [Tags("Users")]
                                             internal static class GetUserEndpoint
                                             {
                                                 [MapGet("/users/{id:int}", Name = nameof(GetUser), Summary = "Gets a user by ID.", Description = "Gets a user by ID when the ID is greater than zero.")]
                                                 public static Results<Ok, NotFound> GetUser2(int id)
                                                 {
                                                     if (id > 0)
                                                         return TypedResults.Ok();

                                                     return TypedResults.NotFound();
                                                 }
                                             }
                                             """, withNamespace
        );
        var result = TestHelpers.RunGenerator(sources);

        await result.VerifyAsync("AddEndpointHandlers.g.cs")
            .UseMethodName($"{nameof(MapGet)}_AddEndpointHandlers_With{(withNamespace ? "" : "out")}Namespace");

        await result.VerifyAsync("MapEndpointHandlers.g.cs")
            .UseMethodName($"{nameof(MapGet)}_MapEndpointHandlers_With{(withNamespace ? "" : "out")}Namespace");
    }
}
