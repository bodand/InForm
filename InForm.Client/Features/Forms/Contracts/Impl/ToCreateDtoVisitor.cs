using InForm.Server.Core.Features.Common;
using InForm.Server.Core.Features.Forms;

namespace InForm.Client.Features.Forms.Contracts.Impl;

internal class ToCreateDtoVisitor :
    ITypedVisitor<StringElementModel, CreateStringElement>
{
    public CreateStringElement Visit(StringElementModel visited)
        => new(visited.Title, 
               visited.Subtitle, 
               visited.MaxAnswerLength ?? 0, 
               visited.Required, 
               visited.TextArea);
}
