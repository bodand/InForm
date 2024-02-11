namespace InForm.Server.Features.FillForms.Db;

/// <summary>
///     The common abstract class representing all element fill data entities.
///     A fill data is a collection of related information for a given form element (field)
///     that are input by the filler of the form.
/// </summary>
public abstract class FillData
{
    /// <summary>
    ///     The database identifier.
    /// </summary>
    public long Id { get; set; }

    public long FillId { get; set; }

    public Fill? Fill { get; set; }
}