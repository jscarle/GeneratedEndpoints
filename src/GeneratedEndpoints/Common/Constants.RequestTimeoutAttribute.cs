using System.Text;
using Microsoft.CodeAnalysis.Text;

namespace GeneratedEndpoints.Common;

internal static partial class Constants
{
    internal const string RequestTimeoutAttributeName = "RequestTimeoutAttribute";
    internal const string RequestTimeoutAttributeHint = $"{RequestTimeoutAttributeFullyQualifiedName}.gs.cs";

    private const string RequestTimeoutAttributeFullyQualifiedName = $"{AttributesNamespace}.{RequestTimeoutAttributeName}";

    internal static readonly SourceText RequestTimeoutAttributeSourceText = CreateRequestTimeoutAttributeSourceText();

    private static SourceText CreateRequestTimeoutAttributeSourceText() => SourceText.From($$"""
                                                                                              {{FileHeader}}

                                                                                              namespace {{AttributesNamespace}};

                                                                                              /// <summary>
                                                                                              /// Applies the request timeout metadata to the annotated endpoint or class.
                                                                                              /// </summary>
                                                                                              [global::System.AttributeUsage(global::System.AttributeTargets.Class | global::System.AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
                                                                                              {{GeneratedTypeAttributes}}
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

                                                                                              """, Encoding.UTF8);
}
