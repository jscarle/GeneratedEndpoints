using System.Collections.Immutable;
using System.Text;
using GeneratedEndpoints.Common;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using static GeneratedEndpoints.Common.Constants;

namespace GeneratedEndpoints;

// ReSharper disable ForCanBeConvertedToForeach
// ReSharper disable LoopCanBeConvertedToQuery
// Do not refactor, use for loop to avoid allocations.

internal static class UseEndpointHandlersGenerator
{
    public static void GenerateSource(SourceProductionContext context, ImmutableSortedDictionary<RequestHandlerClass, ImmutableArray<RequestHandler>> grouped)
    {
        context.CancellationToken.ThrowIfCancellationRequested();

        var requestHandlers = grouped.Values
            .SelectMany(x => x)
            .ToImmutableList();

        var source = new StringBuilder();
        source.AppendLine(FileHeader);

        source.AppendLine();

        source.AppendLine("using Microsoft.AspNetCore.Builder;");
        source.AppendLine("using Microsoft.AspNetCore.Http;");
        source.AppendLine("using Microsoft.AspNetCore.Mvc;");
        source.AppendLine("using Microsoft.AspNetCore.Routing;");
        if (AddUsingRateLimiting(requestHandlers))
            source.AppendLine("using Microsoft.AspNetCore.RateLimiting;");
        source.AppendLine("using Microsoft.Extensions.DependencyInjection;");
        source.AppendLine();

        source.Append("namespace ");
        source.Append(RoutingNamespace);
        source.AppendLine(";");

        source.AppendLine();

        source.Append("internal static class ");
        source.Append(UseEndpointHandlersClassName);
        source.AppendLine();

        source.AppendLine("{");

        source.Append("    internal static IEndpointRouteBuilder ");
        source.Append(UseEndpointHandlersMethodName);
        source.AppendLine("(this IEndpointRouteBuilder builder)");

        source.AppendLine("    {");

        var groupedClasses = GetClassesWithGroups(requestHandlers);

        for (var index = 0; index < groupedClasses.Count; index++)
        {
            var groupedClass = groupedClasses[index];
            var configuration = groupedClass.Configuration;
            if (!configuration.Group.HasValue)
                continue;

            var group = configuration.Group.Value;

            source.Append("        var ");
            source.Append(group.Identifier);
            source.Append(" = builder.MapGroup(");
            source.Append(group.Pattern.ToStringLiteral());
            source.Append(')');
            AppendEndpointConfiguration(source, "            ", configuration);
            source.AppendLine(";");
        }

        if (groupedClasses.Count > 0)
            source.AppendLine();

        for (var index = 0; index < requestHandlers.Count; index++)
        {
            if (index > 0)
                source.AppendLine();

            var requestHandler = requestHandlers[index];
            GenerateMapRequestHandler(source, requestHandler);
        }

        source.AppendLine("""

                                  return builder;
                              }
                          }
                          """
        );

        var sourceText = StringBuilderPool.ToStringAndReturn(source);
        context.AddSource(UseEndpointHandlersMethodHint, SourceText.From(sourceText, Encoding.UTF8));
    }

    private static bool AddUsingRateLimiting(ImmutableList<RequestHandler> requestHandlers)
    {
        for (var index = 0; index < requestHandlers.Count; index++)
        {
            var handler = requestHandlers[index];
            if (handler.Class.Configuration.RequireRateLimiting || handler.Method.Configuration.RequireRateLimiting)
                return true;
        }

        return false;
    }

    private static List<RequestHandlerClass> GetClassesWithGroups(ImmutableList<RequestHandler> requestHandlers)
    {
        if (requestHandlers.Count == 0)
            return [];

        HashSet<string>? seen = null;
        List<RequestHandlerClass>? groupedClasses = null;
        for (var index = 0; index < requestHandlers.Count; index++)
        {
            var handler = requestHandlers[index];
            var handlerClass = handler.Class;
            if (!handlerClass.Configuration.Group.HasValue)
                continue;

            seen ??= new HashSet<string>(StringComparer.Ordinal);
            if (!seen.Add(handlerClass.Name))
                continue;

            groupedClasses ??= [];
            groupedClasses.Add(handlerClass);
        }

        return groupedClasses ?? [];
    }

