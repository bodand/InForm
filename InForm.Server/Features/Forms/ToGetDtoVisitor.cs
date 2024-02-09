using InForm.Features.Forms.Db;
using InForm.Server.Core.Features.Common;
using InForm.Server.Core.Features.Forms;

namespace InForm.Server.Features.Forms;

public class ToGetDtoVisitor :
    ITypedVisitor<StringFormElement, GetFormElement>
{
    public GetFormElement Visit(StringFormElement visited)
        => new GetStringFormElement(
                visited.Title,
                visited.Subtitle,
                visited.MaxLength ?? 0,
                visited.Required,
                visited.RenderAsTextArea
            );
}
