using FluentValidation;

namespace InForm.Web.Features.CreateForm.Elements;

public abstract class ElementModel(CreateFormModel parent) {
    public string Title { get; set; } = string.Empty;
    public string? Subtitle { get; set; }
    public bool Required { get; set; }
    public CreateFormModel Parent { get; } = parent;

	public void Delete() => Parent.RemoveElement(this);

}

public class ElementValidator : AbstractValidator<ElementModel>
{
    public ElementValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("A title is required")
            .MaximumLength(128).WithMessage("A title must be less than {MaxLength} characters long");
        RuleFor(x => x.Subtitle)
            .MaximumLength(256).WithMessage("A subtitle must be less than {MaxLength} characters long");
    }
}