using Geo.Api.Client.Domain.Airports;
using Geo.Api.Client.Domain.Result;
using Geo.Api.Client.Models;
using Geo.Api.Client.Models.Airports;

namespace Geo.Api.Client.Providers;

public interface IAirportsProvider
{
    Task<Result<Airport, ResultError>> GetAsync(GetAirportRequest request,
        CancellationToken cancellationToken = default);

    Task<Result<SearchResult<Airport>, ResultError>> SearchAsync(SearchAirportsRequest request,
        CancellationToken cancellationToken = default);

    Task<Result<Airport, ResultError>> CreateAsync(CreateAirportRequest request,
        CancellationToken cancellationToken = default);

    Task<Result<Airport, ResultError>> UpdateAsync(UpdateAirportRequest request,
        CancellationToken cancellationToken = default);
}