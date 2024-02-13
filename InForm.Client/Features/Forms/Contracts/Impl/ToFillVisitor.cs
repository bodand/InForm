using InForm.Server.Core.Features.Common;
using InForm.Server.Core.Features.Fill;

namespace InForm.Client.Features.Forms.Contracts.Impl;

internal class ToFillVisitor
    : ITypedVisitor<StringElementModel, StringFillElement>
{
    public StringFillElement Visit(StringElementModel visited)
    {
        if (visited is { FillData: null }) 
            throw new InvalidElementException(visited, 
                                              "Element is missing its fill data: was element this shown to the user?");
        if (visited is { Id: null }) 
            throw new InvalidElementException(visited, 
                                              "Element is missing its id: was element saved?");

        return new StringFillElement(visited.Id.Value, visited.FillData.Value);
    }
}
