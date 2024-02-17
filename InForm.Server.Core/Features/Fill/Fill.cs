using InForm.Server.Core.Features.Common;
using System.Text.Json.Serialization;

namespace InForm.Server.Core.Features.Fill;

/// <summary>
///     A request DTO for creating a fill for a form.
///     The form's validation will be executed on the data on the server.
/// </summary>
/// <param name="FormId">The identifier of the form.</param>
/// <param name="Elements">The list of form elements' fills to insert.</param>
public readonly record struct FillRequest(
    Guid FormId,
    List<FillElement> Elements
);

/// <summary>
///     Abstract base of typed fill elements.
/// </summary>
/// <param name="Id">The identifier of the form element this fill element is for.</param>
[JsonPolymorphic(TypeDiscriminatorPropertyName = "$t")]
[JsonDerivedType(typeof(StringFillElement), "string")]
public abstract record FillElement(
    long Id
) : IVisitable {
    /// <inheritdoc/>
    public abstract void Accept(IVisitor visitor);

    /// <inheritdoc/>
    public abstract TResult? Accept<TResult>(IVisitor<TResult> visitor) where TResult : notnull;
}

/// <summary>
///     The fill element for string fill elements.
/// </summary>
/// <param name="Id">The identifier of the string form element.</param>
/// <param name="Value">The value of the element.</param>
public record StringFillElement(
    long Id,
    string? Value
) : FillElement(Id) {
    /// <inheritdoc/>
    public override void Accept(IVisitor visitor)
    {
        if (visitor is not ITypedVisitor<StringFillElement> typed) return;
        typed.Visit(this);
    }

    /// <inheritdoc/>
    public override TResult? Accept<TResult>(IVisitor<TResult> visitor) where TResult : default
    {
        if (visitor is not ITypedVisitor<StringFillElement, TResult> typed) return default;
        return typed.Visit(this);
    }
}

public record MultiChoiceFillElement(
    long Id,
    List<string> Selected
) : FillElement(Id) {
    /// <inheritdoc/>
    public override void Accept(IVisitor visitor)
    {
        if (visitor is not ITypedVisitor<MultiChoiceFillElement> typed) return;
        typed.Visit(this);
    }

    /// <inheritdoc/>
    public override TResult? Accept<TResult>(IVisitor<TResult> visitor) where TResult : default
    {
        if (visitor is not ITypedVisitor<MultiChoiceFillElement, TResult> typed) return default;
        return typed.Visit(this);
    }
}
