using Geo.Api.Repositories.Cities.Models;

namespace Geo.Api.Repositories.Cities;

internal interface ICitiesRepository
{
    Task<CityEntity?> GetAsync(int id, CancellationToken cancellationToken = default);

    Task<CityEntity> CreateAsync(int countryId, int? regionId, string name, double latitude, double longitude, string timezone,
        string osm, string? iata, CancellationToken cancellationToken = default);

    Task<CityEntity> UpdateAsync(int id, int countryId, int? regionId, string name, double latitude, double longitude, string timezone,
        string osm, string? iata, CancellationToken cancellationToken = default);
    
    IQueryable<CityEntity> Queryable { get; }
}