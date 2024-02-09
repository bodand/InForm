using InForm.Server.Core.Features.Common;
using System.ComponentModel.DataAnnotations;

namespace InForm.Features.Forms.Db;

public abstract class FormElementBase : IVisitable
{
    public long Id { get; set; }

    [StringLength(128)]
    public required string Title { get; set; }

    [StringLength(256)]
    public string? Subtitle { get; set; }

    public bool Required { get; set; }

    public Form? ParentForm { get; set; }

    public abstract void Accept(IVisitor visitor);
    public abstract TResult? Accept<TResult>(IVisitor<TResult> visitor) where TResult : notnull;
}