using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ModelConfigurationGenerator;

// todo: merge this with class of the same name in property generator
internal struct GeneratedSource
{
    internal GeneratedSource(string className, string source)
    {
        SourceName = className + ".configurators.g.cs";
        Source = source;
    }

    internal string SourceName { get; private set; }
    internal string Source { get; private set; }
}

[Generator]
public class GenerateCollectedConfig : ISourceGenerator
{
    public void Initialize(GeneratorInitializationContext context)
    {
        context.RegisterForSyntaxNotifications(() => new ConfiguratorFinder());
    }

    public void Execute(GeneratorExecutionContext context)
    {
        if (context.SyntaxReceiver is not ConfiguratorFinder config) return;

        foreach (var klass in config.Configurators.Keys)
        {
            var got = config.Configurators.TryGetValue(klass, out var methods);
            _ = got;
            Debug.Assert(got);
            var result = GenerateForClass(klass.Klass, methods ?? []);
            context.AddSource(result.SourceName, result.Source);
        }
    }

    private GeneratedSource GenerateForClass(ClassDeclarationSyntax klass,
                                             IEnumerable<MethodDeclarationSyntax> configurators)
    {
        const string indentOfCall = "            ";
        var methodCalls =
            configurators.AsParallel()
                         .Select(x =>
                                     $"{indentOfCall}{x.Identifier.ValueText}(modelBuilder);")
                         .Aggregate((acc, call) => acc + "\n" + call);

        var ns = GetNamespace(klass);
        var nsEnding = string.Empty;
        var nsBegin = string.Empty;
        if (!string.IsNullOrEmpty(ns))
        {
            nsBegin = $"namespace {ns} {{";
            nsEnding = "}";
        }

        var keyword = klass.Keyword.ValueText;
        if (klass.IsKind(SyntaxKind.RecordStructDeclaration)) keyword += " struct";

        var src = $$"""
                    // <auto-generated/>
                    using System;
                    using Microsoft.EntityFrameworkCore;

                    #nullable enable

                    {{nsBegin}}
                        {{klass.Modifiers}} {{keyword}} {{klass.Identifier.ValueText}} {
                            private static void ConfigureModels(ModelBuilder modelBuilder) {
                    {{methodCalls}}
                            }
                        }
                    {{nsEnding}}
                    """;
        return new(klass.Identifier.ValueText, src);
    }

    private static string GetNamespace(BaseTypeDeclarationSyntax syntax)
    {
        var nameSpace = string.Empty;
        var potentialNamespaceParent = syntax.Parent;

        while (potentialNamespaceParent != null &&
               potentialNamespaceParent is not NamespaceDeclarationSyntax &&
               potentialNamespaceParent is not FileScopedNamespaceDeclarationSyntax)
        {
            potentialNamespaceParent = potentialNamespaceParent.Parent;
        }

        if (potentialNamespaceParent is not BaseNamespaceDeclarationSyntax namespaceParent)
            return nameSpace;

        nameSpace = namespaceParent.Name.ToString();

        while (true)
        {
            if (namespaceParent.Parent is not NamespaceDeclarationSyntax parent) break;

            nameSpace = $"{namespaceParent.Name}.{nameSpace}";
            namespaceParent = parent;
        }

        return nameSpace;
    }
}
