using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Generated.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace GeneratedEndpoints.Tests.Lab;

[Tags("Users", "Profiles")]
[RequireAuthorization("Users.Read", "Administrators")]
[DisableAntiforgery]
internal static class GetUserEndpoint
{
    [Tags("Featured")]
    [AllowAnonymous]
    [Accepts("application/json", "application/xml", RequestType = typeof(GetUserRequest))]
    [Accepts<GetUserMetadata>("application/json", "application/xml")]
    [ProducesResponse(StatusCodes.Status200OK, "application/json", ResponseType = typeof(UserProfile))]
    [ProducesResponse<UserProfile>(StatusCodes.Status202Accepted, "application/json")]
    [ProducesProblem(StatusCodes.Status500InternalServerError, "application/problem+json")]
    [ProducesValidationProblem(StatusCodes.Status400BadRequest, "application/problem+json")]
    [MapGet("/users/{id:int}", Name = nameof(GetUser), Summary = "Gets a user by ID.", Description = "Gets a user by ID when the ID is greater than zero.")]
    public static Results<Ok<UserProfile>, NotFound, ValidationProblem, ProblemHttpResult> GetUser(int id)
    {
        if (id <= 0)
        {
            var errors = new Dictionary<string, string[]>
            {
                [nameof(id)] = ["The ID must be greater than zero."]
            };
            return TypedResults.ValidationProblem(errors);
        }

        if (id == 13)
        {
            return TypedResults.Problem("User data is temporarily unavailable.");
        }

        if (id == 404)
        {
            return TypedResults.NotFound();
        }

        var profile = new UserProfile(id, $"User {id}", $"user{id}@example.com");
        return TypedResults.Ok(profile);
    }

    public static void Configure<TBuilder>(TBuilder builder)
        where TBuilder : IEndpointConventionBuilder
    {
    }
}

internal sealed record GetUserRequest(int Id);

internal sealed record GetUserMetadata(string RequestedBy, string Purpose);

internal sealed record UserProfile(int Id, string DisplayName, string Email);
