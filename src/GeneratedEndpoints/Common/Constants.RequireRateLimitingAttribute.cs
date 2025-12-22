using System.Text;
using Microsoft.CodeAnalysis.Text;

namespace GeneratedEndpoints.Common;

internal static partial class Constants
{
    internal const string RequireRateLimitingAttributeName = "RequireRateLimitingAttribute";
    internal const string RequireRateLimitingAttributeHint = $"{RequireRateLimitingAttributeFullyQualifiedName}.gs.cs";

    private const string RequireRateLimitingAttributeFullyQualifiedName = $"{AttributesNamespace}.{RequireRateLimitingAttributeName}";

    internal static readonly SourceText RequireRateLimitingAttributeSourceText = CreateRequireRateLimitingAttributeSourceText();

    private static SourceText CreateRequireRateLimitingAttributeSourceText() => SourceText.From($$"""
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
                                                                                                   """, Encoding.UTF8);
}
