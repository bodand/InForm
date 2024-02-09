using InForm.Server.Core.Features.Common;
using InForm.Server.Core.Features.Forms;

namespace InForm.Client.Features.Forms;

internal class FromDtoVisitor(FormModel model)
    : ITypedVisitor<GetFormElement, ElementModel>
{
    public ElementModel Visit(GetFormElement visited)
        => new StringElementModel(model)
        {
            MaxAnswerLength = visited.MaxLength,
            Required = visited.Required,
            Subtitle = visited.Subtitle,
            TextArea = visited.Multiline,
            Title = visited.Title,
        };
}
