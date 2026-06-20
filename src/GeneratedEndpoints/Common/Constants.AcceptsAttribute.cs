using System.Text;
using Microsoft.CodeAnalysis.Text;

namespace GeneratedEndpoints.Common;

internal static partial class Constants
{
    internal const string AcceptsAttributeName = "AcceptsAttribute";
    internal const string AcceptsAttributeHint = $"{AcceptsAttributeFullyQualifiedName}.gs.cs";

    private const string AcceptsAttributeFullyQualifiedName = $"{AttributesNamespace}.{AcceptsAttributeName}";

    internal static readonly SourceText AcceptsAttributeSourceText = CreateAcceptsAttributeSourceText();

    private static SourceText CreateAcceptsAttributeSourceText() => SourceText.From($$"""
                                                                                       {{FileHeader}}

                                                                                       namespace {{AttributesNamespace}};

                                                                                       /// <summary>
                                                                                       /// Specifies the request type and content types accepted by the annotated endpoint or class.
                                                                                       /// </summary>
                                                                                       [global::System.AttributeUsage(global::System.AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
                                                                                       {{GeneratedTypeAttributes}}
                                                                                       internal sealed class {{AcceptsAttributeName}} : global::System.Attribute
                                                                                       {
                                                                                           /// <summary>
                                                                                           /// Gets the CLR type of the endpoint filter.
                                                                                           /// </summary>
                                                                                           public global::System.Type Type { get; }

                                                                                           /// <summary>
                                                                                           /// Gets the primary content type accepted by the endpoint.
                                                                                           /// </summary>
                                                                                           public string ContentType { get; }

                                                                                           /// <summary>
                                                                                           /// Gets the additional content types accepted by the endpoint.
                                                                                           /// </summary>
                                                                                           public string[] AdditionalContentTypes { get; }

                                                                                           /// <summary>
                                                                                           /// Gets a value indicating whether the request body is optional.
                                                                                           /// </summary>
                                                                                           public bool IsOptional { get; init; }

                                                                                           /// <summary>
                                                                                           /// Initializes a new instance of the <see cref="{{AcceptsAttributeName}}"/> class.
                                                                                           /// </summary>
                                                                                           /// <param name="type">The CLR type of the request body.</param>
                                                                                           /// <param name="contentType">The primary content type accepted by the endpoint.</param>
                                                                                           /// <param name="additionalContentTypes">Additional content types accepted by the endpoint.</param>
                                                                                           public {{AcceptsAttributeName}}(global::System.Type type, string contentType = "application/json", params string[] additionalContentTypes)
                                                                                           {
                                                                                               Type = type;
                                                                                               ContentType = contentType;
                                                                                               AdditionalContentTypes = additionalContentTypes;
                                                                                           }
                                                                                       }

                                                                                       /// <summary>
                                                                                       /// Specifies the request type using a generic argument and the content types accepted by the annotated endpoint or class.
                                                                                       /// </summary>
                                                                                       /// <typeparam name="TRequest">The CLR type of the request body.</typeparam>
                                                                                       [global::System.AttributeUsage(global::System.AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
                                                                                       {{GeneratedTypeAttributes}}
                                                                                       internal sealed class {{AcceptsAttributeName}}<TRequest> : global::System.Attribute
                                                                                       {
                                                                                           /// <summary>
                                                                                           /// Gets the CLR type of the endpoint filter.
                                                                                           /// </summary>
                                                                                           public global::System.Type Type => typeof(TRequest);

                                                                                           /// <summary>
                                                                                           /// Gets the primary content type accepted by the endpoint.
                                                                                           /// </summary>
                                                                                           public string ContentType { get; }

                                                                                           /// <summary>
                                                                                           /// Gets the additional content types accepted by the endpoint.
                                                                                           /// </summary>
                                                                                           public string[] AdditionalContentTypes { get; }

                                                                                           /// <summary>
                                                                                           /// Gets a value indicating whether the request body is optional.
                                                                                           /// </summary>
                                                                                           public bool IsOptional { get; init; }

                                                                                           /// <summary>
                                                                                           /// Initializes a new instance of the generic Accepts attribute class.
                                                                                           /// </summary>
                                                                                           /// <param name="contentType">The primary content type accepted by the endpoint.</param>
                                                                                           /// <param name="additionalContentTypes">Additional content types accepted by the endpoint.</param>
                                                                                           public {{AcceptsAttributeName}}(string contentType = "application/json", params string[] additionalContentTypes)
                                                                                           {
                                                                                               ContentType = contentType;
                                                                                               AdditionalContentTypes = additionalContentTypes;
                                                                                           }
                                                                                       }

                                                                                       """, Encoding.UTF8);
}
