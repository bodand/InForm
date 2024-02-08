using FluentValidation;

namespace InForm.Web.Features.CreateForm;

public class CreateFormModel
{
    public string Title { get; set; } = string.Empty;

    public string Subtitle { get; set; } = string.Empty;

    public List<ElementModel> ElementModels { get; set; } = [];
}

public class CreateFormValidator : AbstractValidator<CreateFormModel>
{
    public CreateFormValidator()
    {
        RuleFor(x => x.Title)
            .Length(4, 64)
            .WithMessage("A title must be between {MinLength} and {MaxLength} characters");
        RuleFor(x => x.Subtitle)
            .MaximumLength(64)
            .WithMessage("The subtitle must be less than {MaxLength} characters");
        RuleFor(x => x.ElementModels)
            .NotEmpty().WithMessage("At least one form element is required");
        RuleForEach(x => x.ElementModels)
            .SetValidator(new ElementValidator()); // todo polymorphic validator
    }
}