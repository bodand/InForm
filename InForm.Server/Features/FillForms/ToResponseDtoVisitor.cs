using InForm.Server.Core.Features.Common;
using InForm.Server.Core.Features.Fill;
using InForm.Server.Features.Forms.Db;

namespace InForm.Server.Features.FillForms;

internal class ToResponseDtoVisitor :
    ITypedVisitor<StringFormElement, StringElementResponse>
{
    public StringElementResponse Visit(StringFormElement visited)
    {
        var query =
            from fd in visited.FillData
            group fd.Value by fd.Value into respGroup
            select new { Value = respGroup.Key, Count = respGroup.Count() };
        var dict = query.ToDictionary(x => x.Value, x => x.Count);
        return new StringElementResponse(visited.Id, dict);
    }
}
