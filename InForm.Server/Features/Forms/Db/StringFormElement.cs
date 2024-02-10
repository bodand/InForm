using InForm.Server.Core.Features.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace InForm.Features.Forms.Db;

/// <summary>
///     A concrete entity type representing a single text field of the form.
/// </summary>
public class StringFormElement : FormElementBase
{
    /// <summary>
    ///     Whether this question's response field should be rendered as a multiline control.
    /// </summary>
    public bool RenderAsTextArea { get; set; }

    /// <summary>
    ///     Sets the maximum available character count for the response.
    ///     If unset (null), there is not upper limit.
    /// </summary>
    public int? MaxLength { get; set; }

    /// <summary>
    ///     Navigation property of filled data corresponding to this field.
    /// </summary>
    public List<StringFillData> FillData { get; set; } = [];

    /// <inheritdoc/>
    public override void Accept(IVisitor visitor)
    {
        if (visitor is not ITypedVisitor<StringFormElement> typedVisitor) return;
        typedVisitor.Visit(this);
    }

    /// <inheritdoc/>
    public override TResult? Accept<TResult>(IVisitor<TResult> visitor) where TResult : default
    {
        if (visitor is not ITypedVisitor<StringFormElement, TResult> typedVisitor) return default;
        return typedVisitor.Visit(this);
    }
}

/// <summary>
///     An entity representing a fill data entity of a string form element.
///     It contains but a simple string field.
/// </summary>
public class StringFillData : FillData
{
    /// <summary>
    ///     The form element this fill data is for.
    /// </summary>
    public StringFormElement? ParentElement { get; set; }

    /// <summary>
    ///     The string value provided by the filler.
    ///     Null if the value is not required by the parent element, and the filler did not fill this value.
    /// </summary>
    public string? Value { get; set; }
}

