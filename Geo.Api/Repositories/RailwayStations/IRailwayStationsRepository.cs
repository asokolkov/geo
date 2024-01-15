using Geo.Api.Repositories.RailwayStations.Models;

namespace Geo.Api.Repositories.RailwayStations;

public interface IRailwayStationsRepository
{
    Task<RailwayStationEntity?> GetAsync(int id, CancellationToken cancellationToken = default);

    Task<RailwayStationEntity> CreateAsync(int cityId, RailwayStationCodeEntity code,
        RailwayStationNameEntity name, RailwayStationGeometryEntity geometry,
        string osm, bool needToUpdate, int? utcOffset = default, CancellationToken cancellationToken = default);

    Task<RailwayStationEntity> UpdateAsync(int id, int cityId, RailwayStationCodeEntity code,
        RailwayStationNameEntity name, RailwayStationGeometryEntity geometry,
        string osm, bool needToUpdate, int? utcOffset = default, CancellationToken cancellationToken = default);

    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}
