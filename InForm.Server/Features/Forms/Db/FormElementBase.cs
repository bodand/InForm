using InForm.Server.Core.Features.Common;
using System.ComponentModel.DataAnnotations;

namespace InForm.Features.Forms.Db;

/// <summary>
///     The base class of all form elements.
///     A form element contains an atomic fillable entity in a form.
///     The type of the value asked of the filler is specified by the 
///     concrete type inherited from this type.
/// </summary>
public abstract class FormElementBase : IVisitable
{
    /// <summary>
    ///     The database identity.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    ///     The title of the fill element.
    ///     Should contain the immediate question the filler should provide their answer.
    ///     For example something along the lines of `Do you have any other feedback for us? Write it below.'.
    /// </summary>
    [StringLength(128)]
    public required string Title { get; set; }

    /// <summary>
    ///     The subtitle of the fill element.
    ///     An optional extension of the type specified.
    /// </summary>
    [StringLength(256)]
    public string? Subtitle { get; set; }

    /// <summary>
    ///     Whether filling this element is required.
    /// </summary>
    public bool Required { get; set; }

    /// <summary>
    ///     Navigation property to the owning Form entity.
    /// </summary>
    public Form? ParentForm { get; set; }

    /// <inheritdoc/>
    public abstract void Accept(IVisitor visitor);

    /// <inheritdoc/>
    public abstract TResult? Accept<TResult>(IVisitor<TResult> visitor) where TResult : notnull;
}

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
}