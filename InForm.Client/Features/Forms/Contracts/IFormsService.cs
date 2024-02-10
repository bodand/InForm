namespace InForm.Client.Features.Forms.Contracts;

public interface IFormsService
{
	Task<FormModel> GetForm(Guid id);
	Task<Guid> CreateForm(FormModel model);
	Task AddFill(FormModel model);
}
