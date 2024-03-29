using FluentValidation;

namespace InForm.Client.Features.Forms;

public class FormModel
{
    public Guid? Id { get; internal set; }
    public string Title { get; set; } = string.Empty;

    public string Subtitle { get; set; } = string.Empty;
    
    public string? Password { get; set; }

    public event Action? ElementDeleted;
    public List<ElementModel> ElementModels { get; set; } = [];

    public void RemoveElement(ElementModel child)
    {
        ElementModels.Remove(child);
        ElementDeleted?.Invoke();
    }
}

public class CreateFormValidator : AbstractValidator<FormModel>
{
    public CreateFormValidator()
    {
        RuleFor(x => x.Title)
            .Length(4, 64)
            .WithMessage("The form title must be between {MinLength} and {MaxLength} characters");
        RuleFor(x => x.Subtitle)
            .MaximumLength(64)
            .WithMessage("The form subtitle must be less than {MaxLength} characters");
        RuleFor(x => x.ElementModels)
            .NotEmpty().WithMessage("At least one form element is required");
        RuleForEach(x => x.ElementModels)
            .SetInheritanceValidator(poly =>
            {
                poly.Add(new StringElementValidator());
                poly.Add(new MultiChoiceElementValidator());
            })
            .SetValidator(new CreateElementValidator());
    }
}