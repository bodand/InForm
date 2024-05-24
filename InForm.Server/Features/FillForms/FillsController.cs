using InForm.Server.Core.Features.Common;
using InForm.Server.Core.Features.Fill;
using InForm.Server.Db;
using InForm.Server.Features.Common;
using InForm.Server.Features.FillForms.Db;
using InForm.Server.Features.FillForms.Service;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Authentication;

namespace InForm.Server.Features.FillForms;

/// <summary>
///     The controller class handling endpoints related to data
///     submitted to the given forms.
/// </summary>
[Route("/api/fills/{formId:guid}")]
[Consumes("application/json")]
[Produces("application/json")]
public class FillsController(
    InFormDbContext dbContext,
    IFillService fillService
) : ControllerBase {
    /// <summary>
    ///     Adds a set of fill data to the given form.
    ///     The input is validated according to the same rules as it was done 
    ///     on the client side to ensure consistency.
    /// </summary>
    /// <param name="formId">The id of the form to add a fill to.</param>
    /// <param name="request">
    ///     The request body containing the set of data to fill the form with.
    /// </param>
    /// <response code="202">The fill has been successfully submitted.</response>
    /// <response code="400">The request's and the URI's formId mismatches.</response>
    /// <response code="404">The given form does not exist.</response>
    /// <response code="412">The fill failed the validation rules of the form.</response>
    [HttpPost]
    [ProducesResponseType(202)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(412)]
    public async Task<ActionResult> AddFillData(Guid formId, [FromBody] FillRequest request)
    {
        try
        {
            if (formId != request.FormId) return BadRequest();
            await using var tr = await dbContext.Database.BeginTransactionAsync();

            await fillService.FillFormWithData(dbContext, formId, request.Elements);

            await tr.CommitAsync();
            return Accepted();
        }
        catch (EntityNotFoundException)
        {
            return NotFound();
        }
        catch (InvalidOperationException)
        {
            return BadRequest();
        }
    }

    /// <summary> 
    ///     Retrieves the list of fill values for a given form.
    /// </summary>
    /// <param name="formId">The id of the form to add a fill to.</param>
    /// <param name="request">
    ///     The request body contianing the id to retrieve and the optional password.
    /// </param>
    /// <response code="200">The filled in responses of the form.</response>
    /// <response code="400">The request's and the URI's formId mismatches.</response>
    /// <response code="401">
    ///     The form's responses are not public and an invalid password was provided.
    /// </response>
    /// <response code="404">The given form does not exist.</response>
    [HttpPost(":retrieve")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<RetrieveFillsResponse>> GetFillData(Guid formId,
                                                                       [FromBody] RetrieveFillsRequest request)
    {
        try
        {
            if (formId != request.Id) return BadRequest();
            await using var tr = await dbContext.Database.BeginTransactionAsync();

            var (form, responses) =
                await fillService.GetFormFillData(dbContext, formId, request.Password);

            await tr.CommitAsync();
            return Ok(new RetrieveFillsResponse(form.Title, form.Subtitle, [..responses]));
        }
        catch (EntityNotFoundException)
        {
            return NotFound();
        }
        catch (InvalidOperationException)
        {
            return BadRequest();
        }
        catch (InvalidCredentialException)
        {
            return Unauthorized();
        }
    }
}
