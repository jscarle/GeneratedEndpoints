[![Banner](https://raw.githubusercontent.com/jscarle/GeneratedEndpoints/develop/Banner.png)](https://github.com/jscarle/GeneratedEndpoints)

# GeneratedEndpoints - Attribute-driven, source-generated minimal API endpoints for feature-based development

GeneratedEndpoints is a .NET source generator that automatically wires up Minimal API endpoints from attribute-annotated
methods. This simplifies integration of HTTP handlers within Clean Architecture (CA) or Vertical Slice Architecture (VSA)
by keeping endpoint definitions inside their features while generating the boilerplate mapping code.

## Capabilities at a glance

* **Attribute-driven routing** – decorate a method with `[MapGet]`, `[MapPost]`, etc. (including OPTIONS, HEAD, TRACE, CONNECT,
  and the Minimal API-specific `QUERY` verb) and the generator maps it automatically.
* **Static or instance handlers** – declare handlers in static classes or as transient services that participate in dependency
  injection.
* **Metadata composition** – mix class-level and method-level attributes for tags, authorization requirements, content
  negotiation, and antiforgery/anonymous settings. The generator merges everything into the produced endpoint builder.
* **Rich request/response contracts** – describe the shape of your API surface with `[Accepts]`, `[Produces]`, `[ProducesProblem]`,
  and `[ProducesValidationProblem]` so OpenAPI and client tooling stay accurate.
* **Minimal boilerplate** – `AddEndpointHandlers` auto-registers instance handlers with DI, and `MapEndpointHandlers`
  registers every attribute-decorated method.
* **Optional per-feature customization** – provide a `Configure` method in your feature to add filters, OpenAPI metadata, or any
  other conventions using the generated `IEndpointConventionBuilder`.

[![develop](https://img.shields.io/github/actions/workflow/status/jscarle/GeneratedEndpoints/develop.yml?logo=github)](https://github.com/jscarle/GeneratedEndpoints)
[![nuget](https://img.shields.io/nuget/v/GeneratedEndpoints)](https://www.nuget.org/packages/GeneratedEndpoints)
[![downloads](https://img.shields.io/nuget/dt/GeneratedEndpoints)](https://www.nuget.org/packages/GeneratedEndpoints)

## Getting Started

### Installation

Add the package to the Minimal API project that will host your endpoints. You can install it with the .NET CLI:

```bash
dotnet add package GeneratedEndpoints
```

Once the package is referenced, the source generator will contribute its attributes and extension methods to the consuming project at build time.

### 1. Define a request handler

Create a feature class that encapsulates the logic for a single endpoint and decorate its handler method with one of the generated HTTP verb attributes. The attributes live in the `Microsoft.AspNetCore.Generated.Attributes` namespace and map directly to Minimal API routing methods.

Handler classes can be expressed in whichever style best fits the feature:

* **Instance classes** (non-static) allow constructor injection and can expose either instance or static handler methods. When an annotated method is not static the generator will call it on a resolved instance from dependency injection.
* **Static classes** make it easy to group stateless functionality. Every annotated method inside must also be static, mirroring standard C# rules.

#### Instance handler example

```csharp
using Microsoft.AspNetCore.Generated.Attributes;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Todos.Features;

public sealed class GetTodo
{
    private readonly TodoDbContext _db;

    public GetTodo(TodoDbContext db) => _db = db;

    [MapGet("/todos/{id}", Summary = "Retrieve a todo", Description = "Returns the todo matching the provided identifier.")]
    [Tags("Todos")]
    [RequireAuthorization("Todos.Read")]
    public async Task<Results<Ok<Todo>, NotFound>> HandleAsync(Guid id, CancellationToken cancellationToken)
    {
        var entity = await _db.Todos.FindAsync(new object?[] { id }, cancellationToken);
        return entity is null ? TypedResults.NotFound() : TypedResults.Ok(entity);
    }
}
```

Key points:

* Use `[MapGet]`, `[MapPost]`, `[MapPut]`, `[MapDelete]`, `[MapPatch]`, `[MapHead]`, `[MapOptions]`, `[MapQuery]`, `[MapTrace]`, or `[MapConnect]` to describe the HTTP verb and route pattern.
* Optional `Name`, `Summary`, and `Description` named parameters populate the generated `.WithName`, `.WithSummary`, and `.WithDescription` metadata calls. When omitted, the generator derives the endpoint name from the method name (stripping a trailing `Async`).
* Apply standard ASP.NET Core parameter binding attributes (`[FromRoute]`, `[FromQuery]`, `[FromBody]`, `[FromServices]`, `[FromKeyedServices]`, `[AsParameters]`, etc.). The generator mirrors them onto the produced delegate so binding behaves exactly as declared.
* Annotate the **class**, an individual **method**, or both with `[Tags]`, `[RequireAuthorization]`, `[DisableAntiforgery]`, or `[AllowAnonymous]`. Class-level metadata is merged onto every generated endpoint, while method-level attributes can refine or augment the settings for a specific handler. `[AllowAnonymous]` lets a method opt out of authorization even if the enclosing class (or other conventions) require authenticated access.
* Non-static handler classes are automatically registered with dependency injection (as transient services). Their instance methods receive a scoped instance resolved from DI, while static methods continue to behave like any other static helper.

#### Static handler example

The same attribute-driven approach works for static handler types when no dependencies are needed:

```csharp
using Microsoft.AspNetCore.Generated.Attributes;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Todos.Features;

public static class ListTodos
{
    [MapGet("/todos")]
    [Tags("Todos")]
    public static Ok<IReadOnlyList<Todo>> Handle()
        => TypedResults.Ok(TodoStore.All);
}
```

### 2. Wire up the application

The generator emits extension methods in the `Microsoft.AspNetCore.Generated.Routing` namespace. Call them during startup to register handler types and map the generated endpoints.

```csharp
using Microsoft.AspNetCore.Generated.Routing;

var builder = WebApplication.CreateBuilder(args);

// Registers non-static handler classes with the DI container.
builder.Services.AddEndpointHandlers();

var app = builder.Build();

// Maps every method decorated with a Map* attribute.
app.MapEndpointHandlers();

app.Run();
```

`AddEndpointHandlers` ensures any non-static handler types can be resolved from dependency injection, while `MapEndpointHandlers` generates Minimal API route mappings for every annotated method in the application. Both extension methods reside in the `Microsoft.AspNetCore.Generated.Routing` namespace.

### 3. Compose additional endpoints

Add as many handler classes as needed—each annotated method becomes an endpoint. You can mix synchronous and asynchronous methods, return `IResult` or typed `Results<>`, and combine static and instance handlers in the same project. Metadata from attributes composes naturally: class-level attributes are applied to every endpoint, while method-level attributes add to (or override, when relevant) the defaults.

```csharp
using Microsoft.AspNetCore.Generated.Attributes;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Todos.Features;

[Tags("Todos")]
[RequireAuthorization("Todos.Read")]
public sealed class TodoEndpoints
{
    private readonly TodoDbContext _db;

    public TodoEndpoints(TodoDbContext db) => _db = db;

    [MapGet("/todos/{id}", Summary = "Retrieve a todo")]
    public async Task<Results<Ok<Todo>, NotFound>> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        var entity = await _db.Todos.FindAsync(new object?[] { id }, cancellationToken);
        return entity is null ? TypedResults.NotFound() : TypedResults.Ok(entity);
    }

    [MapDelete("/todos/{id}")]
    [RequireAuthorization("Todos.Write")]
    public static async Task<Results<NoContent, NotFound>> DeleteAsync(
        Guid id,
        [FromServices] TodoDbContext db,
        CancellationToken cancellationToken)
    {
        var entity = await db.Todos.FindAsync(new object?[] { id }, cancellationToken);
        if (entity is null)
            return TypedResults.NotFound();

        db.Todos.Remove(entity);
        await db.SaveChangesAsync(cancellationToken);
        return TypedResults.NoContent();
    }
}
```

In this example:

* The class-level `[Tags]` and `[RequireAuthorization]` attributes apply to both endpoints, while the method-level `[RequireAuthorization]` adds an additional policy for the delete handler.
* `GetAsync` is an instance method that uses the injected `TodoDbContext` field, illustrating how non-static handlers can maintain state.
* `DeleteAsync` is a static method in the same class and explicitly receives its dependencies via `[FromServices]`, demonstrating that you can mix static and instance methods in a single handler type.

Every new handler will automatically appear in the generated routing table the next time the project builds—no manual `MapGet`, `MapPost`, or registration code is required.

### 4. Customize generated endpoints with `Configure`

Some scenarios require direct access to the `IEndpointConventionBuilder` that Minimal APIs use when configuring an endpoint—for example, adding endpoint filters, OpenAPI metadata, or other advanced conventions. Handler classes can now opt-in to that level of control by providing a static `Configure` method with the following signature:

```csharp
public static void Configure<TBuilder>(TBuilder builder)
    where TBuilder : IEndpointConventionBuilder

// or, when you need to resolve services while configuring
public static void Configure<TBuilder>(TBuilder builder, IServiceProvider serviceProvider)
    where TBuilder : IEndpointConventionBuilder
```

When the generator detects this method on a handler class it will automatically wrap every mapped endpoint from the class in a `.Configure(...)` call that invokes your method. You can use the provided builder to apply conventions that are difficult or impossible to express via attributes alone.

If you include the optional `IServiceProvider` parameter, the generator passes the `IEndpointRouteBuilder.ServiceProvider` from the `MapEndpointHandlers` call. This makes it easy to resolve scoped services or other helpers needed to configure filters, OpenAPI metadata, or custom conventions without manually re-wiring the app's service provider.

```csharp
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Generated.Attributes;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Todos.Features;

public sealed class CreateTodo
{
    [MapPost("/todos")]
    public static Ok<Todo> Handle([FromBody] Todo todo) => TypedResults.Ok(todo);

    public static void Configure<TBuilder>(TBuilder builder)
        where TBuilder : IEndpointConventionBuilder
    {
        builder.AddEndpointFilter(new TimingFilter());
        builder.WithOpenApi(operation =>
        {
            operation.Summary = "Creates a todo";
            return operation;
        });
    }
}
```

The method is only generated once per handler class, so any conventions you add will automatically flow to all endpoints defined within that class.

### 5. Describe contracts with `Accepts` and `Produces`

GeneratedEndpoints ships with helper attributes for request and response metadata. Apply them to either a handler class or
individual methods to keep your OpenAPI description in sync with the implementation. Attributes on the class are merged into
each method, while method-level attributes can augment or override the defaults.

```csharp
using Microsoft.AspNetCore.Generated.Attributes;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Todos.Features;

[Accepts<CreateTodoRequest>("application/json", "application/xml")]
[Produces<Todo>(StatusCodes.Status201Created)]
[ProducesProblem(StatusCodes.Status500InternalServerError)]
public sealed class CreateTodo
{
    [MapPost("/todos", Summary = "Create a todo")]
    [ProducesValidationProblem(StatusCodes.Status400BadRequest)]
    public static Created<Todo> Handle([FromBody] CreateTodoRequest request)
        => TypedResults.Created($"/todos/{request.Id}", request.ToTodo());
}
```

The generator translates these attributes into `.Accepts`, `.Produces`, `.ProducesProblem`, and `.ProducesValidationProblem`
calls on the endpoint builder.

### Attribute reference

| Attribute | Scope | Purpose |
| --- | --- | --- |
| `[MapGet]`, `[MapPost]`, `[MapPut]`, `[MapDelete]`, `[MapPatch]`, `[MapHead]`, `[MapOptions]`, `[MapTrace]`, `[MapConnect]`, `[MapQuery]` | Method | Declares an endpoint and its route pattern. Named arguments (`Name`, `Summary`, `Description`) fill the generated `.WithName`, `.WithSummary`, and `.WithDescription` calls. |
| `[Tags]` | Class or method | Adds tags to one or more endpoints. Multiple attributes merge without duplication. |
| `[RequireAuthorization]` | Class or method | Requires authorization for the endpoint. Pass an array of policy names to enforce specific policies; when omitted the standard authorization middleware is applied. |
| `[AllowAnonymous]` | Class or method | Explicitly opts a method (or all methods in a class) into anonymous access, overriding `[RequireAuthorization]`. |
| `[DisableAntiforgery]` | Class or method | Calls `.DisableAntiforgery()` on the generated endpoint, matching the ASP.NET Core extension. |
| `[Accepts]` / `[Accepts<TRequest>]` | Class or method | Emits `.Accepts<TRequest>(contentType, additionalContentTypes...)` to document supported request bodies. Multiple attributes are allowed per endpoint. |
| `[Produces]` / `[Produces<TResponse>]` | Class or method | Emits `.Produces<TResponse>(statusCode, contentTypes...)` for each documented response type. Multiple attributes are allowed. |
| `[ProducesProblem]` | Class or method | Emits `.ProducesProblem(statusCode, contentTypes...)` for endpoints that return RFC 7807 problem details. |
| `[ProducesValidationProblem]` | Class or method | Emits `.ProducesValidationProblem(statusCode, contentTypes...)` when validation failures are returned. |

> ℹ️ All metadata attributes defined on a class are applied to every annotated method inside the class. Method-level attributes
> can add additional entries (for tags, accepts, produces, etc.) or override booleans such as `[AllowAnonymous]` and
> `[DisableAntiforgery]`.

### Authorization and security conventions

* **Default authorization** – `[RequireAuthorization]` adds `.RequireAuthorization()` to every endpoint. Supplying policies
  (`[RequireAuthorization("Todos.Read", "Todos.Write")]`) generates `.RequireAuthorization("Todos.Read", "Todos.Write")`.
* **Allow anonymous** – `[AllowAnonymous]` on a class or method maps to `.AllowAnonymous()`, even when authorization is required elsewhere.
* **Antiforgery** – `[DisableAntiforgery]` wires through `.DisableAntiforgery()`.

### Tips for handler classes

* Non-static handler classes are automatically registered as transient services when you call `builder.Services.AddEndpointHandlers();`.
* You can mix static and instance methods within the same class. Instance methods receive the injected handler instance; static methods work like regular static helpers and can continue to rely on `[FromServices]` for dependencies.
* Use the optional `Configure` method for per-feature conventions. Its optional `IServiceProvider` parameter lets you resolve scoped services when adding endpoint filters or other runtime configuration.