    private static void GenerateMapRequestHandler(StringBuilder source, RequestHandler requestHandler)
    {
        var wrapWithConfigure = requestHandler.Class.HasConfigureMethod;
        var configureAcceptsServiceProvider = requestHandler.Class.ConfigureMethodAcceptsServiceProvider;
        var indent = wrapWithConfigure ? "            " : "        ";
        var continuationIndent = indent + "    ";
        var routeBuilderIdentifier = requestHandler.Class.Configuration.Group?.Identifier ?? "builder";

        if (wrapWithConfigure)
        {
            source.Append("        ");
            source.Append(requestHandler.Class.Name);
            source.Append('.');
            source.Append(ConfigureMethodName);
            source.AppendLine("(");
        }

        var isFallback = string.Equals(requestHandler.HttpMethod, FallbackHttpMethod, StringComparison.Ordinal);
        var mapMethodSuffix = isFallback ? null : GetMapMethodSuffix(requestHandler.HttpMethod);

        source.Append(indent);
        if (isFallback)
        {
            source.Append(routeBuilderIdentifier);
            source.Append(".MapFallback(");
            if (!string.IsNullOrEmpty(requestHandler.Pattern))
            {
                source.Append(requestHandler.Pattern.ToStringLiteral());
                source.Append(", ");
            }
        }
        else
        {
            source.Append(routeBuilderIdentifier);
            source.Append(".Map");
            source.Append(mapMethodSuffix ?? "Methods");
            source.Append('(');
            source.Append(requestHandler.Pattern.ToStringLiteral());
            source.Append(", ");
            if (mapMethodSuffix is null)
            {
                source.Append("new[] { \"");
                source.Append(requestHandler.HttpMethod);
                source.Append("\" }, ");
            }
        }
        if (requestHandler.Method.IsStatic)
        {
            source.Append(requestHandler.Class.Name);
            source.Append('.');
            source.Append(requestHandler.Method.Name);
        }
        else
        {
            source.Append("static ([FromServices] ");
            source.Append(requestHandler.Class.Name);
            source.Append(" handler");
            foreach (var parameter in requestHandler.Method.Parameters)
            {
                source.Append(", ");
                source.Append(parameter.BindingPrefix);
                source.Append(parameter.Type);
                source.Append(' ');
                source.Append(parameter.Name);
            }
            source.Append(") => handler.");
            source.Append(requestHandler.Method.Name);
            source.Append('(');
            for (var index = 0; index < requestHandler.Method.Parameters.Count; index++)
            {
                if (index > 0)
                    source.Append(", ");
                var parameter = requestHandler.Method.Parameters[index];
                source.Append(parameter.Name);
            }
            source.Append(')');
        }
        source.Append(')');

        var configuration = requestHandler.Method.Configuration;
        if (!requestHandler.Class.Configuration.Group.HasValue)
            configuration = MergeEndpointConfigurations(requestHandler.Class.Configuration, configuration);

        if (!string.IsNullOrEmpty(requestHandler.Name))
        {
            source.AppendLine();
            source.Append(continuationIndent);
            source.Append(".WithName(");
            source.Append(requestHandler.Name.ToStringLiteral());
            source.Append(')');
        }
        AppendEndpointConfiguration(source, continuationIndent, configuration);

        if (wrapWithConfigure && configureAcceptsServiceProvider)
        {
            source.AppendLine(",");
            source.Append(indent);
            source.Append("builder.ServiceProvider");
        }

        if (wrapWithConfigure)
        {
            source.AppendLine();
            source.Append("        );");
            source.AppendLine();
        }
        else
        {
            source.AppendLine(";");
        }
    }

