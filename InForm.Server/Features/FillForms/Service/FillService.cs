using InForm.Server.Core.Features.Common;
using InForm.Server.Core.Features.Fill;
using InForm.Server.Db;
using InForm.Server.Features.Common;
using InForm.Server.Features.FillForms.Db;
using InForm.Server.Features.Forms.Db;
using Microsoft.EntityFrameworkCore;
using System.Security.Authentication;

namespace InForm.Server.Features.FillForms.Service;

/// <summary>
///     The default implementation of the <see cref="IFillService"/> service
///     interface.
///     The fills are saved in the database.
/// </summary>
/// <param name="passwordHasher">The low-level password hashing utility.</param>
public class FillService(
    IPasswordHasher passwordHasher
) : IFillService {

    /// <inheritdoc/>
    public async Task FillFormWithData(InFormDbContext dbContext,
                                       Guid formId,
                                       IEnumerable<FillElement> fillElements)
    {
        var form = await dbContext.Forms.SingleAsync(x => x.IdGuid == formId);

        var fillObj = new Fill();
        var fills = fillElements.OrderBy(x => x.Id);

        var formElements = await dbContext.LoadAllElementsForForm(form);
        var elementVisitors = formElements.OrderBy(x => x.Id).Select(x => new FillDataDtoInjectorVisitor(fillObj, x));

        elementVisitors.Zip(fills).AsParallel().ForAll(AddWithVisitor);

        dbContext.UpdateRange(formElements);
        await dbContext.SaveChangesAsync();
    }

    private static void AddWithVisitor((FillDataDtoInjectorVisitor, FillElement) pair)
    {
        var (visitor, fill) = pair;
        fill.Accept(visitor);
    }

    /// <inheritdoc />
    public async Task<(Form, IEnumerable<ElementResponse>)>
        GetFormFillData(InFormDbContext dbContext, Guid formId, string? password)
    {
        var form = await dbContext.Forms.SingleAsync(x => x.IdGuid == formId);

        if (form.PasswordHash is {} hash)
        {
            var passResult = passwordHasher.VerifyAndUpdate(password ?? string.Empty, hash);
            if (!passResult.Verified) throw new InvalidCredentialException();

            if (passResult is { UpdatedHash: {} newHash })
            {
                form.PasswordHash = newHash;
                await dbContext.SaveChangesAsync();
            }
        }

        var formElements = await dbContext.LoadAllElementsForFormWithData(form);
        IVisitor<ElementResponse> visitor = new ToResponseDtoVisitor();

        return (form, formElements.Select(x => x.Accept(visitor)!));
    }
}
