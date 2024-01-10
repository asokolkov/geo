using Geo.Api.Repositories.Airports.Models;

namespace Geo.Api.Repositories.Airports;

internal interface IAirportsRepository
{
    Task<AirportEntity?> GetAsync(int id, CancellationToken cancellationToken = default);

    Task<AirportEntity?> GetAsync(string iata, CancellationToken cancellationToken = default);

    Task<AirportEntity> CreateAsync(int cityId, string name, string iataEn, string? iataRu, double latitude,
        double longitude, int utcOffset, string osm, bool needAutomaticUpdate = true,
        CancellationToken cancellationToken = default);

    Task<AirportEntity> UpdateAsync(int id, int cityId, string name, string iataEn, string? iataRu, double latitude,
        double longitude, int utcOffset, string osm, bool needAutomaticUpdate = true,
        CancellationToken cancellationToken = default);

    IQueryable<AirportEntity> Queryable { get; }
}