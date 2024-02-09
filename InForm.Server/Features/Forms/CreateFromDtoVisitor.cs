using InForm.Features.Forms.Db;
using InForm.Server.Core.Features.Common;
using InForm.Server.Core.Features.Forms;

namespace InForm.Server.Features.Forms;

public class CreateFromDtoVisitor(Form parentForm) : 
    ITypedVisitor<CreateStringElement, StringFormElement>
{
    public StringFormElement Visit(CreateStringElement visited) =>
        new()
        {
            ParentForm = parentForm,
            Title = visited.Title,
            Subtitle = visited.Subtitle,
            RenderAsTextArea = visited.Multiline,
            Required = visited.Required,
            MaxLength = visited.MaxLength
        };
}
