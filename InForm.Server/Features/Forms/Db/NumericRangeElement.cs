using InForm.Server.Core.Features.Common;
using InForm.Server.Features.FillForms.Db;
using System.ComponentModel.DataAnnotations;

namespace InForm.Server.Features.Forms.Db;

/// <summary>
///     A form element type representing an element selector from a range.
/// </summary>
/// <remarks>
///     A range element contains an element-level encoded list of values.
///     And a list of questions which can be selected on the given scale.
/// </remarks>
public class NumericRangeElement : FormElementBase
{
    /// <summary>
    ///     The minimum value of the range labels.
    /// </summary>
    public int MinRange { get; set; } = 0;

    /// <summary>
    ///     The maximum value of the range labels.
    /// </summary>
    public int MaxRange { get; set; } = 5;

    /// <summary>
    ///     The list of questions in this range set.
    /// </summary>
    public List<RangeElementQuestion> Questions { get; set; } = [];

    /// <summary>
    ///     The list of this element's fill data.
    /// </summary>
    public List<NumericRangeFillData> FillData { get; set; } = [];

    /// <inheritdoc/>
    public override void Accept(IVisitor visitor)
    {
        if (visitor is not ITypedVisitor<NumericRangeElement> typed) return;
        typed.Visit(this);
    }

    /// <inheritdoc/>
    public override TResult? Accept<TResult>(IVisitor<TResult> visitor) where TResult : default
    {
        if (visitor is not ITypedVisitor<NumericRangeElement, TResult> typed) return default;
        return typed.Visit(this);
    }
}

/// <summary>
///     A scalar string holding a question in a range element's question set.
/// </summary>
public class RangeElementQuestion
{
    /// <summary>
    ///     The database identifier.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    ///     The id of the range this question belongs to.
    /// </summary>
    public required long RangeId { get; set; }

    /// <summary>
    ///     The element this question is part of.
    /// </summary>
    public NumericRangeElement? Parent { get; set; }

    /// <summary>
    ///     The value of the question.
    /// </summary>
    [StringLength(256)]
    public required string Value { get; set; }
}
