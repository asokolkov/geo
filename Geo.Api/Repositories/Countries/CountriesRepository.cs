namespace Geo.Api.Repositories.Countries;

using AutoMapper;
using Domain.Countries;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Internal;
using Models;

internal sealed class CountriesRepository : ICountriesRepository
{
    private readonly ApplicationContext context;
    private readonly ISystemClock systemClock;
    private readonly IMapper mapper;

    public CountriesRepository(ApplicationContext context, ISystemClock systemClock, IMapper mapper)
    {
        this.context = context;
        this.systemClock = systemClock;
        this.mapper = mapper;
    }

    public async Task<Country?> GetAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await context.Countries
            .AsQueryable()
            .AsNoTracking()
            .FirstOrDefaultAsync(country => country.Id == id, cancellationToken);
        return mapper.Map<Country>(entity);
    }

    public async Task<Country> CreateAsync(string name, string iso3116Alpha2Code, string iso3166Alpha3Code,
        string phoneCode, string phoneMask, string osm, CancellationToken cancellationToken = default)
    {
        var now = systemClock.UtcNow;
        var countryEntity =
            new CountryEntity(0, name, iso3116Alpha2Code, iso3166Alpha3Code, phoneCode, phoneMask, osm, now);
        var entry = await context.AddAsync(countryEntity, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return mapper.Map<Country>(entry.Entity);
    }

    public async Task<Country> UpdateAsync(int id, string name, string iso3116Alpha2Code, string iso3166Alpha3Code,
        string phoneCode, string phoneMask, string osm, CancellationToken cancellationToken = default)
    {
        var now = systemClock.UtcNow;
        var countryEntity = await context.Countries.FirstOrDefaultAsync(country => country.Id == id, cancellationToken);

        if (countryEntity is null)
        {
            //todo: выбрасывать нормальное исключение или возвращать Result
            throw new Exception();
        }

        countryEntity.Name = name;
        countryEntity.Iso3116Alpha2Code = iso3116Alpha2Code;
        countryEntity.Iso3166Alpha3Code = iso3166Alpha3Code;
        countryEntity.PhoneCode = phoneCode;
        countryEntity.PhoneMask = phoneMask;
        countryEntity.Osm = osm;
        countryEntity.UpdatedAt = now;

        await context.SaveChangesAsync(cancellationToken);
        return mapper.Map<Country>(countryEntity);
    }
}