    private static void AppendEndpointConfiguration(StringBuilder source, string indent, EndpointConfiguration configuration)
    {
        if (!string.IsNullOrEmpty(configuration.DisplayName))
        {
            source.AppendLine();
            source.Append(indent);
            source.Append(".WithDisplayName(");
            source.Append(configuration.DisplayName.ToStringLiteral());
            source.Append(')');
        }

        if (!string.IsNullOrEmpty(configuration.Summary))
        {
            source.AppendLine();
            source.Append(indent);
            source.Append(".WithSummary(");
            source.Append(configuration.Summary.ToStringLiteral());
            source.Append(')');
        }

        if (!string.IsNullOrEmpty(configuration.Description))
        {
            source.AppendLine();
            source.Append(indent);
            source.Append(".WithDescription(");
            source.Append(configuration.Description.ToStringLiteral());
            source.Append(')');
        }

        if (configuration.Group is { Name.Length: > 0 })
        {
            source.AppendLine();
            source.Append(indent);
            source.Append(".WithGroupName(");
            source.Append(configuration.Group.Value.Name.ToStringLiteral());
            source.Append(')');
        }

        if (configuration.Order is { } orderValue)
        {
            source.AppendLine();
            source.Append(indent);
            source.Append(".WithOrder(");
            source.Append(orderValue);
            source.Append(')');
        }

        if (configuration.ExcludeFromDescription)
        {
            source.AppendLine();
            source.Append(indent);
            source.Append(".ExcludeFromDescription()");
        }

        if (configuration.Tags is { Count: > 0 })
        {
            source.AppendLine();
            source.Append(indent);
            source.Append(".WithTags(");
            AppendCommaSeparatedLiterals(source, configuration.Tags.Value);
            source.Append(')');
        }

        if (configuration.Accepts is { Count: > 0 })
            foreach (var accepts in configuration.Accepts.Value)
            {
                source.AppendLine();
                source.Append(indent);
                source.Append(".Accepts<");
                source.Append(accepts.RequestType);
                source.Append('>');
                source.Append('(');
                if (accepts.IsOptional)
                    source.Append("isOptional: true, ");
                source.Append(accepts.ContentType.ToStringLiteral());
                AppendAdditionalContentTypes(source, accepts.AdditionalContentTypes);
                source.Append(')');
            }

        if (configuration.Produces is { Count: > 0 })
            foreach (var produces in configuration.Produces.Value)
            {
                source.AppendLine();
                source.Append(indent);
                source.Append(".Produces<");
                source.Append(produces.ResponseType);
                source.Append('>');
                source.Append('(');
                source.Append(produces.StatusCode);
                AppendOptionalContentTypes(source, produces.ContentType, produces.AdditionalContentTypes);
                source.Append(')');
            }

        if (configuration.ProducesProblem is { Count: > 0 })
            foreach (var producesProblem in configuration.ProducesProblem.Value)
            {
                source.AppendLine();
                source.Append(indent);
                source.Append(".ProducesProblem(");
                source.Append(producesProblem.StatusCode);
                AppendOptionalContentTypes(source, producesProblem.ContentType, producesProblem.AdditionalContentTypes);
                source.Append(')');
            }

        if (configuration.ProducesValidationProblem is { Count: > 0 })
            foreach (var producesValidationProblem in configuration.ProducesValidationProblem.Value)
            {
                source.AppendLine();
                source.Append(indent);
                source.Append(".ProducesValidationProblem(");
                source.Append(producesValidationProblem.StatusCode);
                AppendOptionalContentTypes(source, producesValidationProblem.ContentType, producesValidationProblem.AdditionalContentTypes);
                source.Append(')');
            }

        if (configuration.RequireAuthorization)
        {
            source.AppendLine();
            if (configuration.AuthorizationPolicies is { Count: > 0 })
            {
                source.Append(indent);
                source.Append(".RequireAuthorization(");
                AppendCommaSeparatedLiterals(source, configuration.AuthorizationPolicies.Value);
                source.Append(')');
            }
            else
            {
                source.Append(indent);
                source.Append(".RequireAuthorization()");
            }
        }

        if (configuration.RequireCors)
        {
            source.AppendLine();
            source.Append(indent);
            if (!string.IsNullOrEmpty(configuration.CorsPolicyName))
            {
                source.Append(".RequireCors(");
                source.Append(configuration.CorsPolicyName.ToStringLiteral());
                source.Append(')');
            }
            else
            {
                source.Append(".RequireCors()");
            }
        }

        if (configuration.RequiredHosts is { Count: > 0 })
        {
            source.AppendLine();
            source.Append(indent);
            source.Append(".RequireHost(");
            AppendCommaSeparatedLiterals(source, configuration.RequiredHosts.Value);
            source.Append(')');
        }

        if (configuration.RequireRateLimiting && !string.IsNullOrEmpty(configuration.RateLimitingPolicyName))
        {
            source.AppendLine();
            source.Append(indent);
            source.Append(".RequireRateLimiting(");
            source.Append(configuration.RateLimitingPolicyName.ToStringLiteral());
            source.Append(')');
        }

        if (configuration.DisableAntiforgery)
        {
            source.AppendLine();
            source.Append(indent);
            source.Append(".DisableAntiforgery()");
        }

        if (configuration.AllowAnonymous)
        {
            source.AppendLine();
            source.Append(indent);
            source.Append(".AllowAnonymous()");
        }

        if (configuration.ShortCircuit)
        {
            source.AppendLine();
            source.Append(indent);
            source.Append(".ShortCircuit()");
        }

        if (configuration.DisableValidation)
        {
            source.AppendLine();
            source.Append(indent);
            source.Append(".DisableValidation()");
            source.AppendLine();
        }

        if (configuration.DisableRequestTimeout)
        {
            source.AppendLine();
            source.Append(indent);
            source.Append(".DisableRequestTimeout()");
        }
        else if (configuration.WithRequestTimeout)
        {
            source.AppendLine();
            source.Append(indent);
            if (!string.IsNullOrEmpty(configuration.RequestTimeoutPolicyName))
            {
                source.Append(".WithRequestTimeout(");
                source.Append(configuration.RequestTimeoutPolicyName.ToStringLiteral());
                source.Append(')');
            }
            else
            {
                source.Append(".WithRequestTimeout()");
            }
        }

        if (configuration.EndpointFilterTypes is { Count: > 0 })
            foreach (var filterType in configuration.EndpointFilterTypes.Value)
            {
                source.AppendLine();
                source.Append(indent);
                source.Append(".AddEndpointFilter<");
                source.Append(filterType);
                source.Append(">()");
            }
    }

