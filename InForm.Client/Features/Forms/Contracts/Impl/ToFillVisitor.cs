using InForm.Server.Core.Features.Common;
using InForm.Server.Core.Features.Fill;

namespace InForm.Client.Features.Forms.Contracts.Impl;

internal class ToFillVisitor
    : ITypedVisitor<StringElementModel, StringFillElement>,
            ITypedVisitor<MultiChoiceElementModel, MultiChoiceFillElement> {
    public StringFillElement Visit(StringElementModel visited) => visited switch
    {
        { FillData: null } =>
            throw new InvalidElementException(visited, "Element is missing its fill data: was element this shown to the user?"),
        { Id: null } =>
            throw new InvalidElementException(visited, "Element is missing its id: was element saved?"),
        _ =>
            new(visited.Id.Value, visited.FillData.Value)
    };
    
    public MultiChoiceFillElement Visit(MultiChoiceElementModel visited) => visited switch
    {
        { FillData: null } =>
            throw new InvalidElementException(visited, "Element is missing its fill data: was element this shown to the user?"),
        { Id: null } =>
            throw new InvalidElementException(visited, "Element is missing its id: was element saved?"),
        _ =>
            new(visited.Id.Value, visited.FillData.Selected)
    };
}
