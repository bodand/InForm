using InForm.Server.Core.Features.Common;
using InForm.Server.Core.Features.Forms;
using InForm.Server.Features.Forms.Db;

namespace InForm.Server.Features.Forms;

internal class FromCreateDtoVisitor(Form parentForm) :
    ITypedVisitor<CreateStringElement, StringFormElement>,
    ITypedVisitor<CreateMultiChoiceElement, MultiChoiceFormElement> {
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

    public MultiChoiceFormElement Visit(CreateMultiChoiceElement visited) =>
        new()
        {
            ParentForm = parentForm,
            Title = visited.Title,
            Subtitle = visited.Subtitle,
            Required = visited.Required,
            MaxSelected = visited.Selectable,
            Options = [.. visited.Options.Select(x => new MultiChoiceOption() { Value = x })]
        };
}
