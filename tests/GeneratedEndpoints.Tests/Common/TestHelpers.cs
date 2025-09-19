using Basic.Reference.Assemblies;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using SourceGeneratorTestHelpers;

namespace GeneratedEndpoints.Tests.Common;

public static class TestHelpers
{
    public static GeneratorDriverRunResult RunGenerator(IEnumerable<string> sources)
    {
        var cSharpParseOptions = new CSharpParseOptions(LanguageVersion.CSharp11).WithPreprocessorSymbols("NET7_0_OR_GREATER");
        var cSharpCompilationOptions = new CSharpCompilationOptions(OutputKind.NetModule).WithNullableContextOptions(NullableContextOptions.Enable);
        return IncrementalGenerator.Run<MinimalApiGenerator>(sources, cSharpParseOptions, ReferenceAssemblies.Net80, cSharpCompilationOptions);
    }

    public static IEnumerable<string> GetSources(string source, bool withNamespace)
    {
        const string usingStatements = """

                                       """;

        if (withNamespace)
            yield return $"""
                          {usingStatements}

                          namespace EntityFrameworkGeneratorTests;

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
