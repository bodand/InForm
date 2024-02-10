using InForm.Server.Core.Features.Forms;
using System.Net.Http.Json;
using System.Text.Json;

namespace InForm.Client.Features.Forms.Contracts.Impl;

internal class FormsService(
    HttpClient httpClient
) : IFormsService
{
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        AllowTrailingCommas = true,
        PropertyNameCaseInsensitive = true,
    };

    public async Task<Guid> CreateForm(FormModel model)
    {
        var request = BuildRequest(model);
        var uri = "/api/forms";
        var response = await PostAsync(httpClient, request, uri);

        await using var stream = await response.Content.ReadAsStreamAsync();
        var responsePayload = await JsonSerializer.DeserializeAsync<CreateFormResponse>(stream, _jsonOptions);
        return responsePayload.Id;
    }

    private CreateFormRequest BuildRequest(FormModel model)
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
        return EnsureValidResponse(uri, response);
    }

    private static HttpResponseMessage EnsureValidResponse(string uri, HttpResponseMessage response)
    {
        if (!response.IsSuccessStatusCode) // todo proper exception type
            throw new ApplicationException($"Failed communicating with service: {uri} returned {response.StatusCode}");
        return response;
    }

    public async Task<FormModel> GetForm(Guid id)
    {
        var uri = $"/api/forms/{id}";
        var response = EnsureValidResponse(uri, await httpClient.GetAsync(uri));
        await using var stream = await response.Content.ReadAsStreamAsync();
        var responsePayload = await JsonSerializer.DeserializeAsync<GetFormReponse>(stream, _jsonOptions);

        var form = new FormModel() { Title = responsePayload.Title, Subtitle = responsePayload.Subtitle ?? string.Empty };
        form.ElementModels.AddRange(from fe in responsePayload.FormElements
                                    let elementVisitor = new FromDtoVisitor(form)
                                    orderby fe.Id
                                    select fe.Accept(elementVisitor));
        return form;
    }
}
