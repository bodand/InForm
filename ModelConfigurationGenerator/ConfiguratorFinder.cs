using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ModelConfigurationGenerator;

public class ClassHolder(ClassDeclarationSyntax klass)
{
    public ClassDeclarationSyntax Klass { get; } = klass;

    protected bool Equals(ClassHolder other) =>
        Klass.Identifier.ValueText.Equals(other.Klass.Identifier.ValueText);

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((ClassHolder)obj);
    }

    public override int GetHashCode() => Klass.Identifier.ValueText.GetHashCode();

    public static bool operator ==(ClassHolder? left, ClassHolder? right) => Equals(left, right);

    public static bool operator !=(ClassHolder? left, ClassHolder? right) => !Equals(left, right);
}

public class ConfiguratorFinder : ISyntaxReceiver
{
    public Dictionary<ClassHolder, List<MethodDeclarationSyntax>>
        Configurators { get; } = [];

    public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
    {
        if (syntaxNode is not MethodDeclarationSyntax method) return;
        if (method.Parent is not ClassDeclarationSyntax klass) return;

        var isConfigMethod = (
                                 from attrList in method.AttributeLists.AsEnumerable()
                                 from attr in attrList.Attributes
                                 select attr.Name.ToString()).Contains("ModelConfiguration");
        if (!isConfigMethod) return;

        var holder = new ClassHolder(klass);
        if (!Configurators.TryGetValue(holder, out var methods))
        {
            methods = [];
            Configurators.Add(holder, methods);
        }

        methods.Add(method);
    }
}
