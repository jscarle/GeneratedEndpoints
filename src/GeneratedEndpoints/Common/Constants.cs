using System.ComponentModel;

namespace GeneratedEndpoints.Common;

internal static partial class Constants
{
    private const string BaseNamespace = "Microsoft.AspNetCore.Generated";
    private const string AttributesNamespace = $"{BaseNamespace}.Attributes";

    internal const string FallbackHttpMethod = "__FALLBACK__";

    internal const string NameAttributeNamedParameter = "Name";
    internal const string ResponseTypeAttributeNamedParameter = "ResponseType";
    internal const string RequestTypeAttributeNamedParameter = "RequestType";
    internal const string IsOptionalAttributeNamedParameter = "IsOptional";

    internal const string RequireAuthorizationAttributeName = "RequireAuthorizationAttribute";
    private const string RequireAuthorizationAttributeFullyQualifiedName = $"{AttributesNamespace}.{RequireAuthorizationAttributeName}";
    internal const string RequireAuthorizationAttributeHint = $"{RequireAuthorizationAttributeFullyQualifiedName}.gs.cs";

    internal const string RequireCorsAttributeName = "RequireCorsAttribute";
    private const string RequireCorsAttributeFullyQualifiedName = $"{AttributesNamespace}.{RequireCorsAttributeName}";
    internal const string RequireCorsAttributeHint = $"{RequireCorsAttributeFullyQualifiedName}.gs.cs";

    internal const string RequireRateLimitingAttributeName = "RequireRateLimitingAttribute";
    private const string RequireRateLimitingAttributeFullyQualifiedName = $"{AttributesNamespace}.{RequireRateLimitingAttributeName}";
    internal const string RequireRateLimitingAttributeHint = $"{RequireRateLimitingAttributeFullyQualifiedName}.gs.cs";

    internal const string RequireHostAttributeName = "RequireHostAttribute";
    private const string RequireHostAttributeFullyQualifiedName = $"{AttributesNamespace}.{RequireHostAttributeName}";
    internal const string RequireHostAttributeHint = $"{RequireHostAttributeFullyQualifiedName}.gs.cs";

    internal const string DisableAntiforgeryAttributeName = "DisableAntiforgeryAttribute";
    private const string DisableAntiforgeryAttributeFullyQualifiedName = $"{AttributesNamespace}.{DisableAntiforgeryAttributeName}";
    internal const string DisableAntiforgeryAttributeHint = $"{DisableAntiforgeryAttributeFullyQualifiedName}.gs.cs";

    internal const string ShortCircuitAttributeName = "ShortCircuitAttribute";
    private const string ShortCircuitAttributeFullyQualifiedName = $"{AttributesNamespace}.{ShortCircuitAttributeName}";
    internal const string ShortCircuitAttributeHint = $"{ShortCircuitAttributeFullyQualifiedName}.gs.cs";

    internal const string DisableRequestTimeoutAttributeName = "DisableRequestTimeoutAttribute";
    private const string DisableRequestTimeoutAttributeFullyQualifiedName = $"{AttributesNamespace}.{DisableRequestTimeoutAttributeName}";
    internal const string DisableRequestTimeoutAttributeHint = $"{DisableRequestTimeoutAttributeFullyQualifiedName}.gs.cs";

    internal const string DisableValidationAttributeName = "DisableValidationAttribute";
    private const string DisableValidationAttributeFullyQualifiedName = $"{AttributesNamespace}.{DisableValidationAttributeName}";
    internal const string DisableValidationAttributeHint = $"{DisableValidationAttributeFullyQualifiedName}.gs.cs";

    internal const string RequestTimeoutAttributeName = "RequestTimeoutAttribute";
    private const string RequestTimeoutAttributeFullyQualifiedName = $"{AttributesNamespace}.{RequestTimeoutAttributeName}";
    internal const string RequestTimeoutAttributeHint = $"{RequestTimeoutAttributeFullyQualifiedName}.gs.cs";

