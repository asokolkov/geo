using Geo.Api.Domain.RailwayStation;

namespace Geo.Api.Repositories.RailwayStations;

internal interface IRailwayStationsRepository
{
    Task<RailwayStation?> GetAsync(int id, CancellationToken cancellationToken = default);

    Task<RailwayStation> CreateAsync(int cityId, int rzdCode, bool isMain, string name, double latitude,
        double longitude, string timezone, string osm, CancellationToken cancellationToken = default);

    Task<RailwayStation> UpdateAsync(int id, int cityId, int rzdCode, bool isMain, string name, double latitude,
        double longitude, string timezone, string osm, CancellationToken cancellationToken = default);
}
