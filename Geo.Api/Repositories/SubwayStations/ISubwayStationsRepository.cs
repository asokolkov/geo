using Geo.Api.Repositories.SubwayStations.Models;

namespace Geo.Api.Repositories.SubwayStations;

public interface ISubwayStationsRepository
{
    Task<SubwayStationEntity?> GetAsync(int id, CancellationToken cancellationToken = default);

    Task<SubwayStationEntity> CreateAsync(int cityId, SubwayStationNameEntity name, SubwayLineNameEntity lineName,
        SubwayStationGeometryEntity geometry, string osm, bool needAutomaticUpdate,
        CancellationToken cancellationToken = default);

    Task<SubwayStationEntity> UpdateAsync(int id, int cityId, SubwayStationNameEntity name, SubwayLineNameEntity lineName,
        SubwayStationGeometryEntity geometry, string osm, bool needAutomaticUpdate,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(int id, CancellationToken cancellationToken = default);

    IQueryable<SubwayStationEntity> Queryable { get; }
}