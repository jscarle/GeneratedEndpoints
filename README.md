[![Banner](https://raw.githubusercontent.com/jscarle/GeneratedEndpoints/main/Banner.png)](https://github.com/jscarle/GeneratedEndpoints)

# GeneratedEndpoints - Attribute-driven, source-generated minimal API endpoints for feature-based development

GeneratedEndpoints is a .NET source generator that automatically wires up Minimal API endpoints from attribute-annotated
methods. This simplifies integration of HTTP handlers within Clean Architecture (CA) or Vertical Slice Architecture (VSA)
by keeping endpoint definitions inside their features while generating the boilerplate mapping code.

[![develop](https://img.shields.io/github/actions/workflow/status/jscarle/GeneratedEndpoints/develop.yml?logo=github)](https://github.com/jscarle/GeneratedEndpoints)
[![nuget](https://img.shields.io/nuget/v/GeneratedEndpoints)](https://www.nuget.org/packages/GeneratedEndpoints)
[![downloads](https://img.shields.io/nuget/dt/GeneratedEndpoints)](https://www.nuget.org/packages/GeneratedEndpoints)

## Getting Started

### Installation

Add the package to the Minimal API project that will host your endpoints. You can install it with the .NET CLI:

```bash
dotnet add package GeneratedEndpoints
```

Or add a `<PackageReference>` to your project file:

```xml
<ItemGroup>
  <PackageReference Include="GeneratedEndpoints" Version="$(GeneratedEndpointsVersion)" />
</ItemGroup>
```

Once the package is referenced, the source generator will contribute its attributes and extension methods to the consuming project at build time.

### 1. Define a request handler

Create a feature class that encapsulates the logic for a single endpoint and decorate its handler method with one of the generated HTTP verb attributes. The attributes live in the `Microsoft.AspNetCore.Generated.Attributes` namespace and map directly to Minimal API routing methods.

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

* Use `[MapGet]`, `[MapPost]`, `[MapPut]`, `[MapDelete]`, `[MapPatch]`, `[MapHead]`, `[MapOptions]`, `[MapTrace]`, or `[MapConnect]` to describe the HTTP verb and route pattern.
* Optional `Name`, `Summary`, and `Description` named parameters populate the generated `.WithName`, `.WithSummary`, and `.WithDescription` metadata calls. When omitted, the generator derives the endpoint name from the method name (stripping a trailing `Async`).
* Apply standard ASP.NET Core parameter binding attributes (`[FromRoute]`, `[FromQuery]`, `[FromBody]`, `[FromServices]`, `[AsParameters]`, etc.). The generator mirrors them onto the produced delegate so binding behaves exactly as declared.
* Annotate the class or method with `[Tags]`, `[RequireAuthorization]`, or `[DisableAntiforgery]` to flow the corresponding Minimal API configuration.
* Non-static handler classes are automatically registered with dependency injection (as transient services), allowing you to use constructor injection for collaborators.

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

`AddEndpointHandlers` ensures any non-static handler types can be resolved from dependency injection, while `MapEndpointHandlers` generates Minimal API route mappings for every annotated method in the application.

### 3. Compose additional endpoints

Add as many handler classes as needed—each annotated method becomes an endpoint. You can mix synchronous and asynchronous methods, return `IResult` or typed `Results<>`, and share common metadata through attributes placed on either the class or individual methods. For example:

```csharp
using Microsoft.AspNetCore.Generated.Attributes;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Todos.Features;

public static class CreateTodo
{
    [MapPost("/todos", Name = "CreateTodo")]
    [DisableAntiforgery]
    public static async Task<Results<Created<Todo>, ValidationProblem>> HandleAsync(
        [FromBody] CreateTodoRequest request,
        [FromServices] TodoService service,
        CancellationToken cancellationToken)
    {
        var result = await service.CreateAsync(request, cancellationToken);
        return result.IsValid
            ? TypedResults.Created($"/todos/{result.Value.Id}", result.Value)
            : TypedResults.ValidationProblem(result.Errors);
    }
}
```

Every new handler will automatically appear in the generated routing table the next time the project builds—no manual `MapGet`, `MapPost`, or registration code is required.

