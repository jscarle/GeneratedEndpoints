using Microsoft.CodeAnalysis;

namespace GeneratedEndpoints.Common;

internal static class TypeSymbolExtensions
{
    public static RequestHandlerAttributeKind GetRequestHandlerAttributeKind(this ITypeSymbol symbol)
    {
        var definition = symbol.OriginalDefinition;
        return definition switch
        {
            {
                MetadataName: "DisplayNameAttribute",
                ContainingNamespace:
                {
                    Name: "ComponentModel",
                    ContainingNamespace: { Name: "System", ContainingNamespace.IsGlobalNamespace: true },
                },
            } => RequestHandlerAttributeKind.DisplayName,
            {
                MetadataName: "DescriptionAttribute",
                ContainingNamespace:
                {
                    Name: "ComponentModel",
                    ContainingNamespace: { Name: "System", ContainingNamespace.IsGlobalNamespace: true },
                },
            } => RequestHandlerAttributeKind.Description,
            {
                MetadataName: "AllowAnonymousAttribute",
                ContainingNamespace:
                {
                    Name: "Authorization",
                    ContainingNamespace: { Name: "AspNetCore", ContainingNamespace: { Name: "Microsoft", ContainingNamespace.IsGlobalNamespace: true } },
                },
            } => RequestHandlerAttributeKind.AllowAnonymous,
            {
                MetadataName: "TagsAttribute",
                ContainingNamespace:
                {
                    Name: "Http",
                    ContainingNamespace: { Name: "AspNetCore", ContainingNamespace: { Name: "Microsoft", ContainingNamespace.IsGlobalNamespace: true } },
                },
            } => RequestHandlerAttributeKind.Tags,
            {
                MetadataName: "ExcludeFromDescriptionAttribute",
                ContainingNamespace:
                {
                    Name: "Routing",
                    ContainingNamespace: { Name: "AspNetCore", ContainingNamespace: { Name: "Microsoft", ContainingNamespace.IsGlobalNamespace: true } },
                },
            } => RequestHandlerAttributeKind.ExcludeFromDescription,
            {
                MetadataName: "ExcludeFromDescriptionAttribute",
                ContainingNamespace:
                {
                    Name: "Attributes",
                    ContainingNamespace:
                    {
                        Name: "Generated",
                        ContainingNamespace: { Name: "AspNetCore", ContainingNamespace: { Name: "Microsoft", ContainingNamespace.IsGlobalNamespace: true } },
                    },
                },
            } => RequestHandlerAttributeKind.ExcludeFromDescription,
            {
                MetadataName: "ShortCircuitAttribute",
                ContainingNamespace:
                {
                    Name: "Attributes",
                    ContainingNamespace:
                    {
                        Name: "Generated",
                        ContainingNamespace: { Name: "AspNetCore", ContainingNamespace: { Name: "Microsoft", ContainingNamespace.IsGlobalNamespace: true } },
                    },
                },
            } => RequestHandlerAttributeKind.ShortCircuit,
            {
                MetadataName: "DisableValidationAttribute",
                ContainingNamespace:
                {
                    Name: "Attributes",
                    ContainingNamespace:
                    {
                        Name: "Generated",
                        ContainingNamespace: { Name: "AspNetCore", ContainingNamespace: { Name: "Microsoft", ContainingNamespace.IsGlobalNamespace: true } },
                    },
                },
            } => RequestHandlerAttributeKind.DisableValidation,
            {
                MetadataName: "DisableRequestTimeoutAttribute",
                ContainingNamespace:
                {
                    Name: "Attributes",
                    ContainingNamespace:
                    {
                        Name: "Generated",
                        ContainingNamespace: { Name: "AspNetCore", ContainingNamespace: { Name: "Microsoft", ContainingNamespace.IsGlobalNamespace: true } },
                    },
                },
            } => RequestHandlerAttributeKind.DisableRequestTimeout,
            {
                MetadataName: "RequestTimeoutAttribute",
                ContainingNamespace:
                {
                    Name: "Attributes",
                    ContainingNamespace:
                    {
                        Name: "Generated",
                        ContainingNamespace: { Name: "AspNetCore", ContainingNamespace: { Name: "Microsoft", ContainingNamespace.IsGlobalNamespace: true } },
                    },
                },
            } => RequestHandlerAttributeKind.RequestTimeout,
            {
                MetadataName: "OrderAttribute",
                ContainingNamespace:
                {
                    Name: "Attributes",
                    ContainingNamespace:
                    {
                        Name: "Generated",
                        ContainingNamespace: { Name: "AspNetCore", ContainingNamespace: { Name: "Microsoft", ContainingNamespace.IsGlobalNamespace: true } },
                    },
                },
            } => RequestHandlerAttributeKind.Order,
            {
                MetadataName: "MapGroupAttribute",
                ContainingNamespace:
                {
                    Name: "Attributes",
                    ContainingNamespace:
                    {
                        Name: "Generated",
                        ContainingNamespace: { Name: "AspNetCore", ContainingNamespace: { Name: "Microsoft", ContainingNamespace.IsGlobalNamespace: true } },
                    },
                },
            } => RequestHandlerAttributeKind.MapGroup,
            {
                MetadataName: "SummaryAttribute",
                ContainingNamespace:
                {
                    Name: "Attributes",
                    ContainingNamespace:
                    {
                        Name: "Generated",
                        ContainingNamespace: { Name: "AspNetCore", ContainingNamespace: { Name: "Microsoft", ContainingNamespace.IsGlobalNamespace: true } },
                    },
                },
            } => RequestHandlerAttributeKind.Summary,
            {
                MetadataName: "AcceptsAttribute",
                ContainingNamespace:
                {
                    Name: "Attributes",
                    ContainingNamespace:
                    {
                        Name: "Generated",
                        ContainingNamespace: { Name: "AspNetCore", ContainingNamespace: { Name: "Microsoft", ContainingNamespace.IsGlobalNamespace: true } },
                    },
                },
            } => RequestHandlerAttributeKind.Accepts,
            {
                MetadataName: "AcceptsAttribute`1",
                ContainingNamespace:
                {
                    Name: "Attributes",
                    ContainingNamespace:
                    {
                        Name: "Generated",
                        ContainingNamespace: { Name: "AspNetCore", ContainingNamespace: { Name: "Microsoft", ContainingNamespace.IsGlobalNamespace: true } },
                    },
                },
            } => RequestHandlerAttributeKind.Accepts,
            {
                MetadataName: "ProducesResponseAttribute",
                ContainingNamespace:
                {
                    Name: "Attributes",
                    ContainingNamespace:
                    {
                        Name: "Generated",
                        ContainingNamespace: { Name: "AspNetCore", ContainingNamespace: { Name: "Microsoft", ContainingNamespace.IsGlobalNamespace: true } },
                    },
                },
            } => RequestHandlerAttributeKind.ProducesResponse,
            {
                MetadataName: "ProducesResponseAttribute`1",
                ContainingNamespace:
                {
                    Name: "Attributes",
                    ContainingNamespace:
                    {
                        Name: "Generated",
                        ContainingNamespace: { Name: "AspNetCore", ContainingNamespace: { Name: "Microsoft", ContainingNamespace.IsGlobalNamespace: true } },
                    },
                },
            } => RequestHandlerAttributeKind.ProducesResponse,
            {
                MetadataName: "RequireAuthorizationAttribute",
                ContainingNamespace:
                {
                    Name: "Attributes",
                    ContainingNamespace:
                    {
                        Name: "Generated",
                        ContainingNamespace: { Name: "AspNetCore", ContainingNamespace: { Name: "Microsoft", ContainingNamespace.IsGlobalNamespace: true } },
                    },
                },
            } => RequestHandlerAttributeKind.RequireAuthorization,
            {
                MetadataName: "RequireCorsAttribute",
                ContainingNamespace:
                {
                    Name: "Attributes",
                    ContainingNamespace:
                    {
                        Name: "Generated",
                        ContainingNamespace: { Name: "AspNetCore", ContainingNamespace: { Name: "Microsoft", ContainingNamespace.IsGlobalNamespace: true } },
                    },
                },
            } => RequestHandlerAttributeKind.RequireCors,
            {
                MetadataName: "RequireHostAttribute",
                ContainingNamespace:
                {
                    Name: "Attributes",
                    ContainingNamespace:
                    {
                        Name: "Generated",
                        ContainingNamespace: { Name: "AspNetCore", ContainingNamespace: { Name: "Microsoft", ContainingNamespace.IsGlobalNamespace: true } },
                    },
                },
            } => RequestHandlerAttributeKind.RequireHost,
            {
                MetadataName: "RequireRateLimitingAttribute",
                ContainingNamespace:
                {
                    Name: "Attributes",
                    ContainingNamespace:
                    {
                        Name: "Generated",
                        ContainingNamespace: { Name: "AspNetCore", ContainingNamespace: { Name: "Microsoft", ContainingNamespace.IsGlobalNamespace: true } },
                    },
                },
            } => RequestHandlerAttributeKind.RequireRateLimiting,
            {
                MetadataName: "EndpointFilterAttribute",
                ContainingNamespace:
                {
                    Name: "Attributes",
                    ContainingNamespace:
                    {
                        Name: "Generated",
                        ContainingNamespace: { Name: "AspNetCore", ContainingNamespace: { Name: "Microsoft", ContainingNamespace.IsGlobalNamespace: true } },
                    },
                },
            } => RequestHandlerAttributeKind.EndpointFilter,
            {
                MetadataName: "EndpointFilterAttribute`1",
                ContainingNamespace:
                {
                    Name: "Attributes",
                    ContainingNamespace:
                    {
                        Name: "Generated",
                        ContainingNamespace: { Name: "AspNetCore", ContainingNamespace: { Name: "Microsoft", ContainingNamespace.IsGlobalNamespace: true } },
                    },
                },
            } => RequestHandlerAttributeKind.EndpointFilter,
            {
                MetadataName: "DisableAntiforgeryAttribute",
                ContainingNamespace:
                {
                    Name: "Attributes",
                    ContainingNamespace:
                    {
                        Name: "Generated",
                        ContainingNamespace: { Name: "AspNetCore", ContainingNamespace: { Name: "Microsoft", ContainingNamespace.IsGlobalNamespace: true } },
                    },
                },
            } => RequestHandlerAttributeKind.DisableAntiforgery,
            {
                MetadataName: "ProducesProblemAttribute",
                ContainingNamespace:
                {
                    Name: "Attributes",
                    ContainingNamespace:
                    {
                        Name: "Generated",
                        ContainingNamespace: { Name: "AspNetCore", ContainingNamespace: { Name: "Microsoft", ContainingNamespace.IsGlobalNamespace: true } },
                    },
                },
            } => RequestHandlerAttributeKind.ProducesProblem,
            {
                MetadataName: "ProducesValidationProblemAttribute",
                ContainingNamespace:
                {
                    Name: "Attributes",
                    ContainingNamespace:
                    {
                        Name: "Generated",
                        ContainingNamespace: { Name: "AspNetCore", ContainingNamespace: { Name: "Microsoft", ContainingNamespace.IsGlobalNamespace: true } },
                    },
                },
            } => RequestHandlerAttributeKind.ProducesValidationProblem,
            _ => RequestHandlerAttributeKind.None,
        };
    }

