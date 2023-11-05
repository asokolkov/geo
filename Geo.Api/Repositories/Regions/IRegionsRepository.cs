namespace Geo.Api.Repositories.Regions;

using Domain.Regions;

internal interface IRegionsRepository
{
    Task<Region?> GetAsync(int id, CancellationToken cancellationToken = default);

    Task<Region> CreateAsync(int countryId, string name, string osm,
        CancellationToken cancellationToken = default);

    Task<Region> UpdateAsync(int id, int countryId, string name, string osm,
        CancellationToken cancellationToken = default);
}