using System.Text;
using Microsoft.CodeAnalysis.Text;

namespace GeneratedEndpoints.Common;

internal static partial class Constants
{
    internal const string RequireHostAttributeName = "RequireHostAttribute";
    internal const string RequireHostAttributeHint = $"{RequireHostAttributeFullyQualifiedName}.gs.cs";

    private const string RequireHostAttributeFullyQualifiedName = $"{AttributesNamespace}.{RequireHostAttributeName}";

    internal static readonly SourceText RequireHostAttributeSourceText = CreateRequireHostAttributeSourceText();

    private static SourceText CreateRequireHostAttributeSourceText() => SourceText.From($$"""
                                                                                           {{FileHeader}}

                                                                                           namespace {{AttributesNamespace}};

                                                                                           /// <summary>
                                                                                           /// Specifies the allowed hosts for the annotated endpoint or class.
                                                                                           /// </summary>
                                                                                           [global::System.AttributeUsage(global::System.AttributeTargets.Class | global::System.AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
                                                                                           {{GeneratedTypeAttributes}}
                                                                                           internal sealed class {{RequireHostAttributeName}} : global::System.Attribute
                                                                                           {
                                                                                               /// <summary>
                                                                                               /// Initializes a new instance of the <see cref="{{RequireHostAttributeName}}"/> class.
                                                                                               /// </summary>
                                                                                               /// <param name="hosts">The hosts that are allowed to access the endpoint.</param>
                                                                                               public {{RequireHostAttributeName}}(params string[] hosts)
                                                                                               {
                                                                                                   Hosts = hosts ?? global::System.Array.Empty<string>();
                                                                                               }

                                                                                               /// <summary>
                                                                                               /// Gets the allowed hosts.
                                                                                               /// </summary>
                                                                                               public string[] Hosts { get; }
                                                                                           }
                                                                                           """, Encoding.UTF8);
}
