using FluentValidation;
using InForm.Server.Core.Features.Forms;

namespace InForm.Client.Features.Forms;

public class StringElementModel(FormModel parent) : ElementModel(parent)
{
    public int? MaxAnswerLength { get; set; }
    public bool TextArea { get; set; }

    public override CreateStringElement ToDto()
        => new(Title, Subtitle, MaxAnswerLength ?? 0, Required, TextArea);
}

public class StringElementValidator : AbstractValidator<StringElementModel>
{
    public StringElementValidator()
    {
        RuleFor(x => x.MaxAnswerLength)
            .GreaterThan(0).WithMessage("Text length requirement must be positive");
    }
}
