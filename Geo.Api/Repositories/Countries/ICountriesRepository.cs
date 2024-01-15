namespace Geo.Api.Repositories.Countries;

using Models;

public interface ICountriesRepository
{
    Task<CountryEntity?> GetAsync(int id, CancellationToken cancellationToken = default);
    
    Task<CountryEntity?> GetAsync(string code, CancellationToken cancellationToken = default);

    Task<CountryEntity> CreateAsync(CountryNameEntity name, string iso3116Alpha2Code, string iso3166Alpha3Code,
        string phoneCode,
        string phoneMask, string osm, bool needAutomaticUpdate, CountryGeometryEntity? geometry = default,
        CancellationToken cancellationToken = default);

    Task<CountryEntity> UpdateAsync(int id, CountryNameEntity name, string iso3116Alpha2Code, string iso3166Alpha3Code,
        string phoneCode, string phoneMask, string osm, bool needAutomaticUpdate,
        CountryGeometryEntity? geometry = default,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}