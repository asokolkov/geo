using Geo.Api.Client.Domain.Cities;
using Geo.Api.Client.Domain.Result;
using Geo.Api.Client.Models;
using Geo.Api.Client.Models.Cities;

namespace Geo.Api.Client.Providers;

public interface ICitiesProvider
{
    Task<Result<City, ResultError>> GetAsync(GetCityRequest request,
        CancellationToken cancellationToken = default);

    Task<Result<SearchResult<City>, ResultError>> SearchAsync(SearchCitiesRequest request,
        CancellationToken cancellationToken = default);

    Task<Result<City, ResultError>> CreateAsync(CreateCityRequest request,
        CancellationToken cancellationToken = default);

    Task<Result<City, ResultError>> UpdateAsync(UpdateCityRequest request,
        CancellationToken cancellationToken = default);
}