    private static EndpointConfiguration MergeEndpointConfigurations(EndpointConfiguration classConfiguration, EndpointConfiguration methodConfiguration)
    {
        var displayName = methodConfiguration.DisplayName ?? classConfiguration.DisplayName;
        var summary = methodConfiguration.Summary ?? classConfiguration.Summary;
        var description = methodConfiguration.Description ?? classConfiguration.Description;
        var tags = MergeDistinctStrings(methodConfiguration.Tags, classConfiguration.Tags);
        var excludeFromDescription = methodConfiguration.ExcludeFromDescription || classConfiguration.ExcludeFromDescription;

        var accepts = ConcatEquatable(methodConfiguration.Accepts, classConfiguration.Accepts);
        var produces = ConcatEquatable(methodConfiguration.Produces, classConfiguration.Produces);
        var producesProblem = ConcatEquatable(methodConfiguration.ProducesProblem, classConfiguration.ProducesProblem);
        var producesValidationProblem = ConcatEquatable(methodConfiguration.ProducesValidationProblem, classConfiguration.ProducesValidationProblem);

        var shortCircuit = methodConfiguration.ShortCircuit || classConfiguration.ShortCircuit;
        var order = methodConfiguration.Order ?? classConfiguration.Order;
        var disableAntiforgery = methodConfiguration.DisableAntiforgery || classConfiguration.DisableAntiforgery;
        var disableValidation = methodConfiguration.DisableValidation || classConfiguration.DisableValidation;
        var requiredHosts = MergeDistinctStrings(methodConfiguration.RequiredHosts, classConfiguration.RequiredHosts);
        var endpointFilterTypes = ConcatEquatable(methodConfiguration.EndpointFilterTypes, classConfiguration.EndpointFilterTypes);

        var (allowAnonymous, requireAuthorization) = ResolveAuthorization(methodConfiguration, classConfiguration);
        var authorizationPolicies = MergeDistinctStrings(methodConfiguration.AuthorizationPolicies, classConfiguration.AuthorizationPolicies);

        var (requireCors, corsPolicyName) = ResolveCors(methodConfiguration, classConfiguration);
        var (requireRateLimiting, rateLimitingPolicyName) = ResolveRateLimiting(methodConfiguration, classConfiguration);

        var (disableRequestTimeout, withRequestTimeout, requestTimeoutPolicyName) = ResolveRequestTimeout(methodConfiguration, classConfiguration);

        var groupIdentifier = methodConfiguration.Group?.Identifier ?? classConfiguration.Group?.Identifier;
        var groupPattern = methodConfiguration.Group?.Pattern ?? classConfiguration.Group?.Pattern;
        var groupName = methodConfiguration.Group?.Name ?? classConfiguration.Group?.Name;

        return new EndpointConfiguration
        {
            DisplayName = displayName,
            Summary = summary,
            Description = description,
            Tags = tags,
            Accepts = accepts,
            Produces = produces,
            ProducesProblem = producesProblem,
            ProducesValidationProblem = producesValidationProblem,
            ExcludeFromDescription = excludeFromDescription,
            RequireAuthorization = requireAuthorization,
            AuthorizationPolicies = authorizationPolicies,
            DisableAntiforgery = disableAntiforgery,
            AllowAnonymous = allowAnonymous,
            RequireCors = requireCors,
            CorsPolicyName = corsPolicyName,
            RequiredHosts = requiredHosts,
            RequireRateLimiting = requireRateLimiting,
            RateLimitingPolicyName = rateLimitingPolicyName,
            EndpointFilterTypes = endpointFilterTypes,
            ShortCircuit = shortCircuit,
            DisableValidation = disableValidation,
            DisableRequestTimeout = disableRequestTimeout,
            WithRequestTimeout = withRequestTimeout,
            RequestTimeoutPolicyName = requestTimeoutPolicyName,
            Order = order,
            Group = groupIdentifier is not null && groupPattern is not null
                ? new EndpointGroup
                {
                    Identifier = groupIdentifier,
                    Pattern = groupPattern,
                    Name = groupName,
                }
                : null,
        };
    }

