using InForm.Server.Core.Features.Common;
using InForm.Server.Core.Features.Forms;

namespace InForm.Client.Features.Forms;

internal class FromDtoVisitor(FormModel model)
    : ITypedVisitor<GetStringFormElement, ElementModel>
{
    public ElementModel Visit(GetStringFormElement visited)
        => new StringElementModel(model)
        {
            Id = visited.Id,
            MaxAnswerLength = visited.MaxLength == 0 ? null : visited.MaxLength,
            Required = visited.Required,
            Subtitle = visited.Subtitle,
            TextArea = visited.Multiline,
            Title = visited.Title,
        };
}
