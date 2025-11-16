using System.Text;

namespace GeneratedEndpoints.Tests.Common;

public static class SourceFactory
{
    public static string BuildFallbackSource(bool includeDefault, bool includeCustom, string? customRoute)
    {
        var builder = new StringBuilder();
        builder.AppendLine("internal static class FallbackEndpoints");
        builder.AppendLine("{");

        if (includeDefault)
        {
            builder.AppendLine("    [MapFallback]");
            builder.AppendLine("    public static Ok Default() => TypedResults.Ok();");
            builder.AppendLine();
        }

        if (includeCustom)
        {
            var route = customRoute ?? "/custom";
            builder.AppendLine($"    [MapFallback(\"{route}\")]");
            builder.AppendLine("    public static Ok Custom() => TypedResults.Ok();");
            builder.AppendLine();
        }

        builder.AppendLine("}");
        return builder.ToString();
    }

    public static string BuildAuthorizationMatrixSource(
        bool classAllowAnonymous,
        bool methodAllowAnonymous,
        bool classRequireAuthorization,
        bool methodRequireAuthorization,
        bool classTags,
        bool methodTags,
        string? classHost,
        string? methodHost,
        bool classRequireCors,
        string? classCorsPolicy,
        bool methodRequireCors,
        string? methodCorsPolicy,
        bool requireRateLimiting,
        string? rateLimitingPolicy,
        bool applyShortCircuit,
        bool applyRequestTimeout,
        string? requestTimeoutPolicy,
        bool disableRequestTimeout,
        int orderValue,
        string? groupName,
        bool excludeFromDescription,
        string? mapGroupPattern = null)
    {
        var builder = new StringBuilder();

        if (classAllowAnonymous)
        {
            builder.AppendLine("[AllowAnonymous]");
        }

        if (classRequireAuthorization)
        {
            builder.AppendLine("[RequireAuthorization(\"ClassPolicy\")]");
        }

        if (classTags)
        {
            builder.AppendLine("[Tags(\"Class\", \"Matrix\")]");
        }

        if (!string.IsNullOrWhiteSpace(classHost))
        {
            builder.AppendLine($"[RequireHost(\"{classHost}\")]");
        }

        if (classRequireCors)
        {
            var cors = string.IsNullOrWhiteSpace(classCorsPolicy) ? "" : $"(\"{classCorsPolicy}\")";
            builder.AppendLine($"[RequireCors{cors}]");
        }

        if (!string.IsNullOrWhiteSpace(groupName) && mapGroupPattern is null)
        {
            mapGroupPattern = string.Empty;
        }

        if (mapGroupPattern is not null)
        {
            var mapGroupAttribute = new StringBuilder();
            mapGroupAttribute.Append($"[MapGroup(\"{mapGroupPattern}\"");
            if (!string.IsNullOrWhiteSpace(groupName))
            {
                mapGroupAttribute.Append($", Name = \"{groupName}\"");
            }

            mapGroupAttribute.Append(")]");
            builder.AppendLine(mapGroupAttribute.ToString());
        }

        if (applyShortCircuit)
        {
            builder.AppendLine("[ShortCircuit]");
        }

        if (applyRequestTimeout)
        {
            var timeoutArgument = string.IsNullOrWhiteSpace(requestTimeoutPolicy)
                ? string.Empty
                : $"(\"{requestTimeoutPolicy}\")";
            builder.AppendLine($"[RequestTimeout{timeoutArgument}]");
        }

        if (disableRequestTimeout)
        {
            builder.AppendLine("[DisableRequestTimeout]");
        }

        if (orderValue != 0)
        {
            builder.AppendLine($"[Order({orderValue})]");
        }

        if (excludeFromDescription)
        {
            builder.AppendLine("[ExcludeFromDescription]");
        }

        builder.AppendLine("internal sealed class AuthorizationMatrixEndpoints");
        builder.AppendLine("{");
        builder.AppendLine("    [MapGet(\"/matrix/{id:int}\", Name = \"GetMatrix\")]");

        if (methodAllowAnonymous)
        {
            builder.AppendLine("    [AllowAnonymous]");
        }

        if (methodRequireAuthorization)
        {
            builder.AppendLine("    [RequireAuthorization(\"MethodPolicy\")]");
        }

        if (methodTags)
        {
            builder.AppendLine("    [Tags(\"Method\", \"Matrix\")]");
        }

        if (!string.IsNullOrWhiteSpace(methodHost))
        {
            builder.AppendLine($"    [RequireHost(\"{methodHost}\", \"contoso.com\")]");
        }

        if (methodRequireCors)
        {
            var methodCors = string.IsNullOrWhiteSpace(methodCorsPolicy) ? string.Empty : $"(\"{methodCorsPolicy}\")";
            builder.AppendLine($"    [RequireCors{methodCors}]");
        }

        if (requireRateLimiting)
        {
            var rateLimit = string.IsNullOrWhiteSpace(rateLimitingPolicy) ? string.Empty : $"(\"{rateLimitingPolicy}\")";
            builder.AppendLine($"    [RequireRateLimiting{rateLimit}]");
        }

        builder.AppendLine("    public static Ok Handle(int id) => id >= 0 ? TypedResults.Ok() : TypedResults.Ok();");

        if (!string.IsNullOrWhiteSpace(mapGroupPattern))
        {
            builder.AppendLine();
            builder.AppendLine("    [MapDelete(\"/matrix/{id:int}\")]");
            builder.AppendLine("    public static Results<NoContent, NotFound> Delete(int id)");
            builder.AppendLine("        => id >= 0 ? TypedResults.NoContent() : TypedResults.NotFound();");
        }

        builder.AppendLine("}");
        return builder.ToString();
    }

