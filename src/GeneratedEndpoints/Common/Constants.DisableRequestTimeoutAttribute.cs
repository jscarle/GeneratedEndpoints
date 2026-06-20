using System.Text;
using Microsoft.CodeAnalysis.Text;

namespace GeneratedEndpoints.Common;

internal static partial class Constants
{
    internal const string DisableRequestTimeoutAttributeName = "DisableRequestTimeoutAttribute";
    internal const string DisableRequestTimeoutAttributeHint = $"{DisableRequestTimeoutAttributeFullyQualifiedName}.gs.cs";

    private const string DisableRequestTimeoutAttributeFullyQualifiedName = $"{AttributesNamespace}.{DisableRequestTimeoutAttributeName}";

    internal static readonly SourceText DisableRequestTimeoutAttributeSourceText = CreateDisableRequestTimeoutAttributeSourceText();

    private static SourceText CreateDisableRequestTimeoutAttributeSourceText() => SourceText.From($$"""
                                                                                                     {{FileHeader}}

                                                                                                     namespace {{AttributesNamespace}};

                                                                                                     /// <summary>
                                                                                                     /// Disables the request timeout for the annotated endpoint or class.
                                                                                                     /// </summary>
                                                                                                     [global::System.AttributeUsage(global::System.AttributeTargets.Class | global::System.AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
                                                                                                     {{GeneratedTypeAttributes}}
                                                                                                     internal sealed class {{DisableRequestTimeoutAttributeName}} : global::System.Attribute
                                                                                                     {
                                                                                                     }

                                                                                                     """, Encoding.UTF8);
}
