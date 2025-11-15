using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Generated.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace GeneratedEndpoints.Tests.Lab;

internal static class GetUserEndpoint
{
    [MapGet("/users/{id:int}", Name = nameof(GetUser), Summary = "Gets a user by ID.", Description = "Gets a user by ID when the ID is greater than zero.")]
    public static Results<Ok, NotFound> GetUser(int id)
    {
        if (id > 0)
            return TypedResults.Ok();

        return TypedResults.NotFound();
    }

    public static void Configure<TBuilder>(TBuilder builder)
        where TBuilder : IEndpointConventionBuilder
    {
    }
}
