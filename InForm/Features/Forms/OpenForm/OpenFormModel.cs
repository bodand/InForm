using System.ComponentModel.DataAnnotations;

namespace InForm.Features.Forms.OpenForm;

public class OpenFormModel
{
    [Required]
    [MinLength(4)]
    public string FormId { get; set; } = string.Empty;
}