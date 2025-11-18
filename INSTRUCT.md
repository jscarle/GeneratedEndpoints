# GeneratedEndpoints Minimal API Implementation Guide

## 1. Package integration
- Install the `GeneratedEndpoints` NuGet package in the ASP.NET Core project that hosts Minimal APIs, then add `using Microsoft.AspNetCore.Generated.Routing;` to `Program.cs`. Call `builder.Services.AddEndpointHandlers();` before building the app and `app.MapEndpointHandlers();` after building so the generated extension methods can register services and map the discovered handlers. The generator outputs both extension methods automatically, so no manual implementation is necessary.
- `AddEndpointHandlers` is emitted inside `Microsoft.AspNetCore.Generated.Routing.EndpointServicesExtensions` and registers every non-static endpoint class as a `Scoped` service via `TryAddScoped<T>()`, ensuring constructor-injected dependencies are available even when multiple endpoints share a class instance.
- `MapEndpointHandlers` is emitted inside `Microsoft.AspNetCore.Generated.Routing.EndpointRouteBuilderExtensions`. It returns the `IEndpointRouteBuilder`, creates any `[MapGroup]` route groups, and maps each handler method with the metadata assembled by the generator so the call can be chained during application startup.

## 2. How handlers are discovered
- The generator analyzes every class method that is decorated with one of the provided `[Map*]` attributes (GET/POST/PUT/PATCH/DELETE/HEAD/OPTIONS/TRACE/CONNECT/QUERY/FALLBACK). These attributes live in `Microsoft.AspNetCore.Generated.Attributes` and are injected into the compilation during generator initialization, so the consuming project only needs to add the `using` directive. `[MapFallback]` routes get special handling so they call `MapFallback` (optionally with a pattern) instead of a verb-specific API.
- Only members of `class` types participate; interfaces, records, and structs are ignored because the generator explicitly requires the containing type to be a class before producing a handler descriptor.
- Handler methods can be `static` or instance. Static methods are mapped directly, while instance methods are wrapped in a generated lambda that resolves the handler via `[FromServices]` and forwards the request parameters, preserving your method signatures exactly.
- Endpoint names default to the method name with any `Async` suffix removed, but the `Name` named argument on the `[Map*]` attributes overrides it. When two endpoints end up with the same name, the generator rewrites those names to `Full.Type.Name.Method` to avoid registration collisions.

## 3. Handler class patterns
- Use static classes when the handler does not need constructor injection. Use instantiable classes (including primary constructors) when you want to inject services once per request; `AddEndpointHandlers` will register the class as scoped so Minimal APIs can resolve it.
- Keep handler methods in the feature or slice that owns the behavior. The generator groups handlers by containing class, which enables class-wide attributes and configuration to cascade to every method.
- Optionally define a static `Configure<TBuilder>(TBuilder builder)` method on the handler class (with `TBuilder` constrained to `IEndpointConventionBuilder`). A second `IServiceProvider` parameter is allowed. When present, the generator wraps every mapped endpoint in a call to `Configure`, letting you apply custom conventions or DI-driven setup across the class.

## 4. Parameters and dependency injection
- All method parameters are preserved in the generated delegate. The generator inspects parameter attributes to decide how to annotate them, automatically applying `[FromRoute]`, `[FromQuery]`, `[FromHeader]`, `[FromBody]`, `[FromForm]`, `[FromServices]`, `[FromKeyedServices]`, or `[AsParameters]` (including custom binding names and keyed service values) when you decorate the parameter accordingly.
- Instance handlers receive `([FromServices] HandlerClass handler, ...) => handler.Method(...)` delegates, so constructor-injected members remain available without manually wiring DI in each endpoint.
- Use standard Minimal API types for return values (`IResult`, typed results, `Results<T1,T2>`, DTOs, etc.). The generator simply passes through your method’s return type to ASP.NET Core’s routing infrastructure.