    internal const string OrderAttributeName = "OrderAttribute";
    private const string OrderAttributeFullyQualifiedName = $"{AttributesNamespace}.{OrderAttributeName}";
    internal const string OrderAttributeHint = $"{OrderAttributeFullyQualifiedName}.gs.cs";

    internal const string MapGroupAttributeName = "MapGroupAttribute";
    private const string MapGroupAttributeFullyQualifiedName = $"{AttributesNamespace}.{MapGroupAttributeName}";
    internal const string MapGroupAttributeHint = $"{MapGroupAttributeFullyQualifiedName}.gs.cs";

    internal const string SummaryAttributeName = "SummaryAttribute";
    private const string SummaryAttributeFullyQualifiedName = $"{AttributesNamespace}.{SummaryAttributeName}";
    internal const string SummaryAttributeHint = $"{SummaryAttributeFullyQualifiedName}.gs.cs";

    internal const string DisplayNameAttributeName = nameof(DisplayNameAttribute);
    internal const string DescriptionAttributeName = nameof(DescriptionAttribute);
    internal const string AllowAnonymousAttributeName = "AllowAnonymousAttribute";
    internal const string TagsAttributeName = "TagsAttribute";
    internal const string ExcludeFromDescriptionAttributeName = "ExcludeFromDescriptionAttribute";

    internal const string EndpointFilterAttributeName = "EndpointFilterAttribute";
    private const string EndpointFilterAttributeFullyQualifiedName = $"{AttributesNamespace}.{EndpointFilterAttributeName}";
    internal const string EndpointFilterAttributeHint = $"{EndpointFilterAttributeFullyQualifiedName}.gs.cs";

    internal const string AcceptsAttributeName = "AcceptsAttribute";
    private const string AcceptsAttributeFullyQualifiedName = $"{AttributesNamespace}.{AcceptsAttributeName}";
    internal const string AcceptsAttributeHint = $"{AcceptsAttributeFullyQualifiedName}.gs.cs";

    internal const string ProducesResponseAttributeName = "ProducesResponseAttribute";
    private const string ProducesResponseAttributeFullyQualifiedName = $"{AttributesNamespace}.{ProducesResponseAttributeName}";
    internal const string ProducesResponseAttributeHint = $"{ProducesResponseAttributeFullyQualifiedName}.gs.cs";

    internal const string ProducesProblemAttributeName = "ProducesProblemAttribute";
    private const string ProducesProblemAttributeFullyQualifiedName = $"{AttributesNamespace}.{ProducesProblemAttributeName}";
    internal const string ProducesProblemAttributeHint = $"{ProducesProblemAttributeFullyQualifiedName}.gs.cs";

    internal const string ProducesValidationProblemAttributeName = "ProducesValidationProblemAttribute";
    private const string ProducesValidationProblemAttributeFullyQualifiedName = $"{AttributesNamespace}.{ProducesValidationProblemAttributeName}";
    internal const string ProducesValidationProblemAttributeHint = $"{ProducesValidationProblemAttributeFullyQualifiedName}.gs.cs";

    internal const string RoutingNamespace = $"{BaseNamespace}.Routing";

    internal const string AddEndpointHandlersClassName = "EndpointServicesExtensions";
    internal const string AddEndpointHandlersMethodName = "AddEndpointHandlers";
    private const string AddEndpointHandlersMethodFullyQualifiedName = $"{RoutingNamespace}.{AddEndpointHandlersMethodName}";
    internal const string AddEndpointHandlersMethodHint = $"{AddEndpointHandlersMethodFullyQualifiedName}.g.cs";

    internal const string UseEndpointHandlersClassName = "EndpointRouteBuilderExtensions";
    internal const string UseEndpointHandlersMethodName = "MapEndpointHandlers";
    private const string UseEndpointHandlersMethodFullyQualifiedName = $"{RoutingNamespace}.{UseEndpointHandlersMethodName}";
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
}