    private static (bool AllowAnonymous, bool RequireAuthorization) ResolveAuthorization(
        EndpointConfiguration methodConfiguration,
        EndpointConfiguration classConfiguration
    )
    {
        var methodReq = methodConfiguration.RequireAuthorization;
        var methodAnon = !methodReq && methodConfiguration.AllowAnonymous;

        var classReq = classConfiguration.RequireAuthorization;
        var classAnon = !classReq && classConfiguration.AllowAnonymous;

        var methodDeclares = methodConfiguration.AllowAnonymous || methodConfiguration.RequireAuthorization;

        if (methodDeclares)
        {
            // Method directive wins
            if (methodReq)
                return (AllowAnonymous: false, RequireAuthorization: true);

            if (methodAnon)
                return (AllowAnonymous: true, RequireAuthorization: false);

            return (false, false);
        }

        if (classReq)
            return (AllowAnonymous: false, RequireAuthorization: true);

        if (classAnon)
            return (AllowAnonymous: true, RequireAuthorization: false);

        return (AllowAnonymous: false, RequireAuthorization: false);
    }

    private static (bool DisableRequestTimeout, bool WithRequestTimeout, string? RequestTimeoutPolicyName) ResolveRequestTimeout(
        EndpointConfiguration methodConfiguration,
        EndpointConfiguration classConfiguration
    )
    {
        var methodWith = methodConfiguration.WithRequestTimeout;
        var methodDisable = !methodWith && methodConfiguration.DisableRequestTimeout;

        var classWith = classConfiguration.WithRequestTimeout;
        var classDisable = !classWith && classConfiguration.DisableRequestTimeout;

        var methodDeclares = methodConfiguration.DisableRequestTimeout || methodConfiguration.WithRequestTimeout;

        if (methodDeclares)
        {
            if (methodWith)
                return (DisableRequestTimeout: false, WithRequestTimeout: true, methodConfiguration.RequestTimeoutPolicyName);

            if (methodDisable)
                return (DisableRequestTimeout: true, WithRequestTimeout: false, null);

            return (false, false, null);
        }

        if (classWith)
            return (DisableRequestTimeout: false, WithRequestTimeout: true, classConfiguration.RequestTimeoutPolicyName);

        if (classDisable)
            return (DisableRequestTimeout: true, WithRequestTimeout: false, null);

        return (DisableRequestTimeout: false, WithRequestTimeout: false, null);
    }

