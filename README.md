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

| Definition | Usage | Description |
| --- | --- | --- |
| `[Accepts(string contentType = "application/json", params string[] additionalContentTypes, RequestType = null, IsOptional = false)]` | Method | Declares the accepted request body CLR type, optional status, and list of content types for the handler. |
| `[Accepts<TRequest>(string contentType = "application/json", params string[] additionalContentTypes, IsOptional = false)]` | Method | Generic shortcut for specifying the request type and accepted content types for the handler. |
| `[AllowAnonymous]` | Class or Method | Allows the annotated endpoint or class to bypass authorization requirements. |
| `[Description(string description)]` | Method | Sets the OpenAPI description metadata for the generated endpoint. |
| `[DisableAntiforgery]` | Class or Method | Disables antiforgery protection for the annotated endpoint(s). |
| `[DisableRequestTimeout]` | Class or Method | Disables request timeout enforcement for the annotated endpoint(s). |
| `[DisableValidation]` | Class or Method | Disables automatic request validation (when supported) for the annotated endpoint(s). |
| `[DisplayName(string displayName)]` | Method | Overrides the endpoint display name used in diagnostics and metadata. |
| `[EndpointFilter(Type filterType)]` | Class or Method | Adds the specified endpoint filter type to the handler pipeline. |
| `[EndpointFilter<TFilter>]` | Class or Method | Generic form for registering an endpoint filter type on the handler pipeline. |
| `[ExcludeFromDescription]` | Class or Method | Hides the endpoint or class from generated API descriptions (e.g., OpenAPI). |
| `[MapConnect(string pattern = "", Name = null)]` | Method | Marks the method as an HTTP CONNECT endpoint using the supplied route pattern and optional name. |
| `[MapDelete(string pattern = "", Name = null)]` | Method | Marks the method as an HTTP DELETE endpoint using the supplied route pattern and optional name. |
| `[MapFallback(string pattern = "", Name = null)]` | Method | Maps the method as the fallback endpoint invoked when no other route matches. |
| `[MapGroup(string pattern, Name = null)]` | Class | Assigns a route group pattern and optional endpoint group name to every handler in the class. |
| `[MapGet(string pattern = "", Name = null)]` | Method | Marks the method as an HTTP GET endpoint using the supplied route pattern and optional name. |
| `[MapHead(string pattern = "", Name = null)]` | Method | Marks the method as an HTTP HEAD endpoint using the supplied route pattern and optional name. |
| `[MapOptions(string pattern = "", Name = null)]` | Method | Marks the method as an HTTP OPTIONS endpoint using the supplied route pattern and optional name. |
| `[MapPatch(string pattern = "", Name = null)]` | Method | Marks the method as an HTTP PATCH endpoint using the supplied route pattern and optional name. |
| `[MapPost(string pattern = "", Name = null)]` | Method | Marks the method as an HTTP POST endpoint using the supplied route pattern and optional name. |
| `[MapPut(string pattern = "", Name = null)]` | Method | Marks the method as an HTTP PUT endpoint using the supplied route pattern and optional name. |
| `[MapQuery(string pattern = "", Name = null)]` | Method | Marks the method as an HTTP QUERY endpoint using the supplied route pattern and optional name. |
| `[MapTrace(string pattern = "", Name = null)]` | Method | Marks the method as an HTTP TRACE endpoint using the supplied route pattern and optional name. |
| `[Order(int order)]` | Method | Controls the order in which endpoint conventions are applied to the handler. |
| `[ProducesProblem(int statusCode = StatusCodes.Status500InternalServerError, string? contentType = null, params string[] additionalContentTypes)]` | Method | Declares that the endpoint emits a problem details payload for the given status code and content types. |
| `[ProducesResponse(int statusCode = StatusCodes.Status200OK, string? contentType = null, params string[] additionalContentTypes, ResponseType = null)]` | Method | Declares response metadata for the handler, including status code, optional CLR type, and content types. |
| `[ProducesResponse<TResponse>(int statusCode = StatusCodes.Status200OK, string? contentType = null, params string[] additionalContentTypes)]` | Method | Generic shorthand for declaring the CLR response type along with status code and content types. |
| `[ProducesValidationProblem(int statusCode = StatusCodes.Status400BadRequest, string? contentType = null, params string[] additionalContentTypes)]` | Method | Declares that the endpoint returns validation problem details for the specified status code and content types. |
| `[RequestTimeout(string? policyName = null)]` | Class or Method | Applies the default or a named request-timeout policy to the handler(s). |
| `[RequireAuthorization(params string[] policies)]` | Class or Method | Enforces authorization on the handler(s), optionally scoping access to specific policies. |
| `[RequireCors(string? policyName = null)]` | Class or Method | Requires the default or a named CORS policy for the annotated handler(s). |
| `[RequireHost(params string[] hosts)]` | Class or Method | Restricts the handler(s) to the specified allowed hostnames. |
| `[RequireRateLimiting(string policyName)]` | Class or Method | Enforces the named rate-limiting policy on the annotated handler(s). |
| `[ShortCircuit]` | Class or Method | Marks the handler(s) to short-circuit the request pipeline when invoked. |
| `[Summary(string summary)]` | Class or Method | Sets the summary metadata applied to the generated endpoint(s). |
| `[Tags(params string[] tags)]` | Class or Method | Assigns OpenAPI tags to the annotated handler(s) for grouping in API docs. |
