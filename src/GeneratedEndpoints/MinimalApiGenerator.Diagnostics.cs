using Microsoft.CodeAnalysis;

namespace GeneratedEndpoints;

public sealed partial class MinimalApiGenerator
{
    private const string DiagnosticCategory = "GeneratedEndpoints";

    private static readonly DiagnosticDescriptor InvalidEndpointTargetDiagnostic = new(
        "GE0001",
        title: "Map attribute must target a method",
        messageFormat: "The '{0}' attribute can only be applied to methods.",
        category: DiagnosticCategory,
        DiagnosticSeverity.Error,
        isEnabledByDefault: true);

    private static readonly DiagnosticDescriptor InvalidEndpointContainerDiagnostic = new(
        "GE0002",
        title: "Endpoint methods must be declared inside a class",
        messageFormat: "The endpoint method '{0}' must be declared inside a class.",
        category: DiagnosticCategory,
        DiagnosticSeverity.Error,
        isEnabledByDefault: true);

    private static readonly DiagnosticDescriptor ConflictingAuthorizationAttributesDiagnostic = new(
        "GE0003",
        title: "Conflicting authorization attributes",
        messageFormat: "The endpoint '{0}' cannot combine [RequireAuthorization] and [AllowAnonymous].",
        category: DiagnosticCategory,
        DiagnosticSeverity.Error,
        isEnabledByDefault: true);

    private static readonly DiagnosticDescriptor DuplicateEndpointNameDiagnostic = new(
        "GE0004",
        title: "Duplicate endpoint name",
        messageFormat: "The endpoint name '{0}' is used by multiple handlers for method '{1}'. Endpoint names must be unique.",
        category: DiagnosticCategory,
        DiagnosticSeverity.Warning,
        isEnabledByDefault: true);

    private static readonly DiagnosticDescriptor InvalidBindingAnnotationDiagnostic = new(
        "GE0005",
        title: "Invalid binding annotation",
        messageFormat: "The binding name specified for parameter '{0}' must be a non-empty string.",
        category: DiagnosticCategory,
        DiagnosticSeverity.Error,
        isEnabledByDefault: true);
}
