using Microsoft.CodeAnalysis;
using static GeneratedEndpoints.Common.Constants;

namespace GeneratedEndpoints.Common;

/// <summary>Provides extension methods for working with named type symbols.</summary>
internal static class NamedTypeSymbolExtensions
{
    public static RequestHandlerAttributeKind GetRequestHandlerAttributeKind(this INamedTypeSymbol definition)
    {
        if (AttributeSymbolMatcher.IsAttribute(definition, DisplayNameAttributeName, ComponentModelNamespaceParts))
            return RequestHandlerAttributeKind.DisplayName;

        if (AttributeSymbolMatcher.IsAttribute(definition, DescriptionAttributeName, ComponentModelNamespaceParts))
            return RequestHandlerAttributeKind.Description;

        if (AttributeSymbolMatcher.IsAttribute(definition, AllowAnonymousAttributeName, AspNetCoreAuthorizationNamespaceParts))
            return RequestHandlerAttributeKind.AllowAnonymous;

        if (AttributeSymbolMatcher.IsAttribute(definition, TagsAttributeName, AspNetCoreHttpNamespaceParts))
            return RequestHandlerAttributeKind.Tags;

        if (AttributeSymbolMatcher.IsAttribute(definition, ExcludeFromDescriptionAttributeName, AspNetCoreRoutingNamespaceParts))
            return RequestHandlerAttributeKind.ExcludeFromDescription;

        if (!AttributeSymbolMatcher.IsInNamespace(definition.ContainingNamespace, AttributesNamespaceParts))
            return RequestHandlerAttributeKind.None;

        return definition.Name switch
        {
            ShortCircuitAttributeName => RequestHandlerAttributeKind.ShortCircuit,
            DisableValidationAttributeName => RequestHandlerAttributeKind.DisableValidation,
            DisableRequestTimeoutAttributeName => RequestHandlerAttributeKind.DisableRequestTimeout,
            RequestTimeoutAttributeName => RequestHandlerAttributeKind.RequestTimeout,
            OrderAttributeName => RequestHandlerAttributeKind.Order,
            MapGroupAttributeName => RequestHandlerAttributeKind.MapGroup,
            SummaryAttributeName => RequestHandlerAttributeKind.Summary,
            AcceptsAttributeName => RequestHandlerAttributeKind.Accepts,
            ProducesResponseAttributeName => RequestHandlerAttributeKind.ProducesResponse,
            RequireAuthorizationAttributeName => RequestHandlerAttributeKind.RequireAuthorization,
            RequireCorsAttributeName => RequestHandlerAttributeKind.RequireCors,
            RequireHostAttributeName => RequestHandlerAttributeKind.RequireHost,
            RequireRateLimitingAttributeName => RequestHandlerAttributeKind.RequireRateLimiting,
            EndpointFilterAttributeName => RequestHandlerAttributeKind.EndpointFilter,
            DisableAntiforgeryAttributeName => RequestHandlerAttributeKind.DisableAntiforgery,
            ProducesProblemAttributeName => RequestHandlerAttributeKind.ProducesProblem,
            ProducesValidationProblemAttributeName => RequestHandlerAttributeKind.ProducesValidationProblem,
            _ => RequestHandlerAttributeKind.None,
        };
    }
}
