using InForm.Server.Core.Features.Common;
using InForm.Server.Features.FillForms.Db;
using System.ComponentModel.DataAnnotations.Schema;

namespace InForm.Server.Features.Forms.Db;

/// <summary>
///     Database entity for a multi-choice form element entity.
///     A multi-choice presents multiple choices to the user for choose from,
///     and they must select up to MaxSelected options.
/// </summary>
public class MultiChoiceFormElement : FormElementBase {
    /// <summary>
    ///     The maximum amount of selectable options.
    /// </summary>
    public int MaxSelected { get; set; } = 1;

    /// <summary>
    ///     The list of options in the multi-choice.
    /// </summary>
    public List<MultiChoiceOption> Options { get; set; } = [];

    /// <summary>
    ///     The list of options as strings.
    /// </summary>
    [NotMapped]
    public IEnumerable<string> StringOptions => Options.Select(x => x.Value);
    
    /// <summary>
    ///     The fill data of the form element entity.
    /// </summary>
    public MultiChoiceFillData? FillData { get; set; }

    /// <inheritdoc />
    public override void Accept(IVisitor visitor)
    {
        if (visitor is not ITypedVisitor<MultiChoiceFormElement> typedVisitor) return;
        typedVisitor.Visit(this);
    }

    /// <inheritdoc />
    public override TResult? Accept<TResult>(IVisitor<TResult> visitor)
        where TResult : default
    {
        if (visitor is not ITypedVisitor<MultiChoiceFormElement, TResult> typed) return default;
        return typed.Visit(this);
    }
}

/// <summary>
///     An option of a multi-choice form element.
/// </summary>
public class MultiChoiceOption {
    /// <summary>
    ///     The database id.
    /// </summary>
    public long Id { get; set; }
    
    /// <summary>
    ///     The identifier of the multiple choice element.
    /// </summary>
    public long ElementId { get; set; }
    
    /// <summary>
    ///     Navigation property to the owning element.
    /// </summary>
    public MultiChoiceFormElement? Element { get; set; }
    
    /// <summary>
    ///     The value of the multi-choice element.
    /// </summary>
    public required string Value { get; set; }
}
