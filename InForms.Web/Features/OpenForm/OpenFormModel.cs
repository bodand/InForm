using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace InForms.Web.Features.OpenForm;

public class OpenFormModel
{
    [DisplayName("FormID")]
    [Required(ErrorMessage = "{0} is required")]
    [MinLength(4,
        ErrorMessage = "{0} must be at least {1} characters")]
    public string FormId { get; set; } = string.Empty;
}