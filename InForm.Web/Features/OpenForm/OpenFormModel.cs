using FluentValidation;

namespace InForm.Web.Features.OpenForm;

public class OpenFormModel
{
    public string FormId { get; set; } = string.Empty;
}

public class OpenFormValidator : AbstractValidator<OpenFormModel>
{
    public OpenFormValidator() { 
        RuleFor(x => x.FormId)
            .MinimumLength(4).WithMessage("FormID must be at least 4 characters");
    }
}