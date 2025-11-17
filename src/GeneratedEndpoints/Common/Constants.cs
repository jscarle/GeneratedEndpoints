using System.ComponentModel;

namespace GeneratedEndpoints.Common;

internal static partial class Constants
{
    internal const string FallbackHttpMethod = "__FALLBACK__";

    internal const string NameAttributeNamedParameter = "Name";
    internal const string ResponseTypeAttributeNamedParameter = "ResponseType";
    internal const string RequestTypeAttributeNamedParameter = "RequestType";
    internal const string IsOptionalAttributeNamedParameter = "IsOptional";

    internal const string RequireAuthorizationAttributeName = "RequireAuthorizationAttribute";
    internal const string RequireAuthorizationAttributeHint = $"{RequireAuthorizationAttributeFullyQualifiedName}.gs.cs";

    internal const string RequireCorsAttributeName = "RequireCorsAttribute";
    internal const string RequireCorsAttributeHint = $"{RequireCorsAttributeFullyQualifiedName}.gs.cs";

    internal const string RequireRateLimitingAttributeName = "RequireRateLimitingAttribute";
    internal const string RequireRateLimitingAttributeHint = $"{RequireRateLimitingAttributeFullyQualifiedName}.gs.cs";

    internal const string RequireHostAttributeName = "RequireHostAttribute";
    internal const string RequireHostAttributeHint = $"{RequireHostAttributeFullyQualifiedName}.gs.cs";

    internal const string DisableAntiforgeryAttributeName = "DisableAntiforgeryAttribute";
    internal const string DisableAntiforgeryAttributeHint = $"{DisableAntiforgeryAttributeFullyQualifiedName}.gs.cs";

    internal const string ShortCircuitAttributeName = "ShortCircuitAttribute";
    internal const string ShortCircuitAttributeHint = $"{ShortCircuitAttributeFullyQualifiedName}.gs.cs";

    internal const string DisableRequestTimeoutAttributeName = "DisableRequestTimeoutAttribute";
    internal const string DisableRequestTimeoutAttributeHint = $"{DisableRequestTimeoutAttributeFullyQualifiedName}.gs.cs";

    internal const string DisableValidationAttributeName = "DisableValidationAttribute";
    internal const string DisableValidationAttributeHint = $"{DisableValidationAttributeFullyQualifiedName}.gs.cs";

    internal const string RequestTimeoutAttributeName = "RequestTimeoutAttribute";
    internal const string RequestTimeoutAttributeHint = $"{RequestTimeoutAttributeFullyQualifiedName}.gs.cs";

    internal const string OrderAttributeName = "OrderAttribute";
    internal const string OrderAttributeHint = $"{OrderAttributeFullyQualifiedName}.gs.cs";

    internal const string MapGroupAttributeName = "MapGroupAttribute";
    internal const string MapGroupAttributeHint = $"{MapGroupAttributeFullyQualifiedName}.gs.cs";

    internal const string SummaryAttributeName = "SummaryAttribute";
    internal const string SummaryAttributeHint = $"{SummaryAttributeFullyQualifiedName}.gs.cs";

    internal const string DisplayNameAttributeName = nameof(DisplayNameAttribute);
    internal const string DescriptionAttributeName = nameof(DescriptionAttribute);
    internal const string AllowAnonymousAttributeName = "AllowAnonymousAttribute";
    internal const string TagsAttributeName = "TagsAttribute";
    internal const string ExcludeFromDescriptionAttributeName = "ExcludeFromDescriptionAttribute";

    internal const string EndpointFilterAttributeName = "EndpointFilterAttribute";
    internal const string EndpointFilterAttributeHint = $"{EndpointFilterAttributeFullyQualifiedName}.gs.cs";

    internal const string AcceptsAttributeName = "AcceptsAttribute";
    internal const string AcceptsAttributeHint = $"{AcceptsAttributeFullyQualifiedName}.gs.cs";

    internal const string ProducesResponseAttributeName = "ProducesResponseAttribute";
    internal const string ProducesResponseAttributeHint = $"{ProducesResponseAttributeFullyQualifiedName}.gs.cs";

    internal const string ProducesProblemAttributeName = "ProducesProblemAttribute";
    internal const string ProducesProblemAttributeHint = $"{ProducesProblemAttributeFullyQualifiedName}.gs.cs";

    internal const string ProducesValidationProblemAttributeName = "ProducesValidationProblemAttribute";
    internal const string ProducesValidationProblemAttributeHint = $"{ProducesValidationProblemAttributeFullyQualifiedName}.gs.cs";

