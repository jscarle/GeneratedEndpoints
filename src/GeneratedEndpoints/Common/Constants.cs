using System.ComponentModel;

namespace GeneratedEndpoints.Common;

internal static partial class Constants
{
    internal const string BaseNamespace = "Microsoft.AspNetCore.Generated";
    internal const string AttributesNamespace = $"{BaseNamespace}.Attributes";

    internal const string FallbackHttpMethod = "__FALLBACK__";

    internal const string NameAttributeNamedParameter = "Name";
    internal const string ResponseTypeAttributeNamedParameter = "ResponseType";
    internal const string RequestTypeAttributeNamedParameter = "RequestType";
    internal const string IsOptionalAttributeNamedParameter = "IsOptional";
    internal const string PolicyNameAttributeNamedParameter = "PolicyName";

    internal const string RequireAuthorizationAttributeName = "RequireAuthorizationAttribute";
    internal const string RequireAuthorizationAttributeFullyQualifiedName = $"{AttributesNamespace}.{RequireAuthorizationAttributeName}";
    internal const string RequireAuthorizationAttributeHint = $"{RequireAuthorizationAttributeFullyQualifiedName}.gs.cs";

    internal const string RequireCorsAttributeName = "RequireCorsAttribute";
    internal const string RequireCorsAttributeFullyQualifiedName = $"{AttributesNamespace}.{RequireCorsAttributeName}";
    internal const string RequireCorsAttributeHint = $"{RequireCorsAttributeFullyQualifiedName}.gs.cs";

    internal const string RequireRateLimitingAttributeName = "RequireRateLimitingAttribute";
    internal const string RequireRateLimitingAttributeFullyQualifiedName = $"{AttributesNamespace}.{RequireRateLimitingAttributeName}";
    internal const string RequireRateLimitingAttributeHint = $"{RequireRateLimitingAttributeFullyQualifiedName}.gs.cs";

    internal const string RequireHostAttributeName = "RequireHostAttribute";
    internal const string RequireHostAttributeFullyQualifiedName = $"{AttributesNamespace}.{RequireHostAttributeName}";
    internal const string RequireHostAttributeHint = $"{RequireHostAttributeFullyQualifiedName}.gs.cs";

    internal const string DisableAntiforgeryAttributeName = "DisableAntiforgeryAttribute";
    internal const string DisableAntiforgeryAttributeFullyQualifiedName = $"{AttributesNamespace}.{DisableAntiforgeryAttributeName}";
    internal const string DisableAntiforgeryAttributeHint = $"{DisableAntiforgeryAttributeFullyQualifiedName}.gs.cs";

    internal const string ShortCircuitAttributeName = "ShortCircuitAttribute";
    internal const string ShortCircuitAttributeFullyQualifiedName = $"{AttributesNamespace}.{ShortCircuitAttributeName}";
    internal const string ShortCircuitAttributeHint = $"{ShortCircuitAttributeFullyQualifiedName}.gs.cs";

    internal const string DisableRequestTimeoutAttributeName = "DisableRequestTimeoutAttribute";
    internal const string DisableRequestTimeoutAttributeFullyQualifiedName = $"{AttributesNamespace}.{DisableRequestTimeoutAttributeName}";
    internal const string DisableRequestTimeoutAttributeHint = $"{DisableRequestTimeoutAttributeFullyQualifiedName}.gs.cs";

    internal const string DisableValidationAttributeName = "DisableValidationAttribute";
    internal const string DisableValidationAttributeFullyQualifiedName = $"{AttributesNamespace}.{DisableValidationAttributeName}";
    internal const string DisableValidationAttributeHint = $"{DisableValidationAttributeFullyQualifiedName}.gs.cs";

    internal const string RequestTimeoutAttributeName = "RequestTimeoutAttribute";
    internal const string RequestTimeoutAttributeFullyQualifiedName = $"{AttributesNamespace}.{RequestTimeoutAttributeName}";
    internal const string RequestTimeoutAttributeHint = $"{RequestTimeoutAttributeFullyQualifiedName}.gs.cs";

    internal const string OrderAttributeName = "OrderAttribute";
    internal const string OrderAttributeFullyQualifiedName = $"{AttributesNamespace}.{OrderAttributeName}";
    internal const string OrderAttributeHint = $"{OrderAttributeFullyQualifiedName}.gs.cs";

