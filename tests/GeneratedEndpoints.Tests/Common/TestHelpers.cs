using Basic.Reference.Assemblies;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using SourceGeneratorTestHelpers;

namespace GeneratedEndpoints.Tests.Common;

public static class TestHelpers
{
    public static GeneratorDriverRunResult RunGenerator(IEnumerable<string> sources)
    {
        var cSharpParseOptions = new CSharpParseOptions(LanguageVersion.CSharp11).WithPreprocessorSymbols("NET10_0_OR_GREATER");
        var cSharpCompilationOptions = new CSharpCompilationOptions(OutputKind.NetModule).WithNullableContextOptions(NullableContextOptions.Enable);
        var (_, result) =
            IncrementalGenerator.RunWithDiagnostics<MinimalApiGenerator>(sources, cSharpParseOptions, AspNet100.References.All, cSharpCompilationOptions);
        return result;
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
