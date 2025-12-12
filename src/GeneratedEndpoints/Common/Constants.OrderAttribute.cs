using System.Text;
using Microsoft.CodeAnalysis.Text;

namespace GeneratedEndpoints.Common;

internal static partial class Constants
{
    internal const string OrderAttributeName = "OrderAttribute";
    internal const string OrderAttributeHint = $"{OrderAttributeFullyQualifiedName}.gs.cs";

    private const string OrderAttributeFullyQualifiedName = $"{AttributesNamespace}.{OrderAttributeName}";

    internal static readonly SourceText OrderAttributeSourceText = CreateOrderAttributeSourceText();

    private static SourceText CreateOrderAttributeSourceText() => SourceText.From($$"""
                                                                                     {{FileHeader}}

                                                                                     namespace {{AttributesNamespace}};

                                                                                     /// <summary>
                                                                                     /// Specifies the order for the annotated endpoint when building conventions.
                                                                                     /// </summary>
                                                                                     [global::System.AttributeUsage(global::System.AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
                                                                                     internal sealed class {{OrderAttributeName}} : global::System.Attribute
                                                                                     {
                                                                                         /// <summary>
                                                                                         /// Gets the order that will be applied to the endpoint.
                                                                                         /// </summary>
                                                                                         public int Order { get; }

                                                                                         /// <summary>
                                                                                         /// Initializes a new instance of the <see cref="{{OrderAttributeName}}"/> class.
                                                                                         /// </summary>
                                                                                         /// <param name="order">The order value to apply to the endpoint.</param>
                                                                                         public {{OrderAttributeName}}(int order)
                                                                                         {
                                                                                             Order = order;
                                                                                         }
                                                                                     }

                                                                                     """, Encoding.UTF8);
}
