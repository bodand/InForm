using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InForm.Server.Features.Forms.Db;

/// <summary>
///     A form entity stored in the database.
///     Represents a fillable top-level entity in InForm.
/// </summary>
public class Form
{
    /// <summary>
    ///     The database identifier.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    ///     The API identifier.
    /// </summary>
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid IdGuid { get; set; }

    /// <summary>
    ///     The title of the form.
    ///     This allows the creator of the form to name the form in a general sense, 
    ///     allowing, in turn, the filler of the form to know what this form is about.
    /// </summary>
    [StringLength(64)]
    public required string Title { get; set; }

    /// <summary>
    ///     The subtitle of the form.
    ///     Contains extra data about the form, that did not fit, or is not appropriate 
    ///     for the form title.
    /// </summary>
    [StringLength(128)]
    public string? Subtitle { get; set; }
    
    /// <summary>
    ///     The hash of the password, if the results of the form are not public.
    /// </summary>
    [StringLength(120)]
    public string? PasswordHash { get; set; }
    
    /// <summary>
    ///     The list of form elements the form contains.
    /// </summary>
    public List<FormElementBase> FormElementBases { get; set; } = [];
}