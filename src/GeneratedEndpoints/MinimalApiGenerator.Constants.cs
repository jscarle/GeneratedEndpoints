using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace GeneratedEndpoints;

public sealed partial class MinimalApiGenerator
{
    private const string BaseNamespace = "Microsoft.AspNetCore.Generated";
    private const string AttributesNamespace = $"{BaseNamespace}.Attributes";

    private const string FallbackHttpMethod = "__FALLBACK__";

    private const string NameAttributeNamedParameter = "Name";
    private const string ResponseTypeAttributeNamedParameter = "ResponseType";
    private const string RequestTypeAttributeNamedParameter = "RequestType";
    private const string IsOptionalAttributeNamedParameter = "IsOptional";
    private const string PolicyNameAttributeNamedParameter = "PolicyName";

    private const string RequireAuthorizationAttributeName = "RequireAuthorizationAttribute";
    private const string RequireAuthorizationAttributeFullyQualifiedName = $"{AttributesNamespace}.{RequireAuthorizationAttributeName}";
    private const string RequireAuthorizationAttributeHint = $"{RequireAuthorizationAttributeFullyQualifiedName}.gs.cs";

    private const string RequireCorsAttributeName = "RequireCorsAttribute";
    private const string RequireCorsAttributeFullyQualifiedName = $"{AttributesNamespace}.{RequireCorsAttributeName}";
    private const string RequireCorsAttributeHint = $"{RequireCorsAttributeFullyQualifiedName}.gs.cs";

    private const string RequireRateLimitingAttributeName = "RequireRateLimitingAttribute";
    private const string RequireRateLimitingAttributeFullyQualifiedName = $"{AttributesNamespace}.{RequireRateLimitingAttributeName}";
    private const string RequireRateLimitingAttributeHint = $"{RequireRateLimitingAttributeFullyQualifiedName}.gs.cs";

    private const string RequireHostAttributeName = "RequireHostAttribute";
    private const string RequireHostAttributeFullyQualifiedName = $"{AttributesNamespace}.{RequireHostAttributeName}";
    private const string RequireHostAttributeHint = $"{RequireHostAttributeFullyQualifiedName}.gs.cs";

    private const string DisableAntiforgeryAttributeName = "DisableAntiforgeryAttribute";
    private const string DisableAntiforgeryAttributeFullyQualifiedName = $"{AttributesNamespace}.{DisableAntiforgeryAttributeName}";
    private const string DisableAntiforgeryAttributeHint = $"{DisableAntiforgeryAttributeFullyQualifiedName}.gs.cs";

    private const string ShortCircuitAttributeName = "ShortCircuitAttribute";
    private const string ShortCircuitAttributeFullyQualifiedName = $"{AttributesNamespace}.{ShortCircuitAttributeName}";
    private const string ShortCircuitAttributeHint = $"{ShortCircuitAttributeFullyQualifiedName}.gs.cs";

    private const string DisableRequestTimeoutAttributeName = "DisableRequestTimeoutAttribute";
    private const string DisableRequestTimeoutAttributeFullyQualifiedName = $"{AttributesNamespace}.{DisableRequestTimeoutAttributeName}";
    private const string DisableRequestTimeoutAttributeHint = $"{DisableRequestTimeoutAttributeFullyQualifiedName}.gs.cs";

    private const string DisableValidationAttributeName = "DisableValidationAttribute";
    private const string DisableValidationAttributeFullyQualifiedName = $"{AttributesNamespace}.{DisableValidationAttributeName}";
    private const string DisableValidationAttributeHint = $"{DisableValidationAttributeFullyQualifiedName}.gs.cs";

    private const string RequestTimeoutAttributeName = "RequestTimeoutAttribute";
    private const string RequestTimeoutAttributeFullyQualifiedName = $"{AttributesNamespace}.{RequestTimeoutAttributeName}";
    private const string RequestTimeoutAttributeHint = $"{RequestTimeoutAttributeFullyQualifiedName}.gs.cs";

    private const string OrderAttributeName = "OrderAttribute";
    private const string OrderAttributeFullyQualifiedName = $"{AttributesNamespace}.{OrderAttributeName}";
    private const string OrderAttributeHint = $"{OrderAttributeFullyQualifiedName}.gs.cs";