    private static (bool RequireCors, string? CorsPolicyName) ResolveCors(EndpointConfiguration methodConfiguration, EndpointConfiguration classConfiguration)
    {
        if (methodConfiguration.RequireCors)
            return (RequireCors: true, methodConfiguration.CorsPolicyName);

        if (classConfiguration.RequireCors)
            return (RequireCors: true, classConfiguration.CorsPolicyName);

        return (RequireCors: false, CorsPolicyName: null);
    }

    private static (bool RequireRateLimiting, string? RateLimitingPolicyName) ResolveRateLimiting(
        EndpointConfiguration methodConfiguration,
        EndpointConfiguration classConfiguration
    )
    {
        if (methodConfiguration.RequireRateLimiting)
            return (RequireRateLimiting: true, methodConfiguration.RateLimitingPolicyName);

        if (classConfiguration.RequireRateLimiting)
            return (RequireRateLimiting: true, classConfiguration.RateLimitingPolicyName);

        return (RequireRateLimiting: false, RateLimitingPolicyName: null);
    }

    private static EquatableImmutableArray<string>? MergeDistinctStrings(EquatableImmutableArray<string>? first, EquatableImmutableArray<string>? second)
    {
        if (first is not { Count: > 0 })
            return second;
        if (second is not { Count: > 0 })
            return first;

        var merged = MergeUnion(first, second.Value);
        return merged.Count > 0 ? merged : null;
    }

    private static EquatableImmutableArray<string> MergeUnion(EquatableImmutableArray<string>? existing, EquatableImmutableArray<string> values)
    {
        List<string>? list = null;
        HashSet<string>? seen = null;

        if (existing is { Count: > 0 })
        {
            var count = existing.Value.Count;
            list = new List<string>(count + 4);
            list.AddRange(existing.Value);
            seen = new HashSet<string>(existing.Value, StringComparer.OrdinalIgnoreCase);
        }

        foreach (var value in values)
        {
            var normalized = value.NormalizeOptionalString();
            if (normalized is not { Length: > 0 })
                continue;

            seen ??= new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            if (!seen.Add(normalized))
                continue;

            list ??= [];
            list.Add(normalized);
        }

        return list?.ToEquatableImmutableArray() ?? EquatableImmutableArray<string>.Empty;
    }

    private static EquatableImmutableArray<T>? ConcatEquatable<T>(EquatableImmutableArray<T>? first, EquatableImmutableArray<T>? second)
    {
        if (first is not { Count: > 0 })
            return second;
        if (second is not { Count: > 0 })
            return first;

        var builder = ImmutableArray.CreateBuilder<T>(first.Value.Count + second.Value.Count);
        builder.AddRange(first.Value);
        builder.AddRange(second.Value);
        return builder.ToEquatableImmutableArray();
    }

    private static string? GetMapMethodSuffix(string httpMethod)
    {
        return httpMethod switch
        {
            "GET" => "Get",
            "POST" => "Post",
            "PUT" => "Put",
            "DELETE" => "Delete",
            "PATCH" => "Patch",
            _ => null,
        };
    }

    private static void AppendAdditionalContentTypes(StringBuilder source, EquatableImmutableArray<string>? additionalContentTypes)
    {
        if (additionalContentTypes is not { Count: > 0 })
            return;

        foreach (var additional in additionalContentTypes.Value)
        {
            source.Append(", ");
            source.Append(additional.ToStringLiteral());
        }
    }

    private static void AppendCommaSeparatedLiterals(StringBuilder source, EquatableImmutableArray<string> values)
    {
        if (values.Count == 0)
            return;

        source.Append(values[0]
            .ToStringLiteral()
        );
        for (var i = 1; i < values.Count; i++)
        {
            source.Append(", ");
            source.Append(values[i]
                .ToStringLiteral()
            );
        }
    }

    private static void AppendOptionalContentTypes(StringBuilder source, string? contentType, EquatableImmutableArray<string>? additionalContentTypes)
    {
        if (string.IsNullOrEmpty(contentType) && additionalContentTypes is not { Count: > 0 })
            return;

        source.Append(", ");
        source.Append(contentType is { Length: > 0 } ? contentType.ToStringLiteral() : "null");
        AppendAdditionalContentTypes(source, additionalContentTypes);
    }
}
