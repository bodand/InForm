using InForm.Server.Core.Features.Common;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace InForm.Server.Core.Features.Forms;

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

[JsonPolymorphic(TypeDiscriminatorPropertyName = "$t")]
[JsonDerivedType(typeof(CreateStringElement), "string")]
[JsonDerivedType(typeof(CreateNumericRangeElement), "nrange")]
public abstract record CreateFormElement(
    [StringLength(128)]
    string Title,
    [StringLength(256)]
    string? Subtitle,
    bool Required
) : IVisitable
{
    public abstract void Accept(IVisitor visitor);
    public abstract TResult? Accept<TResult>(IVisitor<TResult> visitor) where TResult : notnull;
}

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
    public override void Accept(IVisitor visitor)
    {
        if (visitor is not ITypedVisitor<CreateStringElement> typedVisitor) return;
        typedVisitor.Visit(this);
    }

    public override TResult? Accept<TResult>(IVisitor<TResult> visitor) where TResult : default
    {
        if (visitor is not ITypedVisitor<CreateStringElement, TResult> typedVisitor) return default;
        return typedVisitor.Visit(this);
    }
} 

public record CreateNumericRangeElement(
    [StringLength(128)]
    string Title,
    [StringLength(256)]
    string? Subtitle,
    bool Required,
    int MinRange,
    int MaxRange,
    List<string> Questions
) : CreateFormElement(Title, Subtitle, Required)
{
    public override void Accept(IVisitor visitor)
    {
        if (visitor is not ITypedVisitor<CreateNumericRangeElement> typedVisitor) return;
        typedVisitor.Visit(this);
    }

    public override TResult? Accept<TResult>(IVisitor<TResult> visitor) where TResult : default
    {
        if (visitor is not ITypedVisitor<CreateNumericRangeElement, TResult> typedVisitor) return default;
        return typedVisitor.Visit(this);
    }
}

/// <summary>
///     The response type generated for a valid create form request.
/// </summary>
/// <param name="Id">The public identifier of the newly created form entity.</param>
public readonly record struct CreateFormResponse(
    Guid Id
);