    public static BindingSource GetBindingSource(this ITypeSymbol symbol)
    {
        var definition = symbol.OriginalDefinition;
        return definition switch
        {
            {
                MetadataName: "FromRouteAttribute",
                ContainingNamespace:
                {
                    Name: "Mvc",
                    ContainingNamespace:
                    {
                        Name: "AspNetCore",
                        ContainingNamespace:
                        {
                            Name: "Microsoft",
                            ContainingNamespace.IsGlobalNamespace: true,
                        },
                    },
                },
            } => BindingSource.FromRoute,
            {
                MetadataName: "FromQueryAttribute",
                ContainingNamespace:
                {
                    Name: "Mvc",
                    ContainingNamespace:
                    {
                        Name: "AspNetCore",
                        ContainingNamespace:
                        {
                            Name: "Microsoft",
                            ContainingNamespace.IsGlobalNamespace: true,
                        },
                    },
                },
            } => BindingSource.FromQuery,
            {
                MetadataName: "FromHeaderAttribute",
                ContainingNamespace:
                {
                    Name: "Mvc",
                    ContainingNamespace:
                    {
                        Name: "AspNetCore",
                        ContainingNamespace:
                        {
                            Name: "Microsoft",
                            ContainingNamespace.IsGlobalNamespace: true,
                        },
                    },
                },
            } => BindingSource.FromHeader,
            {
                MetadataName: "FromBodyAttribute",
                ContainingNamespace:
                {
                    Name: "Mvc",
                    ContainingNamespace:
                    {
                        Name: "AspNetCore",
                        ContainingNamespace:
                        {
                            Name: "Microsoft",
                            ContainingNamespace.IsGlobalNamespace: true,
                        },
                    },
                },
            } => BindingSource.FromBody,
            {
                MetadataName: "FromFormAttribute",
                ContainingNamespace:
                {
                    Name: "Mvc",
                    ContainingNamespace:
                    {
                        Name: "AspNetCore",
                        ContainingNamespace:
                        {
                            Name: "Microsoft",
                            ContainingNamespace.IsGlobalNamespace: true,
                        },
                    },
                },
            } => BindingSource.FromForm,
            {
                MetadataName: "FromServicesAttribute",
                ContainingNamespace:
                {
                    Name: "Mvc",
                    ContainingNamespace:
                    {
                        Name: "AspNetCore",
                        ContainingNamespace:
                        {
                            Name: "Microsoft",
                            ContainingNamespace.IsGlobalNamespace: true,
                        },
                    },
                },
            } => BindingSource.FromServices,
            {
                MetadataName: "FromKeyedServicesAttribute",
                ContainingNamespace:
                {
                    Name: "DependencyInjection",
                    ContainingNamespace:
                    {
                        Name: "Extensions",
                        ContainingNamespace:
                        {
                            Name: "Microsoft",
                            ContainingNamespace.IsGlobalNamespace: true,
                        },
                    },
                },
            } => BindingSource.FromKeyedServices,
            {
                MetadataName: "AsParametersAttribute",
                ContainingNamespace:
                {
                    Name: "Http",
                    ContainingNamespace:
                    {
                        Name: "AspNetCore",
                        ContainingNamespace:
                        {
                            Name: "Microsoft",
                            ContainingNamespace.IsGlobalNamespace: true,
                        },
                    },
                },
            } => BindingSource.AsParameters,
            _ => BindingSource.None,
        };
    }

