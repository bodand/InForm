using Microsoft.AspNetCore.Mvc;

namespace InForm.Server.Features.FillForms;

/// <summary>
///     The controller class handling endpoints related to data
///     submitted to the given forms.
/// </summary>
[Route("/api/fills/{formId:guid}")]
[Consumes("application/json")]
[Produces("application/json")]
public class FillsController : ControllerBase
{
    /// <summary>
    ///     Adds a set of fill data to the given form.
    ///     The input is validated according to the same rules as it was done 
    ///     on the client side to ensure consistency.
    /// </summary>
    /// <param name="formId">The id of the form to add a fill to.</param>
    /// <response code="202">The fill has been successfully submitted.</response>
    /// <response code="404">The given form does not exist.</response>
    /// <response code="412">The fill failed the validation rules of the form.</response>
    [HttpPost]
    [ProducesResponseType(202)]
    [ProducesResponseType(404)]
    [ProducesResponseType(412)]
    public async Task<ActionResult> AddFillData(Guid formId)
    {
        return Ok(); // TODO
    }
}
