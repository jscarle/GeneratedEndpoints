using System.Text;
using Microsoft.CodeAnalysis.Text;

namespace GeneratedEndpoints.Common;

internal static partial class Constants
{
    internal const string ProducesResponseAttributeName = "ProducesResponseAttribute";
    internal const string ProducesResponseAttributeHint = $"{ProducesResponseAttributeFullyQualifiedName}.gs.cs";

    private const string ProducesResponseAttributeFullyQualifiedName = $"{AttributesNamespace}.{ProducesResponseAttributeName}";

    internal static readonly SourceText ProducesResponseAttributeSourceText = CreateProducesResponseAttributeSourceText();

    private static SourceText CreateProducesResponseAttributeSourceText() => SourceText.From($$"""
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
                                                                                                    public global::System.Type Type { get; }

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
                                                                                                    /// <param name="type">The CLR type of the response body.</param>
                                                                                                    /// <param name="statusCode">The HTTP status code returned by the endpoint.</param>
                                                                                                    /// <param name="contentType">The primary content type produced by the endpoint.</param>
                                                                                                    /// <param name="additionalContentTypes">Additional content types produced by the endpoint.</param>
                                                                                                    public {{ProducesResponseAttributeName}}(global::System.Type type, int statusCode = global::Microsoft.AspNetCore.Http.StatusCodes.Status200OK, string? contentType = null, params string[] additionalContentTypes)
                                                                                                    {
                                                                                                        Type = type;
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
                                                                                                    public global::System.Type Type => typeof(TResponse);

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

                                                                                                """, Encoding.UTF8);
}
