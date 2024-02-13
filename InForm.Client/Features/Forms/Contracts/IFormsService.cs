using InForm.Server.Core.Features.Forms;

namespace InForm.Client.Features.Forms.Contracts;

public interface IFormsService
{
	Task<FormModel> GetForm(Guid id);
	Task<GetFormNameResponse> GetFormName(Guid id);
	Task<Guid> CreateForm(FormModel model);
}
