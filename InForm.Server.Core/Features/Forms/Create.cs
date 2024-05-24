using InForm.Server.Core.Features.Common;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace InForm.Server.Core.Features.Forms;

/// <summary>
///     The DTO entity for creating a form.
/// </summary>
/// <param name="Title">The title of the whole form to be created.</param>
/// <param name="Subtitle">
///     The optional subtitle of the form to be created.
/// </param>
/// <param name="Password">
///     An optional password to assign to the form. If not null, this is
///     required to view the responses.
/// </param>
/// <param name="Elements">The list of elements to create the form with.</param>
/// <seealso cref="CreateFormElement"/>
public readonly record struct CreateFormRequest(
    [StringLength(64)]
    string Title,
    [StringLength(128)]
    string? Subtitle,
    [MinLength(3)]
    string? Password,
    [MinLength(1)]
    List<CreateFormElement> Elements
);

/// <summary>
///     The polymorphic base of the DTO entity for specifying the elements of a
///     form during creation.
/// </summary>
/// <param name="Title">The title of the element to be created.</param>
/// <param name="Subtitle">
///     The optional subtitle of the element to be created.
/// </param>
/// <param name="Required">
///     Whether this element is required when filling the form.
/// </param>
[JsonPolymorphic(TypeDiscriminatorPropertyName = "$t")]
[JsonDerivedType(typeof(CreateStringElement), "string")]
[JsonDerivedType(typeof(CreateMultiChoiceElement), "mc")]
public abstract record CreateFormElement(
    [StringLength(128)]
    string Title,
    [StringLength(256)]
    string? Subtitle,
    bool Required
) : IVisitable {
    /// <inheritdoc />
    public abstract void Accept(IVisitor visitor);

    /// <inheritdoc />
    public abstract TResult? Accept<TResult>(IVisitor<TResult> visitor) where TResult : notnull;
}

/// <summary>
///     A DTO entity specifying the creation of a string element.
/// </summary>
/// <param name="Title">The title of the element to be created.</param>
/// <param name="Subtitle">
///     The optional subtitle of the element to be created.
/// </param>
/// <param name="Required">
///     Whether this element is required when filling the form.
/// </param>
/// <param name="MaxLength">
///     The maximum length of the string that can be input to this element.
/// </param>
/// <param name="Multiline">
///     Whether this string entry element is to be rendered in a multi-line
///     entry on the fill page.
/// </param>
public record CreateStringElement(
    [StringLength(128)]
    string Title,
    [StringLength(256)]
    string? Subtitle,
    [Range(0, int.MaxValue)]
    int MaxLength,
    bool Required,
    bool Multiline
) : CreateFormElement(Title, Subtitle, Required)
{
    /// <inheritdoc />
    public override void Accept(IVisitor visitor)
    {
        if (visitor is not ITypedVisitor<CreateStringElement> typedVisitor) return;
        typedVisitor.Visit(this);
    }

    /// <inheritdoc />
    public override TResult? Accept<TResult>(IVisitor<TResult> visitor) where TResult : default
    {
        if (visitor is not ITypedVisitor<CreateStringElement, TResult> typedVisitor) return default;
        return typedVisitor.Visit(this);
    }
}

/// <summary>
///     A DTO entity specifying the creation of a multi-choice element.
/// </summary>
/// <param name="Title">The title of the element to be created.</param>
/// <param name="Subtitle">
///     The optional subtitle of the element to be created.
/// </param>
/// <param name="Required">
///     Whether this element is required when filling the form.
/// </param>
/// <param name="Options">
///     The list of elements to show on the element, to allow choosing from.
/// </param>
/// <param name="Selectable">
///     The number of maximum options that can be selected on the element. Only
///     a true multi-choice, if this is >1.
/// </param>
public record CreateMultiChoiceElement(
    string Title,
    string? Subtitle,
    bool Required,
    List<string> Options,
    int Selectable
) : CreateFormElement(Title, Subtitle, Required) {

    /// <inheritdoc />
    public override void Accept(IVisitor visitor)
    {
        if (visitor is not ITypedVisitor<CreateMultiChoiceElement> typed) return;
        typed.Visit(this);
    }

    /// <inheritdoc />
    public override TResult? Accept<TResult>(IVisitor<TResult> visitor)
        where TResult : default
    {
        if (visitor is not ITypedVisitor<CreateMultiChoiceElement, TResult> typed) return default;
        return typed.Visit(this);
    }
}

/// <summary>
///     The response type generated for a valid create form request.
/// </summary>
/// <param name="Id">The public identifier of the newly created form entity.</param>
public readonly record struct CreateFormResponse(
    Guid Id
);