    public static bool IsIEndpointConventionBuilder(this ITypeSymbol symbol)
    {
        return symbol is
        {
            MetadataName: "IEndpointConventionBuilder",
            ContainingNamespace:
            {
                Name: "Builder",
                ContainingNamespace: { Name: "AspNetCore", ContainingNamespace: { Name: "Microsoft", ContainingNamespace.IsGlobalNamespace: true } },
            },
        };
    }

    public static bool IsIServiceProvider(this ITypeSymbol symbol)
    {
        return symbol is
        {
            MetadataName: "IServiceProvider",
            ContainingNamespace:
            {
                Name: "System", ContainingNamespace.IsGlobalNamespace: true,
            },
        };
    }

    public static bool IsAwaitable(this ITypeSymbol symbol)
    {
        return symbol switch
        {
            {
                    MetadataName: "ValueTask`1",
                    ContainingNamespace:
                    {
                        Name: "Tasks",
                        ContainingNamespace: { Name: "Threading", ContainingNamespace: { Name: "System", ContainingNamespace.IsGlobalNamespace: true } },
                    },
                }
                or
                {
                    MetadataName: "Task`1",
                    ContainingNamespace:
                    {
                        Name: "Tasks",
                        ContainingNamespace: { Name: "Threading", ContainingNamespace: { Name: "System", ContainingNamespace.IsGlobalNamespace: true } },
                    },
                }
                or
                {
                    MetadataName: "ValueTask",
                    ContainingNamespace:
                    {
                        Name: "Tasks",
                        ContainingNamespace: { Name: "Threading", ContainingNamespace: { Name: "System", ContainingNamespace.IsGlobalNamespace: true } },
                    },
                }
                or
                {
                    MetadataName: "Task",
                    ContainingNamespace:
                    {
                        Name: "Tasks",
                        ContainingNamespace: { Name: "Threading", ContainingNamespace: { Name: "System", ContainingNamespace.IsGlobalNamespace: true } },
                    },
                } => true,
            _ => false,
        };
    }
}
