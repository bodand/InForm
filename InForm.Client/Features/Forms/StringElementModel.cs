using FluentValidation;
using InForm.Server.Core.Features.Forms;

namespace InForm.Client.Features.Forms;

public class StringElementModel(FormModel parent) : ElementModel(parent)
{
    public int? MaxAnswerLength { get; set; }
    public bool TextArea { get; set; }

    public StringElementFillData? FillData { get; set; }

    public override void MakeFillable()
        => FillData ??= new(this);

    public override CreateStringElement ToDto()
        => new(Title, Subtitle, MaxAnswerLength ?? 0, Required, TextArea);
}

public class StringElementFillData(StringElementModel model)
{
    public string? Value { get; set; }
}

public class StringElementValidator : AbstractValidator<StringElementModel>
{
    public StringElementValidator()
    {
        RuleFor(x => x.MaxAnswerLength)
            .GreaterThan(0).WithMessage("Text length requirement must be positive");
        RuleFor(x => x.FillData)
            .SetValidator((x, data) => new StringValueValidator(x));
    }
}

public class StringValueValidator : AbstractValidator<StringElementFillData?>
{
    public StringValueValidator(StringElementModel elementModel)
    {
        var rules = RuleFor(x => x!.Value);
        if (elementModel.Required)
            rules.NotEmpty()
                 .WithMessage("This field is required");
        if (elementModel.MaxAnswerLength.HasValue)
            rules.MaximumLength(elementModel.MaxAnswerLength.Value)
                 .WithMessage("The response must be less than {MaxLength} characters");
    }
}
