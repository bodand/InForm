using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InForm.Features.Forms.Db;

public class Form
{
    public long Id { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid IdGuid { get; set; }

    [StringLength(64)]
    public required string Title { get; set; }

    [StringLength(128)]
    public string? Subtitle { get; set; }

    public List<FormElementBase> FormElementBases { get; set; } = [];
}