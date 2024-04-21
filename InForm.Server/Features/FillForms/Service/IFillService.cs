using InForm.Server.Core.Features.Fill;
using InForm.Server.Db;
using InForm.Server.Features.Forms.Db;

namespace InForm.Server.Features.FillForms.Service;

/// <summary>
///     The interface representing the service for filling out forms.
/// </summary>
public interface IFillService {
    /// <summary>
    ///     Fills out a form specified by its id.
    /// </summary>
    /// <param name="dbContext">The database context to work on.</param>
    /// <param name="formId">The id of the form.</param>
    /// <param name="fillElements">
    ///     The list of elements to fill the form with.
    /// </param>
    Task FillFormWithData(InFormDbContext dbContext,
                          Guid formId,
                          IEnumerable<FillElement> fillElements);

    /// <summary>
    ///     Retrieves a form and the list of responses given to it.
    /// </summary>
    /// <param name="dbContext">The database context to work on.</param>
    /// <param name="formId">The id of the form.</param>
    /// <param name="password">Optional password, if the responses are private.</param>
    /// <returns>The form and the list of responses.</returns>
    Task<(Form, IEnumerable<ElementResponse>)> GetFormFillData(InFormDbContext dbContext,
                                                               Guid formId,
                                                               string? password);
}