namespace Geo.Api.Repositories.Countries;

using Domain.Countries;
using Models;

internal interface ICountriesRepository
{
    Task<Country?> GetAsync(int id, CancellationToken cancellationToken = default);

    Task<Country> CreateAsync(string name, string iso3116Alpha2Code, string iso3166Alpha3Code, string phoneCode,
        string phoneMask, string osm, CancellationToken cancellationToken = default);

    Task<Country> UpdateAsync(int id, string name, string iso3116Alpha2Code, string iso3166Alpha3Code, string phoneCode,
        string phoneMask, string osm, CancellationToken cancellationToken = default);
}