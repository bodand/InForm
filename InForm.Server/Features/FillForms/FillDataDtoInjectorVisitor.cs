using InForm.Features.Forms.Db;
using InForm.Server.Core.Features.Common;
using InForm.Server.Core.Features.Forms;
using InForm.Server.Db;
using InForm.Server.Features.FillForms.Db;

namespace InForm.Server.Features.FillForms;

public class FillDataDtoInjectorVisitor(InFormDbContext dbContext, FormElementBase formElement) :
    ITypedVisitor<StringFillElement>
{
    public void Visit(StringFillElement visited)
    {
        if (formElement is not StringFormElement stringForm)
            throw new InvalidOperationException();

        stringForm.FillData.Add(new()
        {
            ParentElement = stringForm,
            Value = visited.Value,
        });
    }
}
