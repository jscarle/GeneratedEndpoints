using GeneratedEndpoints.Tests.Common;
using SourceGeneratorTestHelpers.XUnit;

namespace GeneratedEndpoints.Tests;

[UsesVerify]
public class GeneratedEndpointsTests
{
    public GeneratedEndpointsTests()
    {
        ModuleInitializer.Initialize();
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task MapGet(bool withNamespace)
    {
        var sources = TestHelpers.GetSources("""
                                             [Tags("Users")]
                                             internal static class GetUserEndpoint
                                             {
                                                 [MapGet("/users/{id:int}", Name = nameof(GetUser), Summary = "Gets a user by ID.", Description = "Gets a user by ID when the ID is greater than zero.")]
                                                 public static Results<Ok, NotFound> GetUser2(int id)
                                                 {
                                                     if (id > 0)
                                                         return TypedResults.Ok();

                                                     return TypedResults.NotFound();
                                                 }
                                             }
                                             """, withNamespace
        );
        var result = TestHelpers.RunGenerator(sources);

        await result.VerifyAsync("AddEndpointHandlers.g.cs")
            .UseMethodName($"{nameof(MapGet)}_AddEndpointHandlers_With{(withNamespace ? "" : "out")}Namespace");

        await result.VerifyAsync("MapEndpointHandlers.g.cs")
            .UseMethodName($"{nameof(MapGet)}_MapEndpointHandlers_With{(withNamespace ? "" : "out")}Namespace");
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task MapGetWithConfigure(bool withNamespace)
    {
        var sources = TestHelpers.GetSources("""
                                             using Microsoft.AspNetCore.Builder;

                                             internal static class ConfigureEndpoint
                                             {
                                                 [MapGet("/configure")]
                                                 public static Ok Handle()
                                                     => TypedResults.Ok();

                                                 public static void Configure<TBuilder>(TBuilder builder)
                                                     where TBuilder : IEndpointConventionBuilder
                                                 {
                                                     builder.WithMetadata(new object());
                                                 }
                                             }
                                             """, withNamespace
        );

        var result = TestHelpers.RunGenerator(sources);

        await result.VerifyAsync("MapEndpointHandlers.g.cs")
            .UseMethodName($"{nameof(MapGetWithConfigure)}_MapEndpointHandlers_With{(withNamespace ? "" : "out")}Namespace");
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task MapGetWithConfigureServiceProvider(bool withNamespace)
    {
        var sources = TestHelpers.GetSources("""
                                             using Microsoft.AspNetCore.Builder;

                                             internal static class ConfigureEndpoint
                                             {
                                                 [MapGet("/service-provider")]
                                                 public static Ok Handle()
                                                     => TypedResults.Ok();

                                                 public static void Configure<TBuilder>(TBuilder builder, System.IServiceProvider serviceProvider)
                                                     where TBuilder : IEndpointConventionBuilder
                                                 {
                                                     _ = serviceProvider;
                                                     builder.WithMetadata(new object());
                                                 }
                                             }
                                             """, withNamespace
        );

        var result = TestHelpers.RunGenerator(sources);

        await result.VerifyAsync("MapEndpointHandlers.g.cs")
            .UseMethodName($"{nameof(MapGetWithConfigureServiceProvider)}_MapEndpointHandlers_With{(withNamespace ? "" : "out")}Namespace");
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task ClassAllowAnonymousMethodRequireAuthorization(bool withNamespace)
    {
        var sources = TestHelpers.GetSources("""
                                             [AllowAnonymous]
                                             internal sealed class AllowAnonymousClass
                                             {
                                                 [MapGet("/allow-anon")]
                                                 [RequireAuthorization]
                                                 public static Ok Handle()
                                                     => TypedResults.Ok();
                                             }
                                             """, withNamespace
        );

        var result = TestHelpers.RunGenerator(sources);

        await result.VerifyAsync("MapEndpointHandlers.g.cs")
            .UseMethodName($"{nameof(ClassAllowAnonymousMethodRequireAuthorization)}_MapEndpointHandlers_With{(withNamespace ? "" : "out")}Namespace");
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task MapAllAttributesAndHttpMethods(bool withNamespace)
    {
        var sources = TestHelpers.GetSources("""
                                             using System.Threading;
                                             using System.Threading.Tasks;
                                             using Microsoft.AspNetCore.Builder;
                                             using Microsoft.AspNetCore.Http;
                                             using Microsoft.AspNetCore.Mvc;
                                             using Microsoft.Extensions.DependencyInjection;

                                             [Tags("Shared", "ClassLevel")]
                                             [RequireAuthorization("PolicyA", "PolicyB")]
                                             [DisableAntiforgery]
                                             [Accepts("application/xml", "text/xml", RequestType = typeof(ClassLevelRequest))]
                                             [ProducesResponse(201, "application/json", "text/json", ResponseType = typeof(ClassLevelResponse))]
                                             [ProducesProblem(503, "application/problem+json")]
                                             [ProducesValidationProblem(409, "application/problem+json", "text/plain")]
                                             [ExcludeFromDescription]
                                             internal sealed class ComplexEndpoints
                                             {
                                                 private readonly IServiceProvider _serviceProvider;

                                                 public ComplexEndpoints(IServiceProvider serviceProvider)
                                                     => _serviceProvider = serviceProvider;

                                                 public static void Configure<TBuilder>(TBuilder builder, IServiceProvider serviceProvider)
                                                     where TBuilder : IEndpointConventionBuilder
                                                 {
                                                     _ = serviceProvider;
                                                     builder.WithMetadata("configured");
                                                 }

                                                 [MapGet("/complex/{id:int}", Name = nameof(GetComplex), Summary = "Gets complex data.", Description = "Uses every supported attribute.")]
                                                 [AllowAnonymous]
                                                 [Tags("MethodLevel")]
                                                 [RequireAuthorization("MethodPolicy")]
                                                [Accepts<GetRequest>("application/custom", "text/custom")]
                                                  [Microsoft.AspNetCore.Generated.Attributes.ProducesResponse<GetResponse>(200, "application/json", "text/json")]
                                                 [ProducesProblem(400, "application/problem+json", "text/plain")]
                                                 [ProducesValidationProblem(422, "application/problem+json", "text/plain")]
                                                 [ExcludeFromDescription]
                                                 public async Task<Results<Ok<GetResponse>, NotFound>> GetComplex(
                                                     [FromRoute] int id,
                                                     [FromQuery] string? filter,
                                                     [FromHeader(Name = "x-trace-id")] string? traceId,
                                                     [FromBody] GetRequest request,
                                                     [FromForm] string? formValue,
                                                     [FromServices] IServiceProvider services,
                                                     [FromKeyedServices("special")] object keyed,
                                                     [AsParameters] AdditionalParameters parameters,
                                                     CancellationToken cancellationToken)
                                                 {
                                                     _ = _serviceProvider;
                                                     _ = traceId;
                                                     _ = formValue;
                                                     _ = services;
                                                     _ = keyed;
                                                     _ = parameters;
                                                     await Task.Yield();
                                                     cancellationToken.ThrowIfCancellationRequested();
                                                     return TypedResults.Ok(new GetResponse(id));
                                                 }
                                             }

                                             internal sealed record AdditionalParameters(string? Search, int? Page);

                                             internal sealed record ClassLevelRequest(int Value);

                                             internal sealed record ClassLevelResponse(int Value);

                                             internal sealed record GetRequest(int Value);

                                             internal sealed record GetResponse(int Value);

                                             internal static class AllHttpMethodEndpoints
                                             {
                                                 [MapPost("/complex")]
                                                 public static async Task<Created<GetResponse>> CreateComplexAsync([FromBody] GetRequest request)
                                                 {
                                                     await Task.Yield();
                                                     return TypedResults.Created($"/complex/{request.Value}", new GetResponse(request.Value));
                                                 }

                                                 [MapPut("/complex/{id:int}")]
                                                 public static Results<NoContent, NotFound> UpdateComplex(int id)
                                                     => id > 0 ? TypedResults.NoContent() : TypedResults.NotFound();

                                                 [MapDelete("/complex/{id:int}")]
                                                 public static IResult DeleteComplex(int id)
                                                     => id > 0 ? TypedResults.Ok() : TypedResults.NotFound();

                                                 [MapOptions("/complex")]
                                                 public static IResult DescribeComplex()
                                                     => TypedResults.Ok();

                                                 [MapHead("/complex")]
                                                 public static IResult HeadComplex()
                                                     => TypedResults.Ok();

                                                 [MapPatch("/complex/{id:int}")]
                                                 public static IResult PatchComplex(int id)
                                                     => TypedResults.Ok();

                                                 [MapQuery("/complex/query")]
                                                 public static IResult QueryComplex([FromQuery] string term)
                                                     => TypedResults.Ok(term);

                                                 [MapTrace("/complex")]
                                                 public static IResult TraceComplex()
                                                     => TypedResults.Ok();

                                                 [MapConnect("/complex")]
                                                 public static IResult ConnectComplex()
                                                     => TypedResults.Ok();
                                             }
                                             """, withNamespace
        );

        var result = TestHelpers.RunGenerator(sources);

        await result.VerifyAsync("AddEndpointHandlers.g.cs")
            .UseMethodName($"{nameof(MapAllAttributesAndHttpMethods)}_AddEndpointHandlers_With{(withNamespace ? "" : "out")}Namespace");

        await result.VerifyAsync("MapEndpointHandlers.g.cs")
            .UseMethodName($"{nameof(MapAllAttributesAndHttpMethods)}_MapEndpointHandlers_With{(withNamespace ? "" : "out")}Namespace");
    }
}
