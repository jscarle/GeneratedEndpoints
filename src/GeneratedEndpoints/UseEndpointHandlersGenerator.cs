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
    public static void GenerateSource(SourceProductionContext context, ImmutableArray<RequestHandler> requestHandlers)
    {
        context.CancellationToken.ThrowIfCancellationRequested();

        var source = GetUseEndpointHandlersStringBuilder(requestHandlers);
        source.AppendLine(FileHeader);

        source.AppendLine();

        source.AppendLine("using Microsoft.AspNetCore.Builder;");
        source.AppendLine("using Microsoft.AspNetCore.Http;");
        source.AppendLine("using Microsoft.AspNetCore.Mvc;");
        source.AppendLine("using Microsoft.AspNetCore.Routing;");
        if (HasRateLimitedHandlers(requestHandlers))
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

        var groupedClasses = GetClassesWithMapGroups(requestHandlers);

        for (var index = 0; index < groupedClasses.Count; index++)
        {
            var groupedClass = groupedClasses[index];
            source.Append("        var ");
            source.Append(groupedClass.Configuration.GroupIdentifier);
            source.Append(" = builder.MapGroup(");
            source.Append(groupedClass.Configuration.GroupPattern!.ToStringLiteral());
            source.Append(')');
            AppendEndpointConfiguration(source, "            ", groupedClass.Configuration);
            source.AppendLine(";");
        }

        if (groupedClasses.Count > 0)
            source.AppendLine();

        for (var index = 0; index < requestHandlers.Length; index++)
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

        private static bool HasRateLimitedHandlers(ImmutableArray<RequestHandler> requestHandlers)
    {
        for (var index = 0; index < requestHandlers.Length; index++)
        {
            var handler = requestHandlers[index];
            if (handler.Method.Configuration.RequireRateLimiting)
                return true;
        }

        return false;
    }

    private static List<RequestHandlerClass> GetClassesWithMapGroups(ImmutableArray<RequestHandler> requestHandlers)
    {
        var groupedClasses = new List<RequestHandlerClass>();
        if (requestHandlers.IsDefaultOrEmpty)
            return groupedClasses;

        var seen = new HashSet<string>(StringComparer.Ordinal);
        for (var index = 0; index < requestHandlers.Length; index++)
        {
            var handler = requestHandlers[index];
            var handlerClass = handler.Class;
            if (handlerClass.Configuration.GroupPattern is null)
                continue;

            if (seen.Add(handlerClass.Name))
                groupedClasses.Add(handlerClass);
        }

        return groupedClasses;
    }

    private static void GenerateMapRequestHandler(StringBuilder source, RequestHandler requestHandler)
    {
        var wrapWithConfigure = requestHandler.Class.HasConfigureMethod;
        var configureAcceptsServiceProvider = requestHandler.Class.ConfigureMethodAcceptsServiceProvider;
        var indent = wrapWithConfigure ? "            " : "        ";
        var continuationIndent = indent + "    ";
        var routeBuilderIdentifier = requestHandler.Class.Configuration.GroupIdentifier ?? "builder";

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
        if (requestHandler.Class.Configuration.GroupPattern is null)
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

        if (!string.IsNullOrEmpty(configuration.GroupName))
        {
            source.AppendLine();
            source.Append(indent);
            source.Append(".WithGroupName(");
            source.Append(configuration.GroupName.ToStringLiteral());
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
        var tags = MergeDistinctStrings(classConfiguration.Tags, methodConfiguration.Tags);
        var accepts = ConcatEquatable(classConfiguration.Accepts, methodConfiguration.Accepts);
        var produces = ConcatEquatable(classConfiguration.Produces, methodConfiguration.Produces);
        var producesProblem = ConcatEquatable(classConfiguration.ProducesProblem, methodConfiguration.ProducesProblem);
        var producesValidationProblem = ConcatEquatable(classConfiguration.ProducesValidationProblem, methodConfiguration.ProducesValidationProblem);
        var excludeFromDescription = classConfiguration.ExcludeFromDescription || methodConfiguration.ExcludeFromDescription;
        var authorizationPolicies = MergeDistinctStrings(classConfiguration.AuthorizationPolicies, methodConfiguration.AuthorizationPolicies);
        var requiredHosts = MergeDistinctStrings(classConfiguration.RequiredHosts, methodConfiguration.RequiredHosts);
        var endpointFilterTypes = ConcatEquatable(classConfiguration.EndpointFilterTypes, methodConfiguration.EndpointFilterTypes);
        var requireAuthorization = classConfiguration.RequireAuthorization || methodConfiguration.RequireAuthorization;
        var disableAntiforgery = classConfiguration.DisableAntiforgery || methodConfiguration.DisableAntiforgery;
        var allowAnonymous = classConfiguration.AllowAnonymous || methodConfiguration.AllowAnonymous;
        var requireCors = classConfiguration.RequireCors || methodConfiguration.RequireCors;
        var corsPolicyName = methodConfiguration.CorsPolicyName ?? classConfiguration.CorsPolicyName;
        var requireRateLimiting = classConfiguration.RequireRateLimiting || methodConfiguration.RequireRateLimiting;
        var rateLimitingPolicyName = methodConfiguration.RateLimitingPolicyName ?? classConfiguration.RateLimitingPolicyName;
        var shortCircuit = classConfiguration.ShortCircuit || methodConfiguration.ShortCircuit;
        var disableValidation = classConfiguration.DisableValidation || methodConfiguration.DisableValidation;
        var disableRequestTimeout = classConfiguration.DisableRequestTimeout || methodConfiguration.DisableRequestTimeout;
        var withRequestTimeout = classConfiguration.WithRequestTimeout || methodConfiguration.WithRequestTimeout;
        var groupIdentifier = classConfiguration.GroupIdentifier ?? methodConfiguration.GroupIdentifier;
        var groupPattern = classConfiguration.GroupPattern ?? methodConfiguration.GroupPattern;
        var groupName = classConfiguration.GroupName ?? methodConfiguration.GroupName;

        string? requestTimeoutPolicyName = null;
        if (methodConfiguration.WithRequestTimeout)
            requestTimeoutPolicyName = methodConfiguration.RequestTimeoutPolicyName;
        else if (classConfiguration.WithRequestTimeout)
            requestTimeoutPolicyName = classConfiguration.RequestTimeoutPolicyName;

        if (disableRequestTimeout)
        {
            withRequestTimeout = false;
            requestTimeoutPolicyName = null;
        }

        var order = methodConfiguration.Order ?? classConfiguration.Order;

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
            GroupIdentifier = groupIdentifier,
            GroupPattern = groupPattern,
            GroupName = groupName,
        };
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

    private static StringBuilder GetUseEndpointHandlersStringBuilder(ImmutableArray<RequestHandler> requestHandlers)
    {
        const int baseSize = 4096;
        const int perHandler = 512;

        var handlerCount = Math.Max(requestHandlers.Length, 0);
        var estimate = baseSize + (long)perHandler * handlerCount;
        estimate = (long)(estimate * 1.10);

        if (estimate > int.MaxValue)
            estimate = int.MaxValue;

        return StringBuilderPool.Get((int)Math.Max(baseSize, estimate));
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