## 5. Attribute-driven endpoint metadata
- Apply class-level attributes to broadcast settings to all endpoints in the class, and decorate methods to override or augment them. The generator merges the two configurations, giving method-level directives precedence for authorization, request timeout, CORS, and rate limiting while combining tags, filters, and metadata arrays.
- Supported metadata attributes (apply to class and/or method) include:
  - Visibility & documentation: `[DisplayName]`, `[Summary]`, `[Description]`, `[Tags]`, `[ExcludeFromDescription]` control `.WithDisplayName`, `.WithSummary`, `.WithDescription`, `.WithTags`, and `.ExcludeFromDescription()` calls on the generated endpoint builder.
  - Contracts: `[Accepts]`, `[ProducesResponse]`, `[ProducesProblem]`, `[ProducesValidationProblem]` expand to `.Accepts<T>()` and `.Produces*()` builder calls, including optional status codes, content types, and `IsOptional` flags.
  - Security & networking: `[RequireAuthorization]`, `[AllowAnonymous]`, `[RequireCors]`, `[RequireHost]`, `[RequireRateLimiting]`, `[RequestTimeout]`, `[DisableRequestTimeout]`, `[DisableAntiforgery]`, `[DisableValidation]`, `[ShortCircuit]`, `[RequireCors]`, `[RequireRateLimiting]`, `[RequireHost]`, and `[RequireRateLimiting]` translate into the corresponding builder methods. Authorization directives obey the precedence rules in `ResolveAuthorization`, so method-level attributes override class-level ones. Request-timeout and CORS/rate-limiting policies behave similarly through their resolver helpers.
  - Pipeline customization: `[EndpointFilter]` / `[EndpointFilter<T>]` register filters exactly once per type, `[DisableAntiforgery]`, `[DisableValidation]`, `[ShortCircuit]`, and `[Order]` become `.AddEndpointFilter<T>()`, `.DisableAntiforgery()`, `.DisableValidation()`, `.ShortCircuit()`, and `.WithOrder()` respectively.
- Use `[MapGroup("/pattern", Name = "GroupName")]` on the class to generate a route group (`builder.MapGroup(pattern)`), optional group names (`.WithGroupName("...")`), and a reusable builder identifier shared by every handler inside that class.

## 6. Configure method usage
- Implement `public static void Configure<TBuilder>(TBuilder builder)` (or `public static void Configure<TBuilder>(TBuilder builder, IServiceProvider sp)`) where `TBuilder` is constrained to `IEndpointConventionBuilder`. The generator wraps every endpoint mapping in `HandlerClass.Configure(builder => ...)` so you can apply fluent calls not covered by attributes—e.g., `.AddEndpointFilterFactory(...)` or `.WithMetadata(new CustomMetadata())`. If you request `IServiceProvider`, the generator automatically passes `builder.ServiceProvider`. This hook executes once per handler after all attribute-driven configuration has been appended.

## 7. Route mapping behavior
- Each handler ultimately calls the same Minimal API surface area you would use manually. Verb attributes become `builder.MapGet`, `builder.MapPost`, etc., while non-standard verbs fall back to `builder.MapMethods(pattern, new[] { "VERB" })`. `[MapFallback]` uses `builder.MapFallback(pattern?, handler)`. Every mapping returns the builder instance so the generated code can continue chaining metadata and filters.
- When `[MapGroup]` is applied, the generator creates a single variable (e.g., `_MyFeature_Group`) that holds the grouped `RouteGroupBuilder`. All endpoints in the class call `.Map*` on that variable so you can attach shared conventions to the group in `Configure` or via class-level attributes.
- After mapping, `.WithName`, `.WithDisplayName`, `.WithSummary`, `.WithDescription`, `.WithGroupName`, `.WithOrder`, `.WithTags`, `.Accepts`, `.Produces*`, `.RequireAuthorization`, `.RequireCors`, `.RequireHost`, `.RequireRateLimiting`, `.DisableAntiforgery`, `.AllowAnonymous`, `.ShortCircuit`, `.DisableValidation`, `.WithRequestTimeout`, `.DisableRequestTimeout`, `.AddEndpointFilter`, etc., are appended in the exact order emitted inside `AppendEndpointConfiguration`, ensuring consistent, deterministic metadata regardless of how many attributes are applied.

