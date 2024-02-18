using InForm.Server.Core.Features.Common;
using InForm.Server.Core.Features.Fill;
using InForm.Server.Db;
using InForm.Server.Features.FillForms.Db;
using InForm.Server.Features.Forms.Db;

namespace InForm.Server.Features.FillForms;

internal class FillDataDtoInjectorVisitor(Fill fill, FormElementBase formElement) :
    ITypedVisitor<StringFillElement>,
    ITypedVisitor<MultiChoiceFillElement> {
    public void Visit(StringFillElement visited)
    {
        if (formElement is not StringFormElement stringForm)
            throw new InvalidOperationException();

        stringForm.FillData.Add(new StringFillData
        {
            ParentElement = stringForm,
            Value = visited.Value,
            Fill = fill,
        });
    }
    public void Visit(MultiChoiceFillElement visited)
    {
        if (formElement is not MultiChoiceFormElement multiChoiceForm)
            throw new InvalidOperationException();

        multiChoiceForm.FillData.Add(new()
        {
            ParentElement = multiChoiceForm,
            Fill = fill,
            FillId = fill.Id,
            Selected =
            [
                ..visited.Selected.Select(selected => new MultiChoiceFillSelection()
                {
                    OptionId = multiChoiceForm.Options.Single(x => x.Value == selected).Id
                })
            ]
        });
    }
}