    public static string BuildConfigureAndFiltersSource(
        bool configureWithServiceProvider,
        bool configureAddsMetadata,
        bool includeClassLevelFilter,
        bool includeMethodLevelFilter,
        bool includeGenericFilter,
        bool configureRegistersFilter,
        string metadataValue)
    {
        var builder = new StringBuilder();
        builder.AppendLine("using Microsoft.AspNetCore.Builder;");
        builder.AppendLine();

        if (includeClassLevelFilter)
        {
            builder.AppendLine("[EndpointFilter(typeof(TimingFilter))]");
        }

        builder.AppendLine("internal static class ConfigureFilterEndpoints");
        builder.AppendLine("{");
        builder.AppendLine("    [MapGet(\"/configure-filters\")]");

        if (includeMethodLevelFilter)
        {
            builder.AppendLine("    [EndpointFilter(typeof(ValidationFilter))]");
        }

        if (includeGenericFilter)
        {
            builder.AppendLine("    [EndpointFilter<ValidationFilter>]");
        }

        builder.AppendLine("    public static Ok Handle() => TypedResults.Ok();");
        builder.AppendLine();
        builder.AppendLine("    public static void Configure<TBuilder>(TBuilder builder" + (configureWithServiceProvider ? ", IServiceProvider services" : string.Empty) + ")");
        builder.AppendLine("        where TBuilder : IEndpointConventionBuilder");
        builder.AppendLine("    {");
        builder.AppendLine("        _ = builder;");

        if (configureWithServiceProvider)
        {
            builder.AppendLine("        _ = services;");
        }

        if (configureAddsMetadata)
        {
            builder.AppendLine($"        builder.WithMetadata(\"{metadataValue}\");");
        }

        if (configureRegistersFilter)
        {
            builder.AppendLine("        builder.AddEndpointFilterFactory((context, next) => next);");
        }

        builder.AppendLine("    }");
        builder.AppendLine("}");
        builder.AppendLine();
        builder.AppendLine("internal sealed class TimingFilter : IEndpointFilter");
        builder.AppendLine("{");
        builder.AppendLine("    public ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next) => next(context);");
        builder.AppendLine("}");
        builder.AppendLine();
        builder.AppendLine("internal sealed class ValidationFilter : IEndpointFilter");
        builder.AppendLine("{");
        builder.AppendLine("    public ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next) => next(context);");
        builder.AppendLine("}");

        return builder.ToString();
    }

