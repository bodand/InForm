using InForm.Features.Forms.Db;
using InForm.Server.Core.Features.Forms;
using InForm.Server.Db;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InForm.Server.Features.Forms;

[Route("/api/forms")]
[ApiController]
public class FormsController(InFormDbContext dbContext) : ControllerBase
{

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<GetFormReponse>> GetForm(Guid id)
    {
        var form = await dbContext.Forms.AsNoTracking()
                                        .Include(x => x.FormElementBases)
                                        .SingleOrDefaultAsync(x => x.IdGuid == id);
        if (form is null) return NotFound();

        var toDtoVisitor = new ToGetDtoVisitor();
        var elems = form.FormElementBases.AsParallel()
                                         .Select(x => x.Accept(toDtoVisitor)!)
                                         .ToList();

        return new GetFormReponse(form.IdGuid, form.Title, form.Subtitle, elems);
    }

    [HttpPost]
    public async Task<ActionResult<CreateFormResponse>> CreateForm(CreateFormRequest request)
    {
        try
        {
            var newForm = FromDto(request);
            await dbContext.Forms.AddAsync(newForm);
            await dbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetForm),
                new { id = newForm.IdGuid },
                new CreateFormResponse(newForm.IdGuid));
        }
        catch (DbUpdateException)
        {
            return Conflict();
        }
        catch (Exception)
        {
            return BadRequest();
        }
    }

    private static Form FromDto(CreateFormRequest form)
    {
        var res = new Form()
        {
            Title = form.Title,
            Subtitle = form.Subtitle,
            FormElementBases = []
        };
        var elementVisitor = new CreateFromDtoVisitor(res);
        res.FormElementBases.AddRange(from elem in form.Elements
                                      select elem.Accept(elementVisitor));

        return res;
    }
}


