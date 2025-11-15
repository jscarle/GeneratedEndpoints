[![Banner](https://raw.githubusercontent.com/jscarle/GeneratedEndpoints/develop/Banner.png)](https://github.com/jscarle/GeneratedEndpoints)

# GeneratedEndpoints

Attribute-driven, source-generated Minimal API endpoints for feature-based development.

GeneratedEndpoints is a .NET source generator that automatically wires Minimal API endpoints from attribute-decorated methods. You describe the intent of each endpoint through attributes, and the generator produces all the routing and boilerplate needed to expose it. The result is a clean, feature-centric project structure that fits Clean Architecture (CA), Vertical Slice Architecture (VSA), or any other modular approach.

---

## Table of contents

1. [Why GeneratedEndpoints?](#why-generatedendpoints)
2. [Quick start](#quick-start)
3. [Handler styles and routing](#handler-styles-and-routing)
4. [Configuring endpoints](#configuring-endpoints)
5. [Describing requests and responses](#describing-requests-and-responses)
6. [Attribute reference](#attribute-reference)
7. [Tips, patterns, and extra examples](#tips-patterns-and-extra-examples)

---

## Why GeneratedEndpoints?

GeneratedEndpoints focuses on three goals:

* **Attribute-driven routing** – use `[MapGet]`, `[MapPost]`, `[MapDelete]`, `[MapOptions]`, `[MapHead]`, `[MapPatch]`, `[MapTrace]`, `[MapConnect]`, and even `[MapQuery]` to describe the verb and route pattern. The generator creates the matching `Map*` call and wires up metadata like `.WithName`, `.WithSummary`, and `.WithDescription`.
* **Feature-first organization** – keep handlers close to the code they execute (for example, alongside your `Todos` feature). Non-static handler classes are automatically registered with dependency injection so you can inject EF Core DbContexts, services, etc.
* **Metadata composition** – decorate classes and methods with `[Tags]`, `[RequireAuthorization]`, `[DisableAntiforgery]`, `[AllowAnonymous]`, and `[ExcludeFromDescription]`. Apply `[Accepts]`, `[ProducesResponse]`, `[ProducesProblem]`, and `[ProducesValidationProblem]` directly to the methods they describe. Class-level metadata is merged into every method, while method-level metadata can refine or override.

The generator also emits the `AddEndpointHandlers` and `MapEndpointHandlers` extension methods that do all the registration work for you.

---

## Quick start

The fastest way to understand GeneratedEndpoints is to build a minimal feature end-to-end.

### 1. Install the package

Add the package to the Minimal API project that will host your endpoints:

```bash
dotnet add package GeneratedEndpoints
```

After the reference is added, the source generator contributes its attributes and routing extensions to the consuming project at build time.

### 2. Create a handler class

Handlers can be static or instance classes. The following example uses a scoped handler so that EF Core can be injected through the constructor:

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

Key ideas:

* Choose the attribute that matches the verb (`[MapGet]`, `[MapPost]`, `[MapPut]`, `[MapDelete]`, `[MapPatch]`, `[MapHead]`, `[MapOptions]`, `[MapTrace]`, `[MapConnect]`, `[MapQuery]`).
* Named arguments like `Summary`, `Description`, and `Name` are translated into `.WithSummary`, `.WithDescription`, and `.WithName` calls.
* Use existing ASP.NET Core binding attributes (`[FromRoute]`, `[FromQuery]`, `[FromBody]`, `[FromHeader]`, `[FromServices]`, `[FromKeyedServices]`, `[AsParameters]`, etc.). The generator preserves them in the produced delegate.
* Metadata attributes (`[Tags]`, `[RequireAuthorization]`, `[AllowAnonymous]`, `[DisableAntiforgery]`, `[ExcludeFromDescription]`) can be placed on the class, on a method, or on both. Class-level metadata is merged with method-level metadata. Request/response attributes (`[Accepts]`, `[ProducesResponse]`, `[ProducesProblem]`, `[ProducesValidationProblem]`) must be applied directly to the method they describe.

### 3. Register handlers and map endpoints

The generator emits two extension methods in `Microsoft.AspNetCore.Generated.Routing`. Call them during startup:

```csharp
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Generated.Routing;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointHandlers(); // registers every non-static handler as scoped

var app = builder.Build();

app.MapEndpointHandlers(); // emits MapGet/MapPost/etc. calls for every decorated method

app.Run();
```

`AddEndpointHandlers` calls `TryAddScoped<THandler>()` for every non-static handler class. Static handler classes are skipped because they never need DI. `MapEndpointHandlers` iterates over those handler types, maps each annotated method, and returns the `IEndpointRouteBuilder` for further chaining.

### 4. Add more endpoints

Every attribute-decorated method becomes an endpoint the next time the project builds. Mix synchronous and asynchronous methods, return `IResult` or typed `Results<>`, and combine static and instance handlers in the same class. Metadata from attributes composes naturally.

### 5. Run and verify

Build the project (`dotnet build`) or run the app (`dotnet run`)—the generator will emit all routing code, DI registrations, and metadata without writing any manual `app.MapGet` or `app.MapPost` calls.

---

## Handler styles and routing

GeneratedEndpoints supports multiple handler styles so you can pick the one that matches the feature you're building.

### Instance handlers (constructor injection)

```csharp
public sealed class TodoEndpoints
{
    private readonly TodoDbContext _db;

    public TodoEndpoints(TodoDbContext db) => _db = db;

    [MapGet("/todos/{id}")]
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

* Non-static handler classes are registered as scoped services when you call `AddEndpointHandlers`.
* Instance methods are invoked on an injected instance.
* Static methods in the same class still work—they simply receive dependencies via `[FromServices]`.

### Static handlers (stateless logic)

```csharp
public static class ListTodos
{
    [MapGet("/todos")]
    [Tags("Todos")]
    public static Ok<IReadOnlyList<Todo>> Handle()
        => TypedResults.Ok(TodoStore.All);
}
```

Static handlers excel when no services are required. Every annotated method inside the class must also be static, just like regular C# rules.

### Feature layout example

A feature folder might look like this:

```
Todos/
  Features/
    GetTodo.cs
    ListTodos.cs
    CreateTodo.cs
```

Each file contains exactly one handler class with attribute-decorated methods. `MapEndpointHandlers` discovers them automatically. Because everything lives next to the feature, developers can reason about behavior without scanning `Program.cs` or `Startup.cs`.

---

## Configuring endpoints

Some scenarios require more control over each endpoint's `IEndpointConventionBuilder`. The generator supports per-feature configuration through an optional static `Configure` method.

```csharp
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

You can also request an `IServiceProvider` when configuration depends on registered services:

```csharp
public static void Configure<TBuilder>(TBuilder builder, IServiceProvider services)
    where TBuilder : IEndpointConventionBuilder
{
    var conventions = services.GetRequiredService<TodoOpenApiConventions>();
    conventions.Apply(builder);
}
```

The `Configure` method is emitted once per handler class, so every endpoint in the class receives the same conventions.

---

## Describing requests and responses

GeneratedEndpoints ships with attribute helpers for request/response metadata. They keep your OpenAPI description in sync with the implementation.

```csharp
public sealed class CreateTodo
{
    [MapPost("/todos", Summary = "Create a todo")]
    [Accepts<CreateTodoRequest>("application/json", "application/xml")]
    [ProducesResponse<Todo>(StatusCodes.Status201Created)]
    [ProducesProblem(StatusCodes.Status500InternalServerError)]
    [ProducesValidationProblem(StatusCodes.Status400BadRequest)]
    public static Created<Todo> Handle([FromBody] CreateTodoRequest request)
        => TypedResults.Created($"/todos/{request.Id}", request.ToTodo());
}
```

* Use the generic form when the request/response type is known at compile time. For runtime types, set `RequestType` on `[Accepts]` and `ResponseType` on `[ProducesResponse]`.
* Mark `IsOptional = true` on `[Accepts]` to call `.Accepts(..., isOptional: true)`.
* Multiple `[Accepts]`, `[ProducesResponse]`, `[ProducesProblem]`, and `[ProducesValidationProblem]` attributes can be applied to the same method. The generator creates every corresponding `.Accepts` or `.Produces` call.

---

## Attribute reference

| Attribute | Scope | Purpose |
| --- | --- | --- |
| `[MapGet]`, `[MapPost]`, `[MapPut]`, `[MapDelete]`, `[MapPatch]`, `[MapHead]`, `[MapOptions]`, `[MapTrace]`, `[MapConnect]`, `[MapQuery]` | Method | Declares an endpoint and its route pattern. Named arguments fill the generated `.WithName`, `.WithSummary`, and `.WithDescription` calls. |
| `[Tags]` | Class or method | Adds tags to one or more endpoints. Multiple attributes merge without duplication. |
| `[RequireAuthorization]` | Class or method | Requires authorization for the endpoint. Passing policies (`[RequireAuthorization("Todos.Read", "Todos.Write")]`) emits `.RequireAuthorization("Todos.Read", "Todos.Write")`. |
| `[AllowAnonymous]` | Class or method | Explicitly opts an endpoint into anonymous access, overriding `[RequireAuthorization]`. |
| `[DisableAntiforgery]` | Class or method | Calls `.DisableAntiforgery()` on the generated endpoint. |
| `[ExcludeFromDescription]` | Class or method | Generates `.ExcludeFromDescription()` so the endpoint is hidden from OpenAPI/metadata. |
| `[Accepts]` / `[Accepts<TRequest>]` | Method | Emits `.Accepts<TRequest>(contentTypes..., isOptional: true|false)` to document supported request bodies. Multiple attributes allowed. |
| `[ProducesResponse]` / `[ProducesResponse<TResponse>]` | Method | Emits `.Produces<TResponse>(statusCode, contentTypes...)` for each documented response type. |
| `[ProducesProblem]` | Method | Emits `.ProducesProblem(statusCode, contentTypes...)` for endpoints that return RFC 7807 problem details. |
| `[ProducesValidationProblem]` | Method | Emits `.ProducesValidationProblem(statusCode, contentTypes...)`. |

> ℹ️ Metadata defined on a class is applied to every annotated method inside the class. Method-level attributes can add entries (tags, accepts, produces, etc.) or override boolean flags like `[AllowAnonymous]`.

---

## Tips, patterns, and extra examples

### Authorization and security

* `[RequireAuthorization]` adds `.RequireAuthorization()` or `.RequireAuthorization("policy")`.
* `[AllowAnonymous]` opt-in overrides class or global authorization requirements.
* `[DisableAntiforgery]` wires `.DisableAntiforgery()` for CSRF-sensitive endpoints.

### Handling query objects with `[AsParameters]`

```csharp
public sealed record ListTodosQuery([FromQuery] int? Page, [FromQuery] string? Owner);

public static class SearchTodos
{
    [MapGet("/todos/search")]
    public static Ok<IReadOnlyList<Todo>> Handle([AsParameters] ListTodosQuery query)
    {
        var todos = TodoStore.Query(query.Page ?? 1, query.Owner);
        return TypedResults.Ok(todos);
    }
}
```

`[AsParameters]` lets you bundle multiple inputs into a record or class without writing manual binding logic.

### Combining filters with `Configure`

```csharp
public sealed class UpdateTodo
{
    [MapPut("/todos/{id}")]
    [RequireAuthorization("Todos.Write")]
    public async Task<Results<Ok<Todo>, NotFound>> HandleAsync(Guid id, [FromBody] UpdateTodoRequest request)
    {
        // ...
    }

    public static void Configure<TBuilder>(TBuilder builder, IServiceProvider services)
        where TBuilder : IEndpointConventionBuilder
    {
        builder.AddEndpointFilter(new ValidationFilter());
        builder.AddEndpointFilterFactory((context, next) => new LoggingFilter(next, services.GetRequiredService<ILoggerFactory>()));
    }
}
```

### Feature testing helper

When testing, register handlers in a WebApplicationFactory or the new `MinimalApiApplicationBuilder`:

```csharp
var app = MinimalApiApplication.CreateBuilder().Build();
app.MapEndpointHandlers();
```

All annotated methods become endpoints even in test hosts, so integration tests hit the same generated routing table as production.

### Putting it all together

```csharp
[Tags("Todos")]
[RequireAuthorization("Todos.Read")]
public sealed class TodoFeature
{
    private readonly TodoDbContext _db;

    public TodoFeature(TodoDbContext db) => _db = db;

    [MapGet("/todos/{id}", Summary = "Retrieve a todo")]
    public async Task<Results<Ok<Todo>, NotFound>> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        var entity = await _db.Todos.FindAsync(new object?[] { id }, cancellationToken);
        return entity is null ? TypedResults.NotFound() : TypedResults.Ok(entity);
    }

    [MapPost("/todos", Summary = "Create a todo")]
    [ProducesResponse<Todo>(StatusCodes.Status201Created)]
    public async Task<Created<Todo>> CreateAsync([FromBody] CreateTodoRequest request, CancellationToken cancellationToken)
    {
        var todo = request.ToTodo();
        _db.Todos.Add(todo);
        await _db.SaveChangesAsync(cancellationToken);
        return TypedResults.Created($"/todos/{todo.Id}", todo);
    }

    public static void Configure<TBuilder>(TBuilder builder, IServiceProvider services)
        where TBuilder : IEndpointConventionBuilder
    {
        var conventions = services.GetRequiredService<TodoOpenApiConventions>();
        conventions.Apply(builder);
    }
}
```

With these patterns you can grow a feature-based API without ever touching the routing layer manually. Drop a new handler class into your feature folder, decorate methods with the correct attributes, and let GeneratedEndpoints handle the plumbing.
