namespace GeneratedEndpoints.Common;

internal static partial class Constants
{
    internal const string FallbackHttpMethod = "__FALLBACK__";

    internal const string NameAttributeNamedParameter = "Name";
    internal const string IsOptionalAttributeNamedParameter = "IsOptional";

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

    private const string BaseNamespace = "Microsoft.AspNetCore.Generated";
    internal const string AttributesNamespace = $"{BaseNamespace}.Attributes";
    private const string AddEndpointHandlersMethodFullyQualifiedName = $"{RoutingNamespace}.{AddEndpointHandlersMethodName}";
    private const string UseEndpointHandlersMethodFullyQualifiedName = $"{RoutingNamespace}.{UseEndpointHandlersMethodName}";
}
