using Microsoft.CodeAnalysis;

namespace VisitationGenerator;

[Generator]
public class VisitationGenerator : ISourceGenerator {

    public void Initialize(GeneratorInitializationContext context)
    {
        context.RegisterForSyntaxNotifications(() => new VisitationFinder());
    }

    public void Execute(GeneratorExecutionContext context)
    {
        // todo
    }
}
