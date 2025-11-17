using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Generated.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable UnusedParameter.Global
#pragma warning disable CS9113 // Parameter is unread.

namespace GeneratedEndpoints.Tests.Lab;

[Tags("Users", "Profiles")]
[RequireAuthorization("Users.Read", "Administrators")]
[DisableAntiforgery]
internal sealed class GetUserEndpoint(IServiceProvider serviceProvider)
{
    [Tags("Featured")]
    [AllowAnonymous]
    [Accepts(typeof(GetUserRequest), "application/json", "application/xml")]
    [Accepts<GetUserMetadata>("application/json", "application/xml", IsOptional = true)]
    [ProducesResponse(typeof(UserProfile), StatusCodes.Status200OK, "application/json")]
    [ProducesResponse<UserProfile>(StatusCodes.Status202Accepted, "application/json")]
    [ProducesProblem(StatusCodes.Status500InternalServerError, "application/problem+json")]
    [ProducesValidationProblem(StatusCodes.Status400BadRequest, "application/problem+json")]
    [DisplayName("User lookup endpoint")]
    [Description("Gets a user by ID when the ID is greater than zero.")]
    [Summary("Gets a user by ID.")]
    [MapGet("/users/{id:int}", Name = nameof(GetUser))]
    public async ValueTask<Results<Ok<UserProfile>, NotFound, ValidationProblem, ProblemHttpResult>> GetUser(
        [FromHeader(Name = "4")] int id,
        [FromKeyedServices(ServiceLifetime.Scoped)] IServiceCollection services
    )
    {
        await Task.Yield();

        if (id <= 0)
        {
            var errors = new Dictionary<string, string[]>
            {
                [nameof(id)] = ["The ID must be greater than zero."],
            };
            return TypedResults.ValidationProblem(errors);
        }

        if (id == 13)
            return TypedResults.Problem("User data is temporarily unavailable.");

        if (id == 404)
            return TypedResults.NotFound();

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