## 8. Implementation recipe for AI agents
1. **Create or update the hosting project:** make sure it references the `GeneratedEndpoints` package and imports `Microsoft.AspNetCore.Generated.Routing` so startup can call the generated extension methods. No manual source inclusion is required.
2. **Define a feature class:** add a `static` or instantiable `class` inside the relevant namespace (e.g., `Features.Users`). If it needs DI, add constructor parameters; otherwise keep it static.
3. **Author handler methods:** decorate each method with the appropriate `[Map*]` attribute from `Microsoft.AspNetCore.Generated.Attributes`. Keep method signatures natural—parameters map directly, and return types follow normal Minimal API rules. Supply explicit `Name` arguments only when you need deterministic endpoint names or to avoid `Async` suffix removal.
4. **Apply metadata attributes:** add `[Summary]`, `[Description]`, `[Tags]`, `[Accepts]`, `[Produces*]`, `[RequireAuthorization]`, `[AllowAnonymous]`, `[RequireCors]`, `[RequireHost]`, `[RequireRateLimiting]`, `[RequestTimeout]`, `[DisableRequestTimeout]`, `[DisableAntiforgery]`, `[DisableValidation]`, `[ShortCircuit]`, `[Order]`, `[EndpointFilter]`, etc., to the class or method as needed. Rely on the generator to merge and emit the matching fluent calls.
5. **Bind parameters explicitly when required:** decorate method parameters with `[FromRoute(Name = "...")]`, `[FromQuery]`, `[FromHeader]`, `[FromBody]`, `[FromForm]`, `[FromServices]`, `[FromKeyedServices("key")]`, or `[AsParameters]` so the generator stamps the correct binding attributes into the lambda. Otherwise, ASP.NET Core’s default binding rules apply automatically.
6. **Optional group & configure:** add `[MapGroup("/users", Name = "Users")]` for a feature-specific route group, and add a `Configure<TBuilder>` method if you need additional fluent customization. Both apply once per class, affecting every endpoint within it.
7. **Build and run:** the generator emits DI registration and mapping code at compile time. Startup only needs to call the two extension methods; the rest of the boilerplate is source-generated and remains up to date as you add, rename, or remove handlers.

Following these rules keeps endpoint definitions close to their features while letting the generator handle registration, metadata, routing, and dependency injection in a deterministic, attribute-driven way.

## 9. Code examples

Minimal hosting setup:

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointHandlers();

var app = builder.Build();

app.MapEndpointHandlers();

app.Run();
```

Handler class with metadata, DI, and explicit parameter binding:

```csharp
using Microsoft.AspNetCore.Generated.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Features.Users;

[MapGroup("/users", Name = "Users")]
[Summary("User management endpoints")]
public sealed class UsersHandlers(IUserService users)
{
    [MapGet("/{id:int}", Name = "GetUser")]
    [ProducesResponse(StatusCode = StatusCodes.Status200OK)]
    [ProducesResponse(StatusCode = StatusCodes.Status404NotFound)]
    public IResult GetUser([FromRoute] int id)
        => users.TryFind(id, out var user)
            ? Results.Ok(user)
            : Results.NotFound();

    [MapPost("/", Name = "CreateUser")]
    [ProducesResponse(StatusCode = StatusCodes.Status201Created)]
    public async Task<IResult> CreateUser([FromBody] CreateUserRequest request, CancellationToken ct)
    {
        var created = await users.CreateAsync(request, ct);
        return Results.Created($"/users/{created.Id}", created);
    }
}
```
