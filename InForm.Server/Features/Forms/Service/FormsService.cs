using InForm.Server.Core.Features.Common;
using InForm.Server.Db;
using InForm.Server.Features.Common;
using InForm.Server.Features.Forms.Db;
using Microsoft.EntityFrameworkCore;

namespace InForm.Server.Features.Forms.Service;

/// <summary>
///     The default implementation of the form service.
/// </summary>
public class FormsService(
    IPasswordHasher passwordHasher
) : IFormsService {
    /// <inheritdoc />
    public async Task<Form> GetForm(
        InFormDbContext dbContext,
        Guid id)
    {
        var form = await dbContext.Forms.AsNoTracking()
                       .SingleOrDefaultAsync(x => x.IdGuid == id);
        if (form is null) throw new EntityNotFoundException();
        return form;
    }

    /// <inheritdoc />
    public async Task<(Form, List<TFormat>)> GetFormWithElements<TFormat>(
        InFormDbContext dbContext,
        Guid id,
        IVisitor<TFormat> formatter)
        where TFormat : notnull
    {
        var form = await dbContext.Forms.AsNoTracking()
                       .Include(x => x.FormElementBases)
                       .SingleOrDefaultAsync(x => x.IdGuid == id);
        if (form is null) throw new EntityNotFoundException();

        var elems = form.FormElementBases
            .AsParallel()
            .Select(x => x.Accept(formatter)!)
            .ToList();
        return (form, elems);
    }

    /// <inheritdoc />
    public async Task SaveForm(InFormDbContext dbContext,
                               Form form)
    {
        await dbContext.Forms.AddAsync(form);
        await dbContext.SaveChangesAsync();
    }
}
