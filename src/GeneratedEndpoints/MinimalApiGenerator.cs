using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text;
using GeneratedEndpoints.Common;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace GeneratedEndpoints;

[Generator]
public sealed class MinimalApiGenerator : IIncrementalGenerator
{
    private const string BaseNamespace = "Microsoft.AspNetCore.Generated";
    private const string AttributesNamespace = $"{BaseNamespace}.Attributes";

    private const string MapGetAttributeName = "MapGetAttribute";
    private const string MapGetAttributeFullyQualifiedName = $"{AttributesNamespace}.{MapGetAttributeName}";
    private const string MapGetAttributeHint = $"{MapGetAttributeFullyQualifiedName}.gs.cs";

    private const string MapPostAttributeName = "MapPostAttribute";
    private const string MapPostAttributeFullyQualifiedName = $"{AttributesNamespace}.{MapPostAttributeName}";
    private const string MapPostAttributeHint = $"{MapPostAttributeFullyQualifiedName}.gs.cs";

    private const string MapPutAttributeName = "MapPutAttribute";
    private const string MapPutAttributeFullyQualifiedName = $"{AttributesNamespace}.{MapPutAttributeName}";
    private const string MapPutAttributeHint = $"{MapPutAttributeFullyQualifiedName}.gs.cs";

    private const string MapDeleteAttributeName = "MapDeleteAttribute";
    private const string MapDeleteAttributeFullyQualifiedName = $"{AttributesNamespace}.{MapDeleteAttributeName}";
    private const string MapDeleteAttributeHint = $"{MapDeleteAttributeFullyQualifiedName}.gs.cs";

    private const string MapOptionsAttributeName = "MapOptionsAttribute";
    private const string MapOptionsAttributeFullyQualifiedName = $"{AttributesNamespace}.{MapOptionsAttributeName}";
    private const string MapOptionsAttributeHint = $"{MapOptionsAttributeFullyQualifiedName}.gs.cs";

    private const string MapHeadAttributeName = "MapHeadAttribute";
    private const string MapHeadAttributeFullyQualifiedName = $"{AttributesNamespace}.{MapHeadAttributeName}";
    private const string MapHeadAttributeHint = $"{MapHeadAttributeFullyQualifiedName}.gs.cs";

    private const string MapPatchAttributeName = "MapPatchAttribute";
    private const string MapPatchAttributeFullyQualifiedName = $"{AttributesNamespace}.{MapPatchAttributeName}";
    private const string MapPatchAttributeHint = $"{MapPatchAttributeFullyQualifiedName}.gs.cs";

    private const string MapQueryAttributeName = "MapQueryAttribute";
    private const string MapQueryAttributeFullyQualifiedName = $"{AttributesNamespace}.{MapQueryAttributeName}";
    private const string MapQueryAttributeHint = $"{MapQueryAttributeFullyQualifiedName}.gs.cs";

    private const string MapTraceAttributeName = "MapTraceAttribute";
    private const string MapTraceAttributeFullyQualifiedName = $"{AttributesNamespace}.{MapTraceAttributeName}";
    private const string MapTraceAttributeHint = $"{MapTraceAttributeFullyQualifiedName}.gs.cs";

    private const string MapConnectAttributeName = "MapConnectAttribute";
    private const string MapConnectAttributeFullyQualifiedName = $"{AttributesNamespace}.{MapConnectAttributeName}";
    private const string MapConnectAttributeHint = $"{MapConnectAttributeFullyQualifiedName}.gs.cs";

    private const string NameAttributeNamedParameter = "Name";
    private const string SummaryAttributeNamedParameter = "Summary";
    private const string DescriptionAttributeNamedParameter = "Description";
    private const string ResponseTypeAttributeNamedParameter = "ResponseType";
    private const string RequestTypeAttributeNamedParameter = "RequestType";
    private const string IsOptionalAttributeNamedParameter = "IsOptional";

    private const string RequireAuthorizationAttributeName = "RequireAuthorizationAttribute";
    private const string RequireAuthorizationAttributeFullyQualifiedName = $"{AttributesNamespace}.{RequireAuthorizationAttributeName}";
    private const string RequireAuthorizationAttributeHint = $"{RequireAuthorizationAttributeFullyQualifiedName}.gs.cs";

    private const string DisableAntiforgeryAttributeName = "DisableAntiforgeryAttribute";
    private const string DisableAntiforgeryAttributeFullyQualifiedName = $"{AttributesNamespace}.{DisableAntiforgeryAttributeName}";
    private const string DisableAntiforgeryAttributeHint = $"{DisableAntiforgeryAttributeFullyQualifiedName}.gs.cs";

    private const string AllowAnonymousAttributeFullyQualifiedName = "Microsoft.AspNetCore.Authorization.AllowAnonymousAttribute";

    private const string AcceptsAttributeName = "AcceptsAttribute";
    private const string AcceptsAttributeFullyQualifiedName = $"{AttributesNamespace}.{AcceptsAttributeName}";
    private const string AcceptsAttributeHint = $"{AcceptsAttributeFullyQualifiedName}.gs.cs";

    private const string ProducesResponseAttributeName = "ProducesResponseAttribute";
    private const string ProducesResponseAttributeFullyQualifiedName = $"{AttributesNamespace}.{ProducesResponseAttributeName}";
    private const string ProducesResponseAttributeHint = $"{ProducesResponseAttributeFullyQualifiedName}.gs.cs";

    private const string ProducesProblemAttributeName = "ProducesProblemAttribute";
    private const string ProducesProblemAttributeFullyQualifiedName = $"{AttributesNamespace}.{ProducesProblemAttributeName}";
    private const string ProducesProblemAttributeHint = $"{ProducesProblemAttributeFullyQualifiedName}.gs.cs";

    private const string ProducesValidationProblemAttributeName = "ProducesValidationProblemAttribute";
    private const string ProducesValidationProblemAttributeFullyQualifiedName =
        $"{AttributesNamespace}.{ProducesValidationProblemAttributeName}";
    private const string ProducesValidationProblemAttributeHint = $"{ProducesValidationProblemAttributeFullyQualifiedName}.gs.cs";

    private const string RoutingNamespace = $"{BaseNamespace}.Routing";

    private const string AddEndpointHandlersClassName = "EndpointServicesExtensions";
    private const string AddEndpointHandlersMethodName = "AddEndpointHandlers";
    private const string AddEndpointHandlersMethodHint = $"{RoutingNamespace}.{AddEndpointHandlersMethodName}.g.cs";

    private const string UseEndpointHandlersClassName = "EndpointRouteBuilderExtensions";
    private const string UseEndpointHandlersMethodName = "MapEndpointHandlers";
    private const string UseEndpointHandlersMethodHint = $"{RoutingNamespace}.{UseEndpointHandlersMethodName}.g.cs";

    private const string ConfigureMethodName = "Configure";
    private const string AsyncSuffix = "Async";

    private static readonly string FileHeader = $"""
                                                 //-----------------------------------------------------------------------------
                                                 // <auto-generated>
                                                 // This code was generated by {nameof(MinimalApiGenerator)} which can be found
                                                 // in the {typeof(MinimalApiGenerator).Namespace} namespace.
                                                 //
                                                 // Changes to this file may cause incorrect behavior
                                                 // and will be lost if the code is regenerated.
                                                 // </auto-generated>
                                                 //-----------------------------------------------------------------------------

                                                 #nullable enable
                                                 """;

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(RegisterAttributes);

        var getRequestHandlers = context.SyntaxProvider
            .ForAttributeWithMetadataName(MapGetAttributeFullyQualifiedName, RequestHandlerFilter, RequestHandlerTransform)
            .WhereNotNull()
            .Collect();

        var postRequestHandlers = context.SyntaxProvider
            .ForAttributeWithMetadataName(MapPostAttributeFullyQualifiedName, RequestHandlerFilter, RequestHandlerTransform)
            .WhereNotNull()
            .Collect();

        var putRequestHandlers = context.SyntaxProvider
            .ForAttributeWithMetadataName(MapPutAttributeFullyQualifiedName, RequestHandlerFilter, RequestHandlerTransform)
            .WhereNotNull()
            .Collect();

        var deleteRequestHandlers = context.SyntaxProvider
            .ForAttributeWithMetadataName(MapDeleteAttributeFullyQualifiedName, RequestHandlerFilter, RequestHandlerTransform)
            .WhereNotNull()
            .Collect();

        var optionsRequestHandlers = context.SyntaxProvider
            .ForAttributeWithMetadataName(MapOptionsAttributeFullyQualifiedName, RequestHandlerFilter, RequestHandlerTransform)
            .WhereNotNull()
            .Collect();

        var headRequestHandlers = context.SyntaxProvider
            .ForAttributeWithMetadataName(MapHeadAttributeFullyQualifiedName, RequestHandlerFilter, RequestHandlerTransform)
            .WhereNotNull()
            .Collect();

        var patchRequestHandlers = context.SyntaxProvider
            .ForAttributeWithMetadataName(MapPatchAttributeFullyQualifiedName, RequestHandlerFilter, RequestHandlerTransform)
            .WhereNotNull()
            .Collect();

        var queryRequestHandlers = context.SyntaxProvider
            .ForAttributeWithMetadataName(MapQueryAttributeFullyQualifiedName, RequestHandlerFilter, RequestHandlerTransform)
            .WhereNotNull()
            .Collect();