    private const string MapGroupAttributeName = "MapGroupAttribute";
    private const string MapGroupAttributeFullyQualifiedName = $"{AttributesNamespace}.{MapGroupAttributeName}";
    private const string MapGroupAttributeHint = $"{MapGroupAttributeFullyQualifiedName}.gs.cs";

    private const string SummaryAttributeName = "SummaryAttribute";
    private const string SummaryAttributeFullyQualifiedName = $"{AttributesNamespace}.{SummaryAttributeName}";
    private const string SummaryAttributeHint = $"{SummaryAttributeFullyQualifiedName}.gs.cs";

    private const string AllowAnonymousAttributeName = "AllowAnonymousAttribute";

    private const string EndpointFilterAttributeName = "EndpointFilterAttribute";
    private const string EndpointFilterAttributeFullyQualifiedName = $"{AttributesNamespace}.{EndpointFilterAttributeName}";
    private const string EndpointFilterAttributeHint = $"{EndpointFilterAttributeFullyQualifiedName}.gs.cs";

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

    private const string ProducesValidationProblemAttributeFullyQualifiedName = $"{AttributesNamespace}.{ProducesValidationProblemAttributeName}";

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
    private const string GlobalPrefix = "global::";
    private static readonly string[] AttributesNamespaceParts = AttributesNamespace.Split('.');
    private static readonly string[] AspNetCoreHttpNamespaceParts = ["Microsoft", "AspNetCore", "Http"];
    private static readonly string[] AspNetCoreAuthorizationNamespaceParts = ["Microsoft", "AspNetCore", "Authorization"];
    private static readonly string[] AspNetCoreRoutingNamespaceParts = ["Microsoft", "AspNetCore", "Routing"];
    private static readonly string[] ComponentModelNamespaceParts = ["System", "ComponentModel"];
    private static readonly ConditionalWeakTable<Compilation, CompilationTypeCache> CompilationTypeCaches = new();
    private static readonly ConditionalWeakTable<INamedTypeSymbol, RequestHandlerClassCacheEntry> RequestHandlerClassCache = new();
    private static readonly ConditionalWeakTable<INamedTypeSymbol, GeneratedAttributeKindCacheEntry> GeneratedAttributeKindCache = new();

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

    private static readonly ImmutableArray<HttpAttributeDefinition> HttpAttributeDefinitions =
    [
        CreateHttpAttributeDefinition("MapGetAttribute", "GET"),
        CreateHttpAttributeDefinition("MapPostAttribute", "POST"),
        CreateHttpAttributeDefinition("MapPutAttribute", "PUT"),
        CreateHttpAttributeDefinition("MapPatchAttribute", "PATCH"),
        CreateHttpAttributeDefinition("MapDeleteAttribute", "DELETE"),
        CreateHttpAttributeDefinition("MapOptionsAttribute", "OPTIONS"),
        CreateHttpAttributeDefinition("MapHeadAttribute", "HEAD"),
        CreateHttpAttributeDefinition("MapQueryAttribute", "QUERY"),
        CreateHttpAttributeDefinition("MapTraceAttribute", "TRACE"),
        CreateHttpAttributeDefinition("MapConnectAttribute", "CONNECT"),
        CreateHttpAttributeDefinition("MapFallbackAttribute", FallbackHttpMethod, true),
    ];

    private static readonly ImmutableDictionary<string, HttpAttributeDefinition> HttpAttributeDefinitionsByName =
        HttpAttributeDefinitions.ToImmutableDictionary(static definition => definition.Name);

    private static readonly SourceText RequireAuthorizationAttributeSourceText = SourceText.From($$"""
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
                                                                                                   """, Encoding.UTF8
    );

