using System.Collections.Immutable;
using System.Text;
using Microsoft.CodeAnalysis.Text;

namespace GeneratedEndpoints.Common;

internal static partial class Constants
{

    internal static readonly ImmutableArray<HttpAttributeDefinition> HttpAttributeDefinitions =
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

    internal static readonly ImmutableDictionary<string, HttpAttributeDefinition> HttpAttributeDefinitionsByName =
        HttpAttributeDefinitions.ToImmutableDictionary(static definition => definition.Name);

    private static HttpAttributeDefinition CreateHttpAttributeDefinition(string attributeName, string verb, bool allowOptionalPattern = false)
    {
        var fullyQualifiedName = $"{AttributesNamespace}.{attributeName}";
        var hint = $"{fullyQualifiedName}.gs.cs";
        var summaryVerb = verb == FallbackHttpMethod ? "fallback" : verb;
        var source = GenerateHttpAttributeSource(AttributesNamespace, attributeName, summaryVerb, allowOptionalPattern);
        return new HttpAttributeDefinition(attributeName, fullyQualifiedName, hint, verb, SourceText.From(source, Encoding.UTF8));
    }

    private static string GenerateHttpAttributeSource(string attributesNamespace, string attributeName, string summaryVerb, bool allowOptionalPattern = false)
    {
        return $$"""
                 {{FileHeader}}

                 namespace {{attributesNamespace}};

                 /// <summary>
                 /// Identifies a method as an HTTP {{summaryVerb}} minimal API endpoint with the specified route pattern.
                 /// </summary>
                 [global::System.AttributeUsage(global::System.AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
                 {{GeneratedTypeAttributes}}
                 internal sealed class {{attributeName}} : global::System.Attribute
                 {
                     /// <summary>
                     /// Gets the route pattern for the endpoint.
                     /// </summary>
                     public string{{(allowOptionalPattern ? "?" : "")}} Pattern { get; }

                     /// <summary>
                     /// Gets or sets the endpoint name.
                     /// </summary>
                     public string? Name { get; init; }

                     /// <summary>
                     /// Initializes a new instance of the <see cref="{{attributeName}}"/> class.
                     /// </summary>
                     /// <param name="pattern">The route pattern for the endpoint.</param>
                     public {{attributeName}}([global::System.Diagnostics.CodeAnalysis.StringSyntax("Route")] string{{(allowOptionalPattern ? "?" : "")}} pattern{{(allowOptionalPattern ? " = null" : "")}})
                      {
                          Pattern = pattern;
                      }
                 }
                 """;
    }
}
