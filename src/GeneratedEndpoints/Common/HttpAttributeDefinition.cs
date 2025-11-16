using Microsoft.CodeAnalysis.Text;

namespace GeneratedEndpoints.Common;

internal readonly record struct HttpAttributeDefinition(
    string Name,
    string FullyQualifiedName,
    string Hint,
    string Verb,
    SourceText SourceText
);
