using InForm.Server.Core.Features.Common;
using System.Text.Json.Serialization;

namespace InForm.Server.Core.Features.Forms;

/// <summary>
///     The DTO for getting a form for display.
/// </summary>
/// <param name="Id">The identifier of the form.</param>
/// <param name="Title">The title of the form.</param>
/// <param name="Subtitle">
///     The subtitle of the form, if it exists. Null if the form does not have a
///     subtitle specified.
/// </param>
/// <param name="FormElements">A list of elements in the form.</param>
/// <seealso cref="GetFormElement"/>
public readonly record struct GetFormReponse(
    Guid Id,
    string Title,
    string? Subtitle,
    List<GetFormElement> FormElements
);

/// <summary>
///     The polymorphic base of the DTO entity for getting (querying) the
///     elements of a form.
/// </summary>
/// <param name="Id">
///     The id of the element. Guaranteed monotonically increasing sequence
///     within the scope of a form. Ordered in the order the elements should be
///     rendered in.
/// </param>
/// <param name="Title">The title of the element.</param>
/// <param name="Subtitle">
///     The optional subtitle of the element. Null if the form does not have a
///     subtitle specified.
/// </param>
/// <param name="Required">
///     Whether this element is required when filling the form.
/// </param>
[JsonPolymorphic(TypeDiscriminatorPropertyName = "$t")]
[JsonDerivedType(typeof(GetStringFormElement), "string")]
[JsonDerivedType(typeof(GetMultiChoiceElement), "mc")]
public abstract record GetFormElement(
    long Id,
    string Title,
    string? Subtitle,
    bool Required
) : IVisitable {
    /// <inheritdoc />
    public abstract void Accept(IVisitor visitor);

    /// <inheritdoc />
    public abstract TResult? Accept<TResult>(IVisitor<TResult> visitor) where TResult : notnull;
}

/// <summary>
///     A DTO entity for getting (querying) a string element.
/// </summary>
/// <param name="Id">
///     The id of the element. Guaranteed monotonically increasing sequence
///     within the scope of a form. Ordered in the order the elements should be
///     rendered in.
/// </param>
/// <param name="Title">The title of the element.</param>
/// <param name="Subtitle">
///     The optional subtitle of the element. Null if the form does not have a
///     subtitle specified.
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
public record GetStringFormElement(
    long Id,
    string Title,
    string? Subtitle,
    int MaxLength,
    bool Required,
    bool Multiline
) : GetFormElement(Id, Title, Subtitle, Required) {
    /// <inheritdoc />
    public override void Accept(IVisitor visitor)
    {
        if (visitor is not ITypedVisitor<GetStringFormElement> typedVisitor) return;
        typedVisitor.Visit(this);
    }

    /// <inheritdoc />
    public override TResult? Accept<TResult>(IVisitor<TResult> visitor) where TResult : default
    {
        if (visitor is not ITypedVisitor<GetStringFormElement, TResult> typedVisitor) return default;
        return typedVisitor.Visit(this);
    }
}

/// <summary>
///     A DTO entity for getting (querying) a multi-choice element.
/// </summary>
/// <param name="Id">
///     The id of the element. Guaranteed monotonically increasing sequence
///     within the scope of a form. Ordered in the order the elements should be
///     rendered in.
/// </param>
/// <param name="Title">The title of the element.</param>
/// <param name="Subtitle">
///     The optional subtitle of the element. Null if the form does not have a
///     subtitle specified.
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
public record GetMultiChoiceElement(
    long Id,
    string Title,
    string? Subtitle,
    bool Required,
    List<string> Options,
    int Selectable
) : GetFormElement(Id, Title, Subtitle, Required) {
    /// <inheritdoc />
    public override void Accept(IVisitor visitor)
    {
        if (visitor is not ITypedVisitor<GetMultiChoiceElement> typedVisitor) return;
        typedVisitor.Visit(this);
    }

    /// <inheritdoc />
    public override TResult? Accept<TResult>(IVisitor<TResult> visitor) where TResult : default
    {
        if (visitor is not ITypedVisitor<GetMultiChoiceElement, TResult> typedVisitor) return default;
        return typedVisitor.Visit(this);
    }
}
