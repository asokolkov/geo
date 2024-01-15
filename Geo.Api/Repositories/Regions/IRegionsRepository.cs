using Geo.Api.Repositories.Regions.Models;

namespace Geo.Api.Repositories.Regions;

public interface IRegionsRepository
{
    Task<RegionEntity?> GetAsync(int id, CancellationToken cancellationToken = default);

    Task<RegionEntity> CreateAsync(int countryId, RegionNameEntity name, string osm,
        bool needToUpdate, RegionGeometryEntity? geometry = default, int? utcOffset = default,
        CancellationToken cancellationToken = default);

    Task<RegionEntity> UpdateAsync(int id, int countryId, RegionNameEntity name, string osm,
        bool needToUpdate, RegionGeometryEntity? geometry = default, int? utcOffset = default,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}