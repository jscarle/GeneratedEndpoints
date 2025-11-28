using System.Text;
using Microsoft.CodeAnalysis.Text;

namespace GeneratedEndpoints.Common;

internal static partial class Constants
{
    internal const string RequireAuthorizationAttributeName = "RequireAuthorizationAttribute";
    internal const string RequireAuthorizationAttributeHint = $"{RequireAuthorizationAttributeFullyQualifiedName}.gs.cs";

    private const string RequireAuthorizationAttributeFullyQualifiedName = $"{AttributesNamespace}.{RequireAuthorizationAttributeName}";

    internal static readonly SourceText RequireAuthorizationAttributeSourceText = CreateRequireAuthorizationAttributeSourceText();

    private static SourceText CreateRequireAuthorizationAttributeSourceText() => SourceText.From($$"""
                                                                                                    {{FileHeader}}

                                                                                                    namespace {{AttributesNamespace}};

                                                                                                    /// <summary>
                                                                                                    /// Specifies that authorization is required for the annotated endpoint or class.
                                                                                                    /// Optionally restricts access to the specified authorization policies.
                                                                                                    /// </summary>
                                                                                                    [global::System.AttributeUsage(global::System.AttributeTargets.Class | global::System.AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
                                                                                                    internal sealed class {{RequireAuthorizationAttributeName}} : global::System.Attribute
                                                                                                    {
                                                                                                        /// <summary>
                                                                                                        /// Gets the policy names that the endpoint or class requires.
                                                                                                        /// </summary>
                                                                                                        public string[] PolicyNames { get; }

                                                                                                        /// <summary>
                                                                                                        /// Marks the endpoint or class as requiring authorization.
                                                                                                        /// </summary>
                                                                                                        public {{RequireAuthorizationAttributeName}}()
                                                                                                        {
                                                                                                            PolicyNames = [];
                                                                                                        }

                                                                                                        /// <summary>
                                                                                                        /// Marks the endpoint or class as requiring authorization with one or more policies.
                                                                                                        /// </summary>
                                                                                                        public {{RequireAuthorizationAttributeName}}(params string[] policyNames)
                                                                                                        {
                                                                                                            PolicyNames = policyNames ?? [];
                                                                                                        }
                                                                                                    }
                                                                                                    """, Encoding.UTF8);
}
