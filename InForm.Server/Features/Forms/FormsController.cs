using InForm.Server.Core.Features.Common;
using InForm.Server.Core.Features.Forms;
using InForm.Server.Db;
using InForm.Server.Features.Common;
using InForm.Server.Features.Forms.Db;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InForm.Server.Features.Forms;

/// <summary>
///     Controller for managing the endpoint for interacting with forms.
///     Forms are created by and filled by end users, client applications can 
///     access this functionality through the REST API implemented by this controller.
/// </summary>
/// <param name="dbContext">The database access object.</param>
[Route("/api/forms")]
[ApiController]
[Consumes("application/json")]
[Produces("application/json")]
public class FormsController(
    InFormDbContext dbContext,
    IPasswordHasher passwordHasher,
    ILogger<FormsController> logger
) : ControllerBase
{
    /// <summary>
    ///     Return a form by the given form, identified by its id.
    /// </summary>
    /// <param name="id">The id of the form.</param>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<GetFormReponse>> GetForm(Guid id)
    {
        var form = await dbContext.Forms.AsNoTracking()
                                        .Include(x => x.FormElementBases)
                                        .SingleOrDefaultAsync(x => x.IdGuid == id);
        if (form is null) return NotFound();

        IVisitor<GetFormElement> toDtoVisitor = new ToGetDtoVisitor();
        var elems = form.FormElementBases.AsParallel()
                                         .Select(x => x.Accept(toDtoVisitor)!)
                                         .ToList();

        return new GetFormReponse(form.IdGuid, form.Title, form.Subtitle, elems);
    }

    /// <summary>
    ///     Return the name of a given form.
    ///     When only the name is needed, prefer this endpoint over the full form,
    ///     because this is considerable more lightweight.
    /// </summary>
    /// <param name="id">The id of the form.</param>
    [HttpGet("{id:guid}/name")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<GetFormNameResponse>> GetFormName(Guid id)
    {
        var form = await dbContext.Forms.AsNoTracking()
                                        .SingleOrDefaultAsync(x => x.IdGuid == id);
        if (form is null) return NotFound();

        return new GetFormNameResponse(form.IdGuid, form.Title, form.Subtitle);
    }

    /// <summary>
    ///     Creates a thereon fillable form entity.
    ///     This stores the fields that need to be filled by the end-users when filling this form.
    /// </summary>
    /// <param name="request">The data representing the form.</param>
    /// <returns>The identifier of the form.</returns>
    [HttpPost]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(409)]
    public async Task<ActionResult<CreateFormResponse>> CreateForm(CreateFormRequest request)
    {
        try
        {
            await using var tr = await dbContext.Database.BeginTransactionAsync();
            var newForm = FromDto(request);

            await dbContext.Forms.AddAsync(newForm);
            await dbContext.SaveChangesAsync();

            await tr.CommitAsync();

            return CreatedAtAction(nameof(GetForm),
                new { id = newForm.IdGuid },
                new CreateFormResponse(newForm.IdGuid));
        }
        catch (DbUpdateException)
        {
            return Conflict();
        }
        catch (Exception ex)
        {
            logger.LogError(new(2,"error"), ex, "Error processing form creation request.");
            return BadRequest();
        }
    }

    private Form FromDto(CreateFormRequest form)
    {
        var res = new Form()
        {
            Title = form.Title,
            Subtitle = form.Subtitle,
            PasswordHash = form.Password is null ? null : passwordHasher.Hash(form.Password),
            FormElementBases = []
        };
        IVisitor<FormElementBase> elementVisitor = new FromCreateDtoVisitor(res);
        res.FormElementBases.AddRange(from elem in form.Elements
                                      select elem.Accept(elementVisitor));

        return res;
    }
}


