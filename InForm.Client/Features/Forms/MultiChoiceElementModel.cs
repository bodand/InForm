using FluentValidation;
using InForm.Server.Core.Features.Common;
using System.Data;

namespace InForm.Client.Features.Forms;

public class MultiChoiceElementModel(FormModel parent) : ElementModel(parent) {
    public List<string> Options { get; set; } = [];
    public int MaxSelected { get; set; } = 1;

    public MultiChoiceElementFillData? FillData { get; set; }

    public override void MakeFillable() => FillData ??= new(this);

    public override void Accept(IVisitor visitor)
    {
        if (visitor is not ITypedVisitor<MultiChoiceElementModel> typed) return;
        typed.Visit(this);
    }

    public override TResult? Accept<TResult>(IVisitor<TResult> visitor)
        where TResult : default
    {
        if (visitor is not ITypedVisitor<MultiChoiceElementModel, TResult> typed) return default;
        return typed.Visit(this);
    }
}

public class MultiChoiceElementFillData(MultiChoiceElementModel model) {
    public List<string> Selected { get; set; } = [];

    public MultiChoiceElementModel Model { get; } = model;
}

public class MultiChoiceElementValidator : AbstractValidator<MultiChoiceElementModel> {
    public MultiChoiceElementValidator()
    {
        RuleFor(x => x.Options)
            .NotEmpty()
            .WithMessage("At least one option must be supplied");
    }
}

public class MultiChoiceValueValidator : AbstractValidator<MultiChoiceElementFillData> {
    public MultiChoiceValueValidator()
    {
        RuleForEach(x => x.Selected)
            .MinimumLength(1)
            .WithMessage("At least one character must be provided")
            .Must((fill, selected) => fill.Model.Options.Contains(selected))
            .WithMessage("Selected value must be in element valid options");
        RuleFor(x => x.Selected)
            .Must((fill, selects) => selects.Count <= fill.Model.MaxSelected)
            .WithMessage(fill => {
                var max = fill.Model.MaxSelected;
                return $"A maximum of {max} can be selected";
            });
    }
}
