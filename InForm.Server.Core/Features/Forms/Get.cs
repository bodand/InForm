using InForm.Server.Core.Features.Common;
using System.Text.Json.Serialization;

namespace InForm.Server.Core.Features.Forms;

public readonly record struct GetFormReponse(
    Guid Id,
    string Title,
    string? Subtitle,
    List<GetFormElement> FormElements
);

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
