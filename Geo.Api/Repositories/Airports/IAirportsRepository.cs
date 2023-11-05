using Geo.Api.Domain.Airports;

namespace Geo.Api.Repositories.Airports;

internal interface IAirportsRepository
{
    Task<Airport?> GetAsync(int id, CancellationToken cancellationToken = default);

    Task<Airport> CreateAsync(int cityId, string name, string iataEn, string? iataRu, double latitude,
        double longitude, string timezone, string osm, CancellationToken cancellationToken = default);

    Task<Airport> UpdateAsync(int id, int cityId, string name, string iataEn, string? iataRu, double latitude,
        double longitude, string timezone, string osm, CancellationToken cancellationToken = default);
}