[![Banner](https://raw.githubusercontent.com/jscarle/GeneratedEndpoints/develop/Banner.png)](https://github.com/jscarle/GeneratedEndpoints)
# GeneratedEndpoints - Attribute-driven, source-generated Minimal API endpoints for feature-based development

GeneratedEndpoints is a Roslyn source generator that turns small, attribute-decorated endpoint classes into fully wired Minimal API handlers. You write cohesive endpoint classes, decorate them with attributes from `Microsoft.AspNetCore.Generated.Attributes`, and the generator creates all of the registration code, endpoint metadata, and supporting plumbing at compile time. The result is a clean feature folder structure with no runtime reflection and no hand-written routing boilerplate.

## Setup
1. Add the package to any ASP.NET Core project that hosts Minimal APIs:
   ```bash
   dotnet add package GeneratedEndpoints
   ```
2. Update your endpoint class files with the attribute namespace:
   ```csharp
   using Microsoft.AspNetCore.Generated.Attributes;
   ```
3. Ensure the application references the routing namespace emitted by the generator:
   ```csharp
   using Microsoft.AspNetCore.Generated.Routing;
   ```

## Wiring the generator into Program.cs
The generator emits two extension methods that keep your Program.cs lean:
```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointHandlers();

var app = builder.Build();
app.MapEndpointHandlers();

app.Run();
```
`AddEndpointHandlers` registers every generated request handler as a service so that constructor injection, logging, and filters are available. `MapEndpointHandlers` discovers the generated handlers, applies their metadata, and maps them onto the `IEndpointRouteBuilder` in one call.

## The most minimal endpoint
A minimal endpoint is a static class with a static method marked by one of the `[Map*]` attributes. The generator reads the attribute, produces the handler delegate, and automatically hooks the method into routing.
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
Build the project and the generator will emit the registration code behind the scenes. No manual `MapGet` call is required in Program.cs.

## Expanding the endpoint with metadata
You can gradually layer richer metadata just by adding attributes.
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
* `[Summary]` annotates the generated metadata so Swashbuckle/NSwag show friendly descriptions.
* `[RequireAuthorization]` enforces authorization policies per method without touching Program.cs.

## Static class or sealed class with constructor injection?
Static classes are perfect for pure functions that only depend on the method parameters. When you need services, switch to a sealed class and let the generator create the scope-aware instance for you.
```csharp
internal static class HealthEndpoints
{
    [MapGet("/health")]
    public static Ok<string> Check() => TypedResults.Ok("Healthy");
}

internal sealed class EnvironmentEndpoints(IHostEnvironment hostEnvironment)
{
    [MapGet("/env")]
    public Ok<string> GetEnvironmentName() => TypedResults.Ok(hostEnvironment.EnvironmentName);
}
```
Because `EnvironmentEndpoints` is sealed, the generator can new it up through dependency injection, pass in `IHostEnvironment`, and reuse the instance for the lifetime of the request.

## Configure method requirements
Endpoint classes may include an optional `Configure` method to apply conventions after the handler is mapped. The generator looks for the following signature:
```csharp
public static void Configure<TBuilder>(TBuilder builder)
    where TBuilder : IEndpointConventionBuilder
```
Inside `Configure` you can attach endpoint filters, metadata, or additional policies that are not already exposed via attributes. The method runs once per endpoint during startup, after the generator maps the handler but before the application starts listening.

## Attribute reference
* `[MapGet]`, `[MapPost]`, `[MapPut]`, `[MapPatch]`, `[MapDelete]`, `[MapOptions]`, `[MapHead]`, `[MapTrace]`, `[MapConnect]`, `[MapQuery]`, `[MapFallback]` – Constructor: `(string pattern = "", string? displayName = null)`. Optional named parameter `Name` sets the route name.
* `[Summary]` – Constructor: `(string summary)`.
* `[RequireAuthorization]` – Overloads: no parameters to require default authorization; `(params string[] policies)` to require specific policies.
* `[AllowAnonymous]` – No parameters. Available from `Microsoft.AspNetCore.Authorization`.
* `[Tags]` – Constructor: `(params string[] tags)` from `Microsoft.AspNetCore.Http`.
* `[DisableAntiforgery]` – No parameters.
* `[RequireCors]` – Constructor: `(string? policyName = null)`.
* `[RequireHost]` – Constructor: `(params string[] hosts)`.
* `[RequireRateLimiting]` – Constructor: `(string policyName)`.
* `[DisableRequestTimeout]` – No parameters.
* `[RequestTimeout]` – Constructor: `(string? policyName = null)` to apply a named timeout policy.
* `[ShortCircuit]` – No parameters.
* `[Order]` – Constructor: `(int order)`.
* `[GroupName]` – Constructor: `(string name)`.
* `[Accepts]` – Constructor: `(string contentType = "application/json", params string[] additionalContentTypes)` with optional named parameters `RequestType` and `IsOptional`. Generic version `[Accepts<TRequest>]` captures the request type.
* `[ProducesResponse]` – Constructor: `(int statusCode = 200, string? contentType = null, params string[] additionalContentTypes)` with optional named parameter `ResponseType`. Generic version `[ProducesResponse<TResponse>]` infers the response type.
* `[ProducesProblem]` – Constructor: `(int statusCode = 500, string? contentType = null, params string[] additionalContentTypes)`.
* `[ProducesValidationProblem]` – Constructor: `(int statusCode = 400, string? contentType = null, params string[] additionalContentTypes)`.
* `[EndpointFilter]` – Constructor: `(Type filterType)` or generic `[EndpointFilter<TFilter>]`.
* `[RequireCors]`, `[RequireAuthorization]`, and `[RequireRateLimiting]` support the named parameter `PolicyName` to target specific policies when applicable.
* Standard ASP.NET Core attributes `[DisplayName]`, `[Description]`, `[AllowAnonymous]`, `[RequireCors]`, `[Tags]`, and `[ExcludeFromDescription]` are also honored when applied alongside the generated attributes.