    public static string BuildHttpMethodMatrixSource(
        bool includeGet,
        bool includePost,
        bool includePut,
        bool includeDelete,
        bool includeOptions,
        bool includeHead,
        bool includePatch,
        bool includeQuery,
        bool includeTrace,
        bool includeConnect,
        bool includeMethodNameCollision)
    {
        var builder = new StringBuilder();
        builder.AppendLine("using Microsoft.AspNetCore.Mvc;");
        builder.AppendLine();
        builder.AppendLine("internal static class HttpMethodEndpoints");
        builder.AppendLine("{");

        if (includeGet)
        {
            builder.AppendLine("    [MapGet(\"/matrix\")] public static Ok Get() => TypedResults.Ok();");
        }

        if (includePost)
        {
            builder.AppendLine("    [MapPost(\"/matrix\")] public static Created<string> Post() => TypedResults.Created(\"/matrix/1\", \"Created\");");
        }

        if (includePut)
        {
            builder.AppendLine("    [MapPut(\"/matrix/{id:int}\")] public static Results<NoContent, NotFound> Put(int id) => id > 0 ? TypedResults.NoContent() : TypedResults.NotFound();");
        }

        if (includeDelete)
        {
            builder.AppendLine("    [MapDelete(\"/matrix/{id:int}\")] public static IResult Delete(int id) => TypedResults.Ok();");
        }

        if (includeOptions)
        {
            builder.AppendLine("    [MapOptions(\"/matrix\")] public static IResult Options() => TypedResults.Ok();");
        }

        if (includeHead)
        {
            builder.AppendLine("    [MapHead(\"/matrix\")] public static IResult Head() => TypedResults.Ok();");
        }

        if (includePatch)
        {
            builder.AppendLine("    [MapPatch(\"/matrix/{id:int}\")] public static IResult Patch(int id) => TypedResults.Ok();");
        }

        if (includeQuery)
        {
            builder.AppendLine("    [MapQuery(\"/matrix/query\")] public static IResult Query([FromQuery] string value) => TypedResults.Ok(value);");
        }

        if (includeTrace)
        {
            builder.AppendLine("    [MapTrace(\"/matrix\")] public static IResult Trace() => TypedResults.Ok();");
        }

        if (includeConnect)
        {
            builder.AppendLine("    [MapConnect(\"/matrix\")] public static IResult Connect() => TypedResults.Ok();");
        }

        builder.AppendLine("}");

        if (includeMethodNameCollision)
        {
            builder.AppendLine();
            builder.AppendLine("internal static class AlternateEndpoints");
            builder.AppendLine("{");
            builder.AppendLine("    [MapGet(\"/alternate\")] public static Ok Get() => TypedResults.Ok();");
            builder.AppendLine("    [MapPost(\"/alternate\")] public static IResult Post() => TypedResults.Ok();");
            builder.AppendLine("}");
        }

        return builder.ToString();
    }

