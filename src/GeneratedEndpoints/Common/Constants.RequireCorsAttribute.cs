using System.Text;
using Microsoft.CodeAnalysis.Text;

namespace GeneratedEndpoints.Common;

internal static partial class Constants
{
    internal const string RequireCorsAttributeName = "RequireCorsAttribute";
    internal const string RequireCorsAttributeHint = $"{RequireCorsAttributeFullyQualifiedName}.gs.cs";

    private const string RequireCorsAttributeFullyQualifiedName = $"{AttributesNamespace}.{RequireCorsAttributeName}";

    internal static readonly SourceText RequireCorsAttributeSourceText = CreateRequireCorsAttributeSourceText();

    private static SourceText CreateRequireCorsAttributeSourceText() => SourceText.From($$"""
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
                                                                                           """, Encoding.UTF8);
}
