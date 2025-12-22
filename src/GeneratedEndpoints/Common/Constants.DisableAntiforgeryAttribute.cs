using System.Text;
using Microsoft.CodeAnalysis.Text;

namespace GeneratedEndpoints.Common;

internal static partial class Constants
{
    internal const string DisableAntiforgeryAttributeName = "DisableAntiforgeryAttribute";
    internal const string DisableAntiforgeryAttributeHint = $"{DisableAntiforgeryAttributeFullyQualifiedName}.gs.cs";

    private const string DisableAntiforgeryAttributeFullyQualifiedName = $"{AttributesNamespace}.{DisableAntiforgeryAttributeName}";

    internal static readonly SourceText DisableAntiforgeryAttributeSourceText = CreateDisableAntiforgeryAttributeSourceText();

    private static SourceText CreateDisableAntiforgeryAttributeSourceText() => SourceText.From($$"""
                                                                                                  {{FileHeader}}

                                                                                                  namespace {{AttributesNamespace}};

                                                                                                  /// <summary>
                                                                                                  /// Disables antiforgery protection for the annotated endpoint or class.
                                                                                                  /// </summary>
                                                                                                  [global::System.AttributeUsage(global::System.AttributeTargets.Class | global::System.AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
                                                                                                  internal sealed class {{DisableAntiforgeryAttributeName}} : global::System.Attribute
                                                                                                  {
                                                                                                  }

                                                                                                  """, Encoding.UTF8);
}
