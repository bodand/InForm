using System.ComponentModel.DataAnnotations;

namespace InForm.Features.Forms.Db;

public class FormElementBase
{
    public long Id { get; set; }

    [StringLength(128)]
    public required string Title { get; set; }

    [StringLength(256)]
    public string? Subtitle { get; set; }

    public bool Required { get; set; }

    public Form? ParentForm { get; set; }
}