using FluentValidation;
using InForm.Server.Core.Features.Common;
using InForm.Server.Core.Features.Forms;

namespace InForm.Client.Features.Forms;

public class StringElementModel(FormModel parent) : ElementModel(parent)
{
    public int? MaxAnswerLength { get; set; }
    public bool TextArea { get; set; }

    public StringElementFillData? FillData { get; set; }

    public override void Accept(IVisitor visitor)
    {
        if (visitor is not ITypedVisitor<StringElementModel> typed) return;
        typed.Visit(this);
    }

    public override TResult? Accept<TResult>(IVisitor<TResult> visitor) where TResult : default
    {
        if (visitor is not ITypedVisitor<StringElementModel, TResult> typed) return default;
        return typed.Visit(this);
    }

    public override void MakeFillable()
        => FillData ??= new(this);
}

public class StringElementFillData(StringElementModel model)
{
    public string? Value { get; set; }
    public StringElementModel Model { get; } = model;
}

public class StringElementValidator : AbstractValidator<StringElementModel>
{
    public StringElementValidator()
    {
        RuleFor(x => x.MaxAnswerLength)
            .GreaterThan(0).WithMessage("Text length requirement must be positive");
        RuleFor(x => x.FillData)
            .SetValidator(new StringValueValidator());
    }
}

public class StringValueValidator : AbstractValidator<StringElementFillData?>
{
    public StringValueValidator()
    {
        When(x => x?.Model.Required ?? false, () =>
        {
            RuleFor(x => x.Value).NotEmpty().WithMessage("This field is required");
        });

        When(x => x?.Model.MaxAnswerLength.HasValue ?? false, () =>
        {
            RuleFor(x => x!.Value)
                .Must((data, value) =>
                {
                    if (data is null || value is null) return true;
                    var max = data.Model.MaxAnswerLength ?? 0;
                    return value.Length <= max;
                })
                .WithMessage(x =>
                {
                    var max = x?.Model.MaxAnswerLength ?? 0;
                    return $"The response must be less than {max} characters";
                });
        });
    }
}
