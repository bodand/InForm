using InForm.Server.Features.Forms.Db;

namespace InForm.Server.Features.FillForms.Db;

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
    // ReSharper disable once EntityFramework.ModelValidation.UnlimitedStringLength
    public string? Value { get; set; }
}

