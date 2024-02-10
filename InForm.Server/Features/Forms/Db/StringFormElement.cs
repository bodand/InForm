using InForm.Server.Core.Features.Common;
using InForm.Server.Features.FillForms.Db;
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

