using InForm.Server.Core.Features.Common;
using InForm.Server.Core.Features.Fill;
using InForm.Server.Db;
using InForm.Server.Features.FillForms.Db;
using InForm.Server.Features.Forms.Db;

namespace InForm.Server.Features.FillForms;

internal class FillDataDtoInjectorVisitor(Fill fill, FormElementBase formElement) :
    ITypedVisitor<StringFillElement>
{
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
}
