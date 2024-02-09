using InForm.Client.Features.Forms.Contracts;
using InForm.Server.Core.Features.Forms;
using System.Net.Http.Json;
using System.Text.Json;

namespace InForm.Client.Features.Forms.Impl;

internal class FormsService(
	HttpClient httpClient
) : IFormsService
{
	private readonly JsonSerializerOptions _jsonOptions = new()
	{
		AllowTrailingCommas = true,
		PropertyNameCaseInsensitive = true,
	};

	public async Task<Guid> CreateForm(CreateFormModel model)
	{
		var request = BuildRequest(model);
		var uri = "/api/forms";
		var response = await PostAsync(httpClient, request, uri);

		await using var stream = await response.Content.ReadAsStreamAsync();
		var responsePayload = await JsonSerializer.DeserializeAsync<CreateFormResponse>(stream, _jsonOptions);
		return responsePayload.Id;
	}

	private CreateFormRequest BuildRequest(CreateFormModel model)
	{
		var request = new CreateFormRequest(model.Title, model.Subtitle, []);
		request.Elements.AddRange(model.ElementModels.Select(CreateDtoFromModel));
		return request;
	}

	private CreateFormElement CreateDtoFromModel(ElementModel elementModel) 
		=> elementModel.ToDto();

	private static async Task<HttpResponseMessage> PostAsync(HttpClient httpClient, CreateFormRequest request, string uri)
	{
		var response = await httpClient.PostAsJsonAsync(uri, request);
		if (!response.IsSuccessStatusCode) // todo proper exception type
			throw new ApplicationException($"Failed communicating with service: {uri} returned {response.StatusCode}");
		return response;
	}
}
