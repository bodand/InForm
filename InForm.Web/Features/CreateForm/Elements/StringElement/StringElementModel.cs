using FluentValidation;

namespace InForm.Web.Features.CreateForm.Elements.StringElement;

public class StringElementModel(CreateFormModel parent) : ElementModel(parent)
{
    public int? MaxAnswerLength { get; set; }
    public bool TextArea { get; set; }
}

public class StringElementValidator : AbstractValidator<StringElementModel>
{
    public StringElementValidator()
    {
        RuleFor(x => x.MaxAnswerLength)
            .GreaterThan(0).WithMessage("Text length requirement must be positive");
    }
}
