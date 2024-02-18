using InForm.Server.Core.Features.Common;
using InForm.Server.Core.Features.Fill;
using InForm.Server.Features.Forms.Db;
using InForm.Server.Migrations;

namespace InForm.Server.Features.FillForms;

internal class ToResponseDtoVisitor :
    ITypedVisitor<StringFormElement, StringElementResponse>,
    ITypedVisitor<MultiChoiceFormElement, MultiChoiceElementResponse> {
    public StringElementResponse Visit(StringFormElement visited)
    {
        var query =
            from fd in visited.FillData
            group fd.Value by fd.Value into respGroup
            select new { Value = respGroup.Key, Count = respGroup.Count() };
        var dict = query.ToDictionary(x => x.Value, x => x.Count);
        return new(visited.Id,
                   visited.Title,
                   visited.Subtitle,
                   dict);
    }

    public MultiChoiceElementResponse Visit(MultiChoiceFormElement visited)
    {
        var query =
            from fd in visited.FillData
            from selected in fd.Selected
            let selStr = selected.Option.Value
            group selStr by selStr into respGroup
            select new { Value = respGroup.Key, Count = respGroup.Count() };
        var selects = query.ToDictionary(x => x.Value, x => x.Count);

        var opts = visited.StringOptions.ToDictionary(x => x,
                                                      x => selects.GetValueOrDefault(x, 0));
        return new(visited.Id,
                   visited.Title,
                   visited.Subtitle,
                   opts);
    }
}
