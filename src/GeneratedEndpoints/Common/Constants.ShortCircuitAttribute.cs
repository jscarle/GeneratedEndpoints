using System.Text;
using Microsoft.CodeAnalysis.Text;

namespace GeneratedEndpoints.Common;

internal static partial class Constants
{
    internal const string ShortCircuitAttributeName = "ShortCircuitAttribute";
    internal const string ShortCircuitAttributeHint = $"{ShortCircuitAttributeFullyQualifiedName}.gs.cs";

    private const string ShortCircuitAttributeFullyQualifiedName = $"{AttributesNamespace}.{ShortCircuitAttributeName}";

    internal static readonly SourceText ShortCircuitAttributeSourceText = CreateShortCircuitAttributeSourceText();

    private static SourceText CreateShortCircuitAttributeSourceText() => SourceText.From($$"""
                                                                                            {{FileHeader}}

                                                                                            namespace {{AttributesNamespace}};

                                                                                            /// <summary>
                                                                                            /// Marks the annotated endpoint or class to short-circuit the request pipeline.
                                                                                            /// </summary>
                                                                                            [global::System.AttributeUsage(global::System.AttributeTargets.Class | global::System.AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
                                                                                            internal sealed class {{ShortCircuitAttributeName}} : global::System.Attribute
                                                                                            {
                                                                                            }

                                                                                            """, Encoding.UTF8);
}
