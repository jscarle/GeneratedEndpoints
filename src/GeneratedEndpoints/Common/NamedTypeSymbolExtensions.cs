using Microsoft.CodeAnalysis;
using static GeneratedEndpoints.Common.Constants;

namespace GeneratedEndpoints.Common;

/// <summary>Provides extension methods for working with named type symbols.</summary>
internal static class NamedTypeSymbolExtensions
{
    public static RequestHandlerAttributeKind GetRequestHandlerAttributeKind(this ITypeSymbol definition)
    {
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
                    ContainingNamespace: { Name: "AspNetCore", ContainingNamespace: { Name: "Microsoft", ContainingNamespace.IsGlobalNamespace: true }},
                },
            } => RequestHandlerAttributeKind.AllowAnonymous,
            {
                MetadataName: "TagsAttribute",
                ContainingNamespace:
                {
                    Name: "Http",
                    ContainingNamespace: { Name: "AspNetCore", ContainingNamespace: { Name: "Microsoft", ContainingNamespace.IsGlobalNamespace: true }},
                },
            } => RequestHandlerAttributeKind.Tags,
            {
                MetadataName: "ExcludeFromDescriptionAttribute",
                ContainingNamespace:
                {
                    Name: "Routing",
                    ContainingNamespace: { Name: "AspNetCore", ContainingNamespace: { Name: "Microsoft", ContainingNamespace.IsGlobalNamespace: true }},
                },
            } => RequestHandlerAttributeKind.ExcludeFromDescription,
            {
                MetadataName: "ExcludeFromDescriptionAttribute",
                ContainingNamespace:
                {
                    Name: "Attributes",
                    ContainingNamespace: { Name: "Generated",
                        ContainingNamespace: { Name: "AspNetCore", ContainingNamespace: { Name: "Microsoft", ContainingNamespace.IsGlobalNamespace: true }}},
                },
            } => RequestHandlerAttributeKind.ExcludeFromDescription,
            {
                MetadataName: "ShortCircuitAttribute",
                ContainingNamespace:
                {
                    Name: "Attributes",
                    ContainingNamespace: { Name: "Generated",
                        ContainingNamespace: { Name: "AspNetCore", ContainingNamespace: { Name: "Microsoft", ContainingNamespace.IsGlobalNamespace: true }}},
                },
            } => RequestHandlerAttributeKind.ShortCircuit,
            {
                MetadataName: "DisableValidationAttribute",
                ContainingNamespace:
                {
                    Name: "Attributes",
                    ContainingNamespace: { Name: "Generated",
                        ContainingNamespace: { Name: "AspNetCore", ContainingNamespace: { Name: "Microsoft", ContainingNamespace.IsGlobalNamespace: true }}},
                },
            } => RequestHandlerAttributeKind.DisableValidation,
            {
                MetadataName: "DisableRequestTimeoutAttribute",
                ContainingNamespace:
                {
                    Name: "Attributes",
                    ContainingNamespace: { Name: "Generated",
                        ContainingNamespace: { Name: "AspNetCore", ContainingNamespace: { Name: "Microsoft", ContainingNamespace.IsGlobalNamespace: true }}},
                },
            } => RequestHandlerAttributeKind.DisableRequestTimeout,
            {
                MetadataName: "RequestTimeoutAttribute",
                ContainingNamespace:
                {
                    Name: "Attributes",
                    ContainingNamespace: { Name: "Generated",
                        ContainingNamespace: { Name: "AspNetCore", ContainingNamespace: { Name: "Microsoft", ContainingNamespace.IsGlobalNamespace: true }}},
                },
            } => RequestHandlerAttributeKind.RequestTimeout,
            {
                MetadataName: "OrderAttribute",
                ContainingNamespace:
                {
                    Name: "Attributes",
                    ContainingNamespace: { Name: "Generated",
                        ContainingNamespace: { Name: "AspNetCore", ContainingNamespace: { Name: "Microsoft", ContainingNamespace.IsGlobalNamespace: true }}},
                },
            } => RequestHandlerAttributeKind.Order,
            {
                MetadataName: "MapGroupAttribute",
                ContainingNamespace:
                {
                    Name: "Attributes",
                    ContainingNamespace: { Name: "Generated",
                        ContainingNamespace: { Name: "AspNetCore", ContainingNamespace: { Name: "Microsoft", ContainingNamespace.IsGlobalNamespace: true }}},
                },
            } => RequestHandlerAttributeKind.MapGroup,
            {
                MetadataName: "SummaryAttribute",
                ContainingNamespace:
                {
                    Name: "Attributes",
                    ContainingNamespace: { Name: "Generated",
                        ContainingNamespace: { Name: "AspNetCore", ContainingNamespace: { Name: "Microsoft", ContainingNamespace.IsGlobalNamespace: true }}},
                },
            } => RequestHandlerAttributeKind.Summary,
            {
                MetadataName: "AcceptsAttribute",
                ContainingNamespace:
                {
                    Name: "Attributes",
                    ContainingNamespace: { Name: "Generated",
                        ContainingNamespace: { Name: "AspNetCore", ContainingNamespace: { Name: "Microsoft", ContainingNamespace.IsGlobalNamespace: true }}},
                },
            } => RequestHandlerAttributeKind.Accepts,
            {
                MetadataName: "AcceptsAttribute`1",
                ContainingNamespace:
                {
                    Name: "Attributes",
                    ContainingNamespace: { Name: "Generated",
                        ContainingNamespace: { Name: "AspNetCore", ContainingNamespace: { Name: "Microsoft", ContainingNamespace.IsGlobalNamespace: true }}},
                },
            } => RequestHandlerAttributeKind.Accepts,
            {
                MetadataName: "ProducesResponseAttribute",
                ContainingNamespace:
                {
                    Name: "Attributes",
                    ContainingNamespace: { Name: "Generated",
                        ContainingNamespace: { Name: "AspNetCore", ContainingNamespace: { Name: "Microsoft", ContainingNamespace.IsGlobalNamespace: true }}},
                },
            } => RequestHandlerAttributeKind.ProducesResponse,
            {
                MetadataName: "ProducesResponseAttribute`1",
                ContainingNamespace:
                {
                    Name: "Attributes",
                    ContainingNamespace: { Name: "Generated",
                        ContainingNamespace: { Name: "AspNetCore", ContainingNamespace: { Name: "Microsoft", ContainingNamespace.IsGlobalNamespace: true }}},
                },
            } => RequestHandlerAttributeKind.ProducesResponse,
            {
                MetadataName: "RequireAuthorizationAttribute",
                ContainingNamespace:
                {
                    Name: "Attributes",
                    ContainingNamespace: { Name: "Generated",
                        ContainingNamespace: { Name: "AspNetCore", ContainingNamespace: { Name: "Microsoft", ContainingNamespace.IsGlobalNamespace: true }}},
                },
            } => RequestHandlerAttributeKind.RequireAuthorization,
            {
                MetadataName: "RequireCorsAttribute",
                ContainingNamespace:
                {
                    Name: "Attributes",
                    ContainingNamespace: { Name: "Generated",
                        ContainingNamespace: { Name: "AspNetCore", ContainingNamespace: { Name: "Microsoft", ContainingNamespace.IsGlobalNamespace: true }}},
                },
            } => RequestHandlerAttributeKind.RequireCors,
            {
                MetadataName: "RequireHostAttribute",
                ContainingNamespace:
                {
                    Name: "Attributes",
                    ContainingNamespace: { Name: "Generated",
                        ContainingNamespace: { Name: "AspNetCore", ContainingNamespace: { Name: "Microsoft", ContainingNamespace.IsGlobalNamespace: true }}},
                },
            } => RequestHandlerAttributeKind.RequireHost,
            {
                MetadataName: "RequireRateLimitingAttribute",
                ContainingNamespace:
                {
                    Name: "Attributes",
                    ContainingNamespace: { Name: "Generated",
                        ContainingNamespace: { Name: "AspNetCore", ContainingNamespace: { Name: "Microsoft", ContainingNamespace.IsGlobalNamespace: true }}},
                },
            } => RequestHandlerAttributeKind.RequireRateLimiting,
            {
                MetadataName: "EndpointFilterAttribute",
                ContainingNamespace:
                {
                    Name: "Attributes",
                    ContainingNamespace: { Name: "Generated",
                        ContainingNamespace: { Name: "AspNetCore", ContainingNamespace: { Name: "Microsoft", ContainingNamespace.IsGlobalNamespace: true }}},
                },
            } => RequestHandlerAttributeKind.EndpointFilter,
            {
                MetadataName: "EndpointFilterAttribute`1",
                ContainingNamespace:
                {
                    Name: "Attributes",
                    ContainingNamespace: { Name: "Generated",
                        ContainingNamespace: { Name: "AspNetCore", ContainingNamespace: { Name: "Microsoft", ContainingNamespace.IsGlobalNamespace: true }}},
                },
            } => RequestHandlerAttributeKind.EndpointFilter,
            {
                MetadataName: "DisableAntiforgeryAttribute",
                ContainingNamespace:
                {
                    Name: "Attributes",
                    ContainingNamespace: { Name: "Generated",
                        ContainingNamespace: { Name: "AspNetCore", ContainingNamespace: { Name: "Microsoft", ContainingNamespace.IsGlobalNamespace: true }}},
                },
            } => RequestHandlerAttributeKind.DisableAntiforgery,
            {
                MetadataName: "ProducesProblemAttribute",
                ContainingNamespace:
                {
                    Name: "Attributes",
                    ContainingNamespace: { Name: "Generated",
                        ContainingNamespace: { Name: "AspNetCore", ContainingNamespace: { Name: "Microsoft", ContainingNamespace.IsGlobalNamespace: true }}},
                },
            } => RequestHandlerAttributeKind.ProducesProblem,
            {
                MetadataName: "ProducesValidationProblemAttribute",
                ContainingNamespace:
                {
                    Name: "Attributes",
                    ContainingNamespace: { Name: "Generated",
                        ContainingNamespace: { Name: "AspNetCore", ContainingNamespace: { Name: "Microsoft", ContainingNamespace.IsGlobalNamespace: true }}},
                },
            } => RequestHandlerAttributeKind.ProducesValidationProblem,
            _ => RequestHandlerAttributeKind.None,
        };
    }
}
