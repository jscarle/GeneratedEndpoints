using System.Text;
using Microsoft.CodeAnalysis.Text;

namespace GeneratedEndpoints.Common;

internal static partial class Constants
{
    internal const string EndpointFilterAttributeName = "EndpointFilterAttribute";
    internal const string EndpointFilterAttributeHint = $"{EndpointFilterAttributeFullyQualifiedName}.gs.cs";

    private const string EndpointFilterAttributeFullyQualifiedName = $"{AttributesNamespace}.{EndpointFilterAttributeName}";

    internal static readonly SourceText EndpointFilterAttributeSourceText = CreateEndpointFilterAttributeSourceText();

    private static SourceText CreateEndpointFilterAttributeSourceText() => SourceText.From($$"""
                                                                                              {{FileHeader}}

                                                                                              namespace {{AttributesNamespace}};

                                                                                              /// <summary>
                                                                                              /// Specifies an endpoint filter type to apply to the annotated endpoint or class.
                                                                                              /// </summary>
                                                                                              [global::System.AttributeUsage(global::System.AttributeTargets.Class | global::System.AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
                                                                                              internal sealed class {{EndpointFilterAttributeName}} : global::System.Attribute
                                                                                              {
                                                                                                  /// <summary>
                                                                                                  /// Gets the CLR type of the endpoint filter.
                                                                                                  /// </summary>
                                                                                                  public global::System.Type Type { get; }

                                                                                                  /// <summary>
                                                                                                  /// Initializes a new instance of the <see cref="{{EndpointFilterAttributeName}}"/> class.
                                                                                                  /// </summary>
                                                                                                  /// <param name="type">The CLR type of the endpoint filter.</param>
                                                                                                  public {{EndpointFilterAttributeName}}(global::System.Type type)
                                                                                                  {
                                                                                                      Type = type;
                                                                                                  }
                                                                                              }

                                                                                              /// <summary>
                                                                                              /// Specifies an endpoint filter type using a generic argument.
                                                                                              /// </summary>
                                                                                              /// <typeparam name="TFilter">The CLR type of the endpoint filter.</typeparam>
                                                                                              [global::System.AttributeUsage(global::System.AttributeTargets.Class | global::System.AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
                                                                                              internal sealed class {{EndpointFilterAttributeName}}<TFilter> : global::System.Attribute
                                                                                              {
                                                                                                  /// <summary>
                                                                                                  /// Gets the CLR type of the endpoint filter.
                                                                                                  /// </summary>
                                                                                                  public global::System.Type Type => typeof(TFilter);
                                                                                              }

                                                                                              """, Encoding.UTF8);
}
