using InForm.Server.Core.Features.Common;
using InForm.Server.Core.Features.Forms;
using InForm.Server.Features.Forms.Db;

namespace InForm.Server.Features.Forms;

internal class ToGetDtoVisitor :
    ITypedVisitor<StringFormElement, GetFormElement>
{
    public GetFormElement Visit(StringFormElement visited)
        => new GetStringFormElement(
                visited.Id,
                visited.Title,
                visited.Subtitle,
                visited.MaxLength ?? 0,
                visited.Required,
                visited.RenderAsTextArea
            );
}
