using System.Text;
using Microsoft.CodeAnalysis.Text;

namespace GeneratedEndpoints.Common;

internal static partial class Constants
{
    internal const string SummaryAttributeName = "SummaryAttribute";
    internal const string SummaryAttributeHint = $"{SummaryAttributeFullyQualifiedName}.gs.cs";

    private const string SummaryAttributeFullyQualifiedName = $"{AttributesNamespace}.{SummaryAttributeName}";

    internal static readonly SourceText SummaryAttributeSourceText = CreateSummaryAttributeSourceText();

    private static SourceText CreateSummaryAttributeSourceText() => SourceText.From($$"""
                                                                                       {{FileHeader}}

                                                                                       namespace {{AttributesNamespace}};

                                                                                       /// <summary>
                                                                                       /// Specifies the summary metadata for the annotated endpoint.
                                                                                       /// </summary>
                                                                                       [global::System.AttributeUsage(global::System.AttributeTargets.Class | global::System.AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
                                                                                       {{GeneratedTypeAttributes}}
                                                                                       internal sealed class {{SummaryAttributeName}} : global::System.Attribute
                                                                                       {
                                                                                           /// <summary>
                                                                                           /// Gets the summary value for the endpoint.
                                                                                           /// </summary>
                                                                                           public string Summary { get; }

                                                                                           /// <summary>
                                                                                           /// Initializes a new instance of the <see cref="{{SummaryAttributeName}}"/> class.
                                                                                           /// </summary>
                                                                                           /// <param name="summary">The summary to apply to the endpoint.</param>
                                                                                           public {{SummaryAttributeName}}(string summary)
                                                                                           {
                                                                                               Summary = summary;
                                                                                           }
                                                                                       }

                                                                                       """, Encoding.UTF8);
}