    private static readonly SourceText RequireCorsAttributeSourceText = SourceText.From($$"""
                                                                                          {{FileHeader}}

                                                                                          namespace {{AttributesNamespace}};

                                                                                          /// <summary>
                                                                                          /// Specifies that the annotated endpoint requires a configured CORS policy.
                                                                                          /// </summary>
                                                                                          [global::System.AttributeUsage(global::System.AttributeTargets.Class | global::System.AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
                                                                                          internal sealed class {{RequireCorsAttributeName}} : global::System.Attribute
                                                                                          {
                                                                                              /// <summary>
                                                                                              /// Gets the optional CORS policy name.
                                                                                              /// </summary>
                                                                                              public string? PolicyName { get; }

                                                                                              /// <summary>
                                                                                              /// Marks the endpoint or class as requiring the default CORS policy.
                                                                                              /// </summary>
                                                                                              public {{RequireCorsAttributeName}}()
                                                                                              {
                                                                                              }

                                                                                              /// <summary>
                                                                                              /// Marks the endpoint or class as requiring the specified named CORS policy.
                                                                                              /// </summary>
                                                                                              public {{RequireCorsAttributeName}}(string policyName)
                                                                                              {
                                                                                                  PolicyName = policyName;
                                                                                              }
                                                                                          }
                                                                                          """, Encoding.UTF8
    );

    private static readonly SourceText RequireRateLimitingAttributeSourceText = SourceText.From($$"""
                                                                                                  {{FileHeader}}

                                                                                                  namespace {{AttributesNamespace}};

                                                                                                  /// <summary>
                                                                                                  /// Specifies that the annotated endpoint requires the provided rate limiting policy.
                                                                                                  /// </summary>
                                                                                                  [global::System.AttributeUsage(global::System.AttributeTargets.Class | global::System.AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
                                                                                                  internal sealed class {{RequireRateLimitingAttributeName}} : global::System.Attribute
                                                                                                  {
                                                                                                      /// <summary>
                                                                                                      /// Initializes a new instance of the <see cref="{{RequireRateLimitingAttributeName}}"/> class.
                                                                                                      /// </summary>
                                                                                                      /// <param name="policyName">The rate limiting policy to apply.</param>
                                                                                                      public {{RequireRateLimitingAttributeName}}(string policyName)
                                                                                                      {
                                                                                                          PolicyName = policyName;
                                                                                                      }

                                                                                                      /// <summary>
                                                                                                      /// Gets the rate limiting policy name.
                                                                                                      /// </summary>
                                                                                                      public string PolicyName { get; }
                                                                                                  }
                                                                                                  """, Encoding.UTF8
    );

    private static readonly SourceText RequireHostAttributeSourceText = SourceText.From($$"""
                                                                                          {{FileHeader}}

                                                                                          namespace {{AttributesNamespace}};

                                                                                          /// <summary>
                                                                                          /// Specifies the allowed hosts for the annotated endpoint or class.
                                                                                          /// </summary>
                                                                                          [global::System.AttributeUsage(global::System.AttributeTargets.Class | global::System.AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
                                                                                          internal sealed class {{RequireHostAttributeName}} : global::System.Attribute
                                                                                          {
                                                                                              /// <summary>
                                                                                              /// Initializes a new instance of the <see cref="{{RequireHostAttributeName}}"/> class.
                                                                                              /// </summary>
                                                                                              /// <param name="hosts">The hosts that are allowed to access the endpoint.</param>
                                                                                              public {{RequireHostAttributeName}}(params string[] hosts)
                                                                                              {
                                                                                                  Hosts = hosts ?? [];
                                                                                              }

                                                                                              /// <summary>
                                                                                              /// Gets the allowed hosts.
                                                                                              /// </summary>
                                                                                              public string[] Hosts { get; }
                                                                                          }
                                                                                          """, Encoding.UTF8
    );

    private static readonly SourceText DisableAntiforgeryAttributeSourceText = SourceText.From($$"""
                                                                                                 {{FileHeader}}

                                                                                                 namespace {{AttributesNamespace}};

                                                                                                 /// <summary>
                                                                                                 /// Disables antiforgery protection for the annotated endpoint or class.
                                                                                                 /// </summary>
                                                                                                 [global::System.AttributeUsage(global::System.AttributeTargets.Class | global::System.AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
                                                                                                 internal sealed class {{DisableAntiforgeryAttributeName}} : global::System.Attribute
                                                                                                 {
                                                                                                 }

                                                                                                 """, Encoding.UTF8
    );

    private static readonly SourceText ShortCircuitAttributeSourceText = SourceText.From($$"""
                                                                                           {{FileHeader}}

                                                                                           namespace {{AttributesNamespace}};

                                                                                           /// <summary>
                                                                                           /// Marks the annotated endpoint or class to short-circuit the request pipeline.
                                                                                           /// </summary>
                                                                                           [global::System.AttributeUsage(global::System.AttributeTargets.Class | global::System.AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
                                                                                           internal sealed class {{ShortCircuitAttributeName}} : global::System.Attribute
                                                                                           {
                                                                                           }

                                                                                           """, Encoding.UTF8
    );

