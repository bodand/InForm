using FluentValidation;
using InForm.Server.Core.Features.Common;
using InForm.Server.Core.Features.Forms;

namespace InForm.Client.Features.Forms;

public abstract class ElementModel(FormModel parent) : IVisitable
{
    public long? Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Subtitle { get; set; }
    public bool Required { get; set; }
    public FormModel Parent => parent;

    public void Delete() => Parent.RemoveElement(this);

    public abstract void MakeFillable();

    public abstract void Accept(IVisitor visitor);
    public abstract TResult? Accept<TResult>(IVisitor<TResult> visitor) where TResult : notnull;
}

public class CreateElementValidator : AbstractValidator<ElementModel>
{
    public CreateElementValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("A title is required")
            .MaximumLength(128).WithMessage("A title must be less than {MaxLength} characters long");
        RuleFor(x => x.Subtitle)
            .MaximumLength(256).WithMessage("A subtitle must be less than {MaxLength} characters long");
    }
}