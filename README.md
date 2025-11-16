[![Banner](https://raw.githubusercontent.com/jscarle/GeneratedEndpoints/develop/Banner.png)](https://github.com/jscarle/GeneratedEndpoints)

# GeneratedEndpoints - Attribute-driven, source-generated Minimal API endpoints for feature-based development

GeneratedEndpoints is a .NET source generator that automatically wires up Minimal API endpoints from attribute-annotated
methods. This simplifies integration of HTTP handlers within Clean Architecture (CA) or Vertical Slice Architecture (VSA)
by keeping endpoint definitions inside their features while generating the boilerplate mapping code.

[![develop](https://img.shields.io/github/actions/workflow/status/jscarle/GeneratedEndpoints/develop.yml?logo=github)](https://github.com/jscarle/GeneratedEndpoints)
[![nuget](https://img.shields.io/nuget/v/GeneratedEndpoints)](https://www.nuget.org/packages/GeneratedEndpoints)
[![downloads](https://img.shields.io/nuget/dt/GeneratedEndpoints)](https://www.nuget.org/packages/GeneratedEndpoints)

## Setup

Add the package to any ASP.NET Core project that hosts Minimal APIs:

```bash
dotnet add package GeneratedEndpoints
```

The source generator emits two extension methods:

```csharp
using Microsoft.AspNetCore.Generated.Routing;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointHandlers();

var app = builder.Build();

app.MapEndpointHandlers();

app.Run();
```

- The `AddEndpointHandlers` method registers each endpoint class with injected services as a `Scoped` service.
- The `MapEndpointHandlers` method calls the generated `Map` method for each endpoint.

## A simple endpoint

An endpoint is wired using a class with a method marked by one of the `[Map*]` attributes. The source generator produces the handler delegate and automatically
calls the appropriate map method on the route builder.

```csharp
using Microsoft.AspNetCore.Generated.Attributes;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Contoso.Example.Endpoints;

internal static class TimeEndpoints
{
    [MapGet("/time")]
    public static Ok<string> GetCurrentTime() => TypedResults.Ok(DateTimeOffset.UtcNow.ToString("O"));
}
```

## Expanding the endpoint

You can add additional metadata to endpoints using attributes.

```csharp
using Microsoft.AspNetCore.Generated.Attributes;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Contoso.Example.Endpoints;

[Tags("Users", "Lookup")]
internal static class UserEndpoints
{
    [Summary("Gets a user by ID.")]
    [RequireAuthorization("Users.Read")]
    [MapGet("/users/{id:int}")]
    public static Results<Ok<UserDto>, NotFound> GetUser(int id)
    {
        if (id <= 0)
            return TypedResults.NotFound();

        return TypedResults.Ok(new UserDto(id, $"User {id}"));
    }
}

internal sealed record UserDto(int Id, string DisplayName);
```

* `[Tags]` on the class assigns OpenAPI tags to every endpoint inside the type.
* `[Summary]` annotates the endpoint with OpenAI summary metadata.
* `[RequireAuthorization]` enforces authorization policies on the endpoint.

## Static vs instantiable classes

To avoid clutering your endpoint method with service dependencies, you can specify them using a class constructor. This also allows you to reuse the same
service declaration across multiple endpoints. The source generator will generate the appropriate service injection code for each endpoint method.

```csharp
using Microsoft.AspNetCore.Generated.Attributes;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Contoso.Example.Endpoints;

internal static class HealthEndpoints
{
    [MapGet("/health")]
    public static Ok<string> Check(IHostEnvironment env) => TypedResults.Ok("Healthy, {env.EnvironmentName}");
}

internal sealed class EnvironmentEndpoints(IHostEnvironment env)
{
    [MapGet("/env")]
    public Ok<string> GetEnvironmentName() => TypedResults.Ok(env.EnvironmentName);
}
```

## Advanced configuration

Endpoint classes may include an optional `Configure` method to apply conventions after the handler is mapped. The source generator will look for the following
signature:

```csharp
using Microsoft.AspNetCore.Generated.Attributes;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Contoso.Example.Endpoints;

internal static class HealthEndpoints
{
    [MapGet("/health")]
    public static Ok<string> Check() => TypedResults.Ok("Healthy");

    public static void Configure<TBuilder>(TBuilder builder)
        where TBuilder : IEndpointConventionBuilder
    {
        builder.WithRequestTimeout(TimeSpan.FromSeconds(5));
    }
}
```

Inside `Configure` you can continue to chain configurations that are not already exposed as attributes by the source generator. The `Configure` method is
applied to all endpoints within the class.

## Attribute Reference

* `[Accepts(string contentType = "application/json", params string[] additionalContentTypes, RequestType = null, IsOptional = false)]`
* `[Accepts<TRequest>(string contentType = "application/json", params string[] additionalContentTypes, IsOptional = false)]`
* `[AllowAnonymous]`
* `[Description(string description)]`
* `[DisableAntiforgery]`
* `[DisableRequestTimeout]`
* `[DisableValidation]`
* `[DisplayName(string displayName)]`
* `[EndpointFilter(Type filterType)]`
* `[EndpointFilter<TFilter>]`
* `[ExcludeFromDescription]`
* `[MapConnect(string pattern = "", Name = null)]`
* `[MapDelete(string pattern = "", Name = null)]`
* `[MapFallback(string pattern = "", Name = null)]`
* `[MapGroup(string pattern, Name = null)]`
* `[MapGet(string pattern = "", Name = null)]`
* `[MapHead(string pattern = "", Name = null)]`
* `[MapOptions(string pattern = "", Name = null)]`
* `[MapPatch(string pattern = "", Name = null)]`
* `[MapPost(string pattern = "", Name = null)]`
* `[MapPut(string pattern = "", Name = null)]`
* `[MapQuery(string pattern = "", Name = null)]`
* `[MapTrace(string pattern = "", Name = null)]`
* `[Order(int order)]`
* `[ProducesProblem(int statusCode = StatusCodes.Status500InternalServerError, string? contentType = null, params string[] additionalContentTypes)]`
* `[ProducesResponse(int statusCode = StatusCodes.Status200OK, string? contentType = null, params string[] additionalContentTypes, ResponseType = null)]`
* `[ProducesResponse<TResponse>(int statusCode = StatusCodes.Status200OK, string? contentType = null, params string[] additionalContentTypes)]`
* `[ProducesValidationProblem(int statusCode = StatusCodes.Status400BadRequest, string? contentType = null, params string[] additionalContentTypes)]`
* `[RequestTimeout(string? policyName = null)]`
* `[RequireAuthorization(params string[] policies)]`
* `[RequireCors(string? policyName = null)]`
* `[RequireHost(params string[] hosts)]`
* `[RequireRateLimiting(string policyName)]`
* `[ShortCircuit]`
* `[Summary(string summary)]`
* `[Tags(params string[] tags)]`