    private static readonly SourceText DisableRequestTimeoutAttributeSourceText = SourceText.From($$"""
                                                                                                    {{FileHeader}}

                                                                                                    namespace {{AttributesNamespace}};

                                                                                                    /// <summary>
                                                                                                    /// Disables the request timeout for the annotated endpoint or class.
                                                                                                    /// </summary>
                                                                                                    [global::System.AttributeUsage(global::System.AttributeTargets.Class | global::System.AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
                                                                                                    internal sealed class {{DisableRequestTimeoutAttributeName}} : global::System.Attribute
                                                                                                    {
                                                                                                    }

                                                                                                    """, Encoding.UTF8
    );

    private static readonly SourceText DisableValidationAttributeSourceText = SourceText.From($$"""
                                                                                                #if NET10_0_OR_GREATER
                                                                                                {{FileHeader}}

                                                                                                namespace {{AttributesNamespace}};

                                                                                                /// <summary>
                                                                                                /// Disables request validation for the annotated endpoint or class when targeting .NET 10 or later.
                                                                                                /// </summary>
                                                                                                [global::System.AttributeUsage(global::System.AttributeTargets.Class | global::System.AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
                                                                                                internal sealed class {{DisableValidationAttributeName}} : global::System.Attribute
                                                                                                {
                                                                                                }
                                                                                                #endif

                                                                                                """, Encoding.UTF8
    );

    private static readonly SourceText RequestTimeoutAttributeSourceText = SourceText.From($$"""
                                                                                             {{FileHeader}}

                                                                                             namespace {{AttributesNamespace}};

                                                                                             /// <summary>
                                                                                             /// Applies the request timeout metadata to the annotated endpoint or class.
                                                                                             /// </summary>
                                                                                             [global::System.AttributeUsage(global::System.AttributeTargets.Class | global::System.AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
                                                                                             internal sealed class {{RequestTimeoutAttributeName}} : global::System.Attribute
                                                                                             {
                                                                                                 /// <summary>
                                                                                                 /// Gets the optional request timeout policy name.
                                                                                                 /// </summary>
                                                                                                 public string? PolicyName { get; init; }

                                                                                                 /// <summary>
                                                                                                 /// Applies the default request timeout behavior.
                                                                                                 /// </summary>
                                                                                                 public {{RequestTimeoutAttributeName}}()
                                                                                                 {
                                                                                                 }

                                                                                                 /// <summary>
                                                                                                 /// Applies the specified request timeout policy.
                                                                                                 /// </summary>
                                                                                                 /// <param name="policyName">The request timeout policy name.</param>
                                                                                                 public {{RequestTimeoutAttributeName}}(string policyName)
                                                                                                 {
                                                                                                     PolicyName = policyName;
                                                                                                 }
                                                                                             }

                                                                                             """, Encoding.UTF8
    );

    private static readonly SourceText OrderAttributeSourceText = SourceText.From($$"""
                                                                                    {{FileHeader}}

                                                                                    namespace {{AttributesNamespace}};

                                                                                    /// <summary>
                                                                                    /// Specifies the order for the annotated endpoint when building conventions.
                                                                                    /// </summary>
                                                                                    [global::System.AttributeUsage(global::System.AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
                                                                                    internal sealed class {{OrderAttributeName}} : global::System.Attribute
                                                                                    {
                                                                                        /// <summary>
                                                                                        /// Gets the order that will be applied to the endpoint.
                                                                                        /// </summary>
                                                                                        public int Order { get; }

                                                                                        /// <summary>
                                                                                        /// Initializes a new instance of the <see cref="{{OrderAttributeName}}"/> class.
                                                                                        /// </summary>
                                                                                        /// <param name="order">The order value to apply to the endpoint.</param>
                                                                                        public {{OrderAttributeName}}(int order)
                                                                                        {
                                                                                            Order = order;
                                                                                        }
                                                                                    }

                                                                                    """, Encoding.UTF8
    );