        var traceRequestHandlers = context.SyntaxProvider
            .ForAttributeWithMetadataName(MapTraceAttributeFullyQualifiedName, RequestHandlerFilter, RequestHandlerTransform)
            .WhereNotNull()
            .Collect();

        var connectRequestHandlers = context.SyntaxProvider
            .ForAttributeWithMetadataName(MapConnectAttributeFullyQualifiedName, RequestHandlerFilter, RequestHandlerTransform)
            .WhereNotNull()
            .Collect();

        var requestHandlers = getRequestHandlers.Combine(postRequestHandlers)
            .Select(static (x, _) => x.Left.AddRange(x.Right))
            .Combine(putRequestHandlers)
            .Select(static (x, _) => x.Left.AddRange(x.Right))
            .Combine(patchRequestHandlers)
            .Select(static (x, _) => x.Left.AddRange(x.Right))
            .Combine(deleteRequestHandlers)
            .Select(static (x, _) => x.Left.AddRange(x.Right))
            .Combine(optionsRequestHandlers)
            .Select(static (x, _) => x.Left.AddRange(x.Right))
            .Combine(headRequestHandlers)
            .Select(static (x, _) => x.Left.AddRange(x.Right))
            .Combine(queryRequestHandlers)
            .Select(static (x, _) => x.Left.AddRange(x.Right))
            .Combine(traceRequestHandlers)
            .Select(static (x, _) => x.Left.AddRange(x.Right))
            .Combine(connectRequestHandlers)
            .Select(static (x, _) => x.Left.AddRange(x.Right));

