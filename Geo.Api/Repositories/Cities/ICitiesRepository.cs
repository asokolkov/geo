using Geo.Api.Repositories.Cities.Models;

namespace Geo.Api.Repositories.Cities;

public interface ICitiesRepository
{
    Task<CityEntity?> GetAsync(int id, CancellationToken cancellationToken = default);

    Task<CityEntity?> GetAsync(string code, CancellationToken cancellationToken = default);

    Task<CityEntity> CreateAsync(int countryId, int? regionId, CityNameEntity name, CityGeometryEntity geometry,
        string osm, string code, bool needToUpdate, int? utcOffset, CancellationToken cancellationToken = default);

    Task<CityEntity> UpdateAsync(int id, int countryId, int? regionId, CityNameEntity name, CityGeometryEntity geometry,
        string osm, string code, bool needToUpdate, int? utcOffset, CancellationToken cancellationToken = default);

    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    
    IQueryable<CityEntity> Queryable { get; }
}