    private static readonly SourceText MapGroupAttributeSourceText = SourceText.From($$"""
                                                                                       {{FileHeader}}

                                                                                       namespace {{AttributesNamespace}};

                                                                                       /// <summary>
                                                                                       /// Specifies the route group for the annotated class.
                                                                                       /// </summary>
                                                                                       [global::System.AttributeUsage(global::System.AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
                                                                                       internal sealed class {{MapGroupAttributeName}} : global::System.Attribute
                                                                                       {
                                                                                           /// <summary>
                                                                                           /// Gets the route group pattern.
                                                                                           /// </summary>
                                                                                           public string Pattern { get; }

                                                                                           /// <summary>
                                                                                           /// Gets or sets the endpoint group name.
                                                                                           /// </summary>
                                                                                           public string? Name { get; init; }

                                                                                           /// <summary>
                                                                                           /// Initializes a new instance of the <see cref="{{MapGroupAttributeName}}"/> class.
                                                                                           /// </summary>
                                                                                           /// <param name="pattern">The route group pattern to apply.</param>
                                                                                           public {{MapGroupAttributeName}}(string pattern)
                                                                                           {
                                                                                               Pattern = pattern;
                                                                                           }
                                                                                       }

                                                                                       """, Encoding.UTF8
    );

    private static readonly SourceText SummaryAttributeSourceText = SourceText.From($$"""
                                                                                      {{FileHeader}}

                                                                                      namespace {{AttributesNamespace}};

                                                                                      /// <summary>
                                                                                      /// Specifies the summary metadata for the annotated endpoint.
                                                                                      /// </summary>
                                                                                      [global::System.AttributeUsage(global::System.AttributeTargets.Class | global::System.AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
                                                                                      internal sealed class {{SummaryAttributeName}} : global::System.Attribute
                                                                                      {
                                                                                          /// <summary>
                                                                                          /// Gets the summary value for the endpoint.
                                                                                          /// </summary>
                                                                                          public string Summary { get; }

                                                                                          /// <summary>
                                                                                          /// Initializes a new instance of the <see cref="{{SummaryAttributeName}}"/> class.
                                                                                          /// </summary>
                                                                                          /// <param name="summary">The summary to apply to the endpoint.</param>
                                                                                          public {{SummaryAttributeName}}(string summary)
                                                                                          {
                                                                                              Summary = summary;
                                                                                          }
                                                                                      }

                                                                                      """, Encoding.UTF8
    );

    private static readonly SourceText AcceptsAttributeSourceText = SourceText.From($$"""
                                                                                      {{FileHeader}}

                                                                                      namespace {{AttributesNamespace}};

                                                                                      /// <summary>
                                                                                      /// Specifies the request type and content types accepted by the annotated endpoint or class.
                                                                                      /// </summary>
                                                                                      [global::System.AttributeUsage(global::System.AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
                                                                                      internal sealed class {{AcceptsAttributeName}} : global::System.Attribute
                                                                                      {
                                                                                          /// <summary>
                                                                                          /// Gets the request type accepted by the endpoint.
                                                                                          /// </summary>
                                                                                          public global::System.Type? RequestType { get; init; }

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
                                                                                      [global::System.AttributeUsage(global::System.AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
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

                                                                                      """, Encoding.UTF8
    );

    private static readonly SourceText EndpointFilterAttributeSourceText = SourceText.From($$"""
                                                                                             {{FileHeader}}

                                                                                             namespace {{AttributesNamespace}};

                                                                                             /// <summary>
                                                                                             /// Specifies an endpoint filter type to apply to the annotated endpoint or class.
                                                                                             /// </summary>
                                                                                             [global::System.AttributeUsage(global::System.AttributeTargets.Class | global::System.AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
                                                                                             internal sealed class {{EndpointFilterAttributeName}} : global::System.Attribute
                                                                                             {
                                                                                                 /// <summary>
                                                                                                 /// Gets the CLR type of the endpoint filter.
                                                                                                 /// </summary>
                                                                                                 public global::System.Type FilterType { get; }

                                                                                                 /// <summary>
                                                                                                 /// Initializes a new instance of the <see cref="{{EndpointFilterAttributeName}}"/> class.
                                                                                                 /// </summary>
                                                                                                 /// <param name="filterType">The CLR type of the endpoint filter.</param>
                                                                                                 public {{EndpointFilterAttributeName}}(global::System.Type filterType)
                                                                                                 {
                                                                                                     FilterType = filterType ?? throw new global::System.ArgumentNullException(nameof(filterType));
                                                                                                 }
                                                                                             }

                                                                                             /// <summary>
                                                                                             /// Specifies an endpoint filter type using a generic argument.
                                                                                             /// </summary>
                                                                                             /// <typeparam name="TFilter">The CLR type of the endpoint filter.</typeparam>
                                                                                             [global::System.AttributeUsage(global::System.AttributeTargets.Class | global::System.AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
                                                                                             internal sealed class {{EndpointFilterAttributeName}}<TFilter> : global::System.Attribute
                                                                                             {
                                                                                                 /// <summary>
                                                                                                 /// Gets the CLR type of the endpoint filter.
                                                                                                 /// </summary>
                                                                                                 public global::System.Type FilterType => typeof(TFilter);
                                                                                             }

                                                                                             """, Encoding.UTF8
    );

