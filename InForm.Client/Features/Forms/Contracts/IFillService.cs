using InForm.Server.Core.Features.Fill;

namespace InForm.Client.Features.Forms.Contracts;

public interface IFillService
{
	Task AddFill(FormModel model);
	Task<RetrieveFillsResponse> GetResponses(Guid id, string? password);
}
