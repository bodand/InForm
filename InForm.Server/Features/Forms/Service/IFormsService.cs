using InForm.Server.Core.Features.Common;
using InForm.Server.Db;
using InForm.Server.Features.Forms.Db;

namespace InForm.Server.Features.Forms.Service;

/// <summary>
///     The interface providing the functionality of manipulating forms.
/// </summary>
public interface IFormsService {
    /// <summary>
    ///     Returns a form by its id.
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="id">The public id of the form to retrieve it by.</param>
    /// <returns>The form entity.</returns>
    Task<Form> GetForm(InFormDbContext dbContext, Guid id);

    /// <summary>
    ///     Returns a form by its id, and lists its elements.
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="id">The public id of the form to retrieve it by.</param>
    /// <param name="formatter">The formatter for the element classes.</param>
    /// <returns>The form entity.</returns>
    Task<(Form, List<TFormat>)> GetFormWithElements<TFormat>(InFormDbContext dbContext,
                                                             Guid id,
                                                             IVisitor<TFormat> formatter)
        where TFormat : notnull;

    /// <summary>
    ///     Saves a form entity for later usage.
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="form">The form entity to save.</param>
    /// <returns>An awaitable task for the form's insertion.</returns>
    Task SaveForm(InFormDbContext dbContext, Form form);
}
