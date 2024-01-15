using Geo.Api.Repositories.Exceptions;

namespace Geo.Api.Repositories.Countries;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Internal;
using Models;

internal sealed class CountriesRepository : ICountriesRepository
{
    private readonly ILogger logger;
    private readonly ApplicationContext context;
    private readonly ISystemClock systemClock;

    public CountriesRepository(ILogger<CountriesRepository> logger, ApplicationContext context,
        ISystemClock systemClock)
    {
        this.logger = logger;
        this.context = context;
        this.systemClock = systemClock;
    }

    public async Task<CountryEntity?> GetAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await context.Countries
            .AsQueryable()
            .AsNoTracking()
            .FirstOrDefaultAsync(country => country.Id == id, cancellationToken);
        return entity;
    }

    public async Task<CountryEntity?> GetAsync(string code, CancellationToken cancellationToken = default)
    {
        var entity = await context.Countries
            .AsQueryable()
            .AsNoTracking()
            .FirstOrDefaultAsync(country => country.Iso3116Alpha2Code == code || country.Iso3166Alpha3Code == code,
                cancellationToken);
        return entity;
    }

    public async Task<CountryEntity> CreateAsync(CountryNameEntity name, string iso3116Alpha2Code,
        string iso3166Alpha3Code, string phoneCode, string phoneMask, string osm, bool needAutomaticUpdate,
        CountryGeometryEntity? geometry = default, CancellationToken cancellationToken = default)
    {
        var now = systemClock.UtcNow;
        geometry ??= new CountryGeometryEntity();
        var countryEntity =
            new CountryEntity(0, name, geometry, iso3116Alpha2Code, iso3166Alpha3Code, phoneCode, phoneMask, osm,
                needAutomaticUpdate, now);
        var entry = await context.AddAsync(countryEntity, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        await entry.ReloadAsync(cancellationToken);
        return entry.Entity;
    }

    public async Task<CountryEntity> UpdateAsync(int id, CountryNameEntity name, string iso3116Alpha2Code,
        string iso3166Alpha3Code, string phoneCode, string phoneMask, string osm, bool needAutomaticUpdate,
        CountryGeometryEntity? geometry = default, CancellationToken cancellationToken = default)
    {
        var now = systemClock.UtcNow;
        var countryEntity = await context.Countries.FirstOrDefaultAsync(country => country.Id == id, cancellationToken);

        if (countryEntity is null)
        {
            logger.LogWarning("Country with id '{Id}' not found", id);
            throw new EntityNotFoundException(typeof(CountryEntity), id);
        }

        countryEntity.Name = name;
        countryEntity.Iso3116Alpha2Code = iso3116Alpha2Code;
        countryEntity.Iso3166Alpha3Code = iso3166Alpha3Code;
        countryEntity.PhoneCode = phoneCode;
        countryEntity.PhoneMask = phoneMask;
        countryEntity.Osm = osm;
        countryEntity.UpdatedAt = now;
        countryEntity.Geometry = geometry ?? new CountryGeometryEntity();
        countryEntity.NeedAutomaticUpdate = needAutomaticUpdate;

        await context.SaveChangesAsync(cancellationToken);

        return countryEntity;
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var now = systemClock.UtcNow;
        var countryEntity = await context.Countries.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        if (countryEntity is null)
        {
            logger.LogWarning("Country with id '{Id}' not found", id);
            throw new EntityNotFoundException(typeof(CountryEntity), id);
        }

        countryEntity.DeletedAt = now;
        countryEntity.UpdatedAt = now;
        await context.SaveChangesAsync(cancellationToken);
    }
}