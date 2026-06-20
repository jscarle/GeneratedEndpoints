using System.Text;
using Microsoft.CodeAnalysis.Text;

namespace GeneratedEndpoints.Common;

internal static partial class Constants
{
    internal const string DisableValidationAttributeName = "DisableValidationAttribute";
    internal const string DisableValidationAttributeHint = $"{DisableValidationAttributeFullyQualifiedName}.gs.cs";

    private const string DisableValidationAttributeFullyQualifiedName = $"{AttributesNamespace}.{DisableValidationAttributeName}";

    internal static readonly SourceText DisableValidationAttributeSourceText = CreateDisableValidationAttributeSourceText();

    private static SourceText CreateDisableValidationAttributeSourceText() => SourceText.From($$"""
                                                                                                 #if NET10_0_OR_GREATER
                                                                                                 {{FileHeader}}

                                                                                                 namespace {{AttributesNamespace}};

                                                                                                 /// <summary>
                                                                                                 /// Disables request validation for the annotated endpoint or class when targeting .NET 10 or later.
                                                                                                 /// </summary>
                                                                                                 [global::System.AttributeUsage(global::System.AttributeTargets.Class | global::System.AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
                                                                                                 {{GeneratedTypeAttributes}}
                                                                                                 internal sealed class {{DisableValidationAttributeName}} : global::System.Attribute
                                                                                                 {
                                                                                                 }
                                                                                                 #endif

                                                                                                 """, Encoding.UTF8);
}
