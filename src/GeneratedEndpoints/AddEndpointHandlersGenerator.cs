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
    public static void GenerateSource(SourceProductionContext context, EquatableImmutableArray<RequestHandler> requestHandlers)
    {
        context.CancellationToken.ThrowIfCancellationRequested();

        var nonStaticClassNames = GetDistinctNonStaticClassNames(requestHandlers);
        var source = GetAddEndpointHandlersStringBuilder(nonStaticClassNames);
        source.AppendLine(FileHeader);

        source.AppendLine();

        source.AppendLine("using Microsoft.Extensions.DependencyInjection;");
        source.AppendLine("using Microsoft.Extensions.DependencyInjection.Extensions;");
        source.AppendLine();

        source.Append("namespace ");
        source.Append(RoutingNamespace);
        source.AppendLine(";");

        source.AppendLine();

        source.Append("internal static class ");
        source.Append(AddEndpointHandlersClassName);
        source.AppendLine();

        source.AppendLine("{");

        source.Append("    internal static void ");
        source.Append(AddEndpointHandlersMethodName);
        source.AppendLine("(this IServiceCollection services)");

        source.AppendLine("    {");

        foreach (var className in nonStaticClassNames)
        {
            source.Append("        services.TryAddScoped<");
            source.Append(className);
            source.Append(">();");
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

    private static List<string> GetDistinctNonStaticClassNames(EquatableImmutableArray<RequestHandler> requestHandlers)
    {
        var classNames = new List<string>();
        if (requestHandlers.Count == 0)
            return classNames;

        var seen = new HashSet<string>(StringComparer.Ordinal);
        for (var index = 0; index < requestHandlers.Count; index++)
        {
            var requestHandler = requestHandlers[index];
            if (requestHandler.Class.IsStatic)
                continue;

            var className = requestHandler.Class.Name;
            if (seen.Add(className))
                classNames.Add(className);
        }

        return classNames;
    }

    private static StringBuilder GetAddEndpointHandlersStringBuilder(List<string> nonStaticClassNames)
    {
        var estimate = 512L;
        for (var index = 0; index < nonStaticClassNames.Count; index++)
        {
            var className = nonStaticClassNames[index];
            estimate += 36 + className.Length;
        }

        estimate += Math.Max(256, nonStaticClassNames.Count * 12);
        estimate = (long)(estimate * 1.10);

        if (estimate < 512)
            estimate = 512;
        else if (estimate > int.MaxValue)
            estimate = int.MaxValue;

        return StringBuilderPool.Get((int)estimate);
    }
}
