using InForm.Server.Core.Features.Common;
using InForm.Server.Core.Features.Forms;
using InForm.Server.Features.Forms.Db;

namespace InForm.Server.Features.Forms;

internal class ToGetDtoVisitor :
    ITypedVisitor<StringFormElement, GetStringFormElement>,
    ITypedVisitor<MultiChoiceFormElement, GetMultiChoiceElement> {
    public GetStringFormElement Visit(StringFormElement visited)
        => new(visited.Id,
               visited.Title,
               visited.Subtitle,
               visited.MaxLength ?? 0,
               visited.Required,
               visited.RenderAsTextArea);

    public GetMultiChoiceElement Visit(MultiChoiceFormElement visited)
        => new(visited.Id,
               visited.Title,
               visited.Subtitle,
               visited.Required,
               [.. visited.StringOptions],
               visited.MaxSelected);
}
