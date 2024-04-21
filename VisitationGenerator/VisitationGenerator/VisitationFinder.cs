using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace VisitationGenerator;

public class VisitationFinder : ISyntaxReceiver {

    internal IList<ClassDeclarationSyntax> Classes { get; } = [];

    public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
    {
        if (syntaxNode is not MethodDeclarationSyntax method) return;
        if (method.Parent is not ClassDeclarationSyntax klass) return;

        var isConfigMethod = (
                                 from attrList in method.AttributeLists.AsEnumerable()
                                 from attr in attrList.Attributes
                                 select attr.Name.ToString()).Contains("ModelConfiguration");
        if (!isConfigMethod) return;

        Classes.Add(klass);
    }
}
