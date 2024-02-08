using FluentValidation;

namespace InForm.Web.Features.CreateForm;

public class ElementModel
{
    public string Title { get; set; } = string.Empty;
}

public class ElementValidator : AbstractValidator<ElementModel>
{
    public ElementValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("A title is required")
            .MaximumLength(64).WithMessage("A title must be less than 64 characters long");
    }
}