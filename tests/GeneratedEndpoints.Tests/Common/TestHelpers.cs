using System.Collections.Immutable;
using Basic.Reference.Assemblies;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using SourceGeneratorTestHelpers;

namespace GeneratedEndpoints.Tests.Common;

public static class TestHelpers
{
    private static readonly CSharpCompilationOptions GeneratorCompilationOptions =
        new CSharpCompilationOptions(OutputKind.NetModule).WithNullableContextOptions(NullableContextOptions.Enable);

    private static readonly CSharpCompilationOptions TestCompilationOptions =
        new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary).WithNullableContextOptions(NullableContextOptions.Enable);

    public static GeneratorDriverRunResult RunGenerator(IEnumerable<string> sources)
    {
        var cSharpParseOptions = new CSharpParseOptions(LanguageVersion.CSharp11).WithPreprocessorSymbols("NET10_0_OR_GREATER");
        var (_, result) =
            IncrementalGenerator.RunWithDiagnostics<MinimalApiGenerator>(sources, cSharpParseOptions, AspNet100.References.All, GeneratorCompilationOptions);
        return result;
    }

    public static ImmutableArray<Diagnostic> GetCompilationErrors(IEnumerable<string> sources, LanguageVersion languageVersion = LanguageVersion.Latest)
    {
        var cSharpParseOptions = new CSharpParseOptions(languageVersion).WithPreprocessorSymbols("NET10_0_OR_GREATER");
        var syntaxTrees = sources.Select(source => CSharpSyntaxTree.ParseText(source, cSharpParseOptions));
        var compilation = CSharpCompilation.Create("GeneratedEndpoints.Tests.GeneratedCode", syntaxTrees, AspNet100.References.All, TestCompilationOptions);

        var driver = CSharpGeneratorDriver.Create([new MinimalApiGenerator().AsSourceGenerator()], parseOptions: cSharpParseOptions);
        driver.RunGeneratorsAndUpdateCompilation(compilation, out var outputCompilation, out var generatorDiagnostics);

        return generatorDiagnostics.Concat(outputCompilation.GetDiagnostics())
            .Where(diagnostic => diagnostic.Severity == DiagnosticSeverity.Error)
            .ToImmutableArray();
    }

    public static string GetGeneratedSource(GeneratorDriverRunResult result, string hintName)
    {
        foreach (var generatorResult in result.Results)
        foreach (var generatedSource in generatorResult.GeneratedSources)
            if (generatedSource.HintName == hintName)
                return generatedSource.SourceText.ToString();

        throw new InvalidOperationException($"Generated source '{hintName}' was not found.");
    }

    public static IEnumerable<string> GetSources(string source, bool withNamespace)
    {
        const string usingStatements = """
                                       using Microsoft.AspNetCore.Authorization;
                                       using Microsoft.AspNetCore.Generated.Attributes;
                                       using Microsoft.AspNetCore.Http;
                                       using Microsoft.AspNetCore.Http.HttpResults;
                                       using Microsoft.AspNetCore.Routing;
                                       using System.ComponentModel;
                                       """;

        if (withNamespace)
            yield return $"""
                          {usingStatements}

                          namespace GeneratedEndpointsTests;

                          {source}
                          """;
        else
            yield return $"""
                          {usingStatements}

                          {source}
                          """;

        yield return """

                     """;
    }
}
