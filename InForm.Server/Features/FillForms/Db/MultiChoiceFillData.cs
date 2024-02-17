using InForm.Server.Features.Forms.Db;
using System.ComponentModel.DataAnnotations.Schema;

namespace InForm.Server.Features.FillForms.Db;

/// <summary>
///     Fill data entity for a multi-choice form element.
/// </summary>
public class MultiChoiceFillData : FillData {

    /// <summary>
    ///     The form element this fill data is for.
    /// </summary>
    public StringFormElement? ParentElement { get; set; }

    /// <summary>
    ///     The list of selected options in this form element fill data.
    /// </summary>
    /// <seealso cref="StringSelected"/>
    public List<MultiChoiceFillSelection> Selected { get; set; } = [];

    /// <summary>
    ///     The list of selected options as strings.
    /// </summary>
    /// <seealso cref="Selected"/>
    [NotMapped]
    public IEnumerable<string> StringSelected => Selected
                                                 .Where(x => x.Option is not null)
                                                 .Select(x => x.Option!.Value);
}

/// <summary>
///     An entity representing an selected entry in a filled multi-choice element.
/// </summary>
public class MultiChoiceFillSelection {
    /// <summary>
    ///     The database id.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    ///     Id of the fill data entity this selection is part of.
    /// </summary>
    public long FillDataId { get; set; }

    /// <summary>
    ///     Navigation property to the owning.
    /// </summary>
    public MultiChoiceFillData? FillData { get; set; }

    /// <summary>
    ///     Id of the selected option.
    /// </summary>
    public long OptionId { get; set; }

    /// <summary>
    ///     Navigation property to the referenced option.
    /// </summary>
    public MultiChoiceOption? Option { get; set; }
}
