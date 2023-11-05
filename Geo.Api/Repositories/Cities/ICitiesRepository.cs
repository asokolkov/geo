using Geo.Api.Domain.Cities;

namespace Geo.Api.Repositories.Cities;

internal interface ICitiesRepository
{
    Task<City?> GetAsync(int id, CancellationToken cancellationToken = default);

    Task<City> CreateAsync(int countryId, int? regionId, string name, double latitude, double longitude, string timezone,
        string osm, string? iata, CancellationToken cancellationToken = default);

    Task<City> UpdateAsync(int id, int countryId, int? regionId, string name, double latitude, double longitude, string timezone,
        string osm, string? iata, CancellationToken cancellationToken = default);
}