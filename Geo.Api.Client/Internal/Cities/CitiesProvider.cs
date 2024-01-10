using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Web;
using Geo.Api.Client.Domain.Airports;
using Geo.Api.Client.Domain.Result;
using Geo.Api.Client.Models;
using Geo.Api.Client.Models.Airports;
using Geo.Api.Client.Providers;

namespace Geo.Api.Client.Internal.Cities;

internal sealed class CitiesProvider : ICitiesProvider
{
    private readonly HttpClient httpClient;

    public CitiesProvider(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<Result<Airport, ResultError>> GetAsync(GetAirportRequest request,
        CancellationToken cancellationToken = default)
    {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        if (request is null)
            return new ResultError(400) { Description = "Request should not be null" };

        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        if (request.Id is null)
            return new ResultError(400) { Description = "Id should not be null" };
        var lang = request.Language.ToString().ToLowerInvariant();
        var response = await httpClient.GetAsync($"{lang}/airport/{request.Id.Value}", cancellationToken);
        if (response.StatusCode != HttpStatusCode.OK)
            return new ResultError((int)response.StatusCode);
        var contentStream = await response.Content.ReadAsStreamAsync(cancellationToken);
        return (await JsonSerializer.DeserializeAsync<Airport>(contentStream,
            cancellationToken: cancellationToken))!;
    }

    public async Task<Result<SearchResult<Airport>, ResultError>> SearchAsync(SearchAirportsRequest request,
        CancellationToken cancellationToken = default)
    {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        if (request is null)
            return new ResultError(400) { Description = "Request should not be null" };
        var lang = request.Language.ToString().ToLowerInvariant();
        var query = HttpUtility.UrlEncode(request.Query);
        var uri =
            $"{lang}/search_objects/avia?query={query}&page_number={request.PageNumber}&page_size={request.PageSize}";
        var response = await httpClient.GetAsync(uri, cancellationToken);
        if (response.StatusCode != HttpStatusCode.OK)
            return new ResultError((int)response.StatusCode);
        var contentStream = await response.Content.ReadAsStreamAsync(cancellationToken);
        return (await JsonSerializer.DeserializeAsync<SearchResult<Airport>>(contentStream,
            cancellationToken: cancellationToken))!;
    }

    public async Task<Result<Airport, ResultError>> CreateAsync(CreateAirportRequest request,
        CancellationToken cancellationToken = default)
    {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        if (request is null)
            return new ResultError(400) { Description = "Request should not be null" };

        var createAirportDto = new CreateAirportDto(request.CityId, request.Name, request.IataEn, request.Latitude,
            request.Longitude, request.Timezone, request.Osm)
        {
            IataRu = request.IataRu
        };

        var json = JsonSerializer.Serialize(createAirportDto);
        var content = JsonContent.Create(json);
        var response = await httpClient.PostAsync("airport", content, cancellationToken);
        if (response.StatusCode != HttpStatusCode.Created)
            return new ResultError((int)response.StatusCode);
        var contentStream = await response.Content.ReadAsStreamAsync(cancellationToken);
        return (await JsonSerializer.DeserializeAsync<Airport>(contentStream,
            cancellationToken: cancellationToken))!;
    }

    public async Task<Result<Airport, ResultError>> UpdateAsync(UpdateAirportRequest request,
        CancellationToken cancellationToken = default)
    {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        if (request is null)
            return new ResultError(400) { Description = "Request should not be null" };

        var updateAirportDto = new UpdateAirportDto(request.CityId, request.Name, request.IataEn, request.Latitude,
            request.Longitude, request.Timezone, request.Osm)
        {
            IataRu = request.IataRu
        };

        var json = JsonSerializer.Serialize(updateAirportDto);
        var content = JsonContent.Create(json);
        var response = await httpClient.PutAsync($"airport/{request.Id.Value}", content, cancellationToken);
        if (response.StatusCode != HttpStatusCode.Created)
            return new ResultError((int)response.StatusCode);
        var contentStream = await response.Content.ReadAsStreamAsync(cancellationToken);
        return (await JsonSerializer.DeserializeAsync<Airport>(contentStream,
            cancellationToken: cancellationToken))!;
    }
}