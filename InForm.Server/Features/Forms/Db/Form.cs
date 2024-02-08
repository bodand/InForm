using System.ComponentModel.DataAnnotations;

namespace InForm.Features.Forms.Db;

public class Form
{
    public long Id { get; set; }

    [StringLength(64)]
    public required string Title { get; set; }

    [StringLength(128)]
    public string? Subtitle { get; set; }

    public List<FormElementBase> FormElementBases { get; set; } = [];
}