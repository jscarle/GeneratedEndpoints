using System.Text;
using Microsoft.CodeAnalysis.Text;

namespace GeneratedEndpoints.Common;

internal static partial class Constants
{
    internal const string MapGroupAttributeName = "MapGroupAttribute";
    internal const string MapGroupAttributeHint = $"{MapGroupAttributeFullyQualifiedName}.gs.cs";

    private const string MapGroupAttributeFullyQualifiedName = $"{AttributesNamespace}.{MapGroupAttributeName}";

    internal static readonly SourceText MapGroupAttributeSourceText;

    private static SourceText CreateMapGroupAttributeSourceText() => SourceText.From($$"""
                                                                                        {{FileHeader}}

                                                                                        namespace {{AttributesNamespace}};

                                                                                        /// <summary>
                                                                                        /// Specifies the route group for the annotated class.
                                                                                        /// </summary>
                                                                                        [global::System.AttributeUsage(global::System.AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
                                                                                        internal sealed class {{MapGroupAttributeName}} : global::System.Attribute
                                                                                        {
                                                                                            /// <summary>
                                                                                            /// Gets the route group pattern.
                                                                                            /// </summary>
                                                                                            public string Pattern { get; }

                                                                                            /// <summary>
                                                                                            /// Gets or sets the endpoint group name.
                                                                                            /// </summary>
                                                                                            public string? Name { get; init; }

                                                                                            /// <summary>
                                                                                            /// Initializes a new instance of the <see cref="{{MapGroupAttributeName}}"/> class.
                                                                                            /// </summary>
                                                                                            /// <param name="pattern">The route group pattern to apply.</param>
                                                                                            public {{MapGroupAttributeName}}(string pattern)
                                                                                            {
                                                                                                Pattern = pattern;
                                                                                            }
                                                                                        }

                                                                                        """, Encoding.UTF8);
}