    private static readonly SourceText ProducesResponseAttributeSourceText = SourceText.From($$"""
                                                                                               {{FileHeader}}

                                                                                               namespace {{AttributesNamespace}};

                                                                                               /// <summary>
                                                                                               /// Specifies a response type, status code, and content types produced by the annotated endpoint or class.
                                                                                               /// </summary>
                                                                                               [global::System.AttributeUsage(global::System.AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
                                                                                               internal sealed class {{ProducesResponseAttributeName}} : global::System.Attribute
                                                                                               {
                                                                                               /// <summary>
                                                                                               /// Gets the response type produced by the endpoint.
                                                                                               /// </summary>
                                                                                               public global::System.Type? ResponseType { get; init; }

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
                                                                                                   public {{ProducesResponseAttributeName}}(int statusCode = global::Microsoft.AspNetCore.Http.StatusCodes.Status200OK, string? contentType = null, params string[] additionalContentTypes)
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
                                                                                               [global::System.AttributeUsage(global::System.AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
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
                                                                                                   public {{ProducesResponseAttributeName}}(int statusCode = global::Microsoft.AspNetCore.Http.StatusCodes.Status200OK, string? contentType = null, params string[] additionalContentTypes)
                                                                                                   {
                                                                                                       StatusCode = statusCode;
                                                                                                       ContentType = contentType;
                                                                                                       AdditionalContentTypes = additionalContentTypes ?? [];
                                                                                                   }
                                                                                               }

                                                                                               """, Encoding.UTF8
    );

    private static readonly SourceText ProducesProblemAttributeSourceText = SourceText.From($$"""
                                                                                              {{FileHeader}}

                                                                                              namespace {{AttributesNamespace}};

                                                                                              /// <summary>
                                                                                              /// Specifies that the endpoint produces a problem details payload.
                                                                                              /// </summary>
                                                                                              [global::System.AttributeUsage(global::System.AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
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
                                                                                                  public {{ProducesProblemAttributeName}}(int statusCode = global::Microsoft.AspNetCore.Http.StatusCodes.Status500InternalServerError, string? contentType = null, params string[] additionalContentTypes)
                                                                                                  {
                                                                                                      StatusCode = statusCode;
                                                                                                      ContentType = contentType;
                                                                                                      AdditionalContentTypes = additionalContentTypes ?? [];
                                                                                                  }
                                                                                              }

                                                                                              """, Encoding.UTF8
    );

    private static readonly SourceText ProducesValidationProblemAttributeSourceText = SourceText.From($$"""
                                                                                                        {{FileHeader}}

                                                                                                        namespace {{AttributesNamespace}};

                                                                                                        /// <summary>
                                                                                                        /// Specifies that the endpoint produces a validation problem details payload.
                                                                                                        /// </summary>
                                                                                                        [global::System.AttributeUsage(global::System.AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
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
                                                                                                            public {{ProducesValidationProblemAttributeName}}(int statusCode = global::Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest, string? contentType = null, params string[] additionalContentTypes)
                                                                                                            {
                                                                                                                StatusCode = statusCode;
                                                                                                                ContentType = contentType;
                                                                                                                AdditionalContentTypes = additionalContentTypes ?? [];
                                                                                                            }
                                                                                                        }

                                                                                                        """, Encoding.UTF8
    );

}
