using InForm.Server.Core.Features.Fill;
using InForm.Server.Db;
using InForm.Server.Features.Common;
using InForm.Server.Features.FillForms.Db;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
    IPasswordHasher passwordHasher
) : ControllerBase
{
    /// <summary>
    ///     Adds a set of fill data to the given form.
    ///     The input is validated according to the same rules as it was done 
    ///     on the client side to ensure consistency.
    /// </summary>
    /// <param name="formId">The id of the form to add a fill to.</param>
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

            var form = await dbContext.Forms.SingleOrDefaultAsync(x => x.IdGuid == formId);
            if (form is null) return NotFound();

            var fillObj = new Fill();
            var fills = request.Elements.OrderBy(x => x.Id);

            var formElements = await dbContext.LoadAllElementsForForm(form);
            var elementVisitors = formElements.OrderBy(x => x.Id).Select(x => new FillDataDtoInjectorVisitor(fillObj, x));

            elementVisitors.Zip(fills).AsParallel().ForAll(AddWithVisitor);

            dbContext.UpdateRange(formElements);
            await dbContext.SaveChangesAsync();
            await tr.CommitAsync();
            return Accepted();
        }
        catch (InvalidElementTypeException)
        {
            // todo log this
            return BadRequest();
        }
    }

    private static void AddWithVisitor((FillDataDtoInjectorVisitor, FillElement) pair)
    {
        var (visitor, fill) = pair;
        fill.Accept(visitor);
    }

    /// <summary> 
    ///     Retrieves the list of fill values for a given form.
    /// </summary>
    /// <param name="formId">The id of the form to add a fill to.</param>
    /// <response code="200">The filled in responses of the form.</response>
    /// <response code="401">The form's responses are not public and an invalid password was provided.</response>
    /// <response code="404">The given form does not exist.</response>
    [HttpPost(":retrieve")]
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<RetrieveFillsResponse>> GetFillData(Guid formId, [FromBody] RetrieveFillsRequest request)
    {
        await using var tr = await dbContext.Database.BeginTransactionAsync();

        var form = await dbContext.Forms.SingleOrDefaultAsync(x => x.IdGuid == formId);
        if (form is null) return NotFound();

        if (form.PasswordHash is { } hash)
        {
            var passResult = passwordHasher.VerifyAndUpdate(request.Password ?? string.Empty, hash);
            if (!passResult.Verified) return Unauthorized();

            if (passResult is { UpdatedHash: { } newHash })
            {
                form.PasswordHash = newHash;
                await dbContext.SaveChangesAsync();
            }
        }

        var formElements = await dbContext.LoadAllElementsForFormWithData(form);
        var visitor = new ToResponseDtoVisitor();
        var response = new RetrieveFillsResponse(
            form.Title,
            form.Subtitle,
            [.. formElements.Select(x => x.Accept(visitor))]);

        await tr.CommitAsync();
        return response;
    }

}