    internal const string RoutingNamespace = $"{BaseNamespace}.Routing";

    internal const string AddEndpointHandlersClassName = "EndpointServicesExtensions";
    internal const string AddEndpointHandlersMethodName = "AddEndpointHandlers";
    internal const string AddEndpointHandlersMethodHint = $"{AddEndpointHandlersMethodFullyQualifiedName}.g.cs";

    internal const string UseEndpointHandlersClassName = "EndpointRouteBuilderExtensions";
    internal const string UseEndpointHandlersMethodName = "MapEndpointHandlers";
    internal const string UseEndpointHandlersMethodHint = $"{UseEndpointHandlersMethodFullyQualifiedName}.g.cs";

    internal const string ConfigureMethodName = "Configure";
    internal const string AsyncSuffix = "Async";
    internal const string ApplicationJsonContentType = "application/json";
    internal const string GlobalPrefix = "global::";
    internal const string Dot = ".";

    internal static readonly string[] AttributesNamespaceParts = AttributesNamespace.Split('.');
    internal static readonly string[] AspNetCoreHttpNamespaceParts = ["Microsoft", "AspNetCore", "Http"];
    internal static readonly string[] AspNetCoreMvcNamespaceParts = ["Microsoft", "AspNetCore", "Mvc"];
    internal static readonly string[] AspNetCoreAuthorizationNamespaceParts = ["Microsoft", "AspNetCore", "Authorization"];
    internal static readonly string[] AspNetCoreRoutingNamespaceParts = ["Microsoft", "AspNetCore", "Routing"];
    internal static readonly string[] ExtensionsDependencyInjectionNamespaceParts = ["Microsoft", "Extensions", "DependencyInjection"];
    internal static readonly string[] ComponentModelNamespaceParts = ["System", "ComponentModel"];
    private const string BaseNamespace = "Microsoft.AspNetCore.Generated";
    private const string AttributesNamespace = $"{BaseNamespace}.Attributes";
    private const string RequireAuthorizationAttributeFullyQualifiedName = $"{AttributesNamespace}.{RequireAuthorizationAttributeName}";
    private const string RequireCorsAttributeFullyQualifiedName = $"{AttributesNamespace}.{RequireCorsAttributeName}";
    private const string RequireRateLimitingAttributeFullyQualifiedName = $"{AttributesNamespace}.{RequireRateLimitingAttributeName}";
    private const string RequireHostAttributeFullyQualifiedName = $"{AttributesNamespace}.{RequireHostAttributeName}";
    private const string DisableAntiforgeryAttributeFullyQualifiedName = $"{AttributesNamespace}.{DisableAntiforgeryAttributeName}";
    private const string ShortCircuitAttributeFullyQualifiedName = $"{AttributesNamespace}.{ShortCircuitAttributeName}";
    private const string DisableRequestTimeoutAttributeFullyQualifiedName = $"{AttributesNamespace}.{DisableRequestTimeoutAttributeName}";
    private const string DisableValidationAttributeFullyQualifiedName = $"{AttributesNamespace}.{DisableValidationAttributeName}";
    private const string RequestTimeoutAttributeFullyQualifiedName = $"{AttributesNamespace}.{RequestTimeoutAttributeName}";
    private const string OrderAttributeFullyQualifiedName = $"{AttributesNamespace}.{OrderAttributeName}";
    private const string MapGroupAttributeFullyQualifiedName = $"{AttributesNamespace}.{MapGroupAttributeName}";
    private const string SummaryAttributeFullyQualifiedName = $"{AttributesNamespace}.{SummaryAttributeName}";
    private const string EndpointFilterAttributeFullyQualifiedName = $"{AttributesNamespace}.{EndpointFilterAttributeName}";
    private const string AcceptsAttributeFullyQualifiedName = $"{AttributesNamespace}.{AcceptsAttributeName}";
    private const string ProducesResponseAttributeFullyQualifiedName = $"{AttributesNamespace}.{ProducesResponseAttributeName}";
    private const string ProducesProblemAttributeFullyQualifiedName = $"{AttributesNamespace}.{ProducesProblemAttributeName}";
    private const string ProducesValidationProblemAttributeFullyQualifiedName = $"{AttributesNamespace}.{ProducesValidationProblemAttributeName}";
    private const string AddEndpointHandlersMethodFullyQualifiedName = $"{RoutingNamespace}.{AddEndpointHandlersMethodName}";
    private const string UseEndpointHandlersMethodFullyQualifiedName = $"{RoutingNamespace}.{UseEndpointHandlersMethodName}";
}
