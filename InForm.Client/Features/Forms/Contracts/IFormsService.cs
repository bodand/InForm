namespace InForm.Client.Features.Forms.Contracts;

public interface IFormsService
{
	Task<Guid> CreateForm(CreateFormModel model);
}
