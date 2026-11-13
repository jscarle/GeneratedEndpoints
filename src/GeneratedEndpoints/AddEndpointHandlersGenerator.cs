using System.Collections.Immutable;
using System.Text;
using GeneratedEndpoints.Common;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using static GeneratedEndpoints.Common.Constants;

namespace GeneratedEndpoints;

// ReSharper disable ForCanBeConvertedToForeach
// ReSharper disable LoopCanBeConvertedToQuery
// Do not refactor, use for loop to avoid allocations.

internal static class AddEndpointHandlersGenerator
{
    public static void GenerateSource(SourceProductionContext context, ImmutableSortedDictionary<RequestHandlerClass, ImmutableArray<RequestHandler>> grouped)
    {
        context.CancellationToken.ThrowIfCancellationRequested();

        var nonStaticClasses = grouped.Keys
            .Where(x => !x.IsStatic && !x.IsAbstract)
            .ToList();

        var source = new StringBuilder();
        source.AppendLine(FileHeader);

        source.AppendLine();

        source.AppendLine("using Microsoft.Extensions.DependencyInjection;");
        source.AppendLine("using Microsoft.Extensions.DependencyInjection.Extensions;");
        source.AppendLine();

        source.Append("namespace ");
        source.Append(RoutingNamespace);
        source.AppendLine(";");

        source.AppendLine();

        source.AppendLine(GeneratedTypeAttributes);
        source.Append("internal static class ");
        source.Append(AddEndpointHandlersClassName);
        source.AppendLine();

        source.AppendLine("{");

        source.Append("    internal static void ");
        source.Append(AddEndpointHandlersMethodName);
        source.AppendLine("(this IServiceCollection services)");

        source.AppendLine("    {");

        foreach (var handlerClass in nonStaticClasses)
        {
            source.Append("        services.TryAddScoped<");

            if (!string.IsNullOrEmpty(handlerClass.InterfaceName))
            {
                source.Append(handlerClass.InterfaceName);
                source.Append(", ");
                source.Append(handlerClass.Name);
                source.Append(">();");
            }
            else
            {
                source.Append(handlerClass.Name);
                source.Append(">();");
            }

            source.AppendLine();
        }

        source.AppendLine("""
                              }
                          }
                          """
        );

        var sourceText = StringBuilderPool.ToStringAndReturn(source);
        context.AddSource(AddEndpointHandlersMethodHint, SourceText.From(sourceText, Encoding.UTF8));
    }
}
