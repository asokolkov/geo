using Geo.Api.Repositories.Airports.Models;

namespace Geo.Api.Repositories.Airports;

public interface IAirportsRepository
{
    Task<AirportEntity?> GetAsync(int id, CancellationToken cancellationToken = default);

    Task<AirportEntity?> GetAsync(string iata, CancellationToken cancellationToken = default);

    Task<AirportEntity> CreateAsync(int cityId, AirportNameEntity name, AirportCodeEntity code,
        AirportGeometryEntity geometry, string osm, bool needAutomaticUpdate, int? utcOffset = default,
        CancellationToken cancellationToken = default);

    Task<AirportEntity> UpdateAsync(int id, int cityId, AirportNameEntity name, AirportCodeEntity code,
        AirportGeometryEntity geometry, string osm, bool needAutomaticUpdate, int? utcOffset = default,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(int id, CancellationToken cancellationToken = default);

    IQueryable<AirportEntity> Queryable { get; }
}