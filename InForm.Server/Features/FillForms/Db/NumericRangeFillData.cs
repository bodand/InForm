using InForm.Server.Features.Forms.Db;

namespace InForm.Server.Features.FillForms.Db;

/// <summary>
///     An entity for storing the fill data for a NumericRange element.
/// </summary>
public class NumericRangeFillData : FillData
{
    /// <summary>
    ///     The database id of the form element this fill data is for.
    /// </summary>
    public long ParnetElementId { get; set; }

    /// <summary>
    ///     The form element this fill data is for.
    /// </summary>
    public NumericRangeElement? ParentElement { get; set; }

    /// <summary>
    ///     The database id of the question entity this fill data holds the
    ///     data for.
    /// </summary>
    public long RangeElementQuestionId { get; set; }

    /// <summary>
    ///     The question entity this fill data holds the data for.
    /// </summary>
    public RangeElementQuestion? RangeElementQuestion { get; set; }

    /// <summary>
    ///     The value entered by the user for a given question.
    /// </summary>
    /// <seealso cref="RangeElementQuestion"/>
    /// <seealso cref="ParentElement"/>
    public int Value { get; set; }
}
