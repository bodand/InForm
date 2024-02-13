using InForm.Server.Core.Features.Common;
using InForm.Server.Core.Features.Fill;
using System.Net.Http.Json;
using System.Text.Json;

namespace InForm.Client.Features.Forms.Contracts.Impl;

internal class FillService(
    HttpClient httpClient
) : IFillService
{ 
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        AllowTrailingCommas = true,
        PropertyNameCaseInsensitive = true,
    };

    public async Task AddFill(FormModel model)
    {
        if (model is { Id: null }) throw new InvalidFormToFillException("Model is missing the form id: is this form saved?");

        var uri = $"/api/fills/{model.Id}";
        var request = CreateFillRequest(model);
        await PostAsync(request, uri);
    }

    public async Task<RetrieveFillsResponse> GetResponses(Guid id, string? password)
    {
        var uri = $"/api/fills/{id}/:retrieve";
        var request = new RetrieveFillsRequest(id, password);
        var response = await httpClient.PostAsJsonAsync(uri, request);
        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            throw new UnauthorizedAccessException();
        var responsePayload = EnsureValidResponse(uri, response);

        await using var stream = await responsePayload.Content.ReadAsStreamAsync();
        return await JsonSerializer.DeserializeAsync<RetrieveFillsResponse>(stream, _jsonOptions);
    }

    private static FillRequest CreateFillRequest(FormModel model) => new()
    {
        FormId = model.Id!.Value,
        Elements = [.. ProcessElements(model.ElementModels, new ToFillVisitor())]
    };

    private async Task<HttpResponseMessage> PostAsync<TRequest>(TRequest request, string uri)
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

    private static IEnumerable<TResult> ProcessElements<TResult>(IEnumerable<ElementModel> elements, IVisitor<TResult> visitor)
        where TResult : notnull
        => from e in elements
           select e.Accept(visitor);

}