    internal const string MapGroupAttributeName = "MapGroupAttribute";
    internal const string MapGroupAttributeFullyQualifiedName = $"{AttributesNamespace}.{MapGroupAttributeName}";
    internal const string MapGroupAttributeHint = $"{MapGroupAttributeFullyQualifiedName}.gs.cs";

    internal const string SummaryAttributeName = "SummaryAttribute";
    internal const string SummaryAttributeFullyQualifiedName = $"{AttributesNamespace}.{SummaryAttributeName}";
    internal const string SummaryAttributeHint = $"{SummaryAttributeFullyQualifiedName}.gs.cs";

    internal const string DisplayNameAttributeName = nameof(DisplayNameAttribute);
    internal const string DescriptionAttributeName = nameof(DescriptionAttribute);
    internal const string AllowAnonymousAttributeName = "AllowAnonymousAttribute";
    internal const string TagsAttributeName = "TagsAttribute";
    internal const string ExcludeFromDescriptionAttributeName = "ExcludeFromDescriptionAttribute";

    internal const string EndpointFilterAttributeName = "EndpointFilterAttribute";
    internal const string EndpointFilterAttributeFullyQualifiedName = $"{AttributesNamespace}.{EndpointFilterAttributeName}";
    internal const string EndpointFilterAttributeHint = $"{EndpointFilterAttributeFullyQualifiedName}.gs.cs";

    internal const string AcceptsAttributeName = "AcceptsAttribute";
    internal const string AcceptsAttributeFullyQualifiedName = $"{AttributesNamespace}.{AcceptsAttributeName}";
    internal const string AcceptsAttributeHint = $"{AcceptsAttributeFullyQualifiedName}.gs.cs";

    internal const string ProducesResponseAttributeName = "ProducesResponseAttribute";
    internal const string ProducesResponseAttributeFullyQualifiedName = $"{AttributesNamespace}.{ProducesResponseAttributeName}";
    internal const string ProducesResponseAttributeHint = $"{ProducesResponseAttributeFullyQualifiedName}.gs.cs";

    internal const string ProducesProblemAttributeName = "ProducesProblemAttribute";
    internal const string ProducesProblemAttributeFullyQualifiedName = $"{AttributesNamespace}.{ProducesProblemAttributeName}";
    internal const string ProducesProblemAttributeHint = $"{ProducesProblemAttributeFullyQualifiedName}.gs.cs";

    internal const string ProducesValidationProblemAttributeName = "ProducesValidationProblemAttribute";
    internal const string ProducesValidationProblemAttributeFullyQualifiedName = $"{AttributesNamespace}.{ProducesValidationProblemAttributeName}";
    internal const string ProducesValidationProblemAttributeHint = $"{ProducesValidationProblemAttributeFullyQualifiedName}.gs.cs";

    internal const string RoutingNamespace = $"{BaseNamespace}.Routing";

    internal const string AddEndpointHandlersClassName = "EndpointServicesExtensions";
    internal const string AddEndpointHandlersMethodName = "AddEndpointHandlers";
    internal const string AddEndpointHandlersMethodHint = $"{RoutingNamespace}.{AddEndpointHandlersMethodName}.g.cs";

    internal const string UseEndpointHandlersClassName = "EndpointRouteBuilderExtensions";
    internal const string UseEndpointHandlersMethodName = "MapEndpointHandlers";
    internal const string UseEndpointHandlersMethodHint = $"{RoutingNamespace}.{UseEndpointHandlersMethodName}.g.cs";

    internal const string ConfigureMethodName = "Configure";
    internal const string AsyncSuffix = "Async";
    internal const string GlobalPrefix = "global::";

    internal static readonly string[] AttributesNamespaceParts = AttributesNamespace.Split('.');
    internal static readonly string[] AspNetCoreHttpNamespaceParts = ["Microsoft", "AspNetCore", "Http"];
    internal static readonly string[] AspNetCoreMvcNamespaceParts = ["Microsoft", "AspNetCore", "Mvc"];
    internal static readonly string[] AspNetCoreAuthorizationNamespaceParts = ["Microsoft", "AspNetCore", "Authorization"];
    internal static readonly string[] AspNetCoreRoutingNamespaceParts = ["Microsoft", "AspNetCore", "Routing"];
    internal static readonly string[] ExtensionsDependencyInjectionNamespaceParts = ["Microsoft", "Extensions", "DependencyInjection"];
    internal static readonly string[] ComponentModelNamespaceParts = ["System", "ComponentModel"];
}