        context.RegisterSourceOutput(requestHandlers, GenerateSource);
    }

    private static void RegisterAttributes(IncrementalGeneratorPostInitializationContext context)
    {
        // Definitions for HTTP method attributes
        var httpAttributes = new[]
        {
            (Name: MapGetAttributeName, FullyQualified: MapGetAttributeFullyQualifiedName, Hint: MapGetAttributeHint, Verb: "GET"),
            (Name: MapPostAttributeName, FullyQualified: MapPostAttributeFullyQualifiedName, Hint: MapPostAttributeHint, Verb: "POST"),
            (Name: MapPutAttributeName, FullyQualified: MapPutAttributeFullyQualifiedName, Hint: MapPutAttributeHint, Verb: "PUT"),
            (Name: MapDeleteAttributeName, FullyQualified: MapDeleteAttributeFullyQualifiedName, Hint: MapDeleteAttributeHint, Verb: "DELETE"),
            (Name: MapOptionsAttributeName, FullyQualified: MapOptionsAttributeFullyQualifiedName, Hint: MapOptionsAttributeHint, Verb: "OPTIONS"),
            (Name: MapHeadAttributeName, FullyQualified: MapHeadAttributeFullyQualifiedName, Hint: MapHeadAttributeHint, Verb: "HEAD"),
            (Name: MapPatchAttributeName, FullyQualified: MapPatchAttributeFullyQualifiedName, Hint: MapPatchAttributeHint, Verb: "PATCH"),
            (Name: MapQueryAttributeName, FullyQualified: MapQueryAttributeFullyQualifiedName, Hint: MapQueryAttributeHint, Verb: "QUERY"),
            (Name: MapTraceAttributeName, FullyQualified: MapTraceAttributeFullyQualifiedName, Hint: MapTraceAttributeHint, Verb: "TRACE"),
            (Name: MapConnectAttributeName, FullyQualified: MapConnectAttributeFullyQualifiedName, Hint: MapConnectAttributeHint, Verb: "CONNECT"),
        };

        foreach (var (name, _, hint, verb) in httpAttributes)
        {
            var source = GenerateHttpAttributeSource(FileHeader, AttributesNamespace, name, verb);
            context.AddSource(hint, SourceText.From(source, Encoding.UTF8));
        }

        // RequireAuthorization
        var requireAuthorizationSource = $$"""
                                           {{FileHeader}}

                                           namespace {{AttributesNamespace}};

                                           /// <summary>
                                           /// Specifies that authorization is required for the annotated endpoint or class.
                                           /// Optionally restricts access to the specified authorization policies.
                                           /// </summary>
                                           [global::System.AttributeUsage(global::System.AttributeTargets.Class | global::System.AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
                                           internal sealed class {{RequireAuthorizationAttributeName}} : global::System.Attribute
                                           {
                                               /// <summary>
                                               /// Gets the policy names that the endpoint or class requires.
                                               /// </summary>
                                               public string[] PolicyNames { get; }

                                               /// <summary>
                                               /// Marks the endpoint or class as requiring authorization.
                                               /// </summary>
                                               public {{RequireAuthorizationAttributeName}}()
                                               {
                                                   PolicyNames = [];
                                               }

                                               /// <summary>
                                               /// Marks the endpoint or class as requiring authorization with one or more policies.
                                               /// </summary>
                                               public {{RequireAuthorizationAttributeName}}(params string[] policyNames)
                                               {
                                                   PolicyNames = policyNames ?? [];
                                               }
                                           }
                                           """;
        context.AddSource(RequireAuthorizationAttributeHint, SourceText.From(requireAuthorizationSource, Encoding.UTF8));

        // DisableAntiforgery
        var disableAntiforgerySource = $$"""
                                         {{FileHeader}}

                                         namespace {{AttributesNamespace}};

                                         /// <summary>
                                         /// Disables antiforgery protection for the annotated endpoint or class.
                                         /// </summary>
                                         [global::System.AttributeUsage(global::System.AttributeTargets.Class | global::System.AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
                                         internal sealed class {{DisableAntiforgeryAttributeName}} : global::System.Attribute
                                         {
                                         }

                                         """;
        context.AddSource(DisableAntiforgeryAttributeHint, SourceText.From(disableAntiforgerySource, Encoding.UTF8));

        // Accepts
        var acceptsSource = $$"""
                               {{FileHeader}}

                               namespace {{AttributesNamespace}};

                               /// <summary>
                               /// Specifies the request type and content types accepted by the annotated endpoint or class.
                               /// </summary>
                               [global::System.AttributeUsage(global::System.AttributeTargets.Class | global::System.AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
                               internal sealed class {{AcceptsAttributeName}} : global::System.Attribute
                               {
                                   /// <summary>
                                   /// Gets the request type accepted by the endpoint.
                                   /// </summary>
                                   public global::System.Type RequestType { get; init; } = default!;

                                   /// <summary>
                                   /// Gets a value indicating whether the request body is optional.
                                   /// </summary>
                                   public bool IsOptional { get; init; }

                                   /// <summary>
                                   /// Gets the primary content type accepted by the endpoint.
                                   /// </summary>
                                   public string ContentType { get; }

                                   /// <summary>
                                   /// Gets the additional content types accepted by the endpoint.
                                   /// </summary>
                                   public string[] AdditionalContentTypes { get; }

                                   /// <summary>
                                   /// Initializes a new instance of the <see cref="{{AcceptsAttributeName}}"/> class.
                                   /// </summary>
                                   /// <param name="contentType">The primary content type accepted by the endpoint.</param>
                                   /// <param name="additionalContentTypes">Additional content types accepted by the endpoint.</param>
                                   public {{AcceptsAttributeName}}(string contentType = "application/json", params string[] additionalContentTypes)
                                   {
                                       ContentType = string.IsNullOrWhiteSpace(contentType) ? "application/json" : contentType;
                                       AdditionalContentTypes = additionalContentTypes ?? [];
                                   }
                               }

                               /// <summary>
                               /// Specifies the request type using a generic argument and the content types accepted by the annotated endpoint or class.
                               /// </summary>
                               /// <typeparam name="TRequest">The CLR type of the request body.</typeparam>
                               [global::System.AttributeUsage(global::System.AttributeTargets.Class | global::System.AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
                               internal sealed class {{AcceptsAttributeName}}<TRequest> : global::System.Attribute
                               {
                                   /// <summary>
                                   /// Gets the request type accepted by the endpoint.
                                   /// </summary>
                                   public global::System.Type RequestType => typeof(TRequest);

                                   /// <summary>
                                   /// Gets a value indicating whether the request body is optional.
                                   /// </summary>
                                   public bool IsOptional { get; init; }

                                   /// <summary>
                                   /// Gets the primary content type accepted by the endpoint.
                                   /// </summary>
                                   public string ContentType { get; }

                                   /// <summary>
                                   /// Gets the additional content types accepted by the endpoint.
                                   /// </summary>
                                   public string[] AdditionalContentTypes { get; }

                                   /// <summary>
                                   /// Initializes a new instance of the generic Accepts attribute class.
                                   /// </summary>
                                   /// <param name="contentType">The primary content type accepted by the endpoint.</param>
                                   /// <param name="additionalContentTypes">Additional content types accepted by the endpoint.</param>
                                   public {{AcceptsAttributeName}}(string contentType = "application/json", params string[] additionalContentTypes)
                                   {
                                       ContentType = string.IsNullOrWhiteSpace(contentType) ? "application/json" : contentType;
                                       AdditionalContentTypes = additionalContentTypes ?? [];
                                   }
                               }

                               """;
        context.AddSource(AcceptsAttributeHint, SourceText.From(acceptsSource, Encoding.UTF8));

        // Produces
        var producesSource = $$"""
                                {{FileHeader}}

                                namespace {{AttributesNamespace}};

                                /// <summary>
                                /// Specifies a response type, status code, and content types produced by the annotated endpoint or class.
                                /// </summary>
                                [global::System.AttributeUsage(global::System.AttributeTargets.Class | global::System.AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
                                internal sealed class {{ProducesResponseAttributeName}} : global::System.Attribute
                                {
                                /// <summary>
                                /// Gets the response type produced by the endpoint.
                                /// </summary>
                                public global::System.Type? ResponseType { get; init; } = null;

                                    /// <summary>
                                    /// Gets the HTTP status code returned by the endpoint.
                                    /// </summary>
                                    public int StatusCode { get; }

                                    /// <summary>
                                    /// Gets the primary content type produced by the endpoint.
                                    /// </summary>
                                    public string? ContentType { get; }

                                    /// <summary>
                                    /// Gets the additional content types produced by the endpoint.
                                    /// </summary>
                                    public string[] AdditionalContentTypes { get; }

                                    /// <summary>
                                    /// Initializes a new instance of the <see cref="{{ProducesResponseAttributeName}}"/> class.
                                    /// </summary>
                                    /// <param name="statusCode">The HTTP status code returned by the endpoint.</param>
                                    /// <param name="contentType">The primary content type produced by the endpoint.</param>
                                    /// <param name="additionalContentTypes">Additional content types produced by the endpoint.</param>
                                    public {{ProducesResponseAttributeName}}(int statusCode = 200, string? contentType = null, params string[] additionalContentTypes)
                                    {
                                        StatusCode = statusCode;
                                        ContentType = contentType;
                                        AdditionalContentTypes = additionalContentTypes ?? [];
                                    }
                                }

                                /// <summary>
                                /// Specifies a response type using a generic argument along with status code and content types produced by the annotated endpoint or class.
                                /// </summary>
                                /// <typeparam name="TResponse">The CLR type of the response body.</typeparam>
                                [global::System.AttributeUsage(global::System.AttributeTargets.Class | global::System.AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
                                internal sealed class {{ProducesResponseAttributeName}}<TResponse> : global::System.Attribute
                                {
                                    /// <summary>
                                    /// Gets the response type produced by the endpoint.
                                    /// </summary>
                                    public global::System.Type ResponseType => typeof(TResponse);

                                    /// <summary>
                                    /// Gets the HTTP status code returned by the endpoint.
                                    /// </summary>
                                    public int StatusCode { get; }

                                    /// <summary>
                                    /// Gets the primary content type produced by the endpoint.
                                    /// </summary>
                                    public string? ContentType { get; }

                                    /// <summary>
                                    /// Gets the additional content types produced by the endpoint.
                                    /// </summary>
                                    public string[] AdditionalContentTypes { get; }

                                    /// <summary>
                                    /// Initializes a new instance of the generic Produces attribute class.
                                    /// </summary>
                                    /// <param name="statusCode">The HTTP status code returned by the endpoint.</param>
                                    /// <param name="contentType">The primary content type produced by the endpoint.</param>
                                    /// <param name="additionalContentTypes">Additional content types produced by the endpoint.</param>
                                    public {{ProducesResponseAttributeName}}(int statusCode = 200, string? contentType = null, params string[] additionalContentTypes)
                                    {
                                        StatusCode = statusCode;
                                        ContentType = contentType;
                                        AdditionalContentTypes = additionalContentTypes ?? [];
                                    }
                                }

                                """;
        context.AddSource(ProducesResponseAttributeHint, SourceText.From(producesSource, Encoding.UTF8));

        // ProducesProblem
        var producesProblemSource = $$"""
                                        {{FileHeader}}

                                        namespace {{AttributesNamespace}};

                                        /// <summary>
                                        /// Specifies that the endpoint produces a problem details payload.
                                        /// </summary>
                                        [global::System.AttributeUsage(global::System.AttributeTargets.Class | global::System.AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
                                        internal sealed class {{ProducesProblemAttributeName}} : global::System.Attribute
                                        {
                                            /// <summary>
                                            /// Gets the HTTP status code returned by the endpoint.
                                            /// </summary>
                                            public int StatusCode { get; }

                                            /// <summary>
                                            /// Gets the primary content type produced by the endpoint.
                                            /// </summary>
                                            public string? ContentType { get; }

                                            /// <summary>
                                            /// Gets the additional content types produced by the endpoint.
                                            /// </summary>
                                            public string[] AdditionalContentTypes { get; }

                                            /// <summary>
                                            /// Initializes a new instance of the <see cref="{{ProducesProblemAttributeName}}"/> class.
                                            /// </summary>
                                            /// <param name="statusCode">The HTTP status code returned by the endpoint.</param>
                                            /// <param name="contentType">The primary content type produced by the endpoint.</param>
                                            /// <param name="additionalContentTypes">Additional content types produced by the endpoint.</param>
                                            public {{ProducesProblemAttributeName}}(int statusCode = 500, string? contentType = null, params string[] additionalContentTypes)
                                            {
                                                StatusCode = statusCode;
                                                ContentType = contentType;
                                                AdditionalContentTypes = additionalContentTypes ?? [];
                                            }
                                        }

                                        """;
        context.AddSource(ProducesProblemAttributeHint, SourceText.From(producesProblemSource, Encoding.UTF8));

        // ProducesValidationProblem
        var producesValidationProblemSource = $$"""
                                                {{FileHeader}}

                                                namespace {{AttributesNamespace}};

                                                /// <summary>
                                                /// Specifies that the endpoint produces a validation problem details payload.
                                                /// </summary>
                                                [global::System.AttributeUsage(global::System.AttributeTargets.Class | global::System.AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
                                                internal sealed class {{ProducesValidationProblemAttributeName}} : global::System.Attribute
                                                {
                                                    /// <summary>
                                                    /// Gets the HTTP status code returned by the endpoint.
                                                    /// </summary>
                                                    public int StatusCode { get; }

                                                    /// <summary>
                                                    /// Gets the primary content type produced by the endpoint.
                                                    /// </summary>
                                                    public string? ContentType { get; }

                                                    /// <summary>
                                                    /// Gets the additional content types produced by the endpoint.
                                                    /// </summary>
                                                    public string[] AdditionalContentTypes { get; }

                                                    /// <summary>
                                                    /// Initializes a new instance of the <see cref="{{ProducesValidationProblemAttributeName}}"/> class.
                                                    /// </summary>
                                                    /// <param name="statusCode">The HTTP status code returned by the endpoint.</param>
                                                    /// <param name="contentType">The primary content type produced by the endpoint.</param>
                                                    /// <param name="additionalContentTypes">Additional content types produced by the endpoint.</param>
                                                    public {{ProducesValidationProblemAttributeName}}(int statusCode = 400, string? contentType = null, params string[] additionalContentTypes)
                                                    {
                                                        StatusCode = statusCode;
                                                        ContentType = contentType;
                                                        AdditionalContentTypes = additionalContentTypes ?? [];
                                                    }
                                                }

                                                """;
        context.AddSource(ProducesValidationProblemAttributeHint, SourceText.From(producesValidationProblemSource, Encoding.UTF8));
    }

    private static string GenerateHttpAttributeSource(string fileHeader, string attributesNamespace, string attributeName, string summaryVerb)
    {
        return $$"""
                 {{fileHeader}}

                 namespace {{attributesNamespace}};

                 /// <summary>
                 /// Identifies a method as an HTTP {{summaryVerb}} minimal API endpoint with the specified route pattern.
                 /// </summary>
                 [global::System.AttributeUsage(global::System.AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
                 internal sealed class {{attributeName}} : global::System.Attribute
                 {
                     /// <summary>
                     /// Gets the route pattern for the endpoint.
                     /// </summary>
                     public string Pattern { get; }

                     /// <summary>
                     /// Gets or sets the endpoint name.
                     /// </summary>
                     public string Name { get; set; } = "";

                     /// <summary>
                     /// Gets or sets the endpoint summary.
                     /// </summary>
                     public string Summary { get; set; } = "";

                     /// <summary>
                     /// Gets or sets the endpoint description.
                     /// </summary>
                     public string Description { get; set; } = "";

                     /// <summary>
                     /// Initializes a new instance of the <see cref="{{attributeName}}"/> class.
                     /// </summary>
                     /// <param name="pattern">The route pattern for the endpoint.</param>
                     public {{attributeName}}([global::System.Diagnostics.CodeAnalysis.StringSyntax("Route")] string pattern)
                     {
                         Pattern = pattern;
                     }
                 }
                 """;
    }

    private static bool RequestHandlerFilter(SyntaxNode syntaxNode, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return syntaxNode is MethodDeclarationSyntax;
    }

    private static RequestHandler? RequestHandlerTransform(GeneratorAttributeSyntaxContext context, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (context.TargetSymbol is not IMethodSymbol requestHandlerMethodSymbol)
            return null;
        var attribute = context.Attributes[0];

        var requestHandlerClassResult = GetRequestHandlerClass(requestHandlerMethodSymbol, context.SemanticModel.Compilation, cancellationToken);
        if (requestHandlerClassResult is null)
            return null;

        var (requestHandlerClassSymbol, requestHandlerClass) = requestHandlerClassResult.Value;

        var requestHandlerMethod = GetRequestHandlerMethod(requestHandlerMethodSymbol, cancellationToken);

        var (httpMethod, pattern, name, summary, description) = GetRequestHandlerAttribute(attribute, cancellationToken);

        var (tags, requireAuthorization, authorizationPolicies, disableAntiforgery, allowAnonymous, excludeFromDescription,
                accepts, produces, producesProblem, producesValidationProblem)
            = GetAdditionalRequestHandlerAttributes(requestHandlerClassSymbol, requestHandlerMethodSymbol, cancellationToken);

        name ??= RemoveAsyncSuffix(requestHandlerMethod.Name);

        var metadata = new RequestHandlerMetadata(
            name,
            summary,
            description,
            tags,
            accepts,
            produces,
            producesProblem,
            producesValidationProblem,
            excludeFromDescription
        );

        var requestHandler = new RequestHandler(requestHandlerClass, requestHandlerMethod, httpMethod, pattern, metadata, requireAuthorization,
            authorizationPolicies, disableAntiforgery, allowAnonymous
        );

        return requestHandler;
    }

    private static string RemoveAsyncSuffix(string methodName)
    {
        if (methodName.EndsWith(AsyncSuffix, StringComparison.OrdinalIgnoreCase) && methodName.Length > AsyncSuffix.Length)
            return methodName[..^AsyncSuffix.Length];

        return methodName;
    }

    private static (string HttpMethod, string Pattern, string? Name, string? Summary, string? Description) GetRequestHandlerAttribute(
        AttributeData attribute,
        CancellationToken cancellationToken
    )
    {
        cancellationToken.ThrowIfCancellationRequested();

        var attributeName = attribute.AttributeClass?.Name ?? "";

        var httpMethod = attributeName switch
        {
            MapGetAttributeName => "Get",
            MapPostAttributeName => "Post",
            MapPutAttributeName => "Put",
            MapDeleteAttributeName => "Delete",
            MapOptionsAttributeName => "OPTIONS",
            MapHeadAttributeName => "HEAD",
            MapPatchAttributeName => "Patch",
            MapQueryAttributeName => "QUERY",
            MapTraceAttributeName => "TRACE",
            MapConnectAttributeName => "CONNECT",
            _ => "",
        };

        var pattern = (attribute.ConstructorArguments.Length > 0 ? attribute.ConstructorArguments[0].Value as string : "") ?? "";

        string? name = null;
        string? summary = null;
        string? description = null;
        foreach (var namedArg in attribute.NamedArguments)
        {
            switch (namedArg.Key)
            {
                case NameAttributeNamedParameter:
                {
                    var value = namedArg.Value.Value as string;
                    name = string.IsNullOrWhiteSpace(value) ? null : value!.Trim();
                    break;
                }
                case SummaryAttributeNamedParameter:
                {
                    var value = namedArg.Value.Value as string;
                    summary = string.IsNullOrWhiteSpace(value) ? null : value!.Trim();
                    break;
                }
                case DescriptionAttributeNamedParameter:
                {
                    var value = namedArg.Value.Value as string;
                    description = string.IsNullOrWhiteSpace(value) ? null : value!.Trim();
                    break;
                }
            }
        }

        return (httpMethod, pattern, name, summary, description);
    }

    private static (
        EquatableImmutableArray<string>? tags,
        bool requireAuthorization,
        EquatableImmutableArray<string>? authorizationPolicies,
        bool disableAntiforgery,
        bool allowAnonymous,
        bool excludeFromDescription,
        EquatableImmutableArray<AcceptsMetadata>? accepts,
        EquatableImmutableArray<ProducesMetadata>? produces,
        EquatableImmutableArray<ProducesProblemMetadata>? producesProblem,
        EquatableImmutableArray<ProducesValidationProblemMetadata>? producesValidationProblem
    ) GetAdditionalRequestHandlerAttributes(INamedTypeSymbol classSymbol, IMethodSymbol methodSymbol, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        EquatableImmutableArray<string>? tags = null;
        var requireAuthorization = false;
        EquatableImmutableArray<string>? authorizationPolicies = null;
        var disableAntiforgery = false;
        var allowAnonymous = false;
        var excludeFromDescription = false;

        List<AcceptsMetadata>? accepts = null;
        List<ProducesMetadata>? produces = null;
        List<ProducesProblemMetadata>? producesProblem = null;
        List<ProducesValidationProblemMetadata>? producesValidationProblem = null;

        var classAttributes = classSymbol.GetAttributes();
        GetAdditionalRequestHandlerAttributeValues(
            classAttributes,
            ref tags,
            ref requireAuthorization,
            ref authorizationPolicies,
            ref disableAntiforgery,
            ref allowAnonymous,
            ref excludeFromDescription,
            ref accepts,
            ref produces,
            ref producesProblem,
            ref producesValidationProblem
        );

        var methodAttributes = methodSymbol.GetAttributes();
        GetAdditionalRequestHandlerAttributeValues(
            methodAttributes,
            ref tags,
            ref requireAuthorization,
            ref authorizationPolicies,
            ref disableAntiforgery,
            ref allowAnonymous,
            ref excludeFromDescription,
            ref accepts,
            ref produces,
            ref producesProblem,
            ref producesValidationProblem
        );

        return (
            tags,
            requireAuthorization,
            authorizationPolicies,
            disableAntiforgery,
            allowAnonymous,
            excludeFromDescription,
            ToEquatableOrNull(accepts),
            ToEquatableOrNull(produces),
            ToEquatableOrNull(producesProblem),
            ToEquatableOrNull(producesValidationProblem)
        );
    }

    private static void GetAdditionalRequestHandlerAttributeValues(
        ImmutableArray<AttributeData> attributes,
        ref EquatableImmutableArray<string>? tags,
        ref bool requireAuthorization,
        ref EquatableImmutableArray<string>? authorizationPolicies,
        ref bool disableAntiforgery,
        ref bool allowAnonymous,
        ref bool excludeFromDescription,
        ref List<AcceptsMetadata>? accepts,
        ref List<ProducesMetadata>? produces,
        ref List<ProducesProblemMetadata>? producesProblem,
        ref List<ProducesValidationProblemMetadata>? producesValidationProblem
    )
    {
        foreach (var attribute in attributes)
        {
            var attributeClass = attribute.AttributeClass;
            if (attributeClass is null)
                continue;

            var fullyQualifiedName = attributeClass.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);

            if (IsGeneratedAttribute(fullyQualifiedName, AcceptsAttributeName))
            {
                TryAddAcceptsMetadata(attribute, attributeClass, ref accepts);
                continue;
            }

            if (IsGeneratedAttribute(fullyQualifiedName, ProducesResponseAttributeName))
            {
                TryAddProducesMetadata(attribute, attributeClass, ref produces);
                continue;
            }

            switch (fullyQualifiedName)
            {
                case "global::Microsoft.AspNetCore.Http.TagsAttribute":
                    if (attribute.ConstructorArguments.Length > 0)
                    {
                        var arg = attribute.ConstructorArguments[0];
                        if (arg.Values.Length > 0)
                        {
                            var values = arg.Values
                                .Select(v => v.Value as string)
                                .Where(s => !string.IsNullOrWhiteSpace(s))
                                .Select(s => s!.Trim());

                            MergeInto(ref tags, values);
                        }
                    }
                    break;
                case $"global::{RequireAuthorizationAttributeFullyQualifiedName}":
                    requireAuthorization = true;
                    if (attribute.ConstructorArguments.Length == 1)
                    {
                        var arg = attribute.ConstructorArguments[0];
                        if (arg.Values.Length > 0)
                        {
                            var values = arg.Values
                                .Select(v => v.Value as string)
                                .Where(s => !string.IsNullOrWhiteSpace(s))
                                .Select(s => s!.Trim());

                            MergeInto(ref authorizationPolicies, values);
                        }
                    }
                    break;
                case $"global::{DisableAntiforgeryAttributeFullyQualifiedName}":
                    disableAntiforgery = true;
                    break;
                case $"global::{AllowAnonymousAttributeFullyQualifiedName}":
                    allowAnonymous = true;
                    break;
                case "global::Microsoft.AspNetCore.Routing.ExcludeFromDescriptionAttribute":
                    excludeFromDescription = true;
                    break;
                case $"global::{ProducesProblemAttributeFullyQualifiedName}":
                {
                    var statusCode = attribute.ConstructorArguments.Length > 0 && attribute.ConstructorArguments[0].Value is int producesProblemStatusCode
                        ? producesProblemStatusCode
                        : 500;
                    var contentType = attribute.ConstructorArguments.Length > 1
                        ? NormalizeOptionalContentType(attribute.ConstructorArguments[1].Value as string)
                        : null;
                    var additionalContentTypes = attribute.ConstructorArguments.Length > 2
                        ? GetStringArrayValues(attribute.ConstructorArguments[2])
                        : null;

                    var producesProblemList = producesProblem ??= [];
                    producesProblemList.Add(new ProducesProblemMetadata(statusCode, contentType, additionalContentTypes));
                    break;
                }
                case $"global::{ProducesValidationProblemAttributeFullyQualifiedName}":
                {
                    var statusCode = attribute.ConstructorArguments.Length > 0 && attribute.ConstructorArguments[0].Value is int producesValidationProblemStatusCode
                        ? producesValidationProblemStatusCode
                        : 400;
                    var contentType = attribute.ConstructorArguments.Length > 1
                        ? NormalizeOptionalContentType(attribute.ConstructorArguments[1].Value as string)
                        : null;
                    var additionalContentTypes = attribute.ConstructorArguments.Length > 2
                        ? GetStringArrayValues(attribute.ConstructorArguments[2])
                        : null;

                    var producesValidationProblemList = producesValidationProblem ??= [];
                    producesValidationProblemList.Add(new ProducesValidationProblemMetadata(statusCode, contentType, additionalContentTypes));
                    break;
                }
            }
        }
    }

    private static void MergeInto(ref EquatableImmutableArray<string>? target, IEnumerable<string> values)
    {
        var merged = MergeUnion(target, values);
        target = merged.Count > 0 ? merged : null;
    }

    private static EquatableImmutableArray<T>? ToEquatableOrNull<T>(List<T>? values)
    {
        return values is { Count: > 0 } ? values.ToEquatableImmutableArray() : null;
    }

    private static string NormalizeRequiredContentType(string? contentType, string defaultValue)
    {
        return string.IsNullOrWhiteSpace(contentType) ? defaultValue : contentType!.Trim();
    }

    private static string? NormalizeOptionalContentType(string? contentType)
    {
        return string.IsNullOrWhiteSpace(contentType) ? null : contentType!.Trim();
    }

    private static EquatableImmutableArray<string>? GetStringArrayValues(TypedConstant typedConstant)
    {
        if (typedConstant.Kind != TypedConstantKind.Array || typedConstant.Values.IsDefaultOrEmpty)
            return null;

        var builder = ImmutableArray.CreateBuilder<string>(typedConstant.Values.Length);
        foreach (var value in typedConstant.Values)
        {
            if (value.Value is string s && !string.IsNullOrWhiteSpace(s))
                builder.Add(s.Trim());
        }

        return builder.Count > 0 ? builder.ToEquatableImmutable() : null;
    }

    private static bool IsGeneratedAttribute(string fullyQualifiedName, string attributeName)
    {
        var prefix = $"global::{AttributesNamespace}.{attributeName}";
        return fullyQualifiedName.StartsWith(prefix, StringComparison.Ordinal);
    }

    private static void TryAddAcceptsMetadata(
        AttributeData attribute,
        INamedTypeSymbol attributeClass,
        ref List<AcceptsMetadata>? accepts)
    {
        string? requestType;
        string contentType;
        EquatableImmutableArray<string>? additionalContentTypes;
        var isOptional = GetNamedBoolValue(attribute, IsOptionalAttributeNamedParameter);

        if (attributeClass is { IsGenericType: true, TypeArguments.Length: 1 })
        {
            requestType = attributeClass.TypeArguments[0].ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            contentType = attribute.ConstructorArguments.Length > 0
                ? NormalizeRequiredContentType(attribute.ConstructorArguments[0].Value as string, "application/json")
                : "application/json";
            additionalContentTypes = attribute.ConstructorArguments.Length > 1
                ? GetStringArrayValues(attribute.ConstructorArguments[1])
                : null;
        }
        else if (GetNamedTypeSymbol(attribute, RequestTypeAttributeNamedParameter) is { } requestTypeSymbol)
        {
            requestType = requestTypeSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            contentType = attribute.ConstructorArguments.Length > 0
                ? NormalizeRequiredContentType(attribute.ConstructorArguments[0].Value as string, "application/json")
                : "application/json";
            additionalContentTypes = attribute.ConstructorArguments.Length > 1
                ? GetStringArrayValues(attribute.ConstructorArguments[1])
                : null;
        }
        else
        {
            return;
        }

        var acceptsList = accepts ??= [];
        acceptsList.Add(new AcceptsMetadata(requestType, contentType, additionalContentTypes, isOptional));
    }

    private static void TryAddProducesMetadata(
        AttributeData attribute,
        INamedTypeSymbol attributeClass,
        ref List<ProducesMetadata>? produces)
    {
        string? responseType;
        int statusCode;
        string? contentType;
        EquatableImmutableArray<string>? additionalContentTypes;

        if (attributeClass is { IsGenericType: true, TypeArguments.Length: 1 })
        {
            responseType = attributeClass.TypeArguments[0].ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            statusCode = attribute.ConstructorArguments.Length > 0 && attribute.ConstructorArguments[0].Value is int producesStatusCode
                ? producesStatusCode
                : 200;
            contentType = attribute.ConstructorArguments.Length > 1
                ? NormalizeOptionalContentType(attribute.ConstructorArguments[1].Value as string)
                : null;
            additionalContentTypes = attribute.ConstructorArguments.Length > 2
                ? GetStringArrayValues(attribute.ConstructorArguments[2])
                : null;
        }
        else if (GetNamedTypeSymbol(attribute, ResponseTypeAttributeNamedParameter) is { } responseTypeSymbol)
        {
            responseType = responseTypeSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            statusCode = attribute.ConstructorArguments.Length > 0 && attribute.ConstructorArguments[0].Value is int producesStatusCode
                ? producesStatusCode
                : 200;
            contentType = attribute.ConstructorArguments.Length > 1
                ? NormalizeOptionalContentType(attribute.ConstructorArguments[1].Value as string)
                : null;
            additionalContentTypes = attribute.ConstructorArguments.Length > 2
                ? GetStringArrayValues(attribute.ConstructorArguments[2])
                : null;
        }
        else
        {
            return;
        }

        var producesList = produces ??= [];
        producesList.Add(new ProducesMetadata(responseType, statusCode, contentType, additionalContentTypes));
    }

    private static ITypeSymbol? GetNamedTypeSymbol(AttributeData attribute, string namedParameter)
    {
        foreach (var namedArg in attribute.NamedArguments)
        {
            if (namedArg.Key == namedParameter && namedArg.Value.Value is ITypeSymbol typeSymbol)
                return typeSymbol;
        }

        return null;
    }

    private static bool GetNamedBoolValue(AttributeData attribute, string namedParameter, bool defaultValue = false)
    {
        foreach (var namedArg in attribute.NamedArguments)
        {
            if (namedArg.Key == namedParameter && namedArg.Value.Value is bool boolValue)
                return boolValue;
        }

        return defaultValue;
    }

    private static EquatableImmutableArray<string> MergeUnion(EquatableImmutableArray<string>? existing, IEnumerable<string> values)
    {
        var list = new List<string>();

        if (existing is { Count: > 0 })
            list.AddRange(existing.Value);

        foreach (var value in values)
        {
            if (string.IsNullOrWhiteSpace(value))
                continue;
            var trimmed = value.Trim();
            if (!list.Contains(trimmed, StringComparer.OrdinalIgnoreCase))
                list.Add(trimmed);
        }

        return list.ToEquatableImmutableArray();
    }

    private static RequestHandlerMethod GetRequestHandlerMethod(IMethodSymbol methodSymbol, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var name = methodSymbol.Name;
        var isStatic = methodSymbol.IsStatic;
        var isAwaitable = methodSymbol.ReturnType.IsTask(out _) || methodSymbol.ReturnType.IsValueTask(out _);
        var parameters = GetRequestHandlerParameters(methodSymbol, cancellationToken);

        var requestHandlerMethod = new RequestHandlerMethod(name, isStatic, isAwaitable, parameters);

        return requestHandlerMethod;
    }

    private static (INamedTypeSymbol RequestHandlerClassSymbol, RequestHandlerClass RequestHandlerClass)? GetRequestHandlerClass(
        IMethodSymbol methodSymbol,
        Compilation compilation,
        CancellationToken cancellationToken
    )
    {
        cancellationToken.ThrowIfCancellationRequested();

        var classSymbol = methodSymbol.ContainingType;
        if (classSymbol.TypeKind != TypeKind.Class)
            return null;

        var name = classSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
        var isStatic = classSymbol.IsStatic;
        var endpointConventionBuilderSymbol = compilation.GetTypeByMetadataName("Microsoft.AspNetCore.Builder.IEndpointConventionBuilder");
        var serviceProviderSymbol = compilation.GetTypeByMetadataName("System.IServiceProvider");
        var configureMethodDetails = GetConfigureMethodDetails(
            classSymbol,
            endpointConventionBuilderSymbol,
            serviceProviderSymbol,
            cancellationToken
        );

        var requestHandlerClass = new RequestHandlerClass(
            name,
            isStatic,
            configureMethodDetails.HasConfigureMethod,
            configureMethodDetails.ConfigureMethodAcceptsServiceProvider
        );

        return (classSymbol, requestHandlerClass);
    }

    private static ConfigureMethodDetails GetConfigureMethodDetails(
        INamedTypeSymbol classSymbol,
        INamedTypeSymbol? endpointConventionBuilderSymbol,
        INamedTypeSymbol? serviceProviderSymbol,
        CancellationToken cancellationToken
    )
    {
        cancellationToken.ThrowIfCancellationRequested();

        var hasConfigureMethod = false;
        var acceptsServiceProvider = false;

        foreach (var member in classSymbol.GetMembers(ConfigureMethodName))
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (member is not IMethodSymbol methodSymbol)
                continue;

            if (IsConfigureMethod(methodSymbol, endpointConventionBuilderSymbol, serviceProviderSymbol, out var methodAcceptsServiceProvider))
            {
                hasConfigureMethod = true;
                if (methodAcceptsServiceProvider)
                {
                    acceptsServiceProvider = true;
                    break;
                }
            }
        }

        return new ConfigureMethodDetails(hasConfigureMethod, acceptsServiceProvider);
    }

    private static bool IsConfigureMethod(
        IMethodSymbol methodSymbol,
        INamedTypeSymbol? endpointConventionBuilderSymbol,
        INamedTypeSymbol? serviceProviderSymbol,
        out bool acceptsServiceProvider
    )
    {
        acceptsServiceProvider = false;

        if (!methodSymbol.IsStatic)
            return false;

        if (methodSymbol.TypeParameters.Length != 1)
            return false;

        if (methodSymbol.Parameters.Length is < 1 or > 2)
            return false;

        var builderTypeParameter = methodSymbol.TypeParameters[0];
        var builderParameter = methodSymbol.Parameters[0];

        if (!SymbolEqualityComparer.Default.Equals(builderParameter.Type, builderTypeParameter))
            return false;

        if (methodSymbol.Parameters.Length == 2)
        {
            var serviceProviderParameter = methodSymbol.Parameters[1];
            if (!IsServiceProviderParameter(serviceProviderParameter.Type, serviceProviderSymbol))
                return false;

            acceptsServiceProvider = true;
        }

        if (!methodSymbol.ReturnsVoid)
            return false;

        if (!HasEndpointConventionBuilderConstraint(builderTypeParameter, methodSymbol, endpointConventionBuilderSymbol))
            return false;

        return true;
    }

    private static bool IsServiceProviderParameter(ITypeSymbol typeSymbol, INamedTypeSymbol? serviceProviderSymbol)
    {
        if (serviceProviderSymbol is not null)
            return SymbolEqualityComparer.Default.Equals(typeSymbol, serviceProviderSymbol);

        return MatchesServiceProvider(typeSymbol);
    }

    private static bool HasEndpointConventionBuilderConstraint(
        ITypeParameterSymbol builderTypeParameter,
        IMethodSymbol methodSymbol,
        INamedTypeSymbol? endpointConventionBuilderSymbol
    )
    {
        var symbolMatches = builderTypeParameter.ConstraintTypes.Any(constraint =>
            endpointConventionBuilderSymbol is not null
                ? SymbolEqualityComparer.Default.Equals(constraint, endpointConventionBuilderSymbol)
                : MatchesEndpointConventionBuilder(constraint)
        );

        if (symbolMatches)
            return true;

        return methodSymbol.DeclaringSyntaxReferences
            .Select(reference => reference.GetSyntax())
            .OfType<MethodDeclarationSyntax>()
            .SelectMany(methodSyntax => methodSyntax.ConstraintClauses)
            .Where(clause => string.Equals(clause.Name.Identifier.ValueText, builderTypeParameter.Name, StringComparison.Ordinal))
            .SelectMany(clause => clause.Constraints.OfType<TypeConstraintSyntax>())
            .Any(constraint => IsEndpointConventionBuilderIdentifier(constraint.Type));
    }

    private static bool IsEndpointConventionBuilderIdentifier(TypeSyntax typeSyntax)
    {
        return typeSyntax switch
        {
            QualifiedNameSyntax qualified => IsEndpointConventionBuilderIdentifier(qualified.Right),
            AliasQualifiedNameSyntax alias => IsEndpointConventionBuilderIdentifier(alias.Name),
            SimpleNameSyntax simple => string.Equals(simple.Identifier.ValueText, "IEndpointConventionBuilder", StringComparison.Ordinal),
            _ => false,
        };
    }

    private static bool MatchesEndpointConventionBuilder(ITypeSymbol typeSymbol)
    {
        if (typeSymbol is not INamedTypeSymbol namedType)
            return false;

        if (!string.Equals(namedType.Name, "IEndpointConventionBuilder", StringComparison.Ordinal))
            return false;

        var containingNamespace = namedType.ContainingNamespace?.ToDisplayString() ?? string.Empty;
        return string.Equals(containingNamespace, "Microsoft.AspNetCore.Builder", StringComparison.Ordinal);
    }

    private static bool MatchesServiceProvider(ITypeSymbol typeSymbol)
    {
        if (typeSymbol is not INamedTypeSymbol namedType)
            return false;

        if (!string.Equals(namedType.Name, "IServiceProvider", StringComparison.Ordinal))
            return false;

        var containingNamespace = namedType.ContainingNamespace?.ToDisplayString() ?? string.Empty;
        return string.Equals(containingNamespace, "System", StringComparison.Ordinal);
    }

    private static EquatableImmutableArray<Parameter> GetRequestHandlerParameters(IMethodSymbol methodSymbol, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var methodParameters = new List<Parameter>();
        foreach (var parameter in methodSymbol.Parameters)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var source = BindingSource.None;
            TypedConstant? typedKey = null;

            var attributes = parameter.GetAttributes();
            foreach (var attribute in attributes)
            {
                var attributeClass = attribute.AttributeClass;
                if (attributeClass is null)
                    continue;

                if (attributeClass.IsFromRouteAttribute())
                    source = BindingSource.FromRoute;
                if (attributeClass.IsFromQueryAttribute())
                    source = BindingSource.FromQuery;
                if (attributeClass.IsFromHeaderAttribute())
                    source = BindingSource.FromHeader;
                if (attributeClass.IsFromBodyAttribute())
                    source = BindingSource.FromBody;
                if (attributeClass.IsFromFormAttribute())
                    source = BindingSource.FromForm;
                if (attributeClass.IsFromServicesAttribute())
                    source = BindingSource.FromServices;
                if (attributeClass.IsFromKeyedServicesAttribute())
                {
                    source = BindingSource.FromKeyedServices;
                    typedKey = attribute.ConstructorArguments.Length > 0 ? attribute.ConstructorArguments[0] : null;
                }
                if (attributeClass.IsAsParametersAttribute())
                    source = BindingSource.AsParameters;
            }

            var parameterName = parameter.Name;
            var parameterType = parameter.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            var key = typedKey.HasValue ? ConstLiteral(typedKey.Value) : null;
            methodParameters.Add(new Parameter(parameterName, parameterType, source, key));
        }

        return methodParameters.ToEquatableImmutableArray();
    }

    private static void GenerateSource(SourceProductionContext context, ImmutableArray<RequestHandler> requestHandlers)
    {
        var sorted = requestHandlers.OrderBy(r => r.Class.Name, StringComparer.Ordinal)
            .ThenBy(r => r.Method.Name, StringComparer.Ordinal)
            .ThenBy(r => r.HttpMethod, StringComparer.Ordinal)
            .ThenBy(r => r.Pattern, StringComparer.Ordinal)
            .ToImmutableArray();

        GenerateAddEndpointHandlersClass(context, sorted);
        GenerateUseEndpointHandlersClass(context, sorted);
    }

    private static void GenerateAddEndpointHandlersClass(SourceProductionContext context, ImmutableArray<RequestHandler> requestHandlers)
    {
        var source = GetAddEndpointHandlersStringBuilder(requestHandlers);
        source.AppendLine(FileHeader);

        source.AppendLine();

        source.AppendLine("using Microsoft.Extensions.DependencyInjection;");
        source.AppendLine("using Microsoft.Extensions.DependencyInjection.Extensions;");
        source.AppendLine();

        source.Append("namespace ");
        source.Append(RoutingNamespace);
        source.AppendLine(";");

        source.AppendLine();

        source.Append("internal static class ");
        source.Append(AddEndpointHandlersClassName);
        source.AppendLine();

        source.AppendLine("{");

        source.Append("    internal static void ");
        source.Append(AddEndpointHandlersMethodName);
        source.AppendLine("(this IServiceCollection services)");

        source.AppendLine("    {");

        foreach (var className in requestHandlers.Where(requestHandler => !requestHandler.Class.IsStatic)
                     .Select(x => x.Class.Name)
                     .Distinct())
        {
            source.Append("        services.TryAddScoped<");
            source.Append(className);
            source.Append(">();");
            source.AppendLine();
        }

        source.AppendLine("""
                              }
                          }
                          """
        );

        context.AddSource(AddEndpointHandlersMethodHint, SourceText.From(source.ToString(), Encoding.UTF8));
    }

    private static StringBuilder GetAddEndpointHandlersStringBuilder(ImmutableArray<RequestHandler> requestHandlers)
    {
        var distinctHandlers = requestHandlers.Select(x => x.Class)
            .Where(x => !x.IsStatic)
            .Distinct()
            .ToArray();

        var estimate = 512 + distinctHandlers.Sum(x => 36 + x.Name.Length);

        estimate += Math.Max(256, distinctHandlers.Length * 12);
        estimate = (int)(estimate * 1.10);

        estimate = estimate switch
        {
            < 512 => 512,
            > 8192 => 8192,
            _ => estimate,
        };

        return new StringBuilder(estimate);
    }

    private static void GenerateUseEndpointHandlersClass(SourceProductionContext context, ImmutableArray<RequestHandler> requestHandlers)
    {
        var source = GetUseEndpointHandlersStringBuilder(requestHandlers);
        source.AppendLine(FileHeader);

        source.AppendLine();

        source.AppendLine("using Microsoft.AspNetCore.Builder;");
        source.AppendLine("using Microsoft.AspNetCore.Http;");
        source.AppendLine("using Microsoft.AspNetCore.Mvc;");
        source.AppendLine("using Microsoft.AspNetCore.Routing;");
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

        context.AddSource(UseEndpointHandlersMethodHint, SourceText.From(source.ToString(), Encoding.UTF8));
    }

    private static void GenerateMapRequestHandler(StringBuilder source, RequestHandler requestHandler)
    {
        var wrapWithConfigure = requestHandler.Class.HasConfigureMethod;
        var configureAcceptsServiceProvider = requestHandler.Class.ConfigureMethodAcceptsServiceProvider;
        var indent = wrapWithConfigure ? "            " : "        ";
        var continuationIndent = indent + "    ";

        if (wrapWithConfigure)
        {
            source.Append("        ");
            source.Append(requestHandler.Class.Name);
            source.Append('.');
            source.Append(ConfigureMethodName);
            source.AppendLine("(");
        }

        source.Append(indent);
        source.Append("builder.Map");
        source.Append(requestHandler.HttpMethod is "Get" or "Post" or "Put" or "Delete" or "Patch" ? requestHandler.HttpMethod : "Methods");
        source.Append('(');
        source.Append(StringLiteral(requestHandler.Pattern));
        source.Append(", ");
        if (requestHandler.HttpMethod is "OPTIONS" or "HEAD" or "TRACE" or "CONNECT" or "QUERY")
        {
            source.Append("new[] { \"");
            source.Append(requestHandler.HttpMethod);
            source.Append("\" }, ");
        }
        if (requestHandler.Method.IsStatic)
        {
            source.Append(requestHandler.Class.Name);
            source.Append('.');
            source.Append(requestHandler.Method.Name);
        }
        else
        {
            source.Append("static ");
            if (requestHandler.Method.IsAwaitable)
                source.Append("async ");
            source.Append("([FromServices] ");
            source.Append(requestHandler.Class.Name);
            source.Append(" handler");
            foreach (var parameter in requestHandler.Method.Parameters)
            {
                source.Append(", ");
                source.Append(GetBindingSourceAttribute(parameter.Source, parameter.Key));
                source.Append(parameter.Type);
                source.Append(' ');
                source.Append(parameter.Name);
            }
            source.Append(") => ");
            if (requestHandler.Method.IsAwaitable)
                source.Append("await ");
            source.Append("handler.");
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

        if (!string.IsNullOrEmpty(requestHandler.Metadata.Name))
        {
            source.AppendLine();
            source.Append(continuationIndent);
            source.Append(".WithName(");
            source.Append(StringLiteral(requestHandler.Metadata.Name));
            source.Append(')');
        }

        if (!string.IsNullOrEmpty(requestHandler.Metadata.Summary))
        {
            source.AppendLine();
            source.Append(continuationIndent);
            source.Append(".WithSummary(");
            source.Append(StringLiteral(requestHandler.Metadata.Summary));
            source.Append(')');
        }

        if (!string.IsNullOrEmpty(requestHandler.Metadata.Description))
        {
            source.AppendLine();
            source.Append(continuationIndent);
            source.Append(".WithDescription(");
            source.Append(StringLiteral(requestHandler.Metadata.Description));
            source.Append(')');
        }

        if (requestHandler.Metadata.ExcludeFromDescription)
        {
            source.AppendLine();
            source.Append(continuationIndent);
            source.Append(".ExcludeFromDescription()");
        }

        if (requestHandler.Metadata.Tags is { Count: > 0 })
        {
            source.AppendLine();
            source.Append(continuationIndent);
            source.Append(".WithTags(");
            source.Append(string.Join(", ", requestHandler.Metadata.Tags.Value.Select(StringLiteral)));
            source.Append(')');
        }

        if (requestHandler.Metadata.Accepts is { Count: > 0 })
        {
            foreach (var accepts in requestHandler.Metadata.Accepts.Value)
            {
                source.AppendLine();
                source.Append(continuationIndent);
                source.Append(".Accepts<");
                source.Append(accepts.RequestType);
                source.Append('>');
                source.Append('(');
                if (accepts.IsOptional)
                {
                    source.Append("isOptional: true, ");
                }
                source.Append(StringLiteral(accepts.ContentType));
                AppendAdditionalContentTypes(source, accepts.AdditionalContentTypes);
                source.Append(')');
            }
        }

        if (requestHandler.Metadata.Produces is { Count: > 0 })
        {
            foreach (var produces in requestHandler.Metadata.Produces.Value)
            {
                source.AppendLine();
                source.Append(continuationIndent);
                source.Append(".Produces<");
                source.Append(produces.ResponseType);
                source.Append('>');
                source.Append('(');
                source.Append(produces.StatusCode);
                AppendOptionalContentTypes(source, produces.ContentType, produces.AdditionalContentTypes);
                source.Append(')');
            }
        }

        if (requestHandler.Metadata.ProducesProblem is { Count: > 0 })
        {
            foreach (var producesProblem in requestHandler.Metadata.ProducesProblem.Value)
            {
                source.AppendLine();
                source.Append(continuationIndent);
                source.Append(".ProducesProblem(");
                source.Append(producesProblem.StatusCode);
                AppendOptionalContentTypes(source, producesProblem.ContentType, producesProblem.AdditionalContentTypes);
                source.Append(')');
            }
        }

        if (requestHandler.Metadata.ProducesValidationProblem is { Count: > 0 })
        {
            foreach (var producesValidationProblem in requestHandler.Metadata.ProducesValidationProblem.Value)
            {
                source.AppendLine();
                source.Append(continuationIndent);
                source.Append(".ProducesValidationProblem(");
                source.Append(producesValidationProblem.StatusCode);
                AppendOptionalContentTypes(source, producesValidationProblem.ContentType, producesValidationProblem.AdditionalContentTypes);
                source.Append(')');
            }
        }

        if (requestHandler.RequireAuthorization)
        {
            source.AppendLine();
            if (requestHandler.AuthorizationPolicies is { Count: > 0 })
            {
                source.Append(continuationIndent);
                source.Append(".RequireAuthorization(");
                source.Append(string.Join(", ", requestHandler.AuthorizationPolicies.Value.Select(StringLiteral)));
                source.Append(')');
            }
            else
            {
                source.Append(continuationIndent);
                source.Append(".RequireAuthorization()");
            }
        }

        if (requestHandler.DisableAntiforgery)
        {
            source.AppendLine();
            source.Append(continuationIndent);
            source.Append(".DisableAntiforgery()");
        }

        if (requestHandler.AllowAnonymous)
        {
            source.AppendLine();
            source.Append(continuationIndent);
            source.Append(".AllowAnonymous()");
        }

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

    private static string GetBindingSourceAttribute(BindingSource source, string? key)
    {
        return source switch
        {
            BindingSource.None => "",
            BindingSource.FromRoute => "[FromRoute] ",
            BindingSource.FromQuery => "[FromQuery] ",
            BindingSource.FromHeader => "[FromHeader] ",
            BindingSource.FromBody => "[FromBody] ",
            BindingSource.FromForm => "[FromForm] ",
            BindingSource.FromServices => "[FromServices] ",
            BindingSource.FromKeyedServices => $"[FromKeyedServices({key})] ",
            BindingSource.AsParameters => "[AsParameters] ",
            _ => throw new NotImplementedException(),
        };
    }

    private static StringBuilder GetUseEndpointHandlersStringBuilder(ImmutableArray<RequestHandler> requestHandlers)
    {
        var estimate = 1024;

        foreach (var rh in requestHandlers)
        {
            var cost = 160;

            cost += 2 + rh.Pattern.Length;
            cost += rh.Class.Name.Length + rh.Method.Name.Length;

            var parameters = rh.Method.Parameters;
            foreach (var p in parameters)
                cost += 12 + p.Type.Length + p.Name.Length;

            var metadata = rh.Metadata;
            if (metadata.Name is { Length: > 0 })
                cost += 22 + metadata.Name.Length;
            if (metadata.Summary is { Length: > 0 })
                cost += 24 + metadata.Summary.Length;
            if (metadata.Description is { Length: > 0 })
                cost += 28 + metadata.Description.Length;
            if (metadata.ExcludeFromDescription)
                cost += 32;

            if (metadata.Tags is { Count: > 0 })
                cost += metadata.Tags.Value.Sum(tag => 6 + tag.Length);

            if (metadata.Accepts is { Count: > 0 })
            {
                foreach (var accepts in metadata.Accepts.Value)
                {
                    var additionalCost = accepts.AdditionalContentTypes is { Count: > 0 }
                        ? accepts.AdditionalContentTypes.Value.Sum(ct => 6 + ct.Length)
                        : 0;
                    var optionalCost = accepts.IsOptional ? 20 : 0;
                    cost += 32 + optionalCost + accepts.RequestType.Length + accepts.ContentType.Length + additionalCost;
                }
            }

            if (metadata.Produces is { Count: > 0 })
            {
                foreach (var produces in metadata.Produces.Value)
                {
                    var additionalCost = produces.AdditionalContentTypes is { Count: > 0 }
                        ? produces.AdditionalContentTypes.Value.Sum(ct => 6 + ct.Length)
                        : 0;
                    var contentTypeLength = produces.ContentType?.Length ?? 0;
                    cost += 40 + produces.ResponseType.Length + contentTypeLength + additionalCost;
                }
            }

            if (metadata.ProducesProblem is { Count: > 0 })
            {
                foreach (var producesProblem in metadata.ProducesProblem.Value)
                {
                    var additionalCost = producesProblem.AdditionalContentTypes is { Count: > 0 }
                        ? producesProblem.AdditionalContentTypes.Value.Sum(ct => 6 + ct.Length)
                        : 0;
                    var contentTypeLength = producesProblem.ContentType?.Length ?? 0;
                    cost += 28 + contentTypeLength + additionalCost;
                }
            }

            if (metadata.ProducesValidationProblem is { Count: > 0 })
            {
                foreach (var producesValidationProblem in metadata.ProducesValidationProblem.Value)
                {
                    var additionalCost = producesValidationProblem.AdditionalContentTypes is { Count: > 0 }
                        ? producesValidationProblem.AdditionalContentTypes.Value.Sum(ct => 6 + ct.Length)
                        : 0;
                    var contentTypeLength = producesValidationProblem.ContentType?.Length ?? 0;
                    cost += 32 + contentTypeLength + additionalCost;
                }
            }

            if (rh.RequireAuthorization)
                cost += 24;
            if (rh.AuthorizationPolicies is { Count: > 0 })
                cost += rh.AuthorizationPolicies.Value.Sum(p => 6 + p.Length);
            if (rh.DisableAntiforgery)
                cost += 24;
            if (rh.AllowAnonymous)
                cost += 24;
            if (rh.Class.HasConfigureMethod)
            {
                cost += 32 + rh.Class.Name.Length;
                if (rh.Class.ConfigureMethodAcceptsServiceProvider)
                    cost += 32;
            }

            estimate += cost;
        }

        estimate += Math.Max(256, requestHandlers.Length * 16);
        estimate = (int)(estimate * 1.12);

        estimate = estimate switch
        {
            < 4096 => 4096,
            > 65536 => 65536,
            _ => estimate,
        };

        return new StringBuilder(estimate);
    }

    [SuppressMessage("Globalization", "CA1308: Normalize strings to uppercase", Justification = "C# boolean literals must be lowercase.")]
    private static string ConstLiteral(TypedConstant tc)
    {
        if (tc.IsNull)
            return "null";
        var v = tc.Value;
        var t = tc.Type;
        if (t is null)
            return "null";

        if (t.TypeKind != TypeKind.Enum)
            return t.SpecialType switch
            {
                SpecialType.System_String => StringLiteral((string?)v),
                SpecialType.System_Char => $"'{EscapeChar((char)v!)}'",
                SpecialType.System_Boolean => ((bool)v!).ToString()
                    .ToLowerInvariant(),
                SpecialType.System_Double => ((double)v!).ToString("R", CultureInfo.InvariantCulture),
                SpecialType.System_Single => ((float)v!).ToString("R", CultureInfo.InvariantCulture) + "f",
                SpecialType.System_Decimal => ((decimal)v!).ToString(CultureInfo.InvariantCulture) + "m",
                SpecialType.System_SByte => ((sbyte)v!).ToString(CultureInfo.InvariantCulture),
                SpecialType.System_Byte => ((byte)v!).ToString(CultureInfo.InvariantCulture),
                SpecialType.System_Int16 => ((short)v!).ToString(CultureInfo.InvariantCulture),
                SpecialType.System_UInt16 => ((ushort)v!).ToString(CultureInfo.InvariantCulture),
                SpecialType.System_Int32 => ((int)v!).ToString(CultureInfo.InvariantCulture),
                SpecialType.System_UInt32 => ((uint)v!).ToString(CultureInfo.InvariantCulture) + "u",
                SpecialType.System_Int64 => ((long)v!).ToString(CultureInfo.InvariantCulture) + "L",
                SpecialType.System_UInt64 => ((ulong)v!).ToString(CultureInfo.InvariantCulture) + "UL",
                _ => StringLiteral(v?.ToString()),
            };

        var field = t.GetMembers()
            .OfType<IFieldSymbol>()
            .FirstOrDefault(f => f.HasConstantValue && Equals(f.ConstantValue, v));

        if (field is not null)
            return $"{t.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)}.{field.Name}";

        var underlying = ((INamedTypeSymbol)t).EnumUnderlyingType!;
        var num = IntegralLiteral(v, underlying.SpecialType);
        return $"({t.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)}){num}";
    }

    private static string IntegralLiteral(object? value, SpecialType underlying)
    {
        return underlying switch
        {
            SpecialType.System_SByte => ((sbyte)value!).ToString(CultureInfo.InvariantCulture),
            SpecialType.System_Byte => ((byte)value!).ToString(CultureInfo.InvariantCulture),
            SpecialType.System_Int16 => ((short)value!).ToString(CultureInfo.InvariantCulture),
            SpecialType.System_UInt16 => ((ushort)value!).ToString(CultureInfo.InvariantCulture),
            SpecialType.System_Int32 => ((int)value!).ToString(CultureInfo.InvariantCulture),
            SpecialType.System_UInt32 => ((uint)value!).ToString(CultureInfo.InvariantCulture) + "u",
            SpecialType.System_Int64 => ((long)value!).ToString(CultureInfo.InvariantCulture) + "L",
            SpecialType.System_UInt64 => ((ulong)value!).ToString(CultureInfo.InvariantCulture) + "UL",
            _ => "0",
        };
    }

    private static string StringLiteral(string? value)
    {
        if (value is null)
            return "null";

        var sb = new StringBuilder(value.Length + 2);
        sb.Append('"');
        foreach (var c in value)
        {
            switch (c)
            {
                case '\"':
                    sb.Append("\\\"");
                    break;
                case '\\':
                    sb.Append("\\\\");
                    break;
                case '\n':
                    sb.Append("\\n");
                    break;
                case '\r':
                    sb.Append("\\r");
                    break;
                case '\t':
                    sb.Append("\\t");
                    break;
                case '\0':
                    sb.Append("\\0");
                    break;
                default:
                    if (char.IsControl(c))
                        sb.Append("\\u")
                            .Append(((int)c).ToString("x4", CultureInfo.InvariantCulture));
                    else
                        sb.Append(c);
                    break;
            }
        }
        sb.Append('"');
        return sb.ToString();
    }

    private static void AppendAdditionalContentTypes(StringBuilder source, EquatableImmutableArray<string>? additionalContentTypes)
    {
        if (additionalContentTypes is not { Count: > 0 })
            return;

        foreach (var additional in additionalContentTypes.Value)
        {
            source.Append(", ");
            source.Append(StringLiteral(additional));
        }
    }

    private static void AppendOptionalContentTypes(StringBuilder source, string? contentType, EquatableImmutableArray<string>? additionalContentTypes)
    {
        if (string.IsNullOrEmpty(contentType) && additionalContentTypes is not { Count: > 0 })
            return;

        source.Append(", ");
        source.Append(contentType is { Length: > 0 } ? StringLiteral(contentType) : "null");
        AppendAdditionalContentTypes(source, additionalContentTypes);
    }

    private static string EscapeChar(char c)
    {
        return c switch
        {
            '\'' => "\\'",
            '\\' => "\\\\",
            '\n' => "\\n",
            '\r' => "\\r",
            '\t' => "\\t",
            '\0' => "\\0",
            _ when char.IsControl(c) => "\\u" + ((int)c).ToString("x4", CultureInfo.InvariantCulture),
            _ => c.ToString(),
        };
    }

    private readonly record struct RequestHandler(
        RequestHandlerClass Class,
        RequestHandlerMethod Method,
        string HttpMethod,
        string Pattern,
        RequestHandlerMetadata Metadata,
        bool RequireAuthorization,
        EquatableImmutableArray<string>? AuthorizationPolicies,
        bool DisableAntiforgery,
        bool AllowAnonymous
    );

    private readonly record struct RequestHandlerClass(
        string Name,
        bool IsStatic,
        bool HasConfigureMethod,
        bool ConfigureMethodAcceptsServiceProvider
    );

    private readonly record struct RequestHandlerMethod(string Name, bool IsStatic, bool IsAwaitable, EquatableImmutableArray<Parameter> Parameters);

    private readonly record struct RequestHandlerMetadata(
        string? Name,
        string? Summary,
        string? Description,
        EquatableImmutableArray<string>? Tags,
        EquatableImmutableArray<AcceptsMetadata>? Accepts,
        EquatableImmutableArray<ProducesMetadata>? Produces,
        EquatableImmutableArray<ProducesProblemMetadata>? ProducesProblem,
        EquatableImmutableArray<ProducesValidationProblemMetadata>? ProducesValidationProblem,
        bool ExcludeFromDescription
    );

    private readonly record struct AcceptsMetadata(
        string RequestType,
        string ContentType,
        EquatableImmutableArray<string>? AdditionalContentTypes,
        bool IsOptional);

    private readonly record struct ProducesMetadata(
        string ResponseType,
        int StatusCode,
        string? ContentType,
        EquatableImmutableArray<string>? AdditionalContentTypes
    );

    private readonly record struct ProducesProblemMetadata(int StatusCode, string? ContentType, EquatableImmutableArray<string>? AdditionalContentTypes);

    private readonly record struct ProducesValidationProblemMetadata(int StatusCode, string? ContentType, EquatableImmutableArray<string>? AdditionalContentTypes);

    private readonly record struct Parameter(string Name, string Type, BindingSource Source, string? Key);

    private readonly record struct ConfigureMethodDetails(bool HasConfigureMethod, bool ConfigureMethodAcceptsServiceProvider);

    private enum BindingSource
    {
        None = 0,
        FromRoute = 1,
        FromQuery = 2,
        FromHeader = 3,
        FromBody = 4,
        FromForm = 5,
        FromServices = 6,
        FromKeyedServices = 7,
        AsParameters = 8,
    }
}
