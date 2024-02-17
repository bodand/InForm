using InForm.Server.Features.Forms.Db;

namespace InForm.Server.Features.FillForms.Db;

/// <summary>
///     An entity for storing the fill data for a NumericRange element.
/// </summary>
public class NumericRangeFillData : FillData
{
    public long ParnetElementId { get; set; }
    public long RangeElementQuestionId { get; set; }

    public NumericRangeElement? ParentElement { get; set; }
    public RangeElementQuestion? RangeElementQuestion { get; set; }

    public int Value { get; set; }
}
