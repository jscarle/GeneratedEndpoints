using System.Text;
using Microsoft.CodeAnalysis.Text;

namespace GeneratedEndpoints.Common;

internal static partial class Constants
{
    internal const string ProducesValidationProblemAttributeName = "ProducesValidationProblemAttribute";
    internal const string ProducesValidationProblemAttributeHint = $"{ProducesValidationProblemAttributeFullyQualifiedName}.gs.cs";

    private const string ProducesValidationProblemAttributeFullyQualifiedName = $"{AttributesNamespace}.{ProducesValidationProblemAttributeName}";

    internal static readonly SourceText ProducesValidationProblemAttributeSourceText = CreateProducesValidationProblemAttributeSourceText();

    private static SourceText CreateProducesValidationProblemAttributeSourceText() => SourceText.From($$"""
                                                                                                         {{FileHeader}}

                                                                                                         namespace {{AttributesNamespace}};

                                                                                                         /// <summary>
                                                                                                         /// Specifies that the endpoint produces a validation problem details payload.
                                                                                                         /// </summary>
                                                                                                         [global::System.AttributeUsage(global::System.AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
                                                                                                         {{GeneratedTypeAttributes}}
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
                                                                                                                 AdditionalContentTypes = additionalContentTypes ?? global::System.Array.Empty<string>();
                                                                                                             }
                                                                                                         }

                                                                                                         """, Encoding.UTF8);
}