    public static string BuildContractsAndBindingSource(
        bool includeBindingNames,
        bool includeAsParameters,
        bool includeFromServices,
        bool includeFromKeyedServices,
        bool includeAccepts,
        bool includeGenericAccepts,
        bool includeProducesResponse,
        bool includeProducesProblem,
        bool includeProducesValidationProblem,
        bool includeSummaryAndDescription,
        bool includeDisplayName,
        bool includeTags,
        bool excludeFromDescription,
        bool allowAnonymous,
        bool methodRequiresAuthorization,
        string? acceptsContentType1,
        string? acceptsContentType2,
        string? producesContentType1,
        string? producesContentType2)
    {
        var builder = new StringBuilder();
        builder.AppendLine("using Microsoft.AspNetCore.Mvc;");
        builder.AppendLine("using Microsoft.Extensions.DependencyInjection;");
        builder.AppendLine();
        builder.AppendLine("internal sealed class ContractEndpoints");
        builder.AppendLine("{");

        if (includeSummaryAndDescription)
        {
            builder.AppendLine("    [Summary(\"Gets detailed content.\")]");
            builder.AppendLine("    [Description(\"Shows binding and contract combinations.\")]");
        }

        if (includeDisplayName)
        {
            builder.AppendLine("    [DisplayName(\"Contract endpoint\")]");
        }

        if (includeTags)
        {
            builder.AppendLine("    [Tags(\"Contracts\", \"Bindings\")]");
        }

        if (excludeFromDescription)
        {
            builder.AppendLine("    [ExcludeFromDescription]");
        }

        if (allowAnonymous)
        {
            builder.AppendLine("    [AllowAnonymous]");
        }

        if (methodRequiresAuthorization)
        {
            builder.AppendLine("    [RequireAuthorization(\"ContractsPolicy\")]");
        }

        builder.AppendLine("    [MapGet(\"/contracts/{id:int}\")]");

        if (includeAccepts)
        {
            var secondContentType = string.IsNullOrWhiteSpace(acceptsContentType2) ? string.Empty : $", \"{acceptsContentType2}\"";
            builder.AppendLine($"    [Accepts(\"{acceptsContentType1 ?? "application/json"}\"{secondContentType})]");
        }

        if (includeGenericAccepts)
        {
            builder.AppendLine($"    [Accepts<RequestRecord>(\"{acceptsContentType1 ?? "application/json"}\")]");
        }

        if (includeProducesResponse)
        {
            var secondProduces = string.IsNullOrWhiteSpace(producesContentType2) ? string.Empty : $", \"{producesContentType2}\"";
            builder.AppendLine($"    [ProducesResponse(200, \"{producesContentType1 ?? "application/json"}\"{secondProduces}, ResponseType = typeof(ResponseRecord))]");
        }

        if (includeProducesProblem)
        {
            builder.AppendLine($"    [ProducesProblem(500, \"{producesContentType1 ?? "application/problem+json"}\")]");
        }

        if (includeProducesValidationProblem)
        {
            builder.AppendLine($"    [ProducesValidationProblem(422, \"{producesContentType1 ?? "application/problem+json"}\")]");
        }

        builder.AppendLine("    public static async Task<Results<Ok<ResponseRecord>, NotFound>> Handle(");
        builder.AppendLine(includeBindingNames
            ? "        [FromRoute(Name = \"route-id\")] int id,"
            : "        [FromRoute] int id,");
        builder.AppendLine(includeBindingNames
            ? "        [FromQuery(Name = \"filter-term\")] string? filter,"
            : "        [FromQuery] string? filter,");
        builder.AppendLine(includeBindingNames
            ? "        [FromHeader(Name = \"x-trace-id\")] string? traceId,"
            : "        [FromHeader] string? traceId,");
        builder.AppendLine("        [FromBody] RequestRecord request,");

        if (includeAsParameters)
        {
            builder.AppendLine("        [AsParameters] AdditionalParameters parameters,");
        }

        if (includeFromServices)
        {
            builder.AppendLine("        [FromServices] IServiceProvider services,");
        }

        if (includeFromKeyedServices)
        {
            builder.AppendLine("        [FromKeyedServices(\"special\")] object keyed,");
        }

        builder.AppendLine("        CancellationToken cancellationToken)");
        builder.AppendLine("    {");
        builder.AppendLine("        await Task.Yield();");
        builder.AppendLine("        cancellationToken.ThrowIfCancellationRequested();");
        builder.AppendLine("        return id > 0 ? TypedResults.Ok(new ResponseRecord(id)) : TypedResults.NotFound();");
        builder.AppendLine("    }");
        builder.AppendLine("}");
        builder.AppendLine();
        builder.AppendLine("internal sealed record RequestRecord(int Value);");
        builder.AppendLine("internal sealed record ResponseRecord(int Value);");

        if (includeAsParameters)
        {
            builder.AppendLine("internal sealed record AdditionalParameters(string? Search, int? Page);");
        }

        return builder.ToString();
    }